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

public partial class esqueceusenha : System.Web.UI.Page
{
    public string acao = ""; 

    protected void Page_Load(object sender, EventArgs e)
    {
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

        if (ClsAreaCliente.TrazSenhaCliente(this.txtemail.Valor.ToString().Trim()))
        {
            Enviar(ClsAreaCliente.Senha, ClsAreaCliente.NomeClienteLogado);
        }
        else
        {
            Mensagem(ClsAreaCliente.critica);
            this.txtemail.Valor = "";
        }
    }

    public void voltar(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }

    public void Enviar(string senha, string nome)
    {
        if (txtemail.Valor == "")
        {
            Mensagem("E-mail deve ser informado. Verifique.");
            return;
        }

        string sFrom = "atendimento@luzonline.com.br";
        string sTo = txtemail.Valor.ToString().Trim();
        string sSubject = "Contato LuzOnline - Pedido de Senha";
        string sMailServer = "smtp.luzonline.com.br";
        string corpo = "";

        corpo += "<html><body><div style='font-family: Courier New; font-size: 12px;'>";
        corpo += "  Pedido feito pelo LuzOnline - " + DateTime.Now.ToString() + "<br><br>";
        corpo += "  Prezado(a) " + nome.Trim() + "<br><br>";
        corpo += "  A sua senha para acesso ao Portal LuzOnline é: " + "<i><b>" + senha.Trim() + "</b></i><br><br>";
        corpo += "  Favor não responder a este e-mail.<br><br><br>";
        corpo += "  Atenciosamente,<br><br>";
        corpo += "  Equipe LuzOnline<br>";
        corpo += "</div></body></html>";

        string sBody = corpo;

        
        System.Web.Mail.MailMessage email = new System.Web.Mail.MailMessage();
        email.From = sFrom.ToString();
        email.To = sTo.ToString();
        email.Priority = System.Web.Mail.MailPriority.Normal;
        email.Subject = sSubject.Trim();
        email.BodyFormat = System.Web.Mail.MailFormat.Html;
        email.Body = corpo;

        System.Web.Mail.SmtpMail.SmtpServer = sMailServer.ToString();

        email.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = sMailServer.ToString();
        email.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
        email.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
        email.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = "atendimento@luzonline.com.br";
        email.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = "luzadm10";

        try
        {
            System.Web.Mail.SmtpMail.Send(email);
            Response.Redirect("login.aspx?acao=esqueceu");
        }
        catch (Exception err)
        {
            Mensagem("E-mail informado não existe ou está inválido. Verifique.");
            return;
        }
        return;
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
