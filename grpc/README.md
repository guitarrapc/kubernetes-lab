## Image for gRPC Server

use sample grpc image on repo [guitarrapc/grpc-lab](https://github.com/guitarrapc/gRPC-lab/tree/master/healthcheck)

Images are available at Docker Hub.

> [guitarrapc/grpc-example-server-csharp](https://hub.docker.com/r/guitarrapc/grpc-example-server-csharp)
> [guitarrapc/grpc-example-client-csharp](https://hub.docker.com/r/guitarrapc/grpc-example-client-csharp)

## Run

k8s. server only deployment, without envoy.

```
kubectl kustomize simple/base | kubectl apply -f -
kubectl get svc
kubectl get pod
kubectl get deploy
kubectl kustomize simple/base | kubectl delete -f -
```

k8s. envoy deployment. (namespace: `grpc-lab-nlb`)

> better grpc loadbalancing handle.

```
kubectl kustomize externalnlb-envoy/base | kubectl apply -f -
kubectl get svc -n grpc-lab-nlb
kubectl get pod -n grpc-lab-nlb
kubectl get deploy -n grpc-lab-nlb
kubectl kustomize externalnlb-envoy/base | kubectl delete -f -
```

> development

```
kubectl kustomize externalnlb-envoy/development | kubectl apply -f -
kubectl kustomize externalnlb-envoy/development | kubectl delete -f -
```

## REF

> [Amazon EKSでgRPCサーバを運用する \- 一休\.com Developers Blog](https://user-first.ikyu.co.jp/entry/2019/08/27/093858)
>
> [Using Envoy Proxy to load\-balance gRPC services on GKE  \|  Solutions  \|  Google Cloud](https://cloud.google.com/solutions/exposing-grpc-services-on-gke-using-envoy-proxy)
>
> [GoogleCloudPlatform/grpc\-gke\-nlb\-tutorial: gRPC load\-balancing on GKE using Envoy](https://github.com/GoogleCloudPlatform/grpc-gke-nlb-tutorial)
> 
> [grpc\-ecosystem/grpc\-health\-probe: A command\-line tool to perform health\-checks for gRPC applications in Kubernetes etc\.](https://github.com/grpc-ecosystem/grpc-health-probe/)
>
> [Health checking gRPC servers on Kubernetes \- Kubernetes](https://kubernetes.io/blog/2018/10/01/health-checking-grpc-servers-on-kubernetes/)
>
> EnvoyをServerに常時置いた別の形式: [EnvoyでgRPCをロードバランスする \- PartyIX](https://h3poteto.hatenablog.com/entry/2019/02/18/130500)