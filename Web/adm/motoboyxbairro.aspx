<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="motoboyxbairro.aspx.cs" Inherits="motoboyxbairro" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="motoxbairro" runat="server">
    <span class="TituloPagina">Motoboy x Bairros</span>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
                                <tr  style="display: none;"align="left">
	                                <td class="LabelCampo" style="width:20%;">Código:</td>
	                                <td style="width:80%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_motoxbai"
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
	                                </td>
                                </tr>
                                <br/>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Bairro:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtbairro" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="40" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Cidade:</td>
	                                <td style="width:70%;"> 
		                                <select id="cidade" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="Cariacica">Cariacica</option> 
										    <option value="Domingos Martins">Domingos Martins</option> 
										    <option value="Fundão">Fundão</option> 
										    <option value="Guarapari">Guarapari</option> 
										    <option value="Marechal Floriano">Marechal Floriano</option> 
										    <option value="Serra">Serra</option> 
										    <option value="Viana">Viana</option> 
										    <option value="Vila Velha">Vila Velha</option> 
										    <option value="Vitoria">Vitória</option>
									    </select>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:30%;">Valor:</td>
                                    <td style="width:70%;"> 
                                        <cc1:CustomTextBox 
                                            ClasseReferenciaCss="text" 
                                            ID="txtvalor" 
                                            runat="server" 
                                            TipoDeCampo="Decimal"
                                            Largura="70" 
                                            Tamanho="10" 
                                            MascaraDoCampo = "000,000.00"
                                            Valor="0" />
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
