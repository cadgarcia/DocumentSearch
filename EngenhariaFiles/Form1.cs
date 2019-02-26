using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EngenhariaFiles
{
    public partial class Form1 : Form
    {
        private string _configFile;

        private IEnumerable<Arquivo> _arquivos;
        public Form1()
        {
            InitializeComponent();
            //lblTitulo.BackColor = ColorTranslator.FromHtml("#7B9732");
            //Color.FromArgb(168, 207, 69);
        }

        private void btnLerArquivos_Click(object sender, EventArgs e)
        {
            var arquivos = LerArquivos(TipoDeArquivo.Vendor, txtVendorOrigem.Text);
            _arquivos = arquivos;

            CarregarCliente(TipoDeArquivo.Vendor);
        }

        private IEnumerable<Arquivo> LerArquivos(TipoDeArquivo tipo, string caminho)
        {
            var arquivos = new List<Arquivo>();
            var pastasTmp = Directory.GetDirectories(caminho);
            var pastas = (from x in pastasTmp
                          let pos = x.LastIndexOf(@"\")
                          let tam = x.Length
                          let name = x.Substring(pos + 1, tam - pos - 1)
                          where name.Length == (tipo == TipoDeArquivo.Design ? 2 : 3)
                          where !(new[] { "DAT", "STP" }).Contains(name.ToUpper())
                          select x);

            foreach (var pasta in pastas.ToArray())
            {
                var arquivosTmp = Directory.GetFiles(pasta, "*.*", SearchOption.AllDirectories);

                var xx = arquivosTmp.Select(x => new Arquivo(tipo, x)).ToArray();

                arquivos.AddRange(xx);
            }

            return arquivos;
        }

        private void LimparGrid()
        {
            dgvArquivos.DataSource = null;
            dgvDesign.DataSource = null;
        }

        private void CarregarCliente(TipoDeArquivo tipo)
        {
            var clientes = _arquivos.Select(x => x.Cliente)
                                    .Where(x => !String.IsNullOrEmpty(x))
                                    .Distinct()
                                    .ToArray();

            (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).DataSource = clientes;
        }

        private void CarregarProjetos(TipoDeArquivo tipo)
        {
            var cliente = (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projetos = _arquivos.Where(x => x.Cliente == cliente)
                                    .Select(x => x.Projeto)
                                    .Distinct()
                                    .ToArray();
            (tipo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).DataSource = projetos;
        }

        private void CarregarVendors(TipoDeArquivo tipo)
        {
            var cliente = (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var vendors = _arquivos.Where(x => x.Cliente == cliente)
                                  .Where(x => x.Projeto == projeto)
                                  .Select(x => x.Vendor)
                                  .Distinct()
                                  .ToArray();

            (tipo == TipoDeArquivo.Vendor ? cbVendor : cbRegiaoDesign).DataSource = vendors;
        }

        private void CarregarTipos(TipoDeArquivo tipo)
        {
            var cliente = (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();

            var tiposTmp = _arquivos.Where(x => x.Cliente == cliente)
                                    .Where(x => x.Projeto == projeto);

            if (tipo == TipoDeArquivo.Vendor)
            {
                var vendor = cbVendor.SelectedValue.ToString();
                tiposTmp = tiposTmp.Where(x => x.Vendor == vendor);
            }
            else
            {
                var regiao = cbRegiaoDesign.SelectedValue.ToString();
                var disciplina = cbDesignDisciplina.SelectedValue.ToString();

                tiposTmp = tiposTmp.Where(x => x.Regiao == regiao);
                tiposTmp = tiposTmp.Where(x => x.Disciplina == disciplina);
            }

            var tipos = tiposTmp.Select(x => x.Tipo).Distinct().ToArray();


            (tipo == TipoDeArquivo.Vendor ? cbTipo : cbDesignTipo).DataSource = tipos;
        }

        private void CarregarNumerais(TipoDeArquivo tipoArquivo)
        {
            var cliente = (tipoArquivo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipoArquivo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var tipo = (tipoArquivo == TipoDeArquivo.Vendor ? cbTipo : cbDesignTipo).SelectedValue.ToString();

            var numeraisTmp = _arquivos.Where(x => x.Cliente == cliente)
                                       .Where(x => x.Projeto == projeto)
                                       .Where(x => x.Tipo == tipo);

            if (tipoArquivo == TipoDeArquivo.Vendor)
            {
                var vendor = cbVendor.SelectedValue.ToString();

                numeraisTmp = numeraisTmp.Where(x => x.Vendor == vendor);
            }
            else
            {
                var regiao = cbRegiaoDesign.SelectedValue.ToString();
                var disciplina = cbDesignDisciplina.SelectedValue.ToString();

                numeraisTmp = numeraisTmp.Where(x => x.Regiao == regiao)
                                         .Where(x => x.Disciplina == disciplina);
            }

            var numerais = numeraisTmp.Select(x => x.Numeral)
                                      .Distinct()
                                      .ToArray();

            (tipoArquivo == TipoDeArquivo.Vendor ? cbNumeral : cbDesignNumeral).DataSource = numerais;
        }

        private void CarregarRevisoes(TipoDeArquivo tipoArquivo)
        {
            var cliente = (tipoArquivo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipoArquivo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var tipo = (tipoArquivo == TipoDeArquivo.Vendor ? cbTipo : cbDesignTipo).SelectedValue.ToString();
            var numeral = (tipoArquivo == TipoDeArquivo.Vendor ? cbNumeral : cbDesignNumeral).SelectedValue.ToString();

            var revisoesTmp = _arquivos.Where(x => x.Cliente == cliente)
                                  .Where(x => x.Projeto == projeto)
                                  .Where(x => x.Tipo == tipo)
                                  .Where(x => x.Numeral == numeral);

            if (tipoArquivo == TipoDeArquivo.Vendor)
            {
                var vendor = cbVendor.SelectedValue.ToString();

                revisoesTmp = revisoesTmp.Where(x => x.Vendor == vendor);
            }
            else
            {
                var regiao = cbRegiaoDesign.SelectedValue.ToString();
                var disciplina = cbDesignDisciplina.SelectedValue.ToString();

                revisoesTmp = revisoesTmp.Where(x => x.Regiao == regiao)
                                         .Where(x => x.Disciplina == disciplina);
            }

            var revisoes = revisoesTmp.OrderBy(x=>x.RevisaoNumeral)
                                      .Select(x => x.Revisao)
                                      .Distinct()
                                      .ToArray();

            //var numeros = revisoes.Select(x => Convert.ToInt32(x))
            //                      .OrderBy(x => x)
            //                      .Select(x => x.ToString());

            (tipoArquivo == TipoDeArquivo.Vendor ? cbRevisao : cbDesignRevisao).DataSource = (new[] { "" }).Concat(revisoes).ToArray();
        }

        private void CarregarRegioes(TipoDeArquivo tipo)
        {
            var cliente = (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var regioes = _arquivos.Where(x => x.Cliente == cliente)
                                  .Where(x => x.Projeto == projeto)
                                  .Select(x => x.Regiao)
                                  .Distinct()
                                  .ToArray();

            cbRegiaoDesign.DataSource = regioes;
        }

        private void CarregarDisciplinas(TipoDeArquivo tipo)
        {
            var cliente = (tipo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue.ToString();
            var projeto = (tipo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var regiao = cbRegiaoDesign.SelectedValue.ToString();

            var regioes = _arquivos.Where(x => x.Cliente == cliente)
                                  .Where(x => x.Projeto == projeto)
                                  .Where(x => x.Regiao == regiao)
                                  .Select(x => x.Disciplina)
                                  .Distinct()
                                  .ToArray();

            cbDesignDisciplina.DataSource = regioes;
        }



        private IEnumerable<Arquivo> Consultar(TipoDeArquivo tipoArquivo)
        {
            var cliente = (tipoArquivo == TipoDeArquivo.Vendor ? cbCliente : cbDesignCliente).SelectedValue?.ToString();
            if (cliente == null)
            {
                return null;
            }

            var projeto = (tipoArquivo == TipoDeArquivo.Vendor ? cbObra : cbDesignObra).SelectedValue.ToString();
            var tipo = (tipoArquivo == TipoDeArquivo.Vendor ? cbTipo : cbDesignTipo).SelectedValue.ToString();
            var numeral = (tipoArquivo == TipoDeArquivo.Vendor ? cbNumeral : cbDesignNumeral).SelectedValue.ToString();
            var revisao = (tipoArquivo == TipoDeArquivo.Vendor ? cbRevisao : cbDesignRevisao).SelectedValue?.ToString();

            var arquivosTmp = _arquivos.Where(x => x.Cliente == cliente)
                                       .Where(x => x.Projeto == projeto)
                                       .Where(x => x.Tipo == tipo)
                                       .Where(x => x.Numeral == numeral);

            if (tipoArquivo == TipoDeArquivo.Vendor)
            {
                var vendor = cbVendor.SelectedValue.ToString();

                arquivosTmp = arquivosTmp.Where(x => x.Vendor == vendor);
            }
            else
            {
                var regiao = cbRegiaoDesign.SelectedValue.ToString();
                var disciplina = cbDesignDisciplina.SelectedValue.ToString();

                arquivosTmp = arquivosTmp.Where(x => x.Regiao == regiao)
                                         .Where(x => x.Disciplina == disciplina);
            }

            if (!String.IsNullOrEmpty(revisao))
            {
                arquivosTmp = arquivosTmp.Where(x => x.Revisao == revisao);
            }

            if ((tipoArquivo == TipoDeArquivo.Vendor ? cbExibirMarkup : cbDesignExibirMarkup).Checked)
            {
                arquivosTmp = arquivosTmp.Where(x => x.Outros.ToUpper().Contains("MARKUP"));
            }

            return from x in arquivosTmp
                   let info = new FileInfo(x.CaminhoCompleto)
                   let size = Extensions.BytesToString(info.Length)
                   let date = info.CreationTime.ToString("dd/MM/yyyy")
                   select new Arquivo(tipoArquivo, x.CaminhoCompleto)
                   {
                       Tamanho = size,
                       Data = date
                   };
        }

        private void Copiar(string destino, IEnumerable<Arquivo> arquivos)
        {
            if (!Directory.Exists(destino))
            {
                MessageBox.Show("Diretório de Destino inexistente. Verifique!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var arquivo in arquivos)
            {
                var nomeDestino = $"{destino}\\{arquivo.Nome}.{arquivo.Extensao}";

                File.Copy(arquivo.CaminhoCompleto, nomeDestino, true);
            }

            MessageBox.Show("Arquivos copiados com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProjetos(TipoDeArquivo.Vendor);
            LimparGrid();
        }

        private void cbObra_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarVendors(TipoDeArquivo.Vendor);
            LimparGrid();
        }

        private void cbVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarTipos(TipoDeArquivo.Vendor);
            LimparGrid();
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarNumerais(TipoDeArquivo.Vendor);
            LimparGrid();
        }

        private void cbNumeral_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarRevisoes(TipoDeArquivo.Vendor);
            LimparGrid();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            var arquivos = Consultar(TipoDeArquivo.Vendor);

            if (arquivos != null)
            {
                var grid = (from x in arquivos
                            select (IGridVendor)x).ToArray();

                dgvArquivos.DataSource = grid;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _configFile = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Diretorios.xml");

            var config = Extensions.XMLLoad(_configFile);
            txtVendorOrigem.Text = config.VendorOrigem.Base64Decode();
            txtVendorDestino.Text = config.VendorDestino.Base64Decode();
            txtDesignOrigem.Text = config.DesignOrigem.Base64Decode();
            txtDesignDestino.Text = config.DesignDestino.Base64Decode();
        }

        private void SalvarDiretorios()
        {
            var config = new Configuracao
            {
                VendorOrigem = txtVendorOrigem.Text.Base64Encode(),
                VendorDestino = txtVendorDestino.Text.Base64Encode(),
                DesignOrigem = txtDesignOrigem.Text.Base64Encode(),
                DesignDestino = txtDesignDestino.Text.Base64Encode(),
            };

            Extensions.XMLSave(_configFile, config);
        }


        private void btnCopiar_Click(object sender, EventArgs e)
        {
            var rows = dgvArquivos.SelectedRows;

            var arquivos = new List<Arquivo>();
            foreach (DataGridViewRow row in rows)
            {
                arquivos.Add(row.DataBoundItem as Arquivo);
            }

            var destino = txtVendorDestino.Text;

            Copiar(destino, arquivos);
        }

        private void cbRevisao_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparGrid();
        }

        private void txtVendorDestino_TextChanged(object sender, EventArgs e)
        {
            SalvarDiretorios();
        }

        private void txtVendorOrigem_TextChanged(object sender, EventArgs e)
        {
            SalvarDiretorios();
        }

        private void txtDesignOrigem_TextChanged(object sender, EventArgs e)
        {
            SalvarDiretorios();
        }

        private void txtDesignDestino_TextChanged(object sender, EventArgs e)
        {
            SalvarDiretorios();
        }

        private void btnDesignLerArquivos_Click(object sender, EventArgs e)
        {
            var arquivos = LerArquivos(TipoDeArquivo.Design, txtVendorOrigem.Text);
            _arquivos = arquivos;

            CarregarCliente(TipoDeArquivo.Design);
        }

        private void btnDesignCopiar_Click(object sender, EventArgs e)
        {
            var rows = dgvDesign.SelectedRows;

            var arquivos = new List<Arquivo>();
            foreach (DataGridViewRow row in rows)
            {
                arquivos.Add(row.DataBoundItem as Arquivo);
            }

            var destino = txtDesignDestino.Text;

            Copiar(destino, arquivos);
        }

        private void cbDesignCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProjetos(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignObra_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarRegioes(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarDisciplinas(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarNumerais(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignNumeral_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarRevisoes(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void btnDesignConsultar_Click(object sender, EventArgs e)
        {
            var arquivos = Consultar(TipoDeArquivo.Design);

            if (arquivos != null)
            {
                var grid = (from x in arquivos
                            select (IGridDesign)x).ToArray();

                dgvDesign.DataSource = grid;
            }
        }

        private void cbDesignDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarTipos(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignRevisao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarRevisoes(TipoDeArquivo.Design);
            LimparGrid();
        }

        private void cbDesignTipo_SelectedValueChanged(object sender, EventArgs e)
        {
            CarregarNumerais(TipoDeArquivo.Design);
            LimparGrid();
        }
    }
}
