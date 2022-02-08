<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" AutoEventWireup="true" CodeFile="faleconosco.aspx.cs" Inherits="faleconosco" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
	    function trim(valor)
	    {
		    var str_valor = new String(valor);
	        return str_valor.replace(/(^\s*)|(\s*$)/g, "");
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
 </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server">Fale Conosco</asp:Label>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;" > 
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 500px;">
                                 <tr  align="left">
	                                <td class="LabelCampo" style="width:30%;">Nome:</td>
	                                <td style="width:70%;">
	                                    <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="CustomTextBox1" 
                                            alt="txtnome" 
                                            runat="server"
                                            TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="35" />
                                    </td>
                                </tr>
                                <tr  align="left">
	                                <td class="LabelCampo" style="width:30%;">Nome:</td>
	                                <td style="width:70%;">
	                                    <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtnome" 
                                            alt="txtnome" 
                                            runat="server"
                                            TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="35" />
                                    </td>
                                </tr>
                                <br />
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Telefone:</td>
	                                <td style="width:70%;">
                                         <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtddd_tel" 
                                            runat="server" 
                                            TipoDeCampo="Livre"
                                            Largura="35" 
                                            Tamanho="4" />
                                        <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txttelefone" 
                                            alt="txttelefone"
                                            runat="server" 
                                            TipoDeCampo="Telefone"
                                            Tamanho = "9"
                                            Largura="65" 
                                            Valor="0"/>
                                    </td>
                              </tr>
                              <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Celular:</td>
	                                <td style="width:70%;">
                                         <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtddd_cel" 
                                            runat="server" 
                                            TipoDeCampo="Livre"
                                            Largura="35" 
                                            Tamanho="4" />
                                        <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtcelular" 
                                            alt="txtcelular"
                                            runat="server" 
                                            TipoDeCampo="Telefone"
                                            Tamanho = "9"
                                            Largura="65" 
                                            Valor="0"/>
                                    </td>
                              </tr>
                              <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">E-mail:</td>
	                                <td style="width:70%;">
                                        <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtemail" 
                                            alt="txtemail" 
                                            runat="server" 
                                            TipoDeCampo="Livre"
                                            Largura="200" 
                                            Tamanho="40" 
                                            OnBlur="validaremail()"/>
                                    </td>
                             </tr>
                             <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Motivo:</td>
	                                <td style="width:70%;"> 
		                                <select id="motivo" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="Critica">Crítica</option> 
										    <option value="Duvida">Dúvida</option>
										    <option value="Pedido">Pedido</option> 
										    <option value="Reclamacao">Reclamação</option> 
										    <option value="Sugestao">Sugestão</option> 
									    </select>
                                    </td>
                                </tr>
                             <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Mensagem:</td>
	                                <td style="width:70%;">
                                        <textarea
                                            id      = "txtmensagem"
                                            name    = "txtmensagem"
                                            alt     = "txtmensagem"
                                            class   = "text" 
                                            runat   = "server"
                                            rows    = "10"
                                            cols    = "10"
                                            style   = "height:80px;width:600px;"
                                             />
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
                                        <asp:Button  CssClass='BtnNormal' ID="btnEnviar" runat="server" OnClick="enviar" Text="Enviar" style="font-size: 10px; font-family: Verdana;" />
                                        <input type="reset" Class='BtnNormal' value="Limpar formulário" style="font-size: 10px; font-family: Verdana; width: 120px;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

