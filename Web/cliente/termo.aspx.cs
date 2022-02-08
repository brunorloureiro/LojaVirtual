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

public partial class termo : System.Web.UI.Page
{
    string email = "";
    string cep = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        email = Request["email"];
        cep = Request["cep"];
        this.chkaceito.Focus();
    }

    public void entrar(object sender, EventArgs e)
    {
        if (!this.chkaceito.Checked)
        {
            Mensagem("Para Continuar é necessário aceitar os Termos de Responsabilidade.");
            return;
        }
        else
        {
            string url = "";
            url += "cadastro.aspx?acao=novo&email=" + email.Trim() + "&cep=" + cep.Trim() + "";
            Response.Redirect(url);
        }
             
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }   
}
