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

public class Calca
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDaCalca = "";
    public string NomeDaCalca = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public string Tamanho = "";
    public string Cor = "";
    public int CodigoDoTipo = 0;
    public string Cintura = "";
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    
    public Calca(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Calca,Tpprod,Marca,Tpcalca";
        string campos = "cd_calca,nm_calca,nm_tpprod,nm_marca, nm_tpcalca";
        string labels = "Código,Nome,Tipo de Produto,Marca,Tipo de Calça";
        string pks    = "txtcd_calca";
        string cond   = " AND Calca.cd_tpprod   = Tpprod.cd_tpprod";
               cond  += " AND Calca.cd_marca    = Marca.cd_marca";
               cond  += " AND Calca.cd_tpcalca  = Tpcalca.cd_tpcalca";
       
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

        if (this.CodigoDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Calça deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Calça deve ser informado. Verifique.";
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

        if (this.CodigoDoTipo == 0)
        {
            this.critica = "Tipo de Calça deve ser informado. Verifique.";
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

            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDaCalca.Trim().ToUpper() + "'";

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

            StrSql  = " INSERT INTO Produto (cd_peca, nm_produto, peso, preco) ";
            StrSql += " VALUES ('" + this.CodigoDaCalca.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDaCalca.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************
            
            StrSql = " SELECT cd_calca FROM Calca WHERE lTrim(rTrim(Upper(cd_calca))) like '" + this.CodigoDaCalca.Trim().ToUpper() + "'";

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
                this.critica = "Já existe calça com o código informado. Verifique.";
                return false;
            }
            
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Calca (cd_calca, nm_calca, cd_tpprod, cd_marca, tamanho, cor, cd_tpcalca, cintura, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDaCalca.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDaCalca.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "         '" + this.Tamanho.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Cor.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipo + ",";
            StrSql += "         '" + this.Cintura.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.FreteGratis + ",";
            StrSql += "         '" + this.Descricao.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Comentario.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_calca) as cd_calca FROM Calca ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaCalca = (string)oDr["cd_calca"];
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

        if (this.CodigoDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Calça deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Calça deve ser informado. Verifique.";
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
        
        if (this.CodigoDoTipo == 0)
        {
            this.critica = "Tipo de Calça deve ser informado. Verifique.";
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
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

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
                StrSql  = " UPDATE  Produto Set ";
                StrSql += "         cd_peca    = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDaCalca.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }
            
            StrSql = " SELECT cd_calca FROM Calca WHERE lTrim(rTrim(Upper(cd_calca))) like '" + this.CodigoDaCalca.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
      
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_calca ";
            StrSql = StrSql + " FROM    Calca   ";
            StrSql = StrSql + " WHERE   Calca.cd_calca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Calça não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Calca Set ";
                StrSql += "         cd_calca    = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_calca    = '" + this.NomeDaCalca.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod  =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca   =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         tamanho    = '" + this.Tamanho.Trim().Replace("'", "´") + "',";
                StrSql += "         cor        = '" + this.Cor.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpcalca =  " + this.CodigoDoTipo.ToString() + ",";
                StrSql += "         cintura    = '" + this.Cintura.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete   =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao  = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_calca    = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Calça deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Calca   ";
            StrSql = StrSql + " WHERE   Calca.cd_calca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Calça não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaCalca =(string)oDr["cd_calca"];
                this.NomeDaCalca = (string)oDr["nm_calca"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.Tamanho = (string)oDr["tamanho"];
                this.Cor = (string)oDr["cor"];
                this.Cintura = (string)oDr["cintura"];
                this.CodigoDoTipo = Convert.ToInt16(oDr["cd_tpcalca"]);
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
        if (this.CodigoDaCalca.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Calça deve ser informado. Verifique.";
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
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Calca ";
            StrSql += " WHERE   Calca.cd_calca = '" + this.CodigoDaCalca.Trim().Replace("'", "´") + "'";

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
