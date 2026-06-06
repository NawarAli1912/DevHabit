# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

DevHabit is an ASP.NET Core 10 Web API project targeting `.NET 10`. It is in early development — the current codebase is a scaffold with OpenTelemetry observability wired in from the start.

## Commands

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project DevHabit.Api
```

### Run with Docker Compose
```bash
docker compose up --build
```
The API is exposed on ports `6000` (HTTP) and `6001` (HTTPS) via Docker.

### Run tests
```bash
dotnet test
```

### Single test
```bash
dotnet test --filter "FullyQualifiedName~<TestName>"
```

## Architecture

Single-project solution (`DevHabit.Api`) — no separate domain/infrastructure layers yet. All application entry is in `Program.cs`.

**Key infrastructure choices already in place:**

- **OpenTelemetry** (`Program.cs`): Traces, metrics, and logs are all exported via OTLP (`UseOtlpExporter()`). Instrumentation covers ASP.NET Core, HTTP client, and .NET runtime. Logs include scopes (`IncludeScopes = true`).
- **OpenAPI**: Mapped via `app.MapOpenApi()` in development only.
- **Central package management**: All NuGet versions are defined in `Directory.Packages.props`; individual `.csproj` files omit version attributes.
- **Build strictness** (`Directory.Build.props`): `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild`, `AnalysisMode=All`, and `SonarAnalyzer.CSharp` are applied solution-wide. All warnings are errors — fix analyzer warnings before committing.
- **Nullable reference types**: Enabled globally; use `?` annotations and null checks appropriately.

## Conventions

- Controllers go in `DevHabit.Api/Controllers/` and use `[ApiController]` + `[Route("[controller]")]`.
- Add new NuGet packages to `Directory.Packages.props` with a version, then reference them from the `.csproj` without a version.
- Docker target OS is Linux (`DockerDefaultTargetOS`).
