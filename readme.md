# KUBECRONMONITOR
##Simple Kubernetes Cron Job Monitor

We were in need for cron job monitor for kubernetes. So I wrote a simple web page. Hope you enjoy.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Install](#Install)

## General info
This project is for displaying kubernetes cronjobs. 
It is automatically queries Kubernetes. Configuration for kubernetes resides in  "secret.app.yaml" file. There is 4 different values in this YAML file:

```yaml
"KubernetesSetting:ServerUrl": QWRkWW91clNlY3JldA==
"KubernetesSetting:AccessToken": QWRkWW91clNlY3JldA==
"KubernetesSetting:UserName": QWRkWW91clNlY3JldA==
"KubernetesSetting:PassWord": QWRkWW91clNlY3JldA==
```

change these secrets according to your environment. If you do not know these ask your admin :) 

"KubernetesSetting:ServerUrl" is mandotary. Others are optional.

I only use "KubernetesSetting:ServerUrl" and "KubernetesSetting:AccessToken" pair.
	
## Technologies
Project is created with:
* ASP.NET Core 6 Blazor App
* Helm v3
	
## Install
Download&Install Helm package manager and run this command:

```bash
helm install kubecronmonitorapp [helm package location]
```

That is it :)

**More detail:**
This project is for kubernetes so logical way is to deploy to kubernetes. You can find helm packages under helm folder.
Helm is a tool that automates the creation, packaging, configuration, and deployment of Kubernetes applications by combining your configuration files into a single reusable package

Just do not forget to change "secret.app.yaml" file with your system values.







