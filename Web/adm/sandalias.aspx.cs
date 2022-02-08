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

public partial class sandalias : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsSandalia.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Sandálias para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());
        ClsSandalia.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsSandalia.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
        ClsSandalia.CarregaLista(this.ddlmateriais, "Material", (Request["ddlmateriais"] == null ? "0" : (string)Request["ddlmateriais"]), "Escolha um Material");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());
        ClsSandalia.CodigoDaSandalia = this.txtcd_sandalia.Text.ToString().Trim();
        ClsSandalia.NomeDaSandalia = this.txtnm_sandalia.Valor.ToString().Trim();
        ClsSandalia.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsSandalia.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsSandalia.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsSandalia.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsSandalia.Cor = this.txtcor.Valor.ToString().Trim();
        ClsSandalia.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsSandalia.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsSandalia.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsSandalia.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsSandalia.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsSandalia.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsSandalia.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsSandalia.Atualizar();
        //**************************

        if (ClsSandalia.critica != "")
        {
            Mensagem(ClsSandalia.critica.ToString());
        }
        lblGrid.Text = ClsSandalia.TrazGrid();

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
        this.txtcd_sandalia.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Sandálias para a Loja Virtual.";
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
        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());
        ClsSandalia.CodigoDaSandalia = this.txtcd_sandalia.Text.ToString().Trim();
        ClsSandalia.NomeDaSandalia = this.txtnm_sandalia.Valor.ToString().Trim();
        ClsSandalia.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsSandalia.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsSandalia.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsSandalia.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsSandalia.Cor = this.txtcor.Valor.ToString().Trim();
        ClsSandalia.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsSandalia.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsSandalia.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsSandalia.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsSandalia.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsSandalia.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsSandalia.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsSandalia.Grava();
        //*********************
        txtcd_sandalia.Text = ClsSandalia.CodigoDaSandalia.ToString();

        if (ClsSandalia.critica != "")
        {
            Mensagem(ClsSandalia.critica.ToString());
        }
        if (ClsSandalia.LimparCampos)
        {
            this.txtcd_sandalia.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsSandalia.TrazGrid();

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
        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsSandalia.CodigoDaSandalia = this.txtcd_sandalia.Text.ToString().Trim();

        resp = ClsSandalia.Consulta();
        //************************
        txtcd_sandalia.Text = ClsSandalia.CodigoDaSandalia.Trim();
        txtnm_sandalia.Valor = ClsSandalia.NomeDaSandalia.Trim();
        ddlprodutos.SelectedValue = ClsSandalia.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsSandalia.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsSandalia.CodigoDoMaterial.ToString();
        tamanho.Value = ClsSandalia.Tamanho.Trim();
        txtcor.Valor = ClsSandalia.Cor.Trim();
        txtpeso.Valor = ClsSandalia.Peso.ToString();
        txtpreco.Valor = ClsSandalia.Preco.ToString();
        chkfrete.Checked = ClsSandalia.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsSandalia.Novo == 1 ? true : false;
        chkativo.Checked = ClsSandalia.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsSandalia.Descricao.Trim();
        txtcomentario.Value = ClsSandalia.Comentario.Trim();
       
        if (ClsSandalia.critica != "")
        {
            Mensagem(ClsSandalia.critica.ToString());
        }
        lblGrid.Text = ClsSandalia.TrazGrid();

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
        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());
        this.txtnm_sandalia.Valor = "";
        ddlprodutos.SelectedValue = ClsSandalia.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsSandalia.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsSandalia.CodigoDoMaterial.ToString();
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
        Sandalia ClsSandalia = new Sandalia(Application["StrConexao"].ToString());

        ClsSandalia.CodigoDaSandalia = this.txtcd_sandalia.Text.ToString().Trim();

        resp = ClsSandalia.Excluir();
        //**********************

        txtcd_sandalia.Text = "";
        txtnm_sandalia.Valor = ClsSandalia.NomeDaSandalia.Trim();
        ddlprodutos.SelectedValue = ClsSandalia.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsSandalia.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsSandalia.CodigoDoMaterial.ToString();
        tamanho.Value = ClsSandalia.Tamanho.Trim();
        txtcor.Valor = ClsSandalia.Cor.Trim();
        txtpeso.Valor = ClsSandalia.Peso.ToString();
        txtpreco.Valor = ClsSandalia.Preco.ToString();
        chkfrete.Checked = ClsSandalia.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsSandalia.Novo == 1 ? true : false;
        chkativo.Checked = ClsSandalia.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsSandalia.Descricao.Trim();
        txtcomentario.Value = ClsSandalia.Comentario.Trim();
       
        if (ClsSandalia.critica != "")
        {
            Mensagem(ClsSandalia.critica.ToString());
        }
        lblGrid.Text = ClsSandalia.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_sandalia.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_sandalia.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
