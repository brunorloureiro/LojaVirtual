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
using System.Xml;
using System.IO;

public partial class descpedido : System.Web.UI.Page
{
   
    public string acao = ""; 
    int pedido = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        AreaCliente ClsAreaCliente = new AreaCliente(Application["StrConexao"].ToString());
        
        
        acao = Request["acao"];
        pedido = Convert.ToInt32(Request["pedido"]);
        
        lblTitulo.Text = "Pedido Nº " +  pedido.ToString().Trim();

        lblPrincipal.Text = ClsAreaCliente.CarregaDescricaoPedido(pedido);
            
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void Redireciona(object sender, EventArgs e)
    {
        Response.Redirect("pedidos.aspx");
    }
}
