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

public class Cheque
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoCheque = 0;
    public int CodigoDoCliente = 0;
    public int CodigoDoBanco = 0;
    public int CodigoDaSituacao = 0;
    public string Tipo = "";
    public string DataDeVencimento = "";
    public decimal Valor = 0;
    public int UsuarioLogado;
    
    public Cheque(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Cheque, Cliente, Banco, Sitcheque  ";
        string campos = "cd_cheque,nm_cliente,nm_banco, valor, dt_vencto, tipo, nm_sitcheque";
        string labels = "Código,Cliente,Banco,Valor,Data de Vencimento, Tipo, Situação";
        string pks = "txtcd_cheque";
        string cond  = " AND Cheque.cd_cliente   = Cliente.cd_cliente";
               cond += " AND Cheque.cd_banco     = Banco.cd_banco";
               cond += " AND Cheque.cd_sitcheque = Sitcheque.cd_sitcheque";
               
       
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

        if (this.CodigoDoCheque == 0)
        {
            this.critica = "N° do Cheque deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoCliente == 0)
        {
            this.critica = "Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoBanco == 0)
        {
            this.critica = "Banco deve ser informado. Verifique.";
            return false;
        }

        
        if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Vencimento deve ser preenchida. Verifique.";
            return false;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor do Cheque deve ser preenchido. Verifique.";
            return false;
        }

        if (this.CodigoDaSituacao == 0)
        {
            this.critica = "Situação do Cheque deve ser informada. Verifique.";
            return false;
        }

         
        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
          
            StrSql = " SELECT cd_cheque FROM Cheque WHERE cd_cheque = " + this.CodigoDoCheque.ToString();

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
                this.critica = "Já existe um cheque com o número informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Cheque (cd_cheque, cd_banco, cd_cliente, valor, dt_vencto, cd_sitcheque, tipo, cd_usu_log) ";
            StrSql += " VALUES (" + this.CodigoDoCheque + ",";
            StrSql += "         " + this.CodigoDoBanco + ",";
            StrSql += "         " + this.CodigoDoCliente + ",";
            StrSql += "         " + this.Valor.ToString().Replace(",",".") + ",";
            StrSql += "        '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd") + "',";
            StrSql += "         " + this.CodigoDaSituacao + ",";
            StrSql += "        '" + this.Tipo.Trim().Replace("'", "´") + "',";
            StrSql += "         " + this.UsuarioLogado + ")";
         

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_cheque) as cd_cheque FROM Cheque ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoCheque = Convert.ToInt32(oDr["cd_cheque"]);
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

        if (this.CodigoDoCheque == 0)
        {
            this.critica = "N° do Cheque deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoCliente == 0)
        {
            this.critica = "Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoBanco == 0)
        {
            this.critica = "Banco deve ser informado. Verifique.";
            return false;
        }


        if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Vencimento deve ser preenchida. Verifique.";
            return false;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor do Cheque deve ser preenchido. Verifique.";
            return false;
        }

        if (this.CodigoDaSituacao == 0)
        {
            this.critica = "Situação do Cheque deve ser informada. Verifique.";
            return false;
        }        

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {

            StrSql = "          SELECT  cd_cheque ";
            StrSql = StrSql + " FROM    Cheque   ";
            StrSql = StrSql + " WHERE   Cheque.cd_cheque = " + this.CodigoDoCheque.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Cheque não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Cheque Set ";
                StrSql += "         cd_cheque    =  " + this.CodigoDoCheque.ToString() + ",";
                StrSql += "         cd_cliente   =  " + this.CodigoDoCliente.ToString() + ",";
                StrSql += "         cd_banco     =  " + this.CodigoDoBanco.ToString() + ",";
                StrSql += "         dt_vencto    = '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd HH:mm:ss") + "',";
                StrSql += "         valor        =  " + this.Valor.ToString().Replace(",", ".") + ",";
                StrSql += "         cd_sitcheque =  " + this.CodigoDaSituacao.ToString() + ",";
                StrSql += "         tipo         = '" + this.Tipo.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_usu_log   =  " + this.UsuarioLogado.ToString();
                StrSql += " WHERE   cd_cheque  =  " + this.CodigoDoCheque.ToString();

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

        if (Convert.ToInt32(this.CodigoDoCheque) <= 0)
        {
            this.critica = "Código da Cheque deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Cheque   ";
            StrSql = StrSql + " WHERE   Cheque.cd_cheque = " + this.CodigoDoCheque.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Cheque não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                
                this.CodigoDoCheque = Convert.ToInt32(oDr["cd_cheque"]);
                this.CodigoDoCliente = Convert.ToInt32(oDr["cd_cliente"]);
                this.CodigoDoBanco = Convert.ToInt16(oDr["cd_banco"]);
                this.CodigoDaSituacao = Convert.ToInt16(oDr["cd_sitcheque"]);
                this.DataDeVencimento = oDr["dt_vencto"].ToString();
                this.Valor = Convert.ToDecimal(oDr["valor"].ToString().Replace(".",","));
                this.Tipo = (string)oDr["tipo"];
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
        if (this.CodigoDoCheque <= 0)
        {
            this.critica = "Código da Cheque deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Cheque ";
            StrSql += " WHERE   Cheque.cd_cheque = " + this.CodigoDoCheque.ToString();

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
