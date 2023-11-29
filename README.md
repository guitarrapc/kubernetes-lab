# kubernetes-lab


## Kubernetes Controller and AWS Relation

Some Kubernetes controllers and containers are closely associated with AWS resources. The figure below is an example of the relationship between Kubernetes resources and AWS resources. Various controllers operating in Kubernetes monitor the status of targeted Kubernetes resources and create or refer to AWS resources in accordance with YAML definitions. For instance, the AWS LoadBalancer Controller monitors the Kubernetes Ingress resources, and when an Ingress resource is created, it corresponds to creating an AWS ALB. Furthermore, External DNS monitors Kubernetes Ingress resources and when an Ingress resource is created, it creates a Route53 record that aligns the hostname and the ALB DNS.

```mermaid
---
title: Kubernetes Resource and AWS Coordination
---
flowchart LR
subgraph "Kubernetes(Reference)"
  ExternalSecrets
  Ingress
  Pod
  Provisioner
end
subgraph "Kubernetes(Create)"
  Node
  Secrets
end
subgraph Kubernetes Controller
  AWSLoadBalancerController["AWS LoadBalancer Controller"]
  ExternalDns["External DNS"]
  ExternalSecretsOperator["External Secrets Operator"]
  Karpenter["Karpenter"]
end
subgraph AWS
  ALB
  Route53
  SecretsManager
  ParameterStore
  EC2
end
Ingress -."1.Watch".- AWSLoadBalancerController --"2.Create AWS Resource"--> ALB
Ingress -."1.Watch".- ExternalDns -."2.Reference ALB DNS".-> ALB
ExternalDns --"3.Create Route53 Record"--> Route53
ExternalSecrets -."1.Watch".- ExternalSecretsOperator
ExternalSecretsOperator -."2.Reference".-> SecretsManager
ExternalSecretsOperator -."2.Reference".-> ParameterStore
ExternalSecretsOperator --"3.Create"--> Secrets
Provisioner -."2.Reference".- Karpenter --"3.Create"--> EC2 -.Register.-> Node
Pod -."1.Watch".- Karpenter
```


## Operate AWS in Kubernetes.

To manage AWS resources from containers running in Kubernetes, you must be authenticated by IAM at the time of AWS operation. However, since IAM is a feature of AWS, it's difficult to call IAM directly from the container, and it's necessary to somehow pass the IAM authentication information to the container. EKS has a mechanism for passing IAM authentication, which is called [IAM Role for ServiceAccount(IRSA)](https://docs.aws.amazon.com/eks/latest/userguide/iam-roles-for-service-accounts.html) and [EKS Pod Identities](https://docs.aws.amazon.com/ja_jp/eks/latest/userguide/pod-identities.html).

**IAM Role for ServiceAccount(IRSA)**

To use IRSA, you must setup OIDC and EKS trust relationship beforehand.

* Create OIDC Provider which trust EKS Cluster.

IRSA is comprised of three steps.

1. Create an IAM Role. At this time, specify the EKS cluster, Service Account, and Namespace that will utilize IAM Role authentication.
2. Set the IAM Role ARN for the ServiceAccount specified in the above step.
3. When using the Service Account specified above in a Pod, the AWS authentication information is inserted into the environment variables by IRSA at the time of Manifest apply. Now Container authenticated by sts and it can operate AWS with temporary auth.

The following diagram illustrates the association between Kubernetes resources and AWS when using IRSA.

```mermaid
---
title: IAM Role for ServiceAccount (IRSA)
---
flowchart LR
Terraform
subgraph AWS
  IAMRole["IAM Role"]
  IAMOIDC["IAM OIDC Provider"]
  ALB
  IAMSTS["IAM STS"]
end
subgraph EKS
  subgraph "Namespace A"
    ServiceAccount
    Pod
  end
end

EKS -."Trust relationship"...-> IAMOIDC
Terraform --"1.Define allowed ServiceAccount and Namespace"--> IAMRole
IAMRole -."1.1.Relation".-> IAMOIDC
ServiceAccount -."2.Specify IAM Role ARN in annotation".-> IAMRole
Pod --"3.Use ServiceAccount"---> ServiceAccount
Pod -."3.1.AssumeRoleWithWebIdentity"..-> IAMSTS
Pod --"3.2.Operate with temp auth"---> ALB
```

**EKS Pod Identities**

[TBD]
