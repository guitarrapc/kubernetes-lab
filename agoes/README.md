## Setting up Agoes cluster on EKS

> REF: https://agones.dev/site/docs/installation/

create eks cluster with 1.12 (agoes currently support 1.12 and n-1 version.)

```
eksctl create cluster \
--name agoes \
--version 1.12 \
--nodegroup-name standard-workers \
--node-type t3.medium \
--nodes 1 \
--nodes-min 1 \
--nodes-max 3 \
--node-ami auto
```

Allow udp on node

```
clusterName=agoes
vpcId=$(aws ec2 describe-vpcs --filters "Name=tag:alpha.eksctl.io/cluster-name,Values=$clusterName" | jq -r '.Vpcs[].VpcId')
groupId=$(aws ec2 describe-security-groups --filters "Name=tag:alpha.eksctl.io/nodegroup-name,Values=standard-workers" "Name=tag:alpha.eksctl.io/cluster-name,Values=$clusterName" "Name=vpc-id,Values=$vpcId" | jq -r '.SecurityGroups[].GroupId')
aws ec2 authorize-security-group-ingress --group-id $groupId --protocol udp --port 7000-8000 --cidr 0.0.0.0/0
```

## Install Agoes

> REF: https://agones.dev/site/docs/installation/#installing-agones

```
kubectl create namespace agones-system
kubectl apply -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/install/yaml/install.yaml
```

confirm

```
kubectl describe --namespace agones-system pods
kubectl get pods --namespace agones-system
```

## Create GameServer

REF: https://agones.dev/site/docs/getting-started/create-gameserver/

> GameServer Specification: https://agones.dev/site/docs/reference/gameserver/

```
kubectl apply -f https://gist.githubusercontent.com/guitarrapc/23aa2c53ad586a45cf9d266ad1e1d497/raw/58c83e9778796561c6f74e71297eebbdffde067e/agones-deploy.yaml
kubectl get gameservers
kubectl get pod
```

confirm gameserver runninng via netcat.

```
$ port=$(kubectl get gs -o json | jq -r .items[].status.ports[].port)
$ nc -u 13.231.213.229 $port

Hello world!
ACK: Hello world!
```

You can shutdown GameServer Pod via running EXIT on netcat.

```
$ nc -u 13.231.213.229 7457

EXIT

$ kubectl get gs
AME         STATE      ADDRESS          PORT   NODE                                                AGE
udp-server   Shutdown   13.231.213.229   7041   ip-192-168-58-250.ap-northeast-1.compute.internal   16m

$ kubectl get pod
NAME         READY   STATUS        RESTARTS   AGE
udp-server   0/2     Terminating   0          16m
```

## Create GameServer Fleet

> REF: https://agones.dev/site/docs/getting-started/create-fleet/

```
kubectl apply -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/fleet.yaml
kubectl get fleet
```

scale out to 5.

```
kubectl scale fleet simple-udp --replicas=5
```

create gameserver allocation.

```
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml
kubectl get fleet
kubectl get gs
```

scale down to 0. Allocated will not scale in.

```
kubectl scale fleet simple-udp --replicas=0
```

connect to the allocated gameserver. any message will echo back to you.

> make sure EXIT will stop udp-server and stop gameserver allocation, but fleet will keep gameserver to 5 even EXIT or `kubectl delete gameserver`

```
$ kubectl get gameservers | grep Allocated | awk '{print $3":"$4 }'
13.231.213.229:7973
$ port=$(kubectl get gs -o json | jq -r .items[].status.ports[].port)
$ netcat -u 13.231.213.229 $port
Hello world!
ACK: Hello world!
EXIT
ACK: EXIT

$ k get gs
No resources found.
```

deploy new version, scale to 5 as initial.

```
kubectl scale fleet simple-udp --replicas=5
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml
kubectl get fleet
```

update deploy pod's port to 6000, then check pod status.

> cannot connect to udp-server anymore. you must roll back to 7654 to connect.

```
$ kubectl edit fleet simple-udp
$ kubectl get gs -w

NAME                     AGE
simple-udp-8vbdw-6m9wv   68s
simple-udp-8vbdw-c9f5x   68s
simple-udp-8vbdw-mpdks   68s
simple-udp-jwlp9-5xzcp   7s
simple-udp-jwlp9-f6lrb   7s
simple-udp-jwlp9-htll6   7s
simple-udp-jwlp9-m99dd   3s
simple-udp-jwlp9-s5z4j   7s
simple-udp-jwlp9-m99dd   5s
simple-udp-8vbdw-6m9wv   70s
simple-udp-8vbdw-mpdks   71s
simple-udp-8vbdw-c9f5x   71s
simple-udp-8vbdw-c9f5x   71s
simple-udp-8vbdw-c9f5x   75s
simple-udp-jwlp9-s5z4j   32s
simple-udp-jwlp9-s5z4j   32s
simple-udp-jwlp9-m99dd   28s
simple-udp-jwlp9-m99dd   28s
simple-udp-jwlp9-f6lrb   36s
simple-udp-jwlp9-f6lrb   36s
```

after confirm new version deployed, roll back to 7654 and allocate gameserver.

```
kubectl edit fleet simple-udp
kubectl get gs -w
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml
kubectl get fleet
kubectl get gs
```

connect to new 7654 deployed gameserver.

```
$ kubectl get gameservers | grep Allocated | awk '{print $3":"$4 }'
13.231.213.229:7973
$ port=$(kubectl get gs -o json | jq -r .items[].status.ports[].port)
$ netcat -u 13.231.213.229 $port
Hello world!
ACK: Hello world!
EXIT
ACK: EXIT

$ k get gs
No resources found.
```

if you wan to scale into 0, use `kubectl scale fleet`.

```
$ kubectl scale fleet simple-udp --replicas=0

NAME         SCHEDULING   DESIRED   CURRENT   ALLOCATED   READY   AGE
simple-udp   Packed       0         0         0           0       1h
```
