#!/bin/bash
kubectl apply -f deployments/rollingupdate-deployment.yaml
kubectl get deploy

kubectl delete -f deployments/rollingupdate-deployment.yaml
