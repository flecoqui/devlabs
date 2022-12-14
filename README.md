# Azure Software Engineering Labs

Azure Software Engineering Labs

## Overview

This github repository contains several Labs associated with Azure Software Engineering.  

## The Labs

Below the list of Labs:  
    - **[Lab 1](./Lab1/README.md)**: Dev environment (Git, Visual Studio Code, Azure CLI, Docker, kubectl)</p>
    - **[Lab 2](./Lab2/README.md)**: Azure (Azure Portal, Azure Active Directory, Azure CLI, ARM template, Cognitive Services,...) </p>
    - **[Lab 3](./Lab3/README.md)**: Docker & Kubernetes</p>
    - **[Lab 4](./Lab4/README.md)**: FastAPI</p>

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

2. Install [Visual Studio Code](https://code.visualstudio.com/).

3. Install the [Remote Development extension pack](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack)

4. Install Git on your machine following the steps [here](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)   

## Next steps

Those Azure Software Engineering Labs could be extended with the following labs:  

1. Azure DevOps Pipelines  
2. Github Action  
