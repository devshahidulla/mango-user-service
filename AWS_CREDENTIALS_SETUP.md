# AWS Credentials in Docker Container

## What Was Changed

The `docker-compose.yml` file has been updated to mount your local AWS credentials into the container.

### Changes Made:

1. **Volume Mount**: Your Windows AWS credentials directory is mounted into the container
   ```yaml
   volumes:
     - ${USERPROFILE}/.aws:/root/.aws:ro
   ```
   - This maps `C:\Users\YourUsername\.aws` to `/root/.aws` in the container
   - `:ro` makes it read-only for security

2. **Environment Variables**: Tell the AWS SDK where to find credentials
   ```yaml
   AWS_SHARED_CREDENTIALS_FILE: /root/.aws/credentials
   AWS_CONFIG_FILE: /root/.aws/config
   ```

## How to Use

### Prerequisites
- Ensure your local AWS credentials exist at: `C:\Users\YourUsername\.aws\credentials`
- Make sure Docker Desktop is running

### Steps to Run:

1. **Start Docker Desktop** (if not already running)

2. **Rebuild and restart the container:**
   ```powershell
   docker-compose down
   docker-compose up -d
   ```

3. **Check the logs to verify AWS connection:**
   ```powershell
   docker-compose logs -f user-api
   ```

4. **Test the API:**
   ```powershell
   curl http://localhost:5001/health
   ```

## Troubleshooting

### If you still get AWS credential errors:

1. **Verify your local credentials exist:**
   ```powershell
   cat $env:USERPROFILE\.aws\credentials
   ```

2. **Check if credentials are mounted in container:**
   ```powershell
   docker exec mango-user-api ls -la /root/.aws
   docker exec mango-user-api cat /root/.aws/credentials
   ```

3. **Verify AWS SDK can read the credentials:**
   ```powershell
   docker exec mango-user-api printenv | findstr AWS
   ```

## Alternative: Using Environment Variables

If you prefer not to mount files, you can pass credentials as environment variables:

```yaml
environment:
  AWS_ACCESS_KEY_ID: your-access-key-id
  AWS_SECRET_ACCESS_KEY: your-secret-access-key
  AWS_DEFAULT_REGION: us-east-1
```

**⚠️ Warning:** This method is less secure as credentials are visible in Docker inspect output.

## Security Notes

- The credentials are mounted as read-only (`:ro`)
- Never commit AWS credentials to version control
- For production, use IAM roles or AWS Secrets Manager instead

