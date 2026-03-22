#!/usr/bin/env bash
set -euo pipefail

CSPROJ="src/KafkaProducer/KafkaProducer.csproj"
BUMP="${1:-patch}"

current=$(grep -oP '(?<=<Version>)[^<]+' "$CSPROJ")
IFS='.' read -r major minor patch <<< "$current"

case "$BUMP" in
  major) major=$((major + 1)); minor=0; patch=0 ;;
  minor) minor=$((minor + 1)); patch=0 ;;
  patch) patch=$((patch + 1)) ;;
  *)
    echo "Usage: $0 [major|minor|patch]"
    exit 1
    ;;
esac

new_version="$major.$minor.$patch"
sed -i "s|<Version>$current</Version>|<Version>$new_version</Version>|" "$CSPROJ"

echo "Version bumped: $current -> $new_version"

if git rev-parse --git-dir > /dev/null 2>&1; then
  git add "$CSPROJ"
  git commit -m "chore: bump version to $new_version"
  git tag "v$new_version"
  echo "Git tag v$new_version created."
fi
