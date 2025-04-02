build:
	@make linux

linux-arm64:
	@dotnet publish --self-contained --runtime linux-arm64

linux:
	@dotnet publish --self-contained --runtime linux-x64

osx:
	@dotnet publish --self-contained --runtime osx-arm64

test:
	@dotnet test
