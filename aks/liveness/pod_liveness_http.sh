#!/bin/bash
kubectl apply -f liveness/pod_liveness_http.yaml
kubectl describe pod liveness-http
kubectl delete -f liveness/pod_liveness_http.yaml