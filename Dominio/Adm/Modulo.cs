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
/// Summary description for Modulo
/// </summary>
public class Modulo
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoModulo = 0;
    public string NomeDoModulo = "";


    public Modulo(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Modulo";
        string campos = "cd_modulo,nm_modulo";
        string labels = "Código,Nome";
        string pks = "txtcd_modulo";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, false);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoModulo.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Módulo deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_modulo FROM Modulo WHERE lTrim(rTrim(Upper(nm_modulo))) like '" + this.NomeDoModulo.Trim().ToUpper() + "'";

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
                this.critica = "Já existe módulo com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Modulo (nm_modulo) ";
            StrSql += " VALUES ('" + this.NomeDoModulo.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_modulo) as cd_modulo FROM Modulo ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoModulo = Convert.ToInt16(oDr["cd_modulo"]);
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

        if (this.CodigoDoModulo <= 0)
        {
            this.critica = "Código do Módulo deve ser informado. Verifique.";
            return true;
        }

        if (this.NomeDoModulo.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Módulo deve ser informado. Verifique.";
            return true;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_modulo FROM Modulo WHERE lTrim(rTrim(Upper(nm_modulo))) like '" + this.NomeDoModulo.Trim().ToUpper() + "' AND cd_modulo <> " + this.CodigoDoModulo.ToString();

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
                this.critica = "Já existe módulo com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_modulo ";
            StrSql = StrSql + " FROM    Modulo   ";
            StrSql = StrSql + " WHERE   Modulo.cd_modulo = " + this.CodigoDoModulo.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Módulo não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE  Modulo Set ";
                StrSql += "         nm_modulo   = '" + this.NomeDoModulo.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_modulo   =  " + this.CodigoDoModulo.ToString();

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

        if (Convert.ToInt16(this.CodigoDoModulo) <= 0)
        {
            this.critica = "Código do Módulo deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Modulo   ";
            StrSql = StrSql + " WHERE   Modulo.cd_modulo = " + this.CodigoDoModulo.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Módulo não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoModulo = Convert.ToInt16(oDr["cd_modulo"]);
                this.NomeDoModulo = (string)oDr["nm_modulo"];
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
        if (this.CodigoDoModulo <= 0)
        {
            this.critica = "Código do Módulo deve ser informado. Verifique.";
            return false;
        }
        string StrSql = "";
        bool Resp = true;
        

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql = " SELECT cd_menu FROM Menu WHERE cd_modulo = " + this.CodigoDoModulo.ToString();

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
                this.critica = "Não é possível a exclusão deste módulo pois já existe(m) Menu(s) relacionado(s) ao mesmo. Operação Cancelada.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " DELETE  FROM Modulo ";
            StrSql += " WHERE   Modulo.cd_modulo = " + this.CodigoDoModulo.ToString();

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
