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
using System.Xml;
using System.IO;

public partial class carrinho : System.Web.UI.Page
{
   
    public string acao = ""; 
    public string produto = "";
    public string produtoescolhido = "";
    public bool semfrete = false;
    public decimal quantidade = 0;
    public decimal total = 0;
    public decimal totalpeso = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        
        bool PrimeiraVez = true;
     
        acao = Request["acao"];
        produto = Request["produto"];
        produtoescolhido = Request["produtoescolhido"];
        quantidade = Convert.ToDecimal(Request["quantidade"]);

        lblTitulo.Text = "Finalizar Compra";

      
        if ((bool)Session["carrinho"] == true)
        {
            lblPrincipal.Text = MontaGridCarrinho();
            lblcep.Text   = "CEP de Destino: " + Request["cep"];
            if (Request["frete"] == "EN")
            {
                lblfrete.Text = "Tipo de Frete: PAC";
            }
            else
            {
                if (Request["frete"] == "SD")
                {
                    lblfrete.Text = "Tipo de Frete: SEDEX";
                }
                else
                {
                    lblfrete.Text = "Tipo de Frete: Entrega via Motoboy";
                }
            }

            lblValor.Text = "Valor Total: R$ " + Request["valor"];
        }
        else
        {
            lblValor.Text = "Seu carrinho está vazio";
        }
      
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

    public string MontaGridCarrinho()
    {
        string StrTable = "";
        string CssUsado = "";
        StrTable += "<table class='TabelaCarrinho' border='1' cellspacing='0' cellpadding='0' width='700px' align='center'>";
        StrTable += "<tr  align='center'>";
        StrTable += "<td nowrap class='TituloCarrinho' >";
        StrTable += "<div align='center'><span align='right'>Foto</span></div>";
        StrTable += "</td>";
        StrTable += "<td nowrap class='TituloCarrinho' >";
        StrTable += "<div align='center'>Produto</div>";
        StrTable += "</td>";
        StrTable += "<td nowrap class='TituloCarrinho' >";
        StrTable += "<div align='center'>Descrição</div>";
        StrTable += "</td>";
        StrTable += "<td nowrap class='TituloCarrinho' >";
        StrTable += "<div align='center'>Quantidade</div>";
        StrTable += "</td>";
        StrTable += "<td nowrap class='TituloCarrinho' >";
        StrTable += "<div align='center'>Preço</div>";
        StrTable += "</td>";
        StrTable += "</tr>";
        for (int x = 1; x < 50; x++)
        {
             if (Session["cd_produto(" + x + ")"] != null)
             {
         
                if (Session["cd_produto(" + x + ")"] != "")
                {
                    StrTable += "<tr class='linha_grid_dois'>";
                    StrTable += "<input type='hidden' id='hidaux' runat='server' />";
                    StrTable += "<td width='77px' height='77px' class='CampoCarrinho'>";
                    StrTable += "<img src='../Images/produtos/1/" + Session["cd_produto(" + x + ")"] + ".jpg' width='75px' heigth='75px'></td>";
                    StrTable += "<td class='CampoCarrinho'>";
                    StrTable += Session["cd_produto(" + x + ")"];
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho'>";
                    StrTable += "<asp:label id='lblcd_" + x + "'> "+Session["nm_produto(" + x + ")"] + "</asp:label>";
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho'>";
                    StrTable += "<asp:label ID='lblqtde_" + x + "'> "+ Session["quantidade(" + x + ")"] + "</asp:label>";
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho'>R$ ";
                    StrTable += Session["preco(" + x + ")"];
                    StrTable += "</td>";
                    StrTable += "</tr>";
                }
            }
        }
        StrTable += "</table>";
        StrTable += "<br><br>";

       
        return StrTable.ToString().Trim();
        
    }

    public void GravaVenda(object sender, EventArgs e)
    {
        
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        int codigo = 0;
        AcessoDados ClsAcessoDados = new AcessoDados(Application["StrConexao"].ToString());

        ClsAcessoDados.CodigoDoCliente = Convert.ToInt32(Session["cd_cliente"].ToString());
        ClsAcessoDados.ValorTotal = Convert.ToDecimal(Request["valor"].ToString().Replace(".", ","));
        ClsAcessoDados.ValorFrete = Convert.ToDecimal(Session["vl_totfrete"].ToString().Replace(".", ","));
        if (Request["frete"] != null)
        {
            ClsAcessoDados.TipoFrete = Request["frete"].ToString().ToUpper();
        }

        ClsAcessoDados.GravaVenda();
        codigo = ClsAcessoDados.CodigoDaVenda;
        Session["codigovenda"] = ClsAcessoDados.CodigoDaVenda; ;

       
        for (int x = 1; x < 50; x++)
        {
            if (Session["cd_produto(" + x + ")"] != null)
            {
                if (Session["cd_produto(" + x + ")"] != "")
                {
                    ClsAcessoDados.CodigoDaVenda = codigo;
                    ClsAcessoDados.CodigoDoProduto = ClsAcessoDados.RetornaCodigo(Session["cd_produto(" + x + ")"].ToString());
                    ClsAcessoDados.Quantidade = Convert.ToInt32(Session["quantidade(" + x + ")"]);
                    ClsAcessoDados.ValorTotalProduto = Convert.ToDecimal(Session["preco(" + x + ")"].ToString().Replace(".", ","));

                    ClsAcessoDados.GravaProdutosVenda();
                }
            }

        }
        ClsAcessoDados.EnviaEmail(codigo);
        string url = "";
        url += "redireciona.aspx?acao=enviar&frete=" + Request["frete"] + "";

        string script = "<script type='text/javascript' language='javascript'>chama('" + url + "');</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "chamatela", script);

    }

    public void Redireciona(object sender, EventArgs e)
    {
        //Response.Redirect("principal.aspx");
        this.pagseg.Enabled = false;
    }
}
