# README

# Blue/Green Deployment on Same Namespace

## same-namespace/alb-weighted-tg

Blue/Green Deployment on same namespace with ALB Weighted TargetGroup.

```mermaid
---
title: Blue/Green Deployment on Same Namespace Architecture (same-namespace/alb-weighted-tg)
---

flowchart LR

Internet
subgraph AWS
    ALB["ALB"]
    ALBListener["ALB Listener"]
    ALBTGBlue["ALB TargetGroup (Blue)"]
    ALBTGGreen["ALB TargetGroup (Green)"]
end

subgraph kubernetes
    subgraph "kube-system ns"
        AWSLoadBalanerController["AWS LoadBalancer Controller"]
    end
    subgraph "hello-kubernetes ns"
        ingress["ingress"]
        subgraph blue
            svc-v1["service-v1"]
            deploy-v1["deploy-v1"]
        end
        subgraph green
            svc-v2["service-v2"]
            deploy-v2["deploy-v2"]
        end
    end
end

Internet --> ALB
ALB --> ALBListener
ALBListener --> ALBTGBlue
ALBListener -.-> ALBTGGreen

AWSLoadBalanerController -."1.watch".-> ingress
AWSLoadBalanerController -."2.Handle".-> AWS
ALBTGBlue --"100%"--> blue
ALBTGGreen -."0%".-> green

ingress -.-> svc-v1
ingress -.-> svc-v2
svc-v1 --> deploy-v1
svc-v2 --> deploy-v2
```

### Getting Started

Deploy Apps.

```shell
helm upgrade --install v1 ./helm/hello-kubernetes \
  --namespace hello-kubernetes \
  --set message="You are reaching hello-kubernetes version 1" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```

```shell
helm upgrade --install v2 ./helm/hello-kubernetes \
  --namespace hello-kubernetes \
  --set message="You are reaching hello-kubernetes version 2" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```

**Blue deployment**

Deploy ingress and create ALB.

```shell
kubectl kustomize ./blue-green/same-namespace/alb-weighted-tg/ingress/overlays/v1 | kubectl apply -f -
```

Access to ALB, make sure all trafics are routed to v1.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
```

![image](https://github.com/guitarrapc/kubernetes-lab/assets/3856350/8c77d4ce-bf48-46be-95e7-14aa463bcd33)

**Green deployment**

Change Ingress route to v2.

```shell
kubectl kustomize ./blue-green/same-namespace/alb-weighted-tg/ingress/overlays/v2 | kubectl apply -f -
```

Access to ALB, make sure all trafics are routed to v2.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
```

![image](https://github.com/guitarrapc/kubernetes-lab/assets/3856350/13f304c4-e3eb-4f42-84c8-290cc5a629f5)


### Canaly deployment

Change Ingress route to use canaly, let's route 90％ traffic to v1 and 10％ to green.

```shell
kubectl kustomize ./blue-green/same-namespace/alb-weighted-tg/ingress/overlays/canaly | kubectl apply -f -
```

Access to ALB, make sure trafiic is routed to v1 and v2 for 90:10.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
```

### Clean up

Delete all resources.

```shell
kubectl kustomize ./blue-green/same-namespace/alb-weighted-tg/ingress/overlays/v1 | kubectl delete -f -
helm uninstall v1 --namespace hello-kubernetes
helm uninstall v2 --namespace hello-kubernetes
kubectl delete ns hello-kubernetes
```


# Blue/Green Deployment on Each Namespace

## each-namespace/alb-weighted-tg

Blue/Green Deployment on different namespace with ALB Weighted TargetGroup.

```mermaid
---
title: Blue/Green Deployment on Each Namespace Architecture (each-namespace/alb-weighted-tg)
---

flowchart TD

Internet
subgraph AWS
    ALB["ALB"]
    ALBListener["ALB Listener"]
    ALBTGBlue["ALB TargetGroup (Blue)"]
    ALBTGGreen["ALB TargetGroup (Green)"]
end

subgraph kubernetes
    subgraph "kube-system ns"
        AWSLoadBalanerController["AWS LoadBalancer Controller"]
    end
    subgraph "hello-kubernetes-share ns"
        ingress["ingress"]
        svc-envoy["service-envoy"]
        deploy-envoy["envoy"]
        svc-external-blue["service-externalname-blue"]
        svc-external-green["service-externalname-green"]
    end
    subgraph "hello-kubernetes-blue ns"
        svc-v1["service-v1"]
        deploy-v1["deploy-v1"]
    end
    subgraph "hello-kubernetes-green ns"
        svc-v2["service-v2"]
        deploy-v2["deploy-v2"]
    end
end

Internet --> ALB
ALB --> ALBListener
ALBListener --> ALBTGBlue
ALBListener -.-> ALBTGGreen

AWSLoadBalanerController -."1.watch".-> ingress
AWSLoadBalanerController -."2.Handle".-> AWS
ALBTGBlue --"100%"--> deploy-envoy
ALBTGGreen -."0%".-> deploy-envoy

ingress -.-> svc-envoy
svc-envoy -."8080"..-> deploy-envoy
svc-envoy -."8081"..-> deploy-envoy
deploy-envoy --> svc-external-blue
deploy-envoy -.-> svc-external-green

svc-external-blue ---> svc-v1
svc-external-green ---> svc-v2
svc-v1 --> deploy-v1
svc-v2 --> deploy-v2
```

> [!TIPS]
> OTHER IDEA. We can split envoy to blue/green. However this has to handle HPA and another envoy. If you don't use HPA, then this can control envoy pod replicas.

```mermaid
---
title: Blue/Green Deployment on Each Namespace Architecture 2 (each-namespace/alb-weighted-tg)
---

flowchart TD

Internet
subgraph AWS
    ALB["ALB"]
    ALBListener["ALB Listener"]
    ALBTGBlue["ALB TargetGroup (Blue)"]
    ALBTGGreen["ALB TargetGroup (Green)"]
end

subgraph kubernetes
    subgraph "kube-system ns"
        AWSLoadBalanerController["AWS LoadBalancer Controller"]
    end
    subgraph "hello-kubernetes-share ns"
        ingress["ingress"]
        svc-envoy["service-envoy"]
        deploy-envoy-blue["envoy-blue"]
        deploy-envoy-green["envoy-green"]
        svc-external-blue["service-externalname-blue"]
        svc-external-green["service-externalname-green"]
    end
    subgraph "hello-kubernetes-blue ns"
        svc-v1["service-v1"]
        deploy-v1["deploy-v1"]
    end
    subgraph "hello-kubernetes-green ns"
        svc-v2["service-v2"]
        deploy-v2["deploy-v2"]
    end
end

Internet --> ALB
ALB --> ALBListener
ALBListener --> ALBTGBlue
ALBListener -.-> ALBTGGreen

AWSLoadBalanerController -."1.watch".-> ingress
AWSLoadBalanerController -."2.Handle".-> AWS
ALBTGBlue --"100%"--> deploy-envoy-blue
ALBTGGreen -."0%".-> deploy-envoy-green

ingress -.-> svc-envoy
svc-envoy -."8080"..-> deploy-envoy-blue
svc-envoy -."8081"..-> deploy-envoy-green
deploy-envoy-blue --> svc-external-blue
deploy-envoy-green -.-> svc-external-green

svc-external-blue ---> svc-v1
svc-external-green ---> svc-v2
svc-v1 --> deploy-v1
svc-v2 --> deploy-v2
```


### Getting Started

Deploy Apps.

```shell
helm upgrade --install v1 ./helm/hello-kubernetes \
  --namespace hello-kubernetes-blue \
  --set message="You are reaching hello-kubernetes version 1" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```

```shell
helm upgrade --install v2 ./helm/hello-kubernetes \
  --namespace hello-kubernetes-green \
  --set message="You are reaching hello-kubernetes version 2" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```


**Blue deployment**

Deploy ingress and create ALB.

```shell
kubectl kustomize ./blue-green/each-namespace/alb-weighted-tg/ingress/overlays/v1 | kubectl apply -f -
```

Access to ALB, make sure all trafics are routed to v1.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes-share -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
```

**Green deployment**

Change Ingress route to v2.

```shell
kubectl kustomize ./blue-green/each-namespace/alb-weighted-tg/ingress/overlays/v2 | kubectl apply -f -
```

Access to ALB, make sure all trafics are routed to v2.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes-share -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
```

### Canaly deployment

Change Ingress route to use canaly, let's route 90％ traffic to v1 and 10％ to green.

```shell
kubectl kustomize ./blue-green/each-namespace/alb-weighted-tg/ingress/overlays/canaly | kubectl apply -f -
```

Access to ALB, make sure trafiic is routed to v1 and v2 for 90:10.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes-share -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 2
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 2
```

### Clean up

Delete all resources.

```shell
kubectl kustomize ./blue-green/each-namespace/alb-weighted-tg/ingress/overlays/v1 | kubectl delete -f -
helm uninstall v1 --namespace hello-kubernetes-blue
helm uninstall v2 --namespace hello-kubernetes-green
kubectl delete ns hello-kubernetes-share
```




### Getting Started

Deploy Apps.

```shell
helm upgrade --install v1 ./helm/hello-kubernetes \
  --namespace hello-kubernetes-blue \
  --set message="You are reaching hello-kubernetes version 1" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```

```shell
helm upgrade --install v2 ./helm/hello-kubernetes \
  --namespace hello-kubernetes-green \
  --set message="You are reaching hello-kubernetes version 2" \
  --set ingress.configured=true \
  --set service.type="ClusterIP" \
  --create-namespace \
  --wait
```


**Blue deployment**

Deploy ingress and create ALB.

```shell
k kustomize ./blue-green/each-namespace/alb-weighted-tg2/ingress/overlays/v1 | kubectl apply -f -
```

Access to ALB, make sure all trafics are routed to v1.

```shell
endpoint=$(kubectl get ingress hello-kubernetes -n hello-kubernetes-share -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "$endpoint"
while true; do curl -s "$endpoint" | grep version; sleep 1; done
```

```
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
  You are reaching hello-kubernetes version 1
```
