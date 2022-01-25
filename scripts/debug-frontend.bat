@echo off
dapr run --app-id frontend --components-path ../components/debug --app-port 4000 --dapr-http-port 4001 --dapr-grpc-port 4002