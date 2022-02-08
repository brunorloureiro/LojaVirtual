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

public partial class motoboyxbairro : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((bool)Session["useradm"] == false)
        {
            Mensagem("Acesso não autorizado. Tela exclusiva do Administrador.");
            this.motoxbairro.Visible = false;
        }
        else
        {
            this.motoxbairro.Visible = true;
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = !false;
            this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
           
            MotoxBairro ClsMotoxBairro = new MotoxBairro(Application["StrConexao"].ToString());

            lblGrid.Text = ClsMotoxBairro.TrazGrid();
            this.lblMsg.Text = "Cadastro de Bairros para entregas via Motoboy.";
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
        MotoxBairro ClsMotoxBairro = new MotoxBairro(Application["StrConexao"].ToString());
        ClsMotoxBairro.Codigo = Convert.ToInt16(this.txtcd_motoxbai.Text.ToString());
        ClsMotoxBairro.Bairro = this.txtbairro.Valor.ToString().Trim();
        ClsMotoxBairro.Cidade = this.cidade.Value.ToString().Trim();
        ClsMotoxBairro.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        resp = ClsMotoxBairro.Atualizar();
        //**************************

        if (ClsMotoxBairro.critica != "")
        {
            Mensagem(ClsMotoxBairro.critica.ToString());
        }
        lblGrid.Text = ClsMotoxBairro.TrazGrid();

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
        this.lblMsg.Text = "Cadastro de Bairros para entregas via Motoboy.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_motoxbai.Text.Trim() != "")
        {
            if (this.txtcd_motoxbai.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        MotoxBairro ClsMotoxBairro = new MotoxBairro(Application["StrConexao"].ToString());

        ClsMotoxBairro.Bairro = this.txtbairro.Valor.ToString().Trim();
        ClsMotoxBairro.Cidade = this.cidade.Value.ToString().Trim();
        ClsMotoxBairro.Valor = Convert.ToDecimal(this.txtvalor.Valor.Replace(".", ","));
        
        resp = ClsMotoxBairro.Grava();
        //*********************
        txtcd_motoxbai.Text = ClsMotoxBairro.Codigo.ToString();

        if (ClsMotoxBairro.critica != "")
        {
            Mensagem(ClsMotoxBairro.critica.ToString());
        }
        lblGrid.Text = ClsMotoxBairro.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        MotoxBairro ClsMotoxBairro = new MotoxBairro(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsMotoxBairro.Codigo = Convert.ToInt16(this.txtcd_motoxbai.Text.ToString());

        resp = ClsMotoxBairro.Consulta();
        //************************
        txtcd_motoxbai.Text = ClsMotoxBairro.Codigo.ToString();
        txtbairro.Valor = ClsMotoxBairro.Bairro.Trim();
        cidade.Value = ClsMotoxBairro.Cidade.Trim();
        txtvalor.Valor = ClsMotoxBairro.Valor.ToString();
       
        if (ClsMotoxBairro.critica != "")
        {
            Mensagem(ClsMotoxBairro.critica.ToString());
        }
        lblGrid.Text = ClsMotoxBairro.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    
    }

    public void LimpaCampo()
    {
        this.txtbairro.Valor = "";
        this.cidade.Value = "Vitoria";
        this.txtvalor.Valor = "0";
    }


    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        MotoxBairro ClsMotoxBairro = new MotoxBairro(Application["StrConexao"].ToString());

        ClsMotoxBairro.Codigo = Convert.ToInt16(this.txtcd_motoxbai.Text.ToString());

        resp = ClsMotoxBairro.Excluir();
        //**********************
        txtcd_motoxbai.Text = ClsMotoxBairro.Codigo.ToString();
        txtbairro.Valor = ClsMotoxBairro.Bairro.Trim();
        cidade.Value = ClsMotoxBairro.Cidade.Trim();
        txtvalor.Valor = ClsMotoxBairro.Valor.ToString();
       
        if (ClsMotoxBairro.critica != "")
        {
            Mensagem(ClsMotoxBairro.critica.ToString());
        }
        lblGrid.Text = ClsMotoxBairro.TrazGrid();

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
