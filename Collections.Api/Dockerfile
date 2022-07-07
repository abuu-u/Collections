FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Collections.Api/Collections.Api.csproj", "Collections.Api/"]
RUN dotnet restore "Collections.Api/Collections.Api.csproj"
COPY . .
WORKDIR "/src/Collections.Api"
RUN dotnet build "Collections.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Collections.Api.csproj" -c Release -o /app/publish \
    --runtime alpine-$(uname -m | sed s/aarch64/arm64/ | sed s/x86_64/x64/) \
    --self-contained true \
    /p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./Collections.Api"]
