#!/bin/bash
k apply -f ./k8s

k exec -it pripod -- dotnet Pripod.SampleApp.dll
pod=$(k get pod -o name | grep pripod- | sed -e 's|pod/||g')
k exec -it $pod -- dotnet Pripod.SampleApp.dll

