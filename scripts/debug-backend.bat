@echo off
dapr run --app-id backend --components-path ../components/debug --app-port 8090 --dapr-http-port 8091 --dapr-grpc-port 8092