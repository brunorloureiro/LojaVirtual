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

public partial class default_cliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["clientelogado"] != null)
        //{
        //    if ((bool)Session["clientelogado"] == false)
        //    {
        //        if (Request["acao"] == "novo")
        //        {
        //            Response.Redirect("cadastro.aspx");
        //        }
        //        else
        //        {
        //            Response.Redirect("../loja/principal.aspx");  
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("login.aspx?acao=logado");
        //    }
        //}
        Response.Redirect("login.aspx?acao=logado");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

}
