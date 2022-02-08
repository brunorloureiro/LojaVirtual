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
/// Summary description for Cliente
/// </summary>
public class Cliente
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";
    public string criticaaux = "";


    public int CodigoDoCliente = 0;
    public string NomeDoCliente = "";
    public string Login = "";
    public string Senha = "";
    public string ContraSenha = "";
    public Int64 CPF = 0;
    public string RG = "";
    public string Email = "";
    public string Sexo = "";
    public string EstadoCivil = "";
    public int CEP = 0;
    public string Endereco = "";
    public int Numero = 0;
    public string Bairro = "";
    public string Cidade = "";
    public string UF = "";
    public string Complemento = "";
    public string DDDResidencial = "";
    public int TelefoneResidencial = 0;
    public string DDDComercial = "";
    public int TelefoneComercial = 0;
    public string DDDCelular = "";
    public int TelefoneCelular = 0;
    public string Profissao = "";
    public int ReceberNoticias = 0;
    public string DataDeNascimento = "";
    public DateTime dh_ult_atz = DateTime.Today;
    
       
    public Cliente(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Cliente";
        string campos = "cd_cliente,nm_cliente,login, cpf, email";
        string labels = "Código,Nome,Login, CPF, E-mail";
        string pks = "txtcd_cliente";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, true, true);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoCliente.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.Login.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Login do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.RG.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "RG do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.Email.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "E-mail do Cliente deve ser informado. Verifique.";
            return false;
        }
      
        if (!ClsPublico.verificaEmail(this.Email))
        {
            this.critica = "E-mail inválido. Verifique.";
            return false;
        }
      
        if (this.DataDeNascimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Nascimento do Cliente deve ser informada. Verifique.";
            return false;
        }

        if (!ClsPublico.validaData(this.DataDeNascimento))
        {
            this.critica = "Data de Nascimento inválida. Verifique.";
            return false;
        }

        if (this.Senha.ToString().Trim().Replace("'", "´") == "" || this.Senha.ToString().Trim().Replace("'", "´") != this.ContraSenha.ToString().Trim().Replace("'", "´"))
        {
            this.critica = "Senha e Contra-Senha devem ser informada e igualadas. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_cliente FROM Cliente WHERE lTrim(rTrim(Upper(login))) like '" + this.Login.Trim().ToUpper() + "'";

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
                this.critica = "Já existe cliente com o login informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = " INSERT INTO Cliente (nm_cliente, login, senha, cpf, rg, email, sexo, est_civil, cep, endereco, numero, bairro, cidade, uf, complemento, ddd_resid, tel_resid, ddd_com, tel_com, ddd_cel, tel_cel, profissao, bl_noticia, dt_nascim) ";
            StrSql += " VALUES ('" + this.NomeDoCliente.Trim().Replace("'", "´") + "'," +
                                "'" + this.Login.Trim().Replace("'", "´") + "'," +
                                "'" + this.Senha.Trim().Replace("'", "´") + "'," +
                                    + this.CPF + "," +
                                "'" + this.RG.Trim().Replace("'", "´") + "'," +
                                "'" + this.Email.Trim().Replace("'", "´") + "'," +
                                "'" + this.Sexo.Trim().Replace("'", "´") + "'," +
                                "'" + this.EstadoCivil.Trim().Replace("'", "´") + "'," +
                                    + this.CEP + "," +
                                "'" + this.Endereco.Trim().Replace("'", "´") + "'," +
                                    + this.Numero + "," +
                                "'" + this.Bairro.Trim().Replace("'", "´") + "'," +
                                "'" + this.Cidade.Trim().Replace("'", "´") + "'," +
                                "'" + this.UF.Trim().Replace("'", "´") + "'," +
                                "'" + this.Complemento.Trim().Replace("'", "´") + "'," +
                                "'" + this.DDDResidencial.Trim().Replace("'", "´") + "'," +
                                    + this.TelefoneResidencial + "," +
                                "'" + this.DDDComercial.Trim().Replace("'", "´") + "'," +
                                    + this.TelefoneComercial + "," +
                                "'" + this.DDDCelular.Trim().Replace("'", "´") + "'," +
                                    + this.TelefoneCelular + "," +
                                "'" + this.Profissao.Trim().Replace("'", "´") + "'," +
                                    + this.ReceberNoticias + "," +
                                    "'" + Convert.ToDateTime(this.DataDeNascimento).ToString("yyyy/MM/dd") + "')";
 
            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_cliente) as cd_cliente FROM Cliente ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoCliente = Convert.ToInt16(oDr["cd_cliente"]);
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

        if (this.CodigoDoCliente <= 0)
        {
            this.critica = "Código do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.NomeDoCliente.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.RG.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "RG do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (this.Email.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "E-mail do Cliente deve ser informado. Verifique.";
            return false;
        }

        if (!ClsPublico.verificaEmail(this.Email))
        {
            this.critica = "E-mail inválido. Verifique.";
            return false;
        }

        if (this.DataDeNascimento.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Data de Nascimento do Cliente deve ser informada. Verifique.";
            return false;
        }

        if (!ClsPublico.validaData(this.DataDeNascimento))
        {
            this.critica = "Data de Nascimento inválida. Verifique";
            return false;
        }

        if (this.Senha.ToString().Trim().Replace("'", "´") == "" || this.Senha.ToString().Trim().Replace("'", "´") != this.ContraSenha.ToString().Trim().Replace("'", "´"))
        {
            this.critica = "Senha e Contra-Senha devem ser informada e igualadas. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_cliente FROM Cliente WHERE lTrim(rTrim(Upper(login))) like '" + this.Login.Trim().ToUpper() + "' AND cd_cliente <> " + this.CodigoDoCliente.ToString();

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
                this.critica = "Já existe cliente com o login informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_cliente ";
            StrSql = StrSql + " FROM    Cliente   ";
            StrSql = StrSql + " WHERE   Cliente.cd_cliente = " + this.CodigoDoCliente.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Cliente não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                    StrSql = " UPDATE   Cliente Set ";
                    StrSql += "         nm_cliente   = '" + this.NomeDoCliente.Trim().Replace("'", "´") + "',";
                    StrSql += "         login        = '" + this.Login.Trim().Replace("'", "´") + "',";
                    StrSql += "         senha        = '" + this.Senha.Trim().Replace("'", "´") + "',";
                    StrSql += "         cpf          =  " + this.CPF.ToString() + ",";
                    StrSql += "         rg           = '" + this.RG.Trim().Replace("'", "´") + "',";
                    StrSql += "         email        = '" + this.Email.Trim().Replace("'", "´") + "',";
                    StrSql += "         sexo         = '" + this.Sexo.Trim().Replace("'", "´") + "',";
                    StrSql += "         est_civil    = '" + this.EstadoCivil.Trim().Replace("'", "´") + "',";
                    StrSql += "         cep          =  " + this.CEP.ToString() + ",";
                    StrSql += "         endereco     = '" + this.Endereco.Trim().Replace("'", "´") + "',";
                    StrSql += "         numero       =  " + this.Numero.ToString() + ",";
                    StrSql += "         bairro       = '" + this.Bairro.Trim().Replace("'", "´") + "',";
                    StrSql += "         cidade       = '" + this.Cidade.Trim().Replace("'", "´") + "',";
                    StrSql += "         uf           = '" + this.UF.Trim().Replace("'", "´") + "',";
                    StrSql += "         complemento  = '" + this.Complemento.Trim().Replace("'", "´") + "',";
                    StrSql += "         ddd_resid    = '" + this.DDDResidencial.Trim().Replace("'", "´") + "',";
                    StrSql += "         tel_resid    =  " + this.TelefoneResidencial.ToString() + ",";
                    StrSql += "         ddd_com      = '" + this.DDDComercial.Trim().Replace("'", "´") + "',";
                    StrSql += "         tel_com      =  " + this.TelefoneComercial.ToString() + ",";
                    StrSql += "         ddd_cel      = '" + this.DDDCelular.Trim().Replace("'", "´") + "',";
                    StrSql += "         tel_cel      =  " + this.TelefoneCelular.ToString() + ",";
                    StrSql += "         profissao    = '" + this.Profissao.Trim().Replace("'", "´") + "',";
                    StrSql += "         bl_noticia   =  " + this.ReceberNoticias.ToString() + ",";
                    StrSql += "         dt_nascim    = '" + Convert.ToDateTime(this.DataDeNascimento).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                    StrSql += " WHERE   cd_cliente   =  " + this.CodigoDoCliente.ToString();

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

        if (Convert.ToInt16(this.CodigoDoCliente) <= 0)
        {
            this.critica = "Código do Cliente deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Cliente   ";
            StrSql = StrSql + " WHERE   Cliente.cd_cliente = " + this.CodigoDoCliente.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Cliente não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoCliente = Convert.ToInt16(oDr["cd_cliente"]);
                this.NomeDoCliente = (string)oDr["nm_cliente"];
                this.Login = (string)oDr["login"];
                this.Senha = (string)oDr["senha"];
                this.CPF = Convert.ToInt64(oDr["cpf"]);
                this.RG = (string)oDr["rg"];
                this.Email = (string)oDr["email"];
                this.Sexo = (string)oDr["sexo"];
                this.EstadoCivil = (string)oDr["est_civil"];
                this.CEP = Convert.ToInt32(oDr["cep"]);
                this.Endereco = (string)oDr["endereco"];
                this.Numero = Convert.ToInt32(oDr["numero"]);
                this.Bairro = (string)oDr["bairro"];
                this.Cidade = (string)oDr["cidade"];
                this.UF = (string)oDr["uf"];
                this.Complemento = (string)oDr["complemento"];
                this.DDDResidencial = (string)oDr["ddd_resid"];
                this.TelefoneResidencial = Convert.ToInt32(oDr["tel_resid"]);
                this.DDDComercial = (string)oDr["ddd_com"];
                this.TelefoneComercial = Convert.ToInt32(oDr["tel_com"]);
                this.DDDCelular = (string)oDr["ddd_cel"];
                this.TelefoneCelular = Convert.ToInt32(oDr["tel_cel"]);
                this.Profissao = (string)oDr["profissao"];
                this.ReceberNoticias = Convert.ToInt16(oDr["bl_noticia"]);
                this.DataDeNascimento = oDr["dt_nascim"].ToString();

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
        if (this.CodigoDoCliente <= 0)
        {
            this.critica = "Código do Cliente deve ser informado. Verifique.";
            return false;
        }

        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************

        try
        {

            StrSql = " SELECT * FROM Notaprom WHERE cd_cliente = " + this.CodigoDoCliente.ToString();

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
                this.critica = "Não é possível a exclusão deste cliente pois já existe(m) Nota(s) Promissória(s) relacionada(s) ao mesmo. Operação Cancelada.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " DELETE  FROM Cliente ";
            StrSql += " WHERE   Cliente.cd_cliente = " + this.CodigoDoCliente.ToString();

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
