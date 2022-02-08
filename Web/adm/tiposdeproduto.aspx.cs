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

public partial class tiposdeproduto : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsTiposDeProduto.TrazGrid();
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


        this.lblMsg.Text = "Gerenciamento de tipos de produto da Área Administrativa.";
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());
        ClsTiposDeProduto.CarregaLista(this.ddlgeneros, "Genero", (Request["ddlgeneros"] == null ? "0" : (string)Request["ddlgeneros"]), "Escolha um Gênero");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());
        ClsTiposDeProduto.CodigoDoTipoDeProduto = Convert.ToInt16(this.txtcd_tpprod.Text.ToString());
        ClsTiposDeProduto.NomeDoTipoDeProduto = this.txtnm_tpprod.Valor.ToString().Trim();
        ClsTiposDeProduto.CodigoDoGenero = Convert.ToInt16(this.ddlgeneros.SelectedValue);
        
        resp = ClsTiposDeProduto.Atualizar();
        //**************************

        if (ClsTiposDeProduto.critica != "")
        {
            Mensagem(ClsTiposDeProduto.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeProduto.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de tipos de produto da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_tpprod.Text.Trim() != "")
        {
            if (this.txtcd_tpprod.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());

        ClsTiposDeProduto.NomeDoTipoDeProduto = this.txtnm_tpprod.Valor.ToString().Trim();
        ClsTiposDeProduto.CodigoDoGenero = Convert.ToInt16(this.ddlgeneros.SelectedValue);
        
        resp = ClsTiposDeProduto.Grava();
        //*********************
        txtcd_tpprod.Text = ClsTiposDeProduto.CodigoDoTipoDeProduto.ToString();

        if (ClsTiposDeProduto.critica != "")
        {
            Mensagem(ClsTiposDeProduto.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeProduto.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsTiposDeProduto.CodigoDoTipoDeProduto = Convert.ToInt16(this.txtcd_tpprod.Text.ToString());

        resp = ClsTiposDeProduto.Consulta();
        //************************
        txtcd_tpprod.Text = ClsTiposDeProduto.CodigoDoTipoDeProduto.ToString();
        txtnm_tpprod.Valor = ClsTiposDeProduto.NomeDoTipoDeProduto.Trim();
        ddlgeneros.SelectedValue = ClsTiposDeProduto.CodigoDoGenero.ToString();
       
        if (ClsTiposDeProduto.critica != "")
        {
            Mensagem(ClsTiposDeProduto.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeProduto.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());
        this.txtnm_tpprod.Valor = "";
        ddlgeneros.SelectedValue = ClsTiposDeProduto.CodigoDoGenero.ToString();
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        TiposDeProduto ClsTiposDeProduto = new TiposDeProduto(Application["StrConexao"].ToString());

        ClsTiposDeProduto.CodigoDoTipoDeProduto = Convert.ToInt16(this.txtcd_tpprod.Text.ToString());

        resp = ClsTiposDeProduto.Excluir();
        //**********************
        txtcd_tpprod.Text = ClsTiposDeProduto.CodigoDoTipoDeProduto.ToString();
        txtnm_tpprod.Valor = ClsTiposDeProduto.NomeDoTipoDeProduto.Trim();
        ddlgeneros.SelectedValue = ClsTiposDeProduto.CodigoDoGenero.ToString();
       
        if (ClsTiposDeProduto.critica != "")
        {
            Mensagem(ClsTiposDeProduto.critica.ToString());
        }
        lblGrid.Text = ClsTiposDeProduto.TrazGrid();

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
