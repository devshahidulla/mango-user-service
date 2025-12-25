# 🐳 Docker Setup for Mango User Service Migrations

## ✅ What's Been Created

### Docker Files:
1. **`Mango.UserService.Migrations/Dockerfile`** - Multi-stage Docker build configuration
2. **`Mango.UserService.Migrations/.dockerignore`** - Excludes unnecessary files from Docker build
3. **`docker-compose.yml`** - Orchestrates PostgreSQL + Migrations
4. **`build-migrations-image.ps1`** - PowerShell script to build image
5. **`build-migrations-image.sh`** - Bash script to build image (Linux/Mac)

### Docker Image Created:
- **Image Name:** `mango-user-service-migrations:latest`
- **Size:** 287MB (Compressed: 79.9MB)
- **Status:** ✅ Successfully built and available in local Docker

---

## 🚀 How to Use

### Method 1: Run Migrations Against Existing Database

If you already have PostgreSQL running (on localhost or elsewhere):

```powershell
docker run --rm \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password" \
  mango-user-service-migrations:latest
```

**Note:** Use `host.docker.internal` to connect to services running on your host machine from within Docker.

---

### Method 2: Use Docker Compose (PostgreSQL + Migrations)

Start both PostgreSQL and run migrations:

```powershell
# First time: Build the migrations image if not already built
docker-compose up

# Stop services
docker-compose down

# Stop and remove volumes (clean database)
docker-compose down -v
```

**What this does:**
1. Starts PostgreSQL container
2. Waits for PostgreSQL to be healthy
3. Runs migrations automatically
4. Exits after migrations complete

---

### Method 3: Use the Build Script

To rebuild the image:

```powershell
# Windows PowerShell
.\build-migrations-image.ps1

# Linux/Mac Bash
chmod +x build-migrations-image.sh
./build-migrations-image.sh
```

---

## 📋 Verify Migrations Were Applied

After running migrations, connect to your database and check:

```sql
-- Check if migrations table exists
SELECT * FROM __migrations;

-- Check users table structure
\d users

-- Verify data
SELECT userid, firstname, lastname, email, role, createdat 
FROM users 
LIMIT 5;
```

---

## 🔧 Configuration

### Environment Variables

You can override the connection string using environment variables:

```powershell
docker run --rm \
  -e ConnectionStrings__DefaultConnection="Host=myserver;Port=5432;Database=mydb;Username=myuser;Password=mypass" \
  mango-user-service-migrations:latest
```

### Connection String Formats

**Local PostgreSQL (from Docker):**
```
Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

**Docker Compose Network:**
```
Host=postgres;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

**Remote PostgreSQL:**
```
Host=myserver.example.com;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

---

## 🛠️ Troubleshooting

### Issue: "Could not connect to the database"

**Solution:** Make sure PostgreSQL is running and accessible. Check:
- PostgreSQL is running: `docker ps` or `Get-Service postgresql*`
- Connection string is correct
- Firewall allows connections
- Use `host.docker.internal` instead of `localhost` when connecting from Docker to host

### Issue: "Migration already applied"

**Solution:** This is normal! Migrations are tracked in the `__migrations` table. Each migration runs only once.

### Issue: "Permission denied"

**Solution:** Make sure the PostgreSQL user has permissions to create tables and modify schema.

---

## 📦 Available Migrations

1. **001_CreateUsersTable.sql** - Creates the initial users table
   - Columns: userid, firstname, lastname, email, passwordhash, role, createdat
   - Indexes on email and role

2. **002_AlterUsersTableSplitFullName.sql** - Migrates from fullname to firstname/lastname
   - Adds firstname and lastname columns
   - Migrates existing fullname data
   - Drops fullname column

---

## 🎯 Next Steps

### To Run Against Your Existing Database:

1. **Update connection string** with your actual credentials
2. **Run the migration:**
   ```powershell
   docker run --rm \
     -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=YOUR_ACTUAL_PASSWORD" \
     mango-user-service-migrations:latest
   ```
3. **Verify** the changes in your database

### To Push to Docker Registry (Optional):

```powershell
# Tag for registry
docker tag mango-user-service-migrations:latest your-registry/mango-user-service-migrations:latest

# Push to registry
docker push your-registry/mango-user-service-migrations:latest
```

---

## 📊 Image Details

```
REPOSITORY                       TAG      SIZE        
mango-user-service-migrations    latest   287MB (compressed: 79.9MB)
```

**Base Images:**
- Runtime: `mcr.microsoft.com/dotnet/runtime:8.0`
- Build: `mcr.microsoft.com/dotnet/sdk:8.0`

**Architecture:** Multi-stage build for minimal final image size

---

## ✅ Summary

Your Docker image is ready! You can now:
- ✅ Run migrations in Docker containers
- ✅ Use in CI/CD pipelines
- ✅ Deploy across different environments consistently
- ✅ Avoid "works on my machine" issues
- ✅ Easily manage database schema changes

**Image is available in your local Docker:** `mango-user-service-migrations:latest`

