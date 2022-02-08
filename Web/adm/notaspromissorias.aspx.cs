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

public partial class notaspromissorias : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_financ"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.notaprom.Visible = false;
        }
        else
        {
            this.notaprom.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());
            lblGrid.Text = ClsNotaPromissoria.TrazGrid();

            this.lblMsg.Text = "Controle de Notas Promissórias.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());
        ClsNotaPromissoria.CarregaLista(this.ddlclientes, "Cliente", (Request["ddlclientes"] == null ? "0" : (string)Request["ddlclientes"]), "Escolha um Cliente");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());

        ClsNotaPromissoria.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsNotaPromissoria.CodigoDaNotaPromissoria = Convert.ToInt32(this.txtcd_notaprom.Valor.ToString());
        ClsNotaPromissoria.CodigoDoCliente = Convert.ToInt32(this.ddlclientes.SelectedValue);
        ClsNotaPromissoria.Situacao = this.situacao.Value.ToString().Trim();
        ClsNotaPromissoria.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsNotaPromissoria.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        
        resp = ClsNotaPromissoria.Atualizar();
        //**************************

        if (ClsNotaPromissoria.critica != "")
        {
            Mensagem(ClsNotaPromissoria.critica.ToString());
        }
        lblGrid.Text = ClsNotaPromissoria.TrazGrid();

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
        this.lblMsg.Text = "Controle de Notas Promissórias.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        bool resp;
        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());

        ClsNotaPromissoria.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsNotaPromissoria.CodigoDaNotaPromissoria = Convert.ToInt32(this.txtcd_notaprom.Valor.ToString());
        ClsNotaPromissoria.CodigoDoCliente = Convert.ToInt32(this.ddlclientes.SelectedValue);
        ClsNotaPromissoria.Situacao = this.situacao.Value.ToString().Trim();
        ClsNotaPromissoria.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsNotaPromissoria.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        resp = ClsNotaPromissoria.Grava();
        //*********************
        txtcd_notaprom.Valor = ClsNotaPromissoria.CodigoDaNotaPromissoria.ToString();

        if (ClsNotaPromissoria.critica != "")
        {
            Mensagem(ClsNotaPromissoria.critica.ToString());
        }
        lblGrid.Text = ClsNotaPromissoria.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsNotaPromissoria.CodigoDaNotaPromissoria = Convert.ToInt32(this.txtcd_notaprom.Valor.ToString());

        resp = ClsNotaPromissoria.Consulta();
        //************************
        txtcd_notaprom.Valor = ClsNotaPromissoria.CodigoDaNotaPromissoria.ToString();
        ddlclientes.SelectedValue = ClsNotaPromissoria.CodigoDoCliente.ToString();
        situacao.Value = ClsNotaPromissoria.Situacao.Trim();
        txtdt_vencto.Valor = ClsNotaPromissoria.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtvalor.Valor = ClsNotaPromissoria.Valor.ToString();
        

        if (ClsNotaPromissoria.critica != "")
        {
            Mensagem(ClsNotaPromissoria.critica.ToString());
        }
        lblGrid.Text = ClsNotaPromissoria.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());
        ddlclientes.SelectedValue = ClsNotaPromissoria.CodigoDoCliente.ToString();
        this.txtdt_vencto.Valor = "";
        this.txtvalor.Valor = "0";
        this.situacao.Value = "A Receber";
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        NotaPromissoria ClsNotaPromissoria = new NotaPromissoria(Application["StrConexao"].ToString());

        ClsNotaPromissoria.CodigoDaNotaPromissoria = Convert.ToInt32(this.txtcd_notaprom.Valor.ToString());

        resp = ClsNotaPromissoria.Excluir();
        //**********************
        txtcd_notaprom.Valor = "0";
        ddlclientes.SelectedValue = ClsNotaPromissoria.CodigoDoCliente.ToString();
        situacao.Value = ClsNotaPromissoria.Situacao.Trim();
        txtdt_vencto.Valor = ClsNotaPromissoria.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtvalor.Valor = ClsNotaPromissoria.Valor.ToString();
        
        if (ClsNotaPromissoria.critica != "")
        {
            Mensagem(ClsNotaPromissoria.critica.ToString());
        }
        lblGrid.Text = ClsNotaPromissoria.TrazGrid();

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
