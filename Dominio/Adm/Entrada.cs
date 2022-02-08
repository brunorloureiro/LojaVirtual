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

/// <summary>
/// Summary description for Entrada
/// </summary>
public class Entrada
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaEntrada = 0;
    public int CodigoDoProduto = 0;
    public int Quantidade = 0;
    public int UsuarioLogado;
   
    public Entrada(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Entrada, Produto ";
        string campos = "cd_entrada,nm_produto,quantidade";
        string labels = "Código,Produto,Quantidade";
        string pks = "txtcd_entrada";
        string cond  = " AND Entrada.cd_produto = Produto.cd_produto";
               cond += " AND Entrada.bl_envio = 0";
       
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

        if (this.Quantidade == 0)
        {
            this.critica = "Quantidade deve ser informada. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
    
            StrSql  = " INSERT INTO Entrada (cd_produto, quantidade, cd_usu_log, dt_entrada) ";
            StrSql += " VALUES (" + this.CodigoDoProduto + ",";
            StrSql += "         " + this.Quantidade + ",";
            StrSql += "         " + this.UsuarioLogado + ",";
            StrSql += "         '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_entrada) as cd_entrada FROM Entrada ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaEntrada = Convert.ToInt16(oDr["cd_entrada"]);
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

        if (this.CodigoDaEntrada <= 0)
        {
            this.critica = "Código da Entrada deve ser informado. Verifique.";
            return true;
        }

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

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
         
            StrSql = "          SELECT  cd_entrada ";
            StrSql = StrSql + " FROM    Entrada   ";
            StrSql = StrSql + " WHERE   Entrada.cd_entrada = " + this.CodigoDaEntrada.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Entrada não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Entrada Set ";
                StrSql += "         cd_produto   =  " + this.CodigoDoProduto.ToString() + ",";
                StrSql += "         quantidade   =  " + this.Quantidade.ToString() + ",";
                StrSql += "         cd_usu_log   =  " + this.UsuarioLogado.ToString() + ",";
                StrSql += "         dt_entrada  =  '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                StrSql += " WHERE   cd_entrada   =  " + this.CodigoDaEntrada.ToString();

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

        if (Convert.ToInt16(this.CodigoDaEntrada) <= 0)
        {
            this.critica = "Código da Entrada deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Entrada   ";
            StrSql = StrSql + " WHERE   Entrada.cd_entrada = " + this.CodigoDaEntrada.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Entrada não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaEntrada = Convert.ToInt16(oDr["cd_entrada"]);
                this.CodigoDoProduto = Convert.ToInt16(oDr["cd_produto"]);
                this.Quantidade = Convert.ToInt32(oDr["quantidade"]);
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
        if (this.CodigoDaEntrada <= 0)
        {
            this.critica = "Código da Entrada deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Entrada ";
            StrSql += " WHERE   Entrada.cd_entrada = " + this.CodigoDaEntrada.ToString();

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
