#!/bin/bash
ACR_ID=$(az acr show --name $ACR_NAME --query id --output tsv)
SP_NAME=sample-acr-service-principal

# password can retrvie on create timing or portal
SP_PASSWORD=$(az ad sp create-for-rbac --name $SP_NAME --role READER --scopes $ACR_ID --query password --output tsv)
APP_ID=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)

echo $APP_ID
echo $SP_PASSWORD
