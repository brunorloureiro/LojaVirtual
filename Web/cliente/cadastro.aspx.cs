using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

public partial class cadastro : System.Web.UI.Page
{
    string email = "";
    string cep = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Cliente ClsCliente = new Cliente(Application["StrConexao"].ToString());
      
        if (!IsPostBack)
        {
            if (Request["acao"] != null)
            {
                email = Request["email"];
                cep = Request["cep"];

                if (email != "" && this.txtemail.Valor == "")
                {
                    this.txtemail.Valor = email;
                }
                if (this.txtcep.Valor == "0" || this.txtcep.Valor == "")
                {
                    if (cep != "0" && cep != "")
                    {
                        this.txtcep.Valor = cep;
                        this.trazcep();
                    }
                }
                this.btn_atualizar.Visible = false;
                this.btn_salvar.Visible = true;
                this.btn_salvar.Enabled = true;
            }
            else
            {
                this.txtcd_cliente.Text = Session["cd_cliente"].ToString();
                procurarcliente();
                this.btn_atualizar.Visible = true;
                this.btn_atualizar.Enabled = true;
                this.btn_salvar.Visible = false;
            }
        }

        
    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void atualizar(object sender, EventArgs e)
    {
        bool resp;
        Cliente ClsCliente = new Cliente(Application["StrConexao"].ToString());

        //if (this.txtcpf.Valor.Trim() == "")
        //{
        //    if (this.txtcpf.Valor.Trim() == "0")
        //    {
        //        Mensagem("CPF deve ser informado. Verifique.");
        //        return;
        //    }
        //}

        if (this.txtcep.Valor.Trim() == "0")
        {
            Mensagem("CEP deve ser informado. Verifique.");
            return;
        }

        ClsCliente.CodigoDoCliente = Convert.ToInt32(this.txtcd_cliente.Text.ToString());
        ClsCliente.NomeDoCliente = this.txtnm_cliente.Valor.ToString().Trim();
        ClsCliente.Login = this.txtlogin.Valor.ToString().Trim();
        ClsCliente.Senha = this.txtsenha.Text.ToString().Trim();
        ClsCliente.ContraSenha = this.txtcontrasenha.Text.ToString().Trim();
        ClsCliente.CPF = Convert.ToInt64(this.txtcpf.Valor.Replace(".", "").Replace("-", ""));
        ClsCliente.RG = this.txtrg.Valor.Trim();
        ClsCliente.Email = this.txtemail.Valor.Trim();
        ClsCliente.Sexo  = this.sexo.Value.Trim();
        ClsCliente.EstadoCivil = this.txtest_civil.Valor.Trim();
        ClsCliente.CEP = Convert.ToInt32(this.txtcep.Valor.Replace(".", "").Replace("-", ""));
        ClsCliente.Endereco = this.txtendereco.Valor.Trim();
        ClsCliente.Numero = Convert.ToInt32(this.txtnumero.Valor);
        ClsCliente.Bairro = this.txtbairro.Valor.Trim();
        ClsCliente.Cidade = this.txtcidade.Valor.Trim();
        ClsCliente.UF = this.uf.Value.Trim();
        ClsCliente.Complemento = this.txtcomplemento.Valor.Trim();
        ClsCliente.DDDResidencial = this.txtddd_resid.Valor.Trim();
        ClsCliente.TelefoneResidencial = Convert.ToInt32(this.txttel_resid.Valor.Replace("-", ""));
        ClsCliente.DDDComercial = this.txtddd_com.Valor.Trim();
        ClsCliente.TelefoneComercial = Convert.ToInt32(this.txttel_com.Valor.Replace("-", ""));
        ClsCliente.DDDCelular = this.txtddd_cel.Valor.Trim();
        ClsCliente.TelefoneCelular = Convert.ToInt32(this.txttel_cel.Valor.Replace("-", ""));
        ClsCliente.Profissao = this.txtprofissao.Valor.Trim();
        ClsCliente.DataDeNascimento = this.txtdt_nascim.Valor.ToString().Trim();
        ClsCliente.ReceberNoticias = Convert.ToInt16(this.chknoticia.Checked);
        
        resp = ClsCliente.Atualizar();
        //**************************

        if (ClsCliente.critica != "")
        {
            Mensagem(ClsCliente.critica.ToString());
        }
    }

    public void salvar(object sender, EventArgs e)
    {
        if (this.txtcd_cliente.Text.Trim() != "")
        {
            if (this.txtcd_cliente.Text.Trim() != "0")
            {
                Mensagem("Código não pode ser informado quando feito tentativa de um novo cadastramento. Verifique.");
                return;
            }
        }

        //if (this.txtcpf.Valor.Trim() == "")
        //{
        //    if (this.txtcpf.Valor.Trim() == "0")
        //    {
        //        Mensagem("CPF do Cliente deve ser informado. Verifique.");
        //        return;
        //    }
        //}

        if (this.txtcep.Valor.Trim() == "0")
        {
            Mensagem("CEP do Cliente deve ser informado. Verifique.");
            return;
        }

        bool resp;
        Cliente ClsCliente = new Cliente(Application["StrConexao"].ToString());

        ClsCliente.NomeDoCliente = this.txtnm_cliente.Valor.ToString().Trim();
        ClsCliente.Login = this.txtlogin.Valor.ToString().Trim();
        ClsCliente.Senha = this.txtsenha.Text.ToString().Trim();
        ClsCliente.ContraSenha = this.txtcontrasenha.Text.ToString().Trim();
        ClsCliente.CPF = Convert.ToInt64(this.txtcpf.Valor.Replace(".", "").Replace("-", ""));
        ClsCliente.RG = this.txtrg.Valor.Trim();
        ClsCliente.Email = this.txtemail.Valor.Trim();
        ClsCliente.Sexo = this.sexo.Value.Trim();
        ClsCliente.EstadoCivil = this.txtest_civil.Valor.Trim();
        ClsCliente.CEP = Convert.ToInt32(this.txtcep.Valor.Replace(".", "").Replace("-", ""));
        ClsCliente.Endereco = this.txtendereco.Valor.Trim();
        ClsCliente.Numero = Convert.ToInt32(this.txtnumero.Valor);
        ClsCliente.Bairro = this.txtbairro.Valor.Trim();
        ClsCliente.Cidade = this.txtcidade.Valor.Trim();
        ClsCliente.UF = this.uf.Value.Trim();
        ClsCliente.Complemento = this.txtcomplemento.Valor.Trim();
        ClsCliente.DDDResidencial = this.txtddd_resid.Valor.Trim();
        ClsCliente.TelefoneResidencial = Convert.ToInt32(this.txttel_resid.Valor.Replace("-", ""));
        ClsCliente.DDDComercial = this.txtddd_com.Valor.Trim();
        ClsCliente.TelefoneComercial = Convert.ToInt32(this.txttel_com.Valor.Replace("-", ""));
        ClsCliente.DDDCelular = this.txtddd_cel.Valor.Trim();
        ClsCliente.TelefoneCelular = Convert.ToInt32(this.txttel_cel.Valor.Replace("-", ""));
        ClsCliente.Profissao = this.txtprofissao.Valor.Trim();
        ClsCliente.DataDeNascimento = this.txtdt_nascim.Valor.ToString().Trim();
        ClsCliente.ReceberNoticias = Convert.ToInt16(this.chknoticia.Checked);
        
        resp = ClsCliente.Grava();
        //*********************
        txtcd_cliente.Text = ClsCliente.CodigoDoCliente.ToString();

        if (ClsCliente.critica != "")
        {
            Mensagem(ClsCliente.critica.ToString());
        }
       
        if (resp)
        {
            this.btn_atualizar.Enabled = true;
            this.btn_salvar.Enabled = false;
            Session["usernomecliente"] = ClsCliente.NomeDoCliente;
            Session["cd_cliente"] = ClsCliente.CodigoDoCliente;
        }
        else
        {
            this.btn_atualizar.Enabled = false;
            this.btn_salvar.Enabled = true;
        }

        

          
    }

    public void voltar(object sender, EventArgs e)
    {
        Response.Redirect("../loja/principal.aspx");
    }

    public void procurarcliente()
    {
        bool resp;
        Cliente ClsCliente = new Cliente(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCliente.CodigoDoCliente = Convert.ToInt32(this.txtcd_cliente.Text.ToString());

        resp = ClsCliente.Consulta();
        //************************
        txtcd_cliente.Text = ClsCliente.CodigoDoCliente.ToString();
        txtnm_cliente.Valor = ClsCliente.NomeDoCliente.Trim();
        txtlogin.Valor = ClsCliente.Login.ToString();
        txtsenha.Text = ClsCliente.Senha.Trim();
        txtcontrasenha.Text = ClsCliente.Senha.Trim();
        txtcpf.Valor = ClsCliente.CPF.ToString();
        txtrg.Valor = ClsCliente.RG.Trim();
        txtemail.Valor = ClsCliente.Email.Trim();
        sexo.Value = ClsCliente.Sexo.Trim();
        txtest_civil.Valor = ClsCliente.EstadoCivil.Trim();
        txtcep.Valor = ClsCliente.CEP.ToString();
        txtendereco.Valor = ClsCliente.Endereco.Trim();
        txtnumero.Valor = ClsCliente.Numero.ToString();
        txtbairro.Valor = ClsCliente.Bairro.Trim();
        txtcidade.Valor = ClsCliente.Cidade.Trim();
        uf.Value = ClsCliente.UF.Trim();
        txtcomplemento.Valor = ClsCliente.Complemento.Trim();
        txtddd_resid.Valor = ClsCliente.DDDResidencial.Trim();
        txttel_resid.Valor = ClsCliente.TelefoneResidencial.ToString();
        txtddd_com.Valor = ClsCliente.DDDComercial.Trim();
        txttel_com.Valor = ClsCliente.TelefoneComercial.ToString();
        txtddd_cel.Valor = ClsCliente.DDDCelular.Trim();
        txttel_cel.Valor = ClsCliente.TelefoneCelular.ToString();
        txtprofissao.Valor = ClsCliente.Profissao.Trim();
        txtdt_nascim.Valor = ClsCliente.DataDeNascimento.Replace("00:00:00", "").Trim();
        chknoticia.Checked = ClsCliente.ReceberNoticias == 1 ? true : false;


        if (ClsCliente.critica != "")
        {
            Mensagem(ClsCliente.critica.ToString());
        }
        //this.btn_atualizar.Enabled = true;
        //this.btn_salvar.Enabled = false;
    }
    
    public void procurar(object sender, EventArgs e)
    {
        bool resp;
        Cliente ClsCliente = new Cliente(Application["StrConexao"].ToString());

        this.LimpaCampo();
        ClsCliente.CodigoDoCliente = Convert.ToInt16(this.txtcd_cliente.Text.ToString());

        resp = ClsCliente.Consulta();
        //************************
        txtcd_cliente.Text = ClsCliente.CodigoDoCliente.ToString();
        txtnm_cliente.Valor = ClsCliente.NomeDoCliente.Trim();
        txtlogin.Valor = ClsCliente.Login.ToString();
        txtsenha.Text = ClsCliente.Senha.Trim();
        txtcontrasenha.Text = ClsCliente.Senha.Trim();
        txtcpf.Valor = ClsCliente.CPF.ToString();
        txtrg.Valor = ClsCliente.RG.Trim();
        txtemail.Valor = ClsCliente.Email.Trim();
        sexo.Value = ClsCliente.Sexo.Trim();
        txtest_civil.Valor = ClsCliente.EstadoCivil.Trim();
        txtcep.Valor = ClsCliente.CEP.ToString();
        txtendereco.Valor = ClsCliente.Endereco.Trim();
        txtnumero.Valor = ClsCliente.Numero.ToString();
        txtbairro.Valor = ClsCliente.Bairro.Trim();
        txtcidade.Valor = ClsCliente.Cidade.Trim();
        uf.Value = ClsCliente.UF.Trim();
        txtcomplemento.Valor = ClsCliente.Complemento.Trim();
        txtddd_resid.Valor = ClsCliente.DDDResidencial.Trim();
        txttel_resid.Valor = ClsCliente.TelefoneResidencial.ToString();
        txtddd_com.Valor = ClsCliente.DDDComercial.Trim();
        txttel_com.Valor = ClsCliente.TelefoneComercial.ToString();
        txtddd_cel.Valor = ClsCliente.DDDCelular.Trim();
        txttel_cel.Valor = ClsCliente.TelefoneCelular.ToString();
        txtprofissao.Valor = ClsCliente.Profissao.Trim();
        txtdt_nascim.Valor = ClsCliente.DataDeNascimento.Replace("00:00:00","").Trim();
        chknoticia.Checked = ClsCliente.ReceberNoticias == 1 ? true : false; 


        if (ClsCliente.critica != "")
        {
            Mensagem(ClsCliente.critica.ToString());
        }
        lblGrid.Text = ClsCliente.TrazGrid();

        this.btn_atualizar.Enabled = resp;
        this.btn_salvar.Enabled = !resp;
    }

    public void LimpaCampo()
    {
        this.txtnm_cliente.Valor = "";
        this.txtlogin.Valor = "";
        this.txtsenha.Text = "";
        this.txtcontrasenha.Text = "";
        this.txtlogin.Valor = "";
        this.txtcpf.Valor = "0";
        this.txtrg.Valor = "";
        this.txtemail.Valor = "";
        this.sexo.Value = "F";
        this.txtest_civil.Valor = "";
        this.txtcep.Valor = "0";
        this.txtendereco.Valor = "";
        this.txtnumero.Valor = "0";
        this.txtbairro.Valor ="";
        this.txtcidade.Valor = "";
        this.uf.Value = "ES";
        this.txtcomplemento.Valor = "";
        this.txtddd_resid.Valor = "";
        this.txttel_resid.Valor = "0";
        this.txtddd_com.Valor = "";
        this.txttel_com.Valor = "0";
        this.txtddd_cel.Valor = "";
        this.txttel_cel.Valor = "0";
        this.txtprofissao.Valor = "";
        this.txtdt_nascim.Valor = "";
        this.chknoticia.Checked = false; 
    }

    public void carregacep(object sender, EventArgs e)
    {
     string urlSite = string.Format(
                       @"http://www.buscarcep.com.br/?cep=+" + txtcep.Valor.ToString().Replace(".", "").Replace("-", "") + "&formato=xml");

      XmlTextReader lerXML = new XmlTextReader(urlSite);

      string sNode;
      string sValue;
     
      lerXML.MoveToContent();

      do
      {
          sNode = lerXML.Name;
          if (lerXML.NodeType == XmlNodeType.Element)
          {
              lerXML.Read();
              sValue = lerXML.Value;
             
              switch (sNode)
              {
                   
                  case "logradouro":
                     this.txtendereco.Valor = sValue;
                     break;
                  
                  case "bairro":
                      this.txtbairro.Valor = sValue;
                      break;
                  
                  case "cidade":
                      this.txtcidade.Valor = sValue;
                      break;
                  
                  case "uf":
                      this.uf.Value = sValue;
                      break;
              }

          }
      } while (lerXML.Read()); // ate chegar no final do XML faça!
  }

    public void trazcep()
    {

        string urlSite = string.Format(
                         @"http://www.buscarcep.com.br/?cep=+" + txtcep.Valor.ToString().Replace(".", "").Replace("-", "") + "&formato=xml");

        XmlTextReader lerXML = new XmlTextReader(urlSite);

        string sNode;
        string sValue;
       
        lerXML.MoveToContent();

        do
        {
            sNode = lerXML.Name;
            if (lerXML.NodeType == XmlNodeType.Element)
            {
                lerXML.Read();
                sValue = lerXML.Value;

                switch (sNode)
                {
                    case "logradouro":
                        this.txtendereco.Valor = sValue;
                        break;

                    case "bairro":
                        this.txtbairro.Valor = sValue;
                        break;

                    case "cidade":
                        this.txtcidade.Valor = sValue;
                        break;

                    case "uf":
                        this.uf.Value = sValue;
                        break;

                }
            }
        } while (lerXML.Read()); // ate chegar no final do XML faça!
    }
  
}
