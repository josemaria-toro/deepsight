#!/usr/bin/env bash
# Ejecuta las pruebas de forma efímera en un contenedor de Docker.
set -euo pipefail

cd "$(dirname "$0")/.."
docker run --rm \
    -v "$PWD":/app \
    -w /app \
    -e UV_PROJECT_ENVIRONMENT=/tmp/venv \
    ghcr.io/astral-sh/uv:python3.13-bookworm \
    sh -c "uv sync --frozen && uv run pytest"
