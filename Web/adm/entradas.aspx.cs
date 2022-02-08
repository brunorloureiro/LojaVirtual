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

public partial class entradas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_entrada"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.entrada.Visible = false;
        }
        else
        {
            this.entrada.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());
            lblGrid.Text = ClsEntrada.TrazGrid();

            this.lblMsg.Text = "Entrada de Peças para o Estoque.";
        }
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }
    
    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());

        ClsEntrada.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsEntrada.CodigoDaEntrada = Convert.ToInt16(this.txtcd_entrada.Text.ToString());
        ClsEntrada.CodigoDoProduto = ClsEntrada.RetornaCodigo(this.txtcd_produto.Text);
        ClsEntrada.Quantidade = Convert.ToInt32(this.txtquantidade.Valor.ToString());
        
        resp = ClsEntrada.Atualizar();
        //**************************

        if (ClsEntrada.critica != "")
        {
            Mensagem(ClsEntrada.critica.ToString());
        }
        lblGrid.Text = ClsEntrada.TrazGrid();

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
        this.lblMsg.Text = "Entrada de Peças para o Estoque.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_entrada.Text.Trim() != "")
        {
            if (this.txtcd_entrada.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());

        ClsEntrada.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsEntrada.CodigoDoProduto = ClsEntrada.RetornaCodigo(this.txtcd_produto.Text);
        ClsEntrada.Quantidade = Convert.ToInt32(this.txtquantidade.Valor.ToString());
        
        resp = ClsEntrada.Grava();
        //*********************
        txtcd_entrada.Text = ClsEntrada.CodigoDaEntrada.ToString();

        if (ClsEntrada.critica != "")
        {
            Mensagem(ClsEntrada.critica.ToString());
        }
        lblGrid.Text = ClsEntrada.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsEntrada.CodigoDaEntrada = Convert.ToInt16(this.txtcd_entrada.Text.ToString());

        resp = ClsEntrada.Consulta();
        //************************
        txtcd_entrada.Text = ClsEntrada.CodigoDaEntrada.ToString();
        txtcd_produto.Text = ClsEntrada.CodigoDaPeca.ToString();
        txtquantidade.Valor = ClsEntrada.Quantidade.ToString();

        if (ClsEntrada.critica != "")
        {
            Mensagem(ClsEntrada.critica.ToString());
        }
        lblGrid.Text = ClsEntrada.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());
        this.txtcd_produto.Text = "0";
        this.txtquantidade.Valor = "0";
    }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());

        ClsEntrada.CodigoDaEntrada = Convert.ToInt16(this.txtcd_entrada.Text.ToString());

        resp = ClsEntrada.Excluir();
        //**********************
        txtcd_entrada.Text = ClsEntrada.CodigoDaEntrada.ToString();
        txtcd_produto.Text = ClsEntrada.CodigoDaPeca.ToString();
        txtquantidade.Valor = ClsEntrada.Quantidade.ToString();

        if (ClsEntrada.critica != "")
        {
            Mensagem(ClsEntrada.critica.ToString());
        }
        lblGrid.Text = ClsEntrada.TrazGrid();

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
        Entrada ClsEntrada = new Entrada(Application["StrConexao"].ToString());

        resp = ClsEntrada.VerificaProduto(this.txtcd_produto.Text);
        ////************************

        if (ClsEntrada.critica != "")
        {
            Mensagem(ClsEntrada.critica.ToString());
            this.LimpaCampo();
        }
       
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
