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
/// Summary description for MotoboyxBairro
/// </summary>
public class MotoxBairro
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int Codigo = 0;
    public string Bairro = "";
    public string Cidade = "";
    public decimal Valor = 0;


    public MotoxBairro(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Motoxbai";
        string campos = "cd_motoxbai,bairro,cidade,valor";
        string labels = "Código,Bairro,Cidade,Valor";
        string pks = "txtcd_motoxbai";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.Bairro.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Bairro deve ser informado. Verifique.";
            return false;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor da Entrega deve ser preenchido. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_motoxbai FROM Motoxbai WHERE lTrim(rTrim(Upper(bairro))) like '" + this.Bairro.Trim().ToUpper() + "' AND lTrim(rTrim(Upper(cidade))) like '" + this.Cidade.Trim() + "'" ;

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
                this.critica = "Já existe bairro para a mesma cidade com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Motoxbai (bairro, cidade, valor) ";
            StrSql += " VALUES ('" + this.Bairro.Trim().Replace("'", "´") + "',";
            StrSql += "        '" + this.Cidade.Trim().Replace("'", "´") + "',"; 
            StrSql += "         " + this.Valor.ToString().Replace(",", ".") + ")";
          

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_motoxBai) as cd_motoxbai FROM Motoxbai ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.Codigo = Convert.ToInt16(oDr["cd_motoxbai"]);
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
            this.critica = "Código do Relacionamento deve ser informado. Verifique.";
            return true;
        }

        if (this.Bairro.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Bairro deve ser informado. Verifique.";
            return true;
        }

        if (this.Valor == 0)
        {
            this.critica = "Valor da Entrega deve ser preenchido. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_motoxbai FROM Motoxbai WHERE lTrim(rTrim(Upper(bairro))) like '" + this.Bairro.Trim().ToUpper() + "' AND lTrim(rTrim(Upper(cidade))) like '" + this.Cidade.Trim() + "'AND cd_motoxbai <> " + this.Codigo.ToString();
           
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
                this.critica = "Já existe bairro para a mesma cidade com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_motoxbai ";
            StrSql = StrSql + " FROM    Motoxbai   ";
            StrSql = StrSql + " WHERE   Motoxbai.cd_motoxbai = " + this.Codigo.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Relacionamento Motoboy x Bairro não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql = " UPDATE  Motoxbai Set ";
                StrSql += "        bairro      = '" + this.Bairro.Trim().Replace("'", "´") + "',";
                StrSql += "        cidade      = '" + this.Cidade.Trim().Replace("'", "´") + "',";
                StrSql += "        valor       =  " + this.Valor.ToString().Replace(",", ".") + "";
                StrSql += " WHERE  cd_motoxbai =  " + this.Codigo.ToString();

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

        if (Convert.ToInt16(this.Codigo) <= 0)
        {
            this.critica = "Código do Relacionamento deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Motoxbai   ";
            StrSql = StrSql + " WHERE   Motoxbai.cd_motoxbai = " + this.Codigo.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Relacionamento Motoboy x Bairro não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.Codigo = Convert.ToInt16(oDr["cd_motoxbai"]);
                this.Bairro = (string)oDr["bairro"];
                this.Cidade = (string)oDr["cidade"]; 
                this.Valor = Convert.ToDecimal(oDr["valor"].ToString().Replace(".", ","));
                
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
        if (this.Codigo <= 0)
        {
            this.critica = "Código do Relacionamento deve ser informado. Verifique.";
            return false;
        }
        string StrSql = "";
        bool Resp = true;
        

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
           
            StrSql  = " DELETE  FROM Motoxbai ";
            StrSql += " WHERE   Motoxbai.cd_motoxbai = " + this.Codigo.ToString();

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
