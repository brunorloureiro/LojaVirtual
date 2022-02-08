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


public class Menu
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoMenu = 0;
    public string NomeDoMenu = "";
    public int CodigoDoModulo = 0;
    public string Url = "";
    public int Ativo = 0;
   
    public Menu(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Menu, Modulo";
        string campos = "cd_menu,nm_menu,nm_modulo";
        string labels = "Código,Nome,Módulo";
        string pks = "txtcd_menu";
        string cond  = " AND Menu.cd_modulo = Modulo.cd_modulo";
       
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, true, true);
    }

    public void CarregaLista(object DDL, string Tabela, string Valor, string Msg)
    {
        ClsPublico.carregaLista(DDL, Tabela, Valor, Msg);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoMenu.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Menu deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoModulo == 0)
        {
            this.critica = "Módulo deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_menu FROM Menu WHERE lTrim(rTrim(Upper(nm_menu))) like '" + this.NomeDoMenu.Trim().ToUpper() + "' AND cd_modulo = " + this.CodigoDoModulo.ToString();

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
                this.critica = "Já existe menu com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Menu (nm_menu, cd_modulo, url, bl_ativo) ";
            StrSql += " VALUES ('" + this.NomeDoMenu.Trim().Replace("'", "´")  + "',";
            StrSql += "         " + this.CodigoDoModulo + ",";
            StrSql += "         '" + this.Url.Trim().Replace("'", "´") + "',";
            StrSql += "         " + this.Ativo + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_menu) as cd_menu FROM Menu ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoMenu = Convert.ToInt16(oDr["cd_menu"]);
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

        if (this.CodigoDoMenu <= 0)
        {
            this.critica = "Código do Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoMenu.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Menu deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoModulo == 0)
        {
            this.critica = "Módulo deve ser informado. Verifique.";
            return false;
        }
       
        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_menu FROM Menu WHERE lTrim(rTrim(Upper(nm_menu))) like '" + this.NomeDoMenu.Trim().ToUpper() + "' AND cd_menu <> " + this.CodigoDoMenu.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_menu ";
            StrSql = StrSql + " FROM    Menu   ";
            StrSql = StrSql + " WHERE   Menu.cd_menu = " + this.CodigoDoMenu.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Menu não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Menu Set ";
                StrSql += "         nm_menu   = '" + this.NomeDoMenu.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_modulo =  " + this.CodigoDoModulo.ToString() + ",";
                StrSql += "         url       = '" + this.Url.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_ativo  =  " + this.Ativo.ToString();
                StrSql += " WHERE   cd_menu   =  " + this.CodigoDoMenu.ToString();

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

        if (Convert.ToInt16(this.CodigoDoMenu) <= 0)
        {
            this.critica = "Código do Menu deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Menu   ";
            StrSql = StrSql + " WHERE   Menu.cd_menu = " + this.CodigoDoMenu.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Menu não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoMenu = Convert.ToInt16(oDr["cd_menu"]);
                this.NomeDoMenu = (string)oDr["nm_menu"];
                this.CodigoDoModulo = Convert.ToInt16(oDr["cd_modulo"]);
                this.Url = (string)oDr["url"];
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
        if (this.CodigoDoMenu <= 0)
        {
            this.critica = "Código do Menu deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM SubMenu ";
            StrSql += " WHERE   SubMenu.cd_menu = " + this.CodigoDoMenu.ToString();

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Menu ";
            StrSql += " WHERE   Menu.cd_menu = " + this.CodigoDoMenu.ToString();

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
