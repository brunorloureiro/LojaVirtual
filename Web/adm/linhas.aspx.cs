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

public partial class linhas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Linha ClsLinha = new Linha(Application["StrConexao"].ToString());
        
        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsLinha.TrazGrid();
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

        this.lblMsg.Text = "Gerenciamento de Linhas Victória Secrets da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Linha ClsLinha = new Linha(Application["StrConexao"].ToString());
        ClsLinha.CodigoDaLinha = Convert.ToInt32(this.txtcd_linha.Text.ToString());
        ClsLinha.NomeDaLinha = this.txtnm_linha.Valor.ToString().Trim();
        
        resp = ClsLinha.Atualizar();
        //**************************

        if (ClsLinha.critica != "")
        {
            Mensagem(ClsLinha.critica.ToString());
        }
        lblGrid.Text = ClsLinha.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de Linhas Victória Secrets da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_linha.Text.Trim() != "")
        {
            if (this.txtcd_linha.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Linha ClsLinha = new Linha(Application["StrConexao"].ToString());

        ClsLinha.NomeDaLinha = this.txtnm_linha.Valor.ToString().Trim();
        
        resp = ClsLinha.Grava();
        //*********************
        txtcd_linha.Text = ClsLinha.CodigoDaLinha.ToString();

        if (ClsLinha.critica != "")
        {
            Mensagem(ClsLinha.critica.ToString());
        }
        lblGrid.Text = ClsLinha.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Linha ClsLinha = new Linha(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsLinha.CodigoDaLinha = Convert.ToInt32(this.txtcd_linha.Text.ToString());

        resp = ClsLinha.Consulta();
        //************************
        txtcd_linha.Text = ClsLinha.CodigoDaLinha.ToString();
        txtnm_linha.Valor = ClsLinha.NomeDaLinha.Trim();
       
        if (ClsLinha.critica != "")
        {
            Mensagem(ClsLinha.critica.ToString());
        }
        lblGrid.Text = ClsLinha.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_linha.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Linha ClsLinha = new Linha(Application["StrConexao"].ToString());

        ClsLinha.CodigoDaLinha = Convert.ToInt32(this.txtcd_linha.Text.ToString());

        resp = ClsLinha.Excluir();
        //**********************
        txtcd_linha.Text = ClsLinha.CodigoDaLinha.ToString();
        txtnm_linha.Valor = ClsLinha.NomeDaLinha.Trim();
       
        if (ClsLinha.critica != "")
        {
            Mensagem(ClsLinha.critica.ToString());
        }
        lblGrid.Text = ClsLinha.TrazGrid();

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
