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

public partial class cheques : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_financ"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.cheque.Visible = false;
        }
        else
        {
            this.cheque.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());
            lblGrid.Text = ClsCheque.TrazGrid();

            this.lblMsg.Text = "Controle de Cheques.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());
        ClsCheque.CarregaLista(this.ddlbancos, "Banco", (Request["ddlbancos"] == null ? "0" : (string)Request["ddlbancos"]), "Escolha um Banco");
        ClsCheque.CarregaLista(this.ddlclientes, "Cliente", (Request["ddlclientes"] == null ? "0" : (string)Request["ddlclientes"]), "Escolha um Cliente");
        ClsCheque.CarregaLista(this.ddlsitcheques, "Sitcheque", (Request["ddlsitcheques"] == null ? "0" : (string)Request["ddlsitcheques"]), "Escolha uma Situação");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());

        ClsCheque.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsCheque.CodigoDoCheque = Convert.ToInt32(this.txtcd_cheque.Valor.ToString());
        ClsCheque.CodigoDoBanco = Convert.ToInt16(this.ddlbancos.SelectedValue);
        ClsCheque.CodigoDoCliente = Convert.ToInt32(this.ddlclientes.SelectedValue);
        ClsCheque.Tipo = this.tipo.Value.ToString().Trim();
        ClsCheque.CodigoDaSituacao = Convert.ToInt16(this.ddlsitcheques.SelectedValue);
        ClsCheque.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsCheque.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        
        resp = ClsCheque.Atualizar();
        //**************************

        if (ClsCheque.critica != "")
        {
            Mensagem(ClsCheque.critica.ToString());
        }
        lblGrid.Text = ClsCheque.TrazGrid();

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
        this.lblMsg.Text = "Controle de Cheques.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        bool resp;
        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());

        ClsCheque.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsCheque.CodigoDoCheque = Convert.ToInt32(this.txtcd_cheque.Valor.ToString());
        ClsCheque.CodigoDoBanco = Convert.ToInt16(this.ddlbancos.SelectedValue);
        ClsCheque.CodigoDoCliente = Convert.ToInt32(this.ddlclientes.SelectedValue);
        ClsCheque.Tipo = this.tipo.Value.ToString().Trim();
        ClsCheque.CodigoDaSituacao = Convert.ToInt16(this.ddlsitcheques.SelectedValue);
        ClsCheque.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsCheque.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        resp = ClsCheque.Grava();
        //*********************
        txtcd_cheque.Valor = ClsCheque.CodigoDoCheque.ToString();

        if (ClsCheque.critica != "")
        {
            Mensagem(ClsCheque.critica.ToString());
        }
        lblGrid.Text = ClsCheque.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCheque.CodigoDoCheque = Convert.ToInt32(this.txtcd_cheque.Valor.ToString());

        resp = ClsCheque.Consulta();
        //************************
        txtcd_cheque.Valor = ClsCheque.CodigoDoCheque.ToString();
        ddlbancos.SelectedValue = ClsCheque.CodigoDoBanco.ToString();
        ddlclientes.SelectedValue = ClsCheque.CodigoDoCliente.ToString();
        ddlsitcheques.SelectedValue = ClsCheque.CodigoDaSituacao.ToString();
        tipo.Value = ClsCheque.Tipo.Trim();
        txtdt_vencto.Valor = ClsCheque.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtvalor.Valor = ClsCheque.Valor.ToString();
        

        if (ClsCheque.critica != "")
        {
            Mensagem(ClsCheque.critica.ToString());
        }
        lblGrid.Text = ClsCheque.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());
        ddlbancos.SelectedValue   = ClsCheque.CodigoDoBanco.ToString();
        ddlclientes.SelectedValue = ClsCheque.CodigoDoCliente.ToString();
        this.txtdt_vencto.Valor = "";
        this.txtvalor.Valor = "0";
        this.tipo.Value = "Próprio";
        ddlsitcheques.SelectedValue = ClsCheque.CodigoDaSituacao.ToString();
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Cheque ClsCheque = new Cheque(Application["StrConexao"].ToString());

        ClsCheque.CodigoDoCheque = Convert.ToInt32(this.txtcd_cheque.Valor.ToString());

        resp = ClsCheque.Excluir();
        //**********************
        txtcd_cheque.Valor = "0";
        ddlbancos.SelectedValue = ClsCheque.CodigoDoBanco.ToString();
        ddlclientes.SelectedValue = ClsCheque.CodigoDoCliente.ToString();
        tipo.Value = ClsCheque.Tipo.Trim();
        ddlsitcheques.SelectedValue = ClsCheque.CodigoDaSituacao.ToString();
        txtdt_vencto.Valor = ClsCheque.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtvalor.Valor = ClsCheque.Valor.ToString();
        
        if (ClsCheque.critica != "")
        {
            Mensagem(ClsCheque.critica.ToString());
        }
        lblGrid.Text = ClsCheque.TrazGrid();

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
