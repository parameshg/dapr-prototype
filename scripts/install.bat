@echo off
kubectl config use-context docker-desktop

kubectl apply -f ../infrastructure/namespace.yml
kubectl apply -f ../infrastructure/redis.yml
kubectl apply -f ../infrastructure/rabbitmq.yml
timeout 30

kubectl apply -f ../components/cache.yml
kubectl apply -f ../components/middleware.yml
kubectl apply -f ../components/secrets.yml
kubectl apply -f ../components/configuration.yml
kubectl apply -f ../components/uppercase.yml
kubectl apply -f ../components/throttler.yml
kubectl apply -f ../components/oauth.yml
kubectl apply -f ../components/tokenizer.yml
timeout 30

kubectl apply -f ../api/security-config.yml
kubectl apply -f ../api/backend-config.yml
kubectl apply -f ../api/frontend-config.yml
kubectl apply -f ../api/security.yml
kubectl apply -f ../api/backend.yml
kubectl apply -f ../api/frontend.yml
timeout 30

kubectl get all -n dapr

echo Execute following commands to forward ports from frontend (tcp/8080), backend (tcp/8090) and security (tcp/8888) api
echo * kubectl port-forward service/security-dapr 8888:80 -n dapr
echo * kubectl port-forward service/frontend-dapr 8080:80 -n dapr
echo * kubectl port-forward service/backend-dapr 8090:80 -n dapr