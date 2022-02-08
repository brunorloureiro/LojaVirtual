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

public partial class lencos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsLenco.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Lenços/Cachecóis para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());
        ClsLenco.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsLenco.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
        ClsLenco.CarregaLista(this.ddlmateriais, "Material", (Request["ddlmateriais"] == null ? "0" : (string)Request["ddlmateriais"]), "Escolha uma Material");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());
        ClsLenco.CodigoDoLenco = this.txtcd_lenco.Text.ToString().Trim();
        ClsLenco.NomeDoLenco = this.txtnm_lenco.Valor.ToString().Trim();
        ClsLenco.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsLenco.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsLenco.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsLenco.Cor = this.txtcor.Valor.ToString().Trim();
        ClsLenco.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsLenco.Comprimento = Convert.ToDecimal(this.txtcomprim.Valor.Replace(".", ","));
        ClsLenco.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsLenco.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsLenco.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsLenco.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsLenco.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsLenco.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsLenco.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsLenco.Atualizar();
        //**************************

        if (ClsLenco.critica != "")
        {
            Mensagem(ClsLenco.critica.ToString());
        }
        lblGrid.Text = ClsLenco.TrazGrid();

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
        this.txtcd_lenco.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Lenços/Cachecóis para a Loja Virtual.";
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
        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());
        ClsLenco.CodigoDoLenco = this.txtcd_lenco.Text.ToString().Trim();
        ClsLenco.NomeDoLenco = this.txtnm_lenco.Valor.ToString().Trim();
        ClsLenco.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsLenco.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsLenco.CodigoDoMaterial = Convert.ToInt16(this.ddlmateriais.SelectedValue);
        ClsLenco.Cor = this.txtcor.Valor.ToString().Trim();
        ClsLenco.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsLenco.Comprimento = Convert.ToDecimal(this.txtcomprim.Valor.Replace(".", ","));
        ClsLenco.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsLenco.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsLenco.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsLenco.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsLenco.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsLenco.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsLenco.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsLenco.Grava();
        //*********************
        txtcd_lenco.Text = ClsLenco.CodigoDoLenco.ToString();

        if (ClsLenco.critica != "")
        {
            Mensagem(ClsLenco.critica.ToString());
        }
        if (ClsLenco.LimparCampos)
        {
            this.txtcd_lenco.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsLenco.TrazGrid();

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
        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsLenco.CodigoDoLenco = this.txtcd_lenco.Text.ToString().Trim();

        resp = ClsLenco.Consulta();
        //************************
        txtcd_lenco.Text = ClsLenco.CodigoDoLenco.Trim();
        txtnm_lenco.Valor = ClsLenco.NomeDoLenco.Trim();
        ddlprodutos.SelectedValue = ClsLenco.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsLenco.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsLenco.CodigoDoMaterial.ToString();
        txtcor.Valor = ClsLenco.Cor.Trim();
        chkestampa.Checked = ClsLenco.Estampa == 1 ? true : false;
        txtcomprim.Valor = ClsLenco.Comprimento.ToString();
        txtpeso.Valor = ClsLenco.Peso.ToString();
        txtpreco.Valor = ClsLenco.Preco.ToString();
        chkfrete.Checked = ClsLenco.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsLenco.Novo == 1 ? true : false;
        chkativo.Checked = ClsLenco.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsLenco.Descricao.Trim();
        txtcomentario.Value = ClsLenco.Comentario.Trim();
       
        if (ClsLenco.critica != "")
        {
            Mensagem(ClsLenco.critica.ToString());
        }
        lblGrid.Text = ClsLenco.TrazGrid();

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
        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());
        this.txtnm_lenco.Valor = "";
        ddlprodutos.SelectedValue = ClsLenco.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsLenco.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsLenco.CodigoDoMaterial.ToString();
        this.txtcor.Valor = "";
        this.chkestampa.Checked = false;
        this.txtcomprim.Valor = "0";
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
        Lenco ClsLenco = new Lenco(Application["StrConexao"].ToString());

        ClsLenco.CodigoDoLenco = this.txtcd_lenco.Text.ToString().Trim();

        resp = ClsLenco.Excluir();
        //**********************

        txtcd_lenco.Text = "";
        txtnm_lenco.Valor = ClsLenco.NomeDoLenco.Trim();
        ddlprodutos.SelectedValue = ClsLenco.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsLenco.CodigoDaMarca.ToString();
        ddlmateriais.SelectedValue = ClsLenco.CodigoDoMaterial.ToString();
        txtcor.Valor = ClsLenco.Cor.Trim();
        chkestampa.Checked = ClsLenco.Estampa == 1 ? true : false;
        txtcomprim.Valor = ClsLenco.Comprimento.ToString();
        txtpeso.Valor = ClsLenco.Peso.ToString();
        txtpreco.Valor = ClsLenco.Preco.ToString();
        chkfrete.Checked = ClsLenco.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsLenco.Novo == 1 ? true : false;
        chkativo.Checked = ClsLenco.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsLenco.Descricao.Trim();
        txtcomentario.Value = ClsLenco.Comentario.Trim();
       
        if (ClsLenco.critica != "")
        {
            Mensagem(ClsLenco.critica.ToString());
        }
        lblGrid.Text = ClsLenco.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_lenco.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "/web/Images/Produtos/1/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_lenco.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_lenco.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
