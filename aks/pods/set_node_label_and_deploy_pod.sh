#!/bin/bash
kubectl label node aks-nodepool1-35672737-2 server=webap

kubectl create -f pods/nodeselector_pod.yaml 
kubectl delete -f pods/nodeselector_pod.yaml

# remove label
kubectl label nodes aks-nodepool1-35672737-2 server-