#!/bin/bash
# launch as 3
kubectl apply -f hpa/pod_scale.yaml
kubectl get pod
# manual scale to 8
kubectl scale --replicas=8 rs/nginx-replicaset
kubectl get pod

kubectl delete -f hpa/pod_scale.yaml
