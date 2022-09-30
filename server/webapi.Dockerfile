# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY src ./
RUN dotnet restore ./Searchle.GraphQL/Searchle.GraphQL.csproj
RUN dotnet publish ./Searchle.GraphQL/Searchle.GraphQL.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Searchle.GraphQL.dll"]
