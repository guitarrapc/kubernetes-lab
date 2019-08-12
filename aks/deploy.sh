#!/bin/bash
kubectl get node
kubectl get pod

# deploy application
kubectl apply -f tutorial-deployment.yaml
kubectl get pod -o wide

# deploy service (public accessible)
kubectl apply -f tutorial-service.yaml

# confirm external ip
kubectl get svc
chrome <external_ip>

kubectl describe pods photoview-deployment-7cbc745bdf-ckphk

# NOTE:
# lebles: pod-template-hash to get template version
# containers: xxxx-container: Image to get container image

# clean up 
kubectl delete -f deployments/tutorial-service.yaml
kubectl delete -f deployments/tutorial-deployment.yaml

