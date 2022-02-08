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

public partial class generos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        
        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.genero.Visible = false;
        }
        else
        {
            this.btn_atualizar.Visible = false;
            this.genero.Visible = true;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Visible = false;

            Genero ClsGenero = new Genero(Application["StrConexao"].ToString());

            lblGrid.Text = ClsGenero.TrazGrid();
            this.lblMsg.Text = "Gerenciamento de gêneros da Área Administrativa.";
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
        Genero ClsGenero = new Genero(Application["StrConexao"].ToString());
        ClsGenero.CodigoDoGenero = Convert.ToInt16(this.txtcd_genero.Text.ToString());
        ClsGenero.NomeDoGenero = this.txtnm_genero.Valor.ToString().Trim();
        
        resp = ClsGenero.Atualizar();
        //**************************

        if (ClsGenero.critica != "")
        {
            Mensagem(ClsGenero.critica.ToString());
        }
        lblGrid.Text = ClsGenero.TrazGrid();

        if (resp)
        {
            this.btn_salvar.Enabled = !resp;
        }
        else
        {
            this.btn_salvar.Enabled = resp;
        }
    }

   
    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Gerenciamento de gêneros da Área Administrativa.";
        this.btn_salvar.Enabled = true;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_genero.Text.Trim() != "")
        {
            if (this.txtcd_genero.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Genero ClsGenero = new Genero(Application["StrConexao"].ToString());

        ClsGenero.NomeDoGenero = this.txtnm_genero.Valor.ToString().Trim();
        
        resp = ClsGenero.Grava();
        //*********************
        txtcd_genero.Text = ClsGenero.CodigoDoGenero.ToString();

        if (ClsGenero.critica != "")
        {
            Mensagem(ClsGenero.critica.ToString());
        }
        lblGrid.Text = ClsGenero.TrazGrid();

        this.btn_salvar.Enabled = !resp;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Genero ClsGenero = new Genero(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsGenero.CodigoDoGenero = Convert.ToInt16(this.txtcd_genero.Text.ToString());

        resp = ClsGenero.Consulta();
        //************************
        txtcd_genero.Text = ClsGenero.CodigoDoGenero.ToString();
        txtnm_genero.Valor = ClsGenero.NomeDoGenero.Trim();
       
        if (ClsGenero.critica != "")
        {
            Mensagem(ClsGenero.critica.ToString());
        }
        lblGrid.Text = ClsGenero.TrazGrid();

        this.btn_salvar.Enabled = !resp;
   
    }

    public void LimpaCampo()
    {
        this.txtnm_genero.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Genero ClsGenero = new Genero(Application["StrConexao"].ToString());

        ClsGenero.CodigoDoGenero = Convert.ToInt16(this.txtcd_genero.Text.ToString());

        resp = ClsGenero.Excluir();
        //**********************
        txtcd_genero.Text = ClsGenero.CodigoDoGenero.ToString();
        txtnm_genero.Valor = ClsGenero.NomeDoGenero.Trim();
       
        if (ClsGenero.critica != "")
        {
            Mensagem(ClsGenero.critica.ToString());
        }
        lblGrid.Text = ClsGenero.TrazGrid();

        this.btn_salvar.Enabled = resp;
        this.LimpaCampo();
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
