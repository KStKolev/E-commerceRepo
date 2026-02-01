FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["E-commerceApplication/E-commerceApplication.csproj", "E-commerceApplication/"]
COPY ["E-commerceApplication.Business/E-commerceApplication.Business.csproj", "E-commerceApplication.Business/"]
COPY ["E-commerceApplication.DAL/E-commerceApplication.DAL.csproj", "E-commerceApplication.DAL/"]
RUN dotnet restore "./E-commerceApplication/E-commerceApplication.csproj"
COPY . .
WORKDIR "/src/E-commerceApplication"
RUN dotnet build "./E-commerceApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./E-commerceApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-commerceApplication.dll"]