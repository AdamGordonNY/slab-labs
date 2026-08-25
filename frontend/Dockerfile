# Development stage
FROM node:22-alpine AS dev
WORKDIR /app
RUN apk upgrade --no-cache
COPY package*.json ./
RUN npm ci
COPY . .
EXPOSE 3000
CMD ["npm", "run", "dev"]

# Production build stage
FROM node:22-alpine AS build
WORKDIR /app
RUN apk upgrade --no-cache
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production runtime
FROM node:22-alpine AS production
WORKDIR /app
RUN apk upgrade --no-cache
COPY --from=build /app/.next ./.next
COPY --from=build /app/public ./public
COPY --from=build /app/package*.json ./
RUN npm ci --omit=dev
EXPOSE 3000
CMD ["npm", "start"]
