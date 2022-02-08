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

public partial class faleconosco : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void enviar(object sender, EventArgs e)
    {
        ValidaInformacoes();

        string sFrom = txtemail.Valor.ToString().Trim();
        string sTo = "atendimento@luzonline.com.br";
        string sSubject = "Contato LuzOnline - " + motivo.Value.ToString().Trim();
        string sMailServer = "smtp.luzonline.com.br";
        string corpo = "";

        corpo += "<html><body><div style='font-family: Courier New; font-size: 12px;'>";
        corpo += "  Contato feito pelo LuzOnline - " + DateTime.Now.ToString() + "<br><br>";
        corpo += "  Nome .........: " + txtnome.Valor.ToString().Trim() + "<br>";
        corpo += "  E-mail .......: " + txtemail.Valor.ToString().Trim() + "<br>";
        corpo += "  Telefone .....:  (" + txtddd_tel.Valor.ToString().Trim() + ") " + txttelefone.Valor.ToString().Trim() + "<br>";
        corpo += "  Celular ......:  (" + txtddd_cel.Valor.ToString().Trim() + ") " + txtcelular.Valor.ToString().Trim() + "<br>";
        corpo += "  Motivo  ......: " + motivo.Value.ToString().Trim() + "<br>";
        corpo += "  Mensagem .....: " + "<br><i><b>" + txtmensagem.Value.ToString().Trim() + "</b></i><br>";
        corpo += "</div></body></html>";

        string sBody = corpo;

        //'##--------------------------------------------------------------------
        //'##  Envio de Emails pelo SMTP Autênticado usando CDO usando o ASP.NET
        //'##--------------------------------------------------------------------
        //'# Mais informações sobre as possiveis bibliotecas são encontradas no link:
        //'#   http://msdn.microsoft.com/library/en-us/dncdsys/html/cdo_roadmap.asp
        //'#
        //'# Documentação do CDO pode ser encontrada no link:
        //'#   http://msdn.microsoft.com/library/en-us/dnanchor/html/collabdataobjects.asp
        //'#
        //'# Para ler sobre as possiveis configurações do objeto de configuração, acesse:
        //'#   http://msdn.microsoft.com/library/en-us/cdosys/html/_cdosys_schema_configuration.asp
        //'#   http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cdosys/html/_cdosys_imessage_interface.asp
        //'#
        //'# IMPORTANTE
        //'# O codigos de erros estão documentados em:
        //'#   http://msdn.microsoft.com/library/en-us/cdosys/html/_cdosys_error_codes.asp
        //'#
        //'# Para ler sobre a comparação do CDO com CDONTS acesse:
        //'#   http://support.microsoft.com/default.aspx?scid=kb;en-us;177850
        //'##--------------------------------------------------------------------

        System.Web.Mail.MailMessage email = new System.Web.Mail.MailMessage();
        email.From = sFrom.ToString();
        email.To = sTo.ToString();
        //  email.ReplyTo = "EMailDeResposta@DominioDeResposta.com"
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
            Response.Redirect("RespostaFaleConosco.aspx");
        }
        catch (Exception err)
        {
            Mensagem("E-mail informado não existe ou está inválido. Verifique.");
            return;
        }
        return;
    }

    public void ValidaInformacoes()
    {
        if (txtnome.Valor == "")
        {
            Mensagem("Nome deve ser informado. Verifique.");
            return;
        }

        if (txttelefone.Valor == "")
        {
            Mensagem("Telefone deve ser informado. Verifique.");
            return;
        }

        if (txtemail.Valor == "")
        {
            Mensagem("E-mail deve ser informado. Verifique.");
            return;
        }

        if (txtmensagem.Value == "")
        {
            Mensagem("Mensagem deve ser informada. Verifique.");
            return;
        }

    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }
}
