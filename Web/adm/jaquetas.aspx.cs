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

public partial class jaquetas : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());

        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsJaqueta.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Jaquetas para a Loja Virtual.";
    }

    
    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());
        ClsJaqueta.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsJaqueta.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
    }


    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }


    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());
        ClsJaqueta.CodigoDaJaqueta = this.txtcd_jaqueta.Text.ToString().Trim();
        ClsJaqueta.NomeDaJaqueta = this.txtnm_jaqueta.Valor.ToString().Trim();
        ClsJaqueta.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsJaqueta.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsJaqueta.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsJaqueta.Cor = this.txtcor.Valor.ToString().Trim();
        ClsJaqueta.Gorro = Convert.ToInt16(this.chkgorro.Checked);
        ClsJaqueta.Fechicle = Convert.ToInt16(this.chkfechicle.Checked);
        ClsJaqueta.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsJaqueta.LugarDaEstampa = this.estampa.Value.ToString().Trim();
        ClsJaqueta.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsJaqueta.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsJaqueta.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsJaqueta.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsJaqueta.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsJaqueta.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsJaqueta.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsJaqueta.Atualizar();
        //**************************

        if (ClsJaqueta.critica != "")
        {
            Mensagem(ClsJaqueta.critica.ToString());
        }
        lblGrid.Text = ClsJaqueta.TrazGrid();

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
        this.txtcd_jaqueta.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Jaquetas para a Loja Virtual.";
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
        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());
        ClsJaqueta.CodigoDaJaqueta = this.txtcd_jaqueta.Text.ToString().Trim();
        ClsJaqueta.NomeDaJaqueta = this.txtnm_jaqueta.Valor.ToString().Trim();
        ClsJaqueta.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsJaqueta.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsJaqueta.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsJaqueta.Gorro = Convert.ToInt16(this.chkgorro.Checked);
        ClsJaqueta.Fechicle = Convert.ToInt16(this.chkfechicle.Checked);
        ClsJaqueta.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsJaqueta.LugarDaEstampa = this.estampa.Value.ToString().Trim();
        ClsJaqueta.Cor = this.txtcor.Valor.ToString().Trim();
        ClsJaqueta.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsJaqueta.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsJaqueta.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsJaqueta.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsJaqueta.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsJaqueta.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsJaqueta.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsJaqueta.Grava();
        //*********************
        txtcd_jaqueta.Text = ClsJaqueta.CodigoDaJaqueta.ToString();

        if (ClsJaqueta.critica != "")
        {
            Mensagem(ClsJaqueta.critica.ToString());
        }
        if (ClsJaqueta.LimparCampos)
        {
            this.txtcd_jaqueta.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsJaqueta.TrazGrid();

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
        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsJaqueta.CodigoDaJaqueta = this.txtcd_jaqueta.Text.ToString().Trim();

        resp = ClsJaqueta.Consulta();
        //************************
        txtcd_jaqueta.Text = ClsJaqueta.CodigoDaJaqueta.Trim();
        txtnm_jaqueta.Valor = ClsJaqueta.NomeDaJaqueta.Trim();
        ddlprodutos.SelectedValue = ClsJaqueta.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsJaqueta.CodigoDaMarca.ToString();
        tamanho.Value = ClsJaqueta.Tamanho.Trim();
        txtcor.Valor = ClsJaqueta.Cor.Trim();
        chkgorro.Checked = ClsJaqueta.Gorro == 1 ? true : false;
        chkfechicle.Checked = ClsJaqueta.Fechicle == 1 ? true : false;
        chkestampa.Checked = ClsJaqueta.Estampa == 1 ? true : false;
        estampa.Value = ClsJaqueta.LugarDaEstampa.Trim();
        txtpeso.Valor = ClsJaqueta.Peso.ToString();
        txtpreco.Valor = ClsJaqueta.Preco.ToString();
        chkfrete.Checked = ClsJaqueta.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsJaqueta.Novo == 1 ? true : false;
        chkativo.Checked = ClsJaqueta.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsJaqueta.Descricao.Trim();
        txtcomentario.Value = ClsJaqueta.Comentario.Trim();
       
        if (ClsJaqueta.critica != "")
        {
            Mensagem(ClsJaqueta.critica.ToString());
        }
        lblGrid.Text = ClsJaqueta.TrazGrid();

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
        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());
        this.txtnm_jaqueta.Valor = "";
        ddlprodutos.SelectedValue = ClsJaqueta.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsJaqueta.CodigoDaMarca.ToString();
        this.tamanho.Value = "PP";
        this.txtcor.Valor = "";
        this.chkgorro.Checked = false;
        this.chkfechicle.Checked = false;
        this.chkestampa.Checked = false;
        this.estampa.Value = "Sem Estampa";
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
        Jaqueta ClsJaqueta = new Jaqueta(Application["StrConexao"].ToString());

        ClsJaqueta.CodigoDaJaqueta = this.txtcd_jaqueta.Text.ToString().Trim();

        resp = ClsJaqueta.Excluir();
        //**********************

        txtcd_jaqueta.Text = "";
        txtnm_jaqueta.Valor = ClsJaqueta.NomeDaJaqueta.Trim();
        ddlprodutos.SelectedValue = ClsJaqueta.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsJaqueta.CodigoDaMarca.ToString();
        tamanho.Value = ClsJaqueta.Tamanho.Trim();
        txtcor.Valor = ClsJaqueta.Cor.Trim();
        chkgorro.Checked = ClsJaqueta.Gorro == 1 ? true : false;
        chkfechicle.Checked = ClsJaqueta.Fechicle == 1 ? true : false;
        chkestampa.Checked = ClsJaqueta.Estampa == 1 ? true : false;
        estampa.Value = ClsJaqueta.LugarDaEstampa.Trim();
        txtpeso.Valor = ClsJaqueta.Peso.ToString();
        txtpreco.Valor = ClsJaqueta.Preco.ToString();
        chkfrete.Checked = ClsJaqueta.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsJaqueta.Novo == 1 ? true : false;
        chkativo.Checked = ClsJaqueta.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsJaqueta.Descricao.Trim();
        txtcomentario.Value = ClsJaqueta.Comentario.Trim();
       
        if (ClsJaqueta.critica != "")
        {
            Mensagem(ClsJaqueta.critica.ToString());
        }
        lblGrid.Text = ClsJaqueta.TrazGrid();

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

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

    public void NovoImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_jaqueta.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_jaqueta.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
