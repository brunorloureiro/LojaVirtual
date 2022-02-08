<%@ Page Language="C#" CodeFile="aviseme.aspx.cs" Inherits="avise" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Avise-Me</title>
    <meta content="text/html; charset=iso-8859-1" http-equiv="Content-Type" />
    <link id="Link1" rel="stylesheet" href="../css/luz.css" runat="server" />
    <script type="text/javascript" src="../js/mascara.js"></script>
    <script language="javascript" type="text/javascript">
        function validaremail()
        {
            var obj = window.document.getElementsByTagName("input");
            for (x=0; x<obj.length; x++)
            {
                var nome = obj[x].name;
                if (nome.indexOf("txtemail") >= 0)
                {
                   exp = /^[\w-]+(\.[\w-]+)*@(([\w-]{2,63}\.)+[A-Za-z]{2,6}|\[\d{1,3}(\.\d{1,3}){3}\])$/
                   if (obj[x].value != "")
                   {
                       if (!exp.test(obj[x].value))
                       {
                           if (!confirm('E-mail Inválido. Confirma digitação?'))
                           {
                               obj[x].value = ""; 
                               obj[x].focus();
                               return false;
                           }
                       }
                       return true;
                   }
                }
            }
        }
    </script>
</head>
<body >
    <form id="form1" runat="server">
    <div align=center>
    <span class="TituloPagina">Avise-Me</span>
        <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
            <tr align="left">
                <td class="LabelCampo" style="width:35%;">Nome:&nbsp&nbsp</td>
                <td style="width:65%;">
                    <cc1:CustomTextBox 
                        ClasseReferenciaCss="text" 
                        ID="txtnome" 
                        runat="server" 
                        TipoDeCampo="Livre" 
                        Largura="250" 
                        Tamanho="40" />
                 </td>
            </tr>
            <br/><br/>
            <tr align="left">
                <td class="LabelCampo" style="width:35%;">E-mail:</td>
                <td style="width:65%;"> 
                  <cc1:CustomTextBox 
                        ClasseReferenciaCss="text" 
                        ID="txtemail" 
                        runat="server" 
                        TipoDeCampo="Livre"
                        Largura="200" 
                        Tamanho="50" 
                        OnBlur="validaremail()"/>
                </td>
             </tr>
             <br /><br />
             <tr>
                <td align="center" colspan="2">
                    <table class='button_bar' cellpadding='1' cellspacing='1'>
                        <tr>
                            <td>
                                <asp:Button 
                                    CssClass    = 'BtnNormal' 
                                    alt         = "salvar"
                                    id          = 'btn_salvar'
                                    text        = 'Enviar'
                                    onclick     = 'salvar'
                                    runat       = "server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
       </table>
    </form>
    </div>
</body>
</html>