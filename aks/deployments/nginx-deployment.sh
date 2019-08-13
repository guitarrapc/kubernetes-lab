#!/bin/bash
kubectl apply -f deployments/nginx-deployment.yaml
kubectl get pod
kubectl get deploy
kubectl get deploy,rs,pod

# naming rule
# name + -deploy name + -replica name + -pod name

kubectl describe deploy nginx-deployment
# OldReplicaSets: old deploy histories
# NewReplicaSet: this deploy

# update nginx version from 1.14 -> 1.15
kubectl apply -f deployments/nginx-deployment.yaml

# check OldReplicaSets and deploy status
kubectl get deploy
kubectl describe deploy nginx-deployment

# replica set detail
kubectl get replicaset --output=wide

kubectl delete -f deployments/nginx-deployment.yaml
kubectl get deploy,rs,pod
