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


public class TiposDeProduto
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoTipoDeProduto = 0;
    public string NomeDoTipoDeProduto = "";
    public int CodigoDoGenero = 0;
    

    public TiposDeProduto(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Tpprod, Genero";
        string campos = "cd_tpprod,nm_tpprod, nm_genero";
        string labels = "Código,Nome,Gênero";
        string pks = "txtcd_tpprod";
        string cond = " AND Tpprod.cd_genero = Genero.cd_genero";
       
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

        if (this.NomeDoTipoDeProduto.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoGenero == 0)
        {
            this.critica = "Gênero deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpprod FROM Tpprod WHERE lTrim(rTrim(Upper(nm_tpprod))) like '" + this.NomeDoTipoDeProduto.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                //**********
                oDr.Close();
                //**********
                this.critica = "Já existe tipo de produto com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Tpprod (nm_tpprod, cd_genero) ";
            StrSql += " VALUES ('" + this.NomeDoTipoDeProduto.Trim().Replace("'", "´")  + "',";
            StrSql += "         " + this.CodigoDoGenero + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_tpprod) as cd_tpprod FROM Tpprod ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
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

        if (this.CodigoDoTipoDeProduto <= 0)
        {
            this.critica = "Código do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTipoDeProduto.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoGenero == 0)
        {
            this.critica = "Gênero deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpprod FROM Tpprod WHERE lTrim(rTrim(Upper(nm_tpprod))) like '" + this.NomeDoTipoDeProduto.Trim().ToUpper() + "' AND cd_tpprod <> " + this.CodigoDoTipoDeProduto.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                //**********
                oDr.Close();
                //**********
                this.critica = "Já existe tipo de produto com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_tpprod ";
            StrSql = StrSql + " FROM    Tpprod   ";
            StrSql = StrSql + " WHERE   Tpprod.cd_tpprod = " + this.CodigoDoTipoDeProduto.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Tipo de Produto não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Tpprod Set ";
                StrSql += "         nm_tpprod   = '" + this.NomeDoTipoDeProduto.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_genero   =  " + this.CodigoDoGenero.ToString();
                StrSql += " WHERE   cd_tpprod   =  " + this.CodigoDoTipoDeProduto.ToString();

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

        if (Convert.ToInt16(this.CodigoDoTipoDeProduto) <= 0)
        {
            this.critica = "Código do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tpprod   ";
            StrSql = StrSql + " WHERE   Tpprod.cd_tpprod = " + this.CodigoDoTipoDeProduto.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Tipo de Produto não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.NomeDoTipoDeProduto = (string)oDr["nm_tpprod"];
                this.CodigoDoGenero = Convert.ToInt16(oDr["cd_genero"]);
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
        if (this.CodigoDoTipoDeProduto <= 0)
        {
            this.critica = "Código do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Tpprod ";
            StrSql += " WHERE   Tpprod.cd_tpprod = " + this.CodigoDoTipoDeProduto.ToString();

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
