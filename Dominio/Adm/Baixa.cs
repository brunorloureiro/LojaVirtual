using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;

public class Baixa
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaBaixa = 0;
    public int CodigoDoProduto = 0;
    public int Quantidade = 0;
    public int QuantidadeEstoque = 0;
    public int Venda = 0;
    public int CodigoDoCliente = 0;
    public int CodigoDoTipoDePagamento = 0;
    public int NumeroDeParcelas = 0;
    public int CodigoDaVenda = 0;
    public string DataDeVencimento = "";
    public decimal Desconto = 0;
    public decimal Acrescimo = 0;
    public decimal ValorTotal = 0;
    public int UsuarioLogado;
    public bool EstoqueNegativo;
    public bool LimparCampos = false;
   
    public Baixa(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Produto";
        string campos = "nm_produto,qt_estoque";
        string labels = "Produto,Quantidade em Estoque";
        string pks = "txtcd_produto";
        string cond  = "";
        bool mostracheck = false;
       
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, mostracheck);
    }

    public void CarregaLista(object DDL, string Tabela, string Valor, string Msg)
    {
        ClsPublico.carregaLista(DDL, Tabela, Valor, Msg);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.CodigoDoProduto == 0)
        {
            this.critica = "Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.Quantidade == 0)
        {
            this.critica = "Quantidade deve ser informada. Verifique.";
            return false;
        }

        if (this.Venda != 0)
        {
            if (this.CodigoDaVenda == 0)
            {
                this.critica = "Código da Venda deve ser informado. Verifique.";
                return false;
            }

            if (this.CodigoDoCliente == 0)
            {
                this.critica = "Cliente deve ser informado. Verifique.";
                return false;
            }

            if (this.CodigoDoTipoDePagamento == 0)
            {
                this.critica = "Tipo de Pagamento deve ser informado. Verifique.";
                return false;
            }
            else
            {
                if (this.NumeroDeParcelas == 0)
                {
                    this.critica = "Número de Parcelas deve ser informado. Verifique.";
                    return false;
                }
                else
                {
                    if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length == 0)
                    {
                        this.critica = "Data de Vencimento deve ser preenchida. Se por acaso a venda for a vista, preencha com a Data Atual.";
                        return false;
                    }
                }
            }
            
            if (this.Desconto != 0)
            {
                if (this.Acrescimo != 0)
                {
                    this.critica = "Favor inserir ou Desconto ou Acréscimo para a Venda. Verifique.";
                    return false;
                }
                else
                {
                    if (this.Desconto > 100)
                    {
                        this.critica = "Desconto não pode ser maior do que 100%. Verifique.";
                        return false;
                    }
                }
            }
        }
        else 
        {
            if (this.CodigoDaVenda != 0)
            {
                this.critica = "Código da Venda só deve ser informado se a Baixa for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoCliente != 0)
            {
                this.critica = "Cliente só deve ser informado se a Baixa for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoTipoDePagamento != 0)
            {
                this.critica = "Tipo de Pagamento só deve ser informado se a Baixa for registro de Venda. Verifique.";
                return false;
            }
            else
            {
                if (this.NumeroDeParcelas != 0)
                {
                    this.critica = "Número de Parcelas só deve ser informado se a Baixa for registro de Venda. Verifique.";
                    return false;
                }
                else
                {
                    if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length != 0)
                    {
                        this.critica = "Data de Vencimento só deve ser informada se a Baixa for registro de Venda. Verifique.";
                        return false;
                    }
                }
            
            }

            if (this.Desconto != 0)
            {
                this.critica = "Desconto só deve ser informado se a Baixa for registro de Venda. Verifique.";
                return false;
            }

            if (this.Acrescimo != 0)
            {
                this.critica = "Desconto só deve ser informado se a Baixa for registro de Venda. Verifique.";
                return false;
            }
        }
        

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT qt_vezes FROM Tppagto where cd_tppagto = " + this.CodigoDoTipoDePagamento.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                if (this.NumeroDeParcelas > Convert.ToInt32(oDr["qt_vezes"]))
                {
                    //**********
                    oDr.Close();
                    //**********
                    this.critica = "Número de Parcelas maior do que o permitido. Verifique.";
                    return false;
                }
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " SELECT preco FROM Produto where cd_produto = " + this.CodigoDoProduto.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                this.ValorTotal = Convert.ToDecimal(oDr["preco"].ToString().Replace(".", ","));
                if (this.Acrescimo != 0)
                {
                    this.ValorTotal = this.ValorTotal + (this.ValorTotal * (this.Acrescimo / 100));
                }
                else
                {
                    if (this.Desconto != 0)
                    {
                        this.ValorTotal = this.ValorTotal - (this.ValorTotal * (this.Desconto / 100));
                    }
                }
                //**********
                oDr.Close();
                //**********
            }
            else
            {
                //**********
                oDr.Close();
                //**********
            }

            if (!this.EstoqueNegativo)
            {

                StrSql = "          SELECT  qt_estoque ";
                StrSql = StrSql + " FROM    Produto   ";
                StrSql = StrSql + " WHERE   Produto.cd_produto = " + this.CodigoDoProduto.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    int qt_estoque = 0;
                    qt_estoque = (Convert.ToInt32(oDr["qt_estoque"]) - this.Quantidade);
                    if (qt_estoque < 0)
                    {
                        this.critica = "Com essa Baixa o estoque ficará negativo. Operação não permitida.";
                        return false;
                    }
                }
                oDr.Close();
            }

            if (this.Venda == 0)
            {
                StrSql = " INSERT INTO Baixa (cd_produto, quantidade, bl_venda, cd_usu_log, dt_baixa) ";
                StrSql += " VALUES (" + this.CodigoDoProduto + ",";
                StrSql += "         " + this.Quantidade + ",";
                StrSql += "         " + this.Venda + ",";
                StrSql += "         " + this.UsuarioLogado + ",";
                StrSql += "         '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "')";
            }
            else 
            {
                StrSql = " INSERT INTO Baixa (cd_produto, quantidade, bl_venda, cd_venda, cd_cliente, cd_tppagto, qt_vezes, dt_vencto, desconto, acrescimo, vl_total, cd_usu_log, dt_baixa) ";
                StrSql += " VALUES (" + this.CodigoDoProduto + ",";
                StrSql += "         " + this.Quantidade + ",";
                StrSql += "         " + this.Venda + ",";
                StrSql += "         " + this.CodigoDaVenda + ",";
                StrSql += "         " + this.CodigoDoCliente + ",";
                StrSql += "         " + this.CodigoDoTipoDePagamento + ",";
                StrSql += "         " + this.NumeroDeParcelas + ",";
                StrSql += "        '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd") + "',";
                StrSql += "         " + this.Desconto.ToString().Replace(",", ".") + ",";
                StrSql += "         " + this.Acrescimo.ToString().Replace(",", ".") + ",";
                StrSql += "         " + this.ValorTotal.ToString().Replace(",", ".") + ",";
                StrSql += "         " + this.UsuarioLogado + ",";
                StrSql += "         '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "')";
           }


            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_baixa) as cd_baixa FROM Baixa ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaBaixa = Convert.ToInt32(oDr["cd_baixa"]);
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  qt_estoque ";
            StrSql = StrSql + " FROM    Produto   ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = " + this.CodigoDoProduto.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                this.QuantidadeEstoque = (Convert.ToInt32(oDr["qt_estoque"]) - this.Quantidade);
            }
            oDr.Close();


            StrSql = " UPDATE   Produto Set ";
            StrSql += "         qt_estoque   =  " + this.QuantidadeEstoque.ToString();
            StrSql += " WHERE   cd_produto   =  " + this.CodigoDoProduto.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            this.critica = "Registro salvo com sucesso.";
            this.LimparCampos = true;
            
            Resp = true;
        }
        catch (Exception Err)
        {
            this.critica = Err.Message.ToString();
            Resp = false;
        }

        //**************************************************************************************
        if (!ClsPublico.FechaConexao()) { this.critica = ClsPublico.critica; return false; }
        //**************************************************************************************

        return Resp;
        //**********
    }
 }
