FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine@sha256:8c3b9a9184dd1e64bb1dac86794df4ad28f6b8be75baa7233d8949b9f7b5202e AS build
WORKDIR /TodoApp

COPY . ./ 

RUN dotnet restore
RUN dotnet publish TodoApp --self-contained  -o app

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine@sha256:f6c59d378c5ad495eab4c1d175c38c714722549ece6b71945faf7bf6c2c33a92
WORKDIR /TodoApp
COPY --from=build /TodoApp/app .
RUN apk add --update \
    curl \
    && rm -rf /var/cache/apk/*
ENTRYPOINT ["./TodoApp"]
