# Task Management System

A full-stack task management application built with .NET Core and Angular.

## Prerequisites

- Docker
- Docker Compose

## Running the Application

1. Clone the repository
```bash
git clone <repository-url>
```

2. Start the application:
```bash
docker compose up --build -d
```

## Services

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5031
- **SQL Server**: 
  - Port: 1433
  - User: sa
  - Password: P@ssw0rd@SqlServer

## Running Tests

### Backend Tests
```bash
cd TaskManagementApi
dotnet test
```

### Integration Tests
```bash
cd TaskManagementApi/TaskManagementApi.Tests.Integration
dotnet test
```

## Stop Application

```bash
docker-compose down
```

## Tech Stack

- Backend: .NET Core
- Frontend: Angular
- Database: SQL Server 2022
