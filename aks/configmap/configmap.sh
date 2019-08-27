#!/bin/bash
kubectl apply -f configmap/configmap.yaml
kubectl get configmap
kubectl describe configmap project-config

# create config map
kubectl create configmap app-config --from-file=configmap/config/
kubectl get configmap
