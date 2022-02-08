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

public partial class camisas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsCamisa.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Camisas para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());
        ClsCamisa.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsCamisa.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());
        ClsCamisa.CodigoDaCamisa = this.txtcd_camisa.Text.ToString().Trim();
        ClsCamisa.NomeDaCamisa = this.txtnm_camisa.Valor.ToString().Trim();
        ClsCamisa.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCamisa.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCamisa.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCamisa.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCamisa.BabyLook = Convert.ToInt16(this.chkbabylook.Checked);
        ClsCamisa.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsCamisa.LugarDaEstampa = this.estampa.Value.ToString().Trim();
        ClsCamisa.TipoDeManga = this.manga.Value.ToString().Trim();
        ClsCamisa.TipoDeGola = this.gola.Value.ToString().Trim();
        ClsCamisa.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCamisa.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsCamisa.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCamisa.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCamisa.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCamisa.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCamisa.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCamisa.Atualizar();
        //**************************

        if (ClsCamisa.critica != "")
        {
            Mensagem(ClsCamisa.critica.ToString());
        }
        lblGrid.Text = ClsCamisa.TrazGrid();

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
        this.txtcd_camisa.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Camisas para a Loja Virtual.";
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
        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());
        ClsCamisa.CodigoDaCamisa = this.txtcd_camisa.Text.ToString().Trim();
        ClsCamisa.NomeDaCamisa = this.txtnm_camisa.Valor.ToString().Trim();
        ClsCamisa.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCamisa.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCamisa.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCamisa.BabyLook = Convert.ToInt16(this.chkbabylook.Checked);
        ClsCamisa.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsCamisa.LugarDaEstampa = this.estampa.Value.ToString().Trim();
        ClsCamisa.TipoDeManga = this.manga.Value.ToString().Trim();
        ClsCamisa.TipoDeGola = this.gola.Value.ToString().Trim();
        ClsCamisa.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCamisa.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCamisa.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsCamisa.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCamisa.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCamisa.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCamisa.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCamisa.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCamisa.Grava();
        //*********************
        txtcd_camisa.Text = ClsCamisa.CodigoDaCamisa.ToString();

        if (ClsCamisa.critica != "")
        {
            Mensagem(ClsCamisa.critica.ToString());
        }
        if (ClsCamisa.LimparCampos)
        {
            this.txtcd_camisa.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsCamisa.TrazGrid();

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
        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCamisa.CodigoDaCamisa = this.txtcd_camisa.Text.ToString().Trim();

        resp = ClsCamisa.Consulta();
        //************************
        txtcd_camisa.Text = ClsCamisa.CodigoDaCamisa.Trim();
        txtnm_camisa.Valor = ClsCamisa.NomeDaCamisa.Trim();
        ddlprodutos.SelectedValue = ClsCamisa.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamisa.CodigoDaMarca.ToString();
        tamanho.Value = ClsCamisa.Tamanho.Trim();
        txtcor.Valor = ClsCamisa.Cor.Trim();
        chkbabylook.Checked = ClsCamisa.BabyLook == 1 ? true : false;
        chkestampa.Checked = ClsCamisa.Estampa == 1 ? true : false;
        estampa.Value = ClsCamisa.LugarDaEstampa.Trim();
        manga.Value = ClsCamisa.TipoDeManga.Trim();
        gola.Value = ClsCamisa.TipoDeGola.Trim();
        txtpeso.Valor = ClsCamisa.Peso.ToString();
        txtpreco.Valor = ClsCamisa.Preco.ToString();
        chkfrete.Checked = ClsCamisa.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCamisa.Novo == 1 ? true : false;
        chkativo.Checked = ClsCamisa.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCamisa.Descricao.Trim();
        txtcomentario.Value = ClsCamisa.Comentario.Trim();
       
        if (ClsCamisa.critica != "")
        {
            Mensagem(ClsCamisa.critica.ToString());
        }
        lblGrid.Text = ClsCamisa.TrazGrid();

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
        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());
        this.txtnm_camisa.Valor = "";
        ddlprodutos.SelectedValue = ClsCamisa.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamisa.CodigoDaMarca.ToString();
        this.tamanho.Value = "PP";
        this.txtcor.Valor = "";
        this.chkbabylook.Checked = false;
        this.chkestampa.Checked = false;
        this.estampa.Value = "";
        this.manga.Value = "";
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
        Camisa ClsCamisa = new Camisa(Application["StrConexao"].ToString());

        ClsCamisa.CodigoDaCamisa = this.txtcd_camisa.Text.ToString().Trim();

        resp = ClsCamisa.Excluir();
        //**********************

        txtcd_camisa.Text = "";
        txtnm_camisa.Valor = ClsCamisa.NomeDaCamisa.Trim();
        ddlprodutos.SelectedValue = ClsCamisa.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCamisa.CodigoDaMarca.ToString();
        tamanho.Value = ClsCamisa.Tamanho.Trim();
        txtcor.Valor = ClsCamisa.Cor.Trim();
        chkbabylook.Checked = ClsCamisa.BabyLook == 1 ? true : false;
        chkestampa.Checked = ClsCamisa.Estampa == 1 ? true : false;
        estampa.Value = ClsCamisa.LugarDaEstampa.Trim();
        manga.Value = ClsCamisa.TipoDeManga.Trim();
        gola.Value = ClsCamisa.TipoDeGola.Trim();
        txtpeso.Valor = ClsCamisa.Peso.ToString();
        txtpreco.Valor = ClsCamisa.Preco.ToString();
        chkfrete.Checked = ClsCamisa.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCamisa.Novo == 1 ? true : false;
        chkativo.Checked = ClsCamisa.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCamisa.Descricao.Trim();
        txtcomentario.Value = ClsCamisa.Comentario.Trim();
       
        if (ClsCamisa.critica != "")
        {
            Mensagem(ClsCamisa.critica.ToString());
        }
        lblGrid.Text = ClsCamisa.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camisa.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_camisa.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_camisa.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
