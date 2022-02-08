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

public partial class fragrancias : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Fragrancia ClsFragrancia = new Fragrancia(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsFragrancia.TrazGrid();
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

        this.lblMsg.Text = "Gerenciamento de Fragrâncias da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Fragrancia ClsFragrancia = new Fragrancia(Application["StrConexao"].ToString());
        ClsFragrancia.CodigoDaFragrancia = Convert.ToInt16(this.txtcd_fragrancia.Text.ToString());
        ClsFragrancia.NomeDaFragrancia = this.txtnm_fragrancia.Valor.ToString().Trim();
        
        resp = ClsFragrancia.Atualizar();
        //**************************

        if (ClsFragrancia.critica != "")
        {
            Mensagem(ClsFragrancia.critica.ToString());
        }
        lblGrid.Text = ClsFragrancia.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de Fragrâncias da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_fragrancia.Text.Trim() != "")
        {
            if (this.txtcd_fragrancia.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Fragrancia ClsFragrancia = new Fragrancia(Application["StrConexao"].ToString());

        ClsFragrancia.NomeDaFragrancia = this.txtnm_fragrancia.Valor.ToString().Trim();
        
        resp = ClsFragrancia.Grava();
        //*********************
        txtcd_fragrancia.Text = ClsFragrancia.CodigoDaFragrancia.ToString();

        if (ClsFragrancia.critica != "")
        {
            Mensagem(ClsFragrancia.critica.ToString());
        }
        lblGrid.Text = ClsFragrancia.TrazGrid();
        if (resp)
        {
            this.btn_atualizar.Enabled = !resp;
            this.btn_salvar.Enabled = resp;
        }
        else
        {
            this.btn_atualizar.Enabled = resp;
            this.btn_salvar.Enabled = !resp;
        }
      
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Fragrancia ClsFragrancia = new Fragrancia(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsFragrancia.CodigoDaFragrancia = Convert.ToInt16(this.txtcd_fragrancia.Text.ToString());

        resp = ClsFragrancia.Consulta();
        //************************
        txtcd_fragrancia.Text = ClsFragrancia.CodigoDaFragrancia.ToString();
        txtnm_fragrancia.Valor = ClsFragrancia.NomeDaFragrancia.Trim();
       
        if (ClsFragrancia.critica != "")
        {
            Mensagem(ClsFragrancia.critica.ToString());
        }
        lblGrid.Text = ClsFragrancia.TrazGrid();
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

    public void LimpaCampo()
    {
        this.txtnm_fragrancia.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Fragrancia ClsFragrancia = new Fragrancia(Application["StrConexao"].ToString());

        ClsFragrancia.CodigoDaFragrancia = Convert.ToInt16(this.txtcd_fragrancia.Text.ToString());

        resp = ClsFragrancia.Excluir();
        //**********************
        txtcd_fragrancia.Text = ClsFragrancia.CodigoDaFragrancia.ToString();
        txtnm_fragrancia.Valor = ClsFragrancia.NomeDaFragrancia.Trim();
       
        if (ClsFragrancia.critica != "")
        {
            Mensagem(ClsFragrancia.critica.ToString());
        }
        lblGrid.Text = ClsFragrancia.TrazGrid();

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
