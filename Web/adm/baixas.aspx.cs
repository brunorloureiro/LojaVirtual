using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class baixas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["bl_baixa"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.baixa.Visible = false;
        }
        else
        {
            this.baixa.Visible = true;
            this.btn_salvar.Enabled = !false;

            Baixa ClsBaixa = new Baixa(Application["StrConexao"].ToString());
            lblGrid.Text = ClsBaixa.TrazGrid();

            this.lblMsg.Text = "Baixa de Peças do Estoque.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_salvar.Enabled = !false;
  
        Baixa ClsBaixa = new Baixa(Application["StrConexao"].ToString());
        ClsBaixa.CarregaLista(this.ddlclientes, "Cliente", (Request["ddlclientes"] == null ? "0" : (string)Request["ddlclientes"]), "Escolha um Cliente");
        ClsBaixa.CarregaLista(this.ddltppagtos, "Tppagto", (Request["ddltppagtos"] == null ? "0" : (string)Request["ddltppagtos"]), "Escolha um Tipo de Pagamento");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Baixa de Peças do Estoque.";
        this.btn_salvar.Enabled = true;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_baixa.Text.Trim() != "")
        {
            if (this.txtcd_baixa.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Baixa ClsBaixa = new Baixa(Application["StrConexao"].ToString());

        ClsBaixa.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsBaixa.EstoqueNegativo = Convert.ToBoolean(Session["bl_estneg"]);
        ClsBaixa.CodigoDoProduto = ClsBaixa.RetornaCodigo(this.txtcd_produto.Text);
        ClsBaixa.Quantidade = Convert.ToInt32(this.txtquantidade.Valor.ToString());
        ClsBaixa.Venda = Convert.ToInt16(this.chkvenda.Checked);
        ClsBaixa.CodigoDaVenda = Convert.ToInt32(this.txtcd_venda.Valor.ToString());
        ClsBaixa.CodigoDoCliente = Convert.ToInt16(this.ddlclientes.SelectedValue);
        ClsBaixa.CodigoDoTipoDePagamento = Convert.ToInt16(this.ddltppagtos.SelectedValue);
        ClsBaixa.NumeroDeParcelas = Convert.ToInt16(this.txtqt_vezes.Valor.ToString());
        ClsBaixa.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsBaixa.Desconto = Convert.ToDecimal(this.txtdesconto.Valor.Replace(".", ","));
        ClsBaixa.Acrescimo = Convert.ToDecimal(this.txtacrescimo.Valor.Replace(".", ","));
        
        resp = ClsBaixa.Grava();
        //*********************
        txtcd_baixa.Text = ClsBaixa.CodigoDaBaixa.ToString();

        if (ClsBaixa.critica != "")
        {
            Mensagem(ClsBaixa.critica.ToString());
        }
        if (ClsBaixa.LimparCampos)
        {
            LimpaCampo();
        }
        lblGrid.Text = ClsBaixa.TrazGrid();

        

        this.btn_salvar.Enabled = !resp;
    }
  
    public void LimpaCampo()
    {
        Baixa ClsBaixa = new Baixa(Application["StrConexao"].ToString());
        this.txtcd_produto.Text = "0";
        this.txtquantidade.Valor = "0";
        this.chkvenda.Checked = false;
        ddlclientes.SelectedValue = ClsBaixa.CodigoDoCliente.ToString();
        ddltppagtos.SelectedValue = ClsBaixa.CodigoDoTipoDePagamento.ToString();
        this.txtcd_venda.Valor = "0";
        this.txtqt_vezes.Valor = "0";
        this.txtdt_vencto.Valor = "";
        this.txtdesconto.Valor = "0";
        this.txtacrescimo.Valor = "0";

     }

    public void validaproduto(object sender, EventArgs e)
    {
        if (this.txtcd_produto.Text.Trim() == "0" || this.txtcd_produto.Text.Trim() == "")
        {
            Mensagem("Código do Produto deve ser informado. Verifique.");
            return;
        }

        bool resp;
        Baixa ClsBaixa = new Baixa(Application["StrConexao"].ToString());

        resp = ClsBaixa.VerificaProduto(this.txtcd_produto.Text);
        ////************************

        if (ClsBaixa.critica != "")
        {
            Mensagem(ClsBaixa.critica.ToString());
            this.LimpaCampo();
        }

    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
