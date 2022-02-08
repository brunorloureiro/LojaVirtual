<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="trocas.aspx.cs" Inherits="trocas" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
        function verificaprodutodev()
        {
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "proddev")
                {
                    botao(i).click();
                }
            }
        }
        
        function verificaprodutolev()
        {
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "prodlev")
                {
                    botao(i).click();
                }
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div id="troca" runat="server">
    <span class="TituloPagina">Troca de Produtos Pós-Venda</span>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 500px;">
                                <tr  style="display: none;"align="left">
	                                <td class="LabelCampo" style="width:30%;">Código:</td>
	                                <td style="width:70%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_troca"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "10"
			                                text        = "0"			                                
			                                AutoCompleteType = "Disabled" />
			                            <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_proddev' 
                                            alt         = "proddev"
                                            text        = "..."
                                            style       = 'width: 20px;'
                                            OnClick     = 'validaprodutodevolvido'
                                            runat       = "server" />
                                         <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_prodlev' 
                                            alt         = "prodlev"
                                            text        = "..."
                                            style       = 'width: 20px;'
                                            OnClick     = 'validaprodutolevado'
                                            runat       = "server" />
	                                </td>
                                </tr>
                                <br/>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Motivo da Troca:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtmotivo" 
		                                    runat="server"
		                                    TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="35" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Cliente:</td>
	                                <td style="width:70%;">
	                                    <asp:DropDownList
	                                        ID          = "ddlclientes"
	                                        AutoPostBack = "false"
	                                        CssClass    = "Campos"
	                                        runat       = "server"
	                                        OnInit      = "carregaLista"
	                                        style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;"  />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Produto Devolvido:</td>
	                                <td style="width:70%;">
	                                     <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_proddev"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "15"
			                                text        = "0"
			                                OnBlur      ="verificaprodutodev()"			                                
			                                AutoCompleteType = "Disabled" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Quantidade Devolvida:</td>
	                                <td style="width:70%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtqt_dev" 
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro" 
		                                    MascaraDoCampo="000000"
                                            Largura="50" 
                                            Valor="0"/>
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Produto Levado:</td>
	                                <td style="width:70%;">
	                                     <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_prodlev"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "15"
			                                text        = "0"
			                                OnBlur      ="verificaprodutolev()"			                                
			                                AutoCompleteType = "Disabled" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Quantidade Levada:</td>
	                                <td style="width:70%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtqt_lev" 
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro" 
		                                    MascaraDoCampo="000000"
                                            Largura="50" 
                                            Valor="0"/>
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Diferença Paga:</td>
	                                <td style="width:70%;">
		                               <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtdifpaga" 
		                                    runat="server" 
		                                    TipoDeCampo="Decimal"
		                                    Largura="70" 
                                            Tamanho="10" 
                                            MascaraDoCampo = "000,000.00" 
                                            Valor="0"/>
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
