# Docker & Kubernetes Lab

Docker & Kubernetes Lab

## Overview

This github repository folder contains the Docker Lab and Kubernetes Lab.  

## The Lab

During this lab, you will see how to:
- use Docker to run existing docker image,
- use Docker to build and run docker image,
- deploy Azure Container Registry,
- use Docker to build docker image and store the image on Azure Container Registry,
- deploy Azure Kubernetes Service,
- deploy container image using kubectl,
- deploy container image using Helm,

## Prerequisites

You need to install the following pre-requisite on your machine

1. Install and configure [Docker](https://www.docker.com/get-started) for your operating system.

   - Windows / macOS:

     1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop) for Windows/Mac.

     2. Right-click on the Docker task bar item, select Settings / Preferences and update Resources > File Sharing with any locations your source code is kept. See [tips and tricks](https://code.visualstudio.com/docs/remote/troubleshooting#_container-tips) for troubleshooting.

     3. If you are using WSL 2 on Windows, to enable the [Windows WSL 2 back-end](https://docs.docker.com/docker-for-windows/wsl/): Right-click on the Docker taskbar item and select Settings. Check Use the WSL 2 based engine and verify your distribution is enabled under Resources > WSL Integration.

   - Linux:

     1. Follow the official install [instructions for Docker CE/EE for your distribution](https://docs.docker.com/get-docker/). If you are using Docker Compose, follow the [Docker Compose directions](https://docs.docker.com/compose/install/) as well.

     2. Add your user to the docker group by using a terminal to run: sudo usermod -aG docker $USER

     3. Sign out and back in again so your changes take effect.

  

### Some docker commands

Below a list of useful docker commands:

1. Run hello-world docker image:
```bash
    docker run hello-world
```

2. Run ubuntu docker image :
```bash
    docker run --name ubuntu  -it ubuntu bash    
```
3. Enter 'exit' to leave ubuntu bash

4. List local docker images:
```bash
    docker image list     
```
5. Inspect an image:
```bash
    docker image inspect ubuntu     
```
6. List all containers:
```bash
    docker container list --all     
```
7. List running containers:
```bash
    docker container list     
```
or
```bash
    docker ps     
```
8. List all the networks:
```bash
    docker network list     
```
9. Inspect bridge network:
```bash
    docker network inspect bridge     
```
10. Inspect a running container :
   Launch ubuntu container with --rm and -d option :
```bash
    docker run -it -d --rm --name ubuntu -t ubuntu:latest bash     
```
   List the running container, check ubuntu container is running
```bash
    docker ps     
```
   Display further information about the running container:
```bash
    docker container inspect ubuntu      
```
   Display running container local IP address
```bash
    docker container inspect ubuntu | jq -r '.[].NetworkSettings.Networks.bridge.IPAddress'
```
11. Run a command on a running container:
```bash
    docker exec -it ubuntu /bin/bash
```
12. Stop a running container 
```bash
    docker stop ubuntu
```
12. Remove a container 
```bash
    docker rm ubuntu
```
13. List docker volume  
```bash
    docker volume ls
```
14. Create a volume on the host machine  
    Run this command with the host machine shell (not in the devcontainer)
```bash
    docker volume create --name test_volume --opt type=none --opt device=/mnt/c/git/devLabs/Lab3 --opt o=bind
```
15. Run a container using the host machine volume  
```bash
    docker run -it -v test_volume:/lab3 --rm --name ubuntu -t ubuntu:latest bash
```
    Check the /lab3 folder in the container is associated with the devLabs/Lab3 folder in the host machine.
16. Remove the volume on the host machine  
```bash
    docker volume rm test_volume 
```

### Building and running Docker image hosting dotnet based REST API locally

1. Run the following commands to build a dotnet based REST API:

```bash
    DOTNET_PORT_HTTP=8000
    DOTNET_APP_VERSION=$(date +"%Y%m%d.%H%M%S")
    DOTNET_REST_API_NAME="dotnet-rest-api"

    PORT_HTTP=${DOTNET_PORT_HTTP}
    APP_VERSION=${DOTNET_APP_VERSION}
    REST_API_NAME=${DOTNET_REST_API_NAME}

    IMAGE_NAME="${REST_API_NAME}-image"
    IMAGE_TAG=${APP_VERSION}
    CONTAINER_NAME="${REST_API_NAME}-container"
    ALTERNATIVE_TAG="latest"
    APP_ENVIRONMENT="Development"

    echo "PORT_HTTP $PORT_HTTP"
    echo "APP_VERSION $APP_VERSION"
    echo "IMAGE_NAME $IMAGE_NAME"
    echo "IMAGE_TAG $IMAGE_TAG"
    echo "ALTERNATIVE_TAG $ALTERNATIVE_TAG"

    echo "Building container"
    docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f ./dotnet_rest_api/Dockerfile --build-arg APP_VERSION=${IMAGE_TAG} --build-arg ARG_PORT_HTTP=${PORT_HTTP}  ./dotnet_rest_api/
    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${IMAGE_NAME}:${ALTERNATIVE_TAG}
```

2. Run the following commands to run the container running a dotnet based REST API:

```bash
    docker run -d -e ARG_PORT_HTTP=${PORT_HTTP} -e APP_ENVIRONMENT=${APP_ENVIRONMENT} -e APP_VERSION=${IMAGE_TAG} -p ${PORT_HTTP}:${PORT_HTTP}/tcp  --rm --name ${CONTAINER_NAME}    ${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    echo "open http://localhost:${PORT_HTTP}/swagger/index.html with your browser"
```

3. Test the container running a dotnet based REST API from the dev container:

```bash
    IP_ADDRESS=$(docker inspect ${CONTAINER_NAME} | jq -r '.[].NetworkSettings.Networks.bridge.IPAddress')
    VERSION=$(curl  -s -X  GET http://${IP_ADDRESS}:${PORT_HTTP}/version | jq -r .version)
    if [ ${VERSION} == ${APP_VERSION} ];
    then
      echo "Deployment successful Container running version ${VERSION}"
    else
      echo "Deployment failed Container running version ${VERSION} instead of ${APP_VERSION}"
    fi
```

4. Test the container running a dotnet based REST API from the host machine:

```bash
    IP_ADDRESS=127.0.0.1
    VERSION=$(curl  -s -X  GET http://${IP_ADDRESS}:${PORT_HTTP}/version | jq -r .version)
    if [ ${VERSION} == ${APP_VERSION} ];
    then
      echo "Deployment successful Container running version ${VERSION}"
    else
      echo "Deployment failed Container running version ${VERSION} instead of ${APP_VERSION}"
    fi
```
5. Stop the container running a dotnet based REST API :

```bash
    docker stop ${CONTAINER_NAME}
```

### Building and running Docker image hosting fastapi based REST API locally

1. Run the following commands to build a fastapi based REST API:

```bash
    FASTAPI_PORT_HTTP=7000
    FASTAPI_APP_VERSION=$(date +"%Y%m%d.%H%M%S")
    FASTAPI_REST_API_NAME="fastapi-rest-api"

    PORT_HTTP=${FASTAPI_PORT_HTTP}
    APP_VERSION=${FASTAPI_APP_VERSION}
    REST_API_NAME=${FASTAPI_REST_API_NAME}

    IMAGE_NAME="${REST_API_NAME}-image"
    IMAGE_TAG=${APP_VERSION}
    CONTAINER_NAME="${REST_API_NAME}-container"
    ALTERNATIVE_TAG="latest"
    APP_ENVIRONMENT="Development"

    echo "PORT_HTTP $PORT_HTTP"
    echo "APP_VERSION $APP_VERSION"
    echo "IMAGE_NAME $IMAGE_NAME"
    echo "IMAGE_TAG $IMAGE_TAG"
    echo "ALTERNATIVE_TAG $ALTERNATIVE_TAG"

    echo "Building container"
    docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f ./fastapi_rest_api/Dockerfile --build-arg APP_VERSION=${IMAGE_TAG} --build-arg ARG_PORT_HTTP=${PORT_HTTP}  ./fastapi_rest_api/
    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${IMAGE_NAME}:${ALTERNATIVE_TAG}
```

2. Run the following commands to run the container running a fastapi based REST API:

```bash
    docker run -d -e ARG_PORT_HTTP=${PORT_HTTP} -e APP_ENVIRONMENT=${APP_ENVIRONMENT} -e APP_VERSION=${IMAGE_TAG} -p ${PORT_HTTP}:${PORT_HTTP}/tcp  --rm --name ${CONTAINER_NAME}    ${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    echo "open http://localhost:${PORT_HTTP}/docs with your browser"
```

3. Test the container running a fastapi based REST API from the dev container:

```bash
    IP_ADDRESS=$(docker inspect ${CONTAINER_NAME} | jq -r '.[].NetworkSettings.Networks.bridge.IPAddress')
    VERSION=$(curl  -s -X  GET http://${IP_ADDRESS}:${PORT_HTTP}/version)
    if [ ${VERSION//\"/} == ${APP_VERSION} ];
    then
      echo "Deployment successful Container running version ${VERSION}"
    else
      echo "Deployment failed Container running version ${VERSION} instead of ${APP_VERSION}"
    fi
```

4. Test the container running a fastapi based REST API from the host machine:

```bash
    IP_ADDRESS=127.0.0.1
    VERSION=$(curl  -s -X  GET http://${IP_ADDRESS}:${PORT_HTTP}/version)
    if [ ${VERSION//\"/} == ${APP_VERSION} ];
    then
      echo "Deployment successful Container running version ${VERSION}"
    else
      echo "Deployment failed Container running version ${VERSION} instead of ${APP_VERSION}"
    fi
```

5. Stop the container running a fastapi based REST API :

```bash
    docker stop ${CONTAINER_NAME}
```

### Deploy Azure Container Registry pull and pull image with Azure CLI

1. Run the following command in the dev container terminal

```bash
    cd ./Lab3
    az login
```

Azure CLi open the default browser to load an Azure sign-in page.

2. Once connected, run the following command to display the Azure Subcription Id and the Tenant Id:

```bash
    az account show 
```

3. Create the resource group

```bash
    AZURE_SUBSCRIPTION_ID=$(az account show  | jq -r .id)
    AZURE_REGION=eastus2
    AZURE_RESOURCE_GROUP=rgtestacraks$(shuf -i 1000-9999 -n 1)
    az group create  --subscription $AZURE_SUBSCRIPTION_ID --location $AZURE_REGION --name $AZURE_RESOURCE_GROUP 
```

4. Create the Azure Container Registry

```bash
    ACR_NAME=testacr$(shuf -i 1000-9999 -n 1)
    POST_DEPLOYMENT_NAME="POST-$(date +"%y%m%d-%H%M%S")"
    az deployment group create \
        --name $POST_DEPLOYMENT_NAME \
        --resource-group ${AZURE_RESOURCE_GROUP} \
        --subscription ${AZURE_SUBSCRIPTION_ID} \
        --template-file azuredeploy-container-registry.json \
        --output none \
        --parameters \
        acrName=${ACR_NAME} 
    ACR_LOGIN_SERVER=$(az deployment group show --resource-group "$AZURE_RESOURCE_GROUP" -n "$POST_DEPLOYMENT_NAME" | jq -r '.properties.outputs.acrLoginServer.value')
```

5. Run the following commands to build and push a dotnet based REST API:

```bash
    DOTNET_PORT_HTTP=8000
    DOTNET_APP_VERSION=$(date +"%Y%m%d.%H%M%S")
    DOTNET_REST_API_NAME="dotnet-rest-api"

    PORT_HTTP=${DOTNET_PORT_HTTP}
    APP_VERSION=${DOTNET_APP_VERSION}
    REST_API_NAME=${DOTNET_REST_API_NAME}

    IMAGE_NAME="${REST_API_NAME}-image"
    IMAGE_TAG=${APP_VERSION}
    CONTAINER_NAME="${REST_API_NAME}-container"
    ALTERNATIVE_TAG="latest"
    APP_ENVIRONMENT="Development"

    echo "PORT_HTTP $PORT_HTTP"
    echo "APP_VERSION $APP_VERSION"
    echo "IMAGE_NAME $IMAGE_NAME"
    echo "IMAGE_TAG $IMAGE_TAG"
    echo "ALTERNATIVE_TAG $ALTERNATIVE_TAG"

    echo "Login on ACR: $ACR_LOGIN_SERVER"
    az acr login --name $ACR_NAME

    echo "Building container"
    docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f ./dotnet_rest_api/Dockerfile --build-arg APP_VERSION=${IMAGE_TAG} --build-arg ARG_PORT_HTTP=${PORT_HTTP}  ./dotnet_rest_api/
    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${IMAGE_NAME}:${ALTERNATIVE_TAG}

    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${IMAGE_TAG} 
    docker tag ${IMAGE_NAME}:${ALTERNATIVE_TAG} ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    docker push ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${IMAGE_TAG}
    docker push ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
```

6. Run the following commands to pull and run the container running a dotnet based REST API:

```bash
    docker pull ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
    docker run -d -e ARG_PORT_HTTP=${PORT_HTTP} -e APP_ENVIRONMENT=${APP_ENVIRONMENT} -e APP_VERSION=${IMAGE_TAG} -p ${PORT_HTTP}:${PORT_HTTP}/tcp  --rm --name ${CONTAINER_NAME}    ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    echo "open http://localhost:${PORT_HTTP}/swagger/index.html with your browser"
```

7. Run the following commands to stop the container running a dotnet based REST API:

```bash
    docker stop ${CONTAINER_NAME}
```

8. Run the following commands to build and push a fastapi based REST API:

```bash
    FASTAPI_PORT_HTTP=7000
    FASTAPI_APP_VERSION=$(date +"%Y%m%d.%H%M%S")
    FASTAPI_REST_API_NAME="fastapi-rest-api"

    PORT_HTTP=${FASTAPI_PORT_HTTP}
    APP_VERSION=${FASTAPI_APP_VERSION}
    REST_API_NAME=${FASTAPI_REST_API_NAME}

    IMAGE_NAME="${REST_API_NAME}-image"
    IMAGE_TAG=${APP_VERSION}
    CONTAINER_NAME="${REST_API_NAME}-container"
    ALTERNATIVE_TAG="latest"
    APP_ENVIRONMENT="Development"

    echo "PORT_HTTP $PORT_HTTP"
    echo "APP_VERSION $APP_VERSION"
    echo "IMAGE_NAME $IMAGE_NAME"
    echo "IMAGE_TAG $IMAGE_TAG"
    echo "ALTERNATIVE_TAG $ALTERNATIVE_TAG"

    echo "Login on ACR: $ACR_LOGIN_SERVER"
    az acr login --name $ACR_NAME

    echo "Building container"
    docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f ./fastapi_rest_api/Dockerfile --build-arg APP_VERSION=${IMAGE_TAG} --build-arg ARG_PORT_HTTP=${PORT_HTTP}  ./fastapi_rest_api/
    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${IMAGE_NAME}:${ALTERNATIVE_TAG}

    docker tag ${IMAGE_NAME}:${IMAGE_TAG} ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${IMAGE_TAG} 
    docker tag ${IMAGE_NAME}:${ALTERNATIVE_TAG} ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    docker push ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${IMAGE_TAG}
    docker push ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
```

9. Run the following commands to pull and run the container running a fastapi based REST API:

```bash
    docker pull ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
    docker run -d -e ARG_PORT_HTTP=${PORT_HTTP} -e APP_ENVIRONMENT=${APP_ENVIRONMENT} -e APP_VERSION=${IMAGE_TAG} -p ${PORT_HTTP}:${PORT_HTTP}/tcp  --rm --name ${CONTAINER_NAME}    ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG} 
    echo "open http://localhost:${PORT_HTTP}/docs with your browser"
```

10. Run the following commands to stop the container running a fastapi based REST API:

```bash
    docker stop ${CONTAINER_NAME}
```


### Deploy Azure Kubernetes Service 

1. Run the following command in the dev container terminal

```bash
    cd ./Lab3
    az login
```
    Azure CLi open the default browser to load an Azure sign-in page.

2. Once connected, run the following command to display the Azure Subcription Id and the Tenant Id:

```bash
    az account show 
```

3. Reuse the resource group created for the Azure Container Registry

```bash
    echo "Resource group: $AZURE_RESOURCE_GROUP" 
```

4. Create the Azure Kubernetes Service

```bash
    AKS_NAME=testaks$(shuf -i 1000-9999 -n 1)
    POST_DEPLOYMENT_NAME="POST-$(date +"%y%m%d-%H%M%S")"
    if [ ! -f ./out${AKS_NAME}key.pub ]
    then
        ssh-keygen -t rsa -b 2048 -f ./out${AKS_NAME}key -q -P ""
    fi
    if [ -f "./out${AKS_NAME}key.pub" ]
    then
        AZURE_SSH_PUBLIC_KEY="\"$(cat ./out${AKS_NAME}key.pub)\""
    else
        AZURE_SSH_PUBLIC_KEY=""
    fi
    if [ -f "./out${AKS_NAME}key" ]
    then
        AZURE_SSH_PRIVATE_KEY="\"$(cat ./out${AKS_NAME}key)\""
    else
        AZURE_SSH_PRIVATE_KEY=""
    fi
    
    cmd="az deployment group create \
        --name $POST_DEPLOYMENT_NAME \
        --resource-group ${AZURE_RESOURCE_GROUP} \
        --subscription ${AZURE_SUBSCRIPTION_ID} \
        --template-file azuredeploy-aks.json \
        --output none \
        --parameters \
        aksclusterName=${AKS_NAME}cluster dnsPrefix=${AKS_NAME} agentCount=1 linuxAdminUsername=aksadmmin sshRSAPublicKey=${AZURE_SSH_PUBLIC_KEY}"
    echo "$cmd"
    eval "$cmd"

    AKS_FQDN=$(az deployment group show --resource-group "$AZURE_RESOURCE_GROUP" -n "$POST_DEPLOYMENT_NAME" | jq -r '.properties.outputs.controlPlaneFQDN.value')
    echo "AKS dns name: $AKS_FQDN"
```



5. Link AKS Cluster with Azure Container Registry

```bash
    az aks update -n "${AKS_NAME}cluster" -g "${AZURE_RESOURCE_GROUP}" --attach-acr "${ACR_NAME}"
```

### Using Kubectl  

1. Link kubectl with the new AKS Cluster

```bash
    az aks get-credentials --name "${AKS_NAME}cluster" --overwrite-existing --resource-group "$AZURE_RESOURCE_GROUP"
    kubectl config view
```

2. Get the list of default services 

```bash
    kubectl get services
```

3. Get the list of all services 

```bash
    kubectl get services --all-namespaces    
    kubectl get services -A
```

4. Get the list of default pods 

```bash
    kubectl get pods 
```

5. Get the list of default pods with details 

```bash
    kubectl get pods -o wide
```

6. Get the list of all pods with details  

```bash
    kubectl get pods --all-namespaces  -o wide  
    kubectl get pods -A  -o wide 
```

### Deploy a container hosting the dotnet REST API with Kubectl 

1. Create the dotnet REST API deployment

```bash
PORT_HTTP=${DOTNET_PORT_HTTP}
APP_VERSION=${DOTNET_APP_VERSION}
REST_API_NAME=${DOTNET_REST_API_NAME}

echo -e "
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  replicas: 1
  selector:
    matchLabels:
      app: ${REST_API_NAME}
  template:
    metadata:
      labels:
        app: ${REST_API_NAME}
    spec:
      containers:
      - name: ${REST_API_NAME}
        image: ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
        env:
        - name: PORT_HTTP
          value: \"${PORT_HTTP}\"
        - name: APP_ENVIRONMENT
          value: \"${APP_ENVIRONMENT}\"        
        - name: APP_VERSION
          value: \"${APP_VERSION}\"    
" | kubectl apply -f -

```
  you can also use the commands below to create the deployment:
```bash
    kubectl create deploy dotnet-rest-api --image=${ACR_NAME}.azurecr.io/dotnet-rest-api-image:latest
    kubectl create -f deployment-dotnet-rest-api.yaml
```

  you can check if the deployment is present:
```bash
    kubectl get deployments -A
```


2. Create the dotnet REST API service

```bash
PORT_HTTP=${DOTNET_PORT_HTTP}
APP_VERSION=${DOTNET_APP_VERSION}
REST_API_NAME=${DOTNET_REST_API_NAME}

echo -e "
apiVersion: v1
kind: Service
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  type: ClusterIP
  ports:
  - port: 80
    protocol: TCP
    targetPort: ${PORT_HTTP}              
  selector:
    app: ${REST_API_NAME}   
" | kubectl apply -f -

```

  you can also use the commands below to create the service:
```bash
    kubectl expose deploy dotnet-rest-api --type=ClusterIP --port=8000
    kubectl create -f service-dotnet-rest-api.yaml
```

  you can check if the service is present:
```bash
    kubectl get services -A    
```

3. Check the pod status and pod name

```bash
    kubectl get pods -A
    cmd="kubectl get pods -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${DOTNET_REST_API_NAME}\")).metadata.name'"
    DOTNET_REST_API_POD=$(eval "$cmd")
    kubectl describe pods "${DOTNET_REST_API_POD}"
```

4. Get the logs associated with pod

```bash
    kubectl logs ${DOTNET_REST_API_POD}
```

5. Connect the local kubectl with your pod

```bash
    kubectl port-forward ${DOTNET_REST_API_POD} 3000:${PORT_HTTP}
```

6. Test the REST API with your browser

```bash
    curl http://localhost:3000/version
```
```bash
    curl http://localhost:3000/time
```

7. Delete dotnet REST API service

```bash
    kubectl delete  svc "${REST_API_NAME}"
```

8. Delete dotnet REST API deployment

```bash
    kubectl delete  deploy "${REST_API_NAME}"
```


### Deploy a container hosting the fastapi REST API with Kubectl 

1. Create the fastapi REST API deployment

```bash
PORT_HTTP=${FASTAPI_PORT_HTTP}
APP_VERSION=${FASTAPI_APP_VERSION}
REST_API_NAME=${FASTAPI_REST_API_NAME}

echo -e "
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  replicas: 1
  selector:
    matchLabels:
      app: ${REST_API_NAME}
  template:
    metadata:
      labels:
        app: ${REST_API_NAME}
    spec:
      containers:
      - name: ${REST_API_NAME}
        image: ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
        env:
        - name: PORT_HTTP
          value: \"${PORT_HTTP}\"
        - name: APP_ENVIRONMENT
          value: \"${APP_ENVIRONMENT}\"        
        - name: APP_VERSION
          value: \"${APP_VERSION}\"    
" | kubectl apply -f -

```
  you can also use the commands below to create the deployment:
```bash
    kubectl create deploy fastapi-rest-api --image=${ACR_NAME}.azurecr.io/fastapi-web-api-image:latest
    kubectl create -f deployment-fastapi-rest-api.yaml
```

  you can check if the deployment is present:
```bash
    kubectl get deployments -A
```

2. Create the fastapi REST API service

```bash
PORT_HTTP=${FASTAPI_PORT_HTTP}
APP_VERSION=${FASTAPI_APP_VERSION}
REST_API_NAME=${FASTAPI_REST_API_NAME}

echo -e "
apiVersion: v1
kind: Service
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  type: ClusterIP
  ports:
  - port: 80
    protocol: TCP
    targetPort: ${PORT_HTTP}              
  selector:
    app: ${REST_API_NAME}   
" | kubectl apply -f -

```

  you can also use the commands below to create the service:
```bash
    kubectl expose deploy fastapi-rest-api --type=ClusterIP --port=8000
    kubectl create -f service-fastapi-rest-api.yaml
```

3. Check the pod status

```bash
    kubectl get pods -A
    cmd="kubectl get pods -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${FASTAPI_REST_API_NAME}\")).metadata.name'"
    FASTAPI_REST_API_POD=$(eval "$cmd")
    kubectl describe pods "${FASTAPI_REST_API_POD}"
```

4. Get the logs associated with pod

```bash
    kubectl logs ${FASTAPI_REST_API_POD}
```

5. Connect the local kubectl with your pod

```bash
    kubectl port-forward ${FASTAPI_REST_API_POD} 3000:${PORT_HTTP}
```

6. Test the REST API with your browser

```bash
    curl http://localhost:3000/version
```
```bash
    curl http://localhost:3000/time
```

7. Delete fastapi REST API service

```bash
    kubectl delete  svc "${REST_API_NAME}"
```

8. Delete fastapi REST API deployment

```bash
    kubectl delete  deploy "${REST_API_NAME}"
```


### Deploy a container hosting the dotnet REST API with public access  

1. Create the dotnet REST API deployment

```bash
PORT_HTTP=${DOTNET_PORT_HTTP}
APP_VERSION=${DOTNET_APP_VERSION}
REST_API_NAME=${DOTNET_REST_API_NAME}

echo -e "
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  replicas: 1
  selector:
    matchLabels:
      app: ${REST_API_NAME}
  template:
    metadata:
      labels:
        app: ${REST_API_NAME}
    spec:
      containers:
      - name: ${REST_API_NAME}
        image: ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
        env:
        - name: PORT_HTTP
          value: \"${PORT_HTTP}\"
        - name: APP_ENVIRONMENT
          value: \"${APP_ENVIRONMENT}\"        
        - name: APP_VERSION
          value: \"${APP_VERSION}\"    
" | kubectl apply -f -

```
  you can also use the commands below to create the deployment:
```bash
    kubectl create deploy dotnet-rest-api --image=${ACR_NAME}.azurecr.io/dotnet-rest-api-image:latest
    kubectl create -f deployment-dotnet-rest-api.yaml
```

  you can check if the deployment is present:
```bash
    kubectl get deployments -A
```


2. Create the dotnet REST API service

```bash
PORT_HTTP=${DOTNET_PORT_HTTP}
APP_VERSION=${DOTNET_APP_VERSION}
REST_API_NAME=${DOTNET_REST_API_NAME}

echo -e "
apiVersion: v1
kind: Service
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  type: LoadBalancer
  ports:
  - port: 80
    protocol: TCP
    targetPort: ${PORT_HTTP}              
  selector:
    app: ${REST_API_NAME}   
" | kubectl apply -f -

```

  you can also use the commands below to create the service:
```bash
    kubectl expose deploy dotnet-rest-api --type=ClusterIP --port=8000
    kubectl create -f service-dotnet-rest-api.yaml
```

  you can check if the service is present:
```bash
    kubectl get services -A   
```


3. Get Public IP address of the services and the dns name

```bash
    cmd="kubectl get services -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${DOTNET_REST_API_NAME}\")).status.loadBalancer.ingress[0].ip'"
    DOTNET_REST_API_PUBLIC_IP=$(eval "$cmd")
    echo "Public IP address: ${DOTNET_REST_API_PUBLIC_IP}"
    AKS_RESOURCE_GROUP=$(az aks show --name "${AKS_NAME}cluster" --resource-group "${AZURE_RESOURCE_GROUP}" | jq -r '.nodeResourceGroup')
    DOTNET_REST_API_PUBLIC_IP_ID=$(az network public-ip list -g ${AKS_RESOURCE_GROUP} --query "[?ipAddress!=null]|[?contains(ipAddress, '${DOTNET_REST_API_PUBLIC_IP}')].[id]" --output tsv)
    az network public-ip update --ids ${DOTNET_REST_API_PUBLIC_IP_ID} --dns-name ${AKS_NAME}

    DOTNET_REST_API_PUBLIC_DNS_NAME=$(az network public-ip list -g ${AKS_RESOURCE_GROUP} --query "[?ipAddress!=null]|[?contains(ipAddress, '${DOTNET_REST_API_PUBLIC_IP}')].[dnsSettings.fqdn]" --output tsv)
```

4. Check the pod status and pod name

```bash
    kubectl get pods -A
    cmd="kubectl get pods -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${DOTNET_REST_API_NAME}\")).metadata.name'"
    DOTNET_REST_API_POD=$(eval "$cmd")
    kubectl describe pods "${DOTNET_REST_API_POD}"
```

5. Get the logs associated with pod

```bash
    kubectl logs ${DOTNET_REST_API_POD}
```


6. Test the REST API with your browser

```bash
    curl http://${DOTNET_REST_API_PUBLIC_DNS_NAME}/version
```
```bash
    curl http://${DOTNET_REST_API_PUBLIC_DNS_NAME}/time
```

7. Delete dotnet REST API service

```bash
    kubectl delete  svc "${REST_API_NAME}"
```

8. Delete dotnet REST API deployment

```bash
    kubectl delete  deploy "${REST_API_NAME}"
```

### Deploy a container hosting the fastapi REST API with public access  

1. Create the fastapi REST API deployment

```bash
PORT_HTTP=${FASTAPI_PORT_HTTP}
APP_VERSION=${FASTAPI_APP_VERSION}
REST_API_NAME=${FASTAPI_REST_API_NAME}

echo -e "
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  replicas: 1
  selector:
    matchLabels:
      app: ${REST_API_NAME}
  template:
    metadata:
      labels:
        app: ${REST_API_NAME}
    spec:
      containers:
      - name: ${REST_API_NAME}
        image: ${ACR_LOGIN_SERVER}/${IMAGE_NAME}:${ALTERNATIVE_TAG}
        env:
        - name: PORT_HTTP
          value: \"${PORT_HTTP}\"
        - name: APP_ENVIRONMENT
          value: \"${APP_ENVIRONMENT}\"        
        - name: APP_VERSION
          value: \"${APP_VERSION}\"    
" | kubectl apply -f -

```
  you can also use the commands below to create the deployment:
```bash
    kubectl create deploy fastapi-rest-api --image=${ACR_NAME}.azurecr.io/fastapi-rest-api-image:latest
    kubectl create -f deployment-fastapi-rest-api.yaml
```

  you can check if the deployment is present:
```bash
    kubectl get deployments -A
```


2. Create the fastapi REST API service

```bash
PORT_HTTP=${FASTAPI_PORT_HTTP}
APP_VERSION=${FASTAPI_APP_VERSION}
REST_API_NAME=${FASTAPI_REST_API_NAME}

echo -e "
apiVersion: v1
kind: Service
metadata:
  name: ${REST_API_NAME}
  labels:
    app: ${REST_API_NAME}
spec:
  type: LoadBalancer
  ports:
  - port: 80
    protocol: TCP
    targetPort: ${PORT_HTTP}              
  selector:
    app: ${REST_API_NAME}   
" | kubectl apply -f -

```

  you can also use the commands below to create the service:
```bash
    kubectl expose deploy fastapi-rest-api --type=ClusterIP --port=8000
    kubectl create -f service-fastapi-rest-api.yaml
```

  you can check if the service is present:
```bash
    kubectl get services -A   
```


3. Get Public IP address of the services and the dns name

```bash
    cmd="kubectl get services -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${FASTAPI_REST_API_NAME}\")).status.loadBalancer.ingress[0].ip'"
    FASTAPI_REST_API_PUBLIC_IP=$(eval "$cmd")
    echo "Public IP address: ${FASTAPI_REST_API_PUBLIC_IP}"
    AKS_RESOURCE_GROUP=$(az aks show --name "${AKS_NAME}cluster" --resource-group "${AZURE_RESOURCE_GROUP}" | jq -r '.nodeResourceGroup')
    FASTAPI_REST_API_PUBLIC_IP_ID=$(az network public-ip list -g ${AKS_RESOURCE_GROUP} --query "[?ipAddress!=null]|[?contains(ipAddress, '${FASTAPI_REST_API_PUBLIC_IP}')].[id]" --output tsv)
    az network public-ip update --ids ${FASTAPI_REST_API_PUBLIC_IP_ID} --dns-name ${AKS_NAME}

    FASTAPI_REST_API_PUBLIC_DNS_NAME=$(az network public-ip list -g ${AKS_RESOURCE_GROUP} --query "[?ipAddress!=null]|[?contains(ipAddress, '${FASTAPI_REST_API_PUBLIC_IP}')].[dnsSettings.fqdn]" --output tsv)
```

4. Check the pod status and pod name

```bash
    kubectl get pods -A
    cmd="kubectl get pods -A -o json | jq -r '.items[] | select(.metadata.name | test(\"${FASTAPI_REST_API_NAME}\")).metadata.name'"
    FASTAPI_REST_API_POD=$(eval "$cmd")
    kubectl describe pods "${FASTAPI_REST_API_POD}"
```

5. Get the logs associated with pod

```bash
    kubectl logs ${FASTAPI_REST_API_POD}
```


6. Test the REST API with your browser

```bash
    curl http://${FASTAPI_REST_API_PUBLIC_DNS_NAME}/version
```
```bash
    curl http://${FASTAPI_REST_API_PUBLIC_DNS_NAME}/time
```
    kubectl delete  deploy "${REST_API_NAME}"
7. Delete fastapi REST API service    kubectl delete  deploy "${REST_API_NAME}"

```bash
    kubectl delete  svc "${REST_API_NAME}"
```

8. Delete fastapi REST API deployment

```bash
    kubectl delete  deploy "${REST_API_NAME}"
```

