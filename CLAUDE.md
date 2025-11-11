# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Bolic is a science-based fitness training platform built with Azure Functions, LanguageExt v5, and functional programming principles in C#. The system models hierarchical training structures (macrocycles, microcycles, training days, exercises, and sets) with a focus on composability, type-safety, and Railway-Oriented Programming (ROP).

## Architecture

### Three-Layer Architecture

1. **Bolic.Backend.Domain** - Pure domain models using `Option<T>` types and LanguageExt functional types
2. **Bolic.Backend.Api** - API models with JSON serialization for HTTP requests/responses
3. **Bolic.Backend.Core** - Azure Functions endpoints and business logic using `Eff<Runtime, T>` effects

### Key Architectural Patterns

**Functional Effects with LanguageExt v5**: All operations use `Eff<Runtime, T>` monads for composable, pure functional code. The `Runtime` type provides dependency access (CosmosClient, ILogger) via the `Has<>` trait pattern.

**Transformer Pattern**: Bidirectional conversion between Domain and API layers happens in `Transformers/` using `ToDt()` (to Domain) and `ToApi()` (to API) extension methods. Both return `Option<T>` to handle conversion failures.

**Railway-Oriented Programming**: LINQ query syntax chains effects using `from`/`select`. Errors short-circuit automatically via `Either<Error, T>`. All Azure Functions follow this pattern.

**Patch Operations**: Partial updates use `GetPatchOperations()` extension methods in `PatchOperations/` that convert Domain models to Cosmos DB `PatchOperation` lists, only including fields that are `Some`.

**Shared Packages**: Core functionality comes from `Bolic.Shared.*` NuGet packages:
- `Bolic.Shared.Core` - Runtime, dependency injection via `Has<>`, utility functions
- `Bolic.Shared.Tap` - HTTP request processing via `Tap.Process<T>()`
- `Bolic.Shared.Database` - Cosmos DB operations wrapped in `Eff` (Create, Read, Update, Patch, Query)

### Domain Model Hierarchy

```
Macrocycle (training program)
  └── Microcycle (weekly structure)
      └── TrainingDay (workout template)
          └── TrainingExercise
              └── TrainingSet
```

**TrainingSession** is a runtime instance created from a TrainingDay template, used to record actual workout data.

### Project Structure

- **Bolic.Backend.Core/** - Azure Functions endpoints (e.g., `TrainingDay.cs`, `TrainingExercise.cs`, `TrainingSession.cs`)
  - `Logic/` - Business logic services
  - `Transformers/` - Domain ↔ API conversions
  - `PatchOperations/` - Cosmos DB patch operation builders
  - `Util/` - Extension methods for effects and HTTP responses
- **Bolic.Backend.Domain/** - Pure domain records with `Option<T>` fields
- **Bolic.Backend.Api/** - DTOs for HTTP serialization
- **iac/** - Bicep infrastructure as code for Azure deployment

## Development Commands

### Build and Run

```bash
# Build the solution
dotnet build

# Run Azure Functions locally (from Bolic.Backend.Core)
cd Bolic.Backend.Core
func start
# OR
dotnet run
```

### Testing

The project currently does not have a test project configured. When adding tests, create a `Bolic.Backend.Tests` project.

### Infrastructure

```bash
# Validate Bicep templates
az deployment group validate \
  --resource-group <rg-name> \
  --template-file iac/main.bicep \
  --parameters iac/dev.bicepparam

# Deploy infrastructure (manual)
az deployment group create \
  --name "bolic-develop-deployment-$(date +%s)" \
  --resource-group <rg-name> \
  --template-file iac/main.bicep \
  --parameters iac/dev.bicepparam
```

Automated deployment uses `.github/workflows/infraDev.yaml` with OIDC authentication.

## Code Conventions

### Error Handling

- Use `Exceptional` custom error type with error codes (e.g., `new Exceptional("Missing id", 0101)`)
- Use `guard<Error>()` for validation (e.g., ensuring patch operations exist)
- Database operations return `Either<DatabaseError, T>` that must be matched or mapped

### Option Types

All Domain model fields use `Option<T>`. To extract values:
- `.ToEff()` - converts `Option<T>` to `Eff<T>`, fails if None
- `.ToEff(Error)` - provides custom error when None
- `.IfSome()` - executes action only if Some
- `.Match()` - handles both Some and None cases

### Function Endpoints

Standard pattern for Azure Function endpoints:

```csharp
[Function("FunctionName")]
public async Task<HttpResponseData> MethodName([HttpTrigger("verb", Route = "route")] HttpRequestData req)
{
    var program =
        from request in Tap.Process<Api.ModelType>(req)
        from body in request.Body.ToEff()
        from dt in body.ToDt().ToEff()
        // ... more transformations and database operations
        select databaseResponse;

    return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
}
```

### Cosmos DB Operations

All database operations require:
- **Id** - Document ID (Guid as string)
- **UserId** - Partition key (Guid as string)
- **Container** - Container name (e.g., "training-days", "exercises")
- **Database** - Always "bolic"

Request types: `CreateRequest<T>`, `ReadRequest`, `UpdateRequest<T>`, `PatchRequest<T>`

### Adding New Endpoints

1. Create Domain model in `Bolic.Backend.Domain/` using `Option<T>` fields
2. Create API model in `Bolic.Backend.Api/` with nullable reference types and `[JsonProperty]` attributes
3. Add Transformer in `Bolic.Backend.Core/Transformers/` with `ToDt()` and `ToApi()` methods
4. (If patching) Add `GetPatchOperations()` in `Bolic.Backend.Core/PatchOperations/`
5. Create Azure Function class in `Bolic.Backend.Core/` with CRUD endpoints

### Global Usings

`Global.cs` imports LanguageExt namespaces, Azure Functions types, and Bolic shared packages. Available everywhere without explicit `using` statements.

## Database Schema

**Cosmos DB** with database name "bolic" and containers:
- `training-days` - TrainingDay templates
- `training-sessions` - Actual workout instances
- `exercises` - TrainingExercise definitions

All documents use `UserId` as partition key and `Id` as the document id.

## Target Framework

.NET 8.0 with Azure Functions V4 runtime.
