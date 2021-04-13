```shell
pushd src
    docker build -t guitarrapc/grpcservice-test -f GrpcService/Dockerfile .
    docker tag guitarrapc/grpcservice-test guitarrapc/grpcservice-test:0.1.6
    docker push guitarrapc/grpcservice-test:0.1.6
popd
kubectl kustomize k8s | kubectl apply -f -
kubectl port-forward svc/app-svc 5000:80
```

connect client

restart server

```shell
kubectl rollout restart deploy app
kubectl get po -w
```