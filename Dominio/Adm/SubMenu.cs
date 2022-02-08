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
/// Summary description for SubMenu
/// </summary>
public class SubMenu
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoSubMenu = 0;
    public string NomeDoSubMenu = "";
    public int CodigoDoMenu = 0;
    public int CodigoDoGenero = 0;
    public string Url = "";
    public int Ativo = 0;
   
    public SubMenu(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "SubMenu, Menu, Genero, Modulo ";
        string campos = "cd_submenu,nm_submenu,nm_menu, nm_genero, nm_modulo";
        string labels = "Código,Nome,Menu,Gênero,Módulo";
        string pks = "txtcd_submenu";
        string cond  = " AND SubMenu.cd_menu   = Menu.cd_menu";
               cond += " AND SubMenu.cd_genero = Genero.cd_genero";
               cond += " AND Menu.cd_modulo    = Modulo.cd_modulo";
       
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

        if (this.NomeDoSubMenu.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do SubMenu deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoMenu == 0)
        {
            this.critica = "Menu deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_submenu FROM SubMenu WHERE lTrim(rTrim(Upper(nm_submenu))) like '" + this.NomeDoSubMenu.Trim().ToUpper() + "' AND cd_menu = " + this.CodigoDoMenu.ToString();

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
                this.critica = "Já existe SubMenu com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO SubMenu (nm_submenu, cd_menu, cd_genero, url, bl_ativo) ";
            StrSql += " VALUES ('" + this.NomeDoSubMenu.Trim().Replace("'", "´")  + "',";
            StrSql += "         " + this.CodigoDoMenu + ",";
            StrSql += "         " + this.CodigoDoGenero + ",";
            StrSql += "         '" + this.Url.Trim().Replace("'", "´") + "',";
            StrSql += "         " + this.Ativo + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_submenu) as cd_submenu FROM Submenu ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoSubMenu = Convert.ToInt16(oDr["cd_submenu"]);
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

        if (this.CodigoDoSubMenu <= 0)
        {
            this.critica = "Código do SubMenu deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoSubMenu.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do SubMenu deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoMenu == 0)
        {
            this.critica = "Menu deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_submenu FROM SubMenu WHERE lTrim(rTrim(Upper(nm_submenu))) like '" + this.NomeDoSubMenu.Trim().ToUpper() + "' AND cd_submenu <> " + this.CodigoDoSubMenu.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_submenu ";
            StrSql = StrSql + " FROM    SubMenu   ";
            StrSql = StrSql + " WHERE   SubMenu.cd_submenu = " + this.CodigoDoSubMenu.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "SubMenu não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  SubMenu Set ";
                StrSql += "         nm_submenu   = '" + this.NomeDoSubMenu.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_menu      =  " + this.CodigoDoMenu.ToString() + ",";
                StrSql += "         cd_genero    =  " + this.CodigoDoGenero.ToString() + ",";
                StrSql += "         url          = '" + this.Url.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_ativo     =  " + this.Ativo.ToString();
                StrSql += " WHERE   cd_submenu   =  " + this.CodigoDoSubMenu.ToString();

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

        if (Convert.ToInt16(this.CodigoDoSubMenu) <= 0)
        {
            this.critica = "Código do SubMenu deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    SubMenu   ";
            StrSql = StrSql + " WHERE   SubMenu.cd_submenu = " + this.CodigoDoSubMenu.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "SubMenu não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoSubMenu = Convert.ToInt16(oDr["cd_submenu"]);
                this.NomeDoSubMenu = (string)oDr["nm_submenu"];
                this.CodigoDoMenu = Convert.ToInt16(oDr["cd_menu"]);
                this.CodigoDoGenero = Convert.ToInt16(oDr["cd_genero"]);
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
        if (this.CodigoDoSubMenu <= 0)
        {
            this.critica = "Código do SubMenu deve ser informado. Verifique.";
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
            StrSql += " WHERE   SubMenu.cd_submenu = " + this.CodigoDoSubMenu.ToString();

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
