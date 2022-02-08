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

public partial class camsociais : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsCamsocial.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Camisas Sociais para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());
        ClsCamsocial.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsCamsocial.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());
        ClsCamsocial.CodigoDaCamisaSocial = this.txtcd_camsocial.Text.ToString().Trim();
        ClsCamsocial.NomeDaCamisaSocial = this.txtnm_camsocial.Valor.ToString().Trim();
        ClsCamsocial.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCamsocial.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCamsocial.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCamsocial.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCamsocial.TipoDeManga = this.manga.Value.ToString().Trim();
        ClsCamsocial.TipoDeGola = this.gola.Value.ToString().Trim();
        ClsCamsocial.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCamsocial.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsCamsocial.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCamsocial.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCamsocial.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCamsocial.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCamsocial.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCamsocial.Atualizar();
        //**************************

        if (ClsCamsocial.critica != "")
        {
            Mensagem(ClsCamsocial.critica.ToString());
        }
        lblGrid.Text = ClsCamsocial.TrazGrid();

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
        this.txtcd_camsocial.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Camisas Sociais para a Loja Virtual.";
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
        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());
        ClsCamsocial.CodigoDaCamisaSocial = this.txtcd_camsocial.Text.ToString().Trim();
        ClsCamsocial.NomeDaCamisaSocial = this.txtnm_camsocial.Valor.ToString().Trim();
        ClsCamsocial.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCamsocial.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCamsocial.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCamsocial.TipoDeManga = this.manga.Value.ToString().Trim();
        ClsCamsocial.TipoDeGola = this.gola.Value.ToString().Trim();
        ClsCamsocial.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCamsocial.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCamsocial.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsCamsocial.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCamsocial.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCamsocial.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCamsocial.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCamsocial.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCamsocial.Grava();
        //*********************
        txtcd_camsocial.Text = ClsCamsocial.CodigoDaCamisaSocial.ToString();

        if (ClsCamsocial.critica != "")
        {
            Mensagem(ClsCamsocial.critica.ToString());
        }
        if (ClsCamsocial.LimparCampos)
        {
            this.txtcd_camsocial.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsCamsocial.TrazGrid();

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
        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCamsocial.CodigoDaCamisaSocial = this.txtcd_camsocial.Text.ToString().Trim();

        resp = ClsCamsocial.Consulta();
        //************************
        txtcd_camsocial.Text = ClsCamsocial.CodigoDaCamisaSocial.Trim();
        txtnm_camsocial.Valor = ClsCamsocial.NomeDaCamisaSocial.Trim();
        ddlprodutos.SelectedValue = ClsCamsocial.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamsocial.CodigoDaMarca.ToString();
        tamanho.Value = ClsCamsocial.Tamanho.Trim();
        txtcor.Valor = ClsCamsocial.Cor.Trim();
        manga.Value = ClsCamsocial.TipoDeManga.Trim();
        gola.Value = ClsCamsocial.TipoDeGola.Trim();
        txtpeso.Valor = ClsCamsocial.Peso.ToString();
        txtpreco.Valor = ClsCamsocial.Preco.ToString();
        chkfrete.Checked = ClsCamsocial.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCamsocial.Novo == 1 ? true : false;
        chkativo.Checked = ClsCamsocial.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCamsocial.Descricao.Trim();
        txtcomentario.Value = ClsCamsocial.Comentario.Trim();
       
        if (ClsCamsocial.critica != "")
        {
            Mensagem(ClsCamsocial.critica.ToString());
        }
        lblGrid.Text = ClsCamsocial.TrazGrid();

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
        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());
        this.txtnm_camsocial.Valor = "";
        ddlprodutos.SelectedValue = ClsCamsocial.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamsocial.CodigoDaMarca.ToString();
        this.tamanho.Value = "P";
        this.txtcor.Valor = "";
        this.manga.Value = "C";
        this.gola.Value = "N";
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
        Camsocial ClsCamsocial = new Camsocial(Application["StrConexao"].ToString());

        ClsCamsocial.CodigoDaCamisaSocial = this.txtcd_camsocial.Text.ToString().Trim();

        resp = ClsCamsocial.Excluir();
        //**********************

        txtcd_camsocial.Text = "";
        txtnm_camsocial.Valor = ClsCamsocial.NomeDaCamisaSocial.Trim();
        ddlprodutos.SelectedValue = ClsCamsocial.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamsocial.CodigoDaMarca.ToString();
        tamanho.Value = ClsCamsocial.Tamanho.Trim();
        txtcor.Valor = ClsCamsocial.Cor.Trim();
        manga.Value = ClsCamsocial.TipoDeManga.Trim();
        gola.Value = ClsCamsocial.TipoDeGola.Trim();
        txtpeso.Valor = ClsCamsocial.Peso.ToString();
        txtpreco.Valor = ClsCamsocial.Preco.ToString();
        chkfrete.Checked = ClsCamsocial.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCamsocial.Novo == 1 ? true : false;
        chkativo.Checked = ClsCamsocial.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCamsocial.Descricao.Trim();
        txtcomentario.Value = ClsCamsocial.Comentario.Trim();
       
        if (ClsCamsocial.critica != "")
        {
            Mensagem(ClsCamsocial.critica.ToString());
        }
        lblGrid.Text = ClsCamsocial.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camsocial.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camsocial.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
