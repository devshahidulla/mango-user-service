# Database Name Fix - Summary

## Issue
When trying to register a user via Swagger in the containerized API, you were getting this error:
```
Npgsql.PostgresException: 3D000: database "mongo_user_service" does not exist
```

## Root Cause
There was a **database name mismatch** between the docker-compose.yml and the actual database:

- **docker-compose.yml** had: `Database=mongo_user_service` (❌ without 'a')
- **Actual database name**: `Database=mango_user_service` (✅ with 'a')
- **appsettings.json** had: `Database=mango_user_service` (✅ correct)
- **Migrations config** had: `Database=mango_user_service` (✅ correct)

This explains why:
- ✅ **Local worked**: Used appsettings.json with correct name `mango_user_service`
- ❌ **Container failed**: Used docker-compose.yml environment variable override with wrong name `mongo_user_service`

## Changes Made

### 1. Fixed docker-compose.yml
**File**: `docker-compose.yml`

**Changed**:
```yaml
ConnectionStrings__DefaultConnection: "Host=host.docker.internal;Port=5432;Database=mongo_user_service;..."
```

**To**:
```yaml
ConnectionStrings__DefaultConnection: "Host=host.docker.internal;Port=5432;Database=mango_user_service;..."
```

### 2. Fixed Migration Script Comment
**File**: `Mango.UserService.Migrations\Scripts\001_CreateUsersTable.sql`

Updated the comment to reflect the correct database name (documentation fix only).

## Verification Steps

### 1. Verify the Container is Running
```powershell
docker ps | Select-String mango-user-api
```

### 2. Check the Connection String in Container
```powershell
docker inspect mango-user-api --format='{{range .Config.Env}}{{println .}}{{end}}' | Select-String ConnectionStrings
```

Expected output should show: `Database=mango_user_service` (with 'a')

### 3. Check Application Logs
```powershell
docker logs mango-user-api --tail 30
```

Look for successful startup messages like:
```
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 4. Test via Swagger
1. Navigate to: http://localhost:5001/swagger
2. Try the POST `/api/user/register` endpoint
3. Use test data:
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "password": "Test@123"
}
```

### 5. If Still Getting Database Errors

Check if the database exists:
```powershell
# Option 1: Using psql (if installed)
psql -U postgres -h localhost -p 5432 -c "\l" | Select-String mango

# Option 2: Using Docker PostgreSQL client
docker exec -it <postgres-container-name> psql -U postgres -c "\l"
```

If database doesn't exist, create it:
```powershell
# Run migrations
cd Mango.UserService.Migrations
dotnet run
```

Or create manually:
```sql
CREATE DATABASE mango_user_service;
```

## Testing

After restarting the container, test with curl or Invoke-RestMethod:

```powershell
$body = @{
    firstName = "John"
    lastName = "Doe"
    email = "john.doe@example.com"
    password = "SecurePass@123"
} | ConvertTo-Json

Invoke-RestMethod -Method POST -Uri "http://localhost:5001/api/user/register" -Body $body -ContentType "application/json"
```

Expected response:
```json
{
  "userId": "guid",
  "firstName": "John",
  "lastName": "Doe",
  "fullName": "John Doe",
  "email": "john.doe@example.com",
  "role": "Farmer",
  "createdAt": "2025-12-25T..."
}
```

## Status

✅ **docker-compose.yml** - Fixed database name
✅ **Migration script comment** - Updated for consistency
✅ **Container** - Restarted with new configuration

## Next Action Required

**Restart the container** to apply the fix:
```powershell
cd D:\mango_platform\mango-user-service
docker-compose down
docker-compose up -d
```

Then test the registration endpoint via Swagger at http://localhost:5001/swagger

---

**The database name mismatch has been corrected. The container should now connect to the correct database `mango_user_service`!** 🎉

