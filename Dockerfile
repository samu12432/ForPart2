# ========================
# 1) Imagen de build
# ========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIG=Release
WORKDIR /src

# Copiamos csproj primero para aprovechar cache
COPY *.sln ./
# Si ten�s varios proyectos/capas, copi� los csproj espec�ficos:
# COPY TuProyecto.WebApi/TuProyecto.WebApi.csproj TuProyecto.WebApi/
# COPY TuProyecto.Core/TuProyecto.Core.csproj TuProyecto.Core/
# ...
# Si tu repo es simple (un solo proyecto), pod�s copiar directo:
COPY . ./

# Restaurar (resuelve NuGet)
RUN dotnet restore

# Compilar + publicar en Release (self-contained opcional)
RUN dotnet publish -c $BUILD_CONFIG -o /app/publish --no-restore

# ========================
# 2) Imagen runtime (m�s liviana)
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Puerto en el contenedor (pod�s cambiarlo)
#ENV ASPNETCORE_URLS=http://+:7241
EXPOSE 7241
EXPOSE 80
EXPOSE 443

# (Opcional) Usuario no root
RUN adduser --disabled-password --gecos "" appuser
USER appuser

# Copiamos lo publicado
COPY --from=build /app/publish ./


# Command
ENTRYPOINT ["dotnet", "ForParts.dll"]
