#!/usr/bin/env bash
set -euo pipefail

# Load .env if it exists
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"
[ -f "$ENV_FILE" ] && set -a && source "$ENV_FILE" && set +a

: "${NUGET_API_KEY:?Environment variable NUGET_API_KEY is required}"

CSPROJ="src/KafkaProducer/KafkaProducer.csproj"
OUTPUT_DIR="./artifacts"

echo "Building and packing..."
dotnet pack "$CSPROJ" --configuration Release --output "$OUTPUT_DIR" --no-restore

echo "Publishing to NuGet.org..."
dotnet nuget push "$OUTPUT_DIR"/*.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" \
  --skip-duplicate

echo "Done. Package published to NuGet.org."
