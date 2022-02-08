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
//using Dominio;

public partial class sitcheques : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_financ"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.sitcheque.Visible = false;
        }
        else
        {
            this.sitcheque.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            SitCheque ClsSitCheque = new SitCheque(Application["StrConexao"].ToString());
            lblGrid.Text = ClsSitCheque.TrazGrid();

            this.lblMsg.Text = "Cadastramento de Situações do Cheque para Controle Financeiro.";
        }

    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        SitCheque ClsSitCheque = new SitCheque(Application["StrConexao"].ToString());
        ClsSitCheque.CodigoDaSituacao = Convert.ToInt16(this.txtcd_sitcheque.Text.ToString());
        ClsSitCheque.NomeDaSituacao = this.txtnm_sitcheque.Valor.ToString().Trim();
        
        resp = ClsSitCheque.Atualizar();
        //**************************

        if (ClsSitCheque.critica != "")
        {
            Mensagem(ClsSitCheque.critica.ToString());
        }
        lblGrid.Text = ClsSitCheque.TrazGrid();

        if (resp)
        {
            this.btn_atualizar.Enabled = resp;
            this.btn_salvar.Enabled = !resp;
        }
        else
        {
            this.btn_atualizar.Enabled = !resp;
            this.btn_salvar.Enabled = resp;
        }
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

   
    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Cadastramento de Situações do Cheque para Controle Financeiro.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_sitcheque.Text.Trim() != "")
        {
            if (this.txtcd_sitcheque.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        SitCheque ClsSitCheque = new SitCheque(Application["StrConexao"].ToString());

        ClsSitCheque.NomeDaSituacao = this.txtnm_sitcheque.Valor.ToString().Trim();
        
        resp = ClsSitCheque.Grava();
        //*********************
        txtcd_sitcheque.Text = ClsSitCheque.CodigoDaSituacao.ToString();

        if (ClsSitCheque.critica != "")
        {
            Mensagem(ClsSitCheque.critica.ToString());
        }
        lblGrid.Text = ClsSitCheque.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        SitCheque ClsSitCheque = new SitCheque(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSitCheque.CodigoDaSituacao = Convert.ToInt16(this.txtcd_sitcheque.Text.ToString());

        resp = ClsSitCheque.Consulta();
        //************************
        txtcd_sitcheque.Text = ClsSitCheque.CodigoDaSituacao.ToString();
        txtnm_sitcheque.Valor = ClsSitCheque.NomeDaSituacao.Trim();
       
        if (ClsSitCheque.critica != "")
        {
            Mensagem(ClsSitCheque.critica.ToString());
        }
        lblGrid.Text = ClsSitCheque.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_sitcheque.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        SitCheque ClsSitCheque = new SitCheque(Application["StrConexao"].ToString());

        ClsSitCheque.CodigoDaSituacao = Convert.ToInt16(this.txtcd_sitcheque.Text.ToString());

        resp = ClsSitCheque.Excluir();
        //**********************
        txtcd_sitcheque.Text = ClsSitCheque.CodigoDaSituacao.ToString();
        txtnm_sitcheque.Valor = ClsSitCheque.NomeDaSituacao.Trim();
       
        if (ClsSitCheque.critica != "")
        {
            Mensagem(ClsSitCheque.critica.ToString());
        }
        lblGrid.Text = ClsSitCheque.TrazGrid();

        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.LimpaCampo();
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
