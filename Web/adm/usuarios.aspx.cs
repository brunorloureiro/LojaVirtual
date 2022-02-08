using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class usuarios : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.usuario.Visible = false;
        }
        else
        {
            this.usuario.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Usuario ClsUsuario = new Usuario(Application["StrConexao"].ToString());

            lblGrid.Text = ClsUsuario.TrazGrid();
            this.lblMsg.Text = "Gerenciamento de usuários da Área Administrativa.";
        }
    }

    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Usuario ClsUsuario = new Usuario(Application["StrConexao"].ToString());
        ClsUsuario.CodigoDoUsuario = Convert.ToInt16(this.txtcd_usuario.Text.ToString());
        ClsUsuario.NomeDoUsuario = this.txtnm_usuario.Valor.ToString().Trim();
        ClsUsuario.Senha = this.txtsenha.Text.ToString().Trim();
        ClsUsuario.ContraSenha = this.txtcontrasenha.Text.ToString().Trim();
        ClsUsuario.Administrador = Convert.ToInt16(this.chkadm.Checked);
        ClsUsuario.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsUsuario.Atualizar();
        //**************************

        if (ClsUsuario.critica != "")
        {
            Mensagem(ClsUsuario.critica.ToString());
        }
        lblGrid.Text = ClsUsuario.TrazGrid();

        if (resp)
        {
            this.btn_atualizar.Enabled = resp;
            this.btn_salvar.Enabled = !resp;
        }
        else
        {
            this.btn_atualizar.Enabled = !resp;
            this.btn_salvar.Enabled = resp;
        }
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

   
    public void novo(object sender, EventArgs e)
    {
        this.NovoRegistro();
        this.lblMsg.Text = "Gerenciamento de usuários da Área Administrativa.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_usuario.Text.Trim() != "")
        {
            if (this.txtcd_usuario.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Usuario ClsUsuario = new Usuario(Application["StrConexao"].ToString());

        ClsUsuario.NomeDoUsuario = this.txtnm_usuario.Valor.ToString().Trim();
        ClsUsuario.Senha = this.txtsenha.Text.ToString().Trim();
        ClsUsuario.ContraSenha = this.txtcontrasenha.Text.ToString().Trim();
        ClsUsuario.Administrador = Convert.ToInt16(this.chkadm.Checked);
        ClsUsuario.Ativo = Convert.ToInt16(this.chkativo.Checked);
       
        resp = ClsUsuario.Grava();
        //*********************
        txtcd_usuario.Text = ClsUsuario.CodigoDoUsuario.ToString();

        if (ClsUsuario.critica != "")
        {
            Mensagem(ClsUsuario.critica.ToString());
        }
        lblGrid.Text = ClsUsuario.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Usuario ClsUsuario = new Usuario(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsUsuario.CodigoDoUsuario = Convert.ToInt16(this.txtcd_usuario.Text.ToString());

        resp = ClsUsuario.Consulta();
        //************************
        txtcd_usuario.Text = ClsUsuario.CodigoDoUsuario.ToString();
        txtnm_usuario.Valor = ClsUsuario.NomeDoUsuario.Trim();
        txtsenha.Text = ClsUsuario.Senha.Trim();
        txtcontrasenha.Text = ClsUsuario.Senha.Trim();
        chkadm.Checked = ClsUsuario.Administrador == 1 ? true : false;
        chkativo.Checked = ClsUsuario.Ativo == 1 ? true : false; 
       
        if (ClsUsuario.critica != "")
        {
            Mensagem(ClsUsuario.critica.ToString());
        }
        lblGrid.Text = ClsUsuario.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        this.txtnm_usuario.Valor = "";
        this.txtsenha.Text = "";
        this.txtcontrasenha.Text = "";
        this.chkadm.Checked = false;
        this.chkativo.Checked = false; 
    }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Usuario ClsUsuario = new Usuario(Application["StrConexao"].ToString());

        ClsUsuario.CodigoDoUsuario = Convert.ToInt16(this.txtcd_usuario.Text.ToString());

        resp = ClsUsuario.Excluir();
        //**********************
        txtcd_usuario.Text = ClsUsuario.CodigoDoUsuario.ToString();
        txtnm_usuario.Valor = ClsUsuario.NomeDoUsuario.Trim();
        txtsenha.Text = ClsUsuario.Senha.Trim();
        txtcontrasenha.Text = ClsUsuario.ContraSenha.Trim();
        chkadm.Checked = ClsUsuario.Administrador == 1 ? true : false;
        chkativo.Checked = ClsUsuario.Ativo == 1 ? true : false; 

        if (ClsUsuario.critica != "")
        {
            Mensagem(ClsUsuario.critica.ToString());
        }
        lblGrid.Text = ClsUsuario.TrazGrid();

        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.LimpaCampo();
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
