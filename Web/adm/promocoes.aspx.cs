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

public partial class promocoes : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsPromocao.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de promoções para a Loja Virtual.";
    }

    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());
        ClsPromocao.CodigoDaPromocao = Convert.ToInt32(this.txtcd_promocao.Text.ToString());
        ClsPromocao.NomeDaPromocao = this.txtnm_promocao.Valor.ToString().Trim();
        ClsPromocao.CodigoDoProduto = ClsPromocao.RetornaCodigo(this.txtcd_produto.Text);
        ClsPromocao.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsPromocao.Atualizar();
        //**************************

        if (ClsPromocao.critica != "")
        {
            Mensagem(ClsPromocao.critica.ToString());
        }
        lblGrid.Text = ClsPromocao.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de promoções para a Loja Virtual.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_promocao.Text.Trim() != "")
        {
            if (this.txtcd_promocao.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());

        ClsPromocao.NomeDaPromocao = this.txtnm_promocao.Valor.ToString().Trim();
        ClsPromocao.CodigoDoProduto = ClsPromocao.RetornaCodigo(this.txtcd_produto.Text);
        ClsPromocao.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsPromocao.Grava();
        //*********************
        txtcd_promocao.Text = ClsPromocao.CodigoDaPromocao.ToString();

        if (ClsPromocao.critica != "")
        {
            Mensagem(ClsPromocao.critica.ToString());
        }
        lblGrid.Text = ClsPromocao.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsPromocao.CodigoDaPromocao = Convert.ToInt32(this.txtcd_promocao.Text.ToString());

        resp = ClsPromocao.Consulta();
        //************************
        txtcd_promocao.Text = ClsPromocao.CodigoDaPromocao.ToString();
        txtnm_promocao.Valor = ClsPromocao.NomeDaPromocao.Trim();
        txtcd_produto.Text = ClsPromocao.CodigoDaPeca.ToString();
        chkativo.Checked = ClsPromocao.Ativo == 1 ? true : false; 

        if (ClsPromocao.critica != "")
        {
            Mensagem(ClsPromocao.critica.ToString());
        }
        lblGrid.Text = ClsPromocao.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());
        this.txtnm_promocao.Valor = "";
        this.txtcd_produto.Text = "0";
        this.chkativo.Checked = false;
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());

        ClsPromocao.CodigoDaPromocao = Convert.ToInt32(this.txtcd_promocao.Text.ToString());

        resp = ClsPromocao.Excluir();
        //**********************
        txtcd_promocao.Text = ClsPromocao.CodigoDaPromocao.ToString();
        txtnm_promocao.Valor = ClsPromocao.NomeDaPromocao.Trim();
        txtcd_produto.Text = ClsPromocao.CodigoDaPeca.ToString();
        chkativo.Checked = ClsPromocao.Ativo == 1 ? true : false; 

        if (ClsPromocao.critica != "")
        {
            Mensagem(ClsPromocao.critica.ToString());
        }
        lblGrid.Text = ClsPromocao.TrazGrid();

        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.LimpaCampo();
    }

    public void validaproduto(object sender, EventArgs e)
    {
        if (this.txtcd_produto.Text.Trim() == "0" || this.txtcd_produto.Text.Trim() == "")
        {
            Mensagem("Código do Produto deve ser informado. Verifique.");
            return;
        }

        bool resp;
        Promocao ClsPromocao = new Promocao(Application["StrConexao"].ToString());

        resp = ClsPromocao.VerificaProduto(this.txtcd_produto.Text);
        ////************************

        if (ClsPromocao.critica != "")
        {
            Mensagem(ClsPromocao.critica.ToString());
            this.LimpaCampo();
        }

    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
