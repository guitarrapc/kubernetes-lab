#!/bin/bash
LOCATION=japaneast
AKS_CLUSTER_NAME=AKSCluster
AKS_RES_GROUP=$AKS_CLUSTER_NAME
SP_NAME=sample-acr-service-principal
APP_ID=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)
AKS_VERSION=1.12.8
VM_SIZE=Standard_DS1_v2

az group create --resource-group $AKS_RES_GROUP --location $LOCATION
az aks create \
    --name $AKS_CLUSTER_NAME \
    --resource-group $AKS_RES_GROUP \
    --node-count 3 \
    --kubernetes-version $AKS_VERSION \
    --node-vm-size $VM_SIZE \
    --generate-ssh-keys \
    --service-principal $APP_ID \
    --client-secret $SP_PASSWORD

