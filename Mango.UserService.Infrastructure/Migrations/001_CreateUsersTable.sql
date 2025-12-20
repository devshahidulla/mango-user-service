-- Migration: Create users table
-- Database: mongo_user_service
-- Description: Initial table creation for user management

CREATE TABLE IF NOT EXISTS users (
    userid UUID PRIMARY KEY,
    firstname VARCHAR(100) NOT NULL,
    lastname VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    passwordhash VARCHAR(255) NOT NULL,
    role INTEGER NOT NULL,
    createdat TIMESTAMP NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC')
);

-- Create index on email for faster lookups
CREATE INDEX IF NOT EXISTS idx_users_email ON users(email);

-- Create index on role for filtering by user role
CREATE INDEX IF NOT EXISTS idx_users_role ON users(role);

