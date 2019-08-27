#!/bin/bash
# create vpc
rm -f amazon-eks-vpc-sample.yaml
curl https://amazon-eks.s3-us-west-2.amazonaws.com/cloudformation/2018-08-21/amazon-eks-vpc-sample.yaml -O

cat <<'EOF' > ./eks_cfn_vpc.json
[
  {
    "ParameterKey": "VpcBlock",
    "ParameterValue": "10.10.0.0/16"
  },
  {
    "ParameterKey": "Subnet01Block",
    "ParameterValue": "10.10.0.0/24"
  },
  {
    "ParameterKey": "Subnet02Block",
    "ParameterValue": "10.10.1.0/24"
  },
  {
    "ParameterKey": "Subnet03Block",
    "ParameterValue": "10.10.2.0/24"
  }
]
EOF


vpcstack_arn=$(aws cloudformation create-stack --stack-name askboxvpc --template-body file://./amazon-eks-vpc-sample.yaml --parameters --capabilities CAPABILITY_IAM)
aws cloudformation describe-stacks --stack-name $vpcstack_arn --query Stacks[].StackStatus
