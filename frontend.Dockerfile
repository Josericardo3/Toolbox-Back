FROM nginx:1.25.4-bookworm-perl
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

