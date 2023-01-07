export $(grep -v '^#' ./.env | xargs)
cd ./inti-back/inti-back
dotnet run