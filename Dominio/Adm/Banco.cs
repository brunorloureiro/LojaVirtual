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

public class Banco
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoBanco = 0;
    public string NomeDoBanco = "";
    public string Sigla = "";
    
    public Banco(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Banco";
        string campos = "cd_banco,nm_banco, sigla";
        string labels = "Código,Nome,Sigla";
        string pks = "txtcd_banco";
        string cond = "";
       
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoBanco.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Banco deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_banco FROM Banco WHERE lTrim(rTrim(Upper(nm_banco))) like '" + this.NomeDoBanco.Trim().ToUpper() + "'";

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
                this.critica = "Já existe banco com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Banco (nm_banco, sigla) ";
            StrSql += " VALUES ('" + this.NomeDoBanco.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.Sigla.ToUpper().Trim().Replace("'", "´") + "')";
            
            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_banco) as cd_banco FROM Banco ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoBanco = Convert.ToInt16(oDr["cd_banco"]);
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

        if (this.CodigoDoBanco <= 0)
        {
            this.critica = "Código do Banco deve ser informado. Verifique.";
            return true;
        }

        if (this.NomeDoBanco.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Banco deve ser informado. Verifique.";
            return true;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_banco FROM Banco WHERE lTrim(rTrim(Upper(nm_banco))) like '" + this.NomeDoBanco.Trim().ToUpper() + "' AND cd_banco <> " + this.CodigoDoBanco.ToString();

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
                this.critica = "Já existe banco com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_banco ";
            StrSql = StrSql + " FROM    Banco   ";
            StrSql = StrSql + " WHERE   Banco.cd_banco = " + this.CodigoDoBanco.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Banco não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Banco Set ";
                StrSql += "         nm_banco   = '" + this.NomeDoBanco.Trim().Replace("'", "´") + "',";
                StrSql += "         sigla      = '" + this.Sigla.ToUpper().Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_banco   =  " + this.CodigoDoBanco.ToString();

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

        if (Convert.ToInt16(this.CodigoDoBanco) <= 0)
        {
            this.critica = "Código do Banco deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Banco   ";
            StrSql = StrSql + " WHERE   Banco.cd_banco = " + this.CodigoDoBanco.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Banco não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoBanco = Convert.ToInt16(oDr["cd_banco"]);
                this.NomeDoBanco = (string)oDr["nm_banco"];
                this.Sigla = (string)oDr["sigla"];
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
        if (this.CodigoDoBanco <= 0)
        {
            this.critica = "Código do Banco deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {

            StrSql = " SELECT * FROM Cheque WHERE cd_banco = " + this.CodigoDoBanco.ToString();

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
                this.critica = "Não é possível a exclusão deste banco pois já existe(m) Cheque(s) relacionado(s) ao mesmo. Operação Cancelada.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " DELETE  FROM Banco ";
            StrSql += " WHERE   Banco.cd_banco = " + this.CodigoDoBanco.ToString();

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
