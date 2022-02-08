<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="acessos.aspx.cs" Inherits="acessos" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
<div id="acesso" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;" align="center" >
        <tr>
            <td align="center">
            <span class="TituloPagina">Acessos dos Usuários</span>
                <table cellpadding="1" cellspacing="1" border="0" >
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
                                <tr  style="display: none;"align="left">
	                                <td class="LabelCampo" style="width:40%;">Código:</td>
	                                <td style="width:60%;" colspan="2">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_acesso"
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
                                <br />
                                <tr align="left">
	                                <td class="LabelCampo" style="width:1%;">Usuário:</td>
	                                <td style="width:1%;">
	                                    <asp:DropDownList
	                                        ID          = "ddlusuarios"
	                                        AutoPostBack = "false"
	                                        CssClass    = "Campos"
	                                        runat       = "server"
	                                        OnInit      = "carregaLista"
	                                        style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;"  />
	                                </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkgrava"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Cadastrar e atualizar registros</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkexclui"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Excluir registros</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkconsulta"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Consultar registros</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkentrada"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Entrada de Peças no Estoque</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkbaixa"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Baixa de Peças do Estoque</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkretirada"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Retirada de Peças do Estoque</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkimporta"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Importar Peças para o Estoque</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkestneg"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Permitir Estoque Negativo</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkcontvenda"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Controlar Vendas</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkcontfinanc"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Controlar Financeiro</span>
		                          </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="3">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkcontloja"
			                                tabindex    = "2"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Controlar Loja Virtual</span>
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
                                            onclick     = 'exclui'
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
