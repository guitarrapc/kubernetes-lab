#!/bin/bash

# readinessProbe
kubectl apply -f deployments/rollingupdate-readiness.yaml
kubectl get deploy

# RollingUpdateStrategy:  10% max unavailable, 30% max surge
kubectl describe deploy rolling-deployment

kubectl delete -f deployments/rollingupdate-readiness.yaml
