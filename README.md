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
- Users workout lists and activity data
- Food catalog
- Cookie-based authentication

## 🔨 Tech Stack

* **Backend:** ASP.NET Core Web API
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Authentication:** ASP.NET Core Identity
* **Authorization:** Cookie Authentication (HttpOnly Cookies)
* **API Documentation:** Scalar

## Architecture

The solution is organized into the following projects:

- **FitTrackApi.Server** — ASP.NET Core Web API entry point. Contains controllers, dependency injection configuration, middleware, and application startup.

- **FitTrackApi.Application** — Contains business logic, CQRS service used by MediatR, DTOs, mappers, and interfaces implemented by the Infrastructure layer.

- **FitTrackApi.Domain** — Contains domain entities and core domain models shared across the application.

- **FitTrackApi.Infrastructure** — Contains data access with EF Core (`DataContext`, repositories, migrations) and infrastructure services such as ASP.NET Core Identity, email services, and other external integrations.

### Authentication

Authentication is implemented with **ASP.NET Core Identity** using **HttpOnly authentication cookies**. After a successful sign-in, the browser automatically includes the authentication cookie with subsequent requests, so the client does not need to manage authentication tokens manually. Protected endpoints are secured with the `[Authorize]` attribute, and the authenticated user's identifier is retrieved from `ClaimTypes.NameIdentifier`.


## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|--------------|
| POST | `/api/v1/auth/register` | Register a new user |
| POST | `/api/v1/auth/login` | Log in, sets HttpOnly auth cookie |
| POST | `/api/v1/auth/logout` | Log out, clears auth cookie |
| GET | `/api/v1/auth/me` | Get currently authenticated user |

### User
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/v1/user/me` | Get current user's info |
| GET | `/api/v1/user/{userId}` | Get user info by id |
| POST | `/api/v1/user/{userId}` | Update user info |
| DELETE | `/api/v1/user/{userId}` | Delete user account |


### Exercises
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/v1/exercise?pageNumber=1&pageSize=10` | Get paged list of exercises |
| GET | `/api/v1/exercise/{exerciseId}` | Get exercise details |
| POST | `/api/v1/exercise` | Create a new exercise |
| PUT | `/api/v1/exercise/{exerciseId}` | Update an exercise |
| DELETE | `/api/v1/exercise/{exerciseId}` | Delete an exercise |
 
### Food
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/v1/food?pageNumber=1&pageSize=10` | Get paged list of food items |
| GET | `/api/v1/food/{foodId}` | Get food item details |
| POST | `/api/v1/food` | Create a new food item |
| PUT | `/api/v1/food/{foodId}` | Update a food item |
| DELETE | `/api/v1/food/{foodId}` | Delete a food item |
 
### Workouts
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/v1/workout?pageNumber=1&pageSize=10` | Get paged list of workouts for the current user |
| GET | `/api/v1/workout/{workoutId}` | Get a specific workout |
| GET | `/api/v1/workout/activity` | Get the current user's workout activity dates |
| POST | `/api/v1/workout` | Create a new workout |
| PUT | `/api/v1/workout/{workoutId}` | Update a workout |
| DELETE | `/api/v1/workout/{workoutId}` | Delete a workout |

## 🚀 Getting started 
### Prerequisites
- .NET 10 SDK
- Microsoft SQL Server (Express, LocalDB, or full edition — any works for local development)

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
   > dotnet run --project /src/FitTrackApi.Server
   ```

5. Open Scalar UI at `https://localhost:7114/scalar/v1`