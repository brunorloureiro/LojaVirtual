<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="login.aspx.cs" Inherits="login" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
   <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <span class="TituloPagina">Área Administrativa</span>
                <table cellpadding="1" cellspacing="1" border="0">
                    <br />
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Usuário:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnm_usuario" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="250" 
                                            Tamanho="30" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Senha:</td>
	                                <td style="width:80%;">
		                                <asp:TextBox
			                                CssClass    = "password"
			                                id          = "txtsenha"
			                                style       = "width:80px" 
			                                runat       = "server" 
			                                maxlength   = "8"
			                                TextMode    = "Password"
			                                AutoCompleteType = "Disabled" />
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
                                            alt         = "Entrar"
                                            id          = 'btn_entrar' 
                                            text        = 'Entrar'
                                            onclick     = 'entrar'
                                            runat       = "server" />
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "Cancelar"
                                            id          = 'btn_voltar'
                                            text        = 'Sair'
                                            onclick     = 'sair'
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
