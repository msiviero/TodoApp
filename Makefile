build:
	@make linux

linux:
	@dotnet publish --self-contained --runtime linux-arm64

osx:
	@dotnet publish --self-contained --runtime osx-arm64
