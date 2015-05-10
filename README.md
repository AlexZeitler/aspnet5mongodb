# aspnet5mongodb

This repository contains the files for my talk about the deployment of ASP.NET 5 using Docker at dotnet cologne 2015 ([Slides](http://slides.com/alexzeitler/deck/live#/)).

The commands you need to link the MongoDb container with the application container are as follows:

```
# pull the MongoDb image and run the container
docker pull mongo:latest
docker run -v "$(pwd)":/data --name mongo -d mongo mongod --smallfiles
```

``` 
# build an Docker image for our application
sudo docker build -t mvc6mongo .
sudo docker run -t -d --link mongo:mongo --name aspnet5mongodbapp -p 80:5004 mvc6mongo
```

You should now be able to call ```http://localhost/api/speakers``` on your host.
