#!/bin/bash
# enable docker and k8s on windows and enable "Expose daemon on tcp://localhost:2375 without TLS"
# make sure k8s is running on Windows, if not begin reset docker.

# make sure docker can run on wsl
docker ps 

# install kubeadm
curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key add -
sudo apt-add-repository "deb http://apt.kubernetes.io/ kubernetes-xenial main"
sudo apt update
sudo apt install kubeadm -y

# switch to docker-desktop k8s cluster
kubectl config use-context docker-desktop
kubectl cluster-info

# try k8s from wsl
kubectl run hello-world --image k8s.gcr.io/echoserver:1.10 --port 8080
kubectl get svc
kubectl get pods
kubectl get deployment
kubectl expose deployment hello-world --type NodePort
kubectl get svc
curl http://localhost:30448

# remove pods
kubectl delete deployment hello-world
kubectl delete svc hello-world
