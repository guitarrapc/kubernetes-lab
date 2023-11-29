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
