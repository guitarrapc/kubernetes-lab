## README

1. Install ArgoCD.

    > https://artifacthub.io/packages/helm/argo/argo-cd

    ```sh
    helm repo add argo https://argoproj.github.io/argo-helm
    helm upgrade --install argocd argo/argo-cd --set "server.extraArgs={--insecure}" --set server.extensions.enabled=true --set server.service.type=LoadBalancer --set server.service.servicePortHttp=3080 --set server.service.servicePortHttps=3443 -n argocd --create-namespace --wait
    ```

2. Get password for ArgoCD UI's admin user.

    ```sh
    kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
    ```

3. Access ArgoCD with http://localhost:3080. user `admin`, password `see above step2`.

    ```sh
    # also you can access via CLI
    argocd login 127.0.0.1:3080 --name local --username admin --password "PASS"
    ```

## sample-app-kustomize

1. Deploy ArgoCD AppProject.

    ```sh
    kubectl apply -f argocd/appproject.yaml
    ```

2. Create Manifest from Kustomize.

    ```sh
    mkdir -p ./argocd/sample-app-kustomize/sync-manifests && kubectl kustomize ./argocd/sample-app-kustomize/kustomize > ./argocd/sample-app-kustomize/sync-manifests/install.yaml
    ```

3. Commit & push manifest.

    ```sh
    git add -A && git commit -am "feat: add sample-app-kustomize" && git push
    ```

4. Deploy ArgoCD Application.

    ```sh
    kubectl apply -f argocd/sample-app-kustomize/app.yaml
    ```
