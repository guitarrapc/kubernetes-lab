## README

local

```shell
kubectl kustomize ./simple/base/ | kubectl apply -f -
kubectl get pod
kubectl get svc
kubectl get sts
kubectl get pv,pvc
kubectl delete pv/xxxx
kubectl delete pvc/xxxx
kubectl kustomize ./simple/base/ | kubectl delete -f -
```
