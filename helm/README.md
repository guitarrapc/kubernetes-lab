## README

Output rendered YAML manifests to stdout.

```shell
helm template hello-kubernetes ./helm/hello-kubernetes \
  --namespace hello-kubernetes \
  --set message="You are reaching hello-kubernetes version 1" \
  --set ingress.configured=true \
  --set service.type="ClusterIP"
```

Apply release to cluster.

```shell
helm upgrade --install hello-kubernetes ./helm/hello-kubernetes \
  --namespace hello-kubernetes \
  --set message="You are reaching hello-kubernetes version 1" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```

Confirm access to pod.

```shell
kubectl proxy &
curl 127.0.0.1:8001/api/v1/namespaces/hello-kubernetes/services/hello-kubernetes-hello-kubernetes:80/proxy/
kill -9 $(pgrep kubectl)
```

Delete release from cluster.

```shell
helm uninstall hello-kubernetes --namespace hello-kubernetes
```
