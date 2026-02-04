namespace M07_GestionDesFichiers
{
    public class GestionDesFichiers
    {
        /// <summary>
        /// Normalement j'aurais fait un interface IGestionFichier
        /// De plus, afin de m'assurer de toujours avoir la bonne signature, les propriétés sont créer avec les bons paramètres.
        /// </summary>
        public string Extension { get; set; } = ".json";
        public string? NomJournal { get; set; } = "_Nouveau_Guid";
        public string? NomFacturation { get; set; } = "_ReferenceCommande_Facture";

        /// <summary>
        /// Gestion de la creation du nom du fichier assister avec IA afin d'éviter que la date reste toujours la même. 
        /// Retourne toujours une nouvelle DateTime puisque la créer au moment `d'utiliser la fonction et non lors de la création de l'objet.
        /// </summary>
        /// <returns></returns>
        public string GenererNomFichierJournal()
        {
            return $"{DateTime.Now:yyyyMMdd_HHmmss_fff}{NomJournal}{Extension}";
        }
        /// <summary>
        /// Gestion de la creation du nom du fichier assister avec IA afin d'éviter que la date reste toujours la même. 
        /// Retourne toujours une nouvelle DateTime puisque la créer au moment `d'utiliser la fonction et non lors de la création de l'objet.
        /// </summary>
        /// <returns></returns>
        public string GenererNomFichierFacturation()
        {
            return $"{DateTime.Now:yyyyMMdd_HHmmss_fff}{NomFacturation}{Extension}";
        }
    }
}
