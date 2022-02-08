<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="sitpedidos.aspx.cs" Inherits="sitpedido" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
        function verificapedido()
        {
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "pedido")
                {
                    botao(i).click();
                }
            }
          
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="SituacaoPedido" runat="server">
    <span class="TituloPagina">Situação dos Pedidos</span>
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
		                                <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_pedido' 
                                            alt         = "pedido"
                                            text        = "..."
                                            style       = 'width: 20px;'
                                            OnClick     = 'trazpedido'
                                            runat       = "server" />
	                                </td>
                                </tr>
                                <br/>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Nº Pedido:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcd_pedido" 
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro"
		                                    Largura="70" 
                                            Tamanho="7"
                                            Valor="0" 
                                            OnBlur="verificapedido()"/>                                        
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Status:</td>
	                                <td style="width:70%;"> 
		                                <select id="status" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="Aguardando Confirmação de Pagamento">Aguardando Confirmação de Pagamento</option> 
										    <option value="Cancelado">Cancelado</option> 
										    <option value="Cobrança não Autorizada">Cobrança não Autorizada</option> 
										    <option value="Entregue com Sucesso">Entregue com Sucesso</option> 
										    <option value="Enviado">Enviado</option> 
										    <option value="Pedido Realizado com Sucesso">Pedido Realizado com Sucesso</option> 
										    <option value="Separado para Postagem">Separado para Postagem</option> 
										</select>
                                    </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:20%;">Rastreio:</td>
	                                <td style="width:80%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtrastreio" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="150" 
                                            Tamanho="15" />
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
                                            alt         = "atualizar"
                                            id          = 'btn_atualizar'
                                            text        = 'Atualizar'
                                            onclick     = 'atualizar'
                                            Enabled     = "false"
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
