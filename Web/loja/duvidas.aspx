<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" AutoEventWireup="true" CodeFile="duvidas.aspx.cs" Inherits="duvidas" %>

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
        runat       = "server">Dúvidas</asp:Label>
    <br /><br /><br />
    <div style="text-align: justify; ">
        <table align=center border=0>
            <tr>
                <td class="style1">
                <b> 1.	Como comprar pelo site?</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Primeiro se cadastre no nosso site, segundo escolha o produto de sua vontade, logo após escolha o tipo de frete ou se preferir entrega via moto boy, em seguida efetue o pagamento através do PAGSEGURO com toda a comodidade e segurança.<br/>
                <br />
                <b> 2.	Vocês aceitam devolução da mercadoria?</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Não aceitamos devoluções de mercadorias, aceitamos a troca por um outro produto de seu interesse, o prazo máximo para a realização desta troca será de 02 dia úteis após o recebimento da mercadoria.<br/>
                <br />
                <b> 3.	Quanto tempo leva para minha mercadoria se enviada?</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Ao ser liberado o pagamento, o pedido será enviado em até 48h (02 dias úteis).<br/>
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
</asp:Content>

