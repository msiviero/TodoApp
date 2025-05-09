 FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
 WORKDIR /TodoApp

COPY . ./ 

RUN dotnet restore
RUN dotnet publish TodoApp --self-contained  -o app

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
WORKDIR /TodoApp
COPY --from=build /TodoApp/app .
ENTRYPOINT ["./TodoApp"]
