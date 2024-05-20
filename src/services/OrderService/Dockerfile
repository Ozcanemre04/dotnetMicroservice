FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5138

ENV ASPNETCORE_URLS=http://+:5138

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/OrderService/OrderService.csproj", "src/services/OrderService/"]
COPY ["src/Commun/Events/Events.csproj", "src/Commun/Events/"]
RUN dotnet restore "src/services/OrderService/OrderService.csproj"
COPY . .
WORKDIR "/src/src/services/OrderService"
RUN dotnet build "OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]
