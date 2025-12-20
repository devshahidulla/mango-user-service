-- Migration: Split fullname into firstname and lastname
-- Database: mongo_user_service
-- Description: Migrate from fullname to firstname/lastname structure

-- Step 1: Add new columns
ALTER TABLE users ADD COLUMN IF NOT EXISTS firstname VARCHAR(100);
ALTER TABLE users ADD COLUMN IF NOT EXISTS lastname VARCHAR(100);

-- Step 2: Migrate existing data from fullname to firstname/lastname
UPDATE users 
SET 
    firstname = COALESCE(SPLIT_PART(fullname, ' ', 1), ''),
    lastname = COALESCE(NULLIF(SPLIT_PART(fullname, ' ', 2), ''), SPLIT_PART(fullname, ' ', 1))
WHERE fullname IS NOT NULL;

-- Step 3: Set NOT NULL constraints (after data migration)
ALTER TABLE users ALTER COLUMN firstname SET NOT NULL;
ALTER TABLE users ALTER COLUMN lastname SET NOT NULL;

-- Step 4: Drop old fullname column
ALTER TABLE users DROP COLUMN IF EXISTS fullname;

-- Verification query (comment out when running)
-- SELECT userid, firstname, lastname, email FROM users LIMIT 10;

