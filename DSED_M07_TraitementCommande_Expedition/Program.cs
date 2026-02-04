using DSED_M07_Commandes;
using M07_GestionDesFichiers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DSED_M07_TraitementCommande_Expedition
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] requetesSujets = { "commande.placee.*" };

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
                    "m07-preparation-expedition",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                    foreach (var requeteSujet in requetesSujets)
                    {
                        channel.QueueBind(
                        queue: "m07-preparation-expedition",
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
                            string listArticle = "";

                            foreach (Article a in commande.Articles)
                            {
                                listArticle += $"{a.NomArticle}, ";
                            }
                            Console.Out.WriteLine($"Préparez les articles suivants : {listArticle}");
                            if (commande.estPremimum)
                            {
                                Console.Out.WriteLine("Il faut utiliser un emballage premium");
                            }
                            else
                            {
                                Console.Out.WriteLine("Il faut utiliser un emballage normal");
                            }
                        }

                        else
                        {
                            throw new Exception();
                        }

                    };
                    channel.BasicConsume(queue: "m07-preparation-expedition",
                    autoAck: true,
                    consumer: consumateur);

                    Console.Out.WriteLine("Appuyer sur [enter] pour quitter");
                    Console.In.ReadLine();
                }
            }

        }
    }
}
