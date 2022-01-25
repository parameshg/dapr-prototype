@echo off
kubectl config use-context docker-desktop
kubectl delete -f ../infrastructure/namespace.yml