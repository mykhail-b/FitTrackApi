[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-purple?logo=dotnet&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple?logo=dotnet&logoColor=white)](#)
![MSSQL](https://img.shields.io/badge/Microsoft_SQL_Server-DB-red?logo=microsoftsqlserver&logoColor=white)
![Web API](https://img.shields.io/badge/Web_API-REST-blue?logo=swagger&logoColor=white)
[![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)](#)
![Build and Test](https://github.com/mykhail-b/FitTrackApi/actions/workflows/build-and-test.yml/badge.svg)


# 💪 Fitness Tracker API 

A RESTful API for tracking workouts, exercises, and body metrics

## 💡 Motivation

I built FitTrack to improve my backend development skills with .NET and to have a project that reflects the kind of software I enjoy building.

In the past, I started several side projects but made them too ambitious and never finished them. With FitTrack, I decided to keep the scope focused and build a complete application instead of constantly adding new features.

Fitness was a natural choice because it involves real business logic, such as workout history and body metrics, while keeping the project manageable.

## Features

- User registration and authentication
- Workout management
- Exercise catalog
- Body metrics tracking
- Workout activity calendar
- Cookie-based authentication

## 🔨 Tech Stack

* **Backend:** ASP.NET Core Web API
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Authentication:** ASP.NET Core Identity
* **Authorization:** Cookie Authentication (HttpOnly Cookies)
* **API Documentation:** Scalar
* **Deployment:** Docker

## Architecture

The solution is organized into the following projects:

- **FitTrackApi.Server** — ASP.NET Core Web API entry point. Contains controllers, dependency injection configuration, middleware, and application startup.

- **FitTrackApi.Application** — Contains business logic, application services, DTOs, mappers, and interfaces implemented by the Infrastructure layer.

- **FitTrackApi.Domain** — Contains domain entities and core domain models shared across the application.

- **FitTrackApi.Infrastructure** — Contains data access with EF Core (`DataContext`, repositories, migrations) and infrastructure services such as ASP.NET Core Identity, email services, and other external integrations.

- **FitTrackApi.Test** — xUnit test project with unit tests for business logic and integration tests using Testcontainers.

### Server-side architecture

`FitTrackApi.Server` is built as a layered monolithic application.

Controllers are responsible for handling HTTP requests and responses, while business logic is implemented in dedicated services. Data access is implemented with Entity Framework Core through repositories backed by `DataContext`, which serves as the application's data access layer. Dependencies are managed using ASP.NET Core's built-in dependency injection.

### Authentication

Authentication is implemented with **ASP.NET Core Identity** using **HttpOnly authentication cookies**. After a successful sign-in, the browser automatically includes the authentication cookie with subsequent requests, so the client does not need to manage authentication tokens manually. Protected endpoints are secured with the `[Authorize]` attribute, and the authenticated user's identifier is retrieved from `ClaimTypes.NameIdentifier`.


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
| GET | `/api/workout/activity` | Get a user workout activity date list |
| POST | `/api/workout` | Create a new workout |
| PUT | `/api/workout/{workoutId}` | Update a workout |
| DELETE | `/api/workout/{workoutId}` | Delete a workout |

### Exercises
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/exercise?pageNumber=1&pageSize=10` | Get paged list of exercises |
| GET | `/api/exercise/{exerciseId}` | Get exercise details |

Full interactive documentation is available via Scalar UI.

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
    > dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=FitTrackDb;Trusted_Connection=True;"
    ```

3. Apply EF Core migrations
   ```bash
   > dotnet ef database update
   ```

4. Run the API
   ```bash
   > dotnet run
   ```

5. Open Scalar UI at `https://localhost:7114/scalar/v1`

### Running tests
```bash
> cd FitTrackApi.Test
> dotnet test
```
Integration tests spin up a real SQL Server instance in Docker via Testcontainers — 
make sure Docker is running before executing them.