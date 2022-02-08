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


        if (!IsPostBack)
        {
            try
            {
                if (Request.Url.ToString().ToLower().IndexOf("login.aspx") > 0)
                {
                    if (Session["clientelogado"] != null)
                    {
                        if ((bool)Session["clientelogado"] == true)
                        {
                            Response.Redirect("../cliente/pedidos.aspx");
                            Response.End();
                        }
                       }
                    Session["clientelogado"] = false;
                    Session["usernomecliente"] = "";
                    lblUser.Text = "";
                    sair.Visible = false;
                    divmenu.Visible = false;
                    principal.Style.Add("float", "center");

                    return;
                }

                if (false)
                {
                    if (Session["usernomecliente"] != "")
                    {
                        lblUser.Text = "Olá " + Session["usernomecliente"].ToString();
                        sair.Visible = true;
                        divmenu.Visible = true;
                        principal.Style.Add("float", "left");
                    }
                    else
                    {
                        lblUser.Text = "Olá Visitante";
                        sair.Visible = false;
                        divmenu.Visible = false;
                        principal.Style.Add("float", "center");
                    }
                }
            }
            catch (Exception Erro)
            {
                Response.End();
            }
            if (Session["usernomecliente"] != "")
            {
                lblUser.Text = "Olá " + Session["usernomecliente"].ToString();
            }
            else
            {
                lblUser.Text = "Olá Visitante";
            }
        }

        AreaCliente ClsAreaCliente = new AreaCliente(Application["StrConexao"].ToString());

        if (Request["acao"] != null)
        {
            if (Request["acao"] != "novo")
            {
                lblMenu.Text = ClsAreaCliente.TrazMenu();
                //lblSubMenu.Text = ClsAreaCliente.TrazSubMenu();
            }
        }
        else
        {
            lblMenu.Text = ClsAreaCliente.TrazMenu();
            //lblSubMenu.Text = ClsAreaCliente.TrazSubMenu();
        }


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
