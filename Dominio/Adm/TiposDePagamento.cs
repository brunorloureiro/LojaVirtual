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
/// Summary description for TiposDePagamento
/// </summary>
public class TiposDePagamento
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoTipoDePagamento = 0;
    public string NomeDoTipoDePagamento = "";
    public int NumeroDeParcelas = 0;
    

    public TiposDePagamento(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Tppagto";
        string campos = "cd_tppagto,nm_tppagto,qt_vezes";
        string labels = "Código,Nome,N° de Parcelas ";
        string pks = "txtcd_tppagto";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoTipoDePagamento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Pagamento deve ser informado. Verifique.";
            return false;
        }

        if (this.NumeroDeParcelas == 0)
        {
            this.critica = "N° de Parcelas deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tppagto FROM Tppagto WHERE lTrim(rTrim(Upper(nm_tppagto))) like '" + this.NomeDoTipoDePagamento.Trim().ToUpper() + "' AND qt_vezes = " + this.NumeroDeParcelas.ToString();

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
                this.critica = "Já existe tipo de pagamento com o nome informado e com número de parcelas iguais. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Tppagto (nm_tppagto,qt_vezes) ";
            StrSql += " VALUES ('" + this.NomeDoTipoDePagamento.Trim().Replace("'", "´")  + "',";
            StrSql += "          " + this.NumeroDeParcelas + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_tppagto) as cd_tppagto FROM Tppagto ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoTipoDePagamento = Convert.ToInt16(oDr["cd_tppagto"]);
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

        if (this.CodigoDoTipoDePagamento <= 0)
        {
            this.critica = "Código do Tipo de Pagamento deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoTipoDePagamento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Tipo de Pagamento deve ser informado. Verifique.";
            return false;
        }

        if (this.NumeroDeParcelas == 0)
        {
            this.critica = "N° de Parcelas deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_tppagto FROM Tppagto WHERE lTrim(rTrim(Upper(nm_tppagto))) like '" + this.NomeDoTipoDePagamento.Trim().ToUpper() + "' AND qt_vezes = " + this.NumeroDeParcelas.ToString() + " AND cd_tppagto <> " + this.CodigoDoTipoDePagamento.ToString();

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
                this.critica = "Já existe tipo de pagamento com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_tppagto ";
            StrSql = StrSql + " FROM    Tppagto   ";
            StrSql = StrSql + " WHERE   Tppagto.cd_tppagto = " + this.CodigoDoTipoDePagamento.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Tipo de Pagamento não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Tppagto Set ";
                StrSql += "         nm_tppagto   = '" + this.NomeDoTipoDePagamento.Trim().Replace("'", "´") + "',";
                StrSql += "         qt_vezes     =  " + this.NumeroDeParcelas.ToString();
                StrSql += " WHERE   cd_tppagto   =  " + this.CodigoDoTipoDePagamento.ToString();

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

        if (Convert.ToInt16(this.CodigoDoTipoDePagamento) <= 0)
        {
            this.critica = "Código do Tipo de Pagamento deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tppagto   ";
            StrSql = StrSql + " WHERE   Tppagto.cd_tppagto = " + this.CodigoDoTipoDePagamento.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Tipo de Pagamento não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoTipoDePagamento = Convert.ToInt16(oDr["cd_tppagto"]);
                this.NomeDoTipoDePagamento = (string)oDr["nm_tppagto"];
                this.NumeroDeParcelas = Convert.ToInt16(oDr["qt_vezes"]);
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
        if (this.CodigoDoTipoDePagamento <= 0)
        {
            this.critica = "Código do Tipo de Pagamento deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Tppagto ";
            StrSql += " WHERE   Tppagto.cd_tppagto = " + this.CodigoDoTipoDePagamento.ToString();

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
