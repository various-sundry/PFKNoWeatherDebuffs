#!/bin/bash
set -euxo pipefail

dotnet publish -c Release -o bin/NoWeatherDebuffs

VERSION=$(git tag --points-at HEAD | grep '^v[0-9]')
ZIPFILE="NoWeatherDebuffs-$VERSION.zip"

cd bin
rm -f "$ZIPFILE"
zip -r "$ZIPFILE" NoWeatherDebuffs
