FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/KarasKino.Api/KarasKino.Api.csproj", "src/KarasKino.Api/"]
COPY ["src/KarasKino.Core/KarasKino.Core.csproj", "src/KarasKino.Core/"]
COPY ["src/KarasKino.Application/KarasKino.Application.csproj", "src/KarasKino.Application/"]
COPY ["src/KarasKino.Infrastructure/KarasKino.Infrastructure.csproj", "src/KarasKino.Infrastructure/"]
COPY ["src/KarasKino.ServiceDefaults/KarasKino.ServiceDefaults.csproj", "src/KarasKino.ServiceDefaults/"]
RUN dotnet restore "src/KarasKino.Api/KarasKino.Api.csproj"
COPY . .
WORKDIR "/src/src/KarasKino.Api"
RUN dotnet build "KarasKino.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KarasKino.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KarasKino.Api.dll"]