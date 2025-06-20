# Build stage
FROM node:22-alpine AS build

WORKDIR /app

# Update packages for security
RUN apk update && apk upgrade && apk add --no-cache dumb-init

# Copy package files
COPY package*.json ./

# Check if pnpm-lock.yaml exists and copy it
COPY pnpm-lock.ya[r]l ./pnpm-lock.yaml* ./

# Install pnpm and dependencies
RUN npm install -g pnpm@latest

# Update lockfile if needed, then install
RUN pnpm install

# Copy source code
COPY . .

# Set production environment for Vite build
ENV NODE_ENV=production

# Build the application for production with explicit mode
RUN pnpm run build --mode production

# Production stage
FROM nginx:alpine

# Copy built app to nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Create non-root user
RUN addgroup -g 1001 -S nodejs
RUN adduser -S nextjs -u 1001

# Set proper permissions
RUN chown -R nextjs:nodejs /usr/share/nginx/html
RUN chown -R nextjs:nodejs /var/cache/nginx
RUN chown -R nextjs:nodejs /var/log/nginx
RUN chown -R nextjs:nodejs /etc/nginx/conf.d
RUN touch /var/run/nginx.pid
RUN chown -R nextjs:nodejs /var/run/nginx.pid

USER nextjs

# Expose port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
