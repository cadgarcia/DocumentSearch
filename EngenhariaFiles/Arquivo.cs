using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EngenhariaFiles
{
    public enum TipoDeArquivo
    {
        Vendor,
        Design
    }

    public class Arquivo : IGridVendor, IGridDesign
    {
        public bool Selecionado { get; set; }
        public string Cliente { get; set; }
        public string Projeto { get; set; }
        public string Vendor { get; set; }
        public string Tipo { get; set; }
        public string Numeral { get; set; }
        public string Regiao { get; set; }
        public string Disciplina { get; set; }

        [DisplayName("Revisão")]
        public string Revisao { get; set; }

        public int RevisaoNumeral { get; set; }

        public string Outros { get; set; }

        public string Diretorio { get; set; }
        public string CaminhoCompleto { get; set; }


        [DisplayName("Extensão")]
        public string Extensao { get; set; }
        public string Nome { get; set; }

        public string Tamanho { get; set; }
        public string Data { get; set; }

        public Arquivo(TipoDeArquivo tipo, string nome)
        {
            var posicao = nome.LastIndexOf(@"\");
            CaminhoCompleto = nome;
            Nome = nome.Substring(posicao + 1, nome.Length - posicao - 1);

            Diretorio = nome.Substring(0, posicao);
            Extensao = Nome.Split('.').Last();

            //replace de extensão nbo nome
            Nome = Nome.Replace($".{Extensao}", "");

            var partes = Nome.Split('-');
            if (partes.Length <= 1)
            {
                return;
            }

            Cliente = partes[0];
            Projeto = partes[1];
            if (tipo == TipoDeArquivo.Vendor)
            {
                Vendor = partes[2];
                Tipo = partes[3];
                Numeral = partes[4];
                DefinirRevisao(partes[5]);


                if (partes.Length > 5)
                {
                    Outros = String.Join(" || ", partes.Skip(6));
                }
            }
            else
            {
                Regiao = partes[2];
                Disciplina = partes[3];
                //Vendor = partes[2];
                Tipo = partes[4];
                Numeral = partes[5];

                DefinirRevisao(partes[6]);

                if (partes.Length > 6)
                {
                    Outros = String.Join(" || ", partes.Skip(7));
                }
            }

        }

        private void DefinirRevisao ( string revisao )
        {
            Revisao = revisao.Replace("R", "");
            if (Revisao.Contains(Extensao))
            {
                Revisao = Revisao.Replace($".{Extensao}", "");
            }

            int numero;
            if ( Int32.TryParse(Revisao, out numero) )
            {
                RevisaoNumeral = numero;
            }
            else
            {
                var numeroRevisao = Regex.Match(Revisao, @"\d+")?.Value;
                RevisaoNumeral = String.IsNullOrEmpty(numeroRevisao) ? 0 : Convert.ToInt32(numeroRevisao);
            }
        }
    }
}
