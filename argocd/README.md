# README

This directory introduce ArgoCD deployment and samples.

## Getting started

1. Install ArgoCD.

    > https://artifacthub.io/packages/helm/argo/argo-cd

    ```shell
    helm repo add argo https://argoproj.github.io/argo-helm
    helm upgrade --install argocd argo/argo-cd --set "server.extraArgs={--insecure}" --set server.extensions.enabled=true --set server.service.type=LoadBalancer --set server.service.servicePortHttp=3080 --set server.service.servicePortHttps=3443 -n argocd --create-namespace --wait
    ```

2. Get password for ArgoCD UI's admin user.

    ```shell
    kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
    ```

3. Access ArgoCD with http://localhost:3080. user `admin`, password `see above step2`.

    ```shell
    # also you can access via CLI
    argocd login 127.0.0.1:3080 --name local --username admin --password "PASS"
    ```

4. Deploy ArgoCD AppProject.

    ```shell
    kubectl apply -f argocd/appproject.yaml
    ```

Now you are ready to deploy applicaiton.

## Sample1: sample-app-kustomize

Deploy application which public image, Kubernetes manifests are manged with Kustomize.

1. Deploy ArgoCD Application.

```shell
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

2. Delete `Application` will remove managed application and namespaces.

    ```shell
    $ k delete app sample-app-kustomize -n argocd
    ```

## Sample2: sample-local-image

Deploy application which use local image and kustomize.

1. Build guestbook-go image.

    ```shellell
    docker build -t guestbook:dev -f argocd/sample-local-image/guestbook-go/Dockerfile argocd/sample-local-image/guestbook-go
    ```

2. Deploy ArgoCD Application.

```shell
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

3. Delete `Application` will remove managed application and namespaces.

    ```shell
    $ k delete app sample-local-image -n argocd
    ```

## Sample3: sample-appset-kustomize

Deploy ApplicationSet which include 2 app, `sample-app-kustomize` and `sample-local-image`.

> [!NOTE]
> Build guestbook-go image before deploying ApplicationSet. See section [#sample2: sample-local-image](#sample2-sample-local-image) for detail.

1. Deploy ArgoCD ApplicationSet.

```shell
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

2. Now you can find applicationset creates application and kubernetes resources.

    ```shell
    $ k get appset
    aNAME            AGE
    sample-appset   91s
    $ k get app
    NAME                   SYNC STATUS   HEALTH STATUS
    sample-app-kustomize   Synced        Healthy
    sample-local-image     Synced        Healthy
    $ k get ns
    NAME                   STATUS   AGE
    argocd                 Active   21m
    default                Active   39h
    kube-node-lease        Active   39h
    kube-public            Active   39h
    kube-system            Active   39h
    sample-app-kustomize   Active   92s
    sample-local-image     Active   92s
    ```

3. Check resources on ArgoCD. There are no `ApplicationSet` view, but you can find each app instead.

    ![image](https://gist.github.com/assets/3856350/63ebdc47-96c6-4a9e-8717-d0c275cc858a)



4. Delete `ApplicationSet` will remove managed application and namespaces.

    ```shell
    $ k delete appset sample-appset -n argocd
    ```

    ![image](https://gist.github.com/assets/3856350/6f79c886-86fb-4697-b2eb-d16e075fd52e)
