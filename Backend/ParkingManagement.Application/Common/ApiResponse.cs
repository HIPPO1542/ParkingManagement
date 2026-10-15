namespace ParkingManagement.Application.Common;

public class ApiResponse<T>
{
    public int Code { get; set; } = 0;
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "Success";
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Code = 0,
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> SuccessMessage(string message = "Success")
    {
        return new ApiResponse<T>
        {
            Code = 0,
            Success = true,
            Message = message,
            Data = default
        };
    }

    public static ApiResponse<T> Fail(string message, List<string>? errors = null, int code = 400)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<T> Fail(string message, int code)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Success = false,
            Message = message,
            Errors = new List<string>()
        };
    }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / (PageSize > 0 ? PageSize : 1));

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
