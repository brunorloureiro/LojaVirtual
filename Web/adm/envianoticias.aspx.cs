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

public partial class envianoticias : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.importa.Visible = false;
        }
        else
        {
            this.importa.Visible = true;
            EnviaEmail ClsEnviaEmail = new EnviaEmail(Application["StrConexao"].ToString());
        }
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void enviar(object sender, EventArgs e)
    {
        bool resp;
        EnviaEmail ClsEnviaEmail = new EnviaEmail(Application["StrConexao"].ToString());

        resp = ClsEnviaEmail.EnviaNoticia();
        //**************************

        if (ClsEnviaEmail.critica != "")
        {
            Mensagem(ClsEnviaEmail.critica.ToString());
        }
    }

 }
