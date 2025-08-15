#!/bin/bash
# Usage: ./add-migration.sh MigrationName
# Example: ./add-migration.sh InitialCreate

if [ -z "$1" ]
then
  echo "Error: You must provide a migration name."
  echo "Usage: ./add-migration.sh MigrationName"
  exit 1
fi

dotnet ef migrations add $1