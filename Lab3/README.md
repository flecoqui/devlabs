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
    docker run -it ubuntu bash    
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
    docker volume rm --name test_volume 
```

### Building Docker image

1. Run hello-world:
```bash
    docker ps 
```


### Azure CLI

Sign in interactively: 

1. Run the following command in the dev container terminal

```bash
    az login
```
    Azure CLi open the default browser to load an Azure sign-in page.

2. Once connected, run the following command to display the Azure Subcription Id and the Tenant Id:

```bash
    az account show 
```

3. If the susbcription Id (.id) is not the expected one, you can change it using the command below:

```bash
    az account set --subscription ${SUBSCRIPTION_ID} 
```

4. You can also display the id of the current user with the following command:

```bash
    az ad signed-in-user show --query id --output tsv
```

Sign in with a Service principal

1. Run the following commands to create the service principal

```bash
    SERVICE_PRINCIPAL_NAME=testsp2222
    AZURE_SUBSCRIPTION_ID=$(az account show  | jq -r .id)
    AZURE_TENANT_ID=$(az account show  | jq -r .tenantId)
    PASSWORD=$(az ad sp create-for-rbac --name "${SERVICE_PRINCIPAL_NAME}" --role contributor --scopes "/subscriptions/${AZURE_SUBSCRIPTION_ID}" --query "password" --output tsv)
    USER_NAME=$(az ad sp list --display-name $SERVICE_PRINCIPAL_NAME --query "[].appId" --output tsv)
    echo "Service principal ID: ${USER_NAME}"
    echo "Service principal password: ${PASSWORD}"
```

2. Run the following commands to create the service principal

```bash
    az login --service-principal -u ${USER_NAME} -p ${PASSWORD} --tenant ${AZURE_TENANT_ID}
```

3. Run the following commands to display create the service principal id used for the connnection

```bash
    az ad sp show --id "$(az account show | jq -r .user.name)" --query id --output tsv  
```

### Deploy and use Azure Storage Account with Azure CLI

1. Run the following command in the dev container terminal

```bash
    cd ./Lab2
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
    AZURE_RESOURCE_GROUP=rgteststo$(shuf -i 1000-9999 -n 1)
    az group create  --subscription $AZURE_SUBSCRIPTION_ID --location $AZURE_REGION --name $AZURE_RESOURCE_GROUP 
```

4. Create the Azure Storage Account

```bash
    STORAGE_ACCOUNT=teststo$(shuf -i 1000-9999 -n 1)
    az storage account create -n ${STORAGE_ACCOUNT} -g ${AZURE_RESOURCE_GROUP} -l westus  --sku Standard_LRS
```

5. Create role assignment for the Azure Storage Account

```bash
    AZURE_USER_ID=$(az ad signed-in-user show --query id --output tsv)
    az role assignment create --assignee-object-id ${AZURE_USER_ID} --assignee-principal-type "User" --scope /subscriptions/"${AZURE_SUBSCRIPTION_ID}"/resourceGroups/"${AZURE_RESOURCE_GROUP}"/providers/Microsoft.Storage/storageAccounts/"${STORAGE_ACCOUNT}" --role "Storage Blob Data Contributor"
```

6. Create the Azure Storage Account Container

```bash
    CONTAINER_NAME=images
    az storage container create --name ${CONTAINER_NAME} --account-name ${STORAGE_ACCOUNT} --auth-mode login  
```

7. Upload files in the container

```bash
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/linux_logo.png  --name linux_logo.png 
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/macos_logo.png  --name macos_logo.png 
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/windows_logo.png  --name windows_logo.png 
```

8. Create the SAS Token for the container

```bash
    AZURE_CONTENT_STORAGE_CONTAINER_URL="https://${STORAGE_ACCOUNT}.blob.core.windows.net/${CONTAINER_NAME}"
    end=$(date -u -d "7 days" '+%Y-%m-%dT%H:%MZ')
    AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN="$(az storage container generate-sas --account-name "$STORAGE_ACCOUNT"  --as-user  --auth-mode login  -n "$CONTAINER_NAME" --https-only --permissions dlrw --expiry "$end" -o tsv)"
```

9. Download images with curl

```bash
    curl -o ./testwindows_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/windows_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
    curl -o ./testmacos_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/macos_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
    curl -o ./testlinux_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/linux_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
```

10. Delete the resource group

```bash
    az group delete  --subscription $AZURE_SUBSCRIPTION_ID  --name $AZURE_RESOURCE_GROUP 
```

### Deploy and use Azure Storage Account with Azure CLI and ARM (Azure Resource Manager) Template

1. Run the following command in the dev container terminal

```bash
    cd ./Lab2
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
    AZURE_RESOURCE_GROUP=rgteststo$(shuf -i 1000-9999 -n 1)
    az group create  --subscription $AZURE_SUBSCRIPTION_ID --location $AZURE_REGION --name $AZURE_RESOURCE_GROUP 
```

4. Create the Azure Storage Account

```bash
    STORAGE_ACCOUNT=teststo$(shuf -i 1000-9999 -n 1)    
    CONTAINER_NAME=images
    POST_DEPLOYMENT_NAME="POST-$(date +"%y%m%d-%H%M%S")"
    az deployment group create \
        --name $POST_DEPLOYMENT_NAME \
        --resource-group ${AZURE_RESOURCE_GROUP} \
        --subscription ${AZURE_SUBSCRIPTION_ID} \
        --template-file azuredeploy-storage.json \
        --output none \
        --parameters \
        storageAccountName=${STORAGE_ACCOUNT} containerName=${CONTAINER_NAME}
```

5. Create role assignment for the Azure Storage Account

```bash
    AZURE_USER_ID=$(az ad signed-in-user show --query id --output tsv)
    az role assignment create --assignee-object-id ${AZURE_USER_ID} --assignee-principal-type "User" --scope /subscriptions/"${AZURE_SUBSCRIPTION_ID}"/resourceGroups/"${AZURE_RESOURCE_GROUP}"/providers/Microsoft.Storage/storageAccounts/"${STORAGE_ACCOUNT}" --role "Storage Blob Data Contributor"
```

6. Upload files in the container

```bash
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/linux_logo.png  --name linux_logo.png 
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/macos_logo.png  --name macos_logo.png 
    az storage blob upload --overwrite --no-progress --account-name "${STORAGE_ACCOUNT}"    --auth-mode login   --container-name "${CONTAINER_NAME}"  --file ./img/windows_logo.png  --name windows_logo.png 
```

7. Create the SAS Token for the container

```bash
    AZURE_CONTENT_STORAGE_CONTAINER_URL="https://${STORAGE_ACCOUNT}.blob.core.windows.net/${CONTAINER_NAME}"
    end=$(date -u -d "7 days" '+%Y-%m-%dT%H:%MZ')
    AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN="$(az storage container generate-sas --account-name "$STORAGE_ACCOUNT"  --as-user  --auth-mode login  -n "$CONTAINER_NAME" --https-only --permissions dlrw --expiry "$end" -o tsv)"
```

8. Download images with curl

```bash
    curl -o ./testwindows_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/windows_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
    curl -o ./testmacos_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/macos_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
    curl -o ./testlinux_logo.png ${AZURE_CONTENT_STORAGE_CONTAINER_URL}/linux_logo.png?${AZURE_CONTENT_STORAGE_CONTAINER_SAS_TOKEN} 
```

9. Delete the resource group

```bash
    az group delete  --subscription $AZURE_SUBSCRIPTION_ID  --name $AZURE_RESOURCE_GROUP 
```

### Azure Cognitive Services: Computer Vision

1. Run the following command in the dev container terminal

```bash
    cd ./Lab2
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
    AZURE_RESOURCE_GROUP=rgtestcv$(shuf -i 1000-9999 -n 1)
    az group create  --subscription $AZURE_SUBSCRIPTION_ID --location $AZURE_REGION --name $AZURE_RESOURCE_GROUP 
```

4. Create the Azure Computer Vision Account

```bash
    COMPUTER_VISION_ACCOUNT=testcompvision$(shuf -i 1000-9999 -n 1)    
    POST_DEPLOYMENT_NAME="POST-$(date +"%y%m%d-%H%M%S")"
    az deployment group create \
        --name $POST_DEPLOYMENT_NAME \
        --resource-group ${AZURE_RESOURCE_GROUP} \
        --subscription ${AZURE_SUBSCRIPTION_ID} \
        --template-file azuredeploy-computervision.json \
        --output none \
        --parameters \
        computerVisionAccountName=${COMPUTER_VISION_ACCOUNT} 
    COMPUTER_VISION_KEY=$(az deployment group show --resource-group "$AZURE_RESOURCE_GROUP" -n "$POST_DEPLOYMENT_NAME" | jq -r '.properties.outputs.computerVisionKey.value')
    COMPUTER_VISION_ENDPOINT=$(az deployment group show --resource-group "$AZURE_RESOURCE_GROUP" -n "$POST_DEPLOYMENT_NAME" | jq -r '.properties.outputs.computerVisionEndpoint.value')
    echo "COMPUTER_VISION_ACCOUNT: ${COMPUTER_VISION_ACCOUNT}"
    echo "COMPUTER_VISION_KEY: ${COMPUTER_VISION_KEY}"
    echo "COMPUTER_VISION_ENDPOINT: ${COMPUTER_VISION_ENDPOINT}"    
```

5. Upload files in the container

```bash
    curl -i -X POST  --data-binary "@./img/frame.jpg" "https://${AZURE_REGION}.api.cognitive.microsoft.com/vision/v3.2/analyze?visualFeatures=Objects,Tags&details=Landmarks&language=en&model-version=latest" -H "Content-Type: application/octet-stream" -H "Ocp-Apim-Subscription-Key: ${COMPUTER_VISION_KEY}"
    
    curl -i -X POST  --data-binary "@./img/frameegypt.jpg" "https://${AZURE_REGION}.api.cognitive.microsoft.com/vision/v3.2/analyze?visualFeatures=Objects,Tags&details=Landmarks&language=en&model-version=latest" -H "Content-Type: application/octet-stream" -H "Ocp-Apim-Subscription-Key: ${COMPUTER_VISION_KEY}"
```


6. Delete the resource group

```bash
    az group delete  --subscription $AZURE_SUBSCRIPTION_ID  --name $AZURE_RESOURCE_GROUP 
```
