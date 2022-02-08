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

public partial class submenus : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.submenu.Visible = false;
        }
        else
        {
            this.submenu.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());

            lblGrid.Text = ClsSubMenu.TrazGrid();
            this.lblMsg.Text = "Cadastro de itens nos SubMenus.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());
        ClsSubMenu.CarregaLista(this.ddlmenus, "Menu", (Request["ddlmenus"] == null ? "0" : (string)Request["ddlmenus"]), "Escolha um Menu");
        ClsSubMenu.CarregaLista(this.ddlgeneros, "Genero", (Request["ddlgeneros"] == null ? "0" : (string)Request["ddlgeneros"]), "Escolha um Gênero");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());
        ClsSubMenu.CodigoDoSubMenu = Convert.ToInt16(this.txtcd_submenu.Text.ToString());
        ClsSubMenu.NomeDoSubMenu = this.txtnm_submenu.Valor.ToString().Trim();
        ClsSubMenu.CodigoDoMenu = Convert.ToInt16(this.ddlmenus.SelectedValue);
        ClsSubMenu.CodigoDoGenero = Convert.ToInt16(this.ddlgeneros.SelectedValue);
        ClsSubMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsSubMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsSubMenu.Atualizar();
        //**************************

        if (ClsSubMenu.critica != "")
        {
            Mensagem(ClsSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSubMenu.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de itens nos SubMenus.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_submenu.Text.Trim() != "")
        {
            if (this.txtcd_submenu.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());

        ClsSubMenu.NomeDoSubMenu = this.txtnm_submenu.Valor.ToString().Trim();
        ClsSubMenu.CodigoDoMenu = Convert.ToInt16(this.ddlmenus.SelectedValue);
        ClsSubMenu.CodigoDoGenero = Convert.ToInt16(this.ddlgeneros.SelectedValue);
        ClsSubMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsSubMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsSubMenu.Grava();
        //*********************
        txtcd_submenu.Text = ClsSubMenu.CodigoDoSubMenu.ToString();

        if (ClsSubMenu.critica != "")
        {
            Mensagem(ClsSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSubMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSubMenu.CodigoDoSubMenu = Convert.ToInt16(this.txtcd_submenu.Text.ToString());

        resp = ClsSubMenu.Consulta();
        //************************
        txtcd_submenu.Text = ClsSubMenu.CodigoDoSubMenu.ToString();
        txtnm_submenu.Valor = ClsSubMenu.NomeDoSubMenu.Trim();
        ddlmenus.SelectedValue = ClsSubMenu.CodigoDoMenu.ToString();
        ddlgeneros.SelectedValue = ClsSubMenu.CodigoDoGenero.ToString();
        txturl.Valor = ClsSubMenu.Url.Trim();
        chkativo.Checked = ClsSubMenu.Ativo == 1 ? true : false; 

        if (ClsSubMenu.critica != "")
        {
            Mensagem(ClsSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSubMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());
        this.txtnm_submenu.Valor = "";
        ddlmenus.SelectedValue = ClsSubMenu.CodigoDoMenu.ToString();
        ddlgeneros.SelectedValue = ClsSubMenu.CodigoDoGenero.ToString();
        this.chkativo.Checked = false;
        this.txturl.Valor = "";
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        SubMenu ClsSubMenu = new SubMenu(Application["StrConexao"].ToString());

        ClsSubMenu.CodigoDoSubMenu = Convert.ToInt16(this.txtcd_submenu.Text.ToString());

        resp = ClsSubMenu.Excluir();
        //**********************
        txtcd_submenu.Text = ClsSubMenu.CodigoDoSubMenu.ToString();
        txtnm_submenu.Valor = ClsSubMenu.NomeDoSubMenu.Trim();
        ddlmenus.SelectedValue = ClsSubMenu.CodigoDoMenu.ToString();
        ddlgeneros.SelectedValue = ClsSubMenu.CodigoDoGenero.ToString();
        txturl.Valor = ClsSubMenu.Url.Trim();
        chkativo.Checked = ClsSubMenu.Ativo == 1 ? true : false; 

        if (ClsSubMenu.critica != "")
        {
            Mensagem(ClsSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSubMenu.TrazGrid();

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
