FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Collections.Api/Collections.Api.csproj", "Collections.Api/"]
RUN dotnet restore "Collections.Api/Collections.Api.csproj"
COPY . .
WORKDIR "/src/Collections.Api"
RUN dotnet build "Collections.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Collections.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Collections.Api.dll"]
