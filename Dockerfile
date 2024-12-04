# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["smarthome.WebApi/smarthome.WebApi.csproj", "smarthome.WebApi/"]
COPY ["smarthome.ServiceInjector/smarthome.ServiceInjector.csproj", "smarthome.ServiceInjector/"]
COPY ["smarthome.BusinessLogic/smarthome.BusinessLogic.csproj", "smarthome.BusinessLogic/"]
COPY ["smarthome.DataAccess/smarthome.DataAccess.csproj", "smarthome.DataAccess/"]
COPY ["smarthome.Domain/smarthome.Domain.csproj", "smarthome.Domain/"]
COPY ["smarthome.Dtos/smarthome.Dtos.csproj", "smarthome.Dtos/"]
COPY ["smarthome.IBusinessLogic/smarthome.IBusinessLogic.csproj", "smarthome.IBusinessLogic/"]
COPY ["smarthome.IDataAccess/smarthome.IDataAccess.csproj", "smarthome.IDataAccess/"]
RUN dotnet restore "smarthome.WebApi/smarthome.WebApi.csproj"

# Copy all source files and build
COPY . .
RUN dotnet build "smarthome.WebApi/smarthome.WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "smarthome.WebApi/smarthome.WebApi.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set ASP.NET Core URLs explicitly
ENV ASPNETCORE_URLS=http://+:1234
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080
EXPOSE 443

ENTRYPOINT ["dotnet", "smarthome.WebApi.dll"]
