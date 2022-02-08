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
//using Dominio;

public partial class menus : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.menu.Visible = false;
        }
        else
        {
            this.menu.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            Menu ClsMenu = new Menu(Application["StrConexao"].ToString());
            
            lblGrid.Text = ClsMenu.TrazGrid();
            this.lblMsg.Text = "Cadastro de itens nos Menus.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());
        ClsMenu.CarregaLista(this.ddlmodulos, "Modulo", (Request["ddlmodulos"] == null ? "0" : (string)Request["ddlmodulos"]), "Escolha um Módulo");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());
        ClsMenu.CodigoDoMenu = Convert.ToInt16(this.txtcd_menu.Text.ToString());
        ClsMenu.NomeDoMenu = this.txtnm_menu.Valor.ToString().Trim();
        ClsMenu.CodigoDoModulo = Convert.ToInt16(this.ddlmodulos.SelectedValue);
        ClsMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsMenu.Atualizar();
        //**************************

        if (ClsMenu.critica != "")
        {
            Mensagem(ClsMenu.critica.ToString());
        }
        lblGrid.Text = ClsMenu.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de itens nos Menus.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_menu.Text.Trim() != "")
        {
            if (this.txtcd_menu.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());

        ClsMenu.NomeDoMenu = this.txtnm_menu.Valor.ToString().Trim();
        ClsMenu.CodigoDoModulo = Convert.ToInt16(this.ddlmodulos.SelectedValue);
        ClsMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsMenu.Grava();
        //*********************
        txtcd_menu.Text = ClsMenu.CodigoDoMenu.ToString();

        if (ClsMenu.critica != "")
        {
            Mensagem(ClsMenu.critica.ToString());
        }
        lblGrid.Text = ClsMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsMenu.CodigoDoMenu = Convert.ToInt16(this.txtcd_menu.Text.ToString());

        resp = ClsMenu.Consulta();
        //************************
        txtcd_menu.Text = ClsMenu.CodigoDoMenu.ToString();
        txtnm_menu.Valor = ClsMenu.NomeDoMenu.Trim();
        ddlmodulos.SelectedValue = ClsMenu.CodigoDoModulo.ToString();
        txturl.Valor = ClsMenu.Url.Trim();
        chkativo.Checked = ClsMenu.Ativo == 1 ? true : false; 

        if (ClsMenu.critica != "")
        {
            Mensagem(ClsMenu.critica.ToString());
        }
        lblGrid.Text = ClsMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

    }

    public void LimpaCampo()
    {
        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());
        this.txtnm_menu.Valor = "";
        ddlmodulos.SelectedValue = ClsMenu.CodigoDoModulo.ToString();
        this.chkativo.Checked = false;
        this.txturl.Valor = "";
     }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Menu ClsMenu = new Menu(Application["StrConexao"].ToString());

        ClsMenu.CodigoDoMenu = Convert.ToInt16(this.txtcd_menu.Text.ToString());

        resp = ClsMenu.Excluir();
        //**********************
        txtcd_menu.Text = ClsMenu.CodigoDoMenu.ToString();
        txtnm_menu.Valor = ClsMenu.NomeDoMenu.Trim();
        ddlmodulos.SelectedValue = ClsMenu.CodigoDoModulo.ToString();
        txturl.Valor = ClsMenu.Url.Trim();
        chkativo.Checked = ClsMenu.Ativo == 1 ? true : false; 

        if (ClsMenu.critica != "")
        {
            Mensagem(ClsMenu.critica.ToString());
        }
        lblGrid.Text = ClsMenu.TrazGrid();

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
