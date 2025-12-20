using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Mango.UserService.Migrations;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        Console.WriteLine("Starting database migration...");
        
        await RunMigrationsAsync(connectionString);
        
        Console.WriteLine("Migration completed successfully!");
    }

    static async Task RunMigrationsAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        // Create migration tracking table
        await CreateMigrationTableAsync(connection);

        // Get all migration files
        var scriptsPath = Path.Combine(AppContext.BaseDirectory, "Scripts");
        var migrationFiles = Directory.GetFiles(scriptsPath, "*.sql")
            .OrderBy(f => Path.GetFileName(f))
            .ToList();

        foreach (var file in migrationFiles)
        {
            var fileName = Path.GetFileName(file);
            
            if (await IsMigrationAppliedAsync(connection, fileName))
            {
                Console.WriteLine($"Skipping {fileName} (already applied)");
                continue;
            }

            Console.WriteLine($"Applying {fileName}...");
            var sql = await File.ReadAllTextAsync(file);
            
            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                await using var command = new NpgsqlCommand(sql, connection, transaction);
                await command.ExecuteNonQueryAsync();
                
                await RecordMigrationAsync(connection, transaction, fileName);
                await transaction.CommitAsync();
                
                Console.WriteLine($"✓ {fileName} applied successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"✗ Failed to apply {fileName}: {ex.Message}");
                throw;
            }
        }
    }

    static async Task CreateMigrationTableAsync(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS __migrations (
                id SERIAL PRIMARY KEY,
                migration_name VARCHAR(255) NOT NULL UNIQUE,
                applied_at TIMESTAMP NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC')
            )";
        
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    static async Task<bool> IsMigrationAppliedAsync(NpgsqlConnection connection, string migrationName)
    {
        const string sql = "SELECT COUNT(*) FROM __migrations WHERE migration_name = @name";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("name", migrationName);
        
        var count = (long)(await command.ExecuteScalarAsync())!;
        return count > 0;
    }

    static async Task RecordMigrationAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, string migrationName)
    {
        const string sql = "INSERT INTO __migrations (migration_name) VALUES (@name)";
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("name", migrationName);
        await command.ExecuteNonQueryAsync();
    }
}

