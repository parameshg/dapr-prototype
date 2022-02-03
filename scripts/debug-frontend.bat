@echo off
dapr run --app-id frontend --components-path ../components/debug --app-port 8080 --dapr-http-port 8081 --dapr-grpc-port 8082