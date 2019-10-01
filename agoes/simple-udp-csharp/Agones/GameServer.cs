using System;
using System.Collections.Generic;
using System.Text;

namespace Agones
{
    public class GameServer
    {
        public Spec Spec { get; set; }
        public GameServerStatus Status { get; set; }
    }

    public class Spec
    {
        public string Container { get; set; }
        public GameServerPorts Ports { get; set; }
        public Health Health { get; set; }
        // Packed or Distributed
        public string SchedulingStrategy { get; set; }
        public SdkServer SdkServer { get; set; }
    }
    public class GameServerPorts
    {
        public string Name { get; set; }
        public string PortPolicy { get; set; }
        public int ContainerPort { get; set; }
        public int HostPort { get; set; }
    }
    public class Health
    {
        public bool Disabled { get; set; }
        public int PeriodSeconds { get; set; }
        public int FailureThreshold { get; set; }
        public int InitialDelaySeconds { get; set; }
    }
    public class SdkServer
    {
        public string LogLEvel { get; set; }
        public int GrpcPort { get; set; }
        public int HttpPort { get; set; }
    }

    public class GameServerStatus
    {
        // GameServerState is the current state of a GameServer, e.g. Creating, Starting, Ready, etc
        public string State { get; set; }
        public GameServerStatusPort[] Ports { get; set; }
        public string Address { get; set; }
        public string NodeName { get; set; }
        public string ReservedUntil { get; set; }
    }
    public class GameServerStatusPort
    {
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
