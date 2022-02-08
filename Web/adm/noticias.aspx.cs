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
using System.IO;

public partial class noticias : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Noticia ClsNoticia = new Noticia(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_loja"] == false)
            {
                Mensagem("Acesso não autorizado pelo Administrador.");
                this.noticia.Visible = false;
            }
        }
      
        lblGrid.Text = ClsNoticia.TrazGrid();
            
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        this.lblMsg.Text = "Gerenciamento de notícias para a Loja Virtual.";
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }
    
    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Noticia ClsNoticia = new Noticia(Application["StrConexao"].ToString());
        ClsNoticia.CodigoDaNoticia = Convert.ToInt16(this.txtcd_noticia.Text.ToString());
        ClsNoticia.TituloDaNoticia = this.txtnm_titulo.Valor.ToString().Trim();
        ClsNoticia.DataDaNoticia = this.txtdh_noticia.Valor.ToString().Trim();
        ClsNoticia.DescricaoDaNoticia = this.txtds_noticia.Value.ToString().Trim();
        ClsNoticia.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsNoticia.Popup = Convert.ToInt16(this.chkpopup.Checked);
        
        resp = ClsNoticia.Atualizar();
        //**************************

        if (ClsNoticia.critica != "")
        {
            Mensagem(ClsNoticia.critica.ToString());
        }
        lblGrid.Text = ClsNoticia.TrazGrid();
        
        if (this.fluImagem.HasFile)
        {
            if (this.fluImagem.PostedFile.ContentType.ToString().ToLower().IndexOf("jpeg") >= 0)
            {
                if (this.fluImagem.PostedFile.ContentLength > 100000)
                {
                    Mensagem("Tamanho do arquivo informado é maior que o permitido. Verifique.");
                }
                else
                {
                    //this.fluImagem.SaveAs(Server.MapPath("/").ToString() + "\\images\\artigos\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
                    this.fluImagem.SaveAs("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
                }
            }
            else
            {
                Mensagem("Tipo do arquivo informado deve ser JPG. Verifique.");
            }
        }

        if (resp)
        {
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = true;
        }
        else
        {
            this.btn_atualizar.Enabled = true;
            this.btn_salvar.Enabled = false;
        }
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
    }

    public void novo(object sender, EventArgs e)
    {
        this.txtcd_noticia.Text = "0";
        LimpaCampo();
        this.imgNoticia.Visible = true;
        this.lblMsg.Text = "Gerenciamento de notícias para a Loja Virtual.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        {
            imgNoticia.ImageUrl = "../Images/noticias/" + this.txtcd_noticia.Text.ToString() + ".jpg";
            imgNoticia.Visible = true;
        }
        else
        {
            imgNoticia.Visible = false;
        }
  
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_noticia.Text.Trim() != "")
        {
            if (this.txtcd_noticia.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        bool resp;
        Noticia ClsNoticia = new Noticia(Application["StrConexao"].ToString());
        ClsNoticia.TituloDaNoticia = this.txtnm_titulo.Valor.ToString().Trim();
        ClsNoticia.DescricaoDaNoticia = this.txtds_noticia.Value.ToString().Trim();
        ClsNoticia.DataDaNoticia = this.txtdh_noticia.Valor.ToString().Trim();
        ClsNoticia.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsNoticia.Popup = Convert.ToInt16(this.chkpopup.Checked);

        resp = ClsNoticia.Grava();
        //*********************
        txtcd_noticia.Text = ClsNoticia.CodigoDaNoticia.ToString();

       
        if (ClsNoticia.critica != "")
        {
            Mensagem(ClsNoticia.critica.ToString());
        }
        lblGrid.Text = ClsNoticia.TrazGrid();

        if (resp)
        {
            this.btn_atualizar.Enabled = true;
            this.btn_salvar.Enabled = false;
        }
        else
        {
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = true;
        }
       
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;
         //if (File.Exists(Server.MapPath("/").ToString() + "\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        {
            imgNoticia.ImageUrl = "../Images/noticias/" + this.txtcd_noticia.Text.ToString() + ".jpg";
            imgNoticia.Visible = true;
        }
        else
        {
            imgNoticia.Visible = false;
        }

       
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Noticia ClsNoticia = new Noticia(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsNoticia.CodigoDaNoticia = Convert.ToInt16(this.txtcd_noticia.Text.ToString());

        resp = ClsNoticia.Consulta();
        //************************
        txtcd_noticia.Text = ClsNoticia.CodigoDaNoticia.ToString();
        txtnm_titulo.Valor = ClsNoticia.TituloDaNoticia.Trim();
        txtdh_noticia.Valor = ClsNoticia.DataDaNoticia.Replace("00:00:00", "").Trim();
        txtds_noticia.Value = ClsNoticia.DescricaoDaNoticia.Trim();
        chkativo.Checked = ClsNoticia.Ativo == 1 ? true : false;
        chkpopup.Checked = ClsNoticia.Popup == 1 ? true : false; 
               
        if (ClsNoticia.critica != "")
        {
            Mensagem(ClsNoticia.critica.ToString());
        }
        lblGrid.Text = ClsNoticia.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        {
            imgNoticia.ImageUrl = "../Images/noticias/" + this.txtcd_noticia.Text.ToString() + ".jpg";
            imgNoticia.Visible = true;
        }
        else
        {
            imgNoticia.Visible = false;
        }

    }

    public void LimpaCampo()
    {
        this.txtnm_titulo.Valor = "";
        this.txtds_noticia.Value = "";
        this.txtdh_noticia.Valor = "";
        this.chkativo.Checked = false;
        this.chkpopup.Checked = false; 
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        Noticia ClsNoticia = new Noticia(Application["StrConexao"].ToString());

        ClsNoticia.CodigoDaNoticia = Convert.ToInt16(this.txtcd_noticia.Text.ToString());

        resp = ClsNoticia.Excluir();
        //**********************

        //if (Convert.ToInt16(txtcd_noticia.Text) == 0)
        //{
        //    File.Delete("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
        //}

        txtcd_noticia.Text = ClsNoticia.CodigoDaNoticia.ToString();
        txtnm_titulo.Valor = ClsNoticia.TituloDaNoticia.Trim();
        txtds_noticia.Value = ClsNoticia.DescricaoDaNoticia.Trim();
        txtdh_noticia.Valor = ClsNoticia.DataDaNoticia.Trim();
        chkativo.Checked = ClsNoticia.Ativo == 1 ? true : false;
        chkpopup.Checked = ClsNoticia.Popup == 1 ? true : false; 

        if (ClsNoticia.critica != "")
        {
            Mensagem(ClsNoticia.critica.ToString());
        }
        lblGrid.Text = ClsNoticia.TrazGrid();
       
        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

       // if (File.Exists(Server.MapPath("/").ToString() + "\\Images\\Noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        File.Delete("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        {
            imgNoticia.ImageUrl = "../Images/noticias/" + this.txtcd_noticia.Text.ToString() + ".jpg";
            imgNoticia.Visible = true;
        }
        else
        {
            imgNoticia.Visible = false;
        }
        this.LimpaCampo();
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluImagem.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\noticias\\" + this.txtcd_noticia.Text.ToString() + ".jpg"))
        {
            imgNoticia.ImageUrl = "../Images/noticias/" + this.txtcd_noticia.Text.ToString() + ".jpg";
            imgNoticia.Visible = true;
        }
        else
        {
            imgNoticia.Visible = false;
        }
    }

 
}
