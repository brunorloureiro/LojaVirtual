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

public partial class busca : System.Web.UI.Page
{
    string tipo = "";
    string tipoproduto = "";
    string precoini = "";
    string precofim = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());

        tipo = Request["tipo"].Trim();
        tipoproduto = Request["tipoproduto"];
        precoini = Request["precoini"];
        precofim = Request["precofim"];

        lblTitulo.Text = "Resultado da Busca";
        if (tipo == "S")
        {
            lblPrincipal.Text = ClsLoja.TrazResultadoBuscaSimples(Request["descricao"].Trim());
        }
        else
        {
            if (tipo == "A")
            {   
                lblPrincipal.Text = ClsLoja.TrazResultadoBuscaAvancada(Request["descricao"].Trim(), tipoproduto, precoini, precofim);
            }
        }
        lblNenhum.Style.Add("display", ClsLoja.TrazNumeroDeRegistrosDaBusca() == true ? "none" : "");
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
