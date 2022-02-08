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
/// Summary description for Marca
/// </summary>
public class Marca
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaMarca = 0;
    public string NomeDaMarca = "";
    

    public Marca(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Marca";
        string campos = "cd_marca,nm_marca";
        string labels = "Código,Nome";
        string pks = "txtcd_marca";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
      }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDaMarca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Marca deve ser informado. Verifique.";
            return false;
        }

       
        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_marca FROM Marca WHERE lTrim(rTrim(Upper(nm_marca))) like '" + this.NomeDaMarca.Trim().ToUpper() + "'";

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
                this.critica = "Já existe notícia com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Marca (nm_marca) ";
            StrSql += " VALUES ('" + this.NomeDaMarca.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_marca) as cd_marca FROM Marca ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
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

        if (this.CodigoDaMarca <= 0)
        {
            this.critica = "Código da Marca deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaMarca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Marca deve ser informado. Verifique.";
            return false;
        }


        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_marca FROM Marca WHERE lTrim(rTrim(Upper(nm_marca))) like '" + this.NomeDaMarca.Trim().ToUpper() + "' AND cd_marca <> " + this.CodigoDaMarca.ToString();

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
                this.critica = "Já existe marca com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_marca ";
            StrSql = StrSql + " FROM    Marca   ";
            StrSql = StrSql + " WHERE   Marca.cd_marca = " + this.CodigoDaMarca.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Marca não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE Marca Set ";
                StrSql += "         nm_marca  = '" + this.NomeDaMarca.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_marca  =  " + this.CodigoDaMarca.ToString();

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

        if (Convert.ToInt16(this.CodigoDaMarca) <= 0)
        {
            this.critica = "Código da Marca deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Marca   ";
            StrSql = StrSql + " WHERE   Marca.cd_marca = " + this.CodigoDaMarca.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Marca não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.NomeDaMarca = (string)oDr["nm_marca"];
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
        if (this.CodigoDaMarca <= 0)
        {
            this.critica = "Código da Marca deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Marca ";
            StrSql += " WHERE   Marca.cd_marca = " + this.CodigoDaMarca.ToString();

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

    public string TrazMarca(int codigo, string pathArtigo)
    {
        string Resp = "";
        string StrSql = "", StrArtigo = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { return ClsPublico.critica; }
        //*************************************************************************************
        try
        {
            StrArtigo += "<table border='0' cellpadding='0' celspacing='0'>";

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Marca   ";
            StrSql = StrSql + " WHERE   Marca.bl_ativo = 1 ";
            StrSql = StrSql + " AND     Marca.cd_marca = " + codigo.ToString();

            oCmd.Connection = ClsPublico.oConn;
             //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (oDr.Read())
            {
                if (File.Exists(pathArtigo + codigo.ToString() + ".jpg"))
                {
                    StrArtigo += "<tr><td><div class='artigo'><img style='margin-right: 10px; margin-top: 05px;' src='../images/artigos/" + codigo.ToString() + ".jpg' align='left'>";
                }
                else
                {
                    StrArtigo += "<tr><td><div class='artigo'>";
                }

                StrArtigo += "<span class='titulo'>" + oDr["nm_titulo"].ToString().Trim() + "</span><br><br>";
                StrArtigo += oDr["dh_Marca"].ToString().Trim() + " - " + oDr["ds_Marca"].ToString().Trim() + "<br><br>";
                StrArtigo += "</div></td></tr>";
            }
            oDr.Close();
            //**********

            StrSql = "          SELECT  top 30 * ";
            StrSql = StrSql + " FROM    Marca   ";
            StrSql = StrSql + " WHERE   Marca.bl_ativo = 1 ";
            StrSql = StrSql + " AND     Marca.cd_marca <> " + codigo.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            while (oDr.Read())
            {
                StrArtigo += "<tr><td><span class='indice'><li onclick='javascript: window.location.href = " + '"' + "artigos.aspx?id=" + oDr["cd_marca"].ToString() + '"' + ";' style='cursor: pointer;'>" + oDr["dh_Marca"].ToString().Trim() + " - " + oDr["nm_titulo"].ToString().Trim() + "</li></span></td></tr>";
            }
            oDr.Close();
            //**********

            StrArtigo += "</table>";
        }
        catch (Exception Err)
        {
            this.critica = Err.Message.ToString();
            Resp = this.critica;
        }

        //**************************************************************************************
        if (!ClsPublico.FechaConexao()) { return ClsPublico.critica; }
        //**************************************************************************************

        return StrArtigo;
        //***************
    }
}
