using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M07_GestionDesFichiers
{
    public class Signature
    {
        public string HashMethod { get; set; } = "L3P0UL3T";
        public string BodySignature { get; set; }
        public string HeaderSignature { get; set; }
    }
}
