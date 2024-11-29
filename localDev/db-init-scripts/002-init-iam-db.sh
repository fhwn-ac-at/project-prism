#!/bin/bash
# Pay attention to line ending when edditing this file on Windows!!! It must be Unix style (LF).
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "postgres" <<-EOSQL
	CREATE USER "$KC_DB_USERNAME" WITH ENCRYPTED PASSWORD '$KC_DB_PASSWORD';
CREATE DATABASE "$KC_DB_SCHEMA" WITH OWNER="$KC_DB_USERNAME" ENCODING = 'UTF8' CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE "$KC_DB_SCHEMA" to "$KC_DB_USERNAME";

EOSQL