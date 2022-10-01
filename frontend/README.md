## Getting Started

First, run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.


## Building and Running in Docker

From the `frontend` directory, run the following command to build a docker image:

```
docker build -f ./src/frontend.Dockerfile -t searchle/frontend:latest ./src
```

Now run the following command to start a new container using the image:

```
docker run -d -p 3000:3000 --name searchle-frontent searchle/frontend:latest
```
