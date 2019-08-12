#!/bin/bash
kubectl apply -f hpa/replicaset_hpa.yaml
kubectl get pod

kubectl apply -f hpa/hpa.yaml
kubectl get pod -w
kubectl top pod

kubectl delete -f hpa/replicaset_hpa.yaml
kubectl delete -f hpa/hpa.yaml

kubectl get pod -n kube-system -o wide