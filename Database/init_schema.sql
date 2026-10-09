-- ====================================================================
-- SMART PARKING MANAGEMENT SYSTEM
-- DATABASE SCHEMA DEFINITION (SQLITE)
-- Architecture: Domain-Driven Design (DDD) & Clean Architecture
-- ====================================================================

PRAGMA foreign_keys = ON;

-- 1. SYSTEM MANAGERS
CREATE TABLE IF NOT EXISTS "system_managers" (
    "id" TEXT NOT NULL CONSTRAINT "PK_system_managers" PRIMARY KEY,
    "full_name" TEXT NOT NULL,
    "position" TEXT NOT NULL,
    "phone_number" TEXT NOT NULL,
    "email" TEXT NOT NULL,
    "username" TEXT NOT NULL UNIQUE,
    "password_hash" TEXT NOT NULL,
    "role" INTEGER NOT NULL DEFAULT 1,
    "is_active" INTEGER NOT NULL DEFAULT 1
);

-- 2. VEHICLE TYPES
CREATE TABLE IF NOT EXISTS "vehicle_types" (
    "id" TEXT NOT NULL CONSTRAINT "PK_vehicle_types" PRIMARY KEY,
    "name" TEXT NOT NULL,
    "description" TEXT NOT NULL
);

-- 3. BUSINESS OPERATORS
CREATE TABLE IF NOT EXISTS "business_operators" (
    "id" TEXT NOT NULL CONSTRAINT "PK_business_operators" PRIMARY KEY,
    "system_manager_id" TEXT NOT NULL,
    "name" TEXT NOT NULL,
    "representative" TEXT NOT NULL,
    "street" TEXT NOT NULL,
    "ward" TEXT NOT NULL,
    "district" TEXT NOT NULL,
    "city" TEXT NOT NULL,
    "phone_number" TEXT NOT NULL,
    "email" TEXT NOT NULL,
    "is_active" INTEGER NOT NULL DEFAULT 1,
    "username" TEXT NOT NULL UNIQUE,
    "password_hash" TEXT NOT NULL,
    CONSTRAINT "FK_business_operators_system_managers_system_manager_id" 
        FOREIGN KEY ("system_manager_id") REFERENCES "system_managers" ("id") ON DELETE RESTRICT
);

-- 4. PARKING LOTS
CREATE TABLE IF NOT EXISTS "parking_lots" (
    "id" TEXT NOT NULL CONSTRAINT "PK_parking_lots" PRIMARY KEY,
    "business_operator_id" TEXT NOT NULL,
    "name" TEXT NOT NULL,
    "street" TEXT NOT NULL,
    "ward" TEXT NOT NULL,
    "district" TEXT NOT NULL,
    "city" TEXT NOT NULL,
    "total_capacity" INTEGER NOT NULL,
    "available_spots" INTEGER NOT NULL,
    "open_time" TEXT NOT NULL DEFAULT '06:00',
    "close_time" TEXT NOT NULL DEFAULT '22:00',
    "is_active" INTEGER NOT NULL DEFAULT 1,
    CONSTRAINT "FK_parking_lots_business_operators_business_operator_id" 
        FOREIGN KEY ("business_operator_id") REFERENCES "business_operators" ("id") ON DELETE CASCADE
);

-- 5. TARIFFS (PRICING SCHEMES)
CREATE TABLE IF NOT EXISTS "tariffs" (
    "id" TEXT NOT NULL CONSTRAINT "PK_tariffs" PRIMARY KEY,
    "parking_lot_id" TEXT NOT NULL,
    "vehicle_type_id" TEXT NOT NULL,
    "price_amount" TEXT NOT NULL,
    "price_currency" TEXT NOT NULL DEFAULT 'VND',
    "pricing_type" INTEGER NOT NULL DEFAULT 1,
    "effective_date" TEXT NOT NULL,
    CONSTRAINT "FK_tariffs_parking_lots_parking_lot_id" 
        FOREIGN KEY ("parking_lot_id") REFERENCES "parking_lots" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_tariffs_vehicle_types_vehicle_type_id" 
        FOREIGN KEY ("vehicle_type_id") REFERENCES "vehicle_types" ("id") ON DELETE RESTRICT
);

-- 6. PARKING SESSIONS (ENTRY / EXIT TRACKING)
CREATE TABLE IF NOT EXISTS "parking_sessions" (
    "id" TEXT NOT NULL CONSTRAINT "PK_parking_sessions" PRIMARY KEY,
    "parking_lot_id" TEXT NOT NULL,
    "vehicle_type_id" TEXT NOT NULL,
    "province_code" TEXT NOT NULL,
    "plate_number" TEXT NOT NULL,
    "check_in_time" TEXT NOT NULL,
    "check_out_time" TEXT NULL,
    "fee_amount" TEXT NOT NULL DEFAULT '0',
    "fee_currency" TEXT NOT NULL DEFAULT 'VND',
    "status" INTEGER NOT NULL DEFAULT 1,
    CONSTRAINT "FK_parking_sessions_parking_lots_parking_lot_id" 
        FOREIGN KEY ("parking_lot_id") REFERENCES "parking_lots" ("id") ON DELETE RESTRICT,
    CONSTRAINT "FK_parking_sessions_vehicle_types_vehicle_type_id" 
        FOREIGN KEY ("vehicle_type_id") REFERENCES "vehicle_types" ("id") ON DELETE RESTRICT
);

-- INDEXES
CREATE INDEX IF NOT EXISTS "IX_business_operators_system_manager_id" ON "business_operators" ("system_manager_id");
CREATE INDEX IF NOT EXISTS "IX_parking_lots_business_operator_id" ON "parking_lots" ("business_operator_id");
CREATE INDEX IF NOT EXISTS "IX_tariffs_parking_lot_id" ON "tariffs" ("parking_lot_id");
CREATE INDEX IF NOT EXISTS "IX_tariffs_vehicle_type_id" ON "tariffs" ("vehicle_type_id");
CREATE INDEX IF NOT EXISTS "IX_parking_sessions_parking_lot_id" ON "parking_sessions" ("parking_lot_id");
CREATE INDEX IF NOT EXISTS "IX_parking_sessions_vehicle_type_id" ON "parking_sessions" ("vehicle_type_id");
