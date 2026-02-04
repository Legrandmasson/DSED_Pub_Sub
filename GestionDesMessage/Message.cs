using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M07_GestionDesFichiers
{
    public class Message<T>
    {
        public Guid Id { get; set; }
        public string Action { get; set; } 
        public string Entite { get; set; } 
        public T Donnee { get; set; }

        public Message()
        {

        }
        public Message(string p_action)
        {
            this.Id = Guid.NewGuid();
            this.Action = p_action;
            this.Entite = "Commande";
        }
    }

}

