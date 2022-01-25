@echo off
kubectl config use-context docker-desktop
kubectl apply -f ../infrastructure/namespace.yml
kubectl apply -f ../infrastructure/redis.yml
kubectl apply -f ../infrastructure/rabbitmq.yml
timeout 30
kubectl apply -f ../components/cache.yml
kubectl apply -f ../components/middleware.yml
timeout 30
kubectl apply -f ../api/backend.yml
kubectl apply -f ../api/frontend.yml
timeout 30
kubectl get all -n dapr
echo Run the following commands to forward ports from frontend (tcp/4000) and backend (tcp/5000) api
echo * kubectl port-forward service/frontend-dapr 4000:80 -n dapr
echo * kubectl port-forward service/backend-dapr 5000:80 -n dapr