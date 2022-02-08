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
//using Dominio;

public partial class tiposdepagamento : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        TiposDePagamento ClsTiposDePagamento = new TiposDePagamento(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsTiposDePagamento.TrazGrid();
        }

        if ((bool)Session["bl_exclui"] == true)
        {
            this.btn_excluir.Visible = true;
        }
        else
        {
            this.btn_excluir.Visible = false;
        }

        if ((bool)Session["bl_grava"] == true)
        {
            this.btn_novo.Visible = true;
            this.btn_atualizar.Visible = true;
            this.btn_salvar.Visible = true;
        }
        else
        {
            this.btn_novo.Visible = false;
            this.btn_atualizar.Visible = false;
            this.btn_salvar.Visible = false;
        }

        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        this.lblMsg.Text = "Gerenciamento de tipos de pagamento da Área Administrativa.";
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        TiposDePagamento ClsTiposDePagamento = new TiposDePagamento(Application["StrConexao"].ToString());
        ClsTiposDePagamento.CodigoDoTipoDePagamento = Convert.ToInt16(this.txtcd_tppagto.Text.ToString());
        ClsTiposDePagamento.NomeDoTipoDePagamento = this.txtnm_tppagto.Valor.ToString().Trim();
        ClsTiposDePagamento.NumeroDeParcelas = Convert.ToInt16(this.txtqt_vezes.Valor.ToString());
        
        resp = ClsTiposDePagamento.Atualizar();
        //**************************

        if (ClsTiposDePagamento.critica != "")
        {
            Mensagem(ClsTiposDePagamento.critica.ToString());
        }
        lblGrid.Text = ClsTiposDePagamento.TrazGrid();

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
        this.lblMsg.Text = "Gerenciamento de tipos de pagamento da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_tppagto.Text.Trim() != "")
        {
            if (this.txtcd_tppagto.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        TiposDePagamento ClsTiposDePagamento = new TiposDePagamento(Application["StrConexao"].ToString());

        ClsTiposDePagamento.NomeDoTipoDePagamento = this.txtnm_tppagto.Valor.ToString().Trim();
        ClsTiposDePagamento.NumeroDeParcelas = Convert.ToInt16(this.txtqt_vezes.Valor.ToString());
        
        resp = ClsTiposDePagamento.Grava();
        //*********************
        txtcd_tppagto.Text = ClsTiposDePagamento.CodigoDoTipoDePagamento.ToString();

        if (ClsTiposDePagamento.critica != "")
        {
            Mensagem(ClsTiposDePagamento.critica.ToString());
        }
        lblGrid.Text = ClsTiposDePagamento.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        TiposDePagamento ClsTiposDePagamento = new TiposDePagamento(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsTiposDePagamento.CodigoDoTipoDePagamento = Convert.ToInt16(this.txtcd_tppagto.Text.ToString());

        resp = ClsTiposDePagamento.Consulta();
        //************************
        txtcd_tppagto.Text = ClsTiposDePagamento.CodigoDoTipoDePagamento.ToString();
        txtnm_tppagto.Valor = ClsTiposDePagamento.NomeDoTipoDePagamento.Trim();
        txtqt_vezes.Valor = ClsTiposDePagamento.NumeroDeParcelas.ToString();
       
        if (ClsTiposDePagamento.critica != "")
        {
            Mensagem(ClsTiposDePagamento.critica.ToString());
        }
        lblGrid.Text = ClsTiposDePagamento.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_tppagto.Valor = "";
        this.txtqt_vezes.Valor = "0";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        TiposDePagamento ClsTiposDePagamento = new TiposDePagamento(Application["StrConexao"].ToString());

        ClsTiposDePagamento.CodigoDoTipoDePagamento = Convert.ToInt16(this.txtcd_tppagto.Text.ToString());

        resp = ClsTiposDePagamento.Excluir();
        //**********************
        txtcd_tppagto.Text = ClsTiposDePagamento.CodigoDoTipoDePagamento.ToString();
        txtnm_tppagto.Valor = ClsTiposDePagamento.NomeDoTipoDePagamento.Trim();
        txtqt_vezes.Valor = ClsTiposDePagamento.NumeroDeParcelas.ToString();
       
        if (ClsTiposDePagamento.critica != "")
        {
            Mensagem(ClsTiposDePagamento.critica.ToString());
        }
        lblGrid.Text = ClsTiposDePagamento.TrazGrid();

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
