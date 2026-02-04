using DSED_M07_Commandes;
using M07_GestionDesFichiers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DSED_M07_TraitementCommande_journal
{
    public class Program
    {
        static void Main(string[] args)
        {
            GestionDesFichiers gestionnaireDeMessage = new GestionDesFichiers();

            string dossier = "Journalisation"; // Nom de dossier.


            if (!Directory.Exists(dossier)) // Création du dossier si non créer.
            {
                Directory.CreateDirectory(dossier);
            }

            string[] requetesSujets = { "#" };

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
                    "m07-journal",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                    foreach (var requeteSujet in requetesSujets)
                    {
                        channel.QueueBind(queue: "m07-journal", exchange: "m07-commandes",
                            routingKey: requeteSujet);
                    }

                    EventingBasicConsumer consumateur = new EventingBasicConsumer(channel);
                    consumateur.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        string sujet = ea.RoutingKey;

                        string nomFichier = gestionnaireDeMessage.GenererNomFichierJournal(); // création nom de fichier.

                        string chemin = Path.Combine(dossier, nomFichier); // création chemin du fichier.

                        try
                        {
                            File.WriteAllBytes(chemin, body);
                            Console.Out.WriteLine($"[OK] Message enregistré : ");
                            Console.Out.WriteLine($"{nomFichier}");
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine($"[ERREUR] Impossible d'écrire le fichier : {ex.Message}");
                        }
                    };

                    channel.BasicConsume(
                    queue: "m07-journal",
                    autoAck: true,
                    consumer: consumateur
                    );

                    Console.Out.WriteLine("Appuyer [enter] pour quitter");
                    Console.In.ReadLine();
                }
            }
        }
    }
}
