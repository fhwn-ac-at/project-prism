#!/bin/bash
# Pay attention to line ending when edditing this file on Windows!!! It must be Unix style (LF).
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "postgres" <<-EOSQL
	CREATE USER $DEV_USER WITH ENCRYPTED PASSWORD '$DEV_PASSWORD';
CREATE DATABASE $DEV_SCHEMA WITH OWNER=$DEV_USER ENCODING = 'UTF8' CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE $DEV_SCHEMA to $DEV_USER;

ALTER SYSTEM SET max_connections = 500;

EOSQL