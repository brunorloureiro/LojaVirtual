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

public partial class avise : System.Web.UI.Page
{

    public string produto = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btn_salvar.Enabled = true;
        AviseMe ClsAviseMe = new AviseMe(Application["StrConexao"].ToString());

        
        produto = Request["codigo"];

    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void salvar(object sender, EventArgs e)
    {
        if (this.txtnome.Valor == "")
        {
            Mensagem("Nome deve ser informado. Verifique.");
            return;
        }

        if (this.txtemail.Valor == "")
        {
            Mensagem("E-mail deve ser informado. Verifique.");
            return;
        }

        bool resp;
        AviseMe ClsAviseMe = new AviseMe(Application["StrConexao"].ToString());

        ClsAviseMe.CodigoDoProduto = produto.ToString().Trim();
        ClsAviseMe.Nome  = this.txtnome.Valor.ToString().Trim();
        ClsAviseMe.Email = this.txtemail.Valor.ToString().Trim();
        
        resp = ClsAviseMe.Grava();
        //************************
        if (resp)
        {
            Mensagem("Obrigado! Assim que o produto estiver disponível avisaremos a você.");
        }
        else
        {
            Mensagem("Ocorreu um erro. Tente Novamente mais tarde.");
            this.txtnome.Valor = "";
            this.txtemail.Valor = "";
        }

        this.btn_salvar.Enabled = true;
        string script = "<script type='text/javascript' language='javascript'>window.close();</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "buscapai", script);
    }

}
