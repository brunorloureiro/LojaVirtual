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

public partial class saltos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Salto ClsSalto = new Salto(Application["StrConexao"].ToString());
        
        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsSalto.TrazGrid();
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

        this.lblMsg.Text = "Gerenciamento de Materiais da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Salto ClsSalto = new Salto(Application["StrConexao"].ToString());
        ClsSalto.CodigoDoSalto = Convert.ToInt32(this.txtcd_salto.Text.ToString());
        ClsSalto.NomeDoSalto = this.txtnm_salto.Valor.ToString().Trim();
        
        resp = ClsSalto.Atualizar();
        //**************************

        if (ClsSalto.critica != "")
        {
            Mensagem(ClsSalto.critica.ToString());
        }
        lblGrid.Text = ClsSalto.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de Materiais da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_salto.Text.Trim() != "")
        {
            if (this.txtcd_salto.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Salto ClsSalto = new Salto(Application["StrConexao"].ToString());

        ClsSalto.NomeDoSalto = this.txtnm_salto.Valor.ToString().Trim();
        
        resp = ClsSalto.Grava();
        //*********************
        txtcd_salto.Text = ClsSalto.CodigoDoSalto.ToString();

        if (ClsSalto.critica != "")
        {
            Mensagem(ClsSalto.critica.ToString());
        }
        lblGrid.Text = ClsSalto.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Salto ClsSalto = new Salto(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSalto.CodigoDoSalto = Convert.ToInt32(this.txtcd_salto.Text.ToString());

        resp = ClsSalto.Consulta();
        //************************
        txtcd_salto.Text = ClsSalto.CodigoDoSalto.ToString();
        txtnm_salto.Valor = ClsSalto.NomeDoSalto.Trim();
       
        if (ClsSalto.critica != "")
        {
            Mensagem(ClsSalto.critica.ToString());
        }
        lblGrid.Text = ClsSalto.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_salto.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Salto ClsSalto = new Salto(Application["StrConexao"].ToString());

        ClsSalto.CodigoDoSalto = Convert.ToInt32(this.txtcd_salto.Text.ToString());

        resp = ClsSalto.Excluir();
        //**********************
        txtcd_salto.Text = ClsSalto.CodigoDoSalto.ToString();
        txtnm_salto.Valor = ClsSalto.NomeDoSalto.Trim();
       
        if (ClsSalto.critica != "")
        {
            Mensagem(ClsSalto.critica.ToString());
        }
        lblGrid.Text = ClsSalto.TrazGrid();

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
