FROM nginx:1.27.2-bookworm-perl
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

