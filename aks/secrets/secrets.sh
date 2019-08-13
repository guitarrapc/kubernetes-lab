#!/bin/bash
kubectl apply -f secrets/secrets.yaml
kubectl get secret
kubectl describe secrets api-key

# create secret from file
kubectl create secret generic apl-auth --from-file=secrets/key
kubectl get secret
