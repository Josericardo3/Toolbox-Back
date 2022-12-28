FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /inti
COPY . .

WORKDIR /inti/inti-back/inti-back
RUN dotnet build
RUN pwd
RUN ls ./bin/Debug/net6.0 -las
CMD ./bin/Debug/net6.0/inti-back 
