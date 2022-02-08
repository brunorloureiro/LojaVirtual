
	function verificaValor(nomeObjeto, tipoObjeto, mascaraObjeto)
    {
        
        var txtcampos = document.getElementsByTagName("input");
        for (i=0;i<txtcampos.length;i++)
        {
            if  (txtcampos[i].id.indexOf(nomeObjeto, 0) >= 0)
            {
                txtcampos[i].onfocus = 
                                function() 
                                {
                                    this.select();
                                };

                if (tipoObjeto.toString().toUpperCase() == "DECIMAL")
                {
                    txtcampos[i].value     = txtcampos[i].value; //parseFloat(mascaraObjeto).toFixed(mascaraObjeto.toString().length - mascaraObjeto.toString().indexOf(".") - 1);
	                txtcampos[i].style.textAlign = "right";

	                txtcampos[i].onkeydown = 
	                                function() 
	                                {
	                                    if (TeclaPadraoUsada()) return true;
	                                    if (!MascaraInteiro()) return false;
	                                    return true;
	                                };

	                txtcampos[i].onkeyup = 
	                                function() 
	                                {
	                                    formataValor(this, mascaraObjeto);
	                                    return true;
	                                };

	                txtcampos[i].onblur = 
	                                function() 
	                                {
                                        if (trim(this) == "") this.value = "0";
	                                    return true;
	                                };
	            }

                if (tipoObjeto.toString().toUpperCase() == "INTEIRO")
                {
                    if  (trim(txtcampos[i]) == "")
                    {
                        txtcampos[i].value     = parseFloat(mascaraObjeto);
                    }
	                txtcampos[i].style.textAlign = "left";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraNumerico(this, mascaraObjeto);
	                                };

//	                txtcampos[i].onkeyup = 
//	                                function() 
//	                                {
//	                                    if (this.value.length == 0)
//	                                    {
//	                                        this.value = "0";
//	                                    }	                                    
//	                                    this.value = parseInt(this.value);	                                    
//	                                    return true;
//	                                };

	            }

                if ((tipoObjeto.toString().toUpperCase() == "CPF")      ||
                    (tipoObjeto.toString().toUpperCase() == "CNPJ"))
                {
                    if  (trim(txtcampos[i]) == "")
                    {
                        txtcampos[i].value     = parseFloat(mascaraObjeto);
                    }
	                txtcampos[i].style.textAlign = "right";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraNumerico(this, mascaraObjeto);
	                                };

	                txtcampos[i].onblur = 
	                                function() 
	                                {
	                                    if (trim(this) == "") this.value = "0";
	                                    return true;
	                                };
	            }

                if (tipoObjeto.toString().toUpperCase() == "TELEFONE")
                {
	                txtcampos[i].style.textAlign = "right";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraTelefone(this);
	                                };

	                txtcampos[i].onblur = 
	                                function() 
	                                {
	                                    if (trim(this) == "") this.value = "";
	                                    return true;
	                                };
	            }

                if (tipoObjeto.toString().toUpperCase() == "CEP")
                {
	                txtcampos[i].style.textAlign = "left";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraCep(this);
	                                };

//	                txtcampos[i].onblur = 
//	                                function() 
//	                                {
//	                                    if (trim(this) == "") this.value = "";
//	                                    return true;
//	                                };
	            }

                if ((tipoObjeto.toString().toUpperCase() == "EMAIL"))
                {
                    //txtcampos[i].value           = "";
	                txtcampos[i].style.textAlign = "left";
	                txtcampos[i].onblur = 
	                                function() 
	                                {   
	                                    if (trim(this) == "") return;
	                                    if (!ValidaEmail(this))
	                                    {
	                                        this.value = "";
	                                    }
	                                    return true;
	                                };
	            }

                if ((tipoObjeto.toString().toUpperCase() == "DATA"))
                {
                    //txtcampos[i].value           = "";
	                txtcampos[i].style.textAlign = "left";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {   
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraData(this);
	                                };

//	                txtcampos[i].onblur = 
//	                                function() 
//	                                {   
//	                                    if (trim(this) == "") return true;
//	                                    if (!ValidaData(this))
//	                                    {
//	                                        this.value = "";
//	                                    }
//	                                    return true;
//	                                };
	            }
	            
                if ((tipoObjeto.toString().toUpperCase() == "DATAHORA"))
                {
                    //txtcampos[i].value           = "";
	                txtcampos[i].style.textAlign = "left";
	                txtcampos[i].onkeydown = 
	                                function() 
	                                {   
	                                    if (TeclaPadraoUsada()) return true;
	                                    return MascaraDataHora(this);
	                                };

//	                txtcampos[i].onblur = 
//	                                function() 
//	                                {   
//	                                    if (trim(this) == "") return true;
//	                                    if (!ValidaDataHora(this))
//	                                    {
//	                                        this.value = "";
//	                                    }
//	                                    return true;
//	                                };
	            }
	            return false;
            }
        }
    }
     
    //retirar espaços e branco.
	function trim(campo)
    {
        var str_valor = new String(campo.value);
        return str_valor.replace(/(^\s*)|(\s*$)/g, "");
    };

    //repassa um carater por outro.
    function replaceAll(de, para)
	{
	    var str = this;
		var pos = str.indexOf(de);
		while (pos > -1)
		{
			str = str.replace(de, para);
			pos_atual = str.indexOf(de);
			if  (pos_atual == pos)
			{
			    return (str);
			}
		}
    };

    //adiciona mascara de cnpj
    function MascaraCNPJ(cnpj)
    {
        if (MascaraInteiro(cnpj) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(cnpj, '00.000.000/0000-00', event);
    }
     
    //adiciona mascara de cep
    function MascaraCep(cep)
    {
        if (MascaraInteiro(cep) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(cep, '00.000-000', event);
    }
     
    //adiciona mascara de cep
    function MascaraNumerico(valor, mascara)
    {
        if (MascaraInteiro(valor) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(valor, mascara, event);
    }
     
    //adiciona mascara de data
    function MascaraData(data)
    {
        if (MascaraInteiro(data) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(data, '00/00/0000', event);
    }
     
    //adiciona mascara de data
    function MascaraDataHora(data)
    {
        if (MascaraInteiro(data) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(data, '00/00/0000 00:00:00', event);
    }
     
    //adiciona mascara de data formato mysql
    function MascaraDataSQL(data)
    {
        if (MascaraInteiro(data) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(data, '0000-00-00', event);
    }
     
    //adiciona mascara IP
    function MascaraIP(ip)
    {
        if (MascaraInteiro(ip) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(ip, '000.000.000.000', event);
    }
     
    //adiciona mascara ao telefone
    function MascaraTelefone(tel)
    {    
        if (MascaraInteiro(tel) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(tel, '0000-0000', event);
    }
     
    //adiciona mascara ao CPF
    function MascaraCPF(cpf)
    {
        if (MascaraInteiro(cpf) == false)
        {
            event.returnValue = false;
        }    
        return formataCampo(cpf, '000.000.000-00', event);
    }
     
    //valida telefone
    function ValidaTelefone(tel)
    {
        exp = /\(\d{2}\)\ \d{4}\-\d{4}/
        if(!exp.test(tel.value))
        {
            alert('Número de Telefone Inválido!');
            return false;
        }
        return true;
    }
     
    //valida CEP
    function ValidaCep(cep)
    {
        exp = /\d{2}\.\d{3}\-\d{3}/
        if(!exp.test(cep.value))
        {
            alert('Número do CEP Inválido.');        
            return false;
        }
        return true;
    }
     
    //valida documentos
    function ValidaDocs(doc)
    {
        exp = /^[a-zA-Z0-9-_\.]+\.(pdf|txt|doc|xls|ppt)$/
        if(!exp.test(doc.value))
        {
            alert('Documento Inválido!');        
            return false;
        }
        return true;
    }
     
    //valida imagens
    function ValidaImg(img)
    {
        exp = /^[a-zA-Z0-9-_\.]+\.(jpg|gif|png)$/
        if(!exp.test(img.value))
        {
            alert('Imagem Inválida!');        
            return false;
        }
        return true;
    }
     
    //valida ip
    function ValidaIp(ip)
    {
        exp = /^((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){3}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})$/
        if(!exp.test(ip.value))
        {
            alert('Endereço IP Inválido!');        
            return false;
        }
        return true;
    }
     
    //valida data mysql
    function ValidaDataSQL(sql)
    {
        exp = /^\d{4}-(0[0-9]|1[0,1,2])-([0,1,2][0-9]|3[0,1])$/
        if(!exp.test(sql.value))
        {
            alert('Endereço IP Inválido!');        
            return false;
        }
        return true;
    }
     
    //valida data
    function ValidaData(data)
    {
        exp = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$/
        if(!exp.test(data.value))
        {
            alert('Data Inválida!');            
            return false;
        }
        return true;
    }     
     
    //valida dígito
    function ValidaDigito(digito)
    {
        exp = /^\d+$/
        if(!exp.test(digito.value))
        {
            alert('Dígito Inválido!');            
            return false;
        }
        return true;
    }
     
    //valida email
    function ValidaEmail(email)
    {
        exp = /^[\w-]+(\.[\w-]+)*@(([\w-]{2,63}\.)+[A-Za-z]{2,6}|\[\d{1,3}(\.\d{1,3}){3}\])$/
        if(!exp.test(email.value))
        {
            if (!confirm('E-mail Inválido. Confirma digitação?'))
            {
                return false;
            }
        }
        return true;
    }
     
    //valida o CPF digitado
    function ValidarCPF(Objcpf)
    {
        var cpf = Objcpf.value;
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
            alert('CPF Inválido!');        
        }
    }
     
    //valida numero inteiro com mascara
    function MascaraInteiro()
    {
        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))
        {
            return true;
        }
        event.returnValue = false;
        return false;
    }
     
    //valida o CNPJ digitado
    function ValidarCNPJ(ObjCnpj)
    {
        var cnpj = ObjCnpj.value;
        var valida = new Array(6,5,4,3,2,9,8,7,6,5,4,3,2);
        var dig1= new Number;
        var dig2= new Number;
     
        exp = /\.|\-|\//g
        cnpj = cnpj.toString().replace( exp, "" );
        var digito = new Number(eval(cnpj.charAt(12)+cnpj.charAt(13)));
     
        for(i = 0; i<valida.length; i++)
        {
            dig1 += (i>0? (cnpj.charAt(i-1)*valida[i]):0);    
            dig2 += cnpj.charAt(i)*valida[i];    
        }
        dig1 = (((dig1%11)<2)? 0:(11-(dig1%11)));
        dig2 = (((dig2%11)<2)? 0:(11-(dig2%11)));
     
        if(((dig1*10)+dig2) != digito)    
        {
            alert('CNPJ Inválido!');
        }
    }
     
    function ValidaDataHora(data)
    {
        exp = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4} \d{2}:\d{2}:\d{2}$/
        if(!exp.test(data.value))
        {
            alert('Data Inválida!');
            return false;
        }
        return true;
    }     
     
    function TeclaPadraoUsada()
    {
        if ((event.keyCode >= 37 && event.keyCode <= 40) || (event.keyCode == 46) || (event.keyCode == 8) || (event.keyCode == 9))
        {
            return true;
        }
        return false;
    }
     
    //formata de forma generica os campos
    function formataCampo(campo, Mascara, evento) 
    {
        var boleanoMascara;
     
        var Digitato = evento.keyCode;
        exp = /\-|\.|\/|\(|\)| /g
        exp = /\:|\-|\.|\/|\(|\)| /g

        campoSoNumeros = campo.value.toString().replace( exp, "" );
     
        var posicaoCampo = 0;    
        var NovoValorCampo="";
        var TamanhoMascara = campoSoNumeros.length;;
     
        if (Digitato != 8) 
        { 
            // backspace
            for(i=0; i<= TamanhoMascara; i++) 
            {
                boleanoMascara  = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".") || (Mascara.charAt(i) == ":") || (Mascara.charAt(i) == "/"))
                boleanoMascara  = boleanoMascara || ((Mascara.charAt(i) == "(") || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
                if (boleanoMascara) 
                {
                    NovoValorCampo += Mascara.charAt(i);
                    TamanhoMascara++;
                }
                else 
                {
                    NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                    posicaoCampo++;
                }           
            }    
            campo.value = NovoValorCampo;
            return true;
        }
        else 
        {
            return true;
        }
    }
    
    function formataMascara(format, field)
    {
	    var result = "";
	    var maskIdx = format.length - 1;
	    var error = false;
	    var valor = field.value;
	    var posFinal = false;
	    if( field.setSelectionRange ) 
	    {
    	    if(field.selectionStart == valor.length)
    		    posFinal = true;
        }
	    valor = valor.replace(/[^0123456789Xx]/g,'')
	    for (var valIdx = valor.length - 1; valIdx >= 0 && maskIdx >= 0; --maskIdx)
	    {
		    var chr = valor.charAt(valIdx);
		    var chrMask = format.charAt(maskIdx);
		    switch (chrMask)
		    {
		    case '#':
			    if(!(/\d/.test(chr)))
				    error = true;
			    result = chr + result;
			    --valIdx;
			    break;
		    case '@':
			    result = chr + result;
			    --valIdx;
			    break;
		    default:
			    result = chrMask + result;
		    }
	    }

	    field.value = result;
	    field.style.color = error ? 'red' : '';
	    if(posFinal)
	    {
		    field.selectionStart = result.length;
		    field.selectionEnd = result.length;
	    }
	    return result;
    }

    // Formata o campo valor
    function formataValor(campo, mascara) {
	    campo.value = filtraCampo(campo);
	    vr = campo.value;
	    tam = vr.length;
	    dec = mascara.toString().length - mascara.toString().indexOf(".") - 1;

	    if ( tam <= dec )
	    { 
 		    campo.value = vr ; 
 		}
 	    if ( tam > dec )
 	    {
 		    campo.value = vr.substr( 0, tam - dec ) + '.' + vr.substr( tam - dec, tam ) ; 
 		}
 		return;
 		
 	    if ( (tam >= 6) && (tam <= 8) ){
 		    campo.value = vr.substr( 0, tam - 5 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
 	    if ( (tam >= 9) && (tam <= 11) ){
 		    campo.value = vr.substr( 0, tam - 8 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
 	    if ( (tam >= 12) && (tam <= 14) ){
 		    campo.value = vr.substr( 0, tam - 11 ) + '.' + vr.substr( tam - 11, 3 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
 	    if ( (tam >= 15) && (tam <= 18) ){
 		    campo.value = vr.substr( 0, tam - 14 ) + '.' + vr.substr( tam - 14, 3 ) + '.' + vr.substr( tam - 11, 3 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ;}
    }

    // limpa todos os caracteres especiais do campo solicitado
    function filtraCampo(campo){
	    var s = "";
	    var cp = "";
	    vr = campo.value;
	    tam = vr.length;
	    for (i = 0; i < tam ; i++) 
	    {  
		    if (vr.substring(i,i + 1) != "/" && vr.substring(i,i + 1) != "-" && vr.substring(i,i + 1) != "."  && vr.substring(i,i + 1) != "," )
		    {
		 	    s = s + vr.substring(i,i + 1);
		 	}
	    }
	    campo.value = s;
	    return cp = campo.value
    }
	