<%@ Page Language="C#" AutoEventWireup="true" CodeFile="redireciona.aspx.cs" Inherits="redireciona" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>... LuzOnline - A moda iluminando você ...</title>
</head>

<script type="text/javascript">
    function fecha()
    {
        window.opener = window;
        window.close();
    }
</script>
<script type="text/javascript">
    try {
        var pageTracker = _gat._getTracker("UA-8436585-1");
        pageTracker._trackPageview();
    } catch(err) {}
</script>

<body>
    <form id="Form1" target="PagSeguro" action="https://pagseguro.uol.com.br/security/webpagamentos/webpagto.aspx" method="post" runat="server">
    <div>
        <input type="hidden" name="email_cobranca" value="diego_tironi@yahoo.com.br" />
        <input type="hidden" name="tipo" value="CP" />
        <input type="hidden" name="moeda" value="BRL" />
        <input type="hidden" name="cliente_pais" value="BRA" />
        <asp:Label
            ID          = "lblhiddens"
            runat       = "server"></asp:Label>
    </div>
    </form>
</body>
</html>
