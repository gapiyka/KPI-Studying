FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:3.1-alpine AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "test", "labs-1-2-gapiyka-tests/labs-1-2-gapiyka-tests.csproj"]