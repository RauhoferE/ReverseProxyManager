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

        public string? Issuer { get; set; }

        public string? Subject { get; set; }

        public DateTime? ValidNotBefore { get; set; }

        public DateTime? ValidNotAfter { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // This is used to determine if the nginx config is containing the new information
        public bool IsUpToDate { get; set; } = true;

        public bool FileAttached { get; set; } = true;

        // FK
        public int? ServerId { get; set; } = null;

        public ServerEntity? ServerEntity { get; set; } = null;
    }
}
