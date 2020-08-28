## TL;DR

Prometheus + Grafana basics.

## SAMPLE

**TIPS: add Grafana dashboard json**

* add Grafana dashboard definition json to `./base/`.
* run following script will generate configmap for each json.

```shell
./base/gen-configmap-dashboards.sh
```

* add configmap to kustomization.yaml.

```shell
code ./base/kustomization.yaml
```

**namespace & ingress & configmap**

add namespace & ingress & configmap to show dashboards.

```shell
kubectl kustomize ./overlays/development/ | kubectl apply -f -
```

**Prometheus**

> REF: https://eksworkshop.com/monitoring/deploy-prometheus/

```shell
helm upgrade --install prometheus --namespace monitoring -f ./helm/development/prometheus-values.yaml stable/prometheus
```

check

```shell
kubectl get deploy -n monitoring
kubectl rollout status deploy prometheus-alertmanager -n monitoring
kubectl rollout status deploy prometheus-kube-state-metrics -n monitoring
kubectl rollout status deploy prometheus-server -n monitoring
```

> port forward するなら(基本不要): `kubectl port-forward -n monitoring deploy/prometheus-server 8080:9090`

**Grafana**

> * REF: https://eksworkshop.com/monitoring/deploy-grafana/
> * REF: [How to Monitor Your Kubernetes Cluster With Prometheus and Grafana](https://blog.container-solutions.com/how-to-monitor-your-kubernetes-cluster-with-prometheus-and-grafana)

Use ingress for Dashboards. Including prometheus-server, prometheus-alertmanager and grafana

```shell
helm upgrade --install grafana --namespace monitoring -f ./helm/development/grafana-values.yaml stable/grafana
```

check

```shell
kubectl get ingress -n monitoring
kubectl get secret --namespace monitoring grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo
kubectl rollout status deploy grafana -n monitoring
```

**clean up**

```shell
helm uninstall grafana
helm uninstall prometheus
kubectl kustomize ./overlays/development/ | kubectl delete -f -
```

## BASIC

Helm

> * Grafana: [charts/stable/grafana at master · helm/charts](https://github.com/helm/charts/tree/master/stable/grafana)
    > * Charts: [grafana 7\.1\.1 for Kubernetes \| Helm Hub \| Monocular](https://hub.helm.sh/charts/stable/grafana)
> * Prometheus: [charts/stable/prometheus at master · helm/charts](https://github.com/helm/charts/tree/master/stable/prometheus)
    > * Charts: [prometheus 2\.20\.1 for Kubernetes \| Helm Hub \| Monocular](https://hub.helm.sh/charts/stable/prometheus)

use side car and import dashboard.

> * [How to Monitor Your Kubernetes Cluster With Prometheus and Grafana](https://blog.container-solutions.com/how-to-monitor-your-kubernetes-cluster-with-prometheus-and-grafana)

> * [kubernetes helm \- stable/prometheus\-operator \- adding persistent grafana dashboards \- Stack Overflow](https://stackoverflow.com/questions/57322022/stable-prometheus-operator-adding-persistent-grafana-dashboards)

> * [notes/Grafana at master · tretos53/notes](https://github.com/tretos53/notes/tree/master/Grafana)

Totally good, but no sidecar pattern. Direct config as json.

* [Argo CD と Helm で構築する Grafana/Prometheus 環境 \#Zaim｜autumn](https://blog.zaim.co.jp/n/n9c0ab77fb593)

## Dashboard Samples

Grafana Dashboards

> * [Grafana Dashboards \- discover and share dashboards for Grafana\. \| Grafana Labs](https://grafana.com/grafana/dashboards?direction=desc&orderBy=reviewsCount&search=kubernetes&dataSource=prometheus)

I don't like this style of like a thief....

> * [【暫定版】 Kubernetesの性能監視で必要なメトリクス一覧とPrometheusでのHowTo \- kashionki38 blog](https://kashionki38.hatenablog.com/entry/2020/08/06/011420)
> * [prometheus\-sample\-yaml/grafana at master · kashinoki38/prometheus\-sample\-yaml](https://github.com/kashinoki38/prometheus-sample-yaml/tree/master/grafana)


## Plugins

> * [Grafana Plugins \- extend and customize your Grafana\. \| Grafana Labs](https://grafana.com/grafana/plugins)

## TIPS

Kubernetes 1.16 changes several cAdvisor keys.

* `pod_name` -> `pod`
* `container_name` -> `container`
* `xxxx_size` -> `xxxx_size_bytes`

> [No metrics for pods in Grafana, targets missing in prometheus · Issue \#188 · prometheus\-operator/kube\-prometheus](https://github.com/prometheus-operator/kube-prometheus/issues/188)