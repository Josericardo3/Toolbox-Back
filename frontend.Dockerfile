FROM nginx:1.29.0-bookworm-perl
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

