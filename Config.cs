using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridNet
{
    public class Config
    {
        public string ApiKey { get; set; }
        public string ApiKeyPass { get; set; }
        public string Smtp { get; set; }
        public string MailRemitente { get; set; }
    }
}
