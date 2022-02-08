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

public partial class mostraprodutos : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        string produto, titulo = "";
        int cd_genero = 0;

        produto = Request["produto"];
        
        //1 - Masculino
        //2 - Feminino
        //4 - Unissex

        # region Gêneros
     
        if (Request["genero"] == "U")
        {
            cd_genero = 4;
        }
        else
        {
            if (Request["genero"] == "F")
            {
                cd_genero = 2;
            }
            else
            {
                cd_genero = 1;
            }
        }

        # endregion

        # region Produtos

        if (Request["produto"] == "BER")
        {
            if (cd_genero == 4)
            {
                titulo = "Bermudas Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Bermudas Masculinas";
                }
                else
                {
                    titulo = "Bermudas Femininas";
                }
            }
        }
        if (Request["produto"] == "BOL")
        {
            if (cd_genero == 4)
            {
                titulo = "Bolsas Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Bolsas Masculinas";
                }
                else
                {
                    titulo = "Bolsas Femininas";
                }
            }
        }
        if (Request["produto"] == "BON")
        {
            if (cd_genero == 4)
            {
                titulo = "Bonés Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Bonés Masculinos";
                }
                else
                {
                    titulo = "Bonés Femininos";
                }
            }
        }
        if (Request["produto"] == "CAL")
        {
            if (cd_genero == 4)
            {
                titulo = "Calças Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Calças Masculinas";
                }
                else
                {
                    titulo = "Calças Femininas";
                }
            }
        }
        if (Request["produto"] == "CAM")
        {
            if (cd_genero == 4)
            {
                titulo = "Camisas Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Camisas Masculinas";
                }
                else
                {
                    titulo = "Camisas Femininas";
                }
            }
        }
        if (Request["produto"] == "CAR")
        {
            if (cd_genero == 4)
            {
                titulo = "Carteiras Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Carteiras Masculinas";
                }
                else
                {
                    titulo = "Carteiras Femininas";
                }
            }
        }
        if (Request["produto"] == "JAQ")
        {
            if (cd_genero == 4)
            {
                titulo = "Jaquetas Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Jaquetas Masculinas";
                }
                else
                {
                    titulo = "Jaquetas Femininas";
                }
            }
        }
        if (Request["produto"] == "PRA")
        {
            if (cd_genero == 4)
            {
                titulo = "Pratas Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Pratas Masculinas";
                }
                else
                {
                    titulo = "Pratas Femininas";
                }
            }
        }
        if (Request["produto"] == "TEN")
        {
            if (cd_genero == 4)
            {
                titulo = "Tênis Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Tênis Masculinos";
                }
                else
                {
                    titulo = "Tênis Femininos";
                }
            }
        }
        if (Request["produto"] == "VIC")
        {
            if (cd_genero == 4)
            {
                titulo = "Victória Secrets Unissex";

            }
            else
            {
                if (cd_genero == 1)
                {
                    titulo = "Victória Secrets Masculinos";
                }
                else
                {
                    titulo = "Victória Secrets Femininos";
                }
            }
        }
       
        # endregion

        lblTitulo.Text = titulo;
        lblPrincipal.Text = ClsLoja.CarregaMostraProdutos(produto, cd_genero);
        lblNenhum.Style.Add("display", ClsLoja.TrazNumeroDeRegistros() == true ? "none" : "");
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }

}
