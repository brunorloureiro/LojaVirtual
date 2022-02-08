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

public partial class principal : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());

        if ((bool)Session["bl_consulta"] == true)
        {
            lblGrid.Text = ClsPrincipal.TrazGrid();
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

        this.lblMsg.Text = "Definição de Produtos para a Página Principal da Loja Virtual.";
    }

    

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());
        ClsPrincipal.CodigoPrincipal = Convert.ToInt16(this.txtcd_principal.Text.ToString());
        ClsPrincipal.CodigoDoProduto = ClsPrincipal.RetornaCodigo(this.txtcd_produto.Text);
        ClsPrincipal.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsPrincipal.Atualizar();
        //**************************

        if (ClsPrincipal.critica != "")
        {
            Mensagem(ClsPrincipal.critica.ToString());
        }
        lblGrid.Text = ClsPrincipal.TrazGrid();

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
        this.lblMsg.Text = "Definição de Produtos para a Página Principal da Loja Virtual.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_principal.Text.Trim() != "")
        {
            if (this.txtcd_principal.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());

        ClsPrincipal.CodigoDoProduto = ClsPrincipal.RetornaCodigo(this.txtcd_produto.Text);
        ClsPrincipal.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsPrincipal.Grava();
        //*********************
        txtcd_principal.Text = ClsPrincipal.CodigoPrincipal.ToString();

        if (ClsPrincipal.critica != "")
        {
            Mensagem(ClsPrincipal.critica.ToString());
        }
        lblGrid.Text = ClsPrincipal.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsPrincipal.CodigoPrincipal = Convert.ToInt16(this.txtcd_principal.Text.ToString());

        resp = ClsPrincipal.Consulta();
        //************************
        txtcd_principal.Text = ClsPrincipal.CodigoPrincipal.ToString();
        txtcd_produto.Text = ClsPrincipal.CodigoDaPeca.ToString();
        chkativo.Checked = ClsPrincipal.Ativo == 1 ? true : false; 

        if (ClsPrincipal.critica != "")
        {
            Mensagem(ClsPrincipal.critica.ToString());
        }
        lblGrid.Text = ClsPrincipal.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());
        this.txtcd_produto.Text = "0";
        this.chkativo.Checked = false;
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());

        ClsPrincipal.CodigoPrincipal = Convert.ToInt16(this.txtcd_principal.Text.ToString());

        resp = ClsPrincipal.Excluir();
        //**********************
        txtcd_principal.Text = ClsPrincipal.CodigoPrincipal.ToString();
        txtcd_produto.Text = ClsPrincipal.CodigoDaPeca.ToString();
        chkativo.Checked = ClsPrincipal.Ativo == 1 ? true : false; 

        if (ClsPrincipal.critica != "")
        {
            Mensagem(ClsPrincipal.critica.ToString());
        }
        lblGrid.Text = ClsPrincipal.TrazGrid();

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
        Principal ClsPrincipal = new Principal(Application["StrConexao"].ToString());

        resp = ClsPrincipal.VerificaProduto(this.txtcd_produto.Text);
        ////************************

        if (ClsPrincipal.critica != "")
        {
            Mensagem(ClsPrincipal.critica.ToString());
            this.LimpaCampo();
        }

    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
