#!/bin/bash
# Usage: ./generate-sql.sh [FromMigration] [ToMigration]
# Generates SQL script from migrations

FROM=${1:-0}   # Default from first migration
TO=${2:-}      # Default to latest

if [ -z "$TO" ]; then
    dotnet ef migrations script $FROM
else
    dotnet ef migrations script $FROM $TO
fi