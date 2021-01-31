SQLite:
dotnet ef migrations add InitialMigration --context SQLiteApplicationDbContext --output-dir Migrations/SQLiteMigrations

SQLServer:
dotnet ef migrations add InitialMigration --context SQLServerApplicationDbContext --output-dir Migrations/SQLServerMigrations