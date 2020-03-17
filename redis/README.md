## Redis and RedisInsight on container

This describe 2 container orchestration deployment, docker-compose and kubernetes.

![image](https://user-images.githubusercontent.com/3856350/76840074-ef19d300-6879-11ea-987c-626fef43a3e9.png)

## Docker deploy

```shell
pushd ./compose && docker-compose up && popd
```

## kubernetes deploy

```shell
kubectl apply -f ./k8s
```

**NOTE**

RedisInsight deploy to Kubernetes with Service needs attention.

You must specify `spec.template.spec.containers[redisinsight].env` for REDISINSIGHT_HOST and REDISINSIGHT_PORT.

```yaml
spec:
  template:
    spec:
      containers:
        - name: redisinsight
          env:
            # require to set HOST/PORT on env to avoid pod port conflict when using svc.
            # ref: https://github.com/RedisLabs/redislabs-docs/issues/676
            - name: REDISINSIGHT_HOST
              value: "0.0.0.0"
            - name: REDISINSIGHT_PORT
              value: "8001"
```

> you don't need this env when you don't use svc.
