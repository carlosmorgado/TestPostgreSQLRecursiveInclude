#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE DATABASE TestPostgreSQLRecursiveInclude;
EOSQL

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "TestPostgreSQLRecursiveInclude" <<-EOSQL
    CREATE TABLE TestClass (ID UUID);
    CREATE TABLE TestClassLink (ID UUID, FromTestClassId UUID, ToTestClassId UUID);
EOSQL