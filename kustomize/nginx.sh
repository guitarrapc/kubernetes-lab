## base
kubectl kustomize ./nginx/base
kubectl kustomize ./nginx/base | kubectl apply -f -
kubectl get pod
kubectl get svc

## overlays
kubectl kustomize ./nginx/overlays/develop
kubectl kustomize ./nginx/overlays/develop | kubectl apply -f -
kubectl get pod
kubectl get svc
kubectl get deployment

## remove
kubectl kustomize ./nginx/overlays/develop | kubectl delete -f -
