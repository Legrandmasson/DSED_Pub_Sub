using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace M07_GestionDesFichiers
{
    public class MessageHeader
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("entity")] 
        public string Entite { get; set; }
    }
}
