namespace DSED_M07_Commandes
{
    public class Article
    {
        public Guid Reference { get; private set; } = Guid.NewGuid();
        public string NomArticle { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
    }
}