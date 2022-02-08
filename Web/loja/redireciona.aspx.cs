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
using System.Net;
using System.IO;
using System.Text;

public partial class redireciona : System.Web.UI.Page
{
    public int tamanho = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        ChamaPagSeguro();
    }

    public void ChamaPagSeguro()
    {
        int codigo = 0;
        string StrHidden = "";
        string CssUsado = "";

        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());

        for (int x = 1; x < 50; x++)
        {
            if (Session["cd_produto(" + x + ")"] != null)
            {
                if (Session["cd_produto(" + x + ")"] != "")
                {
                    StrHidden += "<input type='hidden' name='item_id_" + x + "' value='" + Session["cd_produto(" + x + ")"] + "' />";
                    StrHidden += "<input type='hidden' name='item_descr_" + x + "' value='" + Session["nm_produto(" + x + ")"] + "' />";
                    StrHidden += "<input type='hidden' name='item_quant_" + x + "' value='" + Session["quantidade(" + x + ")"] + "' />";
                    tamanho = Session["preco(" + x + ")"].ToString().Length;
                    if (Session["preco(" + x + ")"].ToString()[tamanho - 2] == ',')
                    {
                        StrHidden += "<input type='hidden' name='item_valor_" + x + "' value='" + Session["preco(" + x + ")"].ToString().Replace(",", "").Replace(".", "") + "0' />";
                    }
                    else
                    {
                        StrHidden += "<input type='hidden' name='item_valor_" + x + "' value='" + Session["preco(" + x + ")"].ToString().Replace(",", "").Replace(".", "") + "' />";
                    }
                }
            }

        }
        if (Request["frete"] == "SD")
        {
            StrHidden += "<input type='hidden' name='item_id_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='SD' />";
            StrHidden += "<input type='hidden' name='item_descr_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='FRETE POR SEDEX' />";
        }
        else
        {
            if (Request["frete"] == "EN")
            {
                StrHidden += "<input type='hidden' name='item_id_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='EN' />";
                StrHidden += "<input type='hidden' name='item_descr_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='FRETE POR PAC' />";
            }
            else
            {
                StrHidden += "<input type='hidden' name='item_id_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='MB' />";
                StrHidden += "<input type='hidden' name='item_descr_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='ENTREGA VIA MOTOBOY' />";
            }
        }

        StrHidden += "<input type='hidden' name='item_quant_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='1' />";
        if (Session["vl_totfrete"] != null)
        {
            if (Session["vl_totfrete"].ToString() != "" && Session["vl_totfrete"].ToString() != "0")
            {
                tamanho = Session["vl_totfrete"].ToString().Length;
                if (Session["vl_totfrete"].ToString()[tamanho - 2] == ',')
                {
                    StrHidden += "<input type='hidden' name='item_valor_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='" + Session["vl_totfrete"].ToString().Replace(",", "").Replace(".", "") + "0' />";
                }
                else
                {
                    StrHidden += "<input type='hidden' name='item_valor_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='" + Session["vl_totfrete"].ToString().Replace(",", "").Replace(".", "") + "' />";
                }
            }
            else
            {
                StrHidden += "<input type='hidden' name='item_valor_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='1' />";
            }
        }
        else
        {
            StrHidden += "<input type='hidden' name='item_valor_" + (Convert.ToInt32(Session["total_prod"]) + 1) + "' value='1' />";
        }


        if (Request["frete"] == "SD")
        {
            StrHidden += "<input type='hidden' name='tipo_frete' value='SD' />";
        }
        else
        {
            StrHidden += "<input type='hidden' name='tipo_frete' value='EN' />";
        }
        StrHidden += "<input type='hidden' name='ref_transacao' value='" + Session["codigovenda"].ToString() + "' />";
        StrHidden += "<input type='hidden' name='cliente_nome' value='" + Session["usernomecliente"].ToString() + "' />";
        StrHidden += "<input type='hidden' name='cliente_cep' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "cep") + "' />";
        StrHidden += "<input type='hidden' name='cliente_end' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "endereco") + "' />";
        StrHidden += "<input type='hidden' name='cliente_num' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "numero") + "' />";
        StrHidden += "<input type='hidden' name='cliente_compl' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "complemento") + "' />";
        StrHidden += "<input type='hidden' name='cliente_bairro' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "bairro") + "' />";
        StrHidden += "<input type='hidden' name='cliente_cidade' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "cidade") + "' />";
        StrHidden += "<input type='hidden' name='cliente_uf' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "uf") + "' />";
        StrHidden += "<input type='hidden' name='cliente_ddd' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "ddd") + "' />";
        StrHidden += "<input type='hidden' name='cliente_tel' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "tel") + "' />";
        StrHidden += "<input type='hidden' name='cliente_email' value='" + ClsLoja.TrazInfoCliente(Session["cd_cliente"].ToString(), "email") + "' />";

        this.lblhiddens.Text = StrHidden.ToString().Trim();
      
        string script = "<script type='text/javascript' language='javascript'>parent.document.forms[0].submit();</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "chama", script);

        script = "<script type='text/javascript' language='javascript'>fecha();</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "fecha", script);
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }
}
