#!/bin/bash
kubectl get pod -n kube-system -o custom-columns=Pod:metadata.name,Node:spec.nodeName
