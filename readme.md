# KubeLife

##Simple ML Ops Tool

We were in need for simple UI help us about our ML projects at our Openshift environment. So I wrote a simple web page. Hope you enjoy.
Basicly you could trigger builds, monitor corn jobs, look your logs...
I am planing to add more features if I will have time. Open to suggestions ;)

#### Notes

*First of all, when i started this project, I tought it can be Kubernetes oriented. But our datascientist demanded other features and it become more focused on Openshift features like BuildConfig, DeploymentConfig, Routes,...*

*Our main approach to deploy an application or cronjobs is giving same name to the each objects in Openshift. Every name is same at DeploymentConfig, Services, Routes, Cronjobs, etc... So this make easy to trigger anything as we like to.*

## Table of contents

- [General info](#general-info)
- [Technologies](#technologies)
- [Installation](#Installation)

## General info

This project is for displaying kubernetes cronjobs.
It is automatically queries Kubernetes. Configuration for kubernetes resides in "secret.app.yaml" file. There is 4 different values in this YAML file:

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

- ASP.NET Core 6
- Blazor App
- Helm v3

## Installation

Download&Install Helm package manager and run this command:

```bash
helm install KubeLifeapp [helm package location]
```

That is it :)

Just do not forget to change "secret.app.yaml" file with your system values.

**More detail:**
This project is for kubernetes so logical way to deploy is kubernetes. You can find helm packages under helm folder.
Helm is a tool that automates the creation, packaging, configuration, and deployment of Kubernetes applications by combining your configuration files into a single reusable package

My kubernetes environment is Openshift. So I prefer DeploymenyConfig over Deployment. But I put commented Deployment.yaml to the helm folders. If your environment not support DeploymentConfig, comment out it and uncomment Deployment.yaml ;)
