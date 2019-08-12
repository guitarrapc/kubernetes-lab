#!/bin/bash
ACR_NAME=sampleGuitarrapcACR
az acr build --registry $ACR_NAME --image photo-view:v1.0 v1.0/
az acr build --registry $ACR_NAME --image photo-view:v2.0 v2.0/

# show
az acr repository show-tags -n $ACR_NAME --repository photo-view
