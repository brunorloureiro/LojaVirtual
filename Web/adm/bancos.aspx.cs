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

public partial class bancos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["bl_financ"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.banco.Visible = false;
        }
        else
        {
            this.banco.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
            Banco ClsBanco = new Banco(Application["StrConexao"].ToString());
            lblGrid.Text = ClsBanco.TrazGrid();

            this.lblMsg.Text = "Cadastro de Bancos para Controle Financeiro.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());
        ClsBanco.CodigoDoBanco = Convert.ToInt16(this.txtcd_banco.Text.ToString());
        ClsBanco.NomeDoBanco = this.txtnm_banco.Valor.ToString().Trim();
        ClsBanco.Sigla = this.txtsigla.Valor.ToString().Trim();
        
        resp = ClsBanco.Atualizar();
        //**************************

        if (ClsBanco.critica != "")
        {
            Mensagem(ClsBanco.critica.ToString());
        }
        lblGrid.Text = ClsBanco.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de Bancos para Controle Financeiro.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_banco.Text.Trim() != "")
        {
            if (this.txtcd_banco.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());

        ClsBanco.NomeDoBanco = this.txtnm_banco.Valor.ToString().Trim();
        ClsBanco.Sigla = this.txtsigla.Valor.ToString().Trim();
        
        resp = ClsBanco.Grava();
        //*********************
        txtcd_banco.Text = ClsBanco.CodigoDoBanco.ToString();

        if (ClsBanco.critica != "")
        {
            Mensagem(ClsBanco.critica.ToString());
        }
        lblGrid.Text = ClsBanco.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsBanco.CodigoDoBanco = Convert.ToInt16(this.txtcd_banco.Text.ToString());

        resp = ClsBanco.Consulta();
        //************************
        txtcd_banco.Text = ClsBanco.CodigoDoBanco.ToString();
        txtnm_banco.Valor = ClsBanco.NomeDoBanco.Trim();
        txtsigla.Valor = ClsBanco.Sigla.Trim();
        
        if (ClsBanco.critica != "")
        {
            Mensagem(ClsBanco.critica.ToString());
        }
        lblGrid.Text = ClsBanco.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());
        this.txtnm_banco.Valor = "";
        this.txtsigla.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Banco ClsBanco = new Banco(Application["StrConexao"].ToString());

        ClsBanco.CodigoDoBanco = Convert.ToInt16(this.txtcd_banco.Text.ToString());

        resp = ClsBanco.Excluir();
        //**********************
        txtcd_banco.Text = ClsBanco.CodigoDoBanco.ToString();
        txtnm_banco.Valor = ClsBanco.NomeDoBanco.Trim();
        txtsigla.Valor = ClsBanco.Sigla.Trim();
       
        if (ClsBanco.critica != "")
        {
            Mensagem(ClsBanco.critica.ToString());
        }
        lblGrid.Text = ClsBanco.TrazGrid();

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
