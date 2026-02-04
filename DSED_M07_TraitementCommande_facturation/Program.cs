using DSED_M07_Commandes;
using M07_GestionDesFichiers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace DSED_M07_TraitementCommande_facturation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GestionDesFichiers gestionnaireDeMessage = new GestionDesFichiers();

            string dossier = "Facturation"; // Nom de dossier.


            if (!Directory.Exists(dossier)) // Création du dossier si non créer.
            {
                Directory.CreateDirectory(dossier);
            }

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
                    "m07-facturation",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                    foreach (var requeteSujet in requetesSujets)
                    {
                        channel.QueueBind(queue: "m07-facturation", exchange: "m07-commandes",
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

                            decimal factureAvecTaxe = CalculerFactureFinalAvecTaxe(commande);

                            decimal factureSansTaxe = CalculerFactureFinalSansTaxe(commande);

                            DonneesFacturation facture = new()
                            {
                                CommandeOriginale = commande,
                                SousTotal = factureSansTaxe,
                                TotalFinal = factureAvecTaxe
                            };

                            string factureJson = JsonSerializer.Serialize(facture);
                            byte[] factureBytes = Encoding.UTF8.GetBytes(factureJson);

                            string nomFichier = gestionnaireDeMessage.GenererNomFichierFacturation(); // création nom de fichier.

                            string chemin = Path.Combine(dossier, nomFichier); // création chemin du fichier.

                            try
                            {
                                File.WriteAllBytes(chemin, factureBytes);
                                Console.WriteLine($"[OK] Facture générée"); 
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ERREUR] Impossible d'écrire le fichier : {ex.Message}");
                            }
                        }

                        else
                        {
                            throw new Exception();
                        }
                    };


                    channel.BasicConsume(
                    queue: "m07-facturation",
                    autoAck: true,
                    consumer: consumateur
                    );

                    Console.Out.WriteLine("Appuyer [enter] pour quitter");
                    Console.In.ReadLine();
                }
            }

        }
        /// <summary>
        /// Fonction statique pour mes calcules de facture avec taxe. 
        /// Ligne 130. Pour le retour, ajoute du MidpointRounding.AwayFromZero pour avoir un calcul précis sur la troisième décimal.
        /// 4 et moins arrondi à la baisse et 5 et plus arrondi à la hausse. 
        /// </summary>
        /// <param name="p_commande"></param>
        /// <returns></returns>
        public static decimal CalculerFactureFinalAvecTaxe(Commande p_commande)
        {
            decimal facture = p_commande.Articles.Sum(article => article.Prix * article.Quantite);

            if (p_commande.estPremimum)
            {
                facture *= 0.95m;
            }

            return Math.Round(facture * 1.14975m, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Fonction statique pour mes calcules de facture avant taxe.
        /// </summary>
        /// <param name="p_commande"></param>
        /// <returns></returns>
        public static decimal CalculerFactureFinalSansTaxe(Commande p_commande)
        {
            decimal facture = p_commande.Articles.Sum(article => article.Prix * article.Quantite);

            if (p_commande.estPremimum)
            {
                facture *= 0.95m;
            }

            return facture;
        }
    }
}


