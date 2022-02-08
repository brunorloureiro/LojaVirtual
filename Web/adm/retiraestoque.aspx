<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="retiraestoque.aspx.cs" Inherits="retiradoestoque" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function importaRegistros(eve)
        {
            if  (!confirm("Confirma a importação para o Estoque?"))
            {
                eve.returnValue = false;
                return;
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="importa" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;">
        <tr>
            <td align="center">
                <table cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td width="100%" colspan="2">
                                <span class="TextoGrande" >Importação de Retiradas de Produtos para o Estoque</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <table class='button_bar' cellpadding='1' cellspacing='1'>
                                <tr>
                                    <td>
                                    <br/>
                                        <asp:Button 
                                            CssClass    = 'BtnImporta' 
                                            id          = 'btn_importar'
                                            text        = 'Importar Retiradas para o Estoque'
                                            onclick     = 'importar'
                                            OnClientClick = "javascript: importaRegistros(event);"
                                            runat       = "server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>    
</asp:Content>
