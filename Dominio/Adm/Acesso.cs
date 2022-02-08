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
/// Summary description for Acesso
/// </summary>
public class Acesso
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";

    public int CodigoDoAcesso = 0;
    public int CodigoDoUsuario = 0;
    public int Gravar = 0;
    public int Excluir = 0;
    public int Consultar = 0;
    public int Entrada = 0;
    public int Baixa = 0;
    public int Retirada = 0;
    public int Importa = 0;
    public int EstoqueNegativo = 0;
    public int ControlaVenda = 0;
    public int ControlaFinanceiro = 0;
    public int ControlaLoja = 0;
   
    public Acesso(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Acesso, Usuario ";
        string campos = "cd_acesso, nm_usuario,bl_grava,bl_exclui,bl_consulta,bl_entrada,bl_baixa,bl_retirada, bl_importa, bl_estneg, bl_contven, bl_financ, bl_loja";
        string labels = "Código, Usuario, Grava, Exclui, Consulta, Entrada, Baixa, Retirada, Importa, Estoque Negativo, Controla Venda, Financeiro, Loja";
        string pks = "txtcd_acesso";
        string cond = " AND Acesso.cd_usuario = Usuario.cd_usuario";
       
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

       
        if (this.CodigoDoUsuario == 0)
        {
            this.critica = "Usuário deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_acesso FROM Acesso WHERE lTrim(rTrim(Upper(cd_usuario))) like " + this.CodigoDoUsuario.ToString();

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
                this.critica = "Já existe acesso para esse usuário. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Acesso (cd_usuario, bl_grava, bl_exclui, bl_consulta, bl_entrada, bl_baixa, bl_retirada, bl_importa, bl_estneg, bl_contven, bl_financ, bl_loja) ";
            StrSql += " VALUES (" + this.CodigoDoUsuario + ",";
            StrSql += "         " + this.Gravar + ",";
            StrSql += "         " + this.Excluir + ",";
            StrSql += "         " + this.Consultar + ",";
            StrSql += "         " + this.Entrada + ",";
            StrSql += "         " + this.Baixa + ",";
            StrSql += "         " + this.Retirada + ",";
            StrSql += "         " + this.Importa + ",";
            StrSql += "         " + this.EstoqueNegativo + ",";
            StrSql += "         " + this.ControlaVenda + ",";
            StrSql += "         " + this.ControlaFinanceiro + ",";
            StrSql += "         " + this.ControlaLoja + ")";

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_acesso) as cd_acesso FROM Acesso ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoAcesso = Convert.ToInt16(oDr["cd_acesso"]);
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

        if (this.CodigoDoAcesso <= 0)
        {
            this.critica = "Código do Acesso deve ser informado. Verifique.";
            return true;
        }

        if (this.CodigoDoUsuario == 0)
        {
            this.critica = "Usuário deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_acesso FROM Acesso WHERE lTrim(rTrim(Upper(cd_usuario))) like " + this.CodigoDoUsuario.ToString() + " AND cd_acesso <> " + this.CodigoDoAcesso.ToString();

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
                this.critica = "Já existe acesso para esse usuário. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_acesso ";
            StrSql = StrSql + " FROM    Acesso   ";
            StrSql = StrSql + " WHERE   Acesso.cd_acesso = " + this.CodigoDoAcesso.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Acesso não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql  = " UPDATE  Acesso Set ";
                StrSql += "         cd_usuario   =  " + this.CodigoDoUsuario.ToString() + ",";
                StrSql += "         bl_grava     =  " + this.Gravar.ToString() + ",";
                StrSql += "         bl_exclui    =  " + this.Excluir.ToString() + ",";
                StrSql += "         bl_consulta  =  " + this.Consultar.ToString() + ",";
                StrSql += "         bl_entrada   =  " + this.Entrada.ToString() + ",";
                StrSql += "         bl_baixa     =  " + this.Baixa.ToString() + ",";
                StrSql += "         bl_retirada  =  " + this.Retirada.ToString() + ",";
                StrSql += "         bl_importa   =  " + this.Importa.ToString() + ",";
                StrSql += "         bl_estneg    =  " + this.EstoqueNegativo.ToString() + ",";
                StrSql += "         bl_contven   =  " + this.ControlaVenda.ToString() + ",";
                StrSql += "         bl_financ    =  " + this.ControlaFinanceiro.ToString() + ",";
                StrSql += "         bl_loja      =  " + this.ControlaLoja.ToString();
                StrSql += " WHERE   cd_acesso    =  " + this.CodigoDoAcesso.ToString();

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

        if (Convert.ToInt16(this.CodigoDoAcesso) <= 0)
        {
            this.critica = "Código do Acesso deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Acesso   ";
            StrSql = StrSql + " WHERE   Acesso.cd_acesso = " + this.CodigoDoAcesso.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Acesso não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoAcesso = Convert.ToInt16(oDr["cd_acesso"]);
                this.CodigoDoUsuario = Convert.ToInt16(oDr["cd_usuario"]);
                this.Gravar = Convert.ToInt16(oDr["bl_grava"]);
                this.Excluir = Convert.ToInt16(oDr["bl_exclui"]);
                this.Consultar = Convert.ToInt16(oDr["bl_consulta"]);
                this.Entrada = Convert.ToInt16(oDr["bl_entrada"]);
                this.Baixa = Convert.ToInt16(oDr["bl_baixa"]);
                this.Retirada = Convert.ToInt16(oDr["bl_retirada"]);
                this.Importa = Convert.ToInt16(oDr["bl_importa"]);
                this.EstoqueNegativo = Convert.ToInt16(oDr["bl_estneg"]);
                this.ControlaVenda = Convert.ToInt16(oDr["bl_contven"]);
                this.ControlaFinanceiro = Convert.ToInt16(oDr["bl_financ"]);
                this.ControlaLoja = Convert.ToInt16(oDr["bl_loja"]);
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

    public bool Exclui()
    {
        if (this.CodigoDoAcesso <= 0)
        {
            this.critica = "Código do Acesso deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {
            StrSql  = " DELETE  FROM Acesso ";
            StrSql += " WHERE   Acesso.cd_acesso = " + this.CodigoDoAcesso.ToString();

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
