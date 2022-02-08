<%@ Page Language="C#" MasterPageFile="~/cliente/ClienteSeco.master" CodeFile="termo.aspx.cs" Inherits="termo" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function chamatelalocal(url)
	    {
            window.location.href = url;
        }
        
    </script>
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
        runat       = "server">Termo de Responsabilidade</asp:Label>
    <br /><br /><br />
    <div style="text-align: justify; ">
        <table align=center border=0>
            <tr>
                <td class="style1">
                <b> 1.	Missão</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Nossa missão é levar até as pessoas produtos de qualidade por um preço acessível ao seu bolso, para que nosso público alvo se sinta sempre na moda e com sua alto-estima sempre elevada.<br/>
                <br />
                <b> 2.	Visão</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Ser uma empresa de e-commerce que leve qualidade, preços competitivos e ótimos prazos ao nosso público alvo, sempre visando a melhoria contínua dos nossos produtos, nossa logística e sistema interno de venda.<br/>
                <br />
                <b> 3.	Dos preços e prazos</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Nossos preços são baseados no que o seu bolso pode pagar, gerando com isso satisfação completa do cliente com ótimos preços e ótima qualidade. Nossos prazos são de até 18X dependendo do seu cartão de crédito, e os pagamentos são realizados via PAGSEGURO.<br/>
                <br />
                <b> 4.	Das devoluções e trocas</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Todos os nossos produtos antes de virem para o site passam por uma verificação de qualidade muito rigorosa, portanto ao efetuar a compra dos produtos não aceitamos devoluções de mercadorias, aceitamos a troca por um outro produto de seu enteresse, o prazo máximo para a realização desta troca será de 02 dias úteis após o recebimento da mercadoria.<br/>
                <br />
                <b> 5.	Dos tipos de produtos</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Os produtos como camisas masculinas e femininas, camisetas masculinas e femininas, calças jeans masculinas e femininas, shorts masculinos, carteiras masculinas e femininas, bolsas femininas, tennis e bonés e outros são réplicas. Já os produtos da Victoria’s Secret e os perfumes são todos originais vindos direto dos Estados Unidos.<br/>
                <br />
                <b> 6.	Dos envio de produtos</b><br/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Nossos produtos são enviados de 2 maneiras: Frete pelos correios via PAC ou SEDEX, os encargos do frete serão adicionados ao fim da compra do produto, nós nos isentamos de qualquer responsabilidade quanto a violação de embalagem vinda dos correios, sendo assim de responsabilidade do próprio correio responder por tal violação ou deformação do produto. No modelo Entrega via Motoboy, será marcado com o cliente o dia, horário e local da entrega do produto, e um moto boy levará o produto para o cliente em casa com total segurança e conforto, cada localidade tem uma taxa de entrega variada, o horário de entrega será de 08:00 h às 18:00 h, caso não haja ninguém no local na hora e horário marcado, o produto voltará para o estoque, caso o cliente queira que haja nova entrega via moto boy, os custos da nova entrega ficarão sob responsabilidade do cliente. Segue abaixo a lista abaixo com a descrição dos valores por área de entrega:<br/></br>
                <b>Zona A – </b>Abrange a região do Centro de Vitória, Praia do Canto, Santa Lúcia, Enseada do Suá, Jardim Camburi e Imediações.</br>
                <b>Zona B – </b>Abrange a região de São Torquato, Jardim América, TCG e Imediações.</br>
                <b>Zona C – </b>Abrange a região de TVV, MSC, Hiper Export, Seltimar, TCVV e Imediações.</br>
                <b>Zona D – </b>Abrange a região de Campo Grande, Centro de Vila Velha, Aeroporto e Imediações.</br> 
                <b>Zona E - </b>Abrange a região de Serra, Civit 1, Civit 2, Terca, Coimex, Silotec, Carapina, Viana Laranjeiras e Imediações.</br>
                <b>Zona F – </b>Abrange a região de Praia Mole e Imediações.</br></br>
                <b>Valores por Zona:</b></br>
                <b>Zona A.............................................................. R$  6,50</b></br>
                <b>Zona B.............................................................. R$  9,00</b></br>
                <b>Zona C.............................................................. R$ 10,00</b></br>
                <b>Zona D.............................................................. R$ 15,00</b></br>
                <b>Zona E.............................................................. R$ 18,00</b></br>
                <b>Zona F.............................................................. R$ 30,00</b></br>  

                </td>
            </tr>
            <tr align="left">
                <td class="LabelCampo" colspan="2">
                </br>
                   <asp:CheckBox
                        CssClass    = "CamposLogin"
                        id          = "chkaceito"
                        tabindex    = "2"
                        style       = "margin-left: -04px;"
                        runat       = "server" />
                  <span class="LabelRadio" colspan="2">&nbsp Li e Aceito os Termos de Responsabilidade</span>
                 </td>
            </tr>
             <tr>
                <td align="center" colspan="2">
                    <table class='button_bar' cellpadding='1' cellspacing='1'>
                        <tr>
                            <td>
                            </br>
                                <asp:Button 
                                    CssClass    = 'BtnNormal' 
                                    alt         = "Continuar"
                                    id          = 'btn_entrar' 
                                    text        = 'Continuar'
                                    onclick     = 'entrar'
                                    runat       = "server" />
                               </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
</asp:Content>

