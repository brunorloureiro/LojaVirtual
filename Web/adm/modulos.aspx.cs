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

public partial class modulos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.modulo.Visible = false;
        }
        else
        {
            this.btn_atualizar.Visible = false;
            this.modulo.Visible = true;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Visible = false;

            Modulo ClsModulo = new Modulo(Application["StrConexao"].ToString());

            lblGrid.Text = ClsModulo.TrazGrid();
            this.lblMsg.Text = "Gerenciamento de módulos.";
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
        Modulo ClsModulo = new Modulo(Application["StrConexao"].ToString());
        ClsModulo.CodigoDoModulo = Convert.ToInt16(this.txtcd_modulo.Text.ToString());
        ClsModulo.NomeDoModulo = this.txtnm_modulo.Valor.ToString().Trim();
        
        resp = ClsModulo.Atualizar();
        //**************************

        if (ClsModulo.critica != "")
        {
            Mensagem(ClsModulo.critica.ToString());
        }
        lblGrid.Text = ClsModulo.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de módulos.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_modulo.Text.Trim() != "")
        {
            if (this.txtcd_modulo.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Modulo ClsModulo = new Modulo(Application["StrConexao"].ToString());

        ClsModulo.NomeDoModulo = this.txtnm_modulo.Valor.ToString().Trim();
        
        resp = ClsModulo.Grava();
        //*********************
        txtcd_modulo.Text = ClsModulo.CodigoDoModulo.ToString();

        if (ClsModulo.critica != "")
        {
            Mensagem(ClsModulo.critica.ToString());
        }
        lblGrid.Text = ClsModulo.TrazGrid();

        this.btn_salvar.Enabled = !resp;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Modulo ClsModulo = new Modulo(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsModulo.CodigoDoModulo = Convert.ToInt16(this.txtcd_modulo.Text.ToString());

        resp = ClsModulo.Consulta();
        //************************
        txtcd_modulo.Text = ClsModulo.CodigoDoModulo.ToString();
        txtnm_modulo.Valor = ClsModulo.NomeDoModulo.Trim();
       
        if (ClsModulo.critica != "")
        {
            Mensagem(ClsModulo.critica.ToString());
        }
        lblGrid.Text = ClsModulo.TrazGrid();

        this.btn_salvar.Enabled = !resp;
    
    }

    public void LimpaCampo()
    {
        this.txtnm_modulo.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Modulo ClsModulo = new Modulo(Application["StrConexao"].ToString());

        ClsModulo.CodigoDoModulo = Convert.ToInt16(this.txtcd_modulo.Text.ToString());

        resp = ClsModulo.Excluir();
        //**********************
        txtcd_modulo.Text = ClsModulo.CodigoDoModulo.ToString();
        txtnm_modulo.Valor = ClsModulo.NomeDoModulo.Trim();
       
        if (ClsModulo.critica != "")
        {
            Mensagem(ClsModulo.critica.ToString());
        }
        lblGrid.Text = ClsModulo.TrazGrid();

        this.btn_salvar.Enabled = resp;
        this.LimpaCampo();
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
