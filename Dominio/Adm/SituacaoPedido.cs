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
/// Summary description for SituacaoPedido
/// </summary>
public class SituacaoPedido
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int Codigo = 0;
    public int Pedido = 0;
    public string Status = "";
    public string Rastreio = "";


    public SituacaoPedido(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Sitpedido";
        string campos = "cd_sitpedido,cd_pedido,status,rastreio";
        string labels = "Código,Nº Pedido,Status,Rastreio";
        string pks = "txtcd_sitpedido";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, true, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.Pedido == 0)
        {
            this.critica = "Nº Pedido deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
           
            StrSql  = " INSERT INTO Sitpedido (cd_pedido, status, rastreio) ";
            StrSql += " VALUES (" + this.Pedido + ",";
            StrSql += "        '" + this.Status.Trim().Replace("'", "´") + "',"; 
            StrSql += "        '" + this.Rastreio.Trim().Replace("'", "´") + "')";
          

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_sitpedido) as cd_sitpedido FROM SitPedido ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.Codigo = Convert.ToInt32(oDr["cd_sitpedido"]);
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

        if (this.Codigo <= 0)
        {
            this.critica = "Código ds Situação do Pedido deve ser informado. Verifique.";
            return true;
        }

        if (this.Pedido.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nº Pedido deve ser informado. Verifique.";
            return true;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
         
            StrSql = "          SELECT  cd_sitpedido ";
            StrSql = StrSql + " FROM    Sitpedido   ";
            StrSql = StrSql + " WHERE   Sitpedido.cd_sitpedido = " + this.Codigo.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Situação do Pedido não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE  Sitpedido Set ";
                StrSql += "        cd_pedido      = " + this.Pedido.ToString() + ",";
                StrSql += "        status         = '" + this.Status.Trim().Replace("'", "´") + "',";
                StrSql += "        rastreio       = '" + this.Rastreio.ToString().Replace("'", "´") + "'";
                StrSql += " WHERE  cd_sitpedido =  " + this.Codigo.ToString();

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

        if (Convert.ToInt32(this.Codigo) <= 0)
        {
            this.critica = "Código da Situação do Pedido deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Sitpedido   ";
            StrSql = StrSql + " WHERE   Sitpedido.cd_sitpedido = " + this.Codigo.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Situação do Pedido não cadastrada. Verifique.";
                Resp = false;
            }
            else
            {
                this.Codigo = Convert.ToInt32(oDr["cd_sitpedido"]);
                this.Pedido = Convert.ToInt32(oDr["cd_pedido"]);
                this.Status = (string)oDr["Status"];
                this.Rastreio = (string)oDr["rastreio"];
                
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

    public bool VerificaPedido()
    {
        bool Resp = true;
        string StrSql = "";

        if (Convert.ToInt32(this.Pedido) <= 0)
        {
            this.critica = "Nº Pedido deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Sitpedido   ";
            StrSql = StrSql + " WHERE   Sitpedido.cd_pedido = " + this.Pedido.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Pedido inexistente. Verifique.";
                Resp = false;
            }
            else
            {
                this.Codigo = Convert.ToInt32(oDr["cd_sitpedido"]);
                this.Pedido = Convert.ToInt32(oDr["cd_pedido"]);
                this.Status = (string)oDr["Status"];
                this.Rastreio = (string)oDr["rastreio"];

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

}
