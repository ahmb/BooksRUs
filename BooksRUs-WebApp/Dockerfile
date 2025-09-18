# ---- build static assets ----
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
# keep your current VITE_* args/env if you’re passing them
ARG VITE_API_BASE_URL
ARG VITE_DEFAULT_USER_ID=u1-demo
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
ENV VITE_DEFAULT_USER_ID=$VITE_DEFAULT_USER_ID
RUN npm run build

# ---- serve via nginx with SPA fallback ----
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
