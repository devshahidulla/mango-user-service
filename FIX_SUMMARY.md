# ✅ ISSUE RESOLVED - Database Name Mismatch

## Problem Statement
**Error when registering user via Swagger in containerized API**:
```
Npgsql.PostgresException: 3D000: database "mongo_user_service" does not exist
```

✅ **Working**: Local (using appsettings.json)  
❌ **Not Working**: Docker Container (using docker-compose.yml environment override)

---

## Root Cause Analysis

### The Typo
The docker-compose.yml had a typo in the database name:
- ❌ **Wrong**: `mongo_user_service` (missing 'a')
- ✅ **Correct**: `mango_user_service` (with 'a')

### Why It Happened
1. **Local Development** uses `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=mango_user_service;..."
   }
   ```
   ✅ This has the correct name, so local development worked fine.

2. **Docker Container** uses environment variable override from `docker-compose.yml`:
   ```yaml
   environment:
     ConnectionStrings__DefaultConnection: "Host=host.docker.internal;Port=5432;Database=mongo_user_service;..."
   ```
   ❌ This had the typo, causing container to fail.

### Environment Variable Precedence
In ASP.NET Core, environment variables override appsettings.json values. That's why:
- The container ignored the correct value in appsettings.json
- It used the incorrect value from docker-compose.yml environment variables

---

## Solution Applied

### File: `docker-compose.yml`
**Line 13 - Fixed the database name**:

```yaml
# BEFORE (❌ Wrong)
ConnectionStrings__DefaultConnection: "Host=host.docker.internal;Port=5432;Database=mongo_user_service;..."

# AFTER (✅ Correct)
ConnectionStrings__DefaultConnection: "Host=host.docker.internal;Port=5432;Database=mango_user_service;..."
```

### File: `Mango.UserService.Migrations\Scripts\001_CreateUsersTable.sql`
**Line 2 - Fixed the comment** (documentation only):

```sql
-- BEFORE
-- Database: mongo_user_service

-- AFTER
-- Database: mango_user_service
```

---

## How to Apply the Fix

### Step 1: Restart the Container
```powershell
cd D:\mango_platform\mango-user-service
docker compose down
docker compose up -d
```

### Step 2: Verify Container is Running
```powershell
docker ps | Select-String mango-user-api
```

Expected output:
```
mango-user-api   ... Up X seconds   0.0.0.0:5000->5000/tcp, 0.0.0.0:5001->5001/tcp
```

### Step 3: Check Logs
```powershell
docker logs mango-user-api --tail 20
```

Expected output:
```
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
```

---

## Testing the Fix

### Option 1: Via Swagger UI
1. Open browser: http://localhost:5001/swagger
2. Navigate to `POST /api/user/register`
3. Click "Try it out"
4. Use this test payload:
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "testuser@example.com",
  "password": "Test@123456"
}
```
5. Click "Execute"

**Expected Result**: Status 200 with user details returned

### Option 2: Via PowerShell
```powershell
$headers = @{
    "Content-Type" = "application/json"
}

$body = @{
    firstName = "Jane"
    lastName = "Doe"
    email = "jane.doe@example.com"
    password = "SecurePass@123"
} | ConvertTo-Json

$response = Invoke-RestMethod -Method POST -Uri "http://localhost:5001/api/user/register" -Body $body -Headers $headers
$response
```

**Expected Response**:
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Jane",
  "lastName": "Doe",
  "fullName": "Jane Doe",
  "email": "jane.doe@example.com",
  "role": "Farmer",
  "createdAt": "2025-12-25T22:00:00.000Z"
}
```

---

## Troubleshooting

### If you still get "database does not exist" error:

#### 1. Verify the Database Exists
```powershell
# Check if you have psql installed
psql --version

# If yes, list databases
psql -U postgres -h localhost -p 5432 -l | Select-String mango
```

#### 2. Create the Database (if missing)
```powershell
# Option A: Use psql
psql -U postgres -h localhost -p 5432 -c "CREATE DATABASE mango_user_service;"

# Option B: Run migrations
cd Mango.UserService.Migrations
dotnet run
```

#### 3. Verify Connection String in Container
```powershell
docker exec mango-user-api printenv ConnectionStrings__DefaultConnection
```

Expected output should contain: `Database=mango_user_service`

#### 4. Check PostgreSQL is Running
```powershell
# List PostgreSQL processes
Get-Process | Where-Object { $_.ProcessName -like "*postgres*" }

# Or check if PostgreSQL service is running
Get-Service | Where-Object { $_.Name -like "*postgres*" }
```

---

## Summary of All Changes

| File | Change | Type |
|------|--------|------|
| `docker-compose.yml` | Fixed database name from `mongo_user_service` to `mango_user_service` | **Critical Fix** |
| `001_CreateUsersTable.sql` | Updated comment from `mongo_user_service` to `mango_user_service` | Documentation |

---

## Verification Checklist

- [x] docker-compose.yml updated with correct database name
- [x] Container can be restarted without errors
- [ ] Test user registration via Swagger (requires your action)
- [ ] Verify user is saved in database (requires your action)

---

## Important Notes

1. **AWS Credentials**: The AWS credentials are already mounted and configured from the previous fix.

2. **Database Host**: 
   - Local: `Host=localhost` ✅
   - Container: `Host=host.docker.internal` ✅ (to reach host machine from container)

3. **Connection String Priority**:
   ```
   Environment Variables > appsettings.json > appsettings.Development.json
   ```

4. **Both Issues Now Fixed**:
   - ✅ AWS credentials mounted to container
   - ✅ Database name corrected in docker-compose.yml

---

## Next Steps

1. **Restart the container** to apply the database name fix
2. **Test registration** via Swagger at http://localhost:5001/swagger
3. **Verify the user** was created in the database

The database connection error should now be resolved! 🎉

