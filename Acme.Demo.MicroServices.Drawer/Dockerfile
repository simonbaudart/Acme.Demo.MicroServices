FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY / ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish Acme.Demo.MicroServices.Drawer -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Acme.Demo.MicroServices.Drawer.dll"]
