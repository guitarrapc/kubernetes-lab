## base
kubectl kustomize ./base
kubectl kustomize ./base | kubectl apply -f -
kubectl get pod
kubectl get svc
kubectl get deploy
kubectl kustomize ./base | kubectl delete -f -
