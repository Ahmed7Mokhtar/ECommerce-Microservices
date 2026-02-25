FROM alpine:latest AS builder
WORKDIR /app
COPY mysql-init ./
RUN apk add --no-cache openssl

FROM mysql:latest

COPY --from=builder /app /docker-entrypoint-initdb.d

ENV MYSQL_ROOT_PASSWORD=admin

EXPOSE 3306

