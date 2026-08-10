# Local and deployment secrets

This project does not store credentials in Git. The checked-in [.env.example](.env.example) lists the required environment-variable names.

For local development, use .NET user secrets (already enabled by the project):

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;User Id=...;Password=...;Encrypt=True;TrustServerCertificate=True"
dotnet user-secrets set "Authentication:Google:ClientId" "..."
dotnet user-secrets set "Authentication:Google:ClientSecret" "..."
```

For GitHub Actions or a hosted deployment, configure the same values as protected secrets/environment variables, replacing `:` with `__`:

```text
ConnectionStrings__DefaultConnection
Authentication__Google__ClientId
Authentication__Google__ClientSecret
```

If the old values were ever pushed to a remote repository, rotate the SQL password and Google OAuth client secret; removing them from the current files does not remove them from Git history.
