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

public partial class vicsecrets : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());
        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsVicSecrets.TrazGrid();
            }

            if ((bool)Session["bl_exclui"] == true)
            {
                this.btn_excluir.Visible = true;
                this.btn_excluirfoto.Visible = true;
            }
            else
            {
                this.btn_excluir.Visible = false;
                this.btn_excluirfoto.Visible = false;
            }

            if ((bool)Session["bl_grava"] == true)
            {
                this.btn_novo.Visible = true;
                this.btn_atualizar.Visible = true;
                this.btn_salvar.Visible = true;
            }
            else
            {
                this.btn_novo.Visible = false;
                this.btn_atualizar.Visible = false;
                this.btn_salvar.Visible = false;
            }
        }
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        this.lblMsg.Text = "Cadastro de Victória Secrets para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());
        ClsVicSecrets.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsVicSecrets.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
        ClsVicSecrets.CarregaLista(this.ddlfragrancias, "Fragrancia", (Request["ddlfragrancias"] == null ? "0" : (string)Request["ddlfragrancias"]), "Escolha uma Fragrância");
        ClsVicSecrets.CarregaLista(this.ddllinhas, "Linha", (Request["ddllinhas"] == null ? "0" : (string)Request["ddllinhas"]), "Escolha uma Linha");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());
        ClsVicSecrets.CodigoDaVicSecrets = this.txtcd_vicsec.Text.ToString().Trim();
        ClsVicSecrets.NomeDaVicSecrets = this.txtnm_vicsec.Valor.ToString().Trim();
        ClsVicSecrets.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsVicSecrets.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsVicSecrets.CodigoDaFragrancia = Convert.ToInt16(this.ddlfragrancias.SelectedValue);
        ClsVicSecrets.CodigoDaLinha = Convert.ToInt32(this.ddllinhas.SelectedValue);
        ClsVicSecrets.Volume = Convert.ToDecimal(this.txtvolume.Valor.Replace(".", ","));
        ClsVicSecrets.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsVicSecrets.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsVicSecrets.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsVicSecrets.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsVicSecrets.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsVicSecrets.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsVicSecrets.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsVicSecrets.Atualizar();
        //**************************

        if (ClsVicSecrets.critica != "")
        {
            Mensagem(ClsVicSecrets.critica.ToString());
        }
        lblGrid.Text = ClsVicSecrets.TrazGrid();

        AtualizaImagens();

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
        this.txtcd_vicsec.Text = "0"; 
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Victória Secrets para a Loja Virtual.";
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = true;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        NovoImagens();
   }

    public void salvar(object sender, EventArgs e)
    {
        bool resp;
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());
        ClsVicSecrets.CodigoDaVicSecrets = this.txtcd_vicsec.Text.ToString().Trim();
        ClsVicSecrets.NomeDaVicSecrets = this.txtnm_vicsec.Valor.ToString().Trim();
        ClsVicSecrets.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsVicSecrets.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsVicSecrets.CodigoDaFragrancia = Convert.ToInt16(this.ddlfragrancias.SelectedValue);
        ClsVicSecrets.CodigoDaLinha = Convert.ToInt32(this.ddllinhas.SelectedValue);
        ClsVicSecrets.Volume = Convert.ToDecimal(this.txtvolume.Valor.Replace(".", ","));
        ClsVicSecrets.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsVicSecrets.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsVicSecrets.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsVicSecrets.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsVicSecrets.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsVicSecrets.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsVicSecrets.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsVicSecrets.Grava();
        //*********************
        txtcd_vicsec.Text = ClsVicSecrets.CodigoDaVicSecrets.ToString();

        if (ClsVicSecrets.critica != "")
        {
            Mensagem(ClsVicSecrets.critica.ToString());
        }
        if (ClsVicSecrets.LimparCampos)
        {
            this.txtcd_vicsec.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsVicSecrets.TrazGrid();

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
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        SalvaImagens();
    }

    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsVicSecrets.CodigoDaVicSecrets = this.txtcd_vicsec.Text.ToString().Trim();

        resp = ClsVicSecrets.Consulta();
        //************************
        txtcd_vicsec.Text = ClsVicSecrets.CodigoDaVicSecrets.Trim();
        txtnm_vicsec.Valor = ClsVicSecrets.NomeDaVicSecrets.Trim();
        ddlprodutos.SelectedValue = ClsVicSecrets.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsVicSecrets.CodigoDaMarca.ToString();
        ddlfragrancias.SelectedValue = ClsVicSecrets.CodigoDaFragrancia.ToString();
        ddllinhas.SelectedValue = ClsVicSecrets.CodigoDaLinha.ToString();
        txtvolume.Valor = ClsVicSecrets.Volume.ToString();
        txtpeso.Valor = ClsVicSecrets.Peso.ToString();
        txtpreco.Valor = ClsVicSecrets.Preco.ToString();
        chkfrete.Checked = ClsVicSecrets.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsVicSecrets.Novo == 1 ? true : false;
        chkativo.Checked = ClsVicSecrets.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsVicSecrets.Descricao.Trim();
        txtcomentario.Value = ClsVicSecrets.Comentario.Trim();
       
        if (ClsVicSecrets.critica != "")
        {
            Mensagem(ClsVicSecrets.critica.ToString());
        }
        lblGrid.Text = ClsVicSecrets.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        ProcuraImagens();

    }

    public void LimpaCampo()
    {
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());
        this.txtnm_vicsec.Valor = "";
        ddlprodutos.SelectedValue = ClsVicSecrets.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsVicSecrets.CodigoDaMarca.ToString();
        ddlfragrancias.SelectedValue = ClsVicSecrets.CodigoDaFragrancia.ToString();
        ddllinhas.SelectedValue = ClsVicSecrets.CodigoDaLinha.ToString();
        this.txtvolume.Valor = "0";
        this.txtpeso.Valor = "0";
        this.txtpreco.Valor = "0";
        this.chkfrete.Checked = false;
        this.chknovo.Checked = false;
        this.chkativo.Checked = false;
        this.txtdescricao.Value = "";
        this.txtcomentario.Value = "";
     }

    public void excluir(object sender, EventArgs e)
    {
        bool resp;
        VicSecrets ClsVicSecrets = new VicSecrets(Application["StrConexao"].ToString());

        ClsVicSecrets.CodigoDaVicSecrets = this.txtcd_vicsec.Text.ToString().Trim();

        resp = ClsVicSecrets.Excluir();
        //**********************

        txtcd_vicsec.Text = "";
        txtnm_vicsec.Valor = ClsVicSecrets.NomeDaVicSecrets.Trim();
        ddlprodutos.SelectedValue = ClsVicSecrets.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsVicSecrets.CodigoDaMarca.ToString();
        ddlfragrancias.SelectedValue = ClsVicSecrets.CodigoDaFragrancia.ToString();
        ddllinhas.SelectedValue = ClsVicSecrets.CodigoDaLinha.ToString();
        txtvolume.Valor = ClsVicSecrets.Volume.ToString();
        txtpeso.Valor = ClsVicSecrets.Peso.ToString();
        txtpreco.Valor = ClsVicSecrets.Preco.ToString();
        chkfrete.Checked = ClsVicSecrets.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsVicSecrets.Novo == 1 ? true : false;
        chkativo.Checked = ClsVicSecrets.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsVicSecrets.Descricao.Trim();
        txtcomentario.Value = ClsVicSecrets.Comentario.Trim();
       
        if (ClsVicSecrets.critica != "")
        {
            Mensagem(ClsVicSecrets.critica.ToString());
        }
        lblGrid.Text = ClsVicSecrets.TrazGrid();

        this.btn_atualizar.Enabled = !resp;
        this.btn_salvar.Enabled = resp;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        ExcluiImagens();
        this.LimpaCampo();
    }

    public void NovoImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void AtualizaImagens()
    {
        if (this.fluProduto1.HasFile)
        {
            if (this.fluProduto1.PostedFile.ContentType.ToString().ToLower().IndexOf("jpeg") >= 0)
            {
                if (this.fluProduto1.PostedFile.ContentLength > 100000)
                {
                    Mensagem("Tamanho do arquivo informado é maior que o permitido. Verifique.");
                }
                else
                {
                    //this.fluImagem.SaveAs(Server.MapPath("/").ToString() + "\\images\\artigos\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
                }
            }
            else
            {
                Mensagem("Tipo do arquivo informado deve ser JPG. Verifique.");
            }
       }

        if (this.fluProduto2.HasFile)
        {
            if (this.fluProduto2.PostedFile.ContentType.ToString().ToLower().IndexOf("jpeg") >= 0)
            {
                if (this.fluProduto2.PostedFile.ContentLength > 100000)
                {
                    Mensagem("Tamanho do arquivo informado é maior que o permitido. Verifique.");
                }
                else
                {
                    //this.fluImagem.SaveAs(Server.MapPath("/").ToString() + "\\images\\artigos\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
                }
            }
            else
            {
                Mensagem("Tipo do arquivo informado deve ser JPG. Verifique.");
            }
        }

        if (this.fluProduto3.HasFile)
        {
            if (this.fluProduto3.PostedFile.ContentType.ToString().ToLower().IndexOf("jpeg") >= 0)
            {
                if (this.fluProduto3.PostedFile.ContentLength > 100000)
                {
                    Mensagem("Tamanho do arquivo informado é maior que o permitido. Verifique.");
                }
                else
                {
                    //this.fluImagem.SaveAs(Server.MapPath("/").ToString() + "\\images\\artigos\\" + this.txtcd_noticia.Text.ToString() + ".jpg");
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
                }
            }
            else
            {
                Mensagem("Tipo do arquivo informado deve ser JPG. Verifique.");
            }
        }
    }

    public void ExcluiImagens()
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_vicsec.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_vicsec.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
