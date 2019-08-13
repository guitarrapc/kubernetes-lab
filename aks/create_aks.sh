#!/bin/bash

# az login
az login

LOCATION=japaneast
ACR_NAME=sampleGuitarrapcACR

# registry
az acr check-name -n $ACR_NAME
ACR_RES_GROUP=$ACR_NAME
az group create --resource-group $ACR_RES_GROUP --location $LOCATION
az acr create --resource-group $ACR_RES_GROUP --name $ACR_NAME --sku Standard --location $LOCATION

# image on oss folder.
cd ./oss/chap02
az acr build --registry $ACR_NAME --image photo-view:v1.0 v1.0/
az acr build --registry $ACR_NAME --image photo-view:v2.0 v2.0/
cd ../../

az acr repository show-tags -n $ACR_NAME --repository photo-view

# iam
ACR_ID=$(az acr show --name $ACR_NAME --query id --output tsv)
SP_NAME=sample-acr-service-principal

# password can retrvie on create timing or portal
SP_PASSWORD=$(az ad sp create-for-rbac --name $SP_NAME --role READER --scopes $ACR_ID --query password --output tsv)
APP_ID=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)

echo $APP_ID
echo $SP_PASSWORD

# create k8s cluster
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


# k8s credential
az aks get-credentials --admin --resource-group $AKS_RES_GROUP --name $AKS_CLUSTER_NAME
# Merged "AKSCluster-admin" as current context in ~/.kube/config
# macos
grep client-certificate-data ~/.kube/config | awk '{print $2}' | base64 -D | openssl x509 -text | grep Subject
# ubuntu
grep client-certificate-data ~/.kube/config | awk '{print $2}' | base64 -d | openssl x509 -text | grep Subject
kubectl get clusterrolebindings
kubectl get clusterrolebindings cluster-admin -o yaml
kubectl get clusterole cluster-admin -o yaml

# kubectl tutorial
kubectl get pod
kubectl get deploy
kubectl get hpa

kubectl cluster-info
kubectl get node
kubectl get node -o=wide
kubectl describe node aks-nodepool1-35672737-0

# enable auto completion
source <(kubectl completion bash)
echo "source <(kubectl completion bash)" >> ~/.bashrc
alias k=kubectl
complete -F __start_kubectl k

# show config
kubectl config view

# clean up
az group delete -name $ACR_RES_GROUP
az group delete -name $AKS_RES_GROUP
az ad sp delete -id=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)

# switch context
kubectl config get-contexts
kubectl config current-context
kubectl config use-context docker-desktop
kubectl config use-context AKSCluster-admin