<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="notaspromissorias.aspx.cs" Inherits="notaspromissorias" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
        function validardata()
        {
        
            var obj = window.document.getElementsByTagName("input");
            
            for (x=0; x<obj.length; x++)
            {
                var nome = obj[x].name;
                if (nome.indexOf("txtdt_vencto") >= 0)
                {
                    exp = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$/
                    if (obj[x].value != "")
                    {
                        if(!exp.test(obj[x].value))
                        {
                            alert('Data Inválida. Verifique.');
                            obj[x].value = ""; 
                            obj[x].focus();           
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="notaprom" runat="server">
    <span class="TituloPagina">Controle de Notas Promissórias</span>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
     <br />
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 500px;">
                                <tr  align="left">
	                                <td class="LabelCampo" style="width:30%;">N° da Promissória:</td>
	                                <td style="width:70%;">
	                                     <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    id  = "txtcd_notaprom"
			                                alt = "txtcodigo"
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro" 
		                                    MascaraDoCampo="00000000"
                                            Largura="60"
                                            Valor="0" 
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
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Vencimento:</td>
	                                <td style="width:75%;">
		                                  <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtdt_vencto" 
		                                    runat="server" 
		                                    TipoDeCampo="Data"
		                                    Largura="80" 
                                            Tamanho="10" 
                                            OnBlur="validardata()"/>
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:30%;">Situação:</td>
	                                <td style="width:70%;"> 
		                                <select id="situacao" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="A Receber">A Receber</option>
										    <option value="Recebida">Recebida</option> 
										    <option value="Cancelada">Cancelada</option> 
									    </select>
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
