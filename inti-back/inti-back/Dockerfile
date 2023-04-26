FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR inti

EXPOSE 80
EXPOSE 8051
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/certificates/F1D87D0BDA00F9FE285A1A521ED8C1B93134208D.pfx
COPY ./*.csproj ./
COPY "./F1D87D0BDA00F9FE285A1A521ED8C1B93134208D.pfx" "/certificates/"
RUN dotnet restore

COPY . .
#RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /inti
COPY --from=build /inti/out .
ENTRYPOINT ["dotnet","inti-back.dll"]
