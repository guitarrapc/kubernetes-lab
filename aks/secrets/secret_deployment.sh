#!/bin/bash
kubectl apply -f secrets/secret_deployment.yaml
kubectl get pod

kubectl exec -it secret-deployment-5fbdd78dc-2r5v7 env | grep SECRET*
# SECRET_ID=dbuser
# SECRET_KEY=aBcD123

kubectl exec -it secret-deployment-5fbdd78dc-2r5v7 ls /etc/secrets
# apl.crt
kubectl exec -it secret-deployment-5fbdd78dc-2r5v7 cat /etc/secrets/apl.crt
# secret
kubectl exec -it secret-deployment-5fbdd78dc-2r5v7 cat /etc/mtab | grep /etc/secrets
# tmpfs /etc/secrets tmpfs ro,relatime 0 0

kubectl delete -f secrets/
