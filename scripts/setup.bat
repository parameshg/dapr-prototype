@echo off
kubectl create namespace dapr-system
helm repo add dapr https://daprio.azureacr.io/helm/v1/repo
helm repo update
helm install dapr dapr/dapr --namepace dapr-system