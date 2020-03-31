## Graceful shutdown

This show how to gracefully shutdown pods.

## 3rd party graceful shutdown with preStop

In this example, use 3rd party app to handle preStop for graceful shutdown.

nginx default doesn't treat TERM signal, so you will see existing connection will lost when TERM.

```shell
kubectl kustomize ./3rdparty/overlays/noprehook | kubectl apply -f -
```



```shell
kubectl kustomize ./3rdparty/overlays/prehook | kubectl apply -f -
```
