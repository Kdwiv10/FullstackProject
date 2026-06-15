# FullstackProject (Shangri)

FullstackProject is an ASP.NET Core MVC web application for a digital game store and subscription service called **Shangri**. The platform combines a game catalog, membership plans, user identity, developer contact management, and customer reviews in a single full-stack application.

## What this project does

The application supports common game platform workflows:

- Browse a storefront of games.
- View game details and related developer information.
- Manage memberships.
- Store and view reviews.
- Manage user game libraries.
- Maintain developer contact details.
- Authenticate users with ASP.NET Identity and optional Google sign-in.
- Restrict admin/developer actions with role-based authorization.

## Programming languages used and their purpose

| Language | Where it appears | What it is used for |
|---|---|---|
| **C#** | `Program.cs`, `Controllers/`, `Model/`, `Data/`, `Areas/Identity/Pages/*.cshtml.cs` | Backend application logic, routing, controller actions, Entity Framework Core data access, identity/authentication setup, and business/domain models. |
| **Razor (CSHTML)** | `Views/**/*.cshtml`, `Areas/Identity/Pages/**/*.cshtml` | Server-rendered UI templates that combine HTML markup with C# expressions and tag helpers. |
| **HTML** | Inside `.cshtml` views/layouts | Page structure for storefront, lists, forms, navigation, and identity pages. |
| **CSS** | `wwwroot/css/*.css`, `Views/Shared/_Layout.cshtml.css` | Styling for UI pages (front page, login/register, memberships, reviews, developer pages, and shared layout). |
| **JavaScript** | `wwwroot/js/site.js`, inline scripts in views, `wwwroot/lib/*` | Client-side behavior such as AJAX autosuggest on developer search and UI interactivity (Bootstrap/jQuery). |
| **SQL (via EF Core + SQL Server)** | Data model mappings in `Model/S22024Group2ProjectContext.cs` and migrations | Persistent storage for users, games, memberships, purchases, payments, libraries, reviews, and roles. Queries are generated through Entity Framework Core. |
| **JSON** | `appsettings*.json`, `Properties/launchSettings.json` | Environment/configuration settings, connection strings, logging, and local run profiles. |

## Technical architecture

### 1) Web layer (MVC)
- **Controllers** (`Controllers/`) expose endpoint actions for major entities:
  - `GamesController`
  - `MembershipsController`
  - `ReviewsController`
  - `GameLibrariesController`
  - `DevelopersController`
  - `HomeController`
- **Views** (`Views/`) render strongly typed Razor pages for list/create/edit/details/delete flows.

### 2) Data layer
- **Entity Framework Core** is used with `DbContext` classes:
  - `ApplicationDbContext` for ASP.NET Core Identity.
  - `S22024Group2ProjectContext` for the application domain model.
- Domain entities in `Model/` include:
  - `Game`, `Developer`, `User`, `Role`
  - `Membership`, `GameLibrary`, `Review`
  - `Purchase`, `Payment`
  - Identity-related entities (`AspNetUser`, `AspNetRole`, etc.)

### 3) Authentication and authorization
- Configured in `Program.cs` with `AddDefaultIdentity<IdentityUser>()` and role support via `AddRoles<IdentityRole>()`.
- Optional Google OAuth login is configured in the authentication pipeline.
- Role-based access control is applied to selected actions using `[Authorize(Roles = "...")]`.

### 4) Frontend/static assets
- Static files are served from `wwwroot/`.
- Bootstrap and jQuery are included for UI components and client-side interactions.
- Custom CSS files provide page-specific themes and layout adjustments.

## Key project flow

1. The user opens the site and navigates through Home, Store, Memberships, Developers, and Reviews.
2. MVC routes map incoming requests to controller actions.
3. Controllers read/write data through EF Core contexts.
4. Razor views render data back to HTML pages.
5. Identity middleware handles registration/login and role checks.

## Repository structure (high-level)

- `FullstackProject.sln` - Solution entry point.
- `FullstackProject/Program.cs` - Application startup and middleware configuration.
- `FullstackProject/Controllers/` - MVC controllers for features.
- `FullstackProject/Views/` - Razor UI templates.
- `FullstackProject/Model/` - Entity classes and primary database context.
- `FullstackProject/Data/` - Identity DB context and migrations.
- `FullstackProject/Areas/Identity/` - Built-in Identity UI pages.
- `FullstackProject/wwwroot/` - CSS, JavaScript, images, and third-party frontend libraries.

## Running the project locally

### Prerequisites
- .NET 8 SDK
- SQL Server (or accessible SQL Server instance)

### Basic run steps
1. Open the repository root.
2. Restore dependencies and build:
   - `dotnet build FullstackProject.sln`
3. Run the web application:
   - `dotnet run --project FullstackProject/FullstackProject.csproj`

The app will then be available at the local URL shown in terminal output.

## Summary

This project is a full-stack game subscription/store web application built primarily with **C# and ASP.NET Core MVC**, using **Razor + HTML/CSS/JavaScript** for the UI and **SQL Server through Entity Framework Core** for persistence. It demonstrates a complete end-to-end architecture with authentication, authorization, CRUD features, and relational data modeling.
