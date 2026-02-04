using DSED_M07_Commandes;

namespace M07_GestionDesFichiers
{
    public class Enveloppe<T>
    {
        /// <summary>
        /// Propriété avec paramètre autogénéré et privé pour éviter tout changement garder intégrité de l'objet actuel.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Action { get; private set; } = "placee";
        public string Entite { get; private set; } = "commande";
        public T Donnee { get; set; }
    }
}
