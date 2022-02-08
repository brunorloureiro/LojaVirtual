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

public partial class login : System.Web.UI.Page
{
    public string acao = ""; 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["acao"] != null)
        {
            acao = Request["acao"];
        }
        if (!IsPostBack)
        {
            if (acao == "esqueceu")
            {
                Mensagem("Senha enviada para o seu e-mail. Verifique");
            }
        }

       lblMsg.Text = "Exclusivo para Clientes.";
       this.txtemail.Focus();

        
    }

    public void entrar(object sender, EventArgs e)
    {
        AreaCliente ClsAreaCliente = new AreaCliente(Application["StrConexao"].ToString());

        if (txtemail.Valor == "")
        {
            Mensagem("E-mail deve ser informado. Verifique");
            return;
        }

        if (txtcep.Valor != "" && txtcep.Valor != "0")
        {
            if (txtsenha.Text != "")
            {
                Mensagem("Senha e CEP não podem ser informados juntos. Verifique");
                return;
            }
        }

        if (rad_cli.Checked)
        {
            if (txtsenha.Text == "")
            {
                Mensagem("Senha deve ser informada. Verifique");
                return;
            }
            else
            {
                if (ClsAreaCliente.FazLoginCliente(this.txtemail.Valor.ToString().Trim(), this.txtsenha.Text.ToString().Trim()))
                {
                    Session["usernomecliente"] = ClsAreaCliente.NomeClienteLogado;
                    Session["cd_cliente"] = ClsAreaCliente.ClienteLogado;

                    Session["clientelogado"] = true;

                    
                    if ((bool)Session["venda"])
                    {
                        if (acao == "compra")
                        {
                            Response.Redirect("../loja/carrinho.aspx");
                        }
                        else
                        {
                            Response.Redirect("pedidos.aspx");
                            //Response.Redirect("../loja/carrinho.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("pedidos.aspx");
                        //Response.Redirect("../loja/carrinho.aspx");
                    }
                }
                else
                {
                    Mensagem(ClsAreaCliente.critica);
                    this.txtsenha.Text = "";
                }

            }
        }
        else
        {
            if (txtcep.Valor == "" || txtcep.Valor == "0")
            {
                Mensagem("CEP deve ser informado. Verifique");
                return;
            }
            else
            {
                if (ClsAreaCliente.VerificaEmailCliente(this.txtemail.Valor.ToString().Trim()))
                {
                    string url = "";
                    url += "termo.aspx?acao=novo&email=" + this.txtemail.Valor + "&cep=" + this.txtcep.Valor + "";
                    Response.Redirect(url);
                 
                }
                else
                {
                    //string url = "";
                    //url += "pedido.aspx";
                    //Response.Redirect(url);
                    Mensagem(ClsAreaCliente.critica);
                    this.txtemail.Valor = "";
                }
    
            }
        
        }

    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void sair(object sender, EventArgs e)
    {
        Response.Redirect("~/default.aspx");
    }
}
