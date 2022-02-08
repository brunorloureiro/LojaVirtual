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

public partial class marcas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {


        Marca ClsMarca = new Marca(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsMarca.TrazGrid();
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

        this.lblMsg.Text = "Gerenciamento de marcas da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Marca ClsMarca = new Marca(Application["StrConexao"].ToString());
        ClsMarca.CodigoDaMarca = Convert.ToInt16(this.txtcd_marca.Text.ToString());
        ClsMarca.NomeDaMarca = this.txtnm_marca.Valor.ToString().Trim();
        
        resp = ClsMarca.Atualizar();
        //**************************

        if (ClsMarca.critica != "")
        {
            Mensagem(ClsMarca.critica.ToString());
        }
        lblGrid.Text = ClsMarca.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de marcas da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_marca.Text.Trim() != "")
        {
            if (this.txtcd_marca.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Marca ClsMarca = new Marca(Application["StrConexao"].ToString());

        ClsMarca.NomeDaMarca = this.txtnm_marca.Valor.ToString().Trim();
        
        resp = ClsMarca.Grava();
        //*********************
        txtcd_marca.Text = ClsMarca.CodigoDaMarca.ToString();

        if (ClsMarca.critica != "")
        {
            Mensagem(ClsMarca.critica.ToString());
        }
        lblGrid.Text = ClsMarca.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Marca ClsMarca = new Marca(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsMarca.CodigoDaMarca = Convert.ToInt16(this.txtcd_marca.Text.ToString());

        resp = ClsMarca.Consulta();
        //************************
        txtcd_marca.Text = ClsMarca.CodigoDaMarca.ToString();
        txtnm_marca.Valor = ClsMarca.NomeDaMarca.Trim();
       
        if (ClsMarca.critica != "")
        {
            Mensagem(ClsMarca.critica.ToString());
        }
        lblGrid.Text = ClsMarca.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_marca.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Marca ClsMarca = new Marca(Application["StrConexao"].ToString());

        ClsMarca.CodigoDaMarca = Convert.ToInt16(this.txtcd_marca.Text.ToString());

        resp = ClsMarca.Excluir();
        //**********************
        txtcd_marca.Text = ClsMarca.CodigoDaMarca.ToString();
        txtnm_marca.Valor = ClsMarca.NomeDaMarca.Trim();
       
        if (ClsMarca.critica != "")
        {
            Mensagem(ClsMarca.critica.ToString());
        }
        lblGrid.Text = ClsMarca.TrazGrid();

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
