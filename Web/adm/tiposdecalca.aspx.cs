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

public partial class tiposdecalca : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        TiposDeCalca ClsTiposDeCalca = new TiposDeCalca(Application["StrConexao"].ToString());
        
        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsTiposDeCalca.TrazGrid();
        }

        if ((bool)Session["bl_exclui"] == true)
        {
            this.btn_excluir.Visible = true;
        }
        else
        {
            this.btn_excluir.Visible = false;
        }

        if ((bool)Session["bl_grava"] == true)
        {
            this.btn_novo.Visible = true;
            this.btn_atualizar.Visible = true;
            this.btn_salvar.Visible = true;
        }
        else
        {
            this.btn_novo.Visible = false;
            this.btn_atualizar.Visible = false;
            this.btn_salvar.Visible = false;
        }

        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        this.lblMsg.Text = "Gerenciamento de tipos de calça da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        TiposDeCalca ClsTiposDeCalca = new TiposDeCalca(Application["StrConexao"].ToString());
        ClsTiposDeCalca.CodigoDoTipoDeCalca = Convert.ToInt16(this.txtcd_tpcalca.Text.ToString());
        ClsTiposDeCalca.NomeDoTipoDeCalca = this.txtnm_tpcalca.Valor.ToString().Trim();
        
        resp = ClsTiposDeCalca.Atualizar();
        //**************************

        if (ClsTiposDeCalca.critica != "")
        {
            Mensagem(ClsTiposDeCalca.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeCalca.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de tipos de calça da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_tpcalca.Text.Trim() != "")
        {
            if (this.txtcd_tpcalca.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        TiposDeCalca ClsTiposDeCalca = new TiposDeCalca(Application["StrConexao"].ToString());

        ClsTiposDeCalca.NomeDoTipoDeCalca = this.txtnm_tpcalca.Valor.ToString().Trim();
        
        resp = ClsTiposDeCalca.Grava();
        //*********************
        txtcd_tpcalca.Text = ClsTiposDeCalca.CodigoDoTipoDeCalca.ToString();

        if (ClsTiposDeCalca.critica != "")
        {
            Mensagem(ClsTiposDeCalca.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeCalca.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        TiposDeCalca ClsTiposDeCalca = new TiposDeCalca(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsTiposDeCalca.CodigoDoTipoDeCalca = Convert.ToInt16(this.txtcd_tpcalca.Text.ToString());

        resp = ClsTiposDeCalca.Consulta();
        //************************
        txtcd_tpcalca.Text = ClsTiposDeCalca.CodigoDoTipoDeCalca.ToString();
        txtnm_tpcalca.Valor = ClsTiposDeCalca.NomeDoTipoDeCalca.Trim();
       
        if (ClsTiposDeCalca.critica != "")
        {
            Mensagem(ClsTiposDeCalca.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeCalca.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_tpcalca.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        TiposDeCalca ClsTiposDeCalca = new TiposDeCalca(Application["StrConexao"].ToString());

        ClsTiposDeCalca.CodigoDoTipoDeCalca = Convert.ToInt16(this.txtcd_tpcalca.Text.ToString());

        resp = ClsTiposDeCalca.Excluir();
        //**********************
        txtcd_tpcalca.Text = ClsTiposDeCalca.CodigoDoTipoDeCalca.ToString();
        txtnm_tpcalca.Valor = ClsTiposDeCalca.NomeDoTipoDeCalca.Trim();
       
        if (ClsTiposDeCalca.critica != "")
        {
            Mensagem(ClsTiposDeCalca.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeCalca.TrazGrid();

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
