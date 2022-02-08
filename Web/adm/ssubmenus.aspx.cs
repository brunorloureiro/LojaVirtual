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

public partial class ssubmenus : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.ssubmenu.Visible = false;
        }
        else
        {
            this.ssubmenu.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

            SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());

            lblGrid.Text = ClsSSubMenu.TrazGrid();
            this.lblMsg.Text = "Cadastro de itens nos Menus dos SubMenu.";
        }
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());
        ClsSSubMenu.CarregaListaSubmenu(this.ddlsubmenus, "SubMenu", (Request["ddlsubmenus"] == null ? "0" : (string)Request["ddlsubmenus"]), "Escolha um SubMenu");
        
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());
        ClsSSubMenu.CodigoDoSSubMenu = Convert.ToInt16(this.txtcd_ssubmenu.Text.ToString());
        ClsSSubMenu.NomeDoSSubMenu = this.txtnm_ssubmenu.Valor.ToString().Trim();
        ClsSSubMenu.CodigoDoSubMenu = Convert.ToInt32(this.ddlsubmenus.SelectedValue);
        ClsSSubMenu.CodigoDoMenu = ClsSSubMenu.RetornaCodigo(Convert.ToInt32(this.ddlsubmenus.SelectedValue));
        ClsSSubMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsSSubMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsSSubMenu.Atualizar();
        //**************************

        if (ClsSSubMenu.critica != "")
        {
            Mensagem(ClsSSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSSubMenu.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de itens nos sSSubMenus.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_ssubmenu.Text.Trim() != "")
        {
            if (this.txtcd_ssubmenu.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());

        ClsSSubMenu.NomeDoSSubMenu = this.txtnm_ssubmenu.Valor.ToString().Trim();
        ClsSSubMenu.CodigoDoSubMenu = Convert.ToInt32(this.ddlsubmenus.SelectedValue);
        ClsSSubMenu.CodigoDoMenu = ClsSSubMenu.RetornaCodigo(Convert.ToInt32(this.ddlsubmenus.SelectedValue));
        ClsSSubMenu.Url = this.txturl.Valor.ToString().Trim();
        ClsSSubMenu.Ativo = Convert.ToInt16(this.chkativo.Checked);
        
        resp = ClsSSubMenu.Grava();
        //*********************
        txtcd_ssubmenu.Text = ClsSSubMenu.CodigoDoSSubMenu.ToString();

        if (ClsSSubMenu.critica != "")
        {
            Mensagem(ClsSSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSSubMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSSubMenu.CodigoDoSSubMenu = Convert.ToInt16(this.txtcd_ssubmenu.Text.ToString());

        resp = ClsSSubMenu.Consulta();
        //************************
        txtcd_ssubmenu.Text = ClsSSubMenu.CodigoDoSSubMenu.ToString();
        txtnm_ssubmenu.Valor = ClsSSubMenu.NomeDoSSubMenu.Trim();
        ddlsubmenus.SelectedValue = ClsSSubMenu.CodigoDoSubMenu.ToString();
        txturl.Valor = ClsSSubMenu.Url.Trim();
        chkativo.Checked = ClsSSubMenu.Ativo == 1 ? true : false; 

        if (ClsSSubMenu.critica != "")
        {
            Mensagem(ClsSSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSSubMenu.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void LimpaCampo()
    {
        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());
        this.txtnm_ssubmenu.Valor = "";
        ddlsubmenus.SelectedValue = ClsSSubMenu.CodigoDoSubMenu.ToString();
        this.chkativo.Checked = false;
        this.txturl.Valor = "";
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        SSubMenu ClsSSubMenu = new SSubMenu(Application["StrConexao"].ToString());

        ClsSSubMenu.CodigoDoSSubMenu = Convert.ToInt16(this.txtcd_ssubmenu.Text.ToString());

        resp = ClsSSubMenu.Excluir();
        //**********************
        txtcd_ssubmenu.Text = ClsSSubMenu.CodigoDoSSubMenu.ToString();
        txtnm_ssubmenu.Valor = ClsSSubMenu.NomeDoSSubMenu.Trim();
        ddlsubmenus.SelectedValue = ClsSSubMenu.CodigoDoSubMenu.ToString();
        txturl.Valor = ClsSSubMenu.Url.Trim();
        chkativo.Checked = ClsSSubMenu.Ativo == 1 ? true : false; 

        if (ClsSSubMenu.critica != "")
        {
            Mensagem(ClsSSubMenu.critica.ToString());
        }
        lblGrid.Text = ClsSSubMenu.TrazGrid();

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
