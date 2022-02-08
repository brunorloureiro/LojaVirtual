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


public class VicSecrets
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDaVicSecrets = "";
    public string NomeDaVicSecrets = "";
    public int CodigoDoTipoDeProduto = 0;
    public int CodigoDaMarca = 0;
    public int CodigoDaFragrancia = 0;
    public decimal Volume = 0;
    public decimal Peso = 0;
    public decimal Preco = 0;
    public int FreteGratis = 0;
    public string Descricao = "";
    public string Comentario = "";
    public bool LimparCampos = false;
    
    
    public VicSecrets(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "VicSecrets,Tpprod,Marca,Fragrancia";
        string campos = "cd_vicsec,nm_vicsec,nm_tpprod,nm_marca,nm_fragrancia";
        string labels = "Código,Nome,Tipo de Produto,Marca,Tipo de Fragrância";
        string pks    = "txtcd_vicsec";
        string cond   = " AND VicSecrets.cd_tpprod     = Tpprod.cd_tpprod";
               cond  += " AND VicSecrets.cd_marca      = Marca.cd_marca";
               cond  += " AND VicSecrets.cd_fragrancia = Fragrancia.cd_fragrancia";
       
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

        if (this.CodigoDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Victória Secrets deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Victória Secrets deve ser informado. Verifique.";
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

        if (this.CodigoDaFragrancia == 0)
        {
            this.critica = "Fragrância deve ser informada. Verifique.";
            return false;
        }

        if (this.Volume == 0)
        {
            this.critica = "Volume deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_peca FROM Produto WHERE lTrim(rTrim(Upper(cd_peca))) like '" + this.CodigoDaVicSecrets.Trim().ToUpper() + "'";

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
            StrSql += " VALUES ('" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.NomeDaVicSecrets.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************
            
            StrSql = " SELECT cd_vicsec FROM VicSecrets WHERE lTrim(rTrim(Upper(cd_vicsec))) like '" + this.CodigoDaVicSecrets.Trim().ToUpper() + "'";

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
                this.critica = "Já existe Victória Secrets com o código informado. Verifique.";
                return false;
            }
            
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO VicSecrets (cd_vicsec, nm_vicsec, cd_tpprod, cd_marca, cd_fragrancia, volume, peso, preco, bl_frete, descricao, comentario) ";
            StrSql += " VALUES ('" + this.CodigoDaVicSecrets.Trim().Replace("'", "´")  + "',";
            StrSql += "         '" + this.NomeDaVicSecrets.Trim().Replace("'", "´") + "',";
            StrSql += "          " + this.CodigoDoTipoDeProduto + ",";
            StrSql += "          " + this.CodigoDaMarca + ",";
            StrSql += "          " + this.CodigoDaFragrancia + ",";
            StrSql += "          " + this.Volume.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Peso.ToString().Replace(",", ".") + ",";
            StrSql += "          " + this.Preco.ToString().Replace(",", ".") + ","; 
            StrSql += "          " + this.FreteGratis + ",";
            StrSql += "         '" + this.Descricao.Trim().Replace("'", "´") + "',";
            StrSql += "         '" + this.Comentario.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_vicsec) as cd_vicsec FROM VicSecrets ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaVicSecrets = (string)oDr["cd_vicsec"];
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

        if (this.CodigoDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Victória Secrets deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome da Victória Secrets deve ser informado. Verifique.";
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
        
        if (this.CodigoDaFragrancia == 0)
        {
            this.critica = "Fragrância deve ser informada. Verifique.";
            return false;
        }

        if (this.Volume == 0)
        {
            this.critica = "Volume deve ser informado. Verifique.";
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
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

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
                StrSql += "         cd_peca    = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_produto    = '" + this.NomeDaVicSecrets.Trim().Replace("'", "´") + "',";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",", ".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".");
                StrSql += " WHERE   cd_peca    = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
            }

            
            StrSql = " SELECT cd_vicsec FROM VicSecrets WHERE lTrim(rTrim(Upper(cd_vicsec))) like '" + this.CodigoDaVicSecrets.Trim().ToUpper() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
      
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_vicsec ";
            StrSql = StrSql + " FROM    VicSecrets   ";
            StrSql = StrSql + " WHERE   VicSecrets.cd_vicsec = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Victória Secrets não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  VicSecrets Set ";
                StrSql += "         cd_vicsec    = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "',";
                StrSql += "         nm_vicsec    = '" + this.NomeDaVicSecrets.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_tpprod  =  " + this.CodigoDoTipoDeProduto.ToString() + ","; 
                StrSql += "         cd_marca   =  " + this.CodigoDaMarca.ToString() + ",";
                StrSql += "         cd_fragrancia    =  " + this.CodigoDaFragrancia.ToString() + ",";
                StrSql += "         volume     =  " + this.Volume.ToString().Replace(",",".") + ",";
                StrSql += "         peso       =  " + this.Peso.ToString().Replace(",",".") + ",";
                StrSql += "         preco      =  " + this.Preco.ToString().Replace(",", ".") + ",";
                StrSql += "         bl_frete   =  " + this.FreteGratis.ToString() + ",";
                StrSql += "         descricao  = '" + this.Descricao.Trim().Replace("'", "´") + "',";
                StrSql += "         comentario = '" + this.Comentario.Trim().Replace("'", "´") + "'";
                StrSql += " WHERE   cd_vicsec    = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

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

        if (this.CodigoDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Victória Secrets deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    VicSecrets   ";
            StrSql = StrSql + " WHERE   VicSecrets.cd_vicsec = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Victória Secrets não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDaVicSecrets =(string)oDr["cd_vicsec"];
                this.NomeDaVicSecrets = (string)oDr["nm_vicsec"];
                this.CodigoDoTipoDeProduto = Convert.ToInt16(oDr["cd_tpprod"]);
                this.CodigoDaMarca = Convert.ToInt16(oDr["cd_marca"]);
                this.CodigoDaFragrancia = Convert.ToInt16(oDr["cd_fragrancia"]);
                this.FreteGratis = Convert.ToInt16(oDr["bl_frete"]);
                this.Volume = Convert.ToDecimal(oDr["volume"].ToString().Replace(".",","));
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
        if (this.CodigoDaVicSecrets.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Código da Victória Secrets deve ser informado. Verifique.";
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
            StrSql += " WHERE   Principal.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Promocao ";
            StrSql += " WHERE   Promocao.cd_produto IN (SELECT cd_produto as cd_prod FROM Produto WHERE Produto.cd_peca = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "')";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Produto ";
            StrSql += " WHERE   Produto.cd_peca = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************
            
            StrSql  = " DELETE  FROM VicSecrets ";
            StrSql += " WHERE   VicSecrets.cd_vicsec = '" + this.CodigoDaVicSecrets.Trim().Replace("'", "´") + "'";

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
