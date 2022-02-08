using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Buffer = true;
        Response.Expires = -1;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");

        int hora = DateTime.Now.Hour;
        string mensagem = "";

        if (hora <= 12)
        {
            mensagem = "Bom Dia ";
        }
        else
        {
            if (hora > 12 && hora <= 18)
            {
                mensagem = "Boa Tarde ";
            }
            else
            {
                mensagem = "Boa Noite ";
            }
        }
       // divmenu.Visible = true;
        if (!IsPostBack)
        {
            try
            {
                if (false)
                {
                    if (Session["usernomecliente"] != "")
                    {
                        lblUser.Text = mensagem + Session["usernomecliente"].ToString();
                    }
                    else
                    {
                        lblUser.Text = mensagem + "Visitante";
                    }
                }
            }
            catch (Exception Erro)
            {
                Response.End();
            }
            
            if (Session["usernomecliente"] != "")
            {
                lblUser.Text = mensagem + Session["usernomecliente"].ToString();
            }
            else
            {
                lblUser.Text = mensagem + "Visitante";
            }
        }
   
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        lblMenu.Text = ClsLoja.TrazMenu();
        lblSubMenu.Text = ClsLoja.TrazSubMenu();

    }

    override protected void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        this.Unload += new EventHandler(Page_Unload);
    }

    private void Page_Unload(object sender, System.EventArgs e)
    {
        if (this.lblMsg.Text.Trim() != "")
        {
            this.lblMsg.Text = "<script>mostraMsg('" + this.lblMsg.Text.Trim() + "');</script>";
        }
    }

    public void validaSenha(object sender, EventArgs e)
    {

    }
}
