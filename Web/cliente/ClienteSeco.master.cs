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

        sair.Visible = false;
        divmenu.Visible = false;
        principal.Style.Add("float", "center");
    
        AreaCliente ClsAreaCliente = new AreaCliente(Application["StrConexao"].ToString());
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
