#!/usr/bin/env bash
set -euo pipefail

# Load .env if it exists
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"
[ -f "$ENV_FILE" ] && set -a && source "$ENV_FILE" && set +a

: "${GITHUB_TOKEN:?Environment variable GITHUB_TOKEN is required}"
: "${GITHUB_OWNER:?Environment variable GITHUB_OWNER is required}"

CSPROJ="src/KafkaProducer/KafkaProducer.csproj"
OUTPUT_DIR="./artifacts"
FEED_URL="https://nuget.pkg.github.com/${GITHUB_OWNER}/index.json"

echo "Building and packing..."
dotnet pack "$CSPROJ" --configuration Release --output "$OUTPUT_DIR" --no-restore

echo "Publishing to GitHub Packages ($FEED_URL)..."
GITHUB_TOKEN="$GITHUB_TOKEN" dotnet nuget push "$OUTPUT_DIR"/*.nupkg \
  --source "github" \
  --api-key "$GITHUB_TOKEN" \
  --skip-duplicate

echo "Done. Package published to GitHub Packages."
