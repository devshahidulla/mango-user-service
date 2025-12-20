﻿# Database Migrations

This project handles database schema migrations for the Mango User Service.

## Running Migrations

```bash
cd Mango.UserService.Migrations
dotnet run
```

## Configuration

Update `appsettings.json` with your PostgreSQL connection string.

## Available Migrations

- **001_CreateUsersTable.sql** - Initial table creation with firstname/lastname structure
- **002_AlterUsersTableSplitFullName.sql** - Migration to convert existing fullname column to firstname/lastname

## Creating New Migrations

1. Create a new `.sql` file in the `Scripts` folder
2. Name it with an incremental number prefix (e.g., `003_AddUserPreferences.sql`)
3. Write your migration SQL
4. Run `dotnet run` to apply

## Migration Tracking

Migrations are tracked in the `__migrations` table to prevent duplicate execution.

## Note

If your existing database has a `fullname` column instead of `firstname` and `lastname`, run migration `002` to update the schema.

