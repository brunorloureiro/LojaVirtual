using System;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;

namespace CustomControls
{
    /// <summary>
    /// Tipos Predefinidos
    /// </summary>
    public enum TipoCampo : short
    {
        Livre, Inteiro, Decimal, Email, Telefone, CPF, CNPJ, Data, DataHora, CEP
    }

    /// <summary>
    /// Campos Predefinidos
    /// </summary>
    public enum CampoPredefinidos : short
    {
        Livre, CodigoEmpresa, CodigoFilial, CodigoCcusto, CodigoRecurso
    }

    /// <summary>
    /// Classe que apresenta controle TextBox com controle de validação
    /// </summary>
    public class CustomTextBox : WebControl, INamingContainer
    {
        private TipoCampo _tipoCampo = TipoCampo.Livre;
        private CampoPredefinidos _campoPredefinidos = CampoPredefinidos.Livre;
        private String _mascara = "";
        private string _onBlur;
        
        /// <summary>
        /// Tipos predefinidos
        /// </summary>
        public TipoCampo TipoDeCampo
        {
            get { return _tipoCampo; }
            set { _tipoCampo = value; }
        }

        /// <summary>
        /// Campos predefinidos
        /// </summary>
        public CampoPredefinidos CampoPredefinido
        {
            get { return _campoPredefinidos; }
            set 
            {
                if (value == CampoPredefinidos.CodigoEmpresa)
                {
                    MascaraDoCampo = "0000000"; // 7 caracteres
                    TipoDeCampo = TipoCampo.Inteiro;
                }
                else
                {
                    if ((value == CampoPredefinidos.CodigoFilial) || (value == CampoPredefinidos.CodigoRecurso))
                    {
                        MascaraDoCampo = "00000000"; // 8 caracteres
                        TipoDeCampo = TipoCampo.Inteiro;
                    }
                    else
                    {
                        if (value == CampoPredefinidos.CodigoCcusto)
                        {
                            MascaraDoCampo = "000000000"; // 9 caracteres
                            TipoDeCampo = TipoCampo.Inteiro;
                        }
                    }
                }
                _campoPredefinidos = value;
            }
                
        }

        /// <summary>
        /// Mascara do Campo
        /// </summary>
        public string MascaraDoCampo
        {
            get { return _mascara; }
            set
            {
                _mascara = value;
                this.Tamanho = _mascara.Length;
            }
        }

        /// <summary>
        /// Controle TextBox
        /// </summary>
        TextBox _textBox = new TextBox();

        /// <summary>
        /// Propriedade Text do controle
        /// </summary>
        public String Valor
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

     
        /// <summary>
        /// Somente leitura
        /// </summary>
        public bool SomenteLeitura
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        /// <summary>
        /// Tratar como TextArea
        /// </summary>
        public bool QuebraLinha
        {
            get { return _textBox.Wrap; }
            set { _textBox.Wrap = value; }
        }

        /// <summary>
        /// Propriedade ID do controle
        /// </summary>
        public override String ID
        {
            get { return _textBox.ID; }
            set { _textBox.ID = value; }
        }

        /// <summary>
        /// Propriedade ID do controle
        /// </summary>
        public string OnBlur
        {
            get { return _onBlur; }
            set
            {
                _onBlur = value;
                _textBox.Attributes.Add("onblur", _onBlur.Trim());
            }
        }

        /// <summary>
        /// Propriedade Altera Conteúdo.
        /// </summary>
        public void OnTextChanged(object sender, EventArgs e)
        {
            _textBox.TextChanged += new EventHandler(OnTextChanged);
        }

        /// <summary>
        /// Trata Estilos ao campo
        /// </summary>
        public void AdicionaEstilo(string chave, string valor)
        {
            _textBox.Style.Add(chave, valor);
        }

        /// <summary>
        /// Move campo para o lado Direito
        /// </summary>
        public void MoverCampoParaDireita(string p_moverPara)
        {
            _textBox.Style.Add("margin-left", p_moverPara.Trim());
        }

       
        /// <summary>
        /// Propriedade ID do controle
        /// </summary>
        public string ClasseReferenciaCss
        {
            get { return _textBox.CssClass; }
            set { _textBox.CssClass = value; }
        }

        /// <summary>
        /// Largura do Controle
        /// </summary>
        public Unit Largura
        {
            get { return _textBox.Width; }
            set { _textBox.Width = value; }
        }

        /// <summary>
        /// Altura do Controle
        /// </summary>
        public Unit Altura
        {
            get { return _textBox.Height; }
            set { _textBox.Height = value; }
        }

        /// <summary>
        /// Tamanho Máximo
        /// </summary>
        public int Tamanho
        {
            get { return _textBox.MaxLength; }
            set { _textBox.MaxLength = value; }
        }

        /// <summary>
        /// Cria controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            // manipula TextBox
            CreateTextBox();
            TrataDetalhes();
        }

        public void TrataDetalhes()
        {
            string script = "<script type='text/javascript'>verificaValor(" + '"' + this.ID.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ", '" + this.TipoDeCampo + "', '" + this.MascaraDoCampo + "');</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "verificaValor" + this.ID.Trim(), script);
        }

        /// <summary>
        /// Monta TextBox e adiciona controle
        /// </summary>
        private void CreateTextBox()
        {
            // mude a aparência do controle, etc. aqui
            // ex.: _textBox.CssClass = "meuCss";

            // adiciona controle TextBox
            Controls.Add(_textBox);
        }
    }
}
