#!/bin/bash
ACR_NAME=sampleGuitarrapcACR
cd oss/chap06/ConfigMap
az acr build --registry $ACR_NAME --image photo-view:v3.0 v3.0/

kubectl apply -f configmap/env_file_deployment.yaml
kubectl get pod
kubectl get svc

kubectl exec -it configmap-deployment-7976496d78-69hm9 /bin/bash
# $ env | grep PROJECT_ID
# PROJECT_ID=hello-k8s

# $ ls /etc/config
# ui.ini
# $ cat /etc/config/ui.ini
# [UI]
# color.top = blue

kubectl delete -f configmap/
