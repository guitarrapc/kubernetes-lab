using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using KubernetesClient.Converters;

namespace KubernetesClient.Requests
{
    public class KubernetesDeploymentCreateRequest
    {
        [Required]
        public string NameSpace { get; set; } = "default";
        /// <summary>
        /// Base64 content of Kubernetes Manifest object
        /// </summary>
        [Required]
        public string Body { get; set; }
        public string BodyContentType { get; set; } = "application/yaml";
    }

    public class KubernetesDeploymentMetadata
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public object spec { get; set; }

        public class Metadata
        {
            public string name { get; set; }
            public string @namespace { get; set; }
        }

    }

    public class KubernetesDeployment
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public Spec spec { get; set; }
        public Status status { get; set; }

        public class Metadata
        {
            public Annotations annotations { get; set; }
            public string clusterName { get; set; }
            public DateTime creationTimestamp { get; set; }
            public int deletionGracePeriodSeconds { get; set; }
            public DateTime deletionTimestamp { get; set; }
            public string[] finalizers { get; set; }
            public string generateName { get; set; }
            public int generation { get; set; }
            public Labels labels { get; set; }
            public Managedfield[] managedFields { get; set; }
            public string name { get; set; }
            public string @namespace { get; set; }
            public Ownerreference[] ownerReferences { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
            public string uid { get; set; }
        }

        public class Annotations
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Labels
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Managedfield
        {
            public string apiVersion { get; set; }
            public string fieldsType { get; set; }
            public Fieldsv1 fieldsV1 { get; set; }
            public string manager { get; set; }
            public string operation { get; set; }
            public DateTime time { get; set; }
        }

        public class Fieldsv1
        {
        }

        public class Ownerreference
        {
            public string apiVersion { get; set; }
            public bool blockOwnerDeletion { get; set; }
            public bool controller { get; set; }
            public string kind { get; set; }
            public string name { get; set; }
            public string uid { get; set; }
        }

        public class Spec
        {
            public int minReadySeconds { get; set; }
            public bool paused { get; set; }
            public int progressDeadlineSeconds { get; set; }
            public int replicas { get; set; }
            public int revisionHistoryLimit { get; set; }
            public Selector selector { get; set; }
            public Strategy strategy { get; set; }
            public Template template { get; set; }
        }

        public class Selector
        {
            public Matchexpression[] matchExpressions { get; set; }
            public Matchlabels matchLabels { get; set; }
        }

        public class Matchlabels
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Strategy
        {
            public Rollingupdate rollingUpdate { get; set; }
            public string type { get; set; }
        }

        public class Rollingupdate
        {
            public string maxSurge { get; set; }
            public string maxUnavailable { get; set; }
        }

        public class Template
        {
            public Metadata1 metadata { get; set; }
            public Spec1 spec { get; set; }
        }

        public class Metadata1
        {
            public Annotations1 annotations { get; set; }
            public string clusterName { get; set; }
            public DateTime creationTimestamp { get; set; }
            public int deletionGracePeriodSeconds { get; set; }
            public DateTime deletionTimestamp { get; set; }
            public string[] finalizers { get; set; }
            public string generateName { get; set; }
            public int generation { get; set; }
            public Labels1 labels { get; set; }
            public Managedfield1[] managedFields { get; set; }
            public string name { get; set; }
            public string _namespace { get; set; }
            public Ownerreference1[] ownerReferences { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
            public string uid { get; set; }
        }

        public class Annotations1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Labels1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Managedfield1
        {
            public string apiVersion { get; set; }
            public string fieldsType { get; set; }
            public Fieldsv11 fieldsV1 { get; set; }
            public string manager { get; set; }
            public string operation { get; set; }
            public DateTime time { get; set; }
        }

        public class Fieldsv11
        {
        }

        public class Ownerreference1
        {
            public string apiVersion { get; set; }
            public bool blockOwnerDeletion { get; set; }
            public bool controller { get; set; }
            public string kind { get; set; }
            public string name { get; set; }
            public string uid { get; set; }
        }

        public class Spec1
        {
            public int activeDeadlineSeconds { get; set; }
            public Affinity affinity { get; set; }
            public bool automountServiceAccountToken { get; set; }
            public Container[] containers { get; set; }
            public Dnsconfig dnsConfig { get; set; }
            public string dnsPolicy { get; set; }
            public bool enableServiceLinks { get; set; }
            public Ephemeralcontainer[] ephemeralContainers { get; set; }
            public Hostalias[] hostAliases { get; set; }
            public bool hostIPC { get; set; }
            public bool hostNetwork { get; set; }
            public bool hostPID { get; set; }
            public string hostname { get; set; }
            public Imagepullsecret[] imagePullSecrets { get; set; }
            public Initcontainer[] initContainers { get; set; }
            public string nodeName { get; set; }
            public Nodeselector nodeSelector { get; set; }
            public Overhead overhead { get; set; }
            public string preemptionPolicy { get; set; }
            public int priority { get; set; }
            public string priorityClassName { get; set; }
            public Readinessgate[] readinessGates { get; set; }
            public string restartPolicy { get; set; }
            public string runtimeClassName { get; set; }
            public string schedulerName { get; set; }
            public Securitycontext securityContext { get; set; }
            public string serviceAccount { get; set; }
            public string serviceAccountName { get; set; }
            public bool shareProcessNamespace { get; set; }
            public string subdomain { get; set; }
            public int terminationGracePeriodSeconds { get; set; }
            public Toleration[] tolerations { get; set; }
            public Topologyspreadconstraint[] topologySpreadConstraints { get; set; }
            public Volume[] volumes { get; set; }
        }

        public class Affinity
        {
            public Nodeaffinity nodeAffinity { get; set; }
            public Podaffinity podAffinity { get; set; }
            public Podantiaffinity podAntiAffinity { get; set; }
        }

        public class Nodeaffinity
        {
            public Preferredduringschedulingignoredduringexecution[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution
        {
            public Nodeselectorterm[] nodeSelectorTerms { get; set; }
        }

        public class Nodeselectorterm
        {
            public Matchexpression1[] matchExpressions { get; set; }
            public Matchfield[] matchFields { get; set; }
        }

        public class Matchexpression1
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Matchfield
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution
        {
            public Preference preference { get; set; }
            public int weight { get; set; }
        }

        public class Preference
        {
            public Matchexpression2[] matchExpressions { get; set; }
            public Matchfield1[] matchFields { get; set; }
        }

        public class Matchexpression2
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Matchfield1
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Podaffinity
        {
            public Preferredduringschedulingignoredduringexecution1[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution1[] requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution1
        {
            public Podaffinityterm podAffinityTerm { get; set; }
            public int weight { get; set; }
        }

        public class Podaffinityterm
        {
            public Labelselector labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector
        {
            public Matchexpression3[] matchExpressions { get; set; }
            public Matchlabels1 matchLabels { get; set; }
        }

        public class Matchlabels1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression3
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution1
        {
            public Labelselector1 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector1
        {
            public Matchexpression4[] matchExpressions { get; set; }
            public Matchlabels2 matchLabels { get; set; }
        }

        public class Matchlabels2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression4
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Podantiaffinity
        {
            public Preferredduringschedulingignoredduringexecution2[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution2[] requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution2
        {
            public Podaffinityterm1 podAffinityTerm { get; set; }
            public int weight { get; set; }
        }

        public class Podaffinityterm1
        {
            public Labelselector2 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector2
        {
            public Matchexpression5[] matchExpressions { get; set; }
            public Matchlabels3 matchLabels { get; set; }
        }

        public class Matchlabels3
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression5
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution2
        {
            public Labelselector3 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector3
        {
            public Matchexpression6[] matchExpressions { get; set; }
            public Matchlabels4 matchLabels { get; set; }
        }

        public class Matchlabels4
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression6
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Dnsconfig
        {
            public string[] nameservers { get; set; }
            public Option[] options { get; set; }
            public string[] searches { get; set; }
        }

        public class Option
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Nodeselector
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Overhead
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext
        {
            public int fsGroup { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions seLinuxOptions { get; set; }
            public int[] supplementalGroups { get; set; }
            public Sysctl[] sysctls { get; set; }
            public Windowsoptions windowsOptions { get; set; }
        }

        public class Selinuxoptions
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Sysctl
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Container
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env[] env { get; set; }
            public Envfrom[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle lifecycle { get; set; }
            public Livenessprobe livenessProbe { get; set; }
            public string name { get; set; }
            public Port[] ports { get; set; }
            public Readinessprobe readinessProbe { get; set; }
            public Resources resources { get; set; }
            public Securitycontext1 securityContext { get; set; }
            public Startupprobe startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice[] volumeDevices { get; set; }
            public Volumemount[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle
        {
            public Poststart postStart { get; set; }
            public Prestop preStop { get; set; }
        }

        public class Poststart
        {
            public Exec exec { get; set; }
            public Httpget httpGet { get; set; }
            public Tcpsocket tcpSocket { get; set; }
        }

        public class Exec
        {
            public string[] command { get; set; }
        }

        public class Httpget
        {
            public string host { get; set; }
            public Httpheader[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Prestop
        {
            public Exec1 exec { get; set; }
            public Httpget1 httpGet { get; set; }
            public Tcpsocket1 tcpSocket { get; set; }
        }

        public class Exec1
        {
            public string[] command { get; set; }
        }

        public class Httpget1
        {
            public string host { get; set; }
            public Httpheader1[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader1
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket1
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Livenessprobe
        {
            public Exec2 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget2 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket2 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec2
        {
            public string[] command { get; set; }
        }

        public class Httpget2
        {
            public string host { get; set; }
            public Httpheader2[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader2
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket2
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Readinessprobe
        {
            public Exec3 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget3 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket3 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec3
        {
            public string[] command { get; set; }
        }

        public class Httpget3
        {
            public string host { get; set; }
            public Httpheader3[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader3
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket3
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Resources
        {
            public Limits limits { get; set; }
            public Requests requests { get; set; }
        }

        public class Limits
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext1
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions1 seLinuxOptions { get; set; }
            public Windowsoptions1 windowsOptions { get; set; }
        }

        public class Capabilities
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions1
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions1
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe
        {
            public Exec4 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget4 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket4 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec4
        {
            public string[] command { get; set; }
        }

        public class Httpget4
        {
            public string host { get; set; }
            public Httpheader4[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader4
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket4
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Env
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom valueFrom { get; set; }
        }

        public class Valuefrom
        {
            public Configmapkeyref configMapKeyRef { get; set; }
            public Fieldref fieldRef { get; set; }
            public Resourcefieldref resourceFieldRef { get; set; }
            public Secretkeyref secretKeyRef { get; set; }
        }

        public class Configmapkeyref
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom
        {
            public Configmapref configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref secretRef { get; set; }
        }

        public class Configmapref
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Ephemeralcontainer
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env1[] env { get; set; }
            public Envfrom1[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle1 lifecycle { get; set; }
            public Livenessprobe1 livenessProbe { get; set; }
            public string name { get; set; }
            public Port1[] ports { get; set; }
            public Readinessprobe1 readinessProbe { get; set; }
            public Resources1 resources { get; set; }
            public Securitycontext2 securityContext { get; set; }
            public Startupprobe1 startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string targetContainerName { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice1[] volumeDevices { get; set; }
            public Volumemount1[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle1
        {
            public Poststart1 postStart { get; set; }
            public Prestop1 preStop { get; set; }
        }

        public class Poststart1
        {
            public Exec5 exec { get; set; }
            public Httpget5 httpGet { get; set; }
            public Tcpsocket5 tcpSocket { get; set; }
        }

        public class Exec5
        {
            public string[] command { get; set; }
        }

        public class Httpget5
        {
            public string host { get; set; }
            public Httpheader5[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader5
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket5
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Prestop1
        {
            public Exec6 exec { get; set; }
            public Httpget6 httpGet { get; set; }
            public Tcpsocket6 tcpSocket { get; set; }
        }

        public class Exec6
        {
            public string[] command { get; set; }
        }

        public class Httpget6
        {
            public string host { get; set; }
            public Httpheader6[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader6
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket6
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Livenessprobe1
        {
            public Exec7 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget7 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket7 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec7
        {
            public string[] command { get; set; }
        }

        public class Httpget7
        {
            public string host { get; set; }
            public Httpheader7[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader7
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket7
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Readinessprobe1
        {
            public Exec8 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget8 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket8 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec8
        {
            public string[] command { get; set; }
        }

        public class Httpget8
        {
            public string host { get; set; }
            public Httpheader8[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader8
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket8
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Resources1
        {
            public Limits1 limits { get; set; }
            public Requests1 requests { get; set; }
        }

        public class Limits1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext2
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities1 capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions2 seLinuxOptions { get; set; }
            public Windowsoptions2 windowsOptions { get; set; }
        }

        public class Capabilities1
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions2
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions2
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe1
        {
            public Exec9 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget9 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket9 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec9
        {
            public string[] command { get; set; }
        }

        public class Httpget9
        {
            public string host { get; set; }
            public Httpheader9[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader9
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket9
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Env1
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom1 valueFrom { get; set; }
        }

        public class Valuefrom1
        {
            public Configmapkeyref1 configMapKeyRef { get; set; }
            public Fieldref1 fieldRef { get; set; }
            public Resourcefieldref1 resourceFieldRef { get; set; }
            public Secretkeyref1 secretKeyRef { get; set; }
        }

        public class Configmapkeyref1
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref1
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref1
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref1
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom1
        {
            public Configmapref1 configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref1 secretRef { get; set; }
        }

        public class Configmapref1
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref1
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port1
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice1
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount1
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Hostalias
        {
            public string[] hostnames { get; set; }
            public string ip { get; set; }
        }

        public class Imagepullsecret
        {
            public string name { get; set; }
        }

        public class Initcontainer
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env2[] env { get; set; }
            public Envfrom2[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle2 lifecycle { get; set; }
            public Livenessprobe2 livenessProbe { get; set; }
            public string name { get; set; }
            public Port2[] ports { get; set; }
            public Readinessprobe2 readinessProbe { get; set; }
            public Resources2 resources { get; set; }
            public Securitycontext3 securityContext { get; set; }
            public Startupprobe2 startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice2[] volumeDevices { get; set; }
            public Volumemount2[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle2
        {
            public Poststart2 postStart { get; set; }
            public Prestop2 preStop { get; set; }
        }

        public class Poststart2
        {
            public Exec10 exec { get; set; }
            public Httpget10 httpGet { get; set; }
            public Tcpsocket10 tcpSocket { get; set; }
        }

        public class Exec10
        {
            public string[] command { get; set; }
        }

        public class Httpget10
        {
            public string host { get; set; }
            public Httpheader10[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader10
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket10
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Prestop2
        {
            public Exec11 exec { get; set; }
            public Httpget11 httpGet { get; set; }
            public Tcpsocket11 tcpSocket { get; set; }
        }

        public class Exec11
        {
            public string[] command { get; set; }
        }

        public class Httpget11
        {
            public string host { get; set; }
            public Httpheader11[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader11
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket11
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Livenessprobe2
        {
            public Exec12 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget12 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket12 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec12
        {
            public string[] command { get; set; }
        }

        public class Httpget12
        {
            public string host { get; set; }
            public Httpheader12[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader12
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket12
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Readinessprobe2
        {
            public Exec13 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget13 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket13 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec13
        {
            public string[] command { get; set; }
        }

        public class Httpget13
        {
            public string host { get; set; }
            public Httpheader13[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader13
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket13
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Resources2
        {
            public Limits2 limits { get; set; }
            public Requests2 requests { get; set; }
        }

        public class Limits2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext3
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities2 capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions3 seLinuxOptions { get; set; }
            public Windowsoptions3 windowsOptions { get; set; }
        }

        public class Capabilities2
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions3
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions3
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe2
        {
            public Exec14 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget14 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket14 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec14
        {
            public string[] command { get; set; }
        }

        public class Httpget14
        {
            public string host { get; set; }
            public Httpheader14[] httpHeaders { get; set; }
            public string path { get; set; }
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader14
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket14
        {
            public string host { get; set; }
            public string port { get; set; }
        }

        public class Env2
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom2 valueFrom { get; set; }
        }

        public class Valuefrom2
        {
            public Configmapkeyref2 configMapKeyRef { get; set; }
            public Fieldref2 fieldRef { get; set; }
            public Resourcefieldref2 resourceFieldRef { get; set; }
            public Secretkeyref2 secretKeyRef { get; set; }
        }

        public class Configmapkeyref2
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref2
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref2
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref2
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom2
        {
            public Configmapref2 configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref2 secretRef { get; set; }
        }

        public class Configmapref2
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref2
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port2
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice2
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount2
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Readinessgate
        {
            public string conditionType { get; set; }
        }

        public class Toleration
        {
            public string effect { get; set; }
            public string key { get; set; }
            public string _operator { get; set; }
            public int tolerationSeconds { get; set; }
            public string value { get; set; }
        }

        public class Topologyspreadconstraint
        {
            public Labelselector4 labelSelector { get; set; }
            public int maxSkew { get; set; }
            public string topologyKey { get; set; }
            public string whenUnsatisfiable { get; set; }
        }

        public class Labelselector4
        {
            public Matchexpression7[] matchExpressions { get; set; }
            public Matchlabels5 matchLabels { get; set; }
        }

        public class Matchlabels5
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression7
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Volume
        {
            public Awselasticblockstore awsElasticBlockStore { get; set; }
            public Azuredisk azureDisk { get; set; }
            public Azurefile azureFile { get; set; }
            public Cephfs cephfs { get; set; }
            public Cinder cinder { get; set; }
            public Configmap configMap { get; set; }
            public Csi csi { get; set; }
            public Downwardapi downwardAPI { get; set; }
            public Emptydir emptyDir { get; set; }
            public Fc fc { get; set; }
            public Flexvolume flexVolume { get; set; }
            public Flocker flocker { get; set; }
            public Gcepersistentdisk gcePersistentDisk { get; set; }
            public Gitrepo gitRepo { get; set; }
            public Glusterfs glusterfs { get; set; }
            public Hostpath hostPath { get; set; }
            public Iscsi iscsi { get; set; }
            public string name { get; set; }
            public Nfs nfs { get; set; }
            public Persistentvolumeclaim persistentVolumeClaim { get; set; }
            public Photonpersistentdisk photonPersistentDisk { get; set; }
            public Portworxvolume portworxVolume { get; set; }
            public Projected projected { get; set; }
            public Quobyte quobyte { get; set; }
            public Rbd rbd { get; set; }
            public Scaleio scaleIO { get; set; }
            public Secret1 secret { get; set; }
            public Storageos storageos { get; set; }
            public Vspherevolume vsphereVolume { get; set; }
        }

        public class Awselasticblockstore
        {
            public string fsType { get; set; }
            public int partition { get; set; }
            public bool readOnly { get; set; }
            public string volumeID { get; set; }
        }

        public class Azuredisk
        {
            public string cachingMode { get; set; }
            public string diskName { get; set; }
            public string diskURI { get; set; }
            public string fsType { get; set; }
            public string kind { get; set; }
            public bool readOnly { get; set; }
        }

        public class Azurefile
        {
            public bool readOnly { get; set; }
            public string secretName { get; set; }
            public string shareName { get; set; }
        }

        public class Cephfs
        {
            public string[] monitors { get; set; }
            public string path { get; set; }
            public bool readOnly { get; set; }
            public string secretFile { get; set; }
            public Secretref3 secretRef { get; set; }
            public string user { get; set; }
        }

        public class Secretref3
        {
            public string name { get; set; }
        }

        public class Cinder
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public Secretref4 secretRef { get; set; }
            public string volumeID { get; set; }
        }

        public class Secretref4
        {
            public string name { get; set; }
        }

        public class Configmap
        {
            public int defaultMode { get; set; }
            public Item[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Csi
        {
            public string driver { get; set; }
            public string fsType { get; set; }
            public Nodepublishsecretref nodePublishSecretRef { get; set; }
            public bool readOnly { get; set; }
            public Volumeattributes volumeAttributes { get; set; }
        }

        public class Nodepublishsecretref
        {
            public string name { get; set; }
        }

        public class Volumeattributes
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Downwardapi
        {
            public int defaultMode { get; set; }
            public Item1[] items { get; set; }
        }

        public class Item1
        {
            public Fieldref3 fieldRef { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
            public Resourcefieldref3 resourceFieldRef { get; set; }
        }

        public class Fieldref3
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref3
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Emptydir
        {
            public string medium { get; set; }
            public string sizeLimit { get; set; }
        }

        public class Fc
        {
            public string fsType { get; set; }
            public int lun { get; set; }
            public bool readOnly { get; set; }
            public string[] targetWWNs { get; set; }
            public string[] wwids { get; set; }
        }

        public class Flexvolume
        {
            public string driver { get; set; }
            public string fsType { get; set; }
            public Options options { get; set; }
            public bool readOnly { get; set; }
            public Secretref5 secretRef { get; set; }
        }

        public class Options
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Secretref5
        {
            public string name { get; set; }
        }

        public class Flocker
        {
            public string datasetName { get; set; }
            public string datasetUUID { get; set; }
        }

        public class Gcepersistentdisk
        {
            public string fsType { get; set; }
            public int partition { get; set; }
            public string pdName { get; set; }
            public bool readOnly { get; set; }
        }

        public class Gitrepo
        {
            public string directory { get; set; }
            public string repository { get; set; }
            public string revision { get; set; }
        }

        public class Glusterfs
        {
            public string endpoints { get; set; }
            public string path { get; set; }
            public bool readOnly { get; set; }
        }

        public class Hostpath
        {
            public string path { get; set; }
            public string type { get; set; }
        }

        public class Iscsi
        {
            public bool chapAuthDiscovery { get; set; }
            public bool chapAuthSession { get; set; }
            public string fsType { get; set; }
            public string initiatorName { get; set; }
            public string iqn { get; set; }
            public string iscsiInterface { get; set; }
            public int lun { get; set; }
            public string[] portals { get; set; }
            public bool readOnly { get; set; }
            public Secretref6 secretRef { get; set; }
            public string targetPortal { get; set; }
        }

        public class Secretref6
        {
            public string name { get; set; }
        }

        public class Nfs
        {
            public string path { get; set; }
            public bool readOnly { get; set; }
            public string server { get; set; }
        }

        public class Persistentvolumeclaim
        {
            public string claimName { get; set; }
            public bool readOnly { get; set; }
        }

        public class Photonpersistentdisk
        {
            public string fsType { get; set; }
            public string pdID { get; set; }
        }

        public class Portworxvolume
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public string volumeID { get; set; }
        }

        public class Projected
        {
            public int defaultMode { get; set; }
            public Source[] sources { get; set; }
        }

        public class Source
        {
            public Configmap1 configMap { get; set; }
            public Downwardapi1 downwardAPI { get; set; }
            public Secret secret { get; set; }
            public Serviceaccounttoken serviceAccountToken { get; set; }
        }

        public class Configmap1
        {
            public Item2[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item2
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Downwardapi1
        {
            public Item3[] items { get; set; }
        }

        public class Item3
        {
            public Fieldref4 fieldRef { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
            public Resourcefieldref4 resourceFieldRef { get; set; }
        }

        public class Fieldref4
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref4
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secret
        {
            public Item4[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item4
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Serviceaccounttoken
        {
            public string audience { get; set; }
            public int expirationSeconds { get; set; }
            public string path { get; set; }
        }

        public class Quobyte
        {
            public string group { get; set; }
            public bool readOnly { get; set; }
            public string registry { get; set; }
            public string tenant { get; set; }
            public string user { get; set; }
            public string volume { get; set; }
        }

        public class Rbd
        {
            public string fsType { get; set; }
            public string image { get; set; }
            public string keyring { get; set; }
            public string[] monitors { get; set; }
            public string pool { get; set; }
            public bool readOnly { get; set; }
            public Secretref7 secretRef { get; set; }
            public string user { get; set; }
        }

        public class Secretref7
        {
            public string name { get; set; }
        }

        public class Scaleio
        {
            public string fsType { get; set; }
            public string gateway { get; set; }
            public string protectionDomain { get; set; }
            public bool readOnly { get; set; }
            public Secretref8 secretRef { get; set; }
            public bool sslEnabled { get; set; }
            public string storageMode { get; set; }
            public string storagePool { get; set; }
            public string system { get; set; }
            public string volumeName { get; set; }
        }

        public class Secretref8
        {
            public string name { get; set; }
        }

        public class Secret1
        {
            public int defaultMode { get; set; }
            public Item5[] items { get; set; }
            public bool optional { get; set; }
            public string secretName { get; set; }
        }

        public class Item5
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Storageos
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public Secretref9 secretRef { get; set; }
            public string volumeName { get; set; }
            public string volumeNamespace { get; set; }
        }

        public class Secretref9
        {
            public string name { get; set; }
        }

        public class Vspherevolume
        {
            public string fsType { get; set; }
            public string storagePolicyID { get; set; }
            public string storagePolicyName { get; set; }
            public string volumePath { get; set; }
        }

        public class Status
        {
            public int availableReplicas { get; set; }
            public int collisionCount { get; set; }
            public Condition[] conditions { get; set; }
            public int observedGeneration { get; set; }
            public int readyReplicas { get; set; }
            public int replicas { get; set; }
            public int unavailableReplicas { get; set; }
            public int updatedReplicas { get; set; }
        }

        public class Condition
        {
            public DateTime lastTransitionTime { get; set; }
            public DateTime lastUpdateTime { get; set; }
            public string message { get; set; }
            public string reason { get; set; }
            public string status { get; set; }
            public string type { get; set; }
        }
    }


    public class KubernetesDeployments
    {
        public string apiVersion { get; set; }
        public Item[] items { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public class Metadata
        {
            public string _continue { get; set; }
            public int remainingItemCount { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
        }

        public class Item
        {
            public string apiVersion { get; set; }
            public string kind { get; set; }
            public Metadata1 metadata { get; set; }
            public Spec spec { get; set; }
            public Status status { get; set; }
        }

        public class Metadata1
        {
            public Annotations annotations { get; set; }
            public string clusterName { get; set; }
            public DateTime? creationTimestamp { get; set; }
            public int deletionGracePeriodSeconds { get; set; }
            public DateTime? deletionTimestamp { get; set; }
            public string[] finalizers { get; set; }
            public string generateName { get; set; }
            public int generation { get; set; }
            public Labels labels { get; set; }
            public Managedfield[] managedFields { get; set; }
            public string name { get; set; }
            public string _namespace { get; set; }
            public Ownerreference[] ownerReferences { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
            public string uid { get; set; }
        }

        public class Annotations
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Labels
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Managedfield
        {
            public string apiVersion { get; set; }
            public string fieldsType { get; set; }
            public Fieldsv1 fieldsV1 { get; set; }
            public string manager { get; set; }
            public string operation { get; set; }
            public DateTime time { get; set; }
        }

        public class Fieldsv1
        {
        }

        public class Ownerreference
        {
            public string apiVersion { get; set; }
            public bool blockOwnerDeletion { get; set; }
            public bool controller { get; set; }
            public string kind { get; set; }
            public string name { get; set; }
            public string uid { get; set; }
        }

        public class Spec
        {
            public int minReadySeconds { get; set; }
            public bool paused { get; set; }
            public int progressDeadlineSeconds { get; set; }
            public int replicas { get; set; }
            public int revisionHistoryLimit { get; set; }
            public Selector selector { get; set; }
            public Strategy strategy { get; set; }
            public Template template { get; set; }
        }

        public class Selector
        {
            public Matchexpression[] matchExpressions { get; set; }
            public Matchlabels matchLabels { get; set; }
        }

        public class Matchlabels
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Strategy
        {
            public Rollingupdate rollingUpdate { get; set; }
            public string type { get; set; }
        }

        public class Rollingupdate
        {
            public string maxSurge { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string maxUnavailable { get; set; }
        }

        public class Template
        {
            public Metadata2 metadata { get; set; }
            public Spec1 spec { get; set; }
        }

        public class Metadata2
        {
            public Annotations1 annotations { get; set; }
            public string clusterName { get; set; }
            public DateTime? creationTimestamp { get; set; }
            public int deletionGracePeriodSeconds { get; set; }
            public DateTime? deletionTimestamp { get; set; }
            public string[] finalizers { get; set; }
            public string generateName { get; set; }
            public int generation { get; set; }
            public Labels1 labels { get; set; }
            public Managedfield1[] managedFields { get; set; }
            public string name { get; set; }
            public string _namespace { get; set; }
            public Ownerreference1[] ownerReferences { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
            public string uid { get; set; }
        }

        public class Annotations1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Labels1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Managedfield1
        {
            public string apiVersion { get; set; }
            public string fieldsType { get; set; }
            public Fieldsv11 fieldsV1 { get; set; }
            public string manager { get; set; }
            public string operation { get; set; }
            public DateTime time { get; set; }
        }

        public class Fieldsv11
        {
        }

        public class Ownerreference1
        {
            public string apiVersion { get; set; }
            public bool blockOwnerDeletion { get; set; }
            public bool controller { get; set; }
            public string kind { get; set; }
            public string name { get; set; }
            public string uid { get; set; }
        }

        public class Spec1
        {
            public int activeDeadlineSeconds { get; set; }
            public Affinity affinity { get; set; }
            public bool automountServiceAccountToken { get; set; }
            public Container[] containers { get; set; }
            public Dnsconfig dnsConfig { get; set; }
            public string dnsPolicy { get; set; }
            public bool enableServiceLinks { get; set; }
            public Ephemeralcontainer[] ephemeralContainers { get; set; }
            public Hostalias[] hostAliases { get; set; }
            public bool hostIPC { get; set; }
            public bool hostNetwork { get; set; }
            public bool hostPID { get; set; }
            public string hostname { get; set; }
            public Imagepullsecret[] imagePullSecrets { get; set; }
            public Initcontainer[] initContainers { get; set; }
            public string nodeName { get; set; }
            public Nodeselector nodeSelector { get; set; }
            public Overhead overhead { get; set; }
            public string preemptionPolicy { get; set; }
            public int priority { get; set; }
            public string priorityClassName { get; set; }
            public Readinessgate[] readinessGates { get; set; }
            public string restartPolicy { get; set; }
            public string runtimeClassName { get; set; }
            public string schedulerName { get; set; }
            public Securitycontext securityContext { get; set; }
            public string serviceAccount { get; set; }
            public string serviceAccountName { get; set; }
            public bool shareProcessNamespace { get; set; }
            public string subdomain { get; set; }
            public int terminationGracePeriodSeconds { get; set; }
            public Toleration[] tolerations { get; set; }
            public Topologyspreadconstraint[] topologySpreadConstraints { get; set; }
            public Volume[] volumes { get; set; }
        }

        public class Affinity
        {
            public Nodeaffinity nodeAffinity { get; set; }
            public Podaffinity podAffinity { get; set; }
            public Podantiaffinity podAntiAffinity { get; set; }
        }

        public class Nodeaffinity
        {
            public Preferredduringschedulingignoredduringexecution[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution
        {
            public Nodeselectorterm[] nodeSelectorTerms { get; set; }
        }

        public class Nodeselectorterm
        {
            public Matchexpression1[] matchExpressions { get; set; }
            public Matchfield[] matchFields { get; set; }
        }

        public class Matchexpression1
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Matchfield
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution
        {
            public Preference preference { get; set; }
            public int weight { get; set; }
        }

        public class Preference
        {
            public Matchexpression2[] matchExpressions { get; set; }
            public Matchfield1[] matchFields { get; set; }
        }

        public class Matchexpression2
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Matchfield1
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Podaffinity
        {
            public Preferredduringschedulingignoredduringexecution1[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution1[] requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution1
        {
            public Podaffinityterm podAffinityTerm { get; set; }
            public int weight { get; set; }
        }

        public class Podaffinityterm
        {
            public Labelselector labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector
        {
            public Matchexpression3[] matchExpressions { get; set; }
            public Matchlabels1 matchLabels { get; set; }
        }

        public class Matchlabels1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression3
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution1
        {
            public Labelselector1 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector1
        {
            public Matchexpression4[] matchExpressions { get; set; }
            public Matchlabels2 matchLabels { get; set; }
        }

        public class Matchlabels2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression4
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Podantiaffinity
        {
            public Preferredduringschedulingignoredduringexecution2[] preferredDuringSchedulingIgnoredDuringExecution { get; set; }
            public Requiredduringschedulingignoredduringexecution2[] requiredDuringSchedulingIgnoredDuringExecution { get; set; }
        }

        public class Preferredduringschedulingignoredduringexecution2
        {
            public Podaffinityterm1 podAffinityTerm { get; set; }
            public int weight { get; set; }
        }

        public class Podaffinityterm1
        {
            public Labelselector2 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector2
        {
            public Matchexpression5[] matchExpressions { get; set; }
            public Matchlabels3 matchLabels { get; set; }
        }

        public class Matchlabels3
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression5
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Requiredduringschedulingignoredduringexecution2
        {
            public Labelselector3 labelSelector { get; set; }
            public string[] namespaces { get; set; }
            public string topologyKey { get; set; }
        }

        public class Labelselector3
        {
            public Matchexpression6[] matchExpressions { get; set; }
            public Matchlabels4 matchLabels { get; set; }
        }

        public class Matchlabels4
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression6
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Dnsconfig
        {
            public string[] nameservers { get; set; }
            public Option[] options { get; set; }
            public string[] searches { get; set; }
        }

        public class Option
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Nodeselector
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Overhead
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext
        {
            public int fsGroup { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions seLinuxOptions { get; set; }
            public int[] supplementalGroups { get; set; }
            public Sysctl[] sysctls { get; set; }
            public Windowsoptions windowsOptions { get; set; }
        }

        public class Selinuxoptions
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Sysctl
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Container
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env[] env { get; set; }
            public Envfrom[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle lifecycle { get; set; }
            public Livenessprobe livenessProbe { get; set; }
            public string name { get; set; }
            public Port[] ports { get; set; }
            public Readinessprobe readinessProbe { get; set; }
            public Resources resources { get; set; }
            public Securitycontext1 securityContext { get; set; }
            public Startupprobe startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice[] volumeDevices { get; set; }
            public Volumemount[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle
        {
            public Poststart postStart { get; set; }
            public Prestop preStop { get; set; }
        }

        public class Poststart
        {
            public Exec exec { get; set; }
            public Httpget httpGet { get; set; }
            public Tcpsocket tcpSocket { get; set; }
        }

        public class Exec
        {
            public string[] command { get; set; }
        }

        public class Httpget
        {
            public string host { get; set; }
            public Httpheader[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Prestop
        {
            public Exec1 exec { get; set; }
            public Httpget1 httpGet { get; set; }
            public Tcpsocket1 tcpSocket { get; set; }
        }

        public class Exec1
        {
            public string[] command { get; set; }
        }

        public class Httpget1
        {
            public string host { get; set; }
            public Httpheader1[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader1
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket1
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Livenessprobe
        {
            public Exec2 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget2 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket2 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec2
        {
            public string[] command { get; set; }
        }

        public class Httpget2
        {
            public string host { get; set; }
            public Httpheader2[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader2
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket2
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Readinessprobe
        {
            public Exec3 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget3 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket3 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec3
        {
            public string[] command { get; set; }
        }

        public class Httpget3
        {
            public string host { get; set; }
            public Httpheader3[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader3
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket3
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Resources
        {
            public Limits limits { get; set; }
            public Requests requests { get; set; }
        }

        public class Limits
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext1
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions1 seLinuxOptions { get; set; }
            public Windowsoptions1 windowsOptions { get; set; }
        }

        public class Capabilities
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions1
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions1
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe
        {
            public Exec4 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget4 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket4 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec4
        {
            public string[] command { get; set; }
        }

        public class Httpget4
        {
            public string host { get; set; }
            public Httpheader4[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader4
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket4
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Env
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom valueFrom { get; set; }
        }

        public class Valuefrom
        {
            public Configmapkeyref configMapKeyRef { get; set; }
            public Fieldref fieldRef { get; set; }
            public Resourcefieldref resourceFieldRef { get; set; }
            public Secretkeyref secretKeyRef { get; set; }
        }

        public class Configmapkeyref
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom
        {
            public Configmapref configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref secretRef { get; set; }
        }

        public class Configmapref
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Ephemeralcontainer
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env1[] env { get; set; }
            public Envfrom1[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle1 lifecycle { get; set; }
            public Livenessprobe1 livenessProbe { get; set; }
            public string name { get; set; }
            public Port1[] ports { get; set; }
            public Readinessprobe1 readinessProbe { get; set; }
            public Resources1 resources { get; set; }
            public Securitycontext2 securityContext { get; set; }
            public Startupprobe1 startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string targetContainerName { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice1[] volumeDevices { get; set; }
            public Volumemount1[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle1
        {
            public Poststart1 postStart { get; set; }
            public Prestop1 preStop { get; set; }
        }

        public class Poststart1
        {
            public Exec5 exec { get; set; }
            public Httpget5 httpGet { get; set; }
            public Tcpsocket5 tcpSocket { get; set; }
        }

        public class Exec5
        {
            public string[] command { get; set; }
        }

        public class Httpget5
        {
            public string host { get; set; }
            public Httpheader5[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader5
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket5
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Prestop1
        {
            public Exec6 exec { get; set; }
            public Httpget6 httpGet { get; set; }
            public Tcpsocket6 tcpSocket { get; set; }
        }

        public class Exec6
        {
            public string[] command { get; set; }
        }

        public class Httpget6
        {
            public string host { get; set; }
            public Httpheader6[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader6
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket6
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Livenessprobe1
        {
            public Exec7 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget7 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket7 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec7
        {
            public string[] command { get; set; }
        }

        public class Httpget7
        {
            public string host { get; set; }
            public Httpheader7[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader7
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket7
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Readinessprobe1
        {
            public Exec8 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget8 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket8 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec8
        {
            public string[] command { get; set; }
        }

        public class Httpget8
        {
            public string host { get; set; }
            public Httpheader8[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader8
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket8
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Resources1
        {
            public Limits1 limits { get; set; }
            public Requests1 requests { get; set; }
        }

        public class Limits1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests1
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext2
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities1 capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions2 seLinuxOptions { get; set; }
            public Windowsoptions2 windowsOptions { get; set; }
        }

        public class Capabilities1
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions2
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions2
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe1
        {
            public Exec9 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget9 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket9 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec9
        {
            public string[] command { get; set; }
        }

        public class Httpget9
        {
            public string host { get; set; }
            public Httpheader9[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader9
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket9
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Env1
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom1 valueFrom { get; set; }
        }

        public class Valuefrom1
        {
            public Configmapkeyref1 configMapKeyRef { get; set; }
            public Fieldref1 fieldRef { get; set; }
            public Resourcefieldref1 resourceFieldRef { get; set; }
            public Secretkeyref1 secretKeyRef { get; set; }
        }

        public class Configmapkeyref1
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref1
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref1
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref1
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom1
        {
            public Configmapref1 configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref1 secretRef { get; set; }
        }

        public class Configmapref1
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref1
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port1
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice1
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount1
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Hostalias
        {
            public string[] hostnames { get; set; }
            public string ip { get; set; }
        }

        public class Imagepullsecret
        {
            public string name { get; set; }
        }

        public class Initcontainer
        {
            public string[] args { get; set; }
            public string[] command { get; set; }
            public Env2[] env { get; set; }
            public Envfrom2[] envFrom { get; set; }
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public Lifecycle2 lifecycle { get; set; }
            public Livenessprobe2 livenessProbe { get; set; }
            public string name { get; set; }
            public Port2[] ports { get; set; }
            public Readinessprobe2 readinessProbe { get; set; }
            public Resources2 resources { get; set; }
            public Securitycontext3 securityContext { get; set; }
            public Startupprobe2 startupProbe { get; set; }
            public bool stdin { get; set; }
            public bool stdinOnce { get; set; }
            public string terminationMessagePath { get; set; }
            public string terminationMessagePolicy { get; set; }
            public bool tty { get; set; }
            public Volumedevice2[] volumeDevices { get; set; }
            public Volumemount2[] volumeMounts { get; set; }
            public string workingDir { get; set; }
        }

        public class Lifecycle2
        {
            public Poststart2 postStart { get; set; }
            public Prestop2 preStop { get; set; }
        }

        public class Poststart2
        {
            public Exec10 exec { get; set; }
            public Httpget10 httpGet { get; set; }
            public Tcpsocket10 tcpSocket { get; set; }
        }

        public class Exec10
        {
            public string[] command { get; set; }
        }

        public class Httpget10
        {
            public string host { get; set; }
            public Httpheader10[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader10
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket10
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Prestop2
        {
            public Exec11 exec { get; set; }
            public Httpget11 httpGet { get; set; }
            public Tcpsocket11 tcpSocket { get; set; }
        }

        public class Exec11
        {
            public string[] command { get; set; }
        }

        public class Httpget11
        {
            public string host { get; set; }
            public Httpheader11[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader11
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket11
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Livenessprobe2
        {
            public Exec12 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget12 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket12 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec12
        {
            public string[] command { get; set; }
        }

        public class Httpget12
        {
            public string host { get; set; }
            public Httpheader12[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader12
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket12
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Readinessprobe2
        {
            public Exec13 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget13 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket13 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec13
        {
            public string[] command { get; set; }
        }

        public class Httpget13
        {
            public string host { get; set; }
            public Httpheader13[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader13
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket13
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Resources2
        {
            public Limits2 limits { get; set; }
            public Requests2 requests { get; set; }
        }

        public class Limits2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Requests2
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Securitycontext3
        {
            public bool allowPrivilegeEscalation { get; set; }
            public Capabilities2 capabilities { get; set; }
            public bool privileged { get; set; }
            public string procMount { get; set; }
            public bool readOnlyRootFilesystem { get; set; }
            public int runAsGroup { get; set; }
            public bool runAsNonRoot { get; set; }
            public int runAsUser { get; set; }
            public Selinuxoptions3 seLinuxOptions { get; set; }
            public Windowsoptions3 windowsOptions { get; set; }
        }

        public class Capabilities2
        {
            public string[] add { get; set; }
            public string[] drop { get; set; }
        }

        public class Selinuxoptions3
        {
            public string level { get; set; }
            public string role { get; set; }
            public string type { get; set; }
            public string user { get; set; }
        }

        public class Windowsoptions3
        {
            public string gmsaCredentialSpec { get; set; }
            public string gmsaCredentialSpecName { get; set; }
            public string runAsUserName { get; set; }
        }

        public class Startupprobe2
        {
            public Exec14 exec { get; set; }
            public int failureThreshold { get; set; }
            public Httpget14 httpGet { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
            public int successThreshold { get; set; }
            public Tcpsocket14 tcpSocket { get; set; }
            public int timeoutSeconds { get; set; }
        }

        public class Exec14
        {
            public string[] command { get; set; }
        }

        public class Httpget14
        {
            public string host { get; set; }
            public Httpheader14[] httpHeaders { get; set; }
            public string path { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
            public string scheme { get; set; }
        }

        public class Httpheader14
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Tcpsocket14
        {
            public string host { get; set; }
            [JsonConverter(typeof(StringConverter))]
            public string port { get; set; }
        }

        public class Env2
        {
            public string name { get; set; }
            public string value { get; set; }
            public Valuefrom2 valueFrom { get; set; }
        }

        public class Valuefrom2
        {
            public Configmapkeyref2 configMapKeyRef { get; set; }
            public Fieldref2 fieldRef { get; set; }
            public Resourcefieldref2 resourceFieldRef { get; set; }
            public Secretkeyref2 secretKeyRef { get; set; }
        }

        public class Configmapkeyref2
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Fieldref2
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref2
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secretkeyref2
        {
            public string key { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Envfrom2
        {
            public Configmapref2 configMapRef { get; set; }
            public string prefix { get; set; }
            public Secretref2 secretRef { get; set; }
        }

        public class Configmapref2
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Secretref2
        {
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Port2
        {
            public int containerPort { get; set; }
            public string hostIP { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string protocol { get; set; }
        }

        public class Volumedevice2
        {
            public string devicePath { get; set; }
            public string name { get; set; }
        }

        public class Volumemount2
        {
            public string mountPath { get; set; }
            public string mountPropagation { get; set; }
            public string name { get; set; }
            public bool readOnly { get; set; }
            public string subPath { get; set; }
            public string subPathExpr { get; set; }
        }

        public class Readinessgate
        {
            public string conditionType { get; set; }
        }

        public class Toleration
        {
            public string effect { get; set; }
            public string key { get; set; }
            public string _operator { get; set; }
            public int tolerationSeconds { get; set; }
            public string value { get; set; }
        }

        public class Topologyspreadconstraint
        {
            public Labelselector4 labelSelector { get; set; }
            public int maxSkew { get; set; }
            public string topologyKey { get; set; }
            public string whenUnsatisfiable { get; set; }
        }

        public class Labelselector4
        {
            public Matchexpression7[] matchExpressions { get; set; }
            public Matchlabels5 matchLabels { get; set; }
        }

        public class Matchlabels5
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Matchexpression7
        {
            public string key { get; set; }
            public string _operator { get; set; }
            public string[] values { get; set; }
        }

        public class Volume
        {
            public Awselasticblockstore awsElasticBlockStore { get; set; }
            public Azuredisk azureDisk { get; set; }
            public Azurefile azureFile { get; set; }
            public Cephfs cephfs { get; set; }
            public Cinder cinder { get; set; }
            public Configmap configMap { get; set; }
            public Csi csi { get; set; }
            public Downwardapi downwardAPI { get; set; }
            public Emptydir emptyDir { get; set; }
            public Fc fc { get; set; }
            public Flexvolume flexVolume { get; set; }
            public Flocker flocker { get; set; }
            public Gcepersistentdisk gcePersistentDisk { get; set; }
            public Gitrepo gitRepo { get; set; }
            public Glusterfs glusterfs { get; set; }
            public Hostpath hostPath { get; set; }
            public Iscsi iscsi { get; set; }
            public string name { get; set; }
            public Nfs nfs { get; set; }
            public Persistentvolumeclaim persistentVolumeClaim { get; set; }
            public Photonpersistentdisk photonPersistentDisk { get; set; }
            public Portworxvolume portworxVolume { get; set; }
            public Projected projected { get; set; }
            public Quobyte quobyte { get; set; }
            public Rbd rbd { get; set; }
            public Scaleio scaleIO { get; set; }
            public Secret1 secret { get; set; }
            public Storageos storageos { get; set; }
            public Vspherevolume vsphereVolume { get; set; }
        }

        public class Awselasticblockstore
        {
            public string fsType { get; set; }
            public int partition { get; set; }
            public bool readOnly { get; set; }
            public string volumeID { get; set; }
        }

        public class Azuredisk
        {
            public string cachingMode { get; set; }
            public string diskName { get; set; }
            public string diskURI { get; set; }
            public string fsType { get; set; }
            public string kind { get; set; }
            public bool readOnly { get; set; }
        }

        public class Azurefile
        {
            public bool readOnly { get; set; }
            public string secretName { get; set; }
            public string shareName { get; set; }
        }

        public class Cephfs
        {
            public string[] monitors { get; set; }
            public string path { get; set; }
            public bool readOnly { get; set; }
            public string secretFile { get; set; }
            public Secretref3 secretRef { get; set; }
            public string user { get; set; }
        }

        public class Secretref3
        {
            public string name { get; set; }
        }

        public class Cinder
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public Secretref4 secretRef { get; set; }
            public string volumeID { get; set; }
        }

        public class Secretref4
        {
            public string name { get; set; }
        }

        public class Configmap
        {
            public int defaultMode { get; set; }
            public Item1[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item1
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Csi
        {
            public string driver { get; set; }
            public string fsType { get; set; }
            public Nodepublishsecretref nodePublishSecretRef { get; set; }
            public bool readOnly { get; set; }
            public Volumeattributes volumeAttributes { get; set; }
        }

        public class Nodepublishsecretref
        {
            public string name { get; set; }
        }

        public class Volumeattributes
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Downwardapi
        {
            public int defaultMode { get; set; }
            public Item2[] items { get; set; }
        }

        public class Item2
        {
            public Fieldref3 fieldRef { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
            public Resourcefieldref3 resourceFieldRef { get; set; }
        }

        public class Fieldref3
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref3
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Emptydir
        {
            public string medium { get; set; }
            public string sizeLimit { get; set; }
        }

        public class Fc
        {
            public string fsType { get; set; }
            public int lun { get; set; }
            public bool readOnly { get; set; }
            public string[] targetWWNs { get; set; }
            public string[] wwids { get; set; }
        }

        public class Flexvolume
        {
            public string driver { get; set; }
            public string fsType { get; set; }
            public Options options { get; set; }
            public bool readOnly { get; set; }
            public Secretref5 secretRef { get; set; }
        }

        public class Options
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Secretref5
        {
            public string name { get; set; }
        }

        public class Flocker
        {
            public string datasetName { get; set; }
            public string datasetUUID { get; set; }
        }

        public class Gcepersistentdisk
        {
            public string fsType { get; set; }
            public int partition { get; set; }
            public string pdName { get; set; }
            public bool readOnly { get; set; }
        }

        public class Gitrepo
        {
            public string directory { get; set; }
            public string repository { get; set; }
            public string revision { get; set; }
        }

        public class Glusterfs
        {
            public string endpoints { get; set; }
            public string path { get; set; }
            public bool readOnly { get; set; }
        }

        public class Hostpath
        {
            public string path { get; set; }
            public string type { get; set; }
        }

        public class Iscsi
        {
            public bool chapAuthDiscovery { get; set; }
            public bool chapAuthSession { get; set; }
            public string fsType { get; set; }
            public string initiatorName { get; set; }
            public string iqn { get; set; }
            public string iscsiInterface { get; set; }
            public int lun { get; set; }
            public string[] portals { get; set; }
            public bool readOnly { get; set; }
            public Secretref6 secretRef { get; set; }
            public string targetPortal { get; set; }
        }

        public class Secretref6
        {
            public string name { get; set; }
        }

        public class Nfs
        {
            public string path { get; set; }
            public bool readOnly { get; set; }
            public string server { get; set; }
        }

        public class Persistentvolumeclaim
        {
            public string claimName { get; set; }
            public bool readOnly { get; set; }
        }

        public class Photonpersistentdisk
        {
            public string fsType { get; set; }
            public string pdID { get; set; }
        }

        public class Portworxvolume
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public string volumeID { get; set; }
        }

        public class Projected
        {
            public int defaultMode { get; set; }
            public Source[] sources { get; set; }
        }

        public class Source
        {
            public Configmap1 configMap { get; set; }
            public Downwardapi1 downwardAPI { get; set; }
            public Secret secret { get; set; }
            public Serviceaccounttoken serviceAccountToken { get; set; }
        }

        public class Configmap1
        {
            public Item3[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item3
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Downwardapi1
        {
            public Item4[] items { get; set; }
        }

        public class Item4
        {
            public Fieldref4 fieldRef { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
            public Resourcefieldref4 resourceFieldRef { get; set; }
        }

        public class Fieldref4
        {
            public string apiVersion { get; set; }
            public string fieldPath { get; set; }
        }

        public class Resourcefieldref4
        {
            public string containerName { get; set; }
            public string divisor { get; set; }
            public string resource { get; set; }
        }

        public class Secret
        {
            public Item5[] items { get; set; }
            public string name { get; set; }
            public bool optional { get; set; }
        }

        public class Item5
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Serviceaccounttoken
        {
            public string audience { get; set; }
            public int expirationSeconds { get; set; }
            public string path { get; set; }
        }

        public class Quobyte
        {
            public string group { get; set; }
            public bool readOnly { get; set; }
            public string registry { get; set; }
            public string tenant { get; set; }
            public string user { get; set; }
            public string volume { get; set; }
        }

        public class Rbd
        {
            public string fsType { get; set; }
            public string image { get; set; }
            public string keyring { get; set; }
            public string[] monitors { get; set; }
            public string pool { get; set; }
            public bool readOnly { get; set; }
            public Secretref7 secretRef { get; set; }
            public string user { get; set; }
        }

        public class Secretref7
        {
            public string name { get; set; }
        }

        public class Scaleio
        {
            public string fsType { get; set; }
            public string gateway { get; set; }
            public string protectionDomain { get; set; }
            public bool readOnly { get; set; }
            public Secretref8 secretRef { get; set; }
            public bool sslEnabled { get; set; }
            public string storageMode { get; set; }
            public string storagePool { get; set; }
            public string system { get; set; }
            public string volumeName { get; set; }
        }

        public class Secretref8
        {
            public string name { get; set; }
        }

        public class Secret1
        {
            public int defaultMode { get; set; }
            public Item6[] items { get; set; }
            public bool optional { get; set; }
            public string secretName { get; set; }
        }

        public class Item6
        {
            public string key { get; set; }
            public int mode { get; set; }
            public string path { get; set; }
        }

        public class Storageos
        {
            public string fsType { get; set; }
            public bool readOnly { get; set; }
            public Secretref9 secretRef { get; set; }
            public string volumeName { get; set; }
            public string volumeNamespace { get; set; }
        }

        public class Secretref9
        {
            public string name { get; set; }
        }

        public class Vspherevolume
        {
            public string fsType { get; set; }
            public string storagePolicyID { get; set; }
            public string storagePolicyName { get; set; }
            public string volumePath { get; set; }
        }

        public class Status
        {
            public int availableReplicas { get; set; }
            public int collisionCount { get; set; }
            public Condition[] conditions { get; set; }
            public int observedGeneration { get; set; }
            public int readyReplicas { get; set; }
            public int replicas { get; set; }
            public int unavailableReplicas { get; set; }
            public int updatedReplicas { get; set; }
        }

        public class Condition
        {
            public DateTime lastTransitionTime { get; set; }
            public DateTime lastUpdateTime { get; set; }
            public string message { get; set; }
            public string reason { get; set; }
            public string status { get; set; }
            public string type { get; set; }
        }

    }
}
