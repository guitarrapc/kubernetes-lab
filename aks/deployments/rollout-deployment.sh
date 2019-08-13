#!/bin/bash
kubectl apply -f deployments/rollout-deployment.yaml
kubectl rollout status deploy rollout-deployment
kubectl get pod
kubectl get svc

# automatically added revision for deployment
# Annotations:            deployment.kubernetes.io/revision: 1

# rolling update status
# Selector:               app=photo-view
# Replicas:               3 desired | 3 updated | 3 total | 3 available | 0 unavailable
# StrategyType:           RollingUpdate
# MinReadySeconds:        0
# RollingUpdateStrategy:  25% max unavailable, 25% max surge

# deploy status
# Conditions:
#   Type           Status  Reason
#   ----           ------  ------
#   Available      True    MinimumReplicasAvailable
#   Progressing    True    NewReplicaSetAvailable
# OldReplicaSets:  <none>
# NewReplicaSet:   rollout-deployment-68464c9cb (3/3 replicas created)
# Events:
#   Type    Reason             Age    From                   Message
#   ----    ------             ----   ----                   -------
#   Normal  ScalingReplicaSet  2m54s  deployment-controller  Scaled up replica set rollout-deployment-68464c9cb to 3
kubectl describe deploy rollout-deployment


# update to v2.0
kubectl apply -f deployments/rollout-deployment.yaml
kubectl get deploy
kubectl get rs
kubectl describe deploy rollout-deployment

# rollback to v1.0
kubectl apply -f deployments/rollout-deployment.yaml
kubectl describe deploy rollout-deployment
