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

public partial class retiradas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["bl_retirada"] == false)
        {
            Mensagem("Acesso não autorizado pelo Administrador.");
            this.retirada.Visible = false;
        }
        else
        {
            this.retirada.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());
            lblGrid.Text = ClsRetirada.TrazGrid();

            this.lblMsg.Text = "Retirada de Peças do Estoque.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());
        ClsRetirada.CarregaLista(this.ddlusuarios, "Usuario", (Request["ddlusuarios"] == null ? "0" : (string)Request["ddlusuarios"]), "Escolha um Usuário");
        ClsRetirada.CarregaLista(this.ddlclientes, "Cliente", (Request["ddlclientes"] == null ? "0" : (string)Request["ddlclientes"]), "Escolha um Cliente");
        ClsRetirada.CarregaLista(this.ddltppagtos, "Tppagto", (Request["ddltppagtos"] == null ? "0" : (string)Request["ddltppagtos"]), "Escolha um Tipo de Pagamento");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());

        ClsRetirada.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsRetirada.EstoqueNegativo = Convert.ToBoolean(Session["bl_estneg"]);
        ClsRetirada.CodigoDaRetirada = Convert.ToInt16(this.txtcd_retirada.Text.ToString());
        ClsRetirada.CodigoDoProduto = ClsRetirada.RetornaCodigo(this.txtcd_produto.Text);
        ClsRetirada.CodigoDoUsuario = Convert.ToInt16(this.ddlusuarios.SelectedValue);
        ClsRetirada.Quantidade = Convert.ToInt32(this.txtquantidade.Valor.ToString());
        ClsRetirada.Venda = Convert.ToInt16(this.chkvenda.Checked);
        ClsRetirada.CodigoDaVenda = Convert.ToInt32(this.txtcd_venda.Valor.ToString());
        ClsRetirada.CodigoDoCliente = Convert.ToInt16(this.ddlclientes.SelectedValue);
        ClsRetirada.CodigoDoTipoDePagamento = Convert.ToInt16(this.ddltppagtos.SelectedValue);
        ClsRetirada.NumeroDeParcelas = Convert.ToInt16(this.txtqt_vezes.Valor.ToString());
        ClsRetirada.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsRetirada.Desconto = Convert.ToDecimal(this.txtdesconto.Valor.Replace(".", ","));
        ClsRetirada.Acrescimo = Convert.ToDecimal(this.txtacrescimo.Valor.Replace(".", ","));
        
        
        resp = ClsRetirada.Atualizar();
        //**************************

        if (ClsRetirada.critica != "")
        {
            Mensagem(ClsRetirada.critica.ToString());
        }
        lblGrid.Text = ClsRetirada.TrazGrid();

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
        this.lblMsg.Text = "Retirada de Peças do Estoque.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_retirada.Text.Trim() != "")
        {
            if (this.txtcd_retirada.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());

        ClsRetirada.UsuarioLogado = Convert.ToInt32(Session["cd_user"].ToString());
        ClsRetirada.EstoqueNegativo = Convert.ToBoolean(Session["bl_estneg"]);
        ClsRetirada.CodigoDoProduto = ClsRetirada.RetornaCodigo(this.txtcd_produto.Text);
        ClsRetirada.CodigoDoUsuario = Convert.ToInt16(this.ddlusuarios.SelectedValue);
        ClsRetirada.Quantidade = Convert.ToInt32(this.txtquantidade.Valor.ToString());
        ClsRetirada.Venda = Convert.ToInt16(this.chkvenda.Checked);
        ClsRetirada.CodigoDaVenda = Convert.ToInt32(this.txtcd_venda.Valor.ToString());
        ClsRetirada.CodigoDoCliente = Convert.ToInt16(this.ddlclientes.SelectedValue);
        ClsRetirada.CodigoDoTipoDePagamento = Convert.ToInt16(this.ddltppagtos.SelectedValue);
        ClsRetirada.NumeroDeParcelas = Convert.ToInt16(this.txtqt_vezes.Valor.ToString());
        ClsRetirada.DataDeVencimento = this.txtdt_vencto.Valor.ToString().Trim();
        ClsRetirada.Desconto = Convert.ToDecimal(this.txtdesconto.Valor.Replace(".", ","));
        ClsRetirada.Acrescimo = Convert.ToDecimal(this.txtacrescimo.Valor.Replace(".", ","));
        
        resp = ClsRetirada.Grava();
        //*********************
        txtcd_retirada.Text = ClsRetirada.CodigoDaRetirada.ToString();

        if (ClsRetirada.critica != "")
        {
            Mensagem(ClsRetirada.critica.ToString());
        }
        lblGrid.Text = ClsRetirada.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsRetirada.CodigoDaRetirada = Convert.ToInt16(this.txtcd_retirada.Text.ToString());

        resp = ClsRetirada.Consulta();
        //************************
        txtcd_retirada.Text = ClsRetirada.CodigoDaRetirada.ToString();
        txtcd_produto.Text = ClsRetirada.CodigoDaPeca.ToString();
        ddlusuarios.SelectedValue = ClsRetirada.CodigoDoUsuario.ToString();
        txtquantidade.Valor = ClsRetirada.Quantidade.ToString();
        chkvenda.Checked = ClsRetirada.Venda == 1 ? true : false;
        txtcd_venda.Valor = ClsRetirada.CodigoDaVenda.ToString();
        ddlclientes.SelectedValue = ClsRetirada.CodigoDoCliente.ToString();
        ddltppagtos.SelectedValue = ClsRetirada.CodigoDoTipoDePagamento.ToString();
        txtqt_vezes.Valor = ClsRetirada.NumeroDeParcelas.ToString();
        txtdt_vencto.Valor = ClsRetirada.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtdesconto.Valor = ClsRetirada.Desconto.ToString();
        txtacrescimo.Valor = ClsRetirada.Acrescimo.ToString();
        

        if (ClsRetirada.critica != "")
        {
            Mensagem(ClsRetirada.critica.ToString());
        }
        lblGrid.Text = ClsRetirada.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());
        this.txtcd_produto.Text = "0";
        this.txtquantidade.Valor = "0";
        this.chkvenda.Checked = false;
        ddlclientes.SelectedValue = ClsRetirada.CodigoDoCliente.ToString();
        ddltppagtos.SelectedValue = ClsRetirada.CodigoDoTipoDePagamento.ToString();
        this.txtqt_vezes.Valor = "0";
        this.txtcd_venda.Valor = "0";
        this.txtdt_vencto.Valor = "";
        this.txtdesconto.Valor = "0";
        this.txtacrescimo.Valor = "0";
      

     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());

        ClsRetirada.CodigoDaRetirada = Convert.ToInt16(this.txtcd_retirada.Text.ToString());

        resp = ClsRetirada.Excluir();
        //**********************
        txtcd_retirada.Text = ClsRetirada.CodigoDaRetirada.ToString();
        txtcd_produto.Text = ClsRetirada.CodigoDaPeca.ToString();
        ddlusuarios.SelectedValue = ClsRetirada.CodigoDoUsuario.ToString();
        txtquantidade.Valor = ClsRetirada.Quantidade.ToString();
        chkvenda.Checked = ClsRetirada.Venda == 1 ? true : false;
        txtcd_venda.Valor = ClsRetirada.CodigoDaVenda.ToString();
        ddlclientes.SelectedValue = ClsRetirada.CodigoDoCliente.ToString();
        ddltppagtos.SelectedValue = ClsRetirada.CodigoDoTipoDePagamento.ToString();
        txtqt_vezes.Valor = ClsRetirada.NumeroDeParcelas.ToString();
        txtdt_vencto.Valor = ClsRetirada.DataDeVencimento.Replace("00:00:00", "").Trim();
        txtdesconto.Valor = ClsRetirada.Desconto.ToString();
        txtacrescimo.Valor = ClsRetirada.Acrescimo.ToString();

        if (ClsRetirada.critica != "")
        {
            Mensagem(ClsRetirada.critica.ToString());
        }
        lblGrid.Text = ClsRetirada.TrazGrid();

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
        Retirada ClsRetirada = new Retirada(Application["StrConexao"].ToString());

        resp = ClsRetirada.VerificaProduto(this.txtcd_produto.Text);
        ////************************

        if (ClsRetirada.critica != "")
        {
            Mensagem(ClsRetirada.critica.ToString());
            this.LimpaCampo();
        }

    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
