<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" CodeFile="buscaavancada.aspx.cs" Inherits="buscaavancada" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
       function buscaavancada()
       {
           tipoproduto = "";
           strbusca = "";
           precoini = "0";
           precofim = "0";
          
           var campos =  window.document.getElementById("aspnetForm");
           for (i=0;i<campos.length;i++)
	       {
	           var nome = campos[i].name;
               if (nome.indexOf("txtnm_produto") >= 0)
               {
                   if (campos(i).value == "")
                   {
                       alert("Nome do Produto deve ser informado. Verifique.");
                       return false;
                   }
                   else
                   {
                       strbusca = campos(i).value;
                   }
               }
           
               if (nome.indexOf("tipo") >= 0)
               {
                   if (campos(i).value != "")
                   {
                        tipoproduto = campos(i).value;
                   }
               }
               
               if (nome.indexOf("txtpreco_ini") >= 0)
               {
                   if (campos(i).value != "0")
                   {
                        precoini = campos(i).value;
                   }
               }
               
               if (nome.indexOf("txtpreco_fim") >= 0)
               {
                   if (campos(i).value != "0")
                   {
                        precofim = campos(i).value;
                   }
                   else
                   {
                       if (precoini != "0" && precoini != "")
                       {
                            alert("Preço Final deve ser informado quando o Preço Inicial estiver preenchido. Verifique.");
                            return false;
                       }
                   }
               }
           }
           window.location.href ='busca.aspx?tipo=A&descricao='+strbusca+'&tipoproduto='+tipoproduto+'&precoini='+precoini+'&precofim='+precofim 
           return true;
        
        }
    

    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:Label
        ID          = "lblTitulo"
        class       = "TituloPaginaLoja"
        runat       = "server"></asp:Label>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 10%;margin-top:10px" > 
        <tr>
            <td align="center">
                <span class="LabelCampo" style="width:10%;" id="Span1" runat="server">&nbsp;Tipo de Produto:&nbsp;</span>
                    <select alt="tipo" name="tipo" id="tipo" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
			        <option value="tod">Todos</option>
			        <option value="ber">Bermudas</option> 
			        <option value="bol">Bolsas</option> 
			        <option value="bon">Bonés</option> 
			        <option value="cal">Calças</option> 
			        <option value="cam">Camisas</option> 
			        <option value="car">Carteiras</option> 
			        <option value="jaq">Jaquetas</option> 
			        <option value="pra">Pratas</option> 
			        <option value="ten">Tênis</option> 
			        <option value="vic">Victória Secrets</option> 
		        </select>
            </td>
        </tr>
        <tr>
            <td align="center">
            <br />
           </td>
        </tr>
        <tr>
            <td align="center">
              <span class="LabelCampo" style="width:10%;" id="lblcep" runat="server">&nbsp; Nome do Produto:&nbsp;</span>
              <cc1:CustomTextBox 
                    ClasseReferenciaCss="text"
                    name = "txtnm_produto" 
                    ID="txtnm_produto" 
                    runat="server" 
                    TipoDeCampo="Livre"
                    Largura="300" 
                    Tamanho="200"
                    Valor="" />&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
            <br />
           </td>
        </tr>    
        <tr>
            <td align="center">
                <span class="LabelCampo" style="width:10%;" runat="server">&nbsp; Preço:&nbsp;</span>
                 <cc1:CustomTextBox 
                    ClasseReferenciaCss="text" 
                    name="txtpreco_ini"
                    ID="txtpreco_ini" 
                    runat="server" 
                    TipoDeCampo="Decimal"
                    Largura="70" 
                    Tamanho="10" 
                    MascaraDoCampo = "000,000.00"
                    Valor="0" />
                    &nbsp Até&nbsp
                 <cc1:CustomTextBox 
                    ClasseReferenciaCss="text" 
                    name="txtpreco_fim"
                    ID="txtpreco_fim" 
                    runat="server" 
                    TipoDeCampo="Decimal"
                    Largura="70" 
                    Tamanho="10" 
                    MascaraDoCampo = "000,000.00"
                    Valor="0" />
             </td>
        </tr>
        <tr>
            <td align="center">
            <br />
           </td>
        </tr>    
        <tr>
            <td align="center">
                <input type =button 
                    Class    = 'BtnProcurar' 
                    id       = 'btn_buscaav' 
                    Value    = 'Traz Resultado(s)'
                    onclick  = "buscaavancada()"
                    runat    = "server" />
            </td>
        </tr>
    </table>
</asp:Content>

