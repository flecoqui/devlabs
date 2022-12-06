# Dev environment Lab

Dev environment Lab

## Overview

In this lab, you will install some tools required to build, debug, deploy and test services running on Azure. Those tools will be installed either directly on your machine or in your dev container.
You will install:

- Visual Studio Code
- Git
- Docker
- Azure CLI
- Kubectl
- Dev Container
  
## The Lab

In this lab you will install:

- Visual Studio Code
- Git
- Docker
- Azure CLI
- Kubectl
- Dev Container

### Pre-requisites

In order to run the labs, you need an internet connection and an Azure Subscription which will be required for the subsequent Labs. You can get further information about Azure Subscription [here](https://azure.microsoft.com/en-us/free).

### Installing Visual Studio Code

1. Install [Visual Studio Code](https://code.visualstudio.com/), click on the links below. 

|[![Windows](./img/windows_logo.png)](https://git-scm.com/download/win) |[![Linux](./img/linux_logo.png)](https://git-scm.com/download/linux)|[![MacOS](./img/macos_logo.png)](https://git-scm.com/download/mac)  |
| :--- | :--- | :--- |
| [Visual Studio Code for Windows](https://code.visualstudio.com/Download)  | [Visual Studio Code for Linux](https://code.visualstudio.com/Download)  &nbsp;| [Visual Studio Code for MacOS](https://code.visualstudio.com/Download) &nbsp; &nbsp;|

2. Install the [Remote Development extension pack](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack)

### Installing Git Client

You also need to install Git client on your machine and Visual Studio Code, below the links.

|[![Windows](./img/windows_logo.png)](https://git-scm.com/download/win) |[![Linux](./img/linux_logo.png)](https://git-scm.com/download/linux)|[![MacOS](./img/macos_logo.png)](https://git-scm.com/download/mac)  |
| :--- | :--- | :--- |
| [Git Client for Windows](https://git-scm.com/download/win) | [Git client for Linux](https://git-scm.com/download/linux)| [Git Client for MacOs](https://git-scm.com/download/mac) |

Once the Git client is installed you can clone the repository on your machine running the following commands:

1. Create a Git directory on your machine

    ```bash
        c:\> mkdir git
        c:\> cd git
        c:\git>
    ```

2. Clone the repository.  
    For instance:

    ```bash
        c:\git> git clone https://github.com/flecoqui/devlabs.git
        c:\git> cd devlabs 
        c:\git\devlabs> 
    ```

### Installing Docker

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

### Installing Azure CLI

In order to deploy resources in Azure, you need to install Azure CLI on your machine.

- Windows:  
Follow the instructions on this page:
https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli

- MacOS:  
Follow the instructions on this page:
https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-macos

- Linux:  
Follow the instructions on this page:
https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-linux?pivots=apt

Once, Azure CLI is installed on your machine, you can run the following command to check the version installed on your machine:

```bash
    az --version
```

### Installing Kubectl

To manage a Kubernetes cluster, we will use the Kubernetes command-line client: [kubectl](https://kubernetes.io/docs/reference/kubectl/kubectl/).

If Azure CLI is already installed on your machine, you can run the following command to install kubectl:

```bash
    az aks install-cli
```

If Azure CLI is not installed on your machine, you can install kubectl directly on your machine.

- Windows:  
Follow the instructions on this page:
https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/

- MacOS:  
Follow the instructions on this page:
https://kubernetes.io/docs/tasks/tools/install-kubectl-macos/

- Linux:  
Follow the instructions on this page:
https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/

Once kubectl is installed, you can check the version installed on your machine running the following command:

```bash
    kubectl version --client --output=yaml
```

### Using Visual Studio Code Dev Container

This repository contains a folder called '.devcontainer'.

When you'll open the project with Visual Studio Code, it will ask you to open the project in container mode provided some pre-requisites are installed on your machine.

#### Installing Visual Studio Code and Docker

You need to install the following pre-requisite on your machine

1. Install and configure [Docker](https://www.docker.com/get-started) for your operating system.

   - Windows:

     1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop) for Windows.

     2. Right-click on the Docker task bar item, select Settings / Preferences and update Resources > File Sharing with any locations your source code is kept. See [tips and tricks](https://code.visualstudio.com/docs/remote/troubleshooting#_container-tips) for troubleshooting.

     3. Enable the [Windows WSL 2 back-end](https://docs.docker.com/docker-for-windows/wsl/): Right-click on the Docker taskbar item and select Settings. Check Use the WSL 2 based engine and verify your distribution is enabled under Resources > WSL Integration.

   - macOS:

     1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop) for Mac.

     2. Right-click on the Docker task bar item, select Settings / Preferences and update Resources > File Sharing with any locations your source code is kept. See [tips and tricks](https://code.visualstudio.com/docs/remote/troubleshooting#_container-tips) for troubleshooting.

   - Linux:

     1. Follow the official install [instructions for Docker CE/EE for your distribution](https://docs.docker.com/get-docker/). If you are using Docker Compose, follow the [Docker Compose directions](https://docs.docker.com/compose/install/) as well.

     2. Add your user to the docker group by using a terminal to run:  
        "sudo usermod -aG docker $USER"

     3. Sign out and back in again so your changes take effect.

2. Install [Visual Studio Code](https://code.visualstudio.com/).

3. Install the [Remote Development extension pack](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack)

#### Using dev container

1. Launch Visual Studio Code in the folder where you stored the devlabs repository

    ```bash
        c:\git\devlabs\Lab1> code .
    ```

2. Once Visual Studio Code is launched, you should see the following dialgog box:

    ![Visual Studio Code](./img/reopen-in-container.png)

3. Click on the button 'Reopen in Container'
4. Visual Studio Code will now open the dev container. If it's the first time you open the project in container mode, it will first build the container, it can take several minutes to build the new container.
5. Once the container is loaded, you can open a new terminal (Terminal -> New Terminal).
6. And you have access to the tools installed in the dev container like az client,....
   Check Azure CLI version:

    ```bash
        vscode ➜ /workspace $ az --version
    ```

   Check Docker version:

    ```bash
        vscode ➜ /workspace $ docker --version
    ```

   Display Dev Container :

    ```bash
        vscode ➜ /workspace $ docker ps
    ```

   Check kubectl version:

    ```bash
        vscode ➜ /workspace $ kubectl version --client --output=yaml
    ```

#### Using the dev container for all the labs

As most of the subsequent labs could be run from the dev container, if you copy the folder "\git\devlabs\Lab1\.devcontainer" into "\git\devlabs", the same dev container will be available for all the labs.

1. Launch Visual Studio Code in the folder where you stored the devlabs repository

    ```bash
        c:\git\devlabs> code .
    ```

2. Once Visual Studio Code is launched, you should see the following dialgog box:

    ![Visual Studio Code](./img/reopen-in-container.png)

3. Click on the button 'Reopen in Container'
4. Visual Studio Code will now open the dev container. If it's the first time you open the project in container mode, it will first build the container, it can take several minutes to build the new container.
5. Once the container is loaded, you can open a new terminal (Terminal -> New Terminal).
6. And you have access to the tools installed in the dev container like az client,....
   Check Azure CLI version:

    ```bash
        vscode ➜ /workspace $ az --version
    ```