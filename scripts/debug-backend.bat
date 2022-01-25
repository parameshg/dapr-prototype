@echo off
dapr run --app-id backend --components-path ../components/debug --app-port 5000 --dapr-http-port 5001 --dapr-grpc-port 5002