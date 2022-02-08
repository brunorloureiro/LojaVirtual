<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="enviaaviseme.aspx.cs" Inherits="enviaaviseme" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function importaRegistros(eve)
        {
            if  (!confirm("Este processo pode demorar alguns minutos. Deseja prosseguir com o envio de disponibilidade para os clientes?"))
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
                                <span class="TextoGrande" >Aviso de Disponibilidade de Produtos</span>
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
                                            id          = 'btn_avisar'
                                            text        = 'Avisar'
                                            onclick     = 'enviar'
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
