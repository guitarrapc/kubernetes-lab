#!/bin/bash
kubectl apply -f pods/pod_resource_limit.yaml
kubectl get pods --output=wide
kubectl describe node aks-nodepool1-35672737-2

kubectl delete -f pods/pod_resource_limit.yaml
