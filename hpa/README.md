## tl;dr;

HPA is standard API resource, so you don't need install it. However HPA requires Metrics Server.

## Metrics Server

## Docker Desktop

    **helm chart**

    ```sh
    helm repo add metrics-server https://kubernetes-sigs.github.io/metrics-server/
    helm upgrade --install metrics-server metrics-server/metrics-server --version 3.10.0 -f ./hpa/values-metrics-server-dockerdesktop.yaml -n kube-system --wait
    kubectl get deployment metrics-server -n kube-system
    ```

### EKS

> see: https://docs.aws.amazon.com/ja_jp/eks/latest/userguide/metrics-server.html

    **helm chart**

    ```sh
    helm repo add metrics-server https://kubernetes-sigs.github.io/metrics-server/
    helm upgrade --install metrics-server metrics-server/metrics-server --version 3.10.0 -f ./hpa/values-metrics-server-eks.yaml -n kube-system --wait
    kubectl get deployment metrics-server -n kube-system
    ```

    **Manifest**

    ```sh
    kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml
    kubectl get deployment metrics-server -n kube-system
    ```

### HPA

Run PHP Server.

```sh
kubens default
kubectl apply -f https://k8s.io/examples/application/php-apache.yaml
kubectl autoscale deployment php-apache --cpu-percent=50 --min=1 --max=10
kubectl get hpa
```

Watch PHP Server status

```sh
kubectl get po -w
```

Open new shell and run LoadTest.

```
kubectl run -i --tty load-generator --rm --image=busybox --restart=Never -- /bin/sh -c "while sleep 0.01; do wget -q -O- http://php-apache; done"
```

This makes scale out PHP Server pod. php-apache pod's requests.

```sh
$ k get po -w
NAME                          READY   STATUS    RESTARTS   AGE
load-generator                0/1     Pending       0          0s
load-generator                0/1     Pending       0          0s
load-generator                0/1     ContainerCreating   0          0s
load-generator                1/1     Running             0          4s
php-apache-5b56f9df94-rz5f2   0/1     Pending             0          0s
php-apache-5b56f9df94-2m446   0/1     Pending             0          0s
php-apache-5b56f9df94-mlr7g   0/1     Pending             0          0s
php-apache-5b56f9df94-rz5f2   0/1     Pending             0          0s
php-apache-5b56f9df94-2m446   0/1     Pending             0          0s
php-apache-5b56f9df94-mlr7g   0/1     Pending             0          0s
php-apache-5b56f9df94-rz5f2   0/1     ContainerCreating   0          0s
php-apache-5b56f9df94-mlr7g   0/1     ContainerCreating   0          0s
php-apache-5b56f9df94-2m446   0/1     ContainerCreating   0          0s
php-apache-5b56f9df94-mlr7g   1/1     Running             0          1s
php-apache-5b56f9df94-2m446   1/1     Running             0          2s
php-apache-5b56f9df94-rz5f2   1/1     Running             0          3s
php-apache-5b56f9df94-864nf   0/1     Pending             0          0s
php-apache-5b56f9df94-44f59   0/1     Pending             0          0s
php-apache-5b56f9df94-864nf   0/1     Pending             0          0s
php-apache-5b56f9df94-44f59   0/1     Pending             0          0s
php-apache-5b56f9df94-864nf   0/1     ContainerCreating   0          0s
php-apache-5b56f9df94-44f59   0/1     ContainerCreating   0          0s
php-apache-5b56f9df94-44f59   1/1     Running             0          1s
php-apache-5b56f9df94-864nf   1/1     Running             0          1s

$ k get deploy
NAME         READY   UP-TO-DATE   AVAILABLE   AGE
php-apache   6/6     6            6           3m27s
```

php-apache requests.cpu is `100m` and HPA is set with CPU percent `50%`, therefore hpa will scale pod when cpu utilization reached 100m.

```sh
$ kubectl resource-capacity --util --pods -n default
NODE             POD                           CPU REQUESTS   CPU LIMITS   CPU UTIL    MEMORY REQUESTS   MEMORY LIMITS   MEMORY UTIL

docker-desktop   *                             200m (2%)      500m (6%)    357m (4%)   0Mi (0%)          0Mi (0%)        13Mi (0%)
docker-desktop   load-generator                0Mi (0%)       0Mi (0%)     17m (0%)    0Mi (0%)          0Mi (0%)        1Mi (0%)
docker-desktop   php-apache-5b56f9df94-tgsx8   200m (2%)      500m (6%)    341m (4%)   0Mi (0%)          0Mi (0%)        12Mi (0%)

$ kubectl resource-capacity --util --pods -n default
NODE             POD                           CPU REQUESTS   CPU LIMITS   CPU UTIL    MEMORY REQUESTS   MEMORY LIMITS   MEMORY UTIL

docker-desktop   *                             200m (2%)      500m (6%)    517m (6%)   0Mi (0%)          0Mi (0%)        13Mi (0%)
docker-desktop   load-generator                0Mi (0%)       0Mi (0%)     17m (0%)    0Mi (0%)          0Mi (0%)        1Mi (0%)
docker-desktop   php-apache-5b56f9df94-tgsx8   200m (2%)      500m (6%)    500m (6%)   0Mi (0%)          0Mi (0%)        12Mi (0%)

$ kubectl resource-capacity --util --pods -n default
NODE             POD                           CPU REQUESTS   CPU LIMITS    CPU UTIL    MEMORY REQUESTS   MEMORY LIMITS   MEMORY UTIL

docker-desktop   *                             800m (10%)     2000m (25%)   592m (7%)   0Mi (0%)          0Mi (0%)        47Mi (0%)
docker-desktop   load-generator                0Mi (0%)       0Mi (0%)      19m (0%)    0Mi (0%)          0Mi (0%)        1Mi (0%)
docker-desktop   php-apache-5b56f9df94-2m446   200m (2%)      500m (6%)     158m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-mlr7g   200m (2%)      500m (6%)     127m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-rz5f2   200m (2%)      500m (6%)     151m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-tgsx8   200m (2%)      500m (6%)     139m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)

$ kubectl resource-capacity --util --pods -n default
NODE             POD                           CPU REQUESTS   CPU LIMITS    CPU UTIL    MEMORY REQUESTS   MEMORY LIMITS   MEMORY UTIL
docker-desktop   *                             1200m (15%)    3000m (37%)   604m (7%)   0Mi (0%)          0Mi (0%)        70Mi (0%)
docker-desktop   load-generator                0Mi (0%)       0Mi (0%)      20m (0%)    0Mi (0%)          0Mi (0%)        1Mi (0%)
docker-desktop   php-apache-5b56f9df94-2m446   200m (2%)      500m (6%)     96m (1%)    0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-44f59   200m (2%)      500m (6%)     90m (1%)    0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-864nf   200m (2%)      500m (6%)     97m (1%)    0Mi (0%)          0Mi (0%)        11Mi (0%)
docker-desktop   php-apache-5b56f9df94-mlr7g   200m (2%)      500m (6%)     49m (0%)    0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-rz5f2   200m (2%)      500m (6%)     121m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)
docker-desktop   php-apache-5b56f9df94-tgsx8   200m (2%)      500m (6%)     135m (1%)   0Mi (0%)          0Mi (0%)        12Mi (0%)
```
