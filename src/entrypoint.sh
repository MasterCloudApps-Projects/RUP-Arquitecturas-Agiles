#!/bin/bash

echo "installing dotnet ef utility..."
until which dotnet-ef || dotnet tool install --global dotnet-ef; do
    sleep 1
done

cd ..
cd ShareThings.Data

echo "Initial Create Database ShareThingsDbContext..." 
until dotnet ef --startup-project ../ShareThings/ migrations add InitialCreateEntryPoint --context ShareThingsDbContext; do
    sleep 1
done

echo "waiting for persisted grant database --> ShareThingsDbContext..."
until dotnet ef --startup-project ../ShareThings/ database update --context ShareThingsDbContext; do
    sleep 1
done

cd ..
cd ShareThings

echo "Initial Create Database ShareThingsIdentityContext..." 
until dotnet ef migrations add InitialCreateEntryPoint --context ShareThingsIdentityContext; do
    sleep 1
done

echo "waiting for persisted grant database --> ShareThingsIdentityContext..."
until dotnet ef -v database update -c ShareThingsIdentityContext; do
    sleep 1
done

echo "starting winged keys..."
dotnet watch run  --context ShareThingsDbContext --urls "https://0.0.0.0:443;http://0.0.0.0:80"  -e ASPNETCORE_Kestrel__Certificates__Default__Password="tfmsharethings1."  -e ASPNETCORE_Kestrel__Certificates__Default__Path="/app/ShareThings/Certs/ShareThings.pfx"