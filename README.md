# WalletCorp API

Sistema de gestión de carteras y beneficios corporativos desarrollado en .NET Core.

## Requisitos Previos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) o superior.
- [PostgreSQL](https://www.postgresql.org/download/) ejecutándose en el puerto `5432`.
- [Herramientas de EF Core](https://learn.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`.

## Configuración de la Base de Datos

1. Asegúrate de tener una instancia de PostgreSQL con las credenciales configuradas en `appsettings.json`.
2. El usuario por defecto es `postgres` con contraseña `postgres123`.
3. Aplica las migraciones iniciales para crear la base de datos `walletcorp`:
   ```bash
   dotnet ef database update
   ```

## Compilación y Ejecución

Para iniciar la API en modo de desarrollo:

```bash
# Restaurar dependencias
dotnet restore

# Compilar y ejecutar
dotnet run --project WalletCorp.API.csproj
```

La API estará disponible en: [http://localhost:5248](http://localhost:5248)

## Documentación de la API

- **OpenAPI/Swagger**: Disponible en `/openapi/v1.json` cuando el entorno es `Development`.
- **Ejemplos de Módulos**: Consulta la carpeta `Docs/Modules/` para ver ejemplos de uso de la API con PowerShell:
  - `auth.txt`: Registro e inicio de sesión.
  - `user.txt`: CRUD de usuarios con token JWT.

## Desarrollo

- **Estructura**: El proyecto sigue una organización por módulos dentro de la carpeta `Modules/`.
- **Autenticación**: Utiliza JWT (JSON Web Tokens). Asegúrate de cambiar la `Jwt:Key` en producción.
