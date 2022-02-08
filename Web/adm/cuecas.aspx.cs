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

public partial class cuecas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsCueca.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Cuecas para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());
        ClsCueca.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsCueca.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());
        ClsCueca.CodigoDaCueca = this.txtcd_cueca.Text.ToString().Trim();
        ClsCueca.NomeDaCueca = this.txtnm_cueca.Valor.ToString().Trim();
        ClsCueca.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCueca.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCueca.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCueca.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCueca.Tipo = this.tipo.Value.ToString().Trim();
        ClsCueca.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCueca.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsCueca.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCueca.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCueca.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCueca.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCueca.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCueca.Atualizar();
        //**************************

        if (ClsCueca.critica != "")
        {
            Mensagem(ClsCueca.critica.ToString());
        }
        lblGrid.Text = ClsCueca.TrazGrid();

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
        this.txtcd_cueca.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Cuecas para a Loja Virtual.";
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
        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());
        ClsCueca.CodigoDaCueca = this.txtcd_cueca.Text.ToString().Trim();
        ClsCueca.NomeDaCueca = this.txtnm_cueca.Valor.ToString().Trim();
        ClsCueca.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsCueca.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsCueca.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsCueca.Cor = this.txtcor.Valor.ToString().Trim();
        ClsCueca.Tipo = this.tipo.Value.ToString().Trim();
        ClsCueca.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsCueca.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsCueca.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsCueca.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsCueca.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsCueca.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsCueca.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsCueca.Grava();
        //*********************
        txtcd_cueca.Text = ClsCueca.CodigoDaCueca.ToString();

        if (ClsCueca.critica != "")
        {
            Mensagem(ClsCueca.critica.ToString());
        }
        if (ClsCueca.LimparCampos)
        {
            this.txtcd_cueca.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsCueca.TrazGrid();

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
        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCueca.CodigoDaCueca = this.txtcd_cueca.Text.ToString().Trim();

        resp = ClsCueca.Consulta();
        //************************
        txtcd_cueca.Text = ClsCueca.CodigoDaCueca.Trim();
        txtnm_cueca.Valor = ClsCueca.NomeDaCueca.Trim();
        ddlprodutos.SelectedValue = ClsCueca.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCueca.CodigoDaMarca.ToString();
        tamanho.Value = ClsCueca.Tamanho.Trim();
        txtcor.Valor = ClsCueca.Cor.Trim();
        tipo.Value = ClsCueca.Tipo.Trim();
        txtpeso.Valor = ClsCueca.Peso.ToString();
        txtpreco.Valor = ClsCueca.Preco.ToString();
        chkfrete.Checked = ClsCueca.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCueca.Novo == 1 ? true : false;
        chkativo.Checked = ClsCueca.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCueca.Descricao.Trim();
        txtcomentario.Value = ClsCueca.Comentario.Trim();
       
        if (ClsCueca.critica != "")
        {
            Mensagem(ClsCueca.critica.ToString());
        }
        lblGrid.Text = ClsCueca.TrazGrid();

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
        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());
        this.txtnm_cueca.Valor = "";
        ddlprodutos.SelectedValue = ClsCueca.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCueca.CodigoDaMarca.ToString();
        this.tamanho.Value = "P";
        this.txtcor.Valor = "";
        this.tipo.Value = "B";
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
        Cueca ClsCueca = new Cueca(Application["StrConexao"].ToString());

        ClsCueca.CodigoDaCueca = this.txtcd_cueca.Text.ToString().Trim();

        resp = ClsCueca.Excluir();
        //**********************

        txtcd_cueca.Text = "";
        txtnm_cueca.Valor = ClsCueca.NomeDaCueca.Trim();
        ddlprodutos.SelectedValue = ClsCueca.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsCueca.CodigoDaMarca.ToString();
        tamanho.Value = ClsCueca.Tamanho.Trim();
        txtcor.Valor = ClsCueca.Cor.Trim();
        tipo.Value = ClsCueca.Tipo.Trim();
        txtpeso.Valor = ClsCueca.Peso.ToString();
        txtpreco.Valor = ClsCueca.Preco.ToString();
        chkfrete.Checked = ClsCueca.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsCueca.Novo == 1 ? true : false;
        chkativo.Checked = ClsCueca.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsCueca.Descricao.Trim();
        txtcomentario.Value = ClsCueca.Comentario.Trim();
       
        if (ClsCueca.critica != "")
        {
            Mensagem(ClsCueca.critica.ToString());
        }
        lblGrid.Text = ClsCueca.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_cueca.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_cueca.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_cueca.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
