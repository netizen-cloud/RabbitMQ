using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5674");

connectionFactory.ClientProvidedName = "Sender RabbitMQ";

using var connection =await connectionFactory.CreateConnectionAsync();
using var channel =await connection.CreateChannelAsync();


string _exchangeName = "Demo Exchange";
string _queueName = "Demo Queue";
string _routingKey = "Demo Queue";

await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);
await channel.QueueDeclareAsync(_queueName, false, false, false, null);
await channel.QueueBindAsync(_queueName, _exchangeName, _routingKey, null);


BasicProperties properties = new BasicProperties();

Console.WriteLine("Type any message to send to Rabbit MQ");
string input = string.Empty;
while (input != "exit")
{
    input = Console.ReadLine();
    
    byte[] msgBody = Encoding.UTF8.GetBytes($"{input} : {DateTime.Now}");
    
    await channel.BasicPublishAsync(_exchangeName, _routingKey, mandatory:true, basicProperties:properties, msgBody );
    
    Console.WriteLine($"[x] {input} [Sent], \n To send another one to Rabbit MQ or Type \"exit\" to exit");
}

await channel.CloseAsync();

await connection.CloseAsync();







