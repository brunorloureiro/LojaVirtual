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
/// Summary description for AviseMe
/// </summary>
public class AviseMe
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public string CodigoDoProduto = "";
    public string Nome = "";
    public string Email = "";


    public AviseMe(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    
    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            
            StrSql = " INSERT INTO Dispprod (cd_peca, nome, email) ";
            StrSql += " VALUES ('" + this.CodigoDoProduto.Trim().Replace("'", "´") + "'," + 
                                "'" + this.Nome.Trim().Replace("'", "´") + "'," +
                                "'" + this.Email.Trim().Replace("'", "´") + "')";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

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
    
}
