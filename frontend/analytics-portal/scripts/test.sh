#!/usr/bin/env bash
# Ejecuta las pruebas de forma efímera en un contenedor de Docker.
set -euo pipefail

cd "$(dirname "$0")/.."
docker run --rm \
    -v "$PWD":/app \
    -w /app \
    mcr.microsoft.com/playwright:v1.60.0-noble \
    sh -c "corepack enable && pnpm install --frozen-lockfile && pnpm test && pnpm test:e2e"
