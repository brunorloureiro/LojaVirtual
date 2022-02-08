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

public partial class bones : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Bone ClsBone = new Bone(Application["StrConexao"].ToString());
        
        if (!IsPostBack)
        {
            if ((bool)Session["bl_consulta"] == true)
            {
                lblGrid.Text = ClsBone.TrazGrid();
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

        this.lblMsg.Text = "Cadastro de Bonés para a Loja Virtual.";
    }

    public void carregaLista(object sender, EventArgs e)
    {
        this.btn_atualizar.Enabled = false;
        this.btn_salvar.Enabled = !false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;

        Bone ClsBone = new Bone(Application["StrConexao"].ToString());
        ClsBone.CarregaLista(this.ddlprodutos, "Tpprod", (Request["ddlprodutos"] == null ? "0" : (string)Request["ddlprodutos"]), "Escolha um Tipo de Produto");
        ClsBone.CarregaLista(this.ddlmarcas, "Marca", (Request["ddlmarcas"] == null ? "0" : (string)Request["ddlmarcas"]), "Escolha uma Marca");
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Bone ClsBone = new Bone(Application["StrConexao"].ToString());
        ClsBone.CodigoDoBone = this.txtcd_bone.Text.ToString().Trim();
        ClsBone.NomeDoBone = this.txtnm_bone.Valor.ToString().Trim();
        ClsBone.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsBone.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsBone.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsBone.Cor = this.txtcor.Valor.ToString().Trim();
        ClsBone.Fecho = Convert.ToInt16(this.chkfecho.Checked);
        ClsBone.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsBone.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsBone.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".",","));
        ClsBone.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsBone.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsBone.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsBone.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsBone.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsBone.Atualizar();
        //**************************

        if (ClsBone.critica != "")
        {
            Mensagem(ClsBone.critica.ToString());
        }
        lblGrid.Text = ClsBone.TrazGrid();

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
        this.txtcd_bone.Text = "0";
        LimpaCampo();
        this.lblMsg.Text = "Cadastro de Bonés para a Loja Virtual.";
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
        Bone ClsBone = new Bone(Application["StrConexao"].ToString());
        ClsBone.CodigoDoBone = this.txtcd_bone.Text.ToString().Trim();
        ClsBone.NomeDoBone = this.txtnm_bone.Valor.ToString().Trim();
        ClsBone.CodigoDoTipoDeProduto = Convert.ToInt16(this.ddlprodutos.SelectedValue);
        ClsBone.CodigoDaMarca = Convert.ToInt16(this.ddlmarcas.SelectedValue);
        ClsBone.Tamanho = this.tamanho.Value.ToString().Trim();
        ClsBone.Fecho = Convert.ToInt16(this.chkfecho.Checked);
        ClsBone.Estampa = Convert.ToInt16(this.chkestampa.Checked);
        ClsBone.Cor = this.txtcor.Valor.ToString().Trim();
        ClsBone.Peso = Convert.ToDecimal(this.txtpeso.Valor.Replace(".", ","));
        ClsBone.Preco = Convert.ToDecimal(this.txtpreco.Valor.Replace(".", ","));
        ClsBone.FreteGratis = Convert.ToInt16(this.chkfrete.Checked);
        ClsBone.Novo = Convert.ToInt16(this.chknovo.Checked);
        ClsBone.Ativo = Convert.ToInt16(this.chkativo.Checked);
        ClsBone.Descricao = this.txtdescricao.Value.ToString().Trim();
        ClsBone.Comentario = this.txtcomentario.Value.ToString().Trim();
        
        resp = ClsBone.Grava();
        //*********************
        txtcd_bone.Text = ClsBone.CodigoDoBone.ToString();

        if (ClsBone.critica != "")
        {
            Mensagem(ClsBone.critica.ToString());
        }
        if (ClsBone.LimparCampos)
        {
            this.txtcd_bone.Text = "";
            LimpaCampo();
        }
        lblGrid.Text = ClsBone.TrazGrid();

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
        Bone ClsBone = new Bone(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsBone.CodigoDoBone = this.txtcd_bone.Text.ToString().Trim();

        resp = ClsBone.Consulta();
        //************************
        txtcd_bone.Text = ClsBone.CodigoDoBone.Trim();
        txtnm_bone.Valor = ClsBone.NomeDoBone.Trim();
        ddlprodutos.SelectedValue = ClsBone.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsBone.CodigoDaMarca.ToString();
        tamanho.Value = ClsBone.Tamanho.Trim();
        txtcor.Valor = ClsBone.Cor.Trim();
        chkfecho.Checked = ClsBone.Fecho == 1 ? true : false;
        chkestampa.Checked = ClsBone.Estampa == 1 ? true : false; 
        txtpeso.Valor = ClsBone.Peso.ToString();
        txtpreco.Valor = ClsBone.Preco.ToString();
        chkfrete.Checked = ClsBone.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsBone.Novo == 1 ? true : false;
        chkativo.Checked = ClsBone.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsBone.Descricao.Trim();
        txtcomentario.Value = ClsBone.Comentario.Trim();
       
        if (ClsBone.critica != "")
        {
            Mensagem(ClsBone.critica.ToString());
        }
        lblGrid.Text = ClsBone.TrazGrid();

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
        Bone ClsBone = new Bone(Application["StrConexao"].ToString());
        this.txtnm_bone.Valor = "";
        ddlprodutos.SelectedValue = ClsBone.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsBone.CodigoDaMarca.ToString();
        this.tamanho.Value = "PP";
        this.txtcor.Valor = "";
        this.chkfecho.Checked = false;
        this.chkestampa.Checked = false; 
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
        Bone ClsBone = new Bone(Application["StrConexao"].ToString());

        ClsBone.CodigoDoBone = this.txtcd_bone.Text.ToString().Trim();

        resp = ClsBone.Excluir();
        //**********************

        txtcd_bone.Text = "";
        txtnm_bone.Valor = ClsBone.NomeDoBone.Trim();
        ddlprodutos.SelectedValue = ClsBone.CodigoDoTipoDeProduto.ToString();
        ddlmarcas.SelectedValue = ClsBone.CodigoDaMarca.ToString();
        tamanho.Value = ClsBone.Tamanho.Trim();
        txtcor.Valor = ClsBone.Cor.Trim();
        chkfecho.Checked = ClsBone.Fecho == 1 ? true : false;
        chkestampa.Checked = ClsBone.Estampa == 1 ? true : false;
        txtpeso.Valor = ClsBone.Peso.ToString();
        txtpreco.Valor = ClsBone.Preco.ToString();
        chkfrete.Checked = ClsBone.FreteGratis == 1 ? true : false;
        chknovo.Checked = ClsBone.Novo == 1 ? true : false;
        chkativo.Checked = ClsBone.Ativo == 1 ? true : false; 
        txtdescricao.Value = ClsBone.Descricao.Trim();
        txtcomentario.Value = ClsBone.Comentario.Trim();
       
        if (ClsBone.critica != "")
        {
            Mensagem(ClsBone.critica.ToString());
        }
        lblGrid.Text = ClsBone.TrazGrid();

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
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_bone.Text.ToString() + ".jpg";
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
                    this.fluProduto1.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg");
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
                    this.fluProduto2.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg");
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
                    this.fluProduto3.SaveAs("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg");
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
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg");

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void SalvaImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }

    public void ProcuraImagens()
    {
        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        } 
    }

    public void excluirfoto(object sender, EventArgs e)
    {
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg");
        File.Delete("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg");

        this.btn_atualizar.Enabled = true;
        this.btn_salvar.Enabled = false;
        this.btn_excluir.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto1.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto2.Enabled = this.btn_atualizar.Enabled;
        this.fluProduto3.Enabled = this.btn_atualizar.Enabled;
        this.btn_excluirfoto.Enabled = this.btn_atualizar.Enabled;

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto1.ImageUrl = "../Images/Produtos/1/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto1.Visible = true;
        }
        else
        {
            imgProduto1.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto2.ImageUrl = "../Images/Produtos/2/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto2.Visible = true;
        }
        else
        {
            imgProduto2.Visible = false;
        }

        if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + this.txtcd_bone.Text.ToString() + ".jpg"))
        {
            imgProduto3.ImageUrl = "../Images/Produtos/3/" + this.txtcd_bone.Text.ToString() + ".jpg";
            imgProduto3.Visible = true;
        }
        else
        {
            imgProduto3.Visible = false;
        }
    }


}
