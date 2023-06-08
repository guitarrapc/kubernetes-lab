## README

Before runnning this sample, install Metrics-Server. see [hpa/README.md](../README.md).

Customized HPA behavior based on Memory.

## Deploy HPA

Run Server with cli or yaml manifest.

yaml manifest.

```sh
kubens default
kubectl apply -f hpa/behavior-memory/memoryleak.yaml
kubectl apply -f hpa/behavior-memory/memoryleak-hpa.yaml
kubectl get hpa
```

## Try HPA with LoadTesting

Before start loadtesting, open a new shell and watch Server status.

```sh
kubectl get po -w
```

Open a new shell and watch HPA status.

```sh
kubectl get hpa -w
```

Open new shell and run LoadTest. This makes scale out memoryleak pod.

```sh
$ kubectl run -i --tty load-generator --rm --image=alpine/bombardier:v1.2.5 --restart=Never -- -c 5 -d 210s http://envoy/alloc_str?size=1024

If you don't see a command prompt, try pressing enter.

[====================================================================================================================================================================================] 3m30sDone!
Statistics        Avg      Stdev        Max
  Reqs/sec      1019.59     796.86    3754.22
  Latency        4.90ms     9.60ms    79.08ms
  HTTP codes:
    1xx - 0, 2xx - 214198, 3xx - 0, 4xx - 0, 5xx - 0
    others - 0
  Throughput:     9.62MB/s
pod "load-generator" deleted
```

## HPA Result

memoryleak requests.cpu is `200m` and HPA is set with CPU percent `60%`, therefore hpa will scale pod when cpu utilization reached 120m.

```sh
$ k get hpa -w
NAME      REFERENCE            TARGETS   MINPODS   MAXPODS   REPLICAS   AGE
memoryleak   Deployment/memoryleak   40%/30%   1         12        1          26m
memoryleak   Deployment/memoryleak   40%/30%   1         12        1          27m
memoryleak   Deployment/memoryleak   41%/30%   1         12        1          28m
memoryleak   Deployment/memoryleak   42%/30%   1         12        2          29m
memoryleak   Deployment/memoryleak   39%/30%   1         12        3          30m
```

```sh
$ kubectl resource-capacity --util --pods -n default
NODE             POD                           CPU REQUESTS   CPU LIMITS   CPU UTIL    MEMORY REQUESTS   MEMORY LIMITS   MEMORY UTIL

docker-desktop   *                             900m (11%)     300m (3%)    755m (9%)   640Mi (2%)        640Mi (2%)      184Mi (0%)
docker-desktop   envoy-7d4864df89-nkssj        300m (3%)      300m (3%)    300m (3%)   256Mi (0%)        256Mi (0%)      29Mi (0%)
docker-desktop   load-generator                0m (0%)        0m (0%)      138m (1%)   0Mi (0%)          0Mi (0%)        4Mi (0%)
docker-desktop   memoryleak-8657d8f9b5-5z96z   200m (2%)      0m (0%)      108m (1%)   128Mi (0%)        128Mi (0%)      41Mi (0%)
docker-desktop   memoryleak-8657d8f9b5-w7qsd   200m (2%)      0m (0%)      106m (1%)   128Mi (0%)        128Mi (0%)      53Mi (0%)
docker-desktop   memoryleak-8657d8f9b5-z9ds7   200m (2%)      0m (0%)      105m (1%)   128Mi (0%)        128Mi (0%)      59Mi (0%)
```

## Clean up

Clean up resources.

```sh
kubectl delete -f hpa/behavior-memory/memoryleak-hpa.yaml
kubectl delete -f hpa/behavior-memory/memoryleak.yaml
```
