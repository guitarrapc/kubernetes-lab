## README

1. Install AWS LoadBalancer Controller.

    > https://artifacthub.io/packages/helm/aws/aws-load-balancer-controller

    ```sh
    helm repo add eks https://aws.github.io/eks-charts
    # If using IAM Roles for service account install as follows -  NOTE: you need to specify both of the chart values `serviceAccount.create=false` and `serviceAccount.name=aws-load-balancer-controller`
    helm upgrade --install aws-load-balancer-controller eks/aws-load-balancer-controller --set clusterName=my-cluster -n kube-system --set serviceAccount.create=false --set serviceAccount.name=aws-load-balancer-controller
    ```


2. Apply Manifests.

    ```sh
    kubectl -k ./aws-loadbalancer-controller/ | kubectl apply -f -
    ```
