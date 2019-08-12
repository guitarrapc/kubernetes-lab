#!/bin/bash
kubectl apply -f liveness/pod_liveness_tcp.yaml
kubectl describe pod liveness-tcp
kubectl delete -f liveness/pod_liveness_tcp.yaml