# Quick Start - Testing AWS Credentials in Container

## ✅ What Was Done
Your Docker container now has AWS credentials mounted from your local machine:
- Local: `C:\Users\md\.aws\credentials`
- Container: `/root/.aws/credentials` (read-only)

## 🚀 Quick Test Commands

### 1. Verify Container is Running
```powershell
docker ps | Select-String mango-user-api
```

### 2. Test the Health Endpoint
```powershell
(Invoke-WebRequest -Uri http://localhost:5001/health).Content
```

### 3. Check AWS Credentials in Container
```powershell
# Check if credentials file exists
docker exec mango-user-api test -f /root/.aws/credentials && echo "OK" || echo "MISSING"

# List AWS directory
docker exec mango-user-api ls -la /root/.aws

# View environment variables
docker exec mango-user-api printenv | findstr AWS
```

### 4. Watch Application Logs
```powershell
docker logs -f mango-user-api
```

### 5. Test API with AWS (when you have an endpoint)
```powershell
$body = @{
    email = "test@example.com"
    password = "Test@123"
    fullName = "Test User"
} | ConvertTo-Json

Invoke-RestMethod -Method POST -Uri http://localhost:5001/api/user/register -Body $body -ContentType "application/json"
```

## 🔄 If You Need to Restart

```powershell
# Navigate to project
cd D:\mango_platform\mango-user-service

# Stop container
docker-compose down

# Start container
docker-compose up -d

# View logs
docker-compose logs -f user-api
```

## 🐛 If AWS Credentials Still Don't Work

1. **Verify local credentials**:
   ```powershell
   cat $env:USERPROFILE\.aws\credentials
   ```

2. **Check container mount**:
   ```powershell
   docker inspect mango-user-api | Select-String -Pattern "\.aws"
   ```

3. **Restart with fresh container**:
   ```powershell
   docker-compose down
   docker-compose up -d --force-recreate
   ```

## 📚 Documentation Files Created

1. `AWS_CREDENTIALS_SETUP.md` - Detailed setup and troubleshooting guide
2. `DEPLOYMENT_STATUS.md` - Complete deployment status and verification
3. `QUICK_START.md` - This file (quick reference)

---

**Status**: ✅ Container is running with AWS credentials mounted
**Next**: Test your API endpoints that use AWS EventBridge!

