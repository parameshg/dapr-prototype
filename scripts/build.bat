@echo off
docker build -t parameshg/dapr-prototype-backend-api:latest -f ../backend/Dockerfile ../backend
docker build -t parameshg/dapr-prototype-frontend-api:latest -f ../frontend/Dockerfile ../frontend
docker build -t parameshg/dapr-prototype-security-api:latest -f ../security/Dockerfile ../security