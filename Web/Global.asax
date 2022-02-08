<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
  //      Application.Add("Email_Cobranca", "bruno_violeiro@hotmail.com");
        Application.Add("InfoBanco", "MySQL");
        Application.Add("StrConexao", ConfigurationManager.ConnectionStrings["StrConexao"].ToString());
      //  Application.Add("UrlUpLoad", ConfigurationManager.AppSettings["localupload"].ToString());
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e) 
    {
//        Session.Add("slogan", "... LuzOnline - A moda iluminando você ...");
       // Session.Timeout = 999999;
        Session.Add("useradm", false);
        Session.Add("cd_user", 0);
        Session.Add("bl_loja", false);
        Session.Add("bl_financ", false);
        Session.Add("bl_contven", false);
        Session.Add("bl_estneg", false);
        Session.Add("bl_importa", false);
        Session.Add("bl_entrada", false);
        Session.Add("bl_retirada", false);
        Session.Add("bl_baixa", false);
        Session.Add("bl_consulta", false);
        Session.Add("bl_exclui", false);
        Session.Add("bl_grava", false);
        Session.Add("usernomeadm", "");
        Session.Add("cd_cliente", 0);
        Session.Add("usernomecliente", "");
        Session.Add("clientelogado", false);
        Session.Add("carrinho", false);
        Session.Add("venda", false);
        Session.Add("codigovenda", 0);
        for (int x = 1; x < 50; x++)
        {
            Session.Add("cd_produto(" + x + ")", "");
            Session.Add("nm_produto(" + x + ")", "");
            Session.Add("preco(" + x + ")", "");
            Session.Add("vl_frete(" + x + ")", "");
            Session.Add("peso(" + x + ")", "");
            Session.Add("quantidade(" + x + ")", 0);
        }
        Session.Add("qt_alterada", 0);
        Session.Add("vl_totfrete", "");
        Session.Add("total_prod", 0);
 
       // Session.Add("cd_produto(1)", "");     
      
     //   Site ClsSite = new Site(Application["StrConexao"].ToString());
     //   ClsSite.AcessoSite(Context.Request.UserHostAddress);
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
       
</script>
