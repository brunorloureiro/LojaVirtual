<%@ Page Language="C#" MasterPageFile="~/cliente/ClienteSeco.master" CodeFile="esqueceusenha.aspx.cs" Inherits="esqueceusenha" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function chamatelalocal(url)
	    {
            window.location.href = url;
        }
        
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
    <style type="text/css">
         .style2
        {
            width: 196px;
        }
    </style>
</asp:Content>  
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
   <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <span class="TituloPagina">Esqueceu a Senha?</span>
                <table cellpadding="1" cellspacing="1" border="0" style="width: 357px">
                    <br />
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 124%;">
                                <tr align="left">
	                                <td class="LabelCampo" colspan=2>E-mail:&nbsp
		                                  <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtemail" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="200" 
                                            Tamanho="40" 
                                            OnBlur="validaremail()"/>
                                        &nbsp&nbsp
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "Continuar"
                                            id          = 'btn_entrar' 
                                            text        = 'Continuar'
                                            onclick     = 'entrar'
                                            runat       = "server" />
                                        &nbsp
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "Voltar"
                                            id          = 'Button1' 
                                            text        = 'Voltar'
                                            onclick     = 'voltar'
                                            runat       = "server" />
                                     </td>
                                </tr>
                            </table>
                        </td>
                    </tr>			
                </table>
            </td>
        </tr>
    </table>
    
</asp:Content>
