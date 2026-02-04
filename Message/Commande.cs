namespace DSED_M07_Commandes
{
    public class Commande
    {
        public string NomClient { get; set; }
        public Guid Reference { get; private set; } = Guid.NewGuid();
        public List<Article> Articles { get; set; } = new List<Article>();
        public bool estPremimum { get; set; }
    }

}
