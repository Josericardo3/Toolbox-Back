FROM nginx:1.26.2-bookworm-perl
WORKDIR /front
COPY ./dist ./dist
WORKDIR /front
RUN ls -lash

