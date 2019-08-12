#!/bin/bash
kubectl apply -f pods/pod_resource_limit_overcap.yaml
kubectl get pods --output=wide
kubectl describe pod limits-pod

kubectl delete -f pods/pod_resource_limit_overcap.yaml