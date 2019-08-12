#!/bin/bash
kubectl get pod -l app=photo-view
kubectl get pod -l app=photo-view,env=prod