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


public class Camisa
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDaCamisa = "";
    public string NomeDaCamisa = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public string Tamanho = "";
    public string Cor = "";
    public int BabyLook = 0;
    public int Estampa = 0;
    public string LugarDaEstampa = "";
    public string TipoDeManga = "";
    public string TipoDeGola = "";
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    public Camisa(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Camisa,Tpprod,Marca";
        string campos = "cd_camisa, nm_camisa,nm_tpprod,nm_marca";
        string labels = "Código,Nome,Tipo de Produto,Marca";
        string pks    = "txtcd_camisa";
        string cond   = " AND Camisa.cd_tpprod = Tpprod.cd_tpprod";
               cond  += " AND Camisa.cd_marca  = Marca.cd_marca";
       
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

        if (this.CodigoDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Camisa deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Camisa deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDaCamisa.Trim().ToUpper() + "'";

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
            StrSql += " VALUES ('" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDaCamisa.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************
            
            
            StrSql = " SELECT cd_camisa FROM Camisa WHERE lTrim(rTrim(Upper(cd_camisa))) like '" + this.CodigoDaCamisa.Trim().ToUpper() + "'";

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
                this.critica = "Já existe Camisa com o código informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Camisa (cd_camisa, nm_camisa, cd_tpprod, cd_marca, tamanho, cor, bl_baby, bl_estampa, estampa, manga, gola, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDaCamisa.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDaCamisa.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "         '" + this.Tamanho.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Cor.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.BabyLook + ",";
            StrSql += "          " + this.Estampa + ",";
            StrSql += "         '" + this.LugarDaEstampa.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.TipoDeManga.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.TipoDeGola.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.FreteGratis + ",";
            StrSql += "         '" + this.Descricao.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Comentario.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_camisa) as cd_camisa FROM Camisa ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaCamisa = (string)oDr["cd_camisa"];
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

        if (this.CodigoDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Camisa deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Camisa deve ser informado. Verifique.";
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
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

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
                StrSql += "         cd_peca    = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDaCamisa.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }
            
            StrSql = " SELECT cd_camisa FROM Camisa WHERE lTrim(rTrim(Upper(cd_camisa))) like '" + this.CodigoDaCamisa.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_camisa ";
            StrSql = StrSql + " FROM    Camisa   ";
            StrSql = StrSql + " WHERE   Camisa.cd_camisa = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Camisa não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Camisa Set ";
                StrSql += "         cd_camisa    = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_camisa    = '" + this.NomeDaCamisa.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod    =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca     =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         tamanho      = '" + this.Tamanho.Trim().Replace("'", "´") + "',";
                StrSql += "         cor          = '" + this.Cor.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_baby      =  " + this.BabyLook.ToString() + ",";
                StrSql += "         bl_estampa   =  " + this.Estampa.ToString() + ",";
                StrSql += "         estampa      = '" + this.LugarDaEstampa.Trim().Replace("'", "´") + "',";
                StrSql += "         manga        = '" + this.TipoDeManga.Trim().Replace("'", "´") + "',";
                StrSql += "         gola         = '" + this.TipoDeGola.Trim().Replace("'", "´") + "',";
                StrSql += "         peso         =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco        =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete     =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao    = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario   = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_camisa    = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Camisa deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Camisa   ";
            StrSql = StrSql + " WHERE   Camisa.cd_camisa = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Camisa não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaCamisa =(string)oDr["cd_camisa"];
                this.NomeDaCamisa = (string)oDr["nm_camisa"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.Tamanho = (string)oDr["tamanho"];
                this.Cor = (string)oDr["cor"];
                this.BabyLook = Convert.ToInt16(oDr["bl_baby"]);
                this.Estampa = Convert.ToInt16(oDr["bl_estampa"]);
                this.LugarDaEstampa = (string)oDr["estampa"];
                this.TipoDeManga = (string)oDr["manga"];
                this.TipoDeGola = (string)oDr["gola"];
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
        if (this.CodigoDaCamisa.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Camisa deve ser informado. Verifique.";
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
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "')";
            
            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************
            
            StrSql  = " DELETE  FROM Camisa ";
            StrSql += " WHERE   Camisa.cd_camisa = '" + this.CodigoDaCamisa.Trim().Replace("'", "´") + "'";

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
