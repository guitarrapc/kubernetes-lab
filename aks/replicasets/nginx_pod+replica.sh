kubectl apply -f replicasets/nginx-pod.yaml
kubectl get pod

kubectl apply -f replicasets/replicaset_nginx.yaml
kubectl get pod

kubectl delete pod nginx-replicaset-llwk4
kubectl get pod
