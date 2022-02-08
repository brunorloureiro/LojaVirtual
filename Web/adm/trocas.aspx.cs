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

public partial class trocas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["bl_contven"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.troca.Visible = false;
        }
        else
        {
            this.troca.Visible = true;
            this.btn_salvar.Enabled = !false;

            Troca ClsTroca = new Troca(Application["StrConexao"].ToString());
            lblGrid.Text = ClsTroca.TrazGrid();
           
            this.lblMsg.Text = "Troca de Produtos Pós-Venda.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_salvar.Enabled = !false;
  
        Troca ClsTroca = new Troca(Application["StrConexao"].ToString());
        ClsTroca.CarregaLista(this.ddlclientes, "Cliente", (Request["ddlclientes"] == null ? "0" : (string)Request["ddlclientes"]), "Escolha um Cliente");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Troca de Produtos Pós-Venda.";
        this.btn_salvar.Enabled = true;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_troca.Text.Trim() != "")
        {
            if (this.txtcd_troca.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Troca ClsTroca = new Troca(Application["StrConexao"].ToString());

        ClsTroca.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsTroca.EstoqueNegativo = Convert.ToBoolean(Session["bl_estneg"]);
        ClsTroca.Motivo = this.txtmotivo.Valor.ToString().Trim();
        ClsTroca.CodigoDoCliente = Convert.ToInt32(this.ddlclientes.SelectedValue);
        ClsTroca.CodigoDoProdutoDevolvido = ClsTroca.RetornaCodigo(this.txtcd_proddev.Text);
        ClsTroca.QuantidadeDevolvida = Convert.ToInt32(this.txtqt_dev.Valor.ToString());
        ClsTroca.CodigoDoProdutoLevado = ClsTroca.RetornaCodigo(this.txtcd_prodlev.Text);
        ClsTroca.QuantidadeLevada = Convert.ToInt32(this.txtqt_lev.Valor.ToString());
        ClsTroca.DiferencaPaga = Convert.ToDecimal(this.txtdifpaga.Valor.Replace(".", ","));
        
        
        resp = ClsTroca.Grava();
        //*********************
        txtcd_troca.Text = ClsTroca.CodigoDaTroca.ToString();

        if (ClsTroca.critica != "")
        {
            Mensagem(ClsTroca.critica.ToString());
        }
        lblGrid.Text = ClsTroca.TrazGrid();
        
        this.btn_salvar.Enabled = !resp;
    }

  
    public void LimpaCampo()
    {
        Troca ClsTroca = new Troca(Application["StrConexao"].ToString());
        this.txtmotivo.Valor = "";
        ddlclientes.SelectedValue = ClsTroca.CodigoDoCliente.ToString();
        this.txtcd_proddev.Text = "0";
        this.txtcd_prodlev.Text = "0";
        this.txtqt_dev.Valor = "0";
        this.txtqt_lev.Valor = "0";
        this.txtdifpaga.Valor = "0";
    }

    public void validaprodutodevolvido(object sender, EventArgs e)
    {
        if (this.txtcd_proddev.Text.Trim() == "0" || this.txtcd_proddev.Text.Trim() == "")
        {
            Mensagem("Código do Produto Devolvido deve ser informado. Verifique.");
            return;
        }
       

        bool resp;
        Troca ClsTroca = new Troca(Application["StrConexao"].ToString());

        resp = ClsTroca.VerificaProduto(this.txtcd_proddev.Text);
        ////************************

        if (ClsTroca.critica != "")
        {
            Mensagem(ClsTroca.critica.ToString());
            this.txtcd_proddev.Text = "0"; 
        }

    }

    public void validaprodutolevado(object sender, EventArgs e)
    {
        if (this.txtcd_prodlev.Text.Trim() == "0" || this.txtcd_prodlev.Text.Trim() == "")
        {
            Mensagem("Código do Produto Levado deve ser informado. Verifique.");
            return;
        }

        bool resp;
        Troca ClsTroca = new Troca(Application["StrConexao"].ToString());

        resp = ClsTroca.VerificaProduto(this.txtcd_prodlev.Text);
        ////************************

        if (ClsTroca.critica != "")
        {
            Mensagem(ClsTroca.critica.ToString());
            this.txtcd_prodlev.Text = "0";
        }

    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
