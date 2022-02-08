﻿using System;
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
using System.IO;

public partial class buscaavancada : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Loja ClsLoja = new Loja(Application["StrConexao"].ToString());
        lblTitulo.Text = "Busca Avançada";

    }
    
    public void Mensagem(string msg)
    {
        string script = "<script type='text/javascript' language='javascript'>alert(" + '"' + msg.Trim().Replace('"', '´').Replace("\r", " ").Replace("\n", " ") + '"' + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", script);
    }

    public void NovoRegistro()
    {
        Response.Redirect(Request.Url.LocalPath.ToString());
    }
  
}
