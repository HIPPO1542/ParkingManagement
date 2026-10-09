using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Services;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.ValueObjects;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;
using Xunit;

namespace ParkingManagement.Tests;

public class TestJwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(Guid userId, string username, string role, Guid? businessOperatorId, string fullName)
    {
        return $"token_for_{username}_{role}";
    }
}

public class AuthServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ParkingDbContext _context;
    private readonly SystemManagerRepository _systemManagerRepo;
    private readonly BusinessOperatorRepository _operatorRepo;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ParkingDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ParkingDbContext(options);
        _context.Database.EnsureCreated();

        _systemManagerRepo = new SystemManagerRepository(_context);
        _operatorRepo = new BusinessOperatorRepository(_context);
        _authService = new AuthService(_systemManagerRepo, _operatorRepo, new TestJwtTokenGenerator());
    }

    [Fact]
    public async Task LoginAsync_ValidAdmin_ReturnsManagerRoleAndToken()
    {
        // Arrange
        var admin = new SystemManager(
            Guid.NewGuid(),
            "Nguyen Van Quan Ly",
            "General Manager",
            new ContactInfo("0909123456", "admin@parking.vn"),
            "admin",
            "admin@123",
            ParkingManagement.Domain.Enums.UserRole.Manager,
            true
        );
        await _context.SystemManagers.AddAsync(admin);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authService.LoginAsync(new LoginRequestDto
        {
            Username = "admin",
            Password = "admin@123"
        });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Manager", result.Role);
        Assert.Equal("token_for_admin_Manager", result.Token);
        Assert.Equal("Nguyen Van Quan Ly", result.FullName);
    }

    [Fact]
    public async Task LoginAsync_ValidBusinessOperator_ReturnsBusinessOperatorRoleAndToken()
    {
        // Arrange
        var managerId = Guid.NewGuid();
        var admin = new SystemManager(
            managerId,
            "System Manager",
            "Head of Operations",
            new ContactInfo("0909123456", "admin2@parking.vn"),
            "admin2",
            "123456",
            ParkingManagement.Domain.Enums.UserRole.Manager,
            true
        );
        await _context.SystemManagers.AddAsync(admin);

        var operatorId = Guid.NewGuid();
        var op = new BusinessOperator(
            operatorId,
            managerId,
            "Saigon Parking Corp",
            "Director Tran",
            new Address("123 Le Loi", "Ben Nghe", "District 1", "HCMC"),
            new ContactInfo("0988776655", "info@sgparking.vn"),
            true,
            "sgparking",
            "123456"
        );
        await _context.BusinessOperators.AddAsync(op);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authService.LoginAsync(new LoginRequestDto
        {
            Username = "sgparking",
            Password = "123456"
        });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("BusinessOperator", result.Role);
        Assert.Equal("token_for_sgparking_BusinessOperator", result.Token);
        Assert.Equal(operatorId, result.BusinessOperatorId);
        Assert.Equal("Saigon Parking Corp", result.FullName);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsBusinessRuleViolationException()
    {
        // Arrange
        var admin = new SystemManager(
            Guid.NewGuid(),
            "Nguyen Van Quan Ly",
            "General Manager",
            new ContactInfo("0909123456", "admin@parking.vn"),
            "admin",
            "admin@123",
            ParkingManagement.Domain.Enums.UserRole.Manager,
            true
        );
        await _context.SystemManagers.AddAsync(admin);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            _authService.LoginAsync(new LoginRequestDto
            {
                Username = "admin",
                Password = "wrongpassword"
            }));
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
