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
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserver.yaml
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

## Fleet Autoslaer

> REF: https://agones.dev/site/docs/getting-started/create-fleetautoscaler/

let(s auto scale fleet every time without running kubectl everytime.

install autoscaler.

```
kubectl apply -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/fleetautoscaler.yaml
```

see autoscaler status.

```
$ kubectl describe fleetautoscaler simple-udp-autoscaler

Name:         simple-udp-autoscaler
Namespace:    default
Labels:       <none>
Annotations:  kubectl.kubernetes.io/last-applied-configuration:
                {"apiVersion":"autoscaling.agones.dev/v1","kind":"FleetAutoscaler","metadata":{"annotations":{},"name":"simple-udp-autoscaler","namespace"...
API Version:  autoscaling.agones.dev/v1
Kind:         FleetAutoscaler
Metadata:
  Creation Timestamp:  2019-09-30T05:37:48Z
  Generation:          1
  Resource Version:    16456
  Self Link:           /apis/autoscaling.agones.dev/v1/namespaces/default/fleetautoscalers/simple-udp-autoscaler
  UID:                 700bc096-e344-11e9-9700-06cdb1357bd8
Spec:
  Fleet Name:  simple-udp
  Policy:
    Buffer:
      Buffer Size:   2
      Max Replicas:  10
      Min Replicas:  0
    Type:            Buffer
Status:
  Able To Scale:     true
  Current Replicas:  2
  Desired Replicas:  2
  Last Scale Time:   2019-09-30T05:37:48Z
  Scaling Limited:   false
Events:
  Type    Reason            Age   From                        Message
  ----    ------            ----  ----                        -------
  Normal  AutoScalingFleet  33s   fleetautoscaler-controller  Scaling fleet simple-udp from 0 to 2
```

`Status.Last Scale Time` will indicate you last scale time, nil for never.
Expected convergenece is in seconds.

allocate gameserver from fleet.

```
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml

rallocation.yaml -o yaml
apiVersion: allocation.agones.dev/v1
kind: GameServerAllocation
metadata:
  creationTimestamp: "2019-09-30T05:44:56Z"
  name: simple-udp-fxcwh-xmv2k
  namespace: default
spec:
  metadata: {}
  multiClusterSetting:
    policySelector: {}
  required:
    matchLabels:
      agones.dev/fleet: simple-udp
  scheduling: Packed
status:
  address: 13.231.213.229
  gameServerName: simple-udp-fxcwh-xmv2k
  nodeName: ip-192-168-58-250.ap-northeast-1.compute.internal
  ports:
  - name: default
    port: 7494
  state: Allocated
```

list gameservers.

```
$ kubectl get gs

NAME                     STATE       ADDRESS          PORT   NODE                                                AGE
simple-udp-fxcwh-n4dwb   Ready       13.231.213.229   7051   ip-192-168-58-250.ap-northeast-1.compute.internal   7m
simple-udp-fxcwh-xmv2k   Allocated   13.231.213.229   7494   ip-192-168-58-250.ap-northeast-1.compute.internal   7m
```

now fleet scaled out to 3, this is controlled with `bufferSize`.

```
$ kubectl describe fleetautoscaler simple-udp-autoscaler

Name:         simple-udp-autoscaler
Namespace:    default
Labels:       <none>
Annotations:  kubectl.kubernetes.io/last-applied-configuration:
                {"apiVersion":"autoscaling.agones.dev/v1","kind":"FleetAutoscaler","metadata":{"annotations":{},"name":"simple-udp-autoscaler","namespace"...
API Version:  autoscaling.agones.dev/v1
Kind:         FleetAutoscaler
Metadata:
  Creation Timestamp:  2019-09-30T05:37:48Z
  Generation:          1
  Resource Version:    17078
  Self Link:           /apis/autoscaling.agones.dev/v1/namespaces/default/fleetautoscalers/simple-udp-autoscaler
  UID:                 700bc096-e344-11e9-9700-06cdb1357bd8
Spec:
  Fleet Name:  simple-udp
  Policy:
    Buffer:
      Buffer Size:   2
      Max Replicas:  10
      Min Replicas:  0
    Type:            Buffer
Status:
  Able To Scale:     true
  Current Replicas:  3
  Desired Replicas:  3
  Last Scale Time:   2019-09-30T05:45:11Z
  Scaling Limited:   false
Events:
  Type    Reason            Age    From                        Message
  ----    ------            ----   ----                        -------
  Normal  AutoScalingFleet  8m12s  fleetautoscaler-controller  Scaling fleet simple-udp from 0 to 2
  Normal  AutoScalingFleet  49s    fleetautoscaler-controller  Scaling fleet simple-udp from 2 to 
```

```
$ kubectl get gs

NAME                     STATE       ADDRESS          PORT   NODE                                                AGE
simple-udp-fxcwh-j49nv   Ready       13.231.213.229   7059   ip-192-168-58-250.ap-northeast-1.compute.internal   56s
simple-udp-fxcwh-n4dwb   Ready       13.231.213.229   7051   ip-192-168-58-250.ap-northeast-1.compute.internal   8m
simple-udp-fxcwh-xmv2k   Allocated   13.231.213.229   7494   ip-192-168-58-250.ap-northeast-1.compute.internal   8m
```

stop allocated server

```
$ kubectl get gameservers | grep Allocated | awk '{print $3":"$4 }'
13.231.213.229:7494
$ nc -u 13.231.213.229 7494
hoge
ACK: hoge
EXIT
ACK: EXIT
```

now fleet scale in from 3 -> 2.

```
$ kubectl describe fleetautoscaler simple-udp-autoscaler

Name:         simple-udp-autoscaler
Namespace:    default
Labels:       <none>
Annotations:  kubectl.kubernetes.io/last-applied-configuration:
                {"apiVersion":"autoscaling.agones.dev/v1","kind":"FleetAutoscaler","metadata":{"annotations":{},"name":"simple-udp-autoscaler","namespace"...
API Version:  autoscaling.agones.dev/v1
Kind:         FleetAutoscaler
Metadata:
  Creation Timestamp:  2019-09-30T05:37:48Z
  Generation:          1
  Resource Version:    17352
  Self Link:           /apis/autoscaling.agones.dev/v1/namespaces/default/fleetautoscalers/simple-udp-autoscaler
  UID:                 700bc096-e344-11e9-9700-06cdb1357bd8
Spec:
  Fleet Name:  simple-udp
  Policy:
    Buffer:
      Buffer Size:   2
      Max Replicas:  10
      Min Replicas:  0
    Type:            Buffer
Status:
  Able To Scale:     true
  Current Replicas:  3
  Desired Replicas:  2
  Last Scale Time:   2019-09-30T05:48:41Z
  Scaling Limited:   false
Events:
  Type    Reason            Age    From                        Message
  ----    ------            ----   ----                        -------
  Normal  AutoScalingFleet  11m    fleetautoscaler-controller  Scaling fleet simple-udp from 0 to 2
  Normal  AutoScalingFleet  3m44s  fleetautoscaler-controller  Scaling fleet simple-udp from 2 to 3
  Normal  AutoScalingFleet  14s    fleetautoscaler-controller  Scaling fleet simple-udp from 3 to 2
```

fleet status.

```
$ kubectl get gs

NAME                     STATE   ADDRESS          PORT   NODE                                                AGE
simple-udp-fxcwh-j49nv   Ready   13.231.213.229   7059   ip-192-168-58-250.ap-northeast-1.compute.internal   4m
simple-udp-fxcwh-n4dwb   Ready   13.231.213.229   7051   ip-192-168-58-250.ap-northeast-1.compute.internal   11m
```

let's change buffer size to 3. change `bufferSize` to 3.

```
kubectl edit fleetautoscaler simple-udp-autoscaler
```

see status.

```
$ kubectl get gs

NAME                     STATE       ADDRESS          PORT   NODE                                                AGE
simple-udp-fxcwh-64z8z   Scheduled   13.231.213.229   7173   ip-192-168-58-250.ap-northeast-1.compute.internal   6s
simple-udp-fxcwh-j49nv   Ready       13.231.213.229   7059   ip-192-168-58-250.ap-northeast-1.compute.internal   5m
simple-udp-fxcwh-n4dwb   Ready       13.231.213.229   7051   ip-192-168-58-250.ap-northeast-1.compute.internal   13m
```

## Fleet Autoscaler with Webhook

> REF: https://agones.dev/site/docs/getting-started/create-webhook-fleetautoscaler/

If you autoscale can't be handle with simple buffer, you can impletement login and set as API servicea.

Deploy webhook service for autoscaling.

```
kubectl apply -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/autoscaler-webhook/autoscaler-service.yaml
kubectl delete -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/autoscaler-webhook/autoscaler-service.yaml
```

confirm liveness probe is fine.

```
kubectl describe pod autoscaler-webhook
```

remove fleet autoscaler if you deployed.

```
kubectl delete -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/fleetautoscaler.yaml
```

deploy webhook fleet autoscaler.

> you can check the logic. https://github.com/googleforgames/agones/tree/release-1.0.0/examples/autoscaler-webhook

/healthと /scale を実装して、response を返すだけでいいのでロジックは言語問わず問題ない。

```
kubectl apply -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/webhookfleetautoscaler.yaml
```

check webhook fleet autoscaler.

```
kubectl describe fleetautoscaler.autoscaling.agones.dev/webhook-fleet-autoscaler
kubectl get gs
```

allocate gameserver 2times.

```
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml
kubectl create -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserverallocation.yaml -o yaml
```

check fleet status, 2 instance are added as allocated + buffer.

```
$ kubectl describe fleetautoscaler webhook-fleet-autoscaler

Events:
  Type    Reason            Age    From                        Message
  ----    ------            ----   ----                        -------
  Normal  AutoScalingFleet  2m16s  fleetautoscaler-controller  Scaling fleet simple-udp from 3 to 2
  Normal  AutoScalingFleet  36s    fleetautoscaler-controller  Scaling fleet simple-udp from 2 to 4

$ kubectl get gs

NAME                     STATE       ADDRESS          PORT   NODE                                                AGE
simple-udp-fxcwh-64z8z   Allocated   13.231.213.229   7173   ip-192-168-58-250.ap-northeast-1.compute.internal   21m
simple-udp-fxcwh-7567x   Ready       13.231.213.229   7245   ip-192-168-58-250.ap-northeast-1.compute.internal   40s
simple-udp-fxcwh-fsph8   Ready       13.231.213.229   7325   ip-192-168-58-250.ap-northeast-1.compute.internal   40s
simple-udp-fxcwh-j49nv   Allocated   13.231.213.229   7059   ip-192-168-58-250.ap-northeast-1.compute.internal   27m
```

let's scale-in, connect and shutdown gameserver.

```
$ nc -u 13.231.213.229 7173
EXIT

$ nc -u 13.231.213.229 7059
EXIT 
```

you may find scaled down event on fleet.

```
$ kubectl describe fleetautoscaler webhook-fleet-autoscaler

Events:
  Type    Reason            Age    From                        Message
  ----    ------            ----   ----                        -------
  Normal  AutoScalingFleet  3m55s  fleetautoscaler-controller  Scaling fleet simple-udp from 3 to 2
  Normal  AutoScalingFleet  2m15s  fleetautoscaler-controller  Scaling fleet simple-udp from 2 to 4
  Normal  AutoScalingFleet  15s    fleetautoscaler-controller  Scaling fleet simple-udp from 4 to 2
```

In case you want setup CA bundle, check it out.

> https://agones.dev/site/docs/getting-started/create-webhook-fleetautoscaler/


## Edit game server (Golang)

> REF: https://agones.dev/site/docs/getting-started/edit-first-gameserver-go/

clone repo.

> git clone https://github.com/googleforgames/agones.git

open `agones/examples/simple-udp/main.go`

change line 107. (exit handling)

```golang
- respond(conn, sender, "ACK: "+txt+"\n")
+ respond(conn, sender, "ACK: Echo says "+txt+"Exit detected"+"\n")
```

change line 159. (message handling)

```golang
- respond(conn, sender, "ACK: "+txt+"\n")
+ respond(conn, sender, "ACK: Echo says "+txt+"\n")
```

> see simple-udp folder in this repo.

(if local build is needed) build golang udp-server.

```
go get agones.dev/agones/pkg/sdk
# linux/macOS
GOOS=linux GOARCH=amd64 CGO_ENABLED=0 go build -o bin/server -a -v main.go
# windows
set GOOS=linux
set GOARCH=amd64
set CGO_ENABLED=0
go build -o bin/server -a -v main.go
```

docker build and push to dockerhub.

```
docker build -t agones-udp-server:modified -f examples/simple-udp/Dockerfile .
docker tag agones-udp-server:modified guitarrapc/agones-udp-server:modified
docker push guitarrapc/agones-udp-server:modified
```

(if using gameserver)

download gameserver.yaml and modify to use own image.

```
curl -LO https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserver.yaml
vim gameserver.yaml
```

```
  template:
    spec:
      containers:
      - name: simple-udp
        image: guitarrapc/agones-udp-server:modified
```

deploy gameserver.yaml

```
kubectl delete -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserver.yaml
kubectl create -f gameserver.yaml
```


(if using fleet autoscaler)

download fleet.yaml and modify to use own image.

```
curl -LO https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/fleet.yaml
vim fleet.yaml
```

apply and check ready is back to desired count.

```
kubectl apply -f fleet.yaml
kubectl get fleet -w
```

scale to 0 and 2

```
kubectl scale --replicas=0 fleet/simple-udp
kubectl scale --replicas=2 fleet/simple-udp
```

connect and confirm change deployed.

```
$ kubectl get gs
$ nc -u 13.231.213.229 7054
hoge
ACK: Echo says hoge
moge
ACK: Echo says moge
EXIT
ACK: Echo says EXITExit detected
```

## Local Development

> REF: https://agones.dev/site/docs/guides/client-sdks/local/

You can execute agones not only on k8s but also local.

download agonessdk-server-x.x.x.zip and use you environment executable with argument `--local`.

> https://github.com/googleforgames/agones/releases

windows sample.

```
sdk-server.windows.amd64.exe --local
```

now you can test your grpc or REST connection with agones sdk.

here's swagger template, use swagger for your REST implementaion.

> https://github.com/googleforgames/agones/blob/release-1.0.0/sdk.swagger.json

> https://editor.swagger.io/

## Edit game server (CSharp)

> REF: https://agones.dev/site/docs/getting-started/edit-first-gameserver-go/

docker build and push to dockerhub.

```
pushd simple-udp-csharp
docker build -t agones-udp-server-csharp:0.2.0 -f Agones/Dockerfile .
docker tag agones-udp-server-csharp:0.2.0 guitarrapc/agones-udp-server-csharp:0.2.0
docker push guitarrapc/agones-udp-server-csharp:0.2.0
popd
```

(if using gameserver)

download gameserver.yaml and modify to use own image.

```
curl -LO https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserver.yaml
vim gameserver.yaml
```

```
  template:
    spec:
      containers:
      - name: simple-udp
        image: guitarrapc/agones-udp-server-csharp:0.1
```

deploy gameserver.yaml

```
kubectl delete -f https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/gameserver.yaml
kubectl create -f gameserver-csharp.yaml
kubectl delete -f gameserver-csharp.yaml
```


(if using fleet autoscaler)

download fleet.yaml and modify to use own image.

```
curl -L -o fleet-chsarp.yaml https://raw.githubusercontent.com/googleforgames/agones/release-1.0.0/examples/simple-udp/fleet.yaml
vim fleet.yaml
```

apply and check ready is back to desired count.

```
kubectl apply -f fleet-csharp.yaml
kubectl get fleet -w
kubectl delete -f fleet-csharp.yaml
```

scale to 0 and 2

```
kubectl scale --replicas=0 fleet/simple-udp
kubectl scale --replicas=1 fleet/simple-udp
kubectl scale --replicas=2 fleet/simple-udp
```

connect and confirm change deployed.

```
$ kubectl get gs
$ nc -u 13.231.213.229 7054
hoge
ACK: Echo says hoge
moge
ACK: Echo says moge
EXIT
ACK: Echo says EXITExit detected
```

## MagicOnion ChatApp

```
docker build -t agones-udp-server-magiconionchatapp:0.1.1 -f ChatApp.Server/Dockerfile .
docker tag agones-udp-server-magiconionchatapp:0.1.1 guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
docker push guitarrapc/agones-udp-server-magiconionchatapp:0.1.1
```

```
vim fleet-csharp-magiconionchatapp.yaml
kubectl apply -f fleet-csharp-magiconionchatapp.yaml
kubectl get fleet -w
kubectl delete -f fleet-csharp-magiconionchatapp.yaml
```

```
kubectl scale --replicas=0 fleet/simple-udp
kubectl scale --replicas=1 fleet/simple-udp
kubectl scale --replicas=2 fleet/simple-udp
```
