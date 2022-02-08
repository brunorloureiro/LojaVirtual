<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" AutoEventWireup="true" CodeFile="quemsomos.aspx.cs" Inherits="quemsomos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 676px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
  <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server">Quem Somos</asp:Label>
    <br /><br /><br />
    <div style="text-align: justify; ">
        <table align=center border=0>
            <tr>
                <td class="style1">
                <b>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Somos uma empresa de vendas pela internet fundada com o intuito de fazer as pessoas se vestirem bem por um preço
                 justo, com produtos de alta qualidade. Nós queremos que as pessoas se sintam bem consigo mesmas, queremos que elas
                 se sintam belas e mais jovens, saindo completamente do comum, e modificando seu estilo de se vestir e de se cuidar.</b><br/>
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
</asp:Content>

