#!/bin/bash
kubectl get pod
kubectl get deploy
kubectl get hpa

kubectl cluster-info
kubectl get node
kubectl get node -o=wide
kubectl describe node aks-nodepool1-35672737-0

