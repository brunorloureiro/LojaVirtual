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


public class Loja
{
    private Publico.Publico ClsPublico = new Publico.Publico();
    private OdbcCommand oCmd = new OdbcCommand();
    private OdbcDataReader oDr;

    public string critica = "";
    public string crit_adm = "";

    public int CodigoDaLoja = 0;
    public string NomeDaLoja = "";
    public string Senha = "";
    public string ContraSenha = "";
    public int Administrador = 0;
    public int Ativo = 0;
    public int ClienteLogado = 0;
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

    public Loja(string StrConn)
    {
        ClsPublico.StrConexao = StrConn.ToString();
    }

    public string TrazGrid()
    {
        string tabela = "Loja";
        string campos = "cd_loja,nm_loja,senha, bl_ativo";
        string labels = "Código,Nome,Senha,Ativo?";
        string pks = "txtcd_loja";
        string cond = "";
        return ClsPublico.Grid(tabela, campos, labels, pks, cond, false, true);
    }

    public string TrazMenu()
    {
        // 1 - Administrativo
        // 2 - Loja Virtual
        // 3 - Clientes
        string modulo = "2";
        return ClsPublico.CarregaMenu(modulo);
    }

    public string TrazSubMenu()
    {
        // 1 - Administrativo
        // 2 - Loja Virtual
        // 3 - Clientes
        string modulo = "2";
        return ClsPublico.CarregaSubMenu(modulo);
    }

    public string CarregaPrincipal()
    {
        return ClsPublico.CarregaPaginaPrincipal();
    }

    public string CarregaPromocoes()
    {
        return ClsPublico.CarregaPaginaPromocoes();
    }

    public string CarregaProduto(string p_cd_produto)
    {
        return ClsPublico.CarregaInfoProduto(p_cd_produto);
    }

    public string TrazResultadoBuscaSimples(string descricao)
    {
        return ClsPublico.CarregaDadosBuscaSimples(descricao);
    }

    public string TrazResultadoBuscaAvancada(string descricao, string tipo, string precoini, string precofim)
    {
        return ClsPublico.CarregaDadosBuscaAvancada(descricao, tipo, precoini, precofim);
    }
     
    public string TrazInfoCarrinho(string produto, string campo)
    {
        return ClsPublico.CarregaInfoCarrinho(produto, campo);
    }

    public bool TrazNumeroDeRegistros()
    {
        return Publico.Publico.registros;
    }

    public bool TrazNumeroDeRegistrosDaBusca()
    {
        return Publico.Publico.registrosbusca;
    }

    public string CarregaMostraProdutos(string p_produto, int p_genero)
    {

        return ClsPublico.CarregaPaginaProdutos(p_produto, p_genero);
    }

    public bool FazLoginCliente(string p_cliente, string p_senha)
    {
        bool Resp = true;
        string StrSql = "";

        //*************************************************************************************
        if (!ClsPublico.AbreConexao()) { this.critica = ClsPublico.critica; return false; }
        //*************************************************************************************
        try
        {
            StrSql += " SELECT  cd_cliente, bl_ativo ";
            StrSql += " FROM    Cliente ";
            StrSql += " WHERE   nm_cliente          LIKE '" + p_cliente.ToString().Trim() + "'";
            StrSql += " AND     ltrim(rtrim(senha)) =    '" + p_senha.ToString().Trim() + "'";

            oCmd.Connection = ClsPublico.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (!oDr.Read())
            {
                this.critica = "Cliente ou senha inválida. Verifique.";
                Resp = false;
            }
            else
            {
                this.ClienteLogado = Convert.ToInt32(oDr["cd_cliente"]);

                if (Convert.ToInt16(oDr["bl_ativo"]) == 0)
                {
                    this.critica = "Cliente desativado. Entre em contato conosco.";
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
            this.critica = "Cliente ou senha inválida. Verifique.";
            Resp = false;
        }

        oDr.Close();

        //**************************************************************************************
        if (!ClsPublico.FechaConexao()) { this.critica = ClsPublico.critica; return false; }
        //**************************************************************************************

        return Resp;
        //**********
    }
}
