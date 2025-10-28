**In Progress ...**

# 🧠 Bolic — Training Platform

Bolic is a functionally designed science based fitness training app — built with **Azure Functions**, **LanguageExt v5**, and **C#** functional programming principles.

The goal of Bolic is to create a fully composable, type-safe, and scalable backend that models real-world training structures (meso / macro cycles, training days, and volume tracking). 

It's designed in a way to allow for quick iterations to always use what's "most optimal," staying up to date with current literature. I wanted something specifically for SBLs (Science Based Lifters) and people who want to track their lifts in the most optimal way.

---

## 📦 Package Structure

### **Bolic.Shared.Core**
> Core functional and runtime abstractions.

This is the foundation of the project. It defines the core types, effects, and runtime configuration used throughout the Bolic ecosystem.

#### Includes:
- `Runtime` — provides access to core dependencies similar to DI (e.g., `CosmosClient`, `ILogger`).
- `Has<>` implementations — allows type-safe dependency extraction via `Eff` and `Ask`.
- `Utils` — functional helpers for parsing HTTP requests (`Option<T>`, JSON deserialization, etc.).
- Global `using` imports for:
  - `LanguageExt`
  - `Azure.Identity`
  - `Microsoft.Azure.Cosmos`
  - Logging abstractions (`ILogger`, `NullLogger`)


---

### **Bolic.Shared.Tap**
> Input processing and request mapping layer.

The **Tap** package ("Transform and Process") is responsible for handling incoming Azure Function HTTP requests, extracting data, and converting it into strongly-typed functional models, domain types in the future.

#### Includes:
- `TapResult<T>` — request box.
- `Tap.Process<T>` — converts `HttpRequestData` into a `TapResult<T>` using composable functional effects.
- Integration with `Utils` for reading and decoding JSON bodies.

#### Depends on:
- `Bolic.Shared.Core`

---

### **Bolic.Shared.Database**
> Cosmos DB access layer.

Encapsulates all database interactions using **LanguageExt `Eff`** effects and explicit error handling through `Either`.

#### Includes:
- `Create`, `Read`, `Query`, and future `Update/Delete` functions.
- `DatabaseError` — functional error type for Cosmos operations.
- `CreateRequest<T>`, `CreateResponse<T>` — structured request/response models.
- Lazy and eager query helpers using `FeedIterator<T>`.

#### Design:
All database actions are modeled as pure functions:
```csharp
Eff<Runtime, Either<DatabaseError, CreateResponse<T>>>