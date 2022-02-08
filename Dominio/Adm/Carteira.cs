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


public class Carteira
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDaCarteira = "";
    public string NomeDaCarteira = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public string Tamanho = "";
    public string Cor = "";
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    
    public Carteira(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Carteira,Tpprod,Marca";
        string campos = "cd_cart, nm_cart, nm_tpprod,nm_marca";
        string labels = "Código,Nome,Tipo de Produto,Marca";
        string pks    = "txtcd_cart";
        string cond   = " AND Carteira.cd_tpprod = Tpprod.cd_tpprod";
               cond  += " AND Carteira.cd_marca  = Marca.cd_marca";
       
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

        if (this.CodigoDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Carteira deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Carteira deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDaCarteira.Trim().ToUpper() + "'";

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
            StrSql += " VALUES ('" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDaCarteira.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT cd_cart FROM Carteira WHERE lTrim(rTrim(Upper(cd_cart))) like '" + this.CodigoDaCarteira.Trim().ToUpper() + "'";

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
                this.critica = "Já existe carteira com o código informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Carteira (cd_cart, nm_cart, cd_tpprod, cd_marca, tamanho, cor, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDaCarteira.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDaCarteira.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "         '" + this.Tamanho.Trim().Replace("'", "´") + "',";
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

            StrSql = " SELECT Max(cd_cart) as cd_cart FROM Carteira ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaCarteira = (string)oDr["cd_cart"];
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

        if (this.CodigoDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Carteira deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Carteira deve ser informado. Verifique.";
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
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

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
                StrSql += "         cd_peca    = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDaCarteira.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }
            
            StrSql = " SELECT cd_cart FROM Carteira WHERE lTrim(rTrim(Upper(cd_cart))) like '" + this.CodigoDaCarteira.Trim().ToUpper()  + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_cart ";
            StrSql = StrSql + " FROM    Carteira   ";
            StrSql = StrSql + " WHERE   Carteira.cd_cart = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Carteira não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Carteira Set ";
                StrSql += "         cd_cart    = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_cart    = '" + this.NomeDaCarteira.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod  =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca   =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         tamanho    = '" + this.Tamanho.Trim().Replace("'", "´") + "',";
                StrSql += "         cor        = '" + this.Cor.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete   =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao  = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_cart    = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Carteira deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Carteira   ";
            StrSql = StrSql + " WHERE   Carteira.cd_cart = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Carteira não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaCarteira =(string)oDr["cd_cart"];
                this.NomeDaCarteira = (string)oDr["nm_cart"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.Tamanho = (string)oDr["tamanho"];
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
        if (this.CodigoDaCarteira.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Carteira deve ser informado. Verifique.";
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
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Carteira ";
            StrSql += " WHERE   Carteira.cd_cart = '" + this.CodigoDaCarteira.Trim().Replace("'", "´") + "'";

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
