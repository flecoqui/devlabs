# Azure (Azure Portal, Azure Active Directory, Azure CLI, ARM template, Cognitive Services,..) Lab

Azure (Azure Portal, Azure Active Directory, Azure CLI, ARM template, Cognitive Services,..) Lab

## Overview

This github repository folder contains the Azure Lab.  
During this lab, you will use Azure Portal, Azure Active Directory, Azure CLI and Azure Cognitive Services.

## The Lab

During this lab, you will see how to:
- send an invitation to a user to join an Azure Subscription,
- use Azure Portal to create, update and remove resources deployed in Azure,
- sign in with Azure CLI,
- use Azure CLI to create, update and remove resources deployed in Azure,
- use Azure CLI and ARM template to deploy resources in Azure,
- use Azure CLI and ARM template to deploy resources in Azure,
- use Azure Cognitive Service Computer Vision.

### Prerequisites

In order to run the labs, you need an internet connection and an Azure Subscription which will be required for the subsequent Labs. You can get further information about Azure Subscription [here](https://azure.microsoft.com/en-us/free).

Ideally, you could also use a Visual Studio dev container for the Azure CLI lab, the ARM template lab and the Cognitive Service lab.

### Azure Active Directory

If you have enough privilege on your Azure Active Directory, you'll be able to invite external user to collaborate with your organization.

Display your Azure Susbcription:

1. With your favorite browser open the url  https://portal.azure.com/ and enter your credentials (login/password).
2. In the Search edit box on the top bar, enter "subscription".
3. Click on the subscriptions icon.
4. The Susbcription page displays the Subscription Name and Subscription Id of the current Azure Subscription.

Display your Active Directory Tenant:

1. In the Search edit box on the top bar, enter "Azure Active Directory".
2. Click on the "Azure Active Directory" icon.
3. The Azure Active Directory page displays the Tenant Name and Tenant Id of the current user.

Invite user to your organization:

1. On the Azure Active Directory page, select the Users link.
2. On the User page, click on the link "+ New user", select "invite external user" on the popup menu.
3. On the New User Page enter the email address of the new user and edit the personal message.
4. Click on the button "Invite" to send the invite.
5. The Azure Active Directory page displays the Tenant Name and Tenant Id of the current user.

Add the new user as Reader of your Azure Susbcription:

1. In the Search edit box on the top bar, enter "subscription".
2. Click on the subscriptions icon.
3. The Susbcription page displays the Subscription Name and Subscription Id of the current Azure Subscription.
4. Click on the subscription Name.
5. On the subscription page, select 'Access Control (IAM)' link
6. Click on the "+ Add" button, and on "Add role Assignment" on the pop up menu
7. Select role "Reader" on the "Add role assignment" page and "Role" tab, then click on "Next" button.
8. On the "Members" tab, click on "+ select members", select the new user in the list and click on "Select" button. 
9. On the "Members" tab, Click on the "Review + assign" to confirm the role assignment.
10. The new user will have access to the new subscription as a Reader.

Switch directory:

1. On the top bar on the right, click on the login icon and on the "Switch directory" link.
2. Select your subscription in the combo box "Default subscription filter"
3. Select your directory in the "All Directories" list.

### Create resources with Azure Portal

Create a resource group:

1. With your favorite browser open the url  https://portal.azure.com/ and enter your credentials (login/password).
2. In the Search edit box on the top bar, enter "resource groups".
3. Click on the "Resource groups" icon.
4. On the Resource groups page, click on button "+ Create", enter the resource group name and  select the Azure Region where you want to deploy the resource group.
5. Click on the "Review + Create" button and then on the "Create" button.

Create an Azure Storage in the new resource group:

1. On the Resource groups page, select the new resource group.
2. Click on button "+ Create".
3. On the Marketplace page, enter "Storage Account" in the search edit box.
4. On the Storage Account panel, click on the Create button.
5. On the page "Create a Storage Account", select the resource group, enter the storage account name, select the region, select the performance "Standard", select the redundancy to "Locally Redundant Storage (LRS)".
6. Click on the "Review" button and then on the "Create" button.


Upload image files on Azure Storage:

1. On the Resource groups page, select the new resource group.
2. On the new page, select the Azure Storage Account.
3. On the Storage Account page click on the Upload button. 
4. On the "Upload blob" page, click on the "Create new" link.
5. On the "New container" page enter "images", leave private access and click on the Ok button.
6. On the "Upload blob" page,  click on the upload button and select the files to upload in the img folder.
7. On the "Upload blob" page, click on the Upload  button.
8. On the Storage Account page, click on the Containers link, and on the "images" container to check the files in this container. 
9. Select a file, right-click on the file, select "Generate SAS" on the popup menu.
10. On the file page, click on the "Generate SAS token and URL" button.
11. Copy Blob SAS URL and open the URL using your browser which should display the image.


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
