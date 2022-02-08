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

public partial class retiradoestoque : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_importa"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.importa.Visible = false;
        }
        else
        {
            this.importa.Visible = true;
            RetiraEstoque ClsRetiraEstoque = new RetiraEstoque(Application["StrConexao"].ToString());
        }
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void importar(object sender, EventArgs e)
    {
        bool resp;
        RetiraEstoque ClsRetiraEstoque = new RetiraEstoque(Application["StrConexao"].ToString());

        resp = ClsRetiraEstoque.Importa();
        //**************************

        if (ClsRetiraEstoque.critica != "")
        {
            Mensagem(ClsRetiraEstoque.critica.ToString());
        }
    }

 }
