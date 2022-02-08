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

public class Promocao
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaPromocao = 0;
    public string NomeDaPromocao = "";
    public int CodigoDoProduto = 0;
    public int Ativo = 0;
   
    public Promocao(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Promocao, Produto ";
        string campos = "cd_promocao,nm_promocao,nm_produto";
        string labels = "Código,Nome,Produto";
        string pks = "txtcd_promocao";
        string cond  = " AND Promocao.cd_produto = Produto.cd_produto";
       
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

        if (this.NomeDaPromocao.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Promoção deve ser informado. Verifique.";
            return true;
        }

        if (this.CodigoDoProduto == 0)
        {
            this.critica = "Produto deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT COUNT(cd_promocao) as total FROM Promocao where bl_ativo = 1";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                if (Convert.ToInt32(oDr["total"]) >= 12)
                {
                    //**********
                    oDr.Close();
                    //**********
                    this.critica = "Não é possível inserir mais de 12 produtos na Área de Promoções.";
                    return false;
                }
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " SELECT cd_promocao FROM Promocao WHERE cd_produto = " + this.CodigoDoProduto.ToString();

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
                this.critica = "Produto já cadastrado na Área de Promoções. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " SELECT cd_promocao FROM Promocao WHERE lTrim(rTrim(Upper(nm_promocao))) like '" + this.NomeDaPromocao.Trim().ToUpper() + "'";

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
                this.critica = "Já existe promoção com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Promocao (nm_promocao, cd_produto, bl_ativo) ";
            StrSql += " VALUES ('" + this.NomeDaPromocao.Trim().Replace("'", "´")  + "',";
            StrSql += "         " + this.CodigoDoProduto + ",";
            StrSql += "         " + this.Ativo + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_promocao) as cd_promocao FROM Promocao ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaPromocao = Convert.ToInt16(oDr["cd_promocao"]);
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

        if (this.CodigoDaPromocao <= 0)
        {
            this.critica = "Código da Promoção deve ser informado. Verifique.";
            return true;
        }

        if (this.NomeDaPromocao.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Promoção deve ser informado. Verifique.";
            return true;
        }

        if (this.CodigoDoProduto == 0)
        {
            this.critica = "Produto deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_promocao FROM Promocao WHERE lTrim(rTrim(Upper(nm_promocao))) like '" + this.NomeDaPromocao.Trim().ToUpper() + "' AND cd_promocao <> " + this.CodigoDaPromocao.ToString();

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
                this.critica = "Já existe promoção com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_promocao ";
            StrSql = StrSql + " FROM    Promocao   ";
            StrSql = StrSql + " WHERE   Promocao.cd_promocao = " + this.CodigoDaPromocao.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Promoção não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Promocao Set ";
                StrSql += "         nm_promocao   = '" + this.NomeDaPromocao.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_produto    =  " + this.CodigoDoProduto.ToString() + ",";
                StrSql += "         bl_ativo      =  " + this.Ativo.ToString();
                StrSql += " WHERE   cd_promocao   =  " + this.CodigoDaPromocao.ToString();

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

        if (Convert.ToInt16(this.CodigoDaPromocao) <= 0)
        {
            this.critica = "Código da Promoção deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Promocao   ";
            StrSql = StrSql + " WHERE   Promocao.cd_promocao = " + this.CodigoDaPromocao.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Promoção não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaPromocao = Convert.ToInt32(oDr["cd_promocao"]);
                this.NomeDaPromocao = (string)oDr["nm_promocao"];
                this.CodigoDoProduto = Convert.ToInt32(oDr["cd_produto"]);
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
        if (this.CodigoDaPromocao <= 0)
        {
            this.critica = "Código da Promoção deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_promocao = " + this.CodigoDaPromocao.ToString();

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
