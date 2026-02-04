using DSED_M07_Commandes;
using M07_GestionDesFichiers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DSED_M07_TraitementCommande_CourrielsPremium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] requetesSujets = { "commande.placee.premium" };

            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                    exchange: "m07-commandes",
                    type: "topic",
                    durable: true,
                    autoDelete: false
                    );

                    channel.QueueDeclare(
                    "m07-courriel-premium",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                    foreach (var requeteSujet in requetesSujets)
                    {
                        channel.QueueBind(
                        queue: "m07-courriel-premium",
                        exchange: "m07-commandes",
                        routingKey: requeteSujet);
                    }

                    EventingBasicConsumer consumateur = new EventingBasicConsumer(channel);

                    consumateur.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        string sujet = ea.RoutingKey;

                        Enveloppe<Commande> enveloppe = JsonSerializer.Deserialize<Enveloppe<Commande>>(message);

                        if (enveloppe.Entite == "commande")
                        {
                            Commande commande = enveloppe.Donnee;
                            Console.Out.WriteLine($"Commande premium : {commande.Reference}");
                        }

                        else
                        {
                            throw new Exception();
                        }

                    };

                    channel.BasicConsume(
                    queue: "m07-courriel-premium",
                    autoAck: true,
                    consumer: consumateur
                    );

                    Console.Out.WriteLine("Appuyer sur [enter] pour quitter");
                    Console.In.ReadLine();
                }
            }
        }
    }
}