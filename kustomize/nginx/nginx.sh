## base
kubectl kustomize ./base
kubectl kustomize ./base | kubectl apply -f -
kubectl get pod
kubectl get svc
kubectl kustomize ./base | kubectl delete -f -

## overlays
kubectl kustomize ./overlays/develop
kubectl kustomize ./overlays/develop | kubectl apply -f -
kubectl get pod
kubectl get svc
kubectl get deployment

## remove
kubectl kustomize ./overlays/develop | kubectl delete -f -
