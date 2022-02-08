<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="promocoes.aspx.cs" Inherits="promocoes" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function excluiRegistro(eve)
        {
            if  (!confirm("Confirma exclusão do registro selecionado?"))
            {
                eve.returnValue = false;
                return;
            }
        }
        
        function verificaproduto()
        {
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "produto")
                {
                    botao(i).click();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="promocao" runat="server">
    <span class="TituloPagina">Promoções</span>
    <br/><br/>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
                                <tr  style="display: none;"align="left">
	                                <td class="LabelCampo" style="width:40%;">Código:</td>
	                                <td style="width:60%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_promocao"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "10"
			                                text        = "0"			                                
			                                AutoCompleteType = "Disabled" />
                                        <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_procurar' 
                                            alt         = "..."
                                            text        = '...'
                                            style       = 'width: 20px;'
                                            OnClick     = 'procurar'
                                            runat       = "server" />
                                         <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_pedido' 
                                            alt         = "produto"
                                            text        = "..."
                                            style       = 'width: 20px;'
                                            OnClick     = 'validaproduto'
                                            runat       = "server" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Título:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnm_promocao" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="250" 
                                            Tamanho="30" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:1%;">Produto:</td>
	                                <td style="width:1%;">
	                                     <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_produto"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "15"
			                                text        = "0"
			                                OnBlur      ="verificaproduto()"			                                
			                                AutoCompleteType = "Disabled" />
	                                </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkativo"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Ativo</span>
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
                                            id          = 'btn_novo' 
                                            text        = 'Novo'
                                            onclick     = 'novo'
                                            runat       = "server" />
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "salvar"
                                            id          = 'btn_salvar'
                                            text        = 'Salvar'
                                            onclick     = 'salvar'
                                            runat       = "server" />
                                       <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "atualizar"
                                            id          = 'btn_atualizar'
                                            text        = 'Atualizar'
                                            onclick     = 'atualizar'
                                            Enabled     = "false"
                                            runat       = "server" />
                                        <asp:Button 
                                            CssClass    = 'BtnExcluir' 
                                            id          = 'btn_excluir'
                                            text        = 'Excluir'
                                            onclick     = 'excluir'
                                            OnClientClick = "javascript: excluiRegistro(event);"
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
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblGrid"
                    runat       = "server">&nbsp;</asp:Label>
            </td>
        </tr>
    </table>
</div>    
</asp:Content>
