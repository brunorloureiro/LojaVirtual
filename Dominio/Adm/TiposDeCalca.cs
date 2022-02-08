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
/// Summary description for TiposDeCalca
/// </summary>
public class TiposDeCalca
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoTipoDeCalca = 0;
    public string NomeDoTipoDeCalca = "";
    
    public TiposDeCalca(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Tpcalca";
        string campos = "cd_tpcalca,nm_tpcalca";
        string labels = "Código,Nome";
        string pks = "txtcd_tpcalca";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoTipoDeCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Calça deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpcalca FROM Tpcalca WHERE lTrim(rTrim(Upper(nm_tpcalca))) like '" + this.NomeDoTipoDeCalca.Trim().ToUpper() + "'";

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
                this.critica = "Já existe tipo de calça com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Tpcalca (nm_tpcalca) ";
            StrSql += " VALUES ('" + this.NomeDoTipoDeCalca.Trim().Replace("'", "´")  + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_tpcalca) as cd_tpcalca FROM Tpcalca ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoTipoDeCalca = Convert.ToInt16(oDr["cd_tpcalca"]);
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

        if (this.CodigoDoTipoDeCalca <= 0)
        {
            this.critica = "Código do Tipo de Calça deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTipoDeCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Calça deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tpcalca FROM Tpcalca WHERE lTrim(rTrim(Upper(nm_tpcalca))) like '" + this.NomeDoTipoDeCalca.Trim().ToUpper() + "' AND cd_tpcalca <> " + this.CodigoDoTipoDeCalca.ToString();

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
                this.critica = "Já existe tipo de calça com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_tpcalca ";
            StrSql = StrSql + " FROM    Tpcalca   ";
            StrSql = StrSql + " WHERE   Tpcalca.cd_tpcalca = " + this.CodigoDoTipoDeCalca.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Tipo de Calça não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Tpcalca Set ";
                StrSql += "         nm_tpcalca   = '" + this.NomeDoTipoDeCalca.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_tpcalca   =  " + this.CodigoDoTipoDeCalca.ToString();

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

        if (Convert.ToInt16(this.CodigoDoTipoDeCalca) <= 0)
        {
            this.critica = "Código do Tipo de Calça deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tpcalca   ";
            StrSql = StrSql + " WHERE   Tpcalca.cd_tpcalca = " + this.CodigoDoTipoDeCalca.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Tipo de Calça não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoTipoDeCalca = Convert.ToInt16(oDr["cd_tpcalca"]);
                this.NomeDoTipoDeCalca = (string)oDr["nm_tpcalca"];
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
        if (this.CodigoDoTipoDeCalca <= 0)
        {
            this.critica = "Código do Tipo de Calça deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Tpcalca ";
            StrSql += " WHERE   Tpcalca.cd_tpcalca = " + this.CodigoDoTipoDeCalca.ToString();

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
