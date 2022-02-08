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

public class NotaPromissoria
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDaNotaPromissoria = 0;
    public int CodigoDoCliente = 0;
    public string Situacao = "";
    public string DataDeVencimento = "";
    public decimal Valor = 0;
    public int UsuarioLogado;
    
    public NotaPromissoria(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Notaprom, Cliente  ";
        string campos = "cd_notaprom, nm_cliente, valor, dt_vencto, situacao";
        string labels = "Código,Cliente,Valor,Data de Vencimento, Situação";
        string pks = "txtcd_notaprom";
        string cond = " AND Notaprom.cd_cliente = Cliente.cd_cliente";
        
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

        if (this.CodigoDaNotaPromissoria == 0)
        {
            this.critica = "N° da Nota Promissória deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoCliente == 0)
        {
            this.critica = "Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Vencimento deve ser preenchida. Verifique.";
            return false;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor do Nota Promissória deve ser preenchido. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
          
            StrSql = " SELECT cd_notaprom FROM Notaprom WHERE cd_notaprom = " + this.CodigoDaNotaPromissoria.ToString();

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
                this.critica = "Já existe uma Nota Promissória com o número informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Notaprom (cd_notaprom, cd_cliente, valor, dt_vencto, situacao, cd_usu_log) ";
            StrSql += " VALUES (" + this.CodigoDaNotaPromissoria + ",";
            StrSql += "         " + this.CodigoDoCliente + ",";
            StrSql += "         " + this.Valor.ToString().Replace(",",".") + ",";
            StrSql += "        '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd") + "',";
            StrSql += "        '" + this.Situacao.Trim().Replace("'", "´") + "',";
            StrSql += "         " + this.UsuarioLogado + ")";
         

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_notaprom) as cd_notaprom FROM Notaprom ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDaNotaPromissoria = Convert.ToInt32(oDr["cd_notaprom"]);
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

        if (this.CodigoDaNotaPromissoria == 0)
        {
            this.critica = "N° do Nota Promissória deve ser informado. Verifique.";
            return false;
        }

        if (this.CodigoDoCliente == 0)
        {
            this.critica = "Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.DataDeVencimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Vencimento deve ser preenchida. Verifique.";
            return false;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor do Nota Promissória deve ser preenchido. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {

            StrSql = "          SELECT  cd_notaprom ";
            StrSql = StrSql + " FROM    Notaprom   ";
            StrSql = StrSql + " WHERE   Notaprom.cd_notaprom = " + this.CodigoDaNotaPromissoria.ToString();

            oCmd.Connection = ClsPublico.oConn;
            //*********************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Nota Promissória não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Notaprom Set ";
                StrSql += "         cd_notaprom    =  " + this.CodigoDaNotaPromissoria.ToString() + ",";
                StrSql += "         cd_cliente   =  " + this.CodigoDoCliente.ToString() + ",";
                StrSql += "         dt_vencto    = '" + Convert.ToDateTime(this.DataDeVencimento).ToString("yyyy/MM/dd HH:mm:ss") + "',";
                StrSql += "         valor        =  " + this.Valor.ToString().Replace(",", ".") + ",";
                StrSql += "         situacao     = '" + this.Situacao.Trim().Replace("'", "´") + "',";
                StrSql += "         cd_usu_log   =  " + this.UsuarioLogado.ToString();
                StrSql += " WHERE   cd_notaprom  =  " + this.CodigoDaNotaPromissoria.ToString();

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

        if (Convert.ToInt32(this.CodigoDaNotaPromissoria) <= 0)
        {
            this.critica = "Código da Nota Promissória deve ser informada. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Notaprom   ";
            StrSql = StrSql + " WHERE   Notaprom.cd_notaprom = " + this.CodigoDaNotaPromissoria.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Nota Promissória não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                
                this.CodigoDaNotaPromissoria = Convert.ToInt32(oDr["cd_notaprom"]);
                this.CodigoDoCliente = Convert.ToInt32(oDr["cd_cliente"]);
                this.DataDeVencimento = oDr["dt_vencto"].ToString();
                this.Valor = Convert.ToDecimal(oDr["valor"].ToString().Replace(".",","));
                this.Situacao = (string)oDr["situacao"];
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
        if (this.CodigoDaNotaPromissoria <= 0)
        {
            this.critica = "Código da Nota Promissória deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Notaprom ";
            StrSql += " WHERE   Notaprom.cd_notaprom = " + this.CodigoDaNotaPromissoria.ToString();

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
