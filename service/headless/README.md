## README

    ```sh
    kubectl kustomize ./service/headless | k apply -f -
    kubectl kustomize ./service/headless | k delete -f -
    ```
