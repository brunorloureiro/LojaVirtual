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
using System.Globalization;
using System.Collections;

/// <summary>
/// Summary description for EnviaEstoque
/// </summary>
public class EnviaEstoque
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;
    private ArrayList Itens = new ArrayList();

    
    public int CodigoDoProduto = 0;
    public string critica = "";
    public int Quantidade = 0;
    public int QuantidadeEstoque = 0;

    public EnviaEstoque(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public bool Importa()
    {
        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Entrada   ";
            StrSql = StrSql + " WHERE   Entrada.bl_envio = 0 ";
            StrSql = StrSql + " ORDER BY Entrada.cd_entrada, Entrada.cd_produto ";


            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Não existem Entradas a serem importadas. Verifique.";
                Resp = false;
                return Resp;
            }
            else
            {
                oDr.Close();
            }

            StrSql = "          SELECT  cd_produto, quantidade ";
            StrSql = StrSql + " FROM    Entrada   ";
            StrSql = StrSql + " WHERE   Entrada.bl_envio = 0 ";
            StrSql = StrSql + " ORDER BY Entrada.cd_entrada, Entrada.cd_produto ";

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            while (oDr.Read())
            {
                this.CodigoDoProduto = Convert.ToInt16(oDr["cd_produto"]);
                this.Quantidade = Convert.ToInt32(oDr["quantidade"]);

                Itens item = new Itens(this.CodigoDoProduto, this.Quantidade);
                this.Itens.Add(item);
            }
            oDr.Close();


            foreach (Itens item in this.Itens)
            {
                StrSql = " UPDATE   Entrada Set ";
                StrSql += "         bl_envio   =  1, ";
                StrSql += "         dt_envio   = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                StrSql += " WHERE   cd_produto = " + item.Codigo.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************

                StrSql = "          SELECT  qt_estoque ";
                StrSql = StrSql + " FROM    Produto   ";
                StrSql = StrSql + " WHERE   Produto.cd_produto = " + item.Codigo.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    this.QuantidadeEstoque = Convert.ToInt32(oDr["qt_estoque"]) + item.Quantidade;
                }
                oDr.Close();


                StrSql  = " UPDATE   Produto Set ";
                StrSql += "         qt_estoque   =  " + this.QuantidadeEstoque.ToString();
                StrSql += " WHERE   cd_produto   =  " + item.Codigo.ToString();

                oCmd.Connection = ClsPublico.oConn;
                //*********************************
                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }
            this.critica = "Importação realizada com sucesso.";
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
