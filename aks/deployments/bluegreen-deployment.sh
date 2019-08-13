#!/bin/bash
kubectl apply -f deployments/blue-deployment.yaml
kubectl apply -f deployments/green-deployment.yaml

kubectl get pod

# lb -> direction to v1.0 = blue
kubectl apply -f deployments/bluegreen-service.yaml
kubectl get svc

# deploy v2.0 = green
kubectl apply -f deployments/bluegreen-service.yaml
kubectl get svc

kubectl delete -f deployments/
