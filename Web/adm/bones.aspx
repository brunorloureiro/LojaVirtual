<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="bones.aspx.cs" Inherits="bones" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
    <span class="TituloPagina">Bonés</span>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 1%;">
                                <tr  align="left">
	                                <td class="LabelCampo" style="width:40%;">Código:</td>
	                                <td style="width:60%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_bone"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "15"
			                                text        = "0"			                                
			                                AutoCompleteType = "Disabled" />
                                        <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_procurar' 
                                            alt         = "..."
                                            text        = "..."
                                            style       = 'width: 20px;'
                                            OnClick     = 'procurar'
                                            runat       = "server" />
	                                </td>
                                </tr>
                                <br/>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Nome:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnm_bone" 
		                                    runat="server"
		                                    TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="35" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:1%;">Produto:</td>
	                                <td style="width:1%;">
	                                    <asp:DropDownList
	                                        ID          = "ddlprodutos"
	                                        AutoPostBack = "false"
	                                        CssClass    = "Campos"
	                                        runat       = "server"
	                                        OnInit      = "carregaLista"
	                                        style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;"  />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:1%;">Marca:</td>
	                                <td style="width:1%;">
	                                    <asp:DropDownList
	                                        ID          = "ddlmarcas"
	                                        AutoPostBack = "false"
	                                        CssClass    = "Campos"
	                                        runat       = "server"
	                                        OnInit      = "carregaLista"
	                                        style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;"  />
	                                </td>
                                </tr>
                                 <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Tamanho:</td>
	                                <td style="width:90%;"> 
		                                <select id="tamanho" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="PP">PP</option>
										    <option value="P">P</option> 
										    <option value="M">M</option> 
										    <option value="G">G</option>
										    <option value="GG">GG</option>
										    <option value="XG">XG</option>   
									    </select>
                                    <span class="LabelCampo" style="width:10%;">&nbsp Cor:&nbsp</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcor" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="110" 
                                            Tamanho="30" />
                                &nbsp&nbsp <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkfecho"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">Possui Fecho?</span>
			                          &nbsp&nbsp <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkestampa"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">Possui Estampa?</span>
		                             </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Peso:</td>
	                                <td style="width:90%;"> 
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtpeso" 
		                                    runat="server" 
		                                    TipoDeCampo="Decimal"
		                                    Largura="70" 
                                            Tamanho="10" 
                                            MascaraDoCampo = "000000.000"
                                            Valor="0" />
                                           
                                    <span class="LabelCampo" style="width:10%;">&nbsp Preço:&nbsp</span>
		                               <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtpreco" 
		                                    runat="server" 
		                                    TipoDeCampo="Decimal"
		                                    Largura="70" 
                                            Tamanho="10" 
                                            MascaraDoCampo = "000,000.00"
                                            Valor="0" />
                                     </td>
                                </tr>
                                <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkfrete"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Frete Grátis?</span>
		                          </td>
                                </tr>
                                 <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chknovo"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Produto Novo?</span>
		                          </td>
                                </tr>
                                 <tr align="left">
	                               <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkativo"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Ativo?</span>
		                          </td>
                                </tr>
                                 <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">Descrição:&nbsp;</td>
	                                <td style="width:90%;">
                                        <textarea
                                            id      = "txtdescricao"
                                            name    = "txtdescricao"
                                            alt     = "txtdescricao"
                                            class   = "text" 
                                            runat   = "server"
                                            rows    = "1"
                                            cols    = "1"
                                            style   = "height:40px;width:600px;"
                                             />
	                                </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">Comentário:&nbsp;</td>
	                                <td style="width:90%;">
                                        <textarea
                                            id      = "txtcomentario"
                                            name    = "txtcomentario"
                                            alt     = "txtcomentario"
                                            class   = "text" 
                                            runat   = "server"
                                            rows    = "1"
                                            cols    = "1"
                                            style   = "height:80px;width:600px;"
                                             />
	                                </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">1ª Foto:</td>
	                                <td style="width:90%;">
		                                <asp:FileUpload
		                                    cssclass    = "file"
		                                    id          = "fluProduto1"
			                                runat       = "server"
			                               />
			                            <span style="font-size: 10px; color: Red;">&nbsp;&nbsp;(uso somente após a salva do produto)</span>
	                                </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">2ª Foto:</td>
	                                <td style="width:90%;">
		                                <asp:FileUpload
		                                    cssclass = "file"
		                                    id          = "fluProduto2"
			                                runat       = "server" />
			                         </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">3ª Foto:</td>
	                                <td style="width:90%;">
		                                <asp:FileUpload
		                                    cssclass = "file"
		                                    id          = "fluProduto3"
			                                runat       = "server" />
			                         </td>
                                </tr>
                                <tr>
                                    <td colspan=2><asp:Image ID="imgProduto1" runat="server" visible="false" style="margin-top: 15px; margin-bottom: 15px;" width="100px" heigth="50px" />
                                    <asp:Image ID="imgProduto2" runat="server" visible="false" style="margin-top: 15px; margin-bottom: 15px;" width="100px" heigth="50px" />
                                    <asp:Image ID="imgProduto3" runat="server" visible="false" style="margin-top: 15px; margin-bottom: 15px;" width="100px" heigth="50px"/></td>
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
                                         &nbsp;&nbsp;&nbsp;
                                        <asp:Button 
                                            CssClass    = 'BtnExcluir' 
                                            id          = 'btn_excluirfoto'
                                            text        = 'Excluir Foto(s)'
                                            onclick     = 'excluirfoto'
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
    
</asp:Content>
