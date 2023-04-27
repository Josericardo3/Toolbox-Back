FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR inti

EXPOSE 80
EXPOSE 8051
COPY ./*.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /inti
COPY --from=build /inti/out .
ENTRYPOINT ["dotnet","inti-back.dll"]
