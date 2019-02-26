using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngenhariaFiles
{
    public interface IGridVendor
    {
        string Nome { get; set; }
        string Extensao { get; set; }
        string Revisao { get; set; }
        string Tamanho { get; set; }
        string Data { get; set; }
    }
}
