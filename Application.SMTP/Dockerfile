FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Application.SMTP/Application.SMTP.csproj", "Application.SMTP/"]
RUN dotnet restore "Application.SMTP/Application.SMTP.csproj"
COPY . .
WORKDIR "/src/Application.SMTP"
RUN dotnet build "Application.SMTP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Application.SMTP.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.SMTP.dll"]
