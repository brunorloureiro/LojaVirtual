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
using System.IO;


/// <summary>
/// Summary description for Fragrancia
/// </summary>
public class Fragrancia
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaFragrancia = 0;
    public string NomeDaFragrancia = "";
    
    public Fragrancia(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Fragrancia";
        string campos = "cd_fragrancia,nm_fragrancia";
        string labels = "Código,Nome";
        string pks = "txtcd_fragrancia";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDaFragrancia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Fragrância deve ser informado. Verifique.";
            return false;
        }

       
        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_fragrancia FROM Fragrancia WHERE lTrim(rTrim(Upper(nm_fragrancia))) like '" + this.NomeDaFragrancia.Trim().ToUpper() + "'";

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
                this.critica = "Já existe fragrância com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Fragrancia (nm_fragrancia) ";
            StrSql += " VALUES ('" + this.NomeDaFragrancia.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_fragrancia) as cd_fragrancia FROM Fragrancia ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaFragrancia = Convert.ToInt16(oDr["cd_fragrancia"]);
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

        if (this.CodigoDaFragrancia <= 0)
        {
            this.critica = "Código da Fragrância deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaFragrancia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Fragrância deve ser informado. Verifique.";
            return false;
        }


        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_fragrancia FROM Fragrancia WHERE lTrim(rTrim(Upper(nm_fragrancia))) like '" + this.NomeDaFragrancia.Trim().ToUpper() + "' AND cd_fragrancia <> " + this.CodigoDaFragrancia.ToString();

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
                this.critica = "Já existe fragrância com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_fragrancia ";
            StrSql = StrSql + " FROM    Fragrancia   ";
            StrSql = StrSql + " WHERE   Fragrancia.cd_fragrancia = " + this.CodigoDaFragrancia.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Fragrância não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE Fragrancia Set ";
                StrSql += "         nm_fragrancia  = '" + this.NomeDaFragrancia.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_fragrancia  =  " + this.CodigoDaFragrancia.ToString();

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

        if (Convert.ToInt16(this.CodigoDaFragrancia) <= 0)
        {
            this.critica = "Código da Fragrância deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Fragrancia   ";
            StrSql = StrSql + " WHERE   Fragrancia.cd_fragrancia = " + this.CodigoDaFragrancia.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Fragrância não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaFragrancia = Convert.ToInt16(oDr["cd_fragrancia"]);
                this.NomeDaFragrancia = (string)oDr["nm_fragrancia"];
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
        if (this.CodigoDaFragrancia <= 0)
        {
            this.critica = "Código da Fragrância deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql = " SELECT * FROM VicSecrets WHERE cd_fragrancia = " + this.CodigoDaFragrancia.ToString();

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
                this.critica = "Não é possível a exclusão desta fragrância pois já existe(m) Victória(s) Secrets(s) relacionado(s) ao mesmo. Operação Cancelada.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " DELETE  FROM Fragrancia ";
            StrSql += " WHERE   Fragrancia.cd_fragrancia = " + this.CodigoDaFragrancia.ToString();

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
