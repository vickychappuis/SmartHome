dev:
	dotnet watch --project ./smarthome.WebApi

start:
	dotnet run --project ./smarthome.WebApi

migrate:
		rm -rf ./smarthome.DataAccess/Migrations && dotnet ef migrations add SmartHomeMigration --project ./smarthome.DataAccess --startup-project ./smarthome.WebApi

