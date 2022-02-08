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

public partial class Retorno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            //o método POST indica que a requisição é o retorno da validação NPI.

            string Token = "BC42300AB50A4BC7A52C029E018FF5E3";
            string Pagina = "https://pagseguro.uol.com.br/pagseguro-ws/checkout/NPI.jhtml";
            string Dados = HttpContext.Current.Request.Form.ToString() + "&Comando=validar" + "&Token=" + Token;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Pagina);

            req.Method = "POST";
            req.ContentLength = Dados.Length;
            req.ContentType = "application/x-www-form-urlencoded";

            System.IO.StreamWriter stOut = new System.IO.StreamWriter(req.GetRequestStream(), System.Text.Encoding.GetEncoding("ISO-8859-1"));
            stOut.Write(Dados);
            stOut.Close();

            System.IO.StreamReader stIn = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding("ISO-8859-1"));
            string Result = stIn.ReadToEnd();
            stIn.Close();

            string p_referencia = (Request["Referencia"] != null) ? (string)Request["Referencia"] : "Referencia";
            string id_transacao = (Request["TransacaoID"] != null) ? (string)Request["TransacaoID"] : "Transacao";
            string st_pagamento = (Request["StatusTransacao"] != null) ? (string)Request["StatusTransacao"] : "Status Pagamento";
            string tp_pagamento = (Request["TipoPagamento"] != null) ? (string)Request["TipoPagamento"] : "Tipo Pagamento";
            
            if (Result == "VERIFICADO")
            {
                AcessoDados ClsAcessoDados = new AcessoDados(Application["StrConexao"].ToString());
                ClsAcessoDados.AtualizarVenda(p_referencia, id_transacao, st_pagamento, tp_pagamento);

                //o post foi validado
            }
            else if (Result == "FALSO")
            {
                //o post nao foi validado
                //Response.Write("DEU PAU"); ;
            }
            else
            {
                //erro na integração com PagSeguro.
            }
        }
        else if (Request.HttpMethod == "GET")
        {
            //o método GET indica que a requisição é o retorno do Checkout PagSeguro para o site vendedor.
            //no término do checkout o usuário é redirecionado para este bloco.
        }
    }

    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + "));</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }
}


//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System;

//public partial class Retorno : System.Web.UI.Page
//{

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        //Valor somente para testes 
//        this.RetornoPagSeguro1.UrlNPI = "https://pagseguro.uol.com.br/pagseguro-ws/checkout/NPI.jhtml";
//        AcessoDados acessoDados = new AcessoDados(Application["StrConexao"].ToString());
//    }

//    protected void RetornoPagSeguro1_VendaEfetuada(UOL.PagSeguro.RetornoVenda retornoVenda)
//    {
//        //Obtendo o número do Pedido 
//        int codigo_pedido = int.Parse(retornoVenda.CodigoReferencia);

//        //Obtendo o código da transação no PagSeguro 
//        string codigo_transacao_pagseguro = retornoVenda.CodigoTransacao;

//        //Obtendo o novo status do pedido 
//        string status = retornoVenda.StatusTransacaoDescricao;

//        //Obtendo a forma de pagamento utilizada 
//        string tipo_pagamento = retornoVenda.TipoPagamentoDescricao;

//      //  Mensagem(retornoVenda.TipoPagamentoDescricao);

//        //Obtendo o valor pago pelo frete 
//        double frete_cobrado = retornoVenda.ValorFrete;

//        //Obtendo a anotação deixada pelo cliente no momento do pagamento 
//        string anotacao_cliente = retornoVenda.Anotacao;

//        //Atualizando na base de dados o status do pedido e as outras informações 
//        AcessoDados acessoDados = new AcessoDados(Application["StrConexao"].ToString());
//        acessoDados.AtualizarVenda(codigo_pedido, codigo_transacao_pagseguro, status, tipo_pagamento, frete_cobrado, anotacao_cliente);

//    }

//    public void Mensagem(string msg)
//    {
//        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
//        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
//    }
//} 


