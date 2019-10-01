docker build -t agones-udp-server-csharp:0.1 -f Agones/Dockerfile .
docker tag agones-udp-server-csharp:0.1 guitarrapc/agones-udp-server-csharp:0.1
docker push guitarrapc/agones-udp-server-csharp:0.1
