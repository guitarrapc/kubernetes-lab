## README

1. Install ArgoCD.

    > https://artifacthub.io/packages/helm/argo/argo-cd

    ```sh
    helm repo add argo https://argoproj.github.io/argo-helm
    helm upgrade --install argocd argo/argo-cd --set "server.extraArgs={--insecure}" --set server.extensions.enabled=true --set server.service.type=LoadBalancer --set server.service.servicePortHttp=3080 --set server.service.servicePortHttps=3443 -n argocd --create-namespace --wait
    ```

2. GetPassword

    ```sh
    kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
    ```

Now you can access ArgoCD with http://localhost:3080 .
