#!/bin/bash
# Usage: ./publish.sh [Configuration] [OutputFolder]
# Publishes the application

CONFIG=${1:-Release}
OUTPUT=${2:-./publish}

dotnet publish -c $CONFIG -o $OUTPUT
echo "Published to $OUTPUT"