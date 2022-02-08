<%@ Page Language="C#" MasterPageFile="~/cliente/Cliente.master" CodeFile="cadastro.aspx.cs" Inherits="cadastro" ValidateRequest="false" AutoEventWireup="true" Title="Untitled Page" %>

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
        
        function validarcpf()
        {
            var objs = window.document.getElementsByTagName("input"); 
            for (x=0; x<objs.length; x++)
            {
                var nome = objs[x].name;
                if (nome.indexOf("txtcpf") >= 0)
                {
                   if (objs[x].value != "0")
                   {
                        var cpf = objs[x].value;
                        exp = /\.|\-/g
                        cpf = cpf.toString().replace( exp, "" );
                        var digitoDigitado = eval(cpf.charAt(9)+cpf.charAt(10));
                        var soma1=0, soma2=0;
                        var vlr =11;
                     
                        for(i=0;i<9;i++)
                        {
                            soma1+=eval(cpf.charAt(i)*(vlr-1));
                            soma2+=eval(cpf.charAt(i)*vlr);
                            vlr--;
                        }    
                        soma1 = (((soma1*10)%11)==10 ? 0:((soma1*10)%11));
                        soma2=(((soma2+(2*soma1))*10)%11);
                     
                        var digitoGerado=(soma1*10)+soma2;
                        if(digitoGerado!=digitoDigitado)    
                        {
                             alert('CPF Inválido. Verifique.');
                             objs[x].value = "0";
                             objs[x].focus();        
                        }
                    }
                }
            }
        }

        function validardata()
        {
            var obj = window.document.getElementsByTagName("input");
            for (x=0; x<obj.length; x++)
            {
                var nome = obj[x].name;
                if (nome.indexOf("txtdt_nascim") >= 0)
                {
                    exp = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$/
                    if (obj[x].value != "")
                    {
                        if(!exp.test(obj[x].value))
                        {
                            alert('Data Inválida. Verifique.');
                            obj[x].value = ""; 
                            obj[x].focus();           
                            return false;
                        }
                    }
                    return true;
                }
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
                
               }
           }
           var botao = window.document.all.tags("input");
           for (i=0;i<botao.length;i++)
           {
               if  (botao(i).alt == "cep")
               {
                   botao(i).click();
               }
           }
        }
        
        function voltar()
        {
           var botao = window.document.all.tags("input");
           for (i=0;i<botao.length;i++)
           {
               if  (botao(i).alt == "voltar")
               {
                   botao(i).click();
               }
           }
        }
        
        function validaremail()
        {
            var obj = window.document.getElementsByTagName("input");
            for (x=0; x<obj.length; x++)
            {
                var nome = obj[x].name;
                if (nome.indexOf("txtemail") >= 0)
                {
                   exp = /^[\w-]+(\.[\w-]+)*@(([\w-]{2,63}\.)+[A-Za-z]{2,6}|\[\d{1,3}(\.\d{1,3}){3}\])$/
                   if (obj[x].value != "")
                   {
                       if (!exp.test(obj[x].value))
                       {
                           if (!confirm('E-mail Inválido. Confirma digitação?'))
                           {
                               obj[x].value = ""; 
                               obj[x].focus();
                               return false;
                           }
                       }
                       return true;
                   }
                }
            }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 60%;">
        <tr>
            <td align="center">
                <span class="TituloPagina">Dados Pessoais</span>
                <table cellpadding="1" cellspacing="1" border="0" >
                    <tr>
                        <td width="100%">
                            <table cellpadding="1" cellspacing="1" border="0" style="width: 700px;">
                                <tr  style="display: none;"align="left">
	                                <td class="LabelCampo" style="width:10%;">Código:</td>
	                                <td style="width:90%;">
		                                <asp:TextBox
			                                cssclass    = "CODIGO"
			                                id          = "txtcd_cliente"
			                                alt         = "txtcodigo"
 			                                runat       = "server" 
			                                maxlength   = "10"
			                                text        = "0"			                                
			                                AutoCompleteType = "Disabled" />
                                        <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_procurar' 
                                            alt         = "..."
                                            text        = '...'
                                            style       = 'width: 20px;'
                                            OnClick     = 'procurar'
                                            runat       = "server" />
                                         <asp:Button 
                                            CssClass    = 'BtnProcurar' 
                                            id          = 'btn_cep' 
                                            alt         = "cep"
                                            text        = '...'
                                            style       = 'width: 20px;'
                                            OnClick     = 'carregacep'
                                            runat       = "server" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Nome:</td>
	                                <td style="width:90%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnm_cliente" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="300" 
                                            Tamanho="70" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Login:</td>
	                                <td style="width:90%;">
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtlogin" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre" 
                                            Largura="75" 
                                            Tamanho="10" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Senha:</td>
	                                <td style="width:90%;">
		                                <asp:TextBox
			                                CssClass    = "password"
			                                id          = "txtsenha"
			                                style       = "width:70px" 
			                                runat       = "server" 
			                                maxlength   = "8"
			                                TextMode    = "Password"
			                                AutoCompleteType = "Disabled" />
	                                    <span class="LabelCampo">&nbsp Repita a senha:&nbsp;</span>
	                                    <asp:TextBox
			                                CssClass    = "password"
			                                id          = "txtcontrasenha"
			                                style       = "width:70px" 
			                                runat       = "server" 
			                                maxlength   = "8"
			                                TextMode    = "Password"
			                                AutoCompleteType = "Disabled" />
	                                </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">CPF:</td>
	                                <td style="width:90%;"> 
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcpf" 
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro"
		                                    Largura="90" 
                                            Tamanho="11"
                                            Valor="0" 
                                            OnBlur="validarcpf()"/> * 
                                     &nbsp <span class="LabelCampo">RG:</span>&nbsp
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtrg" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="90" 
                                            Tamanho="15" /> * 
                                     </td>
                                </tr>
                                 <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">E-mail:</td>
	                                <td style="width:90%;"> 
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtemail" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="200" 
                                            Tamanho="40" />
                                    </td>
                                 </tr>
                                
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Sexo:</td>
	                                <td style="width:90%;"> 
		                                <select id="sexo" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="F">Feminino</option>
										    <option value="M">Masculino</option> 
									    </select>
                                    <span class="LabelCampo" style="width:10%;">&nbsp Est. Civil:&nbsp</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtest_civil" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="110" 
                                            Tamanho="14" /> * 
                                     <span class="LabelCampo" style="width:10%;">&nbsp CEP:&nbsp</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcep" 
		                                    runat="server" 
		                                    TipoDeCampo="CEP"
		                                    Largura="70" 
                                            Tamanho="10"
                                            Valor="0"
                                            OnBlur="validacep()" />
                                    </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:10%;">Endereço:</td>
	                                <td style="width:90%;">
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtendereco" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="400" 
                                            Tamanho="90" />
                                     &nbsp <span class="LabelCampo">Número:</span>&nbsp
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtnumero" 
		                                    runat="server" 
		                                    TipoDeCampo="Inteiro"
		                                    MascaraDoCampo="000000"
		                                    Largura="48" 
		                                    Valor="0"/>
                                    </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:14%;">Complemento:</td>
	                                <td style="width:86%;">
	                                  <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcomplemento" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="480" 
                                            Tamanho="70" />
                                     </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" style="width:14%;">Bairro:</td>
	                                <td style="width:86%;">
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtbairro" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="200" 
                                            Tamanho="45" />
                                     &nbsp <span class="LabelCampo">Cidade:</span>&nbsp
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtcidade" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="150" 
                                            Tamanho="40" />
                                     &nbsp <span class="LabelCampo">UF:</span>&nbsp
		                                <select id="uf" style="color: black; font-family: Microsoft Sans Serif,MS Sans Serif; font-size: 8 pt;background-color: #fffacd; BORDER-RIGHT: #545558 1px solid; BORDER-LEFT: #545558 1px solid; BORDER-BOTTOM: #545558 1px solid; BORDER-TOP: #545558 1px solid;" runat="server">
										    <option value="AC">AC</option>
										    <option value="AL">AL</option> 
										    <option value="AM">AM</option> 
										    <option value="AP">AP</option> 
										    <option value="BA">BA</option> 
										    <option value="CE">CE</option> 
										    <option value="DF">DF</option> 
										    <option selected value="ES">ES</option> 
										    <option value="GO">GO</option> 
										    <option value="MA">MA</option>
										    <option value="MG">MG</option> 
										    <option value="MS">MS</option> 
										    <option value="MT">MT</option> 
										    <option value="PA">PA</option> 
										    <option value="PB">PB</option> 
										    <option value="PE">PE</option> 
										    <option value="PI">PI</option> 
										    <option value="PR">PR</option> 
										    <option value="RJ">RJ</option> 
										    <option value="RN">RN</option> 
										    <option value="RO">RO</option> 
										    <option value="RR">RR</option> 
										    <option value="RS">RS</option> 
										    <option value="SC">SC</option> 
										    <option value="SE">SE</option> 
										    <option value="SP">SP</option> 
										    <option value="TO">TO</option>
									    </select>
                                            
                                    </td>
                                 </tr>
                                 <tr align="left">
	                                <td class="LabelCampo" colspan="2">Tel. Residencial:
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtddd_resid" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="35" 
                                            Tamanho="4" />
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txttel_resid" 
		                                    runat="server" 
		                                    TipoDeCampo="Telefone"
		                                    Tamanho = "9"
		                                    Largura="65" 
		                                    Valor="0"/>
		                              <span class="LabelCampo" colspan="2">&nbsp&nbsp Tel. Comercial:</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtddd_com" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="35" 
                                            Tamanho="4" />
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txttel_com" 
		                                    runat="server" 
		                                    TipoDeCampo="Telefone"
		                                    Tamanho = "9"
		                                    Largura="65" 
		                                    Valor="0"/>
                                      <span class="LabelCampo" colspan="2">&nbsp&nbsp Celular:</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtddd_cel" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="35" 
                                            Tamanho="4" />
		                                <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txttel_cel" 
		                                    runat="server" 
		                                    TipoDeCampo="Telefone"
		                                    Tamanho = "9"
		                                    Largura="65"
		                                    Valor="0" />
                                    </td>
                                </tr>
                                <tr align="left">
	                                <td class="LabelCampo" colspan="2">Data de Nascimento:
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtdt_nascim" 
		                                    runat="server" 
		                                    TipoDeCampo="Data"
		                                    Largura="80" 
                                            Tamanho="10" 
                                            OnBlur="validardata()"/>
                                      <span class="LabelCampo" colspan="2">&nbsp&nbsp Profissão:</span>
		                              <cc1:CustomTextBox 
		                                    ClasseReferenciaCss="text" 
		                                    ID="txtprofissao" 
		                                    runat="server" 
		                                    TipoDeCampo="Livre"
		                                    Largura="350" 
                                            Tamanho="50" />
		                            </td>
                                 </tr>
                                 <br />
                                 <tr align="left">
	                                <td class="LabelCampo" colspan="2">
		                               <asp:CheckBox
			                                CssClass    = "CamposLogin"
			                                id          = "chknoticia"
			                                style       = "margin-left: -04px;"
			                                runat       = "server" />
			                          <span class="LabelRadio" colspan="2">&nbsp Receber Notícias e Promoções por E-Mail</span>
		                             </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>			
                    <tr>
                        <td align="center" colspan="2">
                            <table class='button_bar' cellpadding='1' cellspacing='1'>
                                <tr>
                                    <td>
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "salvar"
                                            id          = 'btn_salvar'
                                            text        = 'Salvar'
                                            onclick     = 'salvar'
                                            runat       = "server" />
                                       <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "atualizar"
                                            id          = 'btn_atualizar'
                                            text        = 'Atualizar'
                                            onclick     = 'atualizar'
                                            Enabled     = "false"
                                            runat       = "server" />
                                        <asp:Button 
                                            CssClass    = 'BtnNormal' 
                                            alt         = "voltar"
                                            id          = 'btn_voltar'
                                            text        = 'Voltar'
                                            onclick     = 'voltar'
                                            runat       = "server" />
                                     </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="left">
                        <td class="LabelCampo" colspan="2">
                            <span class="LabelRadio" colspan="2">* Campos não obrigatórios</span>
                         </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblMsg"
                    CssClass    = "MsgSistema"
                    runat       = "server">&nbsp;</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label
                    ID          = "lblGrid"
                    runat       = "server">&nbsp;</asp:Label>
            </td>
        </tr>
    </table>
    
</asp:Content>
