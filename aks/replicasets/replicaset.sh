#!/bin/bash
kubectl apply -f replicasets/replicaset.yaml
kubectl get pod --show-labels
kubectl describe rs photoview-rs
kubectl delete -f replicasets/replicaset.yaml
