## aws-efs-csi

for efs on k8s.

deploy EFS CSI driver and check daemonset, drivers.

> NOTE: make sure upgrade your `quay.io/k8scsi/livenessprobe` tag to 2.0.0 and above to suppress healthcheck logging.

```shell
helm upgrade --install aws-efs-csi-driver --namespace kube-system https://github.com/kubernetes-sigs/aws-efs-csi-driver/releases/download/v0.3.0/helm-chart.tgz -f ./helm/values.yaml --recreate-pods --force
kubectl get daemonsets efs-csi-node -n kube-system
kubectl get csidrivers.storage.k8s.io

helm delete aws-efs-csi-driver --purge
```

## Persistent Volume and Persistent Volume Claim

after add efs csi, create PV and PVC to use as volume.

```shell
kubectl kustomize ./overlays/development | kubectl apply -f -

kubectl kustomize ./overlays/development | kubectl delete -f -
```

now your can refer `efs-claim` as pvc name.

