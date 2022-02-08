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

public class Troca
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaTroca = 0;
    public string Motivo = "";
    public int CodigoDoProdutoDevolvido = 0;
    public int CodigoDoProdutoLevado = 0;
    public int CodigoDoCliente = 0;
    public int QuantidadeLevada = 0;
    public int QuantidadeEstoqueDevolucao = 0;
    public int QuantidadeDevolvida = 0;
    public int QuantidadeEstoqueTroca = 0;
    public decimal DiferencaPaga = 0;
    public int UsuarioLogado;
    public bool EstoqueNegativo;
    
    public Troca(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Produto";
        string campos = "nm_produto,qt_estoque";
        string labels = "Produto,Quantidade em Estoque";
        string pks = "txtcd_produto";
        string cond = "";
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

        if (this.Motivo == "")
        {
            this.critica = "Motivo da Troca deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoCliente == 0)
        {
            this.critica = "Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoProdutoDevolvido == 0)
        {
            this.critica = "Produto Devolvido deve ser informado. Verifique.";
            return false;
        }

        if (this.QuantidadeDevolvida == 0)
        {
            this.critica = "Quantidade Devolvida deve ser informada. Verifique.";
            return false;
        }

        if (this.CodigoDoProdutoLevado == 0)
        {
            this.critica = "Produto Levado deve ser informado. Verifique.";
            return false;
        }

        if (this.QuantidadeLevada == 0)
        {
            this.critica = "Quantidade Levada deve ser informada. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            if (!this.EstoqueNegativo)
            {
                
                StrSql = "          SELECT  qt_estoque, nm_produto ";
                StrSql = StrSql + " FROM    Produto   ";
                StrSql = StrSql + " WHERE   Produto.cd_produto = " + this.CodigoDoProdutoLevado.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    int qt_estoque = 0;
                    qt_estoque = (Convert.ToInt32(oDr["qt_estoque"]) - this.QuantidadeLevada);
                    if (qt_estoque < 0)
                    {
                        this.critica = "Com essa Troca o estoque do Produto " + (string)oDr["nm_produto"] + " ficará negativo. Operação não permitida.";
                        return false;
                    }
                }
                oDr.Close();
            }

            StrSql = " INSERT INTO Troca (motivo, cd_cliente, cd_pro_dev, qt_dev, cd_pro_lev, qt_lev, dif_paga, cd_usu_log, dt_troca) ";
            StrSql += " VALUES ('" + this.Motivo.Trim().Replace("'", "´") + "',";
            StrSql += "         " + this.CodigoDoCliente + ",";
            StrSql += "         " + this.CodigoDoProdutoDevolvido + ",";
            StrSql += "         " + this.QuantidadeDevolvida + ",";
            StrSql += "         " + this.CodigoDoProdutoLevado + ",";
            StrSql += "         " + this.QuantidadeLevada + ",";
            StrSql += "         " + this.DiferencaPaga.ToString().Replace(",", ".") + ",";
            StrSql += "         " + this.UsuarioLogado + ",";
            StrSql += "         '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "')";


            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_troca) as cd_troca FROM Troca ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaTroca = Convert.ToInt32(oDr["cd_troca"]);
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  qt_estoque ";
            StrSql = StrSql + " FROM    Produto   ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = " + this.CodigoDoProdutoDevolvido.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                this.QuantidadeEstoqueDevolucao = (Convert.ToInt32(oDr["qt_estoque"]) + this.QuantidadeDevolvida);
            }
            oDr.Close();

            StrSql = "          SELECT  qt_estoque ";
            StrSql = StrSql + " FROM    Produto   ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = " + this.CodigoDoProdutoLevado.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                this.QuantidadeEstoqueTroca = (Convert.ToInt32(oDr["qt_estoque"]) - this.QuantidadeLevada);
            }
            oDr.Close();


            StrSql = " UPDATE   Produto Set ";
            StrSql += "         qt_estoque   =  " + this.QuantidadeEstoqueDevolucao.ToString();
            StrSql += " WHERE   cd_produto   =  " + this.CodigoDoProdutoDevolvido.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " UPDATE   Produto Set ";
            StrSql += "         qt_estoque   =  " + this.QuantidadeEstoqueTroca.ToString();
            StrSql += " WHERE   cd_produto   =  " + this.CodigoDoProdutoLevado.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

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
 }
