<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" CodeFile="busca.aspx.cs" Inherits="busca" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
    <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server">&nbsp</asp:Label>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;" > 
        <tr>
            <td align="center">
                <br/><br/>
                <asp:Label
                    ID          = "lblPrincipal"
                    runat       = "server">&nbsp;</asp:Label>
                <asp:Label
                    ID          = "lblNenhum"
                    style       = "font-size: 10pt;color:black;font-weight: bold;"
                    runat       = "server">Nenhum produto foi encontrado.</asp:Label>
            </td>
        </tr>
        
    </table>
</asp:Content>

