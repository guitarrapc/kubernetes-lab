## base
kubectl kustomize ./nginx/base
kubectl kustomize ./nginx/base | kubectl apply -f -
kubectl get pod
kubectl get svc

## overlays
kubectl kustomize ./nginx/overlays/dev
kubectl kustomize ./nginx/overlays/dev | kubectl apply -f -
kubectl get pod
kubectl get svc
