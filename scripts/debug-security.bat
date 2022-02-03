@echo off
dapr run --app-id security --components-path ../components/debug --app-port 8888 --dapr-http-port 8001 --dapr-grpc-port 8002