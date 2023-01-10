FROM nginx:stable
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

