<%@ Page Language="C#" MasterPageFile="~/loja/Loja.master" CodeFile="carrinho.aspx.cs" Inherits="carrinho" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
       function validacep()
       {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("txtcep") >= 0)
               {
                    exp = /\d{2}\.\d{3}\-\d{3}/
                    if (obj[x].value != "" && obj[x].value != "0")
                    {
                       if(!exp.test(obj[x].value))
                       {
                           alert('Número do CEP Inválido.'); 
                           obj[x].value = "0"; 
                           obj[x].focus();            
                           return false;
                       }
                    }
                   return true;
               }
           }
       }

       function verificapac()
       {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_pac") >= 0)
               {
                   var radpac = obj[x].checked;
               }
           }
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_sedex") >= 0)
               {
                   if(radpac)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_moto") >= 0)
               {
                   if(radpac)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "calcula")
                {
                    botao(i).click();
                }
            }
       }
       
       function verificasedex()
       {
           valorpac = 0;
           valorsedex = 0;
           valor = 0;
           
           var obj = window.document.getElementsByTagName("input");
           
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_sedex") >= 0)
               {
                   var radsedex = obj[x].checked;
               }
           }
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_pac") >= 0)
               {
                   if(radsedex)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_moto") >= 0)
               {
                   if(radsedex)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "calcula")
                {
                    botao(i).click();
                }
            }
       }
       
       function verificamoto()
       {
           var obj = window.document.getElementsByTagName("input");
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_moto") >= 0)
               {
                   var radmoto = obj[x].checked;
               }
           }
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_pac") >= 0)
               {
                   if(radmoto)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
           for (x=0; x<obj.length; x++)
           {
               var nome = obj[x].name;
               if (nome.indexOf("rad_sedex") >= 0)
               {
                   if(radmoto)
                   {
                       obj[x].checked = false;
                   }
               }
            }
            
            var botao = window.document.all.tags("input");
            for (i=0;i<botao.length;i++)
            {
                if  (botao(i).alt == "calcula")
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
                    alt         = "calcula"
                    text        = "..."
                    style       = 'width: 20px;display:none'
                    OnClick     = 'CalculaValor'
                    runat       = "server" />
          <%--    <asp:Button 
                    CssClass    = 'BtnProcurar' 
                    id          = 'Button1' 
                    alt         = "pac"
                    text        = "..."
                    style       = 'width: 20px;'
                    OnClick     = 'procurar'
                    runat       = "server" />--%>
                <asp:Label
                    ID          = "lblPrincipal"
                    runat       = "server"></asp:Label>
                <asp:Label
                    ID          = "lblValor"
                    style       = "font-size: 10pt;color:black;font-weight: bold;"
                    runat       = "server"></asp:Label>
            </td>
         </tr>   
         <tr>
            <td align="left">
                  <span class="LabelCampo" style="width:10%;" id="lblcep" runat="server">&nbsp; CEP:&nbsp;</span>
                  <cc1:CustomTextBox 
                        ClasseReferenciaCss="text" 
                        ID="txtcep" 
                        runat="server" 
                        TipoDeCampo="CEP"
                        Largura="70" 
                        Tamanho="10"
                        OnBlur = "validacep()"
                        Valor="" />&nbsp;&nbsp;
                  <asp:Button 
                        CssClass    = 'BtnNormal' 
                        id          = 'btn_calc' 
                        text        = 'Calcula'
                        onclick     = 'Calcula'
                        runat       = "server" />
            </td>
         </tr>
         <tr>
            <td align="center">   
                 <div id="divfrete" runat="server" visible=false>
                 <br /><br />
                    <div id="divcidade" runat="server" >
                    <asp:Label
                        ID          = "lblBairroFrete"
                        style       = "font-size: 10pt;color:black;font-weight: bold;"
                        runat       = "server"></asp:Label>,&nbsp 
                     <asp:Label
                        ID          = "lblCidadeFrete"
                        style       = "font-size: 10pt;color:black;font-weight: bold;"
                        runat       = "server"></asp:Label> - 
                      <asp:Label
                        ID          = "lblUfFrete"
                        style       = "font-size: 10pt;color:black;font-weight: bold;"
                        runat       = "server"></asp:Label>
                       </div>
                        <input type="hidden" id="hidpac" runat="server" value="0" />
                        <input type="hidden" id="hidsedex" runat="server" value="0" />
                        <input type="hidden" id="hidmoto" runat="server" value="0" />
                        <input type="hidden" id="hidvalor" runat="server" value="0" />
                        <br /><br />
                        <asp:RadioButton ID="rad_pac" runat="server" onclick="verificapac()" Checked/><br />
                        <asp:RadioButton ID="rad_sedex" runat="server" onclick="verificasedex()" /><br />
                        <asp:RadioButton ID="rad_moto" runat="server" onclick="verificamoto()" /><br />
                        <br /><br />
                 </div>
            </td>
        </tr>
        <tr>
            <td align="center">
               <%-- <form target="PagSeguro" action="https://pagseguro.uol.com.br/security/webpagamentos/webpagto.aspx" method="post" />
                    <input type="hidden" name="email_cobranca" value="bruno_violeiro@hotmail.com" />
                    <input type="hidden" name="tipo" value="CP" />
                    <input type="hidden" name="moeda" value="BRL" />
                    <input type="hidden" name="tipo_frete" value="PAC" />
                    <input type="hidden" name="item_frete_1" value="0" />
                    <input type="hidden" name="item_peso_1" value="0" />
                    <input type="hidden" name="cliente_nome" value="Nome do cliente" />
                    <input type="hidden" name="cliente_cep" value="29200720" />
                    <input type="hidden" name="cliente_end" value="Rua José Barcelos de Mattos" />
                    <input type="hidden" name="cliente_num" value="12" />
                    <input type="hidden" name="cliente_compl" value="Sala 109" />
                    <input type="hidden" name="cliente_bairro" value="Bairro do cliente" />
                    <input type="hidden" name="cliente_cidade" value="Cidade do cliente" />
                    <input type="hidden" name="cliente_uf" value="ES" />
                    <input type="hidden" name="cliente_pais" value="BRA" />
                    <input type="hidden" name="cliente_ddd" value="27" />
                    <input type="hidden" name="cliente_tel" value="12345678" />
                    <input type="hidden" name="cliente_email" value="emaildocliente@cliente.com.br" />
                    <asp:Label
                        ID          = "lblhiddens"
                        runat       = "server"></asp:Label>
                        <input type="image" src="https://pagseguro.uol.com.br/Security/Imagens/btnfinalizaBR.jpg" name="submit" id="pagseg" alt="Pague com PagSeguro - é rápido, grátis e seguro!" runat="server"/> --%>
                  <asp:Button 
                            CssClass  = 'BtnNormal' 
                            id        = 'btn_compra' 
                            text      = 'Comprar'
                            onclick   = 'Compra'
                            runat     = "server" />
                &nbsp&nbsp
                    <input type =button 
                            Class    = 'BtnNormal' 
                            id       = 'btn_limpa' 
                            Value    = 'Limpar Carrinho'
                            onclick  = "javascript:window.location.href ='carrinho.aspx?acao=limparcarrinho'"
                            runat    = "server" />
              <%--               </form>--%>
            </td>
         </tr>
        
    </table>
</asp:Content>

