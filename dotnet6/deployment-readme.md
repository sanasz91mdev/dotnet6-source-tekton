### Pre Requisites for deployment on linux

Install .net6 SDK, runtime on linux.

https://docs.microsoft.com/en-us/dotnet/core/install/linux

### Publish dotnet application containing .sln file

Clone repo and run dotnet publish in directory

https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish

e.g. 

    dotnet publish -c Release -o published


Yo can then run the app executable [.exe/dll] thats published in /published directory.

### Publishing modes

.NET 6 web apps has two different publishing modes, framework-dependent and self-contained.

1) Framework dependent
2) Self-contained

| Publish mode   |Description |
| ----------- | ----------- |
|Framework-dependent| In this mode, you need to have the .NET 6 runtime installed on the target machine.|
|Self-contained| In this mode, your app is bundled with the .NET 6.0 runtime when it is published, so the target machine does not need the .NET Core runtime installed.|

We will be using **Framework-dependent** publish mode
as then we will need to distribute only app's dll files, as the runtime is already installed on the target machine. This results a smaller distribution size with both on prem deployment and docker images.

Since Docker images uses layer caching, If two Docker images use the same "base" image, then Docker will naturally "de-dupe" the duplicate layers. These duplicate layers don't need to be transferred when pushing or pulling images from registries if the target already has the given layer

If two framework dependent apps are deployed in Docker images, and both use the same framework version, then the effective image size is much smaller, d**ue to the layer caching**. In contrast, even though self-contained deployment images will be smaller over all, they'll never benefit from caching the .NET runtime in a layer, as it's part of your application deployment. That means you may well have to push more bytes around with self-contained deployments.

Also, if you're deploying Docker images to a Kubernetes cluster, then the framework-dependent approach probably makes the most sense. You'll benefit from more layer-caching, so there should be fewer bytes to push to and from Docker registries.

If you are not using docker and want to use self-contained deployment for the added control and assurance it gives you about your runtime environment, you can do so with the following dotnet publish command

    dotnet publish -c Release -r <RID> --self-contained true


### Running With reverse proxy

We can place an existing ASP.NET Core app behind a reverse proxy server.

A reverse proxy:

- Can limit the exposed public surface area of the apps that it hosts.
- Provide an additional layer of configuration and defense.
- Might integrate better with existing infrastructure.
- Simplify load balancing and secure communication (HTTPS) configuration. Only the reverse proxy server requires an X.509 certificate, and that server can communicate with the app's servers on the internal network using plain HTTP.

We can use Nginx which is  non blocking, event - driven web server, consumes very less memory .. its 50 % faster then apache

### Preserving request IP, scheme and host with reverse-proxy in place

Proxy servers, load balancers, and other network appliances often obscure information about the request before it reaches the app:

When HTTPS requests are proxied over HTTP, the original scheme (HTTPS) is lost and must be forwarded in a header.
Because an app receives a request from the proxy and not its true source on the Internet or corporate network, the originating client IP address must also be forwarded in a header.


This information may be important in request processing, for example in 
 - redirects
 - authentication
 - link generation
 - policy evaluation, and 
 - client geolocation.
 
 By convention, proxies forward information in HTTP headers in X - [] - Headers


### Kestrel Forward headers mapping

| HttpContext attribute      | X-Forwarded-[] key |
| ----------- | ----------- |
| HttpContext.Connection.RemoteIpAddress      | Set using the X-Forwarded-For header value. Additional settings influence how the middleware sets RemoteIpAddress. For details, see the Forwarded Headers Middleware options.       |
| HttpContext.Request.Scheme   | Set using the X-Forwarded-Proto header value.        |
| HttpContext.Request.Host   | Set using the X-Forwarded-Host header value.        |

https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0



### Load balancing happens by declaring upstream block within http context 

To pass requests to a server group (declared in upstream block), the name of the group is specified in the proxy_pass directive
Doing this, a virtual server running on NGINX passes all requests to the ["backend"] upstream group defined in below example:

    http {
        upstream backend {
            server 127.0.0.1:5000;
            server 127.0.0.1:6000;
            server 127.0.0.1:7000;
            server 127.0.0.1:7081;

            server 192.0.0.1 backup; #--> if none abave available then use this
        }
        
        server {
            location / {
                proxy_pass http://backend;
            }
        }
    }


- By default Round Robin load balancer method is used .. Server weights option can be used with this method ... other options are Least connections, Least time etc

#### sample nginx.conf:

    user nginx;

    worker_processes    auto;

    events { worker_connections 1024; }

    http {
        include             /etc/nginx/proxy.conf;
        include             /etc/nginx/mime.types;
        limit_req_zone      $binary_remote_addr zone=one:10m rate=5r/s;
        server_tokens       off;
        sendfile            on;
        keepalive_timeout   30; # Adjust to the lowest possible value that makes sense for your use case.
        client_body_timeout 10; client_header_timeout 10; send_timeout 10;

        upstream webapi {
            server 127.0.0.1:5000;
            server 127.0.0.1:6000;
            server 127.0.0.1:7000;
            server 127.0.0.1:7081;
        }

        server {
            listen          80;
            server_name     $hostname;

            location / {
                proxy_pass  http://webapi;
            }
        }
    }

#### Sample proxy.conf

    proxy_http_version 1.1;
    proxy_set_header   Upgrade 					$http_upgrade;
    proxy_set_header   Connection 				keep-alive;
    proxy_set_header   Host                     $host;
    proxy_cache_bypass $http_upgrade;
    proxy_set_header   X-Forwarded-For 			$proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto 		$scheme;
	proxy_set_header   X-Real-IP         		$remote_addr;

### Using kong as an API gateway for .net app

Since kong uses Nginx under the hood, when running .net6 app behind an API Gateway we can use same nginx used by kong for load balancing .net 6 app. Same configuration can be used as described above.

Note that since kong already uses `nginx-kong.conf` to forward X-Headers, you do not need to configure proxy.conf as above.

### Monitoring with supervisord

We can use supervisord for monitoring our .net app running on prem.

Details: [https://community.tpsonline.com/topic/454/supervisord-for-process-monitoring]

Sample configuration


### Building docker images for .net 6 app

#### Deployment purpose

You can run a .net 6 app in docker.
A deployment pipeline that publishes app using .net 6 app using 

    dotnet publish

can use following Dockerfile to build .net 6 web api image

    # https://hub.docker.com/_/microsoft-dotnet
    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /dockerappNet6
    # copy everything from what CICD will publish in a dir and run app
    COPY published/. ./

    ENV ASPNETCORE_URLS=http://+:5000
    ENV ASPNETCORE_ENVIRONMENT="production"

    EXPOSE 5000

    # need for wsl2
    #ENV TZ Asia/Karachi 

    ENTRYPOINT ["dotnet", "DigitalBanking.dll"]


Similarly, for development purpose, developers can use image below to automatically build, publish app and package it in a image so that it can be run in a container.

    # https://hub.docker.com/_/microsoft-dotnet
    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /dockerappNet6

    #copy all from dev dir to work DIR
    COPY /. ./app/

    WORKDIR /dockerappNet6/app/AppNet6

    RUN dotnet publish -c Release -o /published

    #COPY aspnetapp/. ./aspnetapp/


    # copy everything from what publish command pushed to /published
    FROM mcr.microsoft.com/dotnet/aspnet:6.0
    WORKDIR /published
    COPY --from=build /published ./
    #COPY published/. ./

    ENV ASPNETCORE_URLS=http://+:5000
    ENV ASPNETCORE_ENVIRONMENT="development"

    EXPOSE 5000

    # need for wsl2
    ENV TZ Asia/Karachi

    ENTRYPOINT ["dotnet", "DigitalBanking.dll"]


### Running .net 6 app with related services using docker-compose

There might be times when we need to run multiple containers in docker that share same network as .net 6 app container does.
Containers on a single network can reach and discover every other container on the network

With application observability in place, we have this scenario where we need application traces to be exported to jaeger. So with docker compose, we assign both .net 6 app and jager to same container.

So we can use docker compose file for this. Developers, QA can use following docker-compose.yml to do so.


    version: '3.7'
    services:
    jaeger:
        image: 'jaegertracing/all-in-one:latest'
        ports:
        - '6831:6831/udp'
        - '16686:16686'
        - '5778:5778'
        - '5775:5775/udp'
        - '6832:6832/udp'
        - '14268:14268'
        - '14250:14250'
        networks:
        - jaeger-example
        
        
    iris5_digitalbanking:
        image: 'dndb:dev'
        container_name: dotnet6_digitalbanking_dev
        volumes:
        - /home/sana/dotnet6:/home/digitalbanking/
        working_dir: /published 
        networks:
        - jaeger-example        
        depends_on:
        - jaeger    
        ports:
        - '8080:5000'
        environment:
        - OTEL_EXPORTER_JAEGER_AGENT_HOST=jaeger
        - OTEL_EXPORTER_JAEGER_AGENT_PORT=6831
        
        
    networks:
    jaeger-example:



### Deploying .net 6 app with related services in a kubernetees cluster

[In progress]
Refer to K8s folder in POC dotnet6 repo 

============================================================

### Sample logs - to verify client geolocation is preserved


When nginx -> Kestrel


outside world

2022-01-17 18:16:10.119 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Http Method: [GET], Protoclol [HTTP/1.1], Path [/api/v1/users], Request Body: []
2022-01-17 18:16:10.120 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Http Headers: [
{"Accept":["text/html,application/xhtml\u002Bxml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"],
"Connection":["keep-alive"],
"Host":["40.74.55.129"],
"User-Agent":["Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36"],
"Accept-Encoding":["gzip, deflate"],
"Accept-Language":["en-US,en;q=0.9"],
"Cache-Control":["max-age=0"],
"Upgrade-Insecure-Requests":["1"],
"X-Real-IP":["202.47.36.51"],
"X-Original-Proto":["http"],
"X-Forwarded-Host":["docker-centos-s"],
"X-Original-For":["127.0.0.1:33644"]}]


2022-01-17 18:16:10.121 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Http Connection: [202.47.36.51:0]
2022-01-17 18:16:10.121 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Http Scheme: [http]
2022-01-17 18:16:10.122 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Http Host: [40.74.55.129]
2022-01-17 18:16:10.123 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] module: 'UserModule' Message = Get users v1 called.)
2022-01-17 18:16:10.124 +00:00 [INFO] [0HMEPSL1ELTLM:00000002] Response Body: {"name":"sana","contact":"03323344553"}


=================================================================================================================================


Kong - kestrel [without proxy.conf]




2022-01-17 20:09:12.985 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Http Method: [GET], Protoclol [HTTP/1.0], Path [/api/v1/users], Request Body: []
2022-01-17 20:09:12.986 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Http Headers: [{
"Accept":["text/html,application/xhtml\u002Bxml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"],
"Connection":["close"],
"Host":["webapi"],
"User-Agent":["Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36"],
"Accept-Encoding":["gzip, deflate, br"],
"Accept-Language":["en-US,en;q=0.9"],
"Cache-Control":["max-age=0"],
"Upgrade-Insecure-Requests":["1"],
"X-Original-Proto":["http"],
"X-Forwarded-Host":["40.74.55.129"],
"X-Forwarded-Port":["8443"],
"X-Real-IP":["202.47.36.51"],
"sec-ch-ua":["\u0022 Not;A Brand\u0022;v=\u002299\u0022, \u0022Google Chrome\u0022;v=\u002297\u0022, \u0022Chromium\u0022;v=\u002297\u0022"],"sec-ch-ua-mobile":["?0"],"sec-ch-ua-platform":["\u0022Windows\u0022"],
"Sec-Fetch-Site":["none"],
"Sec-Fetch-Mode":["navigate"],
"Sec-Fetch-User":["?1"],
"Sec-Fetch-Dest":["document"],
"X-Original-For":["127.0.0.1:57100"]}]
2022-01-17 20:09:12.987 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Http Connection: [202.47.36.51:0]
2022-01-17 20:09:12.987 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Http Scheme: [https]
2022-01-17 20:09:12.987 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Http Host: [webapi]
2022-01-17 20:09:12.987 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] module: 'UserModule' Message = Get users v1 called.)
2022-01-17 20:09:12.988 +00:00 [INFO] [0HMEPSL1ELTM6:00000002] Response Body: {"name":"sana","contact":"03323344553"}
