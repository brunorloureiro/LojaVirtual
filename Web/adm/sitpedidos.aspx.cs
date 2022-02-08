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

public partial class sitpedido : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.SituacaoPedido.Visible = false;
        }
        else
        {
            this.SituacaoPedido.Visible = true;
            this.btn_atualizar.Enabled = false;
           
            SituacaoPedido ClsSituacaoPedido = new SituacaoPedido(Application["StrConexao"].ToString());

            this.lblMsg.Text = "Atualização de Status dos Pedidos.";
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
        SituacaoPedido ClsSituacaoPedido = new SituacaoPedido(Application["StrConexao"].ToString());
        ClsSituacaoPedido.Pedido = Convert.ToInt32(this.txtcd_pedido.Valor.ToString());
        ClsSituacaoPedido.Status = this.status.Value.ToString().Trim();
        ClsSituacaoPedido.Rastreio = this.txtrastreio.Valor.ToString().Trim();
        
        resp = ClsSituacaoPedido.Atualizar();
        //**************************

        if (ClsSituacaoPedido.critica != "")
        {
            Mensagem(ClsSituacaoPedido.critica.ToString());
        }
      
        if (resp)
        {
            this.btn_atualizar.Enabled = resp;
        }
        else
        {
            this.btn_atualizar.Enabled = !resp;
        }
    }

    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Atualização de Status dos Pedidos.";
        this.btn_atualizar.Enabled = false;
     }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        SituacaoPedido ClsSituacaoPedido = new SituacaoPedido(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSituacaoPedido.Pedido = Convert.ToInt32(this.txtcd_pedido.Valor.ToString());

        resp = ClsSituacaoPedido.Consulta();
        //************************
        txtcd_pedido.Valor = ClsSituacaoPedido.Pedido.ToString();
        status.Value = ClsSituacaoPedido.Status.Trim();
        txtrastreio.Valor = ClsSituacaoPedido.Rastreio.Trim();
       
        if (ClsSituacaoPedido.critica != "")
        {
            Mensagem(ClsSituacaoPedido.critica.ToString());
        }
    
        this.btn_atualizar.Enabled = resp;
     
    }

    public void trazpedido(object sender, EventArgs e)
    {
        bool resp;
        SituacaoPedido ClsSituacaoPedido = new SituacaoPedido(Application["StrConexao"].ToString());

        ClsSituacaoPedido.Pedido = Convert.ToInt32(this.txtcd_pedido.Valor.ToString());

        resp = ClsSituacaoPedido.VerificaPedido();
        //************************
        txtcd_pedido.Valor = ClsSituacaoPedido.Pedido.ToString();
        status.Value = ClsSituacaoPedido.Status.Trim();
        txtrastreio.Valor = ClsSituacaoPedido.Rastreio.Trim();

        if (ClsSituacaoPedido.critica != "")
        {
            Mensagem(ClsSituacaoPedido.critica.ToString());
            this.LimpaCampo();
        }
    
        this.btn_atualizar.Enabled = resp;
   
    }

    public void LimpaCampo()
    {
        this.txtcd_pedido.Valor = "0";
        this.status.Value = "Aguardando Confirmação de Pagamento";
        this.txtrastreio.Valor = "";
    }


    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
