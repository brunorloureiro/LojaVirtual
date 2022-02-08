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

public class Retirada
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaRetirada = 0;
    public int CodigoDoProduto = 0;
    public int CodigoDoUsuario = 0;
    public int Quantidade = 0;
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
   
    public Retirada(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Retirada, Produto, Usuario ";
        string campos = "cd_retirada,nm_produto,nm_usuario,quantidade";
        string labels = "Código,Produto,Usuário,Quantidade";
        string pks = "txtcd_retirada";
        string cond  = " AND Retirada.cd_produto = Produto.cd_produto";
               cond += " AND Retirada.cd_usuario = Usuario.cd_usuario";
               cond += " AND Retirada.bl_baixa = 0";
       
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
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

        if (this.CodigoDoUsuario == 0)
        {
            this.critica = "Usuário deve ser informado. Verifique.";
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
                this.critica = "Código da Venda só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoCliente != 0)
            {
                this.critica = "Cliente só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoTipoDePagamento != 0)
            {
                this.critica = "Tipo de Pagamento só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }
            else
            {
                if (this.NumeroDeParcelas != 0)
                {
                    this.critica = "Número de Parcelas só deve ser informado se a Retirada for registro de Venda. Verifique.";
                    return false;
                }
                else
                {
                    if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length != 0)
                    {
                        this.critica = "Data de Vencimento só deve ser informada se a Retirada for registro de Venda. Verifique.";
                        return false;
                    }
                }
            }

            if (this.Desconto != 0)
            {
                this.critica = "Desconto só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.Acrescimo != 0)
            {
                this.critica = "Desconto só deve ser informado se a Retirada for registro de Venda. Verifique.";
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
                
                StrSql = "          SELECT  quantidade ";
                StrSql = StrSql + " FROM    Retirada   ";
                StrSql = StrSql + " WHERE   Retirada.cd_produto = " + this.CodigoDoProduto.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                
                int qt_est_aux = 0;
                   
                while (oDr.Read())
                {
                    qt_est_aux = qt_est_aux + Convert.ToInt32(oDr["quantidade"]);
                }
                oDr.Close();


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
                    qt_estoque = ((Convert.ToInt32(oDr["qt_estoque"]) - qt_est_aux) - this.Quantidade);
                    if (qt_estoque < 0)
                    {
                        this.critica = "Com essa Retirada o estoque ficará negativo. Operação não permitida.";
                        return false;
                    }
                }
                oDr.Close();
            }

            if (this.Venda == 0)
            {
                StrSql = " INSERT INTO Retirada (cd_produto, cd_usuario, quantidade, bl_venda, cd_venda, cd_usu_log, dt_retirada) ";
                StrSql += " VALUES (" + this.CodigoDoProduto + ",";
                StrSql += "         " + this.CodigoDoUsuario + ",";
                StrSql += "         " + this.Quantidade + ",";
                StrSql += "         " + this.Venda + ",";
                StrSql += "         " + this.CodigoDaVenda + ",";
                StrSql += "         " + this.UsuarioLogado + ",";
                StrSql += "         '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "')";
            }
            else
            {
                StrSql = " INSERT INTO Retirada (cd_produto, cd_usuario, quantidade, bl_venda, cd_venda, cd_cliente, cd_tppagto, qt_vezes, dt_vencto, desconto, acrescimo, vl_total, cd_usu_log, dt_retirada) ";
                StrSql += " VALUES (" + this.CodigoDoProduto + ",";
                StrSql += "         " + this.CodigoDoUsuario + ",";
                StrSql += "         " + this.Quantidade + ",";
                StrSql += "         " + this.Venda + ",";
                StrSql += "         " + this.CodigoDaVenda + ",";
                StrSql += "         " + this.CodigoDoCliente + ",";
                StrSql += "         " + this.CodigoDoTipoDePagamento + ",";
                StrSql += "         " + this.NumeroDeParcelas + ",";
                StrSql += "        '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd") + "',";
                StrSql += "         " + this.Desconto.ToString().Replace(",",".") + ",";
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

            StrSql = " SELECT Max(cd_retirada) as cd_retirada FROM Retirada ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaRetirada = Convert.ToInt16(oDr["cd_retirada"]);
            //**********
            oDr.Close();
            //**********
            this.critica = "Registro salvo com sucesso.";
            
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

    public bool Atualizar()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.CodigoDaRetirada <= 0)
        {
            this.critica = "Código da Retirada deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoProduto == 0)
        {
            this.critica = "Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoUsuario == 0)
        {
            this.critica = "Usuário deve ser informado. Verifique.";
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
                this.critica = "Código da Venda só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoCliente != 0)
            {
                this.critica = "Cliente só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.CodigoDoTipoDePagamento != 0)
            {
                this.critica = "Tipo de Pagamento só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }
            else
            {
                if (this.NumeroDeParcelas != 0)
                {
                    this.critica = "Número de Parcelas só deve ser informado se a Retirada for registro de Venda. Verifique.";
                    return false;
                }
                else
                {
                    if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length != 0)
                    {
                        this.critica = "Data de Vencimento só deve ser informada se a Retirada for registro de Venda. Verifique.";
                        return false;
                    }
                }
            }

            if (this.Desconto != 0)
            {
                this.critica = "Desconto só deve ser informado se a Retirada for registro de Venda. Verifique.";
                return false;
            }

            if (this.Acrescimo != 0)
            {
                this.critica = "Desconto só deve ser informado se a Retirada for registro de Venda. Verifique.";
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

                StrSql = "          SELECT  quantidade ";
                StrSql = StrSql + " FROM    Retirada   ";
                StrSql = StrSql + " WHERE   Retirada.cd_produto = " + this.CodigoDoProduto.ToString();
                StrSql = StrSql + " AND     Retirada.cd_retirada <> " + this.CodigoDaRetirada.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                int qt_est_aux = 0;

                while (oDr.Read())
                {
                    qt_est_aux = qt_est_aux + Convert.ToInt32(oDr["quantidade"]);
                }
                oDr.Close();


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
                    qt_estoque = ((Convert.ToInt32(oDr["qt_estoque"]) - qt_est_aux) - this.Quantidade);
                    if (qt_estoque < 0)
                    {
                        this.critica = "Com essa Retirada o estoque ficará negativo. Operação não permitida.";
                        return false;
                    }
                }
                oDr.Close();
            }

            StrSql = "          SELECT  cd_retirada ";
            StrSql = StrSql + " FROM    Retirada   ";
            StrSql = StrSql + " WHERE   Retirada.cd_retirada = " + this.CodigoDaRetirada.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Retirada não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Retirada Set ";
                StrSql += "         cd_produto   =  " + this.CodigoDoProduto.ToString() + ",";
                StrSql += "         cd_usuario   =  " + this.CodigoDoUsuario.ToString() + ",";
                StrSql += "         quantidade   =  " + this.Quantidade.ToString() + ",";
                StrSql += "         bl_venda     =  " + this.Venda.ToString() + ",";
                if (this.Venda != 0)
                {
                    StrSql += "         cd_venda     =  " + this.CodigoDaVenda.ToString() + ",";
                    StrSql += "         cd_cliente   =  " + this.CodigoDoCliente.ToString() + ",";
                    StrSql += "         cd_tppagto   =  " + this.CodigoDoTipoDePagamento.ToString() + ",";
                    StrSql += "         qt_vezes     =  " + this.NumeroDeParcelas.ToString() + ",";
                    StrSql += "         dt_vencto    = '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd HH:mm:ss") + "',";
                    StrSql += "         desconto     =  " + this.Desconto.ToString().Replace(",", ".") + ",";
                    StrSql += "         acrescimo    =  " + this.Acrescimo.ToString().Replace(",", ".") + ",";
                    StrSql += "         vl_total     =  " + this.ValorTotal.ToString().Replace(",", ".") + ",";
                }
                StrSql += "         cd_usu_log   =  " + this.UsuarioLogado.ToString() + ",";
                StrSql += "         dt_retirada  =  '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                StrSql += " WHERE   cd_retirada  =  " + this.CodigoDaRetirada.ToString();

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
                this.critica = "Registro atualizado com sucesso.";
                Resp = true;
             
            }
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

    public bool Consulta()
    {
        bool Resp = true;
        string StrSql = "";

        if (Convert.ToInt16(this.CodigoDaRetirada) <= 0)
        {
            this.critica = "Código da Retirada deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Retirada   ";
            StrSql = StrSql + " WHERE   Retirada.cd_retirada = " + this.CodigoDaRetirada.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Retirada não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.Venda = 0;
                this.CodigoDoTipoDePagamento = 0;
                
                this.CodigoDaRetirada = Convert.ToInt16(oDr["cd_retirada"]);
                this.CodigoDoProduto = Convert.ToInt16(oDr["cd_produto"]);
                this.CodigoDoUsuario = Convert.ToInt16(oDr["cd_usuario"]);
                this.Quantidade = Convert.ToInt32(oDr["quantidade"]);
                this.Venda = Convert.ToInt16(oDr["bl_venda"]);
                if (this.Venda != 0)
                {
                    this.CodigoDoCliente = Convert.ToInt32(oDr["cd_cliente"]);
                    this.CodigoDaVenda = Convert.ToInt32(oDr["cd_venda"]);
                    this.CodigoDoTipoDePagamento = Convert.ToInt32(oDr["cd_tppagto"]);
                    this.NumeroDeParcelas = Convert.ToInt16(oDr["qt_vezes"]);
                    this.DataDeVencimento = oDr["dt_vencto"].ToString();
                    this.Desconto = Convert.ToDecimal(oDr["desconto"].ToString().Replace(".",","));
                    this.Acrescimo = Convert.ToDecimal(oDr["acrescimo"].ToString().Replace(".", ","));
                }
                Resp = true;
            }

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

    public bool Excluir()
    {
        if (this.CodigoDaRetirada <= 0)
        {
            this.critica = "Código da Retirada deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Retirada ";
            StrSql += " WHERE   Retirada.cd_retirada = " + this.CodigoDaRetirada.ToString();

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************
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
