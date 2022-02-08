<%@ Page Language="C#" MasterPageFile="~/cliente/Cliente.master" CodeFile="descpedido.aspx.cs" Inherits="descpedido" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script language="javascript" type="text/javascript">
       
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
     <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server"></asp:Label>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;margin-top:10px" > 
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblPrincipal"
                    runat       = "server"></asp:Label>
            </td>
         </tr>   
         <tr>
            <td align="center">
                    <asp:Label
                        ID          = "lblhiddens"
                        runat       = "server"></asp:Label>
                    <br /><br />
                    <asp:Button 
                            CssClass  = 'BtnNormal' 
                            id        = 'volta'
                            alt       = "voltar" 
                            text      = 'Voltar'
                            OnClick   = 'Redireciona'
                            runat     = "server" />
                  <br /><br />
            </td>
         </tr>
        
    </table>
</asp:Content>

