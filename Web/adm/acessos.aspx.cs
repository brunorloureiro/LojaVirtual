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

public partial class acessos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.acesso.Visible = false;
        }
        else
        {
            this.acesso.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());

            lblGrid.Text = ClsAcesso.TrazGrid();
            this.lblMsg.Text = "Controle de Acessos dos Usuários.";

            if (Session["userlogado"] != null)
            {
                if ((bool)Session["userlogado"] == false)
                {
                    Response.Redirect("login.aspx");
                }
            }

        }
}

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());
        ClsAcesso.CarregaLista(this.ddlusuarios, "Usuario", (Request["ddlusuarios"] == null ? "0" : (string)Request["ddlusuarios"]), "Escolha um Usuário");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());
        ClsAcesso.CodigoDoAcesso = Convert.ToInt16(this.txtcd_acesso.Text.ToString());
        ClsAcesso.CodigoDoUsuario = Convert.ToInt16(this.ddlusuarios.SelectedValue);
        ClsAcesso.Gravar = Convert.ToInt16(this.chkgrava.Checked);
        ClsAcesso.Excluir = Convert.ToInt16(this.chkexclui.Checked);
        ClsAcesso.Consultar = Convert.ToInt16(this.chkconsulta.Checked);
        ClsAcesso.Entrada = Convert.ToInt16(this.chkentrada.Checked);
        ClsAcesso.Baixa = Convert.ToInt16(this.chkbaixa.Checked);
        ClsAcesso.Retirada = Convert.ToInt16(this.chkretirada.Checked);
        ClsAcesso.Importa = Convert.ToInt16(this.chkimporta.Checked);
        ClsAcesso.EstoqueNegativo = Convert.ToInt16(this.chkestneg.Checked);
        ClsAcesso.ControlaVenda = Convert.ToInt16(this.chkcontvenda.Checked);
        ClsAcesso.ControlaFinanceiro = Convert.ToInt16(this.chkcontfinanc.Checked);
        ClsAcesso.ControlaLoja = Convert.ToInt16(this.chkcontloja.Checked);

        resp = ClsAcesso.Atualizar();
        //**************************

        if (ClsAcesso.critica != "")
        {
            Mensagem(ClsAcesso.critica.ToString());
        }
        lblGrid.Text = ClsAcesso.TrazGrid();

        if (resp)
        {
            this.btn_atualizar.Enabled = resp;
            this.btn_salvar.Enabled = !resp;
        }
        else
        {
            this.btn_atualizar.Enabled = !resp;
            this.btn_salvar.Enabled = resp;
        }
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

   
    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Controle de Acessos dos Usuários.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_acesso.Text.Trim() != "")
        {
            if (this.txtcd_acesso.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());

        ClsAcesso.CodigoDoAcesso = Convert.ToInt16(this.txtcd_acesso.Text.ToString());
        ClsAcesso.CodigoDoUsuario = Convert.ToInt16(this.ddlusuarios.SelectedValue);
        ClsAcesso.Gravar = Convert.ToInt16(this.chkgrava.Checked);
        ClsAcesso.Excluir = Convert.ToInt16(this.chkexclui.Checked);
        ClsAcesso.Consultar = Convert.ToInt16(this.chkconsulta.Checked);
        ClsAcesso.Entrada = Convert.ToInt16(this.chkentrada.Checked);
        ClsAcesso.Baixa = Convert.ToInt16(this.chkbaixa.Checked);
        ClsAcesso.Retirada = Convert.ToInt16(this.chkretirada.Checked);
        ClsAcesso.Importa = Convert.ToInt16(this.chkimporta.Checked);
        ClsAcesso.EstoqueNegativo = Convert.ToInt16(this.chkestneg.Checked);
        ClsAcesso.ControlaVenda = Convert.ToInt16(this.chkcontvenda.Checked);
        ClsAcesso.ControlaFinanceiro = Convert.ToInt16(this.chkcontfinanc.Checked);
        ClsAcesso.ControlaLoja = Convert.ToInt16(this.chkcontloja.Checked);
        
        resp = ClsAcesso.Grava();
        //*********************
        txtcd_acesso.Text = ClsAcesso.CodigoDoAcesso.ToString();

        if (ClsAcesso.critica != "")
        {
            Mensagem(ClsAcesso.critica.ToString());
        }
        lblGrid.Text = ClsAcesso.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsAcesso.CodigoDoAcesso = Convert.ToInt16(this.txtcd_acesso.Text.ToString());

        resp = ClsAcesso.Consulta();
        //************************
        txtcd_acesso.Text = ClsAcesso.CodigoDoAcesso.ToString();
        ddlusuarios.SelectedValue = ClsAcesso.CodigoDoUsuario.ToString();
        chkgrava.Checked = ClsAcesso.Gravar == 1 ? true : false;
        chkexclui.Checked = ClsAcesso.Excluir == 1 ? true : false;
        chkconsulta.Checked = ClsAcesso.Consultar == 1 ? true : false;
        chkentrada.Checked = ClsAcesso.Entrada == 1 ? true : false;
        chkbaixa.Checked = ClsAcesso.Baixa == 1 ? true : false;
        chkretirada.Checked = ClsAcesso.Retirada == 1 ? true : false;
        chkimporta.Checked = ClsAcesso.Importa == 1 ? true : false;
        chkestneg.Checked = ClsAcesso.EstoqueNegativo == 1 ? true : false;
        chkcontvenda.Checked = ClsAcesso.ControlaVenda == 1 ? true : false;
        chkcontfinanc.Checked = ClsAcesso.ControlaFinanceiro == 1 ? true : false;
        chkcontloja.Checked = ClsAcesso.ControlaLoja == 1 ? true : false;
      
        if (ClsAcesso.critica != "")
        {
            Mensagem(ClsAcesso.critica.ToString());
        }
        lblGrid.Text = ClsAcesso.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());
        ddlusuarios.SelectedValue = ClsAcesso.CodigoDoUsuario.ToString();
        this.chkgrava.Checked = false;
        this.chkexclui.Checked = false;
        this.chkconsulta.Checked = false;
        this.chkentrada.Checked = false;
        this.chkbaixa.Checked = false;
        this.chkretirada.Checked = false;
        this.chkimporta.Checked = false;
        this.chkestneg.Checked = false;
        this.chkcontvenda.Checked = false;
        this.chkcontfinanc.Checked = false;
        this.chkcontloja.Checked = false;
     }

    public void exclui(object sender, EventArgs e)
    {
        bool resp;
        Acesso ClsAcesso = new Acesso(Application["StrConexao"].ToString());

        ClsAcesso.CodigoDoAcesso = Convert.ToInt16(this.txtcd_acesso.Text.ToString());

        resp = ClsAcesso.Exclui();
        //**********************
        txtcd_acesso.Text = ClsAcesso.CodigoDoAcesso.ToString();
        ddlusuarios.SelectedValue = ClsAcesso.CodigoDoUsuario.ToString();
        chkgrava.Checked = ClsAcesso.Gravar == 1 ? true : false;
        chkexclui.Checked = ClsAcesso.Excluir == 1 ? true : false;
        chkconsulta.Checked = ClsAcesso.Consultar == 1 ? true : false;
        chkentrada.Checked = ClsAcesso.Entrada == 1 ? true : false;
        chkbaixa.Checked = ClsAcesso.Baixa == 1 ? true : false;
        chkretirada.Checked = ClsAcesso.Retirada == 1 ? true : false;
        chkimporta.Checked = ClsAcesso.Importa == 1 ? true : false;
        chkestneg.Checked = ClsAcesso.EstoqueNegativo == 1 ? true : false;
        chkcontvenda.Checked = ClsAcesso.ControlaVenda == 1 ? true : false;
        chkcontfinanc.Checked = ClsAcesso.ControlaFinanceiro == 1 ? true : false;
        chkcontloja.Checked = ClsAcesso.ControlaLoja == 1 ? true : false;

        if (ClsAcesso.critica != "")
        {
            Mensagem(ClsAcesso.critica.ToString());
        }
        lblGrid.Text = ClsAcesso.TrazGrid();

        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.LimpaCampo();
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
