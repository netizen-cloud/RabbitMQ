1 - First setup the Rabbit MQ server on the docker by using the below mentioned docker command.
docker run -d --hostname rmq --name rabbit-server2 -p 8089:15672 -p 5674:5672 rabbitmq:3-management
2 - Now connect the Receiver and Sender application by using domain name and port. 
3 - Currently it is using port 5674 to connect. if we setup another port while commissioning Rabbit server then we have to use this.
