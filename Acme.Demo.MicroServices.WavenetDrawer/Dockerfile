FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Acme.Demo.MicroServices.WavenetDrawer/Acme.Demo.MicroServices.WavenetDrawer.csproj", "Acme.Demo.MicroServices.WavenetDrawer/"]
RUN dotnet restore "Acme.Demo.MicroServices.WavenetDrawer/Acme.Demo.MicroServices.WavenetDrawer.csproj"
COPY . .
WORKDIR "/src/Acme.Demo.MicroServices.WavenetDrawer"
RUN dotnet build "Acme.Demo.MicroServices.WavenetDrawer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Acme.Demo.MicroServices.WavenetDrawer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acme.Demo.MicroServices.WavenetDrawer.dll"]
