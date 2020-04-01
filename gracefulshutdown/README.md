## Graceful shutdown

This show how to gracefully shutdown pods.

## 3rd party graceful shutdown with preStop

In this example, use 3rd party app to handle preStop for graceful shutdown.

nginx default doesn't treat TERM signal, so you will see existing connection will lost when TERM.

```shell
kubectl kustomize ./3rdparty/overlays/noprehook | kubectl apply -f -
```

you can inject your code when TERM is sent to pod with lifecycle.

```shell
kubectl kustomize ./3rdparty/overlays/prehook | kubectl apply -f -
```

however, but TERM signal may come before pod evicted & deresitered from subsystem.
to guaranteed most case, let's wait seconds before starting graceful shutdown.

```shell
kubectl kustomize ./3rdparty/overlays/sleep_prehook | kubectl apply -f -
```

to prevent all pod's evicted while draining node, use PodDisruptionBudget.

```shell
kubectl kustomize ./3rdparty/overlays/pdb_sleepprehook | kubectl apply -f -
```

if you want keep 100% of pods when evicted, then use `100%` for minAvailable.

```shell
kubectl kustomize ./3rdparty/overlays/pdb_sleepprehook_bluegreen | kubectl apply -f -
```
