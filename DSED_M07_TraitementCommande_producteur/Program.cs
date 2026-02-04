using DSED_M07_Commandes;
using M07_GestionDesFichiers;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class Program
{
    // Afin de faire fonctionne RABBITMQ : docker run --rm -d --name s4_DSED_rabbitmq -p 8080:15672 -p 5672:5672 rabbitmq:3-management

    const int NOMBRE_MESSAGE = 5; // Gestion du nombre de message envoyé.
    private static readonly Random m_random = new();
    static void Main(string[] args)
    {
        string sujet = "";
        string message = "";

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

                for (int index = 0; index < NOMBRE_MESSAGE; index++)
                {
                    Commande commande = new();
                    Article article = new();

                    int selectionComptePremium = m_random.Next(1, 3); // Entre 1 et 2

                    string clientAleatoire = GenereDonnee.GenererClient();

                    commande.NomClient = clientAleatoire;

                    var articlesAleatoires = GenereDonnee.GenererArticlesAleatoires();

                    foreach (var a in articlesAleatoires)
                    {
                        article.NomArticle = a.Nom; // Utilisation des tuples.
                        article.Prix = a.Prix; // Utilisation des tuples.
                        article.Quantite = m_random.Next(1, 11); // Entre 1 et 10.
                        commande.Articles.Add(article);
                    }

                    if (selectionComptePremium == 1) // 1 est pour premium
                    {
                        commande.estPremimum = true;
                        sujet = $"commande.placee.premium";

                        Enveloppe<Commande> envelope = new()
                        {
                            Donnee = commande
                        };

                        message = JsonSerializer.Serialize(envelope);
                    }

                    else if (selectionComptePremium == 2) // 2 est pour normal
                    {
                        commande.estPremimum = false;
                        sujet = $"commande.placee.normal";

                        Enveloppe<Commande> envelope = new()
                        {
                            Donnee = commande
                        };

                        message = JsonSerializer.Serialize(envelope);
                    }

                    Console.Out.WriteLine("Commande envoyé");

                    byte[] body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                    exchange: "m07-commandes",
                    routingKey: sujet,
                    basicProperties: null,
                    body: body
                    );
                }
            }
        }
    }
}
