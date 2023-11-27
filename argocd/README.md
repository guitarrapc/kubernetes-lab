# README

## Prerequisite

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

4. Deploy ArgoCD AppProject.

    ```sh
    kubectl apply -f argocd/appproject.yaml
    ```

## sample-app-kustomize

1. Deploy ArgoCD Application.

```sh
cat <<EOF | kubectl apply -f -
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: sample-app-kustomize
  namespace: argocd
  finalizers:
    - resources-finalizer.argocd.argoproj.io
spec:
  destination:
    server: https://kubernetes.default.svc
    namespace: sample-app-kustomize
  project: kubernetes-lab
  source:
    repoURL: https://github.com/guitarrapc/kubernetes-lab
    targetRevision: "$(git rev-parse --abbrev-ref HEAD)"
    path: argocd/sample-app-kustomize/kustomize
  syncPolicy:
    syncOptions:
      - CreateNamespace=true
    automated:
      selfHeal: true
      prune: true
EOF
```


## sample-local-image

1. Build guestbook-go image.

    ```shell
    docker build -t guestbook:dev -f argocd/sample-local-image/guestbook-go/Dockerfile argocd/sample-local-image/guestbook-go
    ```

2. Deploy ArgoCD Application.

```sh
cat <<EOF | kubectl apply -f -
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: sample-local-image
  namespace: argocd
  finalizers:
    - resources-finalizer.argocd.argoproj.io
spec:
  destination:
    server: https://kubernetes.default.svc
    namespace: sample-local-image
  project: kubernetes-lab
  source:
    repoURL: https://github.com/guitarrapc/kubernetes-lab
    targetRevision: "$(git rev-parse --abbrev-ref HEAD)"
    path: argocd/sample-local-image/kustomize
  syncPolicy:
    syncOptions:
      - CreateNamespace=true
    automated:
      selfHeal: true
      prune: true
EOF
```

## sample-appset-kustomize

1. Deploy ArgoCD Application.

```sh
cat <<EOF | kubectl apply -f -
apiVersion: argoproj.io/v1alpha1
kind: ApplicationSet
metadata:
  name: sample-appset
  namespace: argocd
spec:
  generators:
  - list:
      elements:
      - cluster: https://kubernetes.default.svc
        name: sample-app-kustomize
        path: argocd/sample-app-kustomize/kustomize
      - cluster: https://kubernetes.default.svc
        name: sample-local-image
        path: argocd/sample-local-image/kustomize
  syncPolicy:
    preserveResourcesOnDeletion: false
  template:
    metadata:
      name: '{{ name }}'
    spec:
      project: kubernetes-lab
      source:
        repoURL: https://github.com/guitarrapc/kubernetes-lab
        targetRevision: "$(git rev-parse --abbrev-ref HEAD)"
        path: '{{ path }}'
      destination:
        server: '{{ cluster }}'
        namespace: '{{ name }}'
      ignoreDifferences: []
      syncPolicy:
        syncOptions:
        - CreateNamespace=true
        automated:
          selfHeal: true
          prune: true
EOF
```
