<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" AutoEventWireup="true" CodeFile="retorno.aspx.cs" Inherits="Retorno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 676px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
  <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server">Compra Realizada com Sucesso</asp:Label>
    <br /><br /><br />
    <div style="text-align: justify; ">
        <table align=center border=0>
            <tr>
                <td class="style1">
                <b> Obrigado por comprar no LuzOnline.</b><br/>
                <br /><br />
                <b>Após a confirmação do Pagamento pelo PagSeguro o seu pedido será enviado ou entregue. Acompanhe o andamento do seu Pedido através do <a href="../cliente/login.aspx">Minha Página</a>.</b><br/>
                <br />
                <b> Desde já agradecemos pela preferência.</b><br/>
                <br /><br />
                <b> Atenciosamente,</b><br/><br />
                <b> Equipe LuzOnline</b><br/>
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
</asp:Content>


<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Retorno.aspx.cs" Inherits="Retorno" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::.. Recebimento PagSeguro ..::</title>
</head>

<script type="text/javascript">
    var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
    document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
    try {
        var pageTracker = _gat._getTracker("UA-8436585-1");
        pageTracker._trackPageview();
    } catch(err) {}
</script>

<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
--%>