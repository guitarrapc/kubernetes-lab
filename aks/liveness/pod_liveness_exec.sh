#!/bin/bash
kubectl apply -f liveness/pod_liveness_exec.yaml
kubectl describe pod liveness-exec
kubectl delete -f liveness/pod_liveness_exec.yaml