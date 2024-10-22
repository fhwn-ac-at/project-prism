#!/bin/bash
# Pay attention to line ending when edditing this file on Windows!!! It must be Unix style (LF).
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$KC_DB_SCHEMA" <<-EOSQL
	CREATE SCHEMA $KC_DB_SCHEMA AUTHORIZATION $KC_DB_USERNAME;
	ALTER DEFAULT PRIVILEGES IN SCHEMA $KC_DB_SCHEMA GRANT ALL ON TABLES TO $KC_DB_USERNAME;

	CREATE EXTENSION pgcrypto;
	CREATE EXTENSION "uuid-ossp";

EOSQL