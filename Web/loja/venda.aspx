<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" CodeFile="venda.aspx.cs" Inherits="carrinho" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
      function chama(url)
      {
        window.open(""+url+"",null,"toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=100,Height=40,help=no,edge=raised;");
        var botao = window.document.all.tags("input");
        for (i=0;i<botao.length;i++)
        {
            if  (botao(i).alt == "redirec")
            {
                botao(i).click();
            }
        }
      }
     


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
                <asp:Button 
                    CssClass    = 'BtnProcurar' 
                    id          = 'btn_procurar' 
                    alt         = "redirec"
                    text        = "..."
                    style       = 'width: 20px;display:none'
                    OnClick     = 'Redireciona'
                    runat       = "server" />
                <asp:Label
                    ID          = "lblPrincipal"
                    runat       = "server"></asp:Label>
                <asp:Label
                    ID          = "lblcep"
                    style       = "font-size: 09pt;color:black;font-weight: bold;"
                    runat       = "server"></asp:Label>
                 <br />
                 <asp:Label
                    ID          = "lblfrete"
                    style       = "font-size: 09pt;color:black;font-weight: bold;"
                    runat       = "server"></asp:Label>
                <br /><br />
                <asp:Label
                    ID          = "lblValor"
                    style       = "font-size: 10pt;color:black;font-weight: bold;"
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
                            id        = 'pagseg'
                            alt       = "Pague com PagSeguro - é rápido, grátis e seguro!" 
                            text      = 'Finalizar Compra'
                            OnClick   = 'GravaVenda'
                            runat     = "server" />
                  <br /><br />
            </td>
         </tr>
        
    </table>
</asp:Content>

