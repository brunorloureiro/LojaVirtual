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


public class Jaqueta
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDaJaqueta = "";
    public string NomeDaJaqueta = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public string Tamanho = "";
    public string Cor = "";
    public int Gorro = 0;
    public int Fechicle = 0;
    public int Estampa = 0;
    public string LugarDaEstampa = "";
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    public Jaqueta(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Jaqueta,Tpprod,Marca";
        string campos = "cd_jaqueta, nm_jaqueta,nm_tpprod,nm_marca";
        string labels = "Código,Nome,Tipo de Produto,Marca";
        string pks    = "txtcd_jaqueta";
        string cond   = " AND Jaqueta.cd_tpprod = Tpprod.cd_tpprod";
               cond  += " AND Jaqueta.cd_marca  = Marca.cd_marca";
       
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

        if (this.CodigoDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Jaqueta deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Jaqueta deve ser informado. Verifique.";
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

        if (this.Estampa == 1)
        {
            if (this.LugarDaEstampa.ToString().Trim().Replace("'", "´").Length == 0)
            {
                this.critica = "Estampa deve ser informada. Verifique.";
                LugarDaEstampa = "";
                return false;
            }
        }
        else
        {
            if (this.LugarDaEstampa.ToString().Trim().Replace("'", "´").Length != 0)
            {
                this.critica = "Estampa não deve ser informada quando o parâmetro Possui estampa estiver desmarcado. Verifique.";
                LugarDaEstampa = "";
                return false;
            }
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDaJaqueta.Trim().ToUpper() + "'";

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
            StrSql += " VALUES ('" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDaJaqueta.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************


            StrSql = " SELECT cd_jaqueta FROM Jaqueta WHERE lTrim(rTrim(Upper(cd_jaqueta))) like '" + this.CodigoDaJaqueta.Trim().ToUpper() + "'";

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
                this.critica = "Já existe Jaqueta com o código informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Jaqueta (cd_jaqueta, nm_jaqueta, cd_tpprod, cd_marca, tamanho, cor, bl_gorro, bl_fechic, bl_estampa, estampa, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDaJaqueta.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDaJaqueta.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "         '" + this.Tamanho.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Cor.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Gorro + ",";
            StrSql += "          " + this.Fechicle + ",";
            StrSql += "          " + this.Estampa + ",";
            StrSql += "         '" + this.LugarDaEstampa.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.FreteGratis + ",";
            StrSql += "         '" + this.Descricao.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Comentario.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_jaqueta) as cd_jaqueta FROM Jaqueta ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaJaqueta = (string)oDr["cd_jaqueta"];
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

        if (this.CodigoDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Jaqueta deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Jaqueta deve ser informado. Verifique.";
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

        if (this.Estampa == 1)
        {
            if (this.LugarDaEstampa.ToString().Trim().Replace("'", "´").Length == 0)
            {
                this.critica = "Estampa deve ser informada. Verifique.";
                LugarDaEstampa = "";
                return false;
            }
        }
        else
        {
            if (this.LugarDaEstampa.ToString().Trim().Replace("'", "´").Length != 0)
            {
                this.critica = "Estampa não deve ser informada quando o parâmetro Possui estampa estiver desmarcado. Verifique.";
                LugarDaEstampa = "";
                return false;
            }
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  cd_peca ";
            StrSql = StrSql + " FROM    Produto   ";
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";
            
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
                StrSql += "         cd_peca    = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDaJaqueta.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }


            StrSql = " SELECT cd_jaqueta FROM Jaqueta WHERE lTrim(rTrim(Upper(cd_jaqueta))) like '" + this.CodigoDaJaqueta.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_jaqueta ";
            StrSql = StrSql + " FROM    Jaqueta   ";
            StrSql = StrSql + " WHERE   Jaqueta.cd_jaqueta = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Jaqueta não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Jaqueta Set ";
                StrSql += "         cd_jaqueta   = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_jaqueta   = '" + this.NomeDaJaqueta.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod    =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca     =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         tamanho      = '" + this.Tamanho.Trim().Replace("'", "´") + "',";
                StrSql += "         cor          = '" + this.Cor.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_gorro     =  " + this.Gorro.ToString() + ",";
                StrSql += "         bl_fechic    =  " + this.Fechicle.ToString() + ",";
                StrSql += "         bl_estampa   =  " + this.Estampa.ToString() + ",";
                StrSql += "         estampa      = '" + this.LugarDaEstampa.Trim().Replace("'", "´") + "',";
                StrSql += "         peso         =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco        =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete     =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao    = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario   = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_jaqueta   = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Jaqueta deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Jaqueta   ";
            StrSql = StrSql + " WHERE   Jaqueta.cd_jaqueta = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Jaqueta não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaJaqueta =(string)oDr["cd_jaqueta"];
                this.NomeDaJaqueta = (string)oDr["nm_jaqueta"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.Tamanho = (string)oDr["tamanho"];
                this.Cor = (string)oDr["cor"];
                this.Gorro = Convert.ToInt16(oDr["bl_gorro"]);
                this.Fechicle = Convert.ToInt16(oDr["bl_fechic"]);
                this.Estampa = Convert.ToInt16(oDr["bl_estampa"]);
                this.LugarDaEstampa = (string)oDr["estampa"];
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
        if (this.CodigoDaJaqueta.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Jaqueta deve ser informado. Verifique.";
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
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Jaqueta ";
            StrSql += " WHERE   Jaqueta.cd_jaqueta = '" + this.CodigoDaJaqueta.Trim().Replace("'", "´") + "'";

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
