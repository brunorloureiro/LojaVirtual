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


public class Usuario
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";
    public string crit_adm = "";

    public int CodigoDoUsuario = 0;
    public string NomeDoUsuario = "";
    public string Senha = "";
    public string ContraSenha = "";
    public int Administrador = 0;
    public int Ativo = 0;
    public int UsuarioLogado = 0;
    public bool ControlaLoja = false;
    public bool ControlaFinanceiro = false;
    public bool ControlaVenda = false;
    public bool EstoqueNegativo = false;
    public bool PermiteImportar = false;
    public bool PermiteRetirada = false;
    public bool PermiteEntrada = false;
    public bool PermiteBaixa = false;
    public bool PermiteGravar = false;
    public bool PermiteExcluir = false;
    public bool PermiteConsultar = false;
    public DateTime dh_ult_atz = DateTime.Today;

    public Usuario(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Usuario";
        string campos = "cd_usuario,nm_usuario,senha, bl_ativo";
        string labels = "Código,Nome,Senha,Ativo?";
        string pks = "txtcd_usuario";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
      }

    public string TrazMenu()
    {
        // 1 - Administrativo
        // 2 - Loja Virtual
        // 3 - Clientes
        string modulo = "1";
        return ClsPublico.CarregaMenu(modulo);
    }
    public string TrazSubMenu()
    {
        // 1 - Administrativo
        // 2 - Loja Virtual
        // 3 - Clientes
        string modulo = "1";
        return ClsPublico.CarregaSubMenu(modulo);
    }

    public bool Grava()
    {
        bool Resp = true;
        string StrSql = "";

        if (this.NomeDoUsuario.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Usuário deve ser informado. Verifique.";
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
            StrSql = " SELECT cd_usuario FROM Usuario WHERE lTrim(rTrim(Upper(nm_usuario))) like '" + this.NomeDoUsuario.Trim().ToUpper() + "'";

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
                this.critica = "Já existe usuário com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql  = " INSERT INTO Usuario (nm_usuario, senha, bl_adm, bl_ativo) ";
            StrSql += " VALUES ('" + this.NomeDoUsuario.Trim().Replace("'", "´") + "'," +
                               "'" + this.Senha.Trim().Replace("'", "´") + "'," +
                                   + this.Administrador + "," +
                                   + this.Ativo + ")";
            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oCmd.ExecuteNonQuery();
            //*********************

            StrSql = " SELECT Max(cd_usuario) as cd_usuario FROM Usuario ";

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            oDr.Read();
            //*********
            this.CodigoDoUsuario = Convert.ToInt16(oDr["cd_usuario"]);
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

        if (this.CodigoDoUsuario <= 0)
        {
            this.critica = "Código do Usuário deve ser informado. Verifique.";
            return true;
        }

        if (this.NomeDoUsuario.ToString().Trim().Replace("'", "´").Length == 0)
        {
            this.critica = "Nome do Usuário deve ser informado. Verifique.";
            return true;
        }

        if (this.Senha.ToString().Trim().Replace("'", "´") == "" || this.Senha.ToString().Trim().Replace("'", "´") != this.ContraSenha.ToString().Trim().Replace("'", "´"))
        {
            this.critica = "Senha e Contra-Senha devem ser informada e igualadas. Verifique.";
            return true;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = " SELECT cd_usuario FROM Usuario WHERE lTrim(rTrim(Upper(nm_usuario))) like '" + this.NomeDoUsuario.Trim().ToUpper() + "' AND cd_usuario <> " + this.CodigoDoUsuario.ToString();

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
                this.critica = "Já existe usuário com o nome informado. Verifique.";
                return false;
            }
            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  cd_usuario ";
            StrSql = StrSql + " FROM    Usuario   ";
            StrSql = StrSql + " WHERE   Usuario.cd_usuario = " + this.CodigoDoUsuario.ToString();

            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                oDr.Close();
                this.critica = "Usuário não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                oDr.Close();
                StrSql =  " UPDATE  Usuario Set ";
                StrSql += "         nm_usuario   = '" + this.NomeDoUsuario.Trim().Replace("'", "´") + "',";
                StrSql += "         senha        = '" + this.Senha.Trim().Replace("'", "´") + "',";
                StrSql += "         bl_adm       =  " + this.Administrador.ToString() + ",";
                StrSql += "         bl_ativo     =  " + this.Ativo.ToString();
                StrSql += " WHERE   cd_usuario   =  " + this.CodigoDoUsuario.ToString();

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

        if (Convert.ToInt16(this.CodigoDoUsuario) <= 0)
        {
            this.critica = "Código do Usuário deve ser informado. Verifique.";
            return false;
        }

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Usuario   ";
            StrSql = StrSql + " WHERE   Usuario.cd_usuario = " + this.CodigoDoUsuario.ToString();

            oCmd.Connection = ClsPublico.oConn;
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Usuario não cadastrado. Verifique.";
                Resp = false;
            }
            else
            {
                this.CodigoDoUsuario = Convert.ToInt16(oDr["cd_usuario"]);
                this.NomeDoUsuario = (string)oDr["nm_usuario"];
                this.Senha = (string)oDr["senha"];
                this.Administrador = Convert.ToInt16(oDr["bl_adm"]);
                this.Ativo = Convert.ToInt16(oDr["bl_ativo"]);
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
        if (this.CodigoDoUsuario <= 0)
        {
            this.critica = "Código do Usuário deve ser informado. Verifique.";
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
            StrSql += " WHERE   Acesso.cd_usuario = " + this.CodigoDoUsuario.ToString();

            this.oCmd.Connection = ClsPublico.oConn;
            //*************************************
            this.oCmd.CommandText = StrSql;
            this.oCmd.ExecuteNonQuery();
            //***************************

            StrSql  = " DELETE  FROM Usuario ";
            StrSql += " WHERE   Usuario.cd_usuario = " + this.CodigoDoUsuario.ToString();

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

    public bool FazLogin(string p_usuario, string p_senha)
    {
        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql += " SELECT  cd_usuario, bl_adm, bl_ativo ";
            StrSql += " FROM    Usuario ";
            StrSql += " WHERE   nm_usuario          LIKE '" + p_usuario.ToString().Trim() + "'";
            StrSql += " AND     ltrim(rtrim(senha)) =    '" + p_senha.ToString().Trim() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Usuario ou senha inválido. Verifique.";
                Resp = false;
            }
            else
            {
                this.UsuarioLogado = Convert.ToInt32(oDr["cd_usuario"]);

                if (Convert.ToInt16(oDr["bl_adm"]) == 1)
                {
                    this.crit_adm = "Administrador";
                    Resp = true;
                }
            
                if (Convert.ToInt16(oDr["bl_ativo"]) == 0)
                {
                    this.critica = "Usuário desativado. Entre em contato com o Administrador do Sistema.";
                    Resp = false;
                }
                else
                {
                    Resp = true;
                }
            }
        }
        catch (Exception Err)
        {
            this.critica = "Usuario ou senha inválido. Verifique.";
            Resp = false;
        }

        oDr.Close();

        StrSql  = " SELECT  Acesso.* ";
        StrSql += " FROM    Acesso, Usuario ";
        StrSql += " WHERE   Acesso.cd_usuario = Usuario.cd_usuario";
        StrSql += " AND     Usuario.nm_usuario  LIKE '" + p_usuario.ToString().Trim() + "'";
       
        
        oCmd.Connection = ClsPublico.oConn;
        //*************************************
        oCmd.CommandText = StrSql;
        oDr = oCmd.ExecuteReader();
        //*************************

        if (oDr.Read())
        {
            if (Convert.ToInt16(oDr["bl_loja"]) == 1)
            {
                this.ControlaLoja = true;
            }
            else
            {
                this.ControlaLoja = false;
            }

            if (Convert.ToInt16(oDr["bl_financ"]) == 1)
            {
                this.ControlaFinanceiro = true;
            }
            else
            {
                this.ControlaFinanceiro = false;
            }

            if (Convert.ToInt16(oDr["bl_contven"]) == 1)
            {
                this.ControlaVenda = true;
            }
            else
            {
                this.ControlaVenda = false;
            }

            if (Convert.ToInt16(oDr["bl_estneg"]) == 1)
            {
                this.EstoqueNegativo = true;
            }
            else
            {
                this.EstoqueNegativo = false;
            }

            if (Convert.ToInt16(oDr["bl_importa"]) == 1)
            {
                this.PermiteImportar = true;
            }
            else
            {
                this.PermiteImportar = false;
            }

            if (Convert.ToInt16(oDr["bl_retirada"]) == 1)
            {
                this.PermiteRetirada = true;
            }
            else
            {
                this.PermiteRetirada = false;
            }

            if (Convert.ToInt16(oDr["bl_entrada"]) == 1)
            {
                this.PermiteEntrada = true;
            }
            else
            {
                this.PermiteEntrada = false;
            }

            if (Convert.ToInt16(oDr["bl_baixa"]) == 1)
            {
                this.PermiteBaixa = true;
            }
            else
            {
                this.PermiteBaixa = false;
            }

            if (Convert.ToInt16(oDr["bl_consulta"]) == 1)
            {
                this.PermiteConsultar = true;
            }
            else
            {
                this.PermiteConsultar = false;
            }

            if (Convert.ToInt16(oDr["bl_exclui"]) == 1)
            {
                this.PermiteExcluir = true;
            }
            else
            {
                this.PermiteExcluir = false;
            }

            if (Convert.ToInt16(oDr["bl_grava"]) == 1)
            {
                this.PermiteGravar = true;
            }
            else
            {
                this.PermiteGravar = false;
            }
        }

        oDr.Close();

        //**************************************************************************************
        if (!ClsPublico.FechaConexao()) { this.critica = ClsPublico.critica; return false; }
        //**************************************************************************************

        return Resp;
        //**********
    }
  
}
