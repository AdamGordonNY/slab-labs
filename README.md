# Slab Labs

Full-stack scaffold: Next.js frontend + ASP.NET Core Web API (EF Core / Npgsql) backend + PostgreSQL.

## Structure

- `frontend/` — Next.js 15 app (TypeScript, App Router)
- `backend/` — ASP.NET Core 10 Web API with EF Core, using `Npgsql.EntityFrameworkCore.PostgreSQL`
- `docker-compose.yml` — Local PostgreSQL instance

## Getting started

### 1. Start PostgreSQL

```powershell
docker compose up -d
```

### 2. Backend (ASP.NET Core API)

```powershell
cd backend
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

The API listens on the URL printed in the console (see `Properties/launchSettings.json`), with Swagger/OpenAPI available in development at `/openapi/v1.json`. A sample `Items` CRUD endpoint is at `/api/items`.

Connection string lives in `appsettings.Development.json` (`ConnectionStrings:DefaultConnection`). For non-local environments, set it via user-secrets or the `ConnectionStrings__DefaultConnection` environment variable instead of committing it to `appsettings.json`.

### 3. Frontend (Next.js)

```powershell
cd frontend
npm run dev
```

Runs at http://localhost:3000. Set `NEXT_PUBLIC_API_URL` (e.g. in `frontend/.env.local`) to point at the backend API base URL.

## Notes

- CORS on the backend is configured via `AllowedOrigins` in `appsettings.json` (defaults to `http://localhost:3000`).
- Use `dotnet ef migrations add <Name>` after modifying `Models`/`AppDbContext` to generate new migrations.
