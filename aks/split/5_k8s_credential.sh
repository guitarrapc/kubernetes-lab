#!/bin/bash
AKS_CLUSTER_NAME=AKSCluster
AKS_RES_GROUP=$AKS_CLUSTER_NAME

az aks get-credentials --admin --resource-group $AKS_RES_GROUP --name $AKS_CLUSTER_NAME
# Merged "AKSCluster-admin" as current context in ~/.kube/config
grep client-certificate-data ~/.kube/config | awk '{print $2}' | base64 -D | openssl x509 -text | grep Subject
kubectl get clusterrolebindings
kubectl get clusterrolebindings cluster-admin -o yaml
kubectl get clusterole cluster-admin -o yaml
