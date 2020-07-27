using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using KubernetesClient.Converters;

namespace KubernetesClient.Models
{
    public class V1ObjectMeta
    {
        public IDictionary<string, string> annotations { get; set; }
        public string clusterName { get; set; }
        public DateTime? creationTimestamp { get; set; }
        public long? deletionGracePeriodSeconds { get; set; }
        public DateTime? deletionTimestamp { get; set; }
        public IList<string> finalizers { get; set; }
        public string generateName { get; set; }
        public long? generation { get; set; }
        public IDictionary<string, string> labels { get; set; }
        public IList<V1ManagedFieldsEntry> managedFields { get; set; }
        public string name { get; set; }
        public string @namespace { get; set; }
        public IList<V1Ownerreference> ownerReferences { get; set; }
        public string resourceVersion { get; set; }
        public string selfLink { get; set; }
        public string uid { get; set; }
    }
    public class V1ListMeta
    {
        public string @continue { get; set; }
        public long? remainingItemCount { get; set; }
        public string resourceVersion { get; set; }
        public string selfLink { get; set; }
    }

    public class V1ManagedFieldsEntry
    {
        public string apiVersion { get; set; }
        public string fieldsType { get; set; }
        public object fieldsV1 { get; set; }
        public string manager { get; set; }
        public string operation { get; set; }
        public DateTime? time { get; set; }
    }

    public class V1Ownerreference
    {
        public string apiVersion { get; set; }
        public bool? blockOwnerDeletion { get; set; }
        public bool? controller { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public string uid { get; set; }
    }

    public class V1LabelSelector
    {
        public IList<V1LabelSelectorRequirement> matchExpressions { get; set; }
        public IDictionary<string, string> matchLabels { get; set; }
    }

    public class V1LabelSelectorRequirement
    {
        public string key { get; set; }
        public string @operator { get; set; }
        public IList<string> values { get; set; }
    }

    public class V1DeploymentStrategy
    {
        public V1RollingUpdateDeployment rollingUpdate { get; set; }
        public string type { get; set; }
    }
    public class V1RollingUpdateDeployment
    {
        [JsonConverter(typeof(IntOrStringConverter))]

        public string maxSurge { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string maxUnavailable { get; set; }
    }
    public class V1PodTemplateSpec
    {
        public V1ObjectMeta metadata { get; set; }
        public V1PodSpec spec { get; set; }
    }

    public class V1PodSpec
    {
        public long? activeDeadlineSeconds { get; set; }
        public V1Affinity affinity { get; set; }
        public bool? automountServiceAccountToken { get; set; }
        public IList<V1Container> containers { get; set; }
        public V1PodDNSConfig dnsConfig { get; set; }
        public string dnsPolicy { get; set; }
        public bool? enableServiceLinks { get; set; }
        public IList<V1EphemeralContainer> ephemeralContainers { get; set; }
        public IList<V1HostAlias> hostAliases { get; set; }
        public bool? hostIPC { get; set; }
        public bool? hostNetwork { get; set; }
        public bool? hostPID { get; set; }
        public string hostname { get; set; }
        public IList<V1LocalObjectReference> imagePullSecrets { get; set; }
        public IList<V1Container> initContainers { get; set; }
        public string nodeName { get; set; }
        public IDictionary<string, string> nodeSelector { get; set; }
        public IDictionary<string, ResourceQuantity> overhead { get; set; }
        public string preemptionPolicy { get; set; }
        public int? priority { get; set; }
        public string priorityClassName { get; set; }
        public IList<V1PodReadinessGate> readinessGates { get; set; }
        public string restartPolicy { get; set; }
        public string runtimeClassName { get; set; }
        public string schedulerName { get; set; }
        public V1PodSecurityContext securityContext { get; set; }
        public string serviceAccount { get; set; }
        public string serviceAccountName { get; set; }
        public bool? shareProcessNamespace { get; set; }
        public string subdomain { get; set; }
        public long? terminationGracePeriodSeconds { get; set; }
        public IList<V1Toleration> tolerations { get; set; }
        public IList<V1TopologySpreadConstraint> topologySpreadConstraints { get; set; }
        public IList<V1Volume> volumes { get; set; }
    }
    public class V1EphemeralContainer
    {
        public IList<string> args { get; set; }
        public IList<string> command { get; set; }
        public IList<V1EnvVar> env { get; set; }
        public IList<V1EnvFromSource> envFrom { get; set; }
        public string image { get; set; }
        public string imagePullPolicy { get; set; }
        public V1Lifecycle lifecycle { get; set; }
        public V1Probe livenessProbe { get; set; }
        public string name { get; set; }
        public IList<V1ContainerPort> ports { get; set; }
        public V1Probe readinessProbe { get; set; }
        public V1ResourceRequirements resources { get; set; }
        public V1SecurityContext securityContext { get; set; }
        public V1Probe startupProbe { get; set; }
        public bool? stdin { get; set; }
        public bool? stdinOnce { get; set; }
        public string targetContainerName { get; set; }
        public string terminationMessagePath { get; set; }
        public string terminationMessagePolicy { get; set; }
        public bool? tty { get; set; }
        public IList<V1VolumeDevice> volumeDevices { get; set; }
        public IList<V1VolumeMount> volumeMounts { get; set; }
        public string workingDir { get; set; }
    }
    public class V1HostAlias
    {
        public IList<string> hostnames { get; set; }
        public string ip { get; set; }
    }
    public class V1LocalObjectReference
    {
        public string name { get; set; }
    }
    public class V1PodReadinessGate
    {
        public string conditionType { get; set; }
    }
    public class V1Toleration
    {
        public string effect { get; set; }
        public string key { get; set; }
        public string @operator { get; set; }
        public long? tolerationSeconds { get; set; }
    }
    public class V1TopologySpreadConstraint
    {
        public V1LabelSelector labelSelector { get; set; }
        public int maxSkew { get; set; }
        public string topologyKey { get; set; }
        public string whenUnsatisfiable { get; set; }
    }

    public class V1Volume
    {
        public V1ConfigMapVolumeSource configMap { get; set; }
        public V1EmptyDirVolumeSource emptyDir { get; set; }
        public V1FCVolumeSource fc { get; set; }
        public V1HostPathVolumeSource hostPath { get; set; }
        public string name { get; set; }
        public V1NFSVolumeSource nfs { get; set; }
        public V1PersistentVolumeClaimVolumeSource persistentVolumeClaim { get; set; }
        public V1SecretVolumeSource secret { get; set; }
    }
    public class V1KeyToPath
    {
        public string key { get; set; }
        public int? mode { get; set; }
        public string path { get; set; }
    }
    public class V1ConfigMapVolumeSource
    {
        public int? defaultNode { get; set; }
        public IList<V1KeyToPath> Items { get; set; }
        public string name { get; set; }
    }
    public class V1CSIVolumeSource
    {
        public string driver { get; set; }
        public string fsType { get; set; }
        public V1LocalObjectReference nodePublishSecretRef { get; set; }
        public bool? readOnly { get; set; }
        public IDictionary<string, string> volumeAttributes { get; set; }
    }
    public class V1EmptyDirVolumeSource
    {
        public string medium { get; set; }
        public ResourceQuantity sizeLimit { get; set; }
    }
    public class V1FCVolumeSource
    {
        public string fsType { get; set; }
        public int? lun { get; set; }
        public bool? readOnly { get; set; }
        public IList<string> targetWWNs { get; set; }
        public IList<string> wwids { get; set; }
    }
    public class V1HostPathVolumeSource
    {
        public string path { get; set; }
        public string type { get; set; }
    }
    public class V1NFSVolumeSource
    {
        public string path { get; set; }
        public bool? readOnly { get; set; }
        public string server { get; set; }
    }
    public class V1PersistentVolumeClaimVolumeSource
    {
        public string claimName { get; set; }
        public bool? readOnly { get; set; }
    }
    public class V1SecretVolumeSource
    {
        public int? defaultMode { get; set; }
        public IList<V1KeyToPath> Items { get; set; }
        public bool? optional { get; set; }
        public string secretName { get; set; }
    }

    public class V1Affinity
    {
        public V1Nodeaffinity nodeAffinity { get; set; }
        public V1Podaffinity podAffinity { get; set; }
        public V1Podantiaffinity podAntiAffinity { get; set; }
    }

    public class V1Nodeaffinity
    {
        public IList<V1PreferredSchedulingTerm> preferredDuringSchedulingIgnoredDuringExecution { get; set; }
        public V1NodeSelector requiredDuringSchedulingIgnoredDuringExecution { get; set; }
    }

    public class V1PreferredSchedulingTerm
    {
        public V1NodeSelectorTerm preference { get; set; }
        public int weight { get; set; }
    }

    public class V1NodeSelector
    {
        public IList<V1NodeSelectorTerm> nodeSelectorTerms { get; set; }
    }
    public class V1NodeSelectorTerm
    {
        public IList<V1NodeSelectorRequirement> matchExpressions { get; set; }
        public IList<V1NodeSelectorRequirement> matchFields { get; set; }
    }

    public class V1NodeSelectorRequirement
    {
        public string key { get; set; }
        public string @operator { get; set; }
        public IList<string> values { get; set; }
    }

    public class V1Podaffinity
    {
        public IList<V1WeightedPodAffinityTerm> preferredDuringSchedulingIgnoredDuringExecution { get; set; }
        public IList<V1PodAffinityTerm> requiredDuringSchedulingIgnoredDuringExecution { get; set; }
    }
    public class V1Podantiaffinity
    {
        public IList<V1WeightedPodAffinityTerm> preferredDuringSchedulingIgnoredDuringExecution { get; set; }
        public IList<V1PodAffinityTerm> requiredDuringSchedulingIgnoredDuringExecution { get; set; }
    }

    public class V1PodAffinityTerm
    {
        public V1LabelSelector labelSelector { get; set; }
        public IList<string> namespaces { get; set; }
        public string topologyKey { get; set; }
    }
    public class V1WeightedPodAffinityTerm
    {
        public V1PodAffinityTerm podAffinityTerm { get; set; }
        public int weight { get; set; }
    }

    public class V1DeploymentSpec
    {
        public int? minReadySeconds { get; set; }
        public bool? paused { get; set; }
        public int? progressDeadlineSeconds { get; set; }
        public int? replicas { get; set; }
        public int? revisionHistoryLimit { get; set; }
        public V1LabelSelector selector { get; set; }
        public V1DeploymentStrategy strategy { get; set; }
        public V1PodTemplateSpec template { get; set; }
    }

    public class V1PodDNSConfig
    {
        public IList<string> nameservers { get; set; }
        public IList<V1PodDNSConfigOption> options { get; set; }
        public IList<string> searches { get; set; }
    }

    public class V1PodDNSConfigOption
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class V1PodSecurityContext
    {
        public long? fsGroup { get; set; }
        public string fsGroupChangePolicy { get; set; }
        public long? runAsGroup { get; set; }
        public bool? runAsNonRoot { get; set; }
        public long? runAsUser { get; set; }
        public V1SELinuxOptions seLinuxOptions { get; set; }
        public IList<long?> supplementalGroups { get; set; }
        public IList<V1Sysctl> sysctls { get; set; }
        public V1WindowsSecurityContextOptions windowsOptions { get; set; }
    }

    public class V1SELinuxOptions
    {
        public string level { get; set; }
        public string role { get; set; }
        public string type { get; set; }
        public string user { get; set; }
    }

    public class V1Sysctl
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class V1WindowsSecurityContextOptions
    {
        public string gmsaCredentialSpec { get; set; }
        public string gmsaCredentialSpecName { get; set; }
        public string runAsUserName { get; set; }
    }

    public class V1Container
    {
        public IList<string> args { get; set; }
        public IList<string> command { get; set; }
        public IList<V1EnvVar> env { get; set; }
        public IList<V1EnvFromSource> envFrom { get; set; }
        public string image { get; set; }
        public string imagePullPolicy { get; set; }
        public V1Lifecycle lifecycle { get; set; }
        public V1Probe livenessProbe { get; set; }
        public string name { get; set; }
        public IList<V1ContainerPort> ports { get; set; }
        public V1Probe readinessProbe { get; set; }
        public V1ResourceRequirements resources { get; set; }
        public V1SecurityContext securityContext { get; set; }
        public V1Probe startupProbe { get; set; }
        public bool stdin { get; set; }
        public bool stdinOnce { get; set; }
        public string terminationMessagePath { get; set; }
        public string terminationMessagePolicy { get; set; }
        public bool? tty { get; set; }
        public IList<V1VolumeDevice> volumeDevices { get; set; }
        public IList<V1VolumeMount> volumeMounts { get; set; }
        public string workingDir { get; set; }
    }

    public class V1EnvVar
    {
        public string name { get; set; }
        public string value { get; set; }
        public V1EnvVarSource valueFrom { get; set; }
    }
    public class V1EnvVarSource
    {
        public V1ConfigMapKeySelector configMapKeyRef { get; set; }
        public V1ObjectFieldSelector fieldRef { get; set; }
        public V1ResourceFieldSelector resourceFieldRef { get; set; }
        public V1SecretKeySelector secretKeyRef { get; set; }
    }
    public class V1ConfigMapKeySelector
    {
        public string key { get; set; }
        public string name { get; set; }
        public bool? optional { get; set; }
    }
    public class V1ObjectFieldSelector
    {
        public string apiVersion { get; set; }
        public string fieldPath { get; set; }
    }
    public class V1ResourceFieldSelector
    {
        public string containerName { get; set; }
        public ResourceQuantity divisor { get; set; }
        public string resource { get; set; }
    }
    public class V1SecretKeySelector
    {
        public string key { get; set; }
        public string name { get; set; }
        public bool? optional { get; set; }
    }
    public class V1EnvFromSource
    {
        public V1ConfigMapEnvSource configMapRef { get; set; }
        public string prefix { get; set; }
        public V1SecretEnvSource secretRef { get; set; }
    }
    public class V1ConfigMapEnvSource
    {
        public string name { get; set; }
        public bool? optional { get; set; }
    }
    public class V1SecretEnvSource
    {
        public string name { get; set; }
        public bool? optional { get; set; }
    }
    public class V1Lifecycle
    {
        public V1Handler postStart { get; set; }
        public V1Handler preStop { get; set; }
    }
    public class V1Handler
    {
        public V1ExecAction exec { get; set; }
        public V1HTTPGetAction httpGet { get; set; }
        public V1TCPSocketAction tcpSocket { get; set; }
    }
    public class V1ExecAction
    {
        public IList<string> command { get; set; }
    }
    public class V1HTTPGetAction
    {
        public string host { get; set; }
        public IList<V1HTTPHeader> httpHeaders { get; set; }
        public string path { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]
        public string port { get; set; }
        public string scheme { get; set; }
    }
    public class V1HTTPHeader
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class V1TCPSocketAction
    {
        public string host { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string port { get; set; }
    }

    public class V1Probe
    {
        public V1ExecAction exec { get; set; }
        public int? failureThreshold { get; set; }
        public V1HTTPGetAction httpGet { get; set; }
        public int? initialDelaySeconds { get; set; }
        public int? periodSeconds { get; set; }
        public int? successThreshold { get; set; }
        public V1TCPSocketAction tcpSocket { get; set; }
        public int? timeoutSeconds { get; set; }
    }
    public class V1SecurityContext
    {
        public bool? allowPrivilegeEscalation { get; set; }
        public V1Capabilities capabilities { get; set; }
        public bool? privileged { get; set; }
        public string procMount { get; set; }
        public bool? readOnlyRootFilesystem { get; set; }
        public long? runAsGroup { get; set; }
        public bool? runAsNonRoot { get; set; }
        public long? runAsUser { get; set; }
        public V1SELinuxOptions seLinuxOptions { get; set; }
        public V1WindowsSecurityContextOptions windowsOptions { get; set; }
    }
    public class V1Capabilities
    {
        public IList<string> add { get; set; }
        public IList<string> drop { get; set; }
    }

    public class V1ContainerPort
    {
        public int containerPort { get; set; }
        public string hostIP { get; set; }
        public int hostPort { get; set; }
        public string name { get; set; }
        public string protocol { get; set; }
    }
    public class V1ResourceRequirements
    {
        public IDictionary<string, ResourceQuantity> limits { get; set; }
        public IDictionary<string, ResourceQuantity> requests { get; set; }
    }

    [JsonConverter(typeof(ResourceQuantityConverter))]
    public class ResourceQuantity
    {
        public string value { get; set; }
    }

    public class V1VolumeDevice
    {
        public string devicePath { get; set; }
        public string name { get; set; }
    }

    public class V1VolumeMount
    {
        public string mountPath { get; set; }
        public string mountPropagation { get; set; }
        public string name { get; set; }
        public bool readOnly { get; set; }
        public string subPath { get; set; }
        public string subPathExpr { get; set; }
    }

    public class V1DeploymentList
    {
        public string apiVersion { get; set; }
        public V1Deployment[] items { get; set; }
        public string kind { get; set; }
        public V1ListMeta metadata { get; set; }
    }
    public class V1Deployment
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public V1ObjectMeta metadata { get; set; }
        public V1DeploymentSpec spec { get; set; }
        public V1DeploymentStatus status { get; set; }
    }
    public class V1DeploymentStatus
    {
        public int? availableReplicas { get; set; }
        public int? collisionCount { get; set; }
        public IList<V1DeploymentCondition> conditions { get; set; }
        public long? observedGeneration { get; set; }
        public int? readyReplicas { get; set; }
        public int? replicas { get; set; }
        public int? unavailableReplicas { get; set; }
        public int? updatedReplicas { get; set; }
    }
    public class V1DeploymentCondition
    {
        public DateTime? lastTransitionTime { get; set; }
        public DateTime? lastUpdateTime { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }
}
