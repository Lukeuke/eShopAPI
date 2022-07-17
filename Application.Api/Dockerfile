FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Application.Api/Application.Api.csproj", "Application.Api/"]
RUN dotnet restore "Application.Api/Application.Api.csproj"
COPY . .
WORKDIR "/src/Application.Api"
RUN dotnet build "Application.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Application.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.Api.dll"]
