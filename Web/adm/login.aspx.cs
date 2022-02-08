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
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "Exclusivo para a Administração do Sistema.";
        this.txtnm_usuario.Focus();
    }

    public void entrar(object sender, EventArgs e)
    {
        Usuario ClsLogin = new Usuario(Application["StrConexao"].ToString());
        
        if (ClsLogin.FazLogin(this.txtnm_usuario.Valor.ToString().Trim(), this.txtsenha.Text.ToString().Trim()))
        {
            Session["usernomeadm"] = this.txtnm_usuario.Valor.ToString().Trim();
            Session["cd_user"] = ClsLogin.UsuarioLogado;
 
            if (ClsLogin.crit_adm.ToString().Trim() != "")
            {
                Session["useradm"] = true;
            }
            else 
            {
                Session["useradm"] = false;
            }

            Session["bl_loja"] = ClsLogin.ControlaLoja;
            Session["bl_financ"] = ClsLogin.ControlaFinanceiro;
            Session["bl_contven"] = ClsLogin.ControlaVenda;
            Session["bl_estneg"] = ClsLogin.EstoqueNegativo;
            Session["bl_importa"] = ClsLogin.PermiteImportar;
            Session["bl_retirada"] = ClsLogin.PermiteRetirada;
            Session["bl_entrada"] = ClsLogin.PermiteEntrada;
            Session["bl_baixa"] = ClsLogin.PermiteBaixa;
            Session["bl_consulta"] = ClsLogin.PermiteConsultar;
            Session["bl_exclui"] = ClsLogin.PermiteExcluir;
            Session["bl_grava"] = ClsLogin.PermiteGravar;

            Session["userlogado"] = true;
            
            Response.Redirect("clientes.aspx");
        }
        else
        {
            Mensagem(ClsLogin.critica);
            this.txtsenha.Text = "";
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
