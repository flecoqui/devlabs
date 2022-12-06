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

### ARM (Azure Resource Manager) Template

### Azure Cognitive Services

