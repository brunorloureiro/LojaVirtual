<%@ Page Language="C#" MasterPageFile="~/cliente/Cliente.master" CodeFile="login.aspx.cs" Inherits="login" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
        function validacep()
        {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("txtcep") >= 0)
               {
                    exp = /\d{2}\.\d{3}\-\d{3}/
                    if (obj[x].value != "" && obj[x].value != "0")
                    {
                       if(!exp.test(obj[x].value))
                       {
                           alert('Número do CEP Inválido.'); 
                           obj[x].value = "0"; 
                           obj[x].focus();            
                           return false;
                       }
                    }
                   return true;
               }
           }
        }
        
        function verificacliente()
        {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_cli") >= 0)
               {
                   var radcli = obj[x].checked;
               }
           }
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_novo") >= 0)
               {
                   if(radcli)
                   {
                       obj[x].checked = false;
                   }
               }
               if (nome.indexOf("txtsenha") >= 0)
               {
                  obj[x].disabled = false;
                  obj[x].focus();
               }
               if (nome.indexOf("txtcep") >= 0)
               {
                  obj[x].disabled = true;
                  obj[x].value = "0";
               }
            }
       }
       
       function verificaclientenovo()
       {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_novo") >= 0)
               {
                   var radnovo = obj[x].checked;
               }
           }
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_cli") >= 0)
               {
                   if(radnovo)
                   {
                       obj[x].checked = false;
                   }
               }
               if (nome.indexOf("txtcep") >= 0)
               {
                  obj[x].disabled = false;
                  obj[x].focus();
               }
               if (nome.indexOf("txtsenha") >= 0)
               {
                  obj[x].disabled = true;
                  obj[x].value = "";
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
                <span class="TituloPagina">Área do Cliente</span>
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
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo"> 
	                                    <asp:RadioButton ID="rad_cli" runat="server" text="Já sou cadastrado" onclick="verificacliente()" Checked/></td>
	                                <td style="width:50%;" class="LabelCampo">
		                                Senha: <asp:TextBox
			                                CssClass    = "password"
			                                id          = "txtsenha"
			                                style       = "width:80px" 
			                                runat       = "server" 
			                                maxlength   = "8"
			                                TextMode    = "Password"
			                                AutoCompleteType = "Disabled" />
	                                </td>
                                </tr>
                                 <tr align="left">
	                                <td class="LabelCampo"> 
	                                    <asp:RadioButton ID="rad_novo" text="Minha primeira compra" runat=server onclick="verificaclientenovo()"/></td>	 
	                                <td style="width:50%;" class="LabelCampo">
		                               CEP: &nbsp&nbsp&nbsp <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtcep" 
                                            runat="server" 
                                            TipoDeCampo="CEP"
                                            Largura="70" 
                                            Tamanho="10"
                                            OnBlur = "validacep()"
                                            Valor="0" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td > 
	                                <br />
	                               <span class="InfoInferior" >Esqueceu sua senha? <a href="esqueceusenha.aspx">Clique aqui</a></span>
	                               <br /><br />
	                                </td>
                                </tr>
                            </table>
                        </td>
                    </tr>			
                    <tr>
                        <td align="center" colspan="2">
                            <table class='button_bar' cellpadding='1' cellspacing='1'>
                                <tr>
                                    <td>
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "Continuar"
                                            id          = 'btn_entrar' 
                                            text        = 'Continuar'
                                            onclick     = 'entrar'
                                            runat       = "server" />
                                       </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblMsg"
                    CssClass    = "MsgSistema"
                    runat       = "server">&nbsp;</asp:Label>
            </td>
        </tr>
       </table>
    
</asp:Content>
