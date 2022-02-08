<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="noticias.aspx.cs" Inherits="noticias" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <!-- <link rel="stylesheet" href="../editor/docs/style.css" type="text/css">
	<script type="text/javascript" src="../editor/scripts/wysiwyg.js"></script>
	<script type="text/javascript" src="../editor/scripts/wysiwyg-settings.js"></script> -->
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
                if (nome.indexOf("txtdh_noticia") >= 0)
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
<div id="noticia" runat="server">
    <span class="TituloPagina">Notícias</span>
    <br/><br/>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 60%;">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0" witdth="200%">
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 700px;">
                                <tr  style="display: none;" align="left">
	                                <td class="LabelCampo" style="width:10%;">Código:</td>
	                                <td style="width:90%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_noticia"
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
                               <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Título:</td>
	                                <td style="width:90%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnm_titulo" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="60" />
                                     </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">Data:&nbsp;</td>
	                                <td style="width:90%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtdh_noticia" 
		                                    runat="server" 
		                                    TipoDeCampo="data" 
                                            Largura="80" 
                                            Tamanho="10" 
                                            OnBlur="validardata()"/>
	                                </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">Notícia:&nbsp;</td>
	                                <td style="width:90%;">
                                        <textarea
                                            id      = "txtds_noticia"
                                            name    = "txtds_noticia"
                                            alt     = "txtds_noticia"
                                            class   = "text" 
                                            runat   = "server"
                                            rows    = "10"
                                            cols    = "1"
                                            style   = "height:80px;width:600px;"
                                             />
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
                                 <tr align="left">
                                    <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chkpopup"
			                                tabindex    = "3"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Página Principal</span>
			                      </td>
                                </tr>
                                <tr align="left">
                                    <td class="LabelCampo" style="width:10%;">
                                    Foto:</td>
	                                <td style="width:90%;">
		                                <asp:FileUpload
		                                    cssclass     = "file"
		                                    id          = "fluImagem"
			                                runat       = "server" />
			                            <span style="font-size: 10px; color: Red;">&nbsp;&nbsp;(uso somente após a salva da notícia)</span>
	                                </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Image ID="imgNoticia" visible=false runat="server" style="margin-top: 15px; margin-bottom: 15px;" width="220px" heigth="110px"/></td>
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
                                            text        = 'Excluir Foto'
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
</div>
</asp:Content>
