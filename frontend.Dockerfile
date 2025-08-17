FROM nginx:1.29.1-bookworm-perl
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

