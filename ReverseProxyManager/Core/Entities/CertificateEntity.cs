using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CertificateEntity
    {
        // The filename is a mix between the name and the id
        public int Id { get; set; }
        public string Name { get; set; }

        public string Issuer { get; set; }

        public string Subject { get; set; }

        public DateTime ValidNotBefore { get; set; }

        public DateTime ValidNotAfter { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;

    }
}
