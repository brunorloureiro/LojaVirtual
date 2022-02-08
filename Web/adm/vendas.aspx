<%@ Page Language="C#" MasterPageFile="~/adm/ADM.master" CodeFile="vendas.aspx.cs" Inherits="vendas" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
<div id="Venda" runat="server">
    <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPagina"
        runat       = "server"></asp:Label>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;margin-top:10px" >
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblPrincipal"
                    runat       = "server">&nbsp;</asp:Label>
            </td>
        </tr>
        
    </table>
</div>
</asp:Content>
