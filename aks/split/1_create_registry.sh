#!/bin/bash
LOCATION=japaneast
ACR_NAME=sampleGuitarrapcACR

az acr check-name -n $ACR_NAME

ACR_RES_GROUP=$ACR_NAME
az group create --resource-group $ACR_RES_GROUP --location $LOCATION
az acr create --resource-group $ACR_RES_GROUP --name $ACR_NAME --sku Standard --location $LOCATION
