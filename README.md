![ASP.NET Core](https://badgen.net/badge/ASP.NET/Core/purple?icon=dotnet)
![.NET](https://img.shields.io/badge/.NET-10.0-purple?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-10.0-green?logo=efcore&logoColor=white)
![MSSQL](https://img.shields.io/badge/Microsoft_SQL_Server-DB-red?logo=microsoftsqlserver&logoColor=white)
![Web API](https://img.shields.io/badge/Web_API-REST-blue?logo=swagger&logoColor=white)




# 💪 Fitness Tracker API 

A RESTful API for tracking workouts, exercises, and body metrics

## 💡 Motivation

I built FitTrack to showcase backend development with .NET — my favorite area of software engineering. I've started several side projects before, but often abandoned them once the scope grew too large to finish. This time, I deliberately kept the project focused: a single, well-defined domain with clear business rules, built end-to-end rather than left half-done.

I'm also into fitness myself, which made this domain a natural fit — workout tracking, 
calorie calculations, and body metrics involve just enough real-world logic (BMR/TDEE formulas, nutrient tracking, workout history) to be genuinely interesting to model, 
without ballooning into something unmanageable.


## 🔨 Tech Stack

* **Backend:** ASP.NET Core Web API
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Auth:** ASP.NET Core Identity
* **API Documentation:** Swagger

## Architecture

The solution is split into three projects:

- **FitTrackApi.Core** — Shared class library with EF Core entities, DTOs, configuration models, and extension methods.
- **FitTrackApi.Server** — REST API server containing controllers, services im, and the EF Core `DataContext`.
- **FitTrackApi.Test** — xUnit test project with unit tests for business logic and integration tests that run against an isolated database via Testcontainers (Docker).

### Server-side architecture

`FitTrackApi.Server` follows a **Controller + Service** architecture:

Controller → Service → EF Core DbContext → SQL Server

Controllers are kept thin — they handle HTTP concerns only (routing, model binding, 
status codes) and delegate all business logic to services. Each domain (Users, 
Workouts, Exercises, Body Metrics) has its own service and interface, registered 
via dependency injection.

An earlier iteration used a CQRS-style command/query dispatcher, but for this 
project's CRUD-focused scope it added unnecessary indirection without real benefit — 
refactored to the simpler Controller → Service flow above.

### Authentication

Authentication uses **HttpOnly cookies** via ASP.NET Core Identity, rather than 
JWTs stored client-side. Since the cookie is inaccessible to JavaScript, it 
significantly reduces the risk of token theft via XSS — the browser attaches it 
automatically on each request, and the client never handles the token directly. 
Protected endpoints are guarded with `[Authorize]`, and the current user's id is 
read from `ClaimTypes.NameIdentifier`.

## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|--------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Log in, sets HttpOnly auth cookie |
| POST | `/api/auth/logout` | Log out, clears auth cookie |
| GET | `/api/auth/me` | Get currently authenticated user |

### Users
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/user/me` | Get current user's info |
| GET | `/api/user/{userId}` | Get user info by id |
| POST | `/api/user/{userId}` | Update user info |
| DELETE | `/api/user/{userId}` | Delete user account |

### Body Metrics
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/bodymetrics/{userId}` | Get body metrics (height, weight, BMR/TDEE, macros) |
| PUT | `/api/bodymetrics/{userId}` | Update body metrics |

### Workouts
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/workout` | Get all workouts for current user |
| GET | `/api/workout/{workoutId}` | Get a specific workout |
| POST | `/api/workout` | Create a new workout |
| PUT | `/api/workout/{workoutId}` | Update a workout |
| DELETE | `/api/workout/{workoutId}` | Delete a workout |

### Exercises
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/exercise?pageNumber=1&pageSize=10` | Get paged list of exercises |
| GET | `/api/exercise/{exerciseId}` | Get exercise details |

Full interactive documentation is available via Swagger UI at `/swagger` in Development.

## 🚀 Getting started 
### Prerequisites
- .NET 10 SDK
- Microsoft SQL Server (Express, LocalDB, or full edition — any works for local development)
- Docker (only required for running integration tests via Testcontainers)

### Setup

1. Clone the repository
    ```bash
   > git clone https://github.com/mykhail-b/FitTrackApi.git
   > cd FitTrackApi
   > dotnet restore
    ```

2. Set up local secrets (connection string, SMTP config)
    ```bash
    > cd FitTrackApi.Server
    > dotnet user-secrets init
    > dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=FitTrackDb Trusted_Connection=True;"
    ```

3. Apply EF Core migrations
   ```bash
   > dotnet ef database update
   ```

4. Run the API
   ```bash
   > dotnet run
   ```

5. Open Swagger UI at `https://localhost:7114/swagger`

### Running tests
```bash
> cd FitTrackApi.Test
> dotnet test
```
Integration tests spin up a real SQL Server instance in Docker via Testcontainers — 
make sure Docker is running before executing them.

## ✍️ Lessons Learned

- Refactored from CQRS-style dispatchers to a simpler Controller → Service 
  architecture once it became clear the indirection wasn't paying for itself 
  in a CRUD-focused project
- Learned proper environment-based configuration (User Secrets, env vars) 
  instead of hardcoding connection strings
- First time using Testcontainers for integration tests instead of mocking 
  the database entirely