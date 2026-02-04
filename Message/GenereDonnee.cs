namespace DSED_M07_Commandes
{
    // Classe généré par IA afin de me créer des listes de nom de client ainsi que d'articles. 
    public static class GenereDonnee

    {
        private static readonly Random m_random = new Random();

        private static readonly List<string> m_clients = new List<string>
    {
        "Antoine Masson", "Sophie Tremblay", "Marc-André Roy", "Élodie Gauthier", "Julien Villeneuve"
    };

        private static readonly List<(string Nom, decimal Prix)> m_articlesDisponibles = new List<(string, decimal)>
    {
        ("Tournevis", 12.50m),
        ("Marteau", 18.99m),
        ("Clé à molette", 15.00m),
        ("Ruban à mesurer", 9.75m),
        ("Niveau à bulle", 22.40m)
    };

        /// <summary>
        /// Fonction généré par IA afin de me faire un client aléatoire
        /// </summary>
        /// <returns></returns>
        public static string GenererClient()
        {
            int index = m_random.Next(m_clients.Count);
            return m_clients[index];
        }

        /// <summary>
        /// Fonction généré par IA afin de me faire aléatoirement une liste d'article avec une List de tuple.
        /// Un tuple est une combinaison de deux types qui fait une combinaison des deux. 
        /// La variable peut ensuite être utilisé comme un objet. Voir program producteur ligne 44 et 45.
        /// </summary>
        /// <param name="maxArticles"></param>
        /// <returns></returns>
        public static List<(string Nom, decimal Prix)> GenererArticlesAleatoires(int maxArticles = 5)
        {
            var selection = new List<(string Nom, decimal Prix)>();

            int nbArticles = m_random.Next(1, maxArticles + 1);

            for (int i = 0; i < nbArticles; i++)
            {
                int index = m_random.Next(m_articlesDisponibles.Count);
                selection.Add(m_articlesDisponibles[index]);
            }
            return selection;
        }
    }
}
