FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Oibi.Demo/Oibi.Demo.csproj", "Oibi.Demo/"]
RUN dotnet restore "Oibi.Demo/Oibi.Demo.csproj"
COPY . .
WORKDIR "/src/Oibi.Demo"
RUN dotnet build "Oibi.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Oibi.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oibi.Demo.dll"]