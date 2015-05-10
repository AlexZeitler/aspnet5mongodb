# aspnet5mongodb

This repository contains the files for my talk about the deployment of [ASP.NET 5](https://github.com/aspnet/home) using Docker at [dotnet cologne](http://dotnet-cologne.de/) 2015 ([Slides](http://slides.com/alexzeitler/deck/live#/)).

The commands you need to link the MongoDb container with the application container are as follows:

```
# pull the MongoDb image and run the container
docker pull mongo:latest
docker run -v "$(pwd)":/data --name mongo -d mongo mongod --smallfiles
```

``` 
# build an Docker image for our application
docker build -t mvc6mongo .
docker run -t -d --link mongo:mongo --name aspnet5mongodbapp -p 80:5004 mvc6mongo
```

You should now be able to call ```http://localhost/api/speakers``` on your host.

The process to automatically update your Docker Hub images by pushing your source code changes to GitHub, is described [here](https://docs.docker.com/docker-hub/builds/).

On your live server you can use [Docker-Hook](https://github.com/schickling/docker-hook) to await Docker Hub Hooks when the image has been updated.

You can replace your running containers using this sh script (I named it ```aspnet5mongodb.sh``` and placed it into the home folder of the live server:

```
#! /bin/bash
# aspnet5mongodb.sh

#if [ -z "$1" ]
echo "Getting currently running containers"
OLDPORTS=( `docker ps | grep aspnet5mongodb | awk '{print $1}'` )
echo "pulling new version"

docker pull alexzeitler/aspnet5mongodb

echo "removing old containers"
for i in ${OLDPORTS[@]}
do
        echo "removing old container $i"
        docker kill $i
done


echo "starting new containers"
#for i in `seq 1 $1` ; do
docker run -t -d -e mongodbconnectionString=mongodb://<user>:<password>@<yourdb>.mongolab.com:<port>/<databasename> -p 80:5004 alexzeitler/aspnet5mongodb
#done
```

Now you can run Docker-Hook using: 

```docker-hook -t aspnet5mongodb -c ~/aspnet5mongodb.sh```

Make sure Python is installed on your live server.

```
sudo apt-get update
sudo apt-get install build-essential -y

wget http://www.python.org/ftp/python/2.7.6/Python-2.7.6.tgz
tar -xzf Python-2.7.6.tgz  
cd Python-2.7.6

./configure  
make  
```

If you have any questions, just open an issue for this repository.
