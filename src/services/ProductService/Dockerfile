FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5030

ENV ASPNETCORE_URLS=http://+:5030

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/ProductService/ProductService.csproj", "src/services/ProductService/"]
COPY ["src/Commun/Events/Events.csproj", "src/Commun/Events/"]
RUN dotnet restore "src/services/ProductService/ProductService.csproj"
COPY . .
WORKDIR "/src/src/services/ProductService"
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]
