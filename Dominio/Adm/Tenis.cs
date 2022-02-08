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
/// Summary description for Tenis
/// </summary>
public class Tenis
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDoTenis = "";
    public string NomeDoTenis = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public int Tamanho = 0;
    public string Cor = "";
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    
    public Tenis(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Tenis,Tpprod,Marca";
        string campos = "cd_tenis,nm_tenis,nm_tpprod,nm_marca";
        string labels = "Código,Nome,Tipo de Produto,Marca";
        string pks    = "txtcd_tenis";
        string cond   = " AND Tenis.cd_tpprod = Tpprod.cd_tpprod";
               cond  += " AND Tenis.cd_marca  = Marca.cd_marca";
       
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

        if (this.CodigoDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Tênis deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Tênis deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoTipoDeProduto == 0)
        {
            this.critica = "Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDaMarca == 0)
        {
            this.critica = "Marca deve ser informada. Verifique.";
            return false;
        }

        if (this.Peso == 0)
        {
            this.critica = "Peso deve ser informado. Verifique.";
            return false;
        }

        if (this.Preco == 0)
        {
            this.critica = "Preço deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDoTenis.Trim().ToUpper() + "'";

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
                this.critica = "Já existe um produto com o código informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Produto (cd_peca, nm_produto, peso, preco) ";
            StrSql += " VALUES ('" + this.CodigoDoTenis.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDoTenis.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT cd_tenis FROM Tenis WHERE lTrim(rTrim(Upper(cd_tenis))) like '" + this.CodigoDoTenis.Trim().ToUpper() + "'";

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
                this.critica = "Já existe Tênis com o código informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Tenis (cd_tenis, nm_tenis, cd_tpprod, cd_marca, tamanho, cor, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDoTenis.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDoTenis.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "          " + this.Tamanho + ",";
            StrSql += "         '" + this.Cor.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ","; 
            StrSql += "          " + this.FreteGratis + ",";
            StrSql += "         '" + this.Descricao.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Comentario.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_tenis) as cd_tenis FROM Tenis ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoTenis = (string)oDr["cd_tenis"];
            //**********
            oDr.Close();
            //**********
            this.critica = "Registro salvo com sucesso.";
            this.LimparCampos = true;
            
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

        if (this.CodigoDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Tênis deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Tênis deve ser informado. Verifique.";
            return true;
        }

        if (this.CodigoDoTipoDeProduto == 0)
        {
            this.critica = "Tipo de Produto deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDaMarca == 0)
        {
            this.critica = "Marca deve ser informada. Verifique.";
            return false;
        }

        if (this.Peso == 0)
        {
            this.critica = "Peso deve ser informado. Verifique.";
            return false;
        }

        if (this.Preco == 0)
        {
            this.critica = "Preço deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {

            StrSql = "          SELECT  cd_peca ";
            StrSql = StrSql + " FROM    Produto   ";
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE  Produto Set ";
                StrSql += "         cd_peca    = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDoTenis.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }

            StrSql = " SELECT cd_tenis FROM tenis WHERE lTrim(rTrim(Upper(cd_tenis))) like '" + this.CodigoDoTenis.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_tenis ";
            StrSql = StrSql + " FROM    Tenis   ";
            StrSql = StrSql + " WHERE   Tenis.cd_tenis = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Tênis não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Tenis Set ";
                StrSql += "         cd_tenis   = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_tenis   = '" + this.NomeDoTenis.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod  =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca   =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         tamanho    =  " + this.Tamanho.ToString() + ",";
                StrSql += "         cor        = '" + this.Cor.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete   =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao  = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_tenis   = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código do Tênis deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tenis   ";
            StrSql = StrSql + " WHERE   Tenis.cd_tenis = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Tênis não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoTenis =(string)oDr["cd_tenis"];
                this.NomeDoTenis = (string)oDr["nm_tenis"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.Tamanho = Convert.ToInt16(oDr["tamanho"]);
                this.Cor = (string)oDr["cor"];
                this.FreteGratis = Convert.ToInt16(oDr["bl_frete"]);
                this.Peso = Convert.ToDecimal(oDr["peso"].ToString().Replace(".", ","));
                this.Preco = Convert.ToDecimal(oDr["preco"].ToString().Replace(".", ","));
                this.Descricao = (string)oDr["descricao"];
                this.Comentario = (string)oDr["comentario"]; 
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
        if (this.CodigoDoTenis.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código do Tênis deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Principal ";
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Tenis ";
            StrSql += " WHERE   Tenis.cd_tenis = '" + this.CodigoDoTenis.Trim().Replace("'", "´") + "'";

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
