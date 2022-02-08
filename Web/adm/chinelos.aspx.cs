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

public partial class chinelos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsChinelo.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Chinelos para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());
        ClsChinelo.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsChinelo.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
        ClsChinelo.CarregaLista(this.ddlmateriais, "Material", (Request["ddlmateriais"] == null ? "0" : (string)Request["ddlmateriais"]), "Escolha um Material");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());
        ClsChinelo.CodigoDoChinelo = this.txtcd_chinelo.Text.ToString().Trim();
        ClsChinelo.NomeDoChinelo = this.txtnm_chinelo.Valor.ToString().Trim();
        ClsChinelo.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsChinelo.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsChinelo.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsChinelo.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsChinelo.Cor = this.txtcor.Valor.ToString().Trim();
        ClsChinelo.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsChinelo.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsChinelo.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsChinelo.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsChinelo.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsChinelo.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsChinelo.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsChinelo.Atualizar();
        //**************************

        if (ClsChinelo.critica != "")
        {
            Mensagem(ClsChinelo.critica.ToString());
        }
        lblGrid.Text = ClsChinelo.TrazGrid();

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
        this.txtcd_chinelo.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Chinelos para a Loja Virtual.";
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
        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());
        ClsChinelo.CodigoDoChinelo = this.txtcd_chinelo.Text.ToString().Trim();
        ClsChinelo.NomeDoChinelo = this.txtnm_chinelo.Valor.ToString().Trim();
        ClsChinelo.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsChinelo.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsChinelo.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsChinelo.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsChinelo.Cor = this.txtcor.Valor.ToString().Trim();
        ClsChinelo.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsChinelo.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsChinelo.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsChinelo.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsChinelo.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsChinelo.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsChinelo.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsChinelo.Grava();
        //*********************
        txtcd_chinelo.Text = ClsChinelo.CodigoDoChinelo.ToString();

        if (ClsChinelo.critica != "")
        {
            Mensagem(ClsChinelo.critica.ToString());
        }
        if (ClsChinelo.LimparCampos)
        {
            this.txtcd_chinelo.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsChinelo.TrazGrid();

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
        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsChinelo.CodigoDoChinelo = this.txtcd_chinelo.Text.ToString().Trim();

        resp = ClsChinelo.Consulta();
        //************************
        txtcd_chinelo.Text = ClsChinelo.CodigoDoChinelo.Trim();
        txtnm_chinelo.Valor = ClsChinelo.NomeDoChinelo.Trim();
        ddlprodutos.SelectedValue = ClsChinelo.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsChinelo.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsChinelo.CodigoDoMaterial.ToString();
        tamanho.Value = ClsChinelo.Tamanho.Trim();
        txtcor.Valor = ClsChinelo.Cor.Trim();
        txtpeso.Valor = ClsChinelo.Peso.ToString();
        txtpreco.Valor = ClsChinelo.Preco.ToString();
        chkfrete.Checked = ClsChinelo.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsChinelo.Novo == 1 ? true : false;
        chkativo.Checked = ClsChinelo.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsChinelo.Descricao.Trim();
        txtcomentario.Value = ClsChinelo.Comentario.Trim();
       
        if (ClsChinelo.critica != "")
        {
            Mensagem(ClsChinelo.critica.ToString());
        }
        lblGrid.Text = ClsChinelo.TrazGrid();

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
        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());
        this.txtnm_chinelo.Valor = "";
        ddlprodutos.SelectedValue = ClsChinelo.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsChinelo.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsChinelo.CodigoDoMaterial.ToString();
        this.tamanho.Value = "32";
        this.txtcor.Valor = "";
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
        Chinelo ClsChinelo = new Chinelo(Application["StrConexao"].ToString());

        ClsChinelo.CodigoDoChinelo = this.txtcd_chinelo.Text.ToString().Trim();

        resp = ClsChinelo.Excluir();
        //**********************

        txtcd_chinelo.Text = "";
        txtnm_chinelo.Valor = ClsChinelo.NomeDoChinelo.Trim();
        ddlprodutos.SelectedValue = ClsChinelo.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsChinelo.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsChinelo.CodigoDoMaterial.ToString();
        tamanho.Value = ClsChinelo.Tamanho.Trim();
        txtcor.Valor = ClsChinelo.Cor.Trim();
        txtpeso.Valor = ClsChinelo.Peso.ToString();
        txtpreco.Valor = ClsChinelo.Preco.ToString();
        chkfrete.Checked = ClsChinelo.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsChinelo.Novo == 1 ? true : false;
        chkativo.Checked = ClsChinelo.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsChinelo.Descricao.Trim();
        txtcomentario.Value = ClsChinelo.Comentario.Trim();
       
        if (ClsChinelo.critica != "")
        {
            Mensagem(ClsChinelo.critica.ToString());
        }
        lblGrid.Text = ClsChinelo.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_chinelo.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_chinelo.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
