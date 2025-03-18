using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5674");

connectionFactory.ClientProvidedName = "Receiver RabbitMQ";

using var connection =await connectionFactory.CreateConnectionAsync();
using var channel =await connection.CreateChannelAsync();


string _exchangeName = "Demo Exchange";
string _queueName = "Demo Queue";
string _routingKey = "Demo Queue";

await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);
await channel.QueueDeclareAsync(_queueName, false, false, false, null);
await channel.QueueBindAsync(_queueName, _exchangeName, _routingKey, null);

await channel.BasicQosAsync(0, 1 , false);
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, args) =>
{
    
    // 
   // Console.WriteLine($"Inside msg received block");
    Thread.Sleep(TimeSpan.FromSeconds(3));
    var body = args.Body.ToArray();
    String msg = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {msg}");
    await channel.BasicAckAsync(args.DeliveryTag, false);
};



var cosumerTag = channel.BasicConsumeAsync(_queueName , false , consumer);

//string test_commit = "new Commit";

Console.ReadLine();

await channel.CloseAsync();

await connection.CloseAsync();