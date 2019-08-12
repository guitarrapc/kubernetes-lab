#!/bin/bash
kubectl apply -f pods/pod_request.yaml
kubectl get pods --output wide
kubectl describe node aks-nodepool1-35672737-2

# over capacity pod will not scheduled
kubectl apply -f pods/pod_request_overcap.yaml
kubectl describe pod requests-pod
kubectl delete -f pods/pod_request_overcap.yaml
