using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngenhariaFiles
{
    [Serializable]
    public class Configuracao
    {
        public string VendorOrigem { get; set; }
        public string VendorDestino { get; set; }

        public string DesignOrigem { get; set; }
        public string DesignDestino { get; set; }
    }
}
