# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package.json and install dependencies
COPY package*.json ./
RUN npm ci

# Copy source code and build
COPY . .
RUN npm run build -- --configuration=docker

# Debug: List the contents of the dist directory
RUN ls -la /app/dist && ls -la /app/dist/task-management-ui || true

# Final stage with nginx
FROM nginx:1.25-alpine
COPY --from=build /app/dist/task-management-ui/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
