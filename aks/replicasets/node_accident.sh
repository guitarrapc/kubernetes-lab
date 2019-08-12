#!/bin/bash
az group list
az vm list --group MC_AKSCluster_AKSCluster_japaneast
kubectl get node
az vm stop --name aks-nodepool1-35672737-1 -g MC_AKSCluster_AKSCluster_japaneast
kubectl get node
kubectl get pod -o wide
kubectl describe pod nginx-replicaset-5pcqm
kubectl describe pod nginx-replicaset-bcbz9 

az vm start --name aks-nodepool1-35672737-1 -g MC_AKSCluster_AKSCluster_japaneast
