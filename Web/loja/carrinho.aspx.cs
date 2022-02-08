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
    public int totprod = 0;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        
        bool PrimeiraVez = true;
        acao = Request["acao"];
        produto = Request["produto"];
        produtoescolhido = Request["produtoescolhido"];
        quantidade = Convert.ToDecimal(Request["quantidade"]);

        # region Métodos para Preencher o Carrinho

        if (acao == "inserir")
        {
            acao = "";
            PrimeiraVez = true;
            for (int x = 1; x < 50; x++)
            {
                if (PrimeiraVez)
                {
                    if (Session["cd_produto(" + x + ")"] != null)
                    {

                        if (Session["cd_produto(" + x + ")"].ToString() == "")
                        {

                            Session["cd_produto(" + x + ")"] = ClsLoja.TrazInfoCarrinho(produto, "produto");
                            Session["nm_produto(" + x + ")"] = ClsLoja.TrazInfoCarrinho(produto, "nome");
                            Session["preco(" + x + ")"] = ClsLoja.TrazInfoCarrinho(produto, "preco");
                            Session["peso(" + x + ")"] = ClsLoja.TrazInfoCarrinho(produto, "peso");
                            Session["quantidade(" + x + ")"] = 1;
                            if (Session["cd_produto(" + x + ")"] != "")
                            {
                                PrimeiraVez = false;
                                Session["carrinho"] = true;
                            }
                        }
                        else
                        {
                            if (Session["cd_produto(" + x + ")"].ToString() == produto)
                            {
                                PrimeiraVez = false;
                                Session["carrinho"] = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (acao == "apagar")
            {
                acao = "";
                for (int x = 1; x < 50; x++)
                {
                    if (Session["cd_produto(" + x + ")"].ToString() == produtoescolhido)
                    {
                         Session["cd_produto(" + x + ")"] = "";
                         Session["nm_produto(" + x + ")"] = "";
                         Session["preco(" + x + ")"] = "";
                         Session["peso(" + x + ")"] = "";
                         Session["quantidade(" + x + ")"] = 0;
                    }
                }
            }
            else
            {
                if (acao == "alterar")
                {
                    acao = "";
                    for (int x = 1; x < 50; x++)
                    {
                        if (Session["cd_produto(" + x + ")"].ToString() == produtoescolhido)
                        {
                            if (quantidade == 0)
                            {
                                Session["cd_produto(" + x + ")"] = "";
                                Session["nm_produto(" + x + ")"] = "";
                                Session["preco(" + x + ")"] = "";
                                Session["peso(" + x + ")"] = "";
                                Session["quantidade(" + x + ")"] = 0;
                            }
                            else
                            {
                                if (Convert.ToInt32(ClsLoja.TrazInfoCarrinho(produtoescolhido, "qt_estoque")) - quantidade >= 0)
                                {
                                    Session["quantidade(" + x + ")"] = quantidade;
                                    Session["preco(" + x + ")"] = Math.Round(Convert.ToDecimal(ClsLoja.TrazInfoCarrinho(produtoescolhido, "preco").Replace(".", ",")) * quantidade, 2);
                                    Session["peso(" + x + ")"] = Math.Round(Convert.ToDecimal(ClsLoja.TrazInfoCarrinho(produtoescolhido, "peso").Replace(".", ",")) * quantidade, 2);
                                }
                                else
                                {
                                    Mensagem("Quantidade não disponível no Estoque.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (acao == "limparcarrinho")
                    {
                        acao = "";
                        for (int x = 1; x < 50; x++)
                        {
                            Session["cd_produto(" + x + ")"] = "";
                            Session["nm_produto(" + x + ")"] = "";
                            Session["preco(" + x + ")"] = "";
                            Session["peso(" + x + ")"] = "";
                            Session["quantidade(" + x + ")"] = 0;
                        }
                    }
                }
            }
        }

        PrimeiraVez = true;
        for (int x = 1; x < 50; x++)
        {
            if (PrimeiraVez)
            {
                if (Session["cd_produto(" + x + ")"] != "")
                {
                    PrimeiraVez = false;
                    Session["carrinho"] = true;
                }
                else
                {
                    Session["carrinho"] = false;
                }
            }
            if (Session["preco(" + x + ")"] != "")
            {
                total += Convert.ToDecimal(Session["preco(" + x + ")"].ToString().Replace(".", ","));
            }
            if (Session["peso(" + x + ")"] != "")
            {
                if (ClsLoja.TrazInfoCarrinho(Session["cd_produto(" + x + ")"].ToString(), "frete") == "0")
                {
                    totalpeso += Convert.ToDecimal(Session["peso(" + x + ")"].ToString().Replace(".", ","));
                    semfrete = false;
                }
                else
                {
                   semfrete = true;
                }
            }
            //else
            //{
            //    if (totalpeso == 0)
            //    {
            //        semfrete = true;
            //    }
            //}
        }
        //Mensagem(totalpeso.ToString());
        

        # endregion
       
        lblTitulo.Text = "Meu Carrinho";
      
        if ((bool)Session["carrinho"] == true)
        {
            lblPrincipal.Text = MontaGridCarrinho();
            hidvalor.Value = total.ToString();
            lblValor.Text = "Valor Total: R$ " + hidvalor.Value;
        }
        else
        {
            lblValor.Text = "Seu carrinho está vazio";
            this.btn_calc.Style.Add("display", "none");
            this.btn_compra.Style.Add("display","none");
            this.btn_limpa.Style.Add("display", "none");
            this.rad_pac.Style.Add("display", "none");
            this.rad_sedex.Style.Add("display", "none");
            this.rad_moto.Style.Add("display", "none");
            this.lblcep.Style.Add("display", "none");
            this.txtcep.Style.Add("display", "none");
         
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

    protected void Calcula(object sender, EventArgs e)
    {
        if (txtcep.Valor == "0" || txtcep.Valor == "")
        {
            Mensagem("CEP deve ser informado. Verifique.");
            return;
        }
        else
        {
            if (totalpeso != 0)
            {
                ConsultaFrete();
                divcidade.Visible = true;
            }
            else
            {
                TrazValorMotoBoy();
                rad_pac.Text = "Valor por PAC: GRÁTIS ";
                rad_pac.Checked = true;
                rad_sedex.Visible = false;
                hidsedex.Value = "0";
                divcidade.Visible = false;
            }
                     
        }
        divfrete.Visible = true; 
    }

    protected void CalculaValor(object sender, EventArgs e)
    {
        if (rad_pac.Checked)
        {
            hidvalor.Value = (total + Convert.ToDecimal(hidpac.Value.Replace(".", ","))).ToString();
            lblValor.Text = "Valor Total: R$ " + hidvalor.Value;
        }
        else
        {
            if (rad_sedex.Checked)
            {
                hidvalor.Value = (total + Convert.ToDecimal(hidsedex.Value.Replace(".", ","))).ToString();
                lblValor.Text = "Valor Total: R$ " + hidvalor.Value;
            }
            else
            {
                hidvalor.Value = (total + Convert.ToDecimal(hidmoto.Value.Replace(".", ","))).ToString();
                lblValor.Text = "Valor Total: R$ " + hidvalor.Value;
            }
        }
    }

    protected void Compra(object sender, EventArgs e)
    {
        Session["venda"] = true;

        string url = "";
        string frete = "";
        bool comprar = true;

        if (txtcep.Valor.ToString() == "")
        {
            Mensagem("CEP deve ser preenchido.");
            comprar = false;
        }
        
        if (!hidpac.Visible)
        {
            Mensagem("CEP deve ser calculado para a escolha do tipo de Frete.");
            comprar = false;
        }

        if (rad_pac.Checked)
        {
            hidvalor.Value = (total + Convert.ToDecimal(hidpac.Value.Replace(".", ","))).ToString();
            Session["vl_totfrete"] = (Math.Round(Convert.ToDecimal(hidpac.Value.Replace(".", ",")),2)).ToString();
            frete = "EN";
        }
        else
        {
            if (rad_sedex.Checked)
            {
                hidvalor.Value = (total + Convert.ToDecimal(hidsedex.Value.Replace(".", ","))).ToString();
                Session["vl_totfrete"] = (Math.Round(Convert.ToDecimal(hidsedex.Value.Replace(".", ",")), 2)).ToString();
                frete = "SD";
            }
            else
            {
               hidvalor.Value = (total + Convert.ToDecimal(hidmoto.Value.Replace(".", ","))).ToString();
               Session["vl_totfrete"] = (Math.Round(Convert.ToDecimal(hidmoto.Value.Replace(".", ",")), 2)).ToString();
               frete = "MB";
            }
        }

        for (int x = 1; x < 50; x++)
        {
            if (Session["cd_produto(" + x + ")"] != "")
            {
                totprod += 1;
            }
        }
        Session["total_prod"] = totprod;

        for (int x = 1; x < 50; x++)
        {
            if (Session["cd_produto(" + x + ")"] != "")
            {
                Session["vl_frete(" + x + ")"] = Math.Round(Convert.ToDecimal(Session["vl_totfrete"].ToString().Replace(".", ",")) / totprod,2);
            }
        }

       if (comprar) 
        {
            if (Session["clientelogado"] != null)
            {
                if ((bool)Session["clientelogado"])
                {
                    url += "venda.aspx?acao=fechar&cep=" + txtcep.Valor.ToString() + "&valor=" + hidvalor.Value.ToString() + "&frete=" + frete;
                    Response.Redirect(url);
                }
            }
            Response.Redirect("../cliente/login.aspx?acao=compra");
        }
    }

    protected void Limpa(object sender, EventArgs e)
    {
        for (int x = 1; x < 50; x++)
        {
            Session["cd_produto(" + x + ")"] = "";
            Session["nm_produto(" + x + ")"] = "";
            Session["preco(" + x + ")"] = "";
            Session["peso(" + x + ")"] = "";
            Session["quantidade(" + x + ")"] = 0;
        }
    }

    public void ConsultaFrete()
    {
        TrazValorMotoBoy(); 

        //string urlSite = string.Format(
        //    @"http://frete.w21studio.com/calFrete.xml?cep=" + txtcep.Valor.ToString().Replace(".", "").Replace("-", "") + "&cod=2091&peso=" + totalpeso.ToString().Replace(",",".") + "&comprimento=60&largura=60&altura=5&servico=3");

        string urlSite = string.Format(
            @"http://frete.valuehost.com.br/?codigo=70&peso=" + totalpeso.ToString().Replace(",", ".") + "&cep_destino="+ txtcep.Valor.ToString().Replace(".", "").Replace("-", "") + "");

        string urlCep = string.Format(
            @"http://www.buscarcep.com.br/?cep=" + txtcep.Valor.ToString().Replace(".", "").Replace("-", "") + "&formato=xml");
        
        XmlTextReader lerXML = new XmlTextReader(urlSite);

        string sNode;
        string sValue;
    
        lerXML.MoveToContent();

        do
        {
            sNode = lerXML.Name;
            if (lerXML.NodeType == XmlNodeType.Element)
            {
                lerXML.Read();
                sValue = lerXML.Value;
               
                switch (sNode)
                {
                    case "valor_sedex":
                        rad_sedex.Text = "Valor por SEDEX: R$ " + sValue;
                        hidsedex.Value = sValue;
                        break;

                   case "valor_pac":
                        rad_pac.Text = "Valor por PAC: R$ " + sValue;
                        hidpac.Value = sValue;
                        hidvalor.Value = (total + Convert.ToDecimal(hidpac.Value.Replace(".", ","))).ToString();
                        lblValor.Text = "Valor Total: R$ " + hidvalor.Value;
                        rad_pac.Checked = true;
                        if (semfrete)
                        {
                            rad_sedex.Visible = false;
                        }
                        else
                        {
                            rad_sedex.Visible = true;
                            rad_sedex.Checked = false;
                        }
                        
                        rad_moto.Checked = false;
                        break;
                    
                }

            }
        } while (lerXML.Read());

        XmlTextReader lerXML2 = new XmlTextReader(urlCep);


        lerXML2.MoveToContent();

        do
        {
            sNode = lerXML2.Name;
            if (lerXML2.NodeType == XmlNodeType.Element)
            {
                lerXML2.Read();
                sValue = lerXML2.Value;

                switch (sNode)
                {
                    case "bairro":
                        lblBairroFrete.Text = sValue;
                        break;
                    case "cidade":
                        lblCidadeFrete.Text = sValue;
                        break;
                    case "uf":
                        lblUfFrete.Text = sValue;
                        break;
                }

            }
        } while (lerXML2.Read());

    }

    public string MontaGridCarrinho()
    {
        string StrTable = "";
        string StrHidden = "";
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
        StrTable += "<td nowrap class='TituloCarrinho'  colspan=2>";
        StrTable += "<div align='center'>Ações</div>";
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
                    StrTable += "<input type='text' runat='server' ID='txtqtde_" + x + "' style='width:20px' value='" + Session["quantidade(" + x + ")"] + "' >";
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho'>R$ ";
                    StrTable += Session["preco(" + x + ")"];
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho'>";
                    StrTable += "<input type='button' Class='BtnNormal' value='Alterar' onclick='javascript:window.location.href = " + '"' + "carrinho.aspx?acao=alterar&produtoescolhido=" + Session["cd_produto(" + x + ")"] + "&quantidade=" + '"' + "+txtqtde_" + x + ".value;'>";
                    StrTable += "</td>";
                    StrTable += "<td class='CampoCarrinho' >";
                    StrTable += "<input type='button' Class='BtnNormal' value='Excluir' onclick='javascript:window.location.href = " + '"' + "carrinho.aspx?acao=apagar&produtoescolhido=" + Session["cd_produto(" + x + ")"] + '"' + ";'>";
                    StrTable += "</td>";
                    StrTable += "</tr>";
                }
            }
        }
        StrTable += "</table>";
        StrTable += "<br><br>";

        for (int x = 1; x < 50; x++)
        {
            if (Session["cd_produto(" + x + ")"] != null)
            {
                if (Session["cd_produto(" + x + ")"] != "")
                {
                    StrHidden += "<input type='hidden' name='item_id_" + x + "' value='" + Session["cd_produto(" + x + ")"] + "' />";
                    StrHidden += "<input type='hidden' name='item_descr_" + x + "' value='" + Session["nm_produto(" + x + ")"] + "' />";
                    StrHidden += "<input type='hidden' name='item_quant_" + x + "' value='" + Session["quantidade(" + x + ")"] + "' />";
                    StrHidden += "<input type='hidden' name='item_valor_" + x + "' value='" + Session["preco(" + x + ")"].ToString().Replace(",", "").Replace(".", "") + "0' />";
                }
            }
        }

        //this.lblhiddens.Text = StrHidden.ToString().Trim();

        
        return StrTable.ToString().Trim();

       
    }

    public void TrazValorMotoBoy()
    {
        rad_moto.Visible = false;
        rad_moto.Text = "";
        hidmoto.Value = "0";
     
        string urlSite = string.Format(
                       @"http://www.buscarcep.com.br/?cep="+txtcep.Valor.ToString().Replace(".", "").Replace("-", "")+"&formato=xml");
        
        XmlTextReader lerXML = new XmlTextReader(urlSite);

        string sNode;
        string sValue;
        string cidade = "";

        lerXML.MoveToContent();

        do
        {
            sNode = lerXML.Name;
            if (lerXML.NodeType == XmlNodeType.Element)
            {
                lerXML.Read();
                sValue = lerXML.Value;

                switch (sNode)
                {
                   
                    case "cidade":
                        cidade = sValue;
                        break;

                    case "bairro":
                        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
                        if (ClsLoja.TrazValorMotoboy(cidade, sValue) != "0")
                        {
                            rad_moto.Visible = true;
                            rad_moto.Checked = false;
                            if (totalpeso == 0)
                            {
                                rad_moto.Text = "Entrega via Motoboy: GRÁTIS";
                                hidmoto.Value = "0";
                            }
                            else
                            {
                                rad_moto.Text = "Entrega via Motoboy: R$ " + ClsLoja.TrazValorMotoboy(cidade, sValue);
                                hidmoto.Value = ClsLoja.TrazValorMotoboy(cidade, sValue);
                            }
          
                            
                            hidvalor.Value = (total + Convert.ToDecimal(hidmoto.Value.Replace(".", ","))).ToString();
                         }
                        else
                        {
                            rad_moto.Visible = false;
                            rad_moto.Text = "";
                            hidmoto.Value = "0";
                        }
                       
                        break;
                }

            }
        } while (lerXML.Read());

    }

}
