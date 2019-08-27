#!/bin/bash
ACR_NAME=sampleGuitarrapcACR
ACR_RES_GROUP=$ACR_NAME
AKS_CLUSTER_NAME=AKSCluster
AKS_RES_GROUP=$AKS_CLUSTER_NAME
SP_NAME=sample-acr-service-principal

az group delete --name $ACR_RES_GROUP
az group delete --name $AKS_RES_GROUP
az ad sp delete --id=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)

