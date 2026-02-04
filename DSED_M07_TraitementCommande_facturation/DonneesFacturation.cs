using DSED_M07_Commandes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_facturation
{
    public class DonneesFacturation
    {
        public Commande CommandeOriginale { get; set; }
        public decimal SousTotal { get; set; }
        public decimal TotalFinal { get; set; }
    }
}
