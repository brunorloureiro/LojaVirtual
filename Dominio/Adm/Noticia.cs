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
/// Summary description for Noticia
/// </summary>
public class Noticia
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaNoticia = 0;
    public string TituloDaNoticia = "";
    public string DescricaoDaNoticia = "";
    public string DataDaNoticia = "";
    public int Ativo = 0;
    public DateTime dh_ult_atz = DateTime.Today;
    

    public Noticia(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Noticia";
        string campos = "cd_noticia,nm_titulo,dh_noticia,bl_ativo";
        string labels = "Código,Titulo,Data,Ativo?";
        string pks = "txtcd_noticia";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.TituloDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Título da Notícia deve ser informado. Verifique.";
            return false;
        }

        if (this.DescricaoDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Notícia deve ser informada. Verifique.";
            return false;
        }

        if (this.DataDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data da Notícia deve ser informada. Verifique.";
            return false;
        }

        if (!ClsPublico.validaData(this.DataDaNoticia))
        {
            this.critica = "Data da Notícia inválida. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_noticia FROM Noticia WHERE lTrim(rTrim(Upper(nm_titulo))) like '" + this.TituloDaNoticia.Trim().ToUpper() + "'";

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
                this.critica = "Já existe notícia com o título informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Noticia (nm_titulo, dh_noticia, ds_noticia, bl_ativo) ";
            StrSql += " VALUES ('" + this.TituloDaNoticia.Trim().Replace("'", "´") + "'," +
                               "'" + Convert.ToDateTime(this.DataDaNoticia).ToString("yyyy/MM/dd") + "'," +
                               "'" + this.DescricaoDaNoticia.Trim().Replace("'", "´") + "'," +
                               "'" + this.Ativo.ToString() + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_noticia) as cd_noticia FROM Noticia ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaNoticia = Convert.ToInt16(oDr["cd_noticia"]);
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

        if (this.CodigoDaNoticia <= 0)
        {
            this.critica = "Código da Notícia deve ser informado. Verifique.";
            return false;
        }

        if (this.TituloDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Título da Notícia deve ser informada. Verifique.";
            return false;
        }

        if (this.DescricaoDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Notícia deve ser informada. Verifique.";
            return false;
        }

        if (this.DataDaNoticia.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data da Notícia deve ser informada. Verifique.";
            return false;
        }

        if (!ClsPublico.validaData(this.DataDaNoticia))
        {
            this.critica = "Data da Notícia inválida. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_noticia FROM Noticia WHERE lTrim(rTrim(Upper(nm_titulo))) like '" + this.TituloDaNoticia.Trim().ToUpper() + "' AND cd_noticia <> " + this.CodigoDaNoticia.ToString();

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

            StrSql = "          SELECT  cd_noticia ";
            StrSql = StrSql + " FROM    Noticia   ";
            StrSql = StrSql + " WHERE   Noticia.cd_noticia = " + this.CodigoDaNoticia.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Notícia não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE Noticia Set ";
                StrSql += "         nm_titulo   = '" + this.TituloDaNoticia.Trim().Replace("'", "´") + "',";
                StrSql += "         dh_noticia  = '" + Convert.ToDateTime(this.DataDaNoticia).ToString("yyyy/MM/dd HH:mm:ss") + "',";
                StrSql += "         ds_noticia  = '" + this.DescricaoDaNoticia.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_ativo    = '" + this.Ativo.ToString() + "'";
                StrSql += " WHERE   cd_noticia  =  " + this.CodigoDaNoticia.ToString();

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

        if (Convert.ToInt16(this.CodigoDaNoticia) <= 0)
        {
            this.critica = "Código da Notícia deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Noticia   ";
            StrSql = StrSql + " WHERE   Noticia.cd_Noticia = " + this.CodigoDaNoticia.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Notícia não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaNoticia = Convert.ToInt16(oDr["cd_noticia"]);
                this.TituloDaNoticia = (string)oDr["nm_titulo"];
                this.DataDaNoticia = oDr["dh_noticia"].ToString();
                this.DescricaoDaNoticia = (string)oDr["ds_noticia"];
                this.Ativo = Convert.ToInt16(oDr["bl_ativo"]);
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
        if (this.CodigoDaNoticia <= 0)
        {
            this.critica = "Código da Notícia deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Noticia ";
            StrSql += " WHERE   Noticia.cd_Noticia = " + this.CodigoDaNoticia.ToString();

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

    public string TrazNoticia(int codigo, string pathArtigo)
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
            StrSql = StrSql + " FROM    Noticia   ";
            StrSql = StrSql + " WHERE   Noticia.bl_ativo = 1 ";
            StrSql = StrSql + " AND     Noticia.cd_noticia = " + codigo.ToString();

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
                StrArtigo += oDr["dh_noticia"].ToString().Trim() + " - " + oDr["ds_noticia"].ToString().Trim() + "<br><br>";
                StrArtigo += "</div></td></tr>";
            }
            oDr.Close();
            //**********

            StrSql = "          SELECT  top 30 * ";
            StrSql = StrSql + " FROM    Noticia   ";
            StrSql = StrSql + " WHERE   Noticia.bl_ativo = 1 ";
            StrSql = StrSql + " AND     Noticia.cd_noticia <> " + codigo.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            while (oDr.Read())
            {
                StrArtigo += "<tr><td><span class='indice'><li onclick='javascript: window.location.href = " + '"' + "artigos.aspx?id=" + oDr["cd_noticia"].ToString() + '"' + ";' style='cursor: pointer;'>" + oDr["dh_noticia"].ToString().Trim() + " - " + oDr["nm_titulo"].ToString().Trim() + "</li></span></td></tr>";
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
