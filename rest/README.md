## README

Call Kubernetes REST API from in-cluster pod.

## Access to specific namespace via Role and RoleBinding

Role and RoleBinding is namespace specific permission.

Role/RoleBinding can handle access to the Resource Path, but cannot allo to non-resource api.

```
# resource path role/rolebinding can allow.
/apis/{api-group}/{version}/namespaces
/api/{version}/namespaces
/api/{version}/namespaces/{namespace}
/api/{version}/namespaces/{namespace}/{resource}
/api/{version}/namespaces/{namespace}/{resource}/{resourceName}
/api/v1/namespaces/default/pods
```

let's try REST API with Role/RoleBinding.

```
kubectl kustomize ./role | kubectl apply -f -
kubectl kustomize ./role | kubectl delete -f -
```

call api server via rest api within pod.

```
kubectl exec -it my-app -- apk add curl
kubectl exec -it my-app -- /bin/sh

curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/api/v1/namespaces/default/pods
```

## Access to non-resource/namespace-wide via ClusterRole and ClusterRoleBinding

ClusterRole and ClusterRoleBinding is non-namespace specific, non-resource specific permission.

ClusterRole/ClusterRoleBinding can handle access to the Non-Resource Path.

```
# resource path role/rolebinding can allow.
/api/{version}/{resource}
/api/{version}/{resource}/{resourceName}
/api/v1/pods
```

let's try REST API with Role/RoleBinding.

```
kubectl kustomize ./clusterrole | kubectl apply -f -
kubectl kustomize ./clusterrole | kubectl delete -f -
```

call api server via rest api within pod.

```
kubectl exec -it my-app -- apk add curl
kubectl exec -it my-app -- /bin/sh


curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/api/v1/pods
```

## Agones

let's try REST API to call agones.

```
kubectl kustomize ./agonesrole | kubectl apply -f -
kubectl kustomize ./agonesrole | kubectl delete -f -
```

```
kubectl exec -it my-app -- apk add curl
kubectl exec -it my-app -- /bin/sh

curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/apis/agones.dev/v1/gameservers/
curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/apis/agones.dev/v1/gameservers/magiconion-chatserver-gfrbd
curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/apis/agones.dev/v1/namespace/default/gameservers/magiconion-chatserver-gfrbd
```


```
kubectl exec -it my-app -- kubectl get fleet
kubectl exec -it my-app -- kubectl scale fleet simple-udp --replicas=0
```


```
curl -v --cacert /var/run/secrets/kubernetes.io/serviceaccount/ca.crt -H "Authorization: Bearer $(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" https://kubernetes.default.svc/api/v1/pods
```

agones-sdk cluster role.

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  annotations:
  labels:
    app: agones
    chart: agones-1.0.0
    heritage: Tiller
    release: agones-manual
  name: agones-sdk
rules:
- apiGroups:
  - ""
  resources:
  - events
  verbs:
  - create
- apiGroups:
  - agones.dev
  resources:
  - gameservers
  verbs:
  - list
  - update
  - watch
```

## REF

> [Kubernetes: RBACの設定におけるAPIリソース \- Qiita](https://qiita.com/tkusumi/items/300c566a74b6b64e7e89)
