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


public class TiposDePrata
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoTipoDePrata = 0;
    public string NomeDoTipoDePrata = "";
    
    public TiposDePrata(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Tpprata";
        string campos = "cd_tpprata,nm_tpprata";
        string labels = "Código,Nome";
        string pks = "txtcd_tpprata";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoTipoDePrata.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Prata deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpprata FROM Tpprata WHERE lTrim(rTrim(Upper(nm_tpprata))) like '" + this.NomeDoTipoDePrata.Trim().ToUpper() + "'";

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
                this.critica = "Já existe tipo de prata com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Tpprata (nm_tpprata) ";
            StrSql += " VALUES ('" + this.NomeDoTipoDePrata.Trim().Replace("'", "´")  + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_tpprata) as cd_tpprata FROM Tpprata ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoTipoDePrata = Convert.ToInt16(oDr["cd_tpprata"]);
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

        if (this.CodigoDoTipoDePrata <= 0)
        {
            this.critica = "Código do Tipo de Prata deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTipoDePrata.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Prata deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpprata FROM Tpprata WHERE lTrim(rTrim(Upper(nm_tpprata))) like '" + this.NomeDoTipoDePrata.Trim().ToUpper() + "' AND cd_tpprata <> " + this.CodigoDoTipoDePrata.ToString();

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
                this.critica = "Já existe tipo de Prata com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_tpprata ";
            StrSql = StrSql + " FROM    Tpprata   ";
            StrSql = StrSql + " WHERE   Tpprata.cd_tpprata = " + this.CodigoDoTipoDePrata.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Tipo de Prata não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Tpprata Set ";
                StrSql += "         nm_tpprata   = '" + this.NomeDoTipoDePrata.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_tpprata   =  " + this.CodigoDoTipoDePrata.ToString();

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

        if (Convert.ToInt16(this.CodigoDoTipoDePrata) <= 0)
        {
            this.critica = "Código do Tipo de Prata deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tpprata   ";
            StrSql = StrSql + " WHERE   Tpprata.cd_tpprata = " + this.CodigoDoTipoDePrata.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Tipo de Prata não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoTipoDePrata = Convert.ToInt16(oDr["cd_tpprata"]);
                this.NomeDoTipoDePrata = (string)oDr["nm_tpprata"];
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
        if (this.CodigoDoTipoDePrata <= 0)
        {
            this.critica = "Código do Tipo de Prata deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Tpprata ";
            StrSql += " WHERE   Tpprata.cd_tpprata = " + this.CodigoDoTipoDePrata.ToString();

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
