using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agones
{
    public class EchoUdpServer
    {
        private string _ipAddress;
        private int _port;
        private Encoding _encoding;
        private IAgonesSdk _agonesSdk;

        public EchoUdpServer(string ipAddress, int port, IAgonesSdk agnoesSdk)
        {
            _ipAddress = ipAddress;
            _port = port;
            _encoding = new UTF8Encoding(false);
            _agonesSdk = agnoesSdk;
        }

        public async Task ServerLoop()
        {
            var isReady = await _agonesSdk.Ready();
            if (!isReady) throw new Exception("Could not prepare Agones.");

            var done = false;
            var exited = false;
            var crashed = false;

            var listener = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
            using (var udpClient = new UdpClient(listener))
            {
                while (!done)
                {
                    var receive = await udpClient.ReceiveAsync();
                    var (sender, txt) = (receive.RemoteEndPoint, _encoding.GetString(receive.Buffer)?.TrimStart()?.TrimEnd());
                    var parts = txt.Split(' ');
                    switch (parts[0])
                    {
                        case "EXIT":
                            Console.WriteLine("Shutdown gameserver.");
                            done = true;
                            await _agonesSdk.Shutdown();
                            var exitMessage = _encoding.GetBytes("ACK: " + txt + "\n");
                            await udpClient.SendAsync(exitMessage, exitMessage.Length, sender);
                            break;
                        case "UNHEALTHY":
                            Console.WriteLine("Turns off health pings.");
                            _agonesSdk.HealthEnabled = false;
                            break;
                        case "GAMESERVER":
                            await _agonesSdk.GetGameServer();
                            var gameserverMessage = _encoding.GetBytes("GAMESERVER NAME" + "\n");
                            await udpClient.SendAsync(gameserverMessage, gameserverMessage.Length, sender);
                            break;
                        case "READY":
                            await _agonesSdk.Ready();
                            break;
                        case "ALLOCATE":
                            await _agonesSdk.Allocate();
                            break;
                        case "RESERVE":
                            await _agonesSdk.Reserve();
                            break;
                        case "WATCH":
                            await _agonesSdk.WatchGameServer();
                            break;
                        case "LABEL":
                            switch (parts.Length)
                            {
                                case 1:
                                    // legacy format
                                    await _agonesSdk.SetLabel("timestamp", DateTime.Now.ToUniversalTime().ToString());
                                    break;
                                case 2:
                                    await _agonesSdk.SetLabel(parts[1], parts[2]);
                                    break;
                                default:
                                    var labelMessage = _encoding.GetBytes("ERROR: Invalid LABEL command, must use zero or 2 arguments\n");
                                    await udpClient.SendAsync(labelMessage, labelMessage.Length, sender);
                                    continue;
                            }
                            break;
                        case "CRASH":
                            Console.WriteLine("Crashing.");
                            done = true;
                            crashed = true;
                            break;
                        case "ANNOTATION":
                            switch (parts.Length)
                            {
                                case 1:
                                    // legacy format
                                    await _agonesSdk.SetAnnotation("timestamp", DateTime.UtcNow.ToUniversalTime().ToString());
                                    break;
                                case 3:
                                    await _agonesSdk.SetAnnotation(parts[1], parts[2]);
                                    break;
                                default:
                                    var labelMessage = _encoding.GetBytes("ERROR: Invalid ANNOTATION command, must use zero or 2 arguments\n");
                                    await udpClient.SendAsync(labelMessage, labelMessage.Length, sender);
                                    continue;
                            }
                            break;
                        default:
                            var echoMessage = _encoding.GetBytes("ACK: " + txt + "\n");
                            await udpClient.SendAsync(echoMessage, echoMessage.Length, sender);
                            break;
                    }
                }
            }
            if (exited) Environment.Exit(0);
            if (crashed) Environment.Exit(1);
        }
    }
}
