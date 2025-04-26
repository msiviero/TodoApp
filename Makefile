build:
	@make linux

linux-arm64:
	@cd TodoApp; dotnet publish --self-contained --runtime linux-arm64

linux:
	@cd TodoApp; dotnet publish --self-contained --runtime linux-x64

osx:
	@cd TodoApp; dotnet publish --self-contained --runtime osx-arm64

test:
	@dotnet test

migration:
	cd TodoApp; dotnet ef migrations add migration_$(shell date '+%Y%m%d%H%M%S')

updatedb:
	@cd TodoApp; dotnet ef database update
