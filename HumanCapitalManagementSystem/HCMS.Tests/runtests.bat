@echo off

set CONFIGURATION=Debug
set FRAMEWORK_VERSION=net6.0

set DOCKER_COMPOSE_FILE=docker-compose.yml

set SQL_SERVER_CONTAINER_NAME=hcmstests-sql-server-1
set SA_PASSWORD=SecretPassword12345!
set DATABASE_NAME=HCMSImmedis

set SCRIPT_FILE_PATH=./TestScript-Auto-Generated

rem Start the SQL Server container
docker-compose -f %DOCKER_COMPOSE_FILE% up -d

rem Wait for the SQL Server container to start (adjust as needed)
timeout /t 5 /nobreak > nul

rem Run SQL command to create a database
docker exec -it %SQL_SERVER_CONTAINER_NAME% /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P %SA_PASSWORD% -Q "CREATE DATABASE %DATABASE_NAME%;"

rem Run SQL script to create tables in the database
docker exec -it %SQL_SERVER_CONTAINER_NAME% /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P %SA_PASSWORD% -d %DATABASE_NAME% -i %SCRIPT_FILE_PATH%
