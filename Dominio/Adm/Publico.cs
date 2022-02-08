using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Publico
{
    class Publico
    {
        public string StrConexao = "";
        public OdbcConnection oConn = new OdbcConnection();
        public OdbcCommand oCmd = new OdbcCommand();
        public OdbcDataReader oDr;
        public OdbcConnection oConnAux = new OdbcConnection();
        public OdbcCommand oCmdAux = new OdbcCommand();
        public OdbcDataReader oDrAux;

        public object oDrRet;

        public static bool registros = false;
        public static bool registrosbusca = false;

        public string critica;
        public string criticaaux;

        public bool AbreConexao()
        {
            try
            {
                oConn.ConnectionString = this.StrConexao;
                oConn.Open();
                //***********
                if (oConn.State != ConnectionState.Open)
                {
                    critica = "Conexão não pode ser aberta.";
                    return false;
                    //***********
                }
            }
            catch (Exception Err)
            {
                critica = "Erro: " + Err.Message.ToString();
                return false;
                //***********
            }
            return true;
            //**********
        }

        public bool FechaConexao()
        {
            try
            {
                oConn.Close();
                //***********
                if (oConn.State != ConnectionState.Closed)
                {
                    critica = "Conexão não pode ser fechada. Verifique.";
                    return false;
                    //***********
                }
            }
            catch (Exception Err)
            {
                critica = Err.Message.ToString();
                return false;
                //***********
            }
            return true;
            //**********
        }

        public bool execQuery(string StrSql)
        {
            bool resp = false;

            //*****************
            this.AbreConexao();
            //*****************
            try
            {
                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oCmd.ExecuteNonQuery();
                //*********************
                resp = true;
            }
            catch (Exception)
            {
                resp = false;
            }
            //******************
            this.FechaConexao();
            //******************

            return resp;
        }

        public string Grid(string Tabela, string StrCampos, string DesCampos, string CampoPK, string Cond, bool mostradiv, bool mostrarcheck)
        {
            string StrSql = "";
            int x;
            string[] Campos, Desc;
            string StrTable = "";
            string CssUsado = "shape_grid_dois";

            Campos = StrCampos.Split(',');
            Desc = DesCampos.Split(',');

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  " + StrCampos;
            StrSql = StrSql + " FROM    " + Tabela.ToString().Trim() + "   ";
            StrSql = StrSql + " WHERE 1 = 1 ";
            if (Cond != "")
            {
                StrSql = StrSql + Cond;
            }
            StrSql = StrSql + " ORDER   BY " + Campos[0].ToString().Trim();

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable  = (mostradiv == true ? "<div style='margin-top: 05px; width:500px; height:200px; overflow: scroll;'>" : "");
            StrTable += "<table class='TabelaGrid' border='0' cellspacing='1' cellpadding='0' width='483px' style='height: 20px;" + (mostradiv == true ? "" : "margin-top: 05px;") + "'>";
            StrTable += "<tr>";
            if (mostrarcheck)
            {
                StrTable += "<td class='TituloGrid' height='17'>&nbsp;SEL&nbsp;</td>";
            }
            for (x = 0; x < Campos.Length; x++)
            {
                StrTable += "<td nowrap " + (Desc[x].IndexOf("|N|") >= 0 ? "style='display: none;'" : "") + " class='TituloGrid' " + (Desc[x].IndexOf("|N|") >= 0 ? "style='display: none;'" : "") + "width='" + 100 / Campos.Length + "%'>";
                StrTable += "<div align='center'>" + Desc[x].ToString().Trim() + "</div>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";

            while (oDr.Read())
            {
                if (CssUsado == "linha_grid_dois")
                {
                    CssUsado = "linha_grid_um";
                }
                else
                {
                    CssUsado = "linha_grid_dois";
                }

                StrTable += "<tr class='" + CssUsado.ToString().Trim() + "'>";
                if (mostrarcheck)
                {
                    StrTable += "<td><input type='checkbox' alt='" + CampoPK.ToString() + "' value='" + oDr[0].ToString().Trim() + "' onclick='procurar(this);'></td>";
                }
                for (x = 0; x < Campos.Length; x++)
                {
                    StrTable += "<td style='font-weight: normal; color: darkblue;" + (Desc[x].IndexOf("|N|") >= 0 ? "display: none;'" : "'") + ">";
                    StrTable += oDr[x].ToString().Trim();
                    StrTable += "</td>";
                }
                StrTable += "</tr>";
            }
            StrTable += "</table>";
            StrTable += (mostradiv == true ? "</div>" : "");

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            
            return StrTable.ToString().Trim();
        }

        public string GridFiltro(string Tabela, string StrCampos, string DesCampos, string CampoPK, string Cond, bool bl_foto)
        {
            string StrSql = "";
            int x;
            string[] Campos, Desc;
            string StrTable = "";

            Campos = StrCampos.Split(',');
            Desc = DesCampos.Split(',');

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  " + StrCampos;
            StrSql = StrSql + " FROM    " + Tabela.ToString().Trim();
            StrSql = StrSql + " WHERE   1 = 1 ";
            StrSql = StrSql + " AND     " + Cond;

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<div style='width:500px; height:800px; overflow: scroll;'>";
            StrTable += "<table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='483px'><tr>";
            StrTable += "<td class='titulojanela' height='17'>SEL</td>";
            for (x = 0; x < Campos.Length; x++)
            {
                StrTable += "<td nowrap class='titulojanela' width='" + 100 / Campos.Length + "%'>";
                StrTable += "<div align='center'>" + Desc[x].ToString().Trim() + "</div>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";

            string CssUsado = "shape_grid_dois";

            while (oDr.Read())
            {
                if (CssUsado == "shape_grid_dois")
                {
                    CssUsado = "shape_grid_um";
                }
                else
                {
                    CssUsado = "shape_grid_dois";
                }

                StrTable += "<tr class='" + CssUsado.ToString().Trim() + "'>";
                StrTable += "<td><input type='checkbox' alt='" + CampoPK.ToString() + "' value='" + oDr[0].ToString().Trim() + "' onclick='procurar(this);'></td>";
                for (x = 0; x < Campos.Length; x++)
                {
                    StrTable += "<td style='font-weight: normal; color: darkblue;' valign='top'>";
                    if (bl_foto && x == 0)
                    {
                        StrSql = "          SELECT  cd_foto, extensao " ;
                        StrSql = StrSql + " FROM    AnuncioFoto ";
                        StrSql = StrSql + " WHERE   AnuncioFoto.cd_foto    IN  (SELECT  min(AF.cd_foto) ";
                        StrSql = StrSql + "                                     FROM    AnuncioFoto AF ";
                        StrSql = StrSql + "                                     WHERE   AF.cd_anuncio = " + oDr[x].ToString().Trim() + ")";

                        oCmdAux.Connection = this.oConn;
                        oCmdAux.CommandText = StrSql;
                        oDrAux = oCmdAux.ExecuteReader();
                        //*******************************
                        if (oDrAux.Read())
                        {
                            StrTable += "<img src='../../../../Imagens/Produtos/P/" + oDrAux["cd_foto"].ToString().Trim() + oDrAux["extensao"].ToString().Trim() + "' title='" + oDr[x].ToString().Trim() + "' />";
                        }
                        else
                        {
                            StrTable += "<span  title='" + oDr[x].ToString().Trim() + "'>S/F</span>";
                        }
                        //*************
                        oDrAux.Close();
                        //*************
                    }
                    else
                    {
                        StrTable += oDr[x].ToString().Trim();
                    }
                    StrTable += "</td>";
                }
                StrTable += "</tr>";
            }

            StrTable += "</table></div>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public bool carregaLista(object DDL, string Tabela, string Valor, string Msg)
        {
            string Selecao = "";
            bool Resp = true;
            string StrSql = "";
            DropDownList ListaMenu = (DropDownList)DDL;

            ListaMenu.Items.Clear();

            //*************************************************************************************
            if (!this.AbreConexao()) { this.critica = this.critica; return false; }
            //*************************************************************************************
            try
            {
                ListaMenu.Items.Add(new ListItem(" -- " + Msg.Trim() + " -- ", "0"));

                StrSql += " SELECT  cd_" + Tabela.ToString().Trim() + ", nm_" + Tabela.ToString().Trim();
                StrSql += " FROM    " + Tabela.ToString().Trim() + "   ";
                StrSql += " ORDER   BY nm_" + Tabela.ToString().Trim() + " DESC ";

                oCmd.Connection = this.oConn;
                 //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                while (oDr.Read())
                {
                    ListaMenu.Items.Add(new ListItem((string)oDr[1].ToString().Trim(), oDr[0].ToString().Trim()));
                }
                ListaMenu.Items.FindByValue(Valor).Selected = true;
                oDr.Close();
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
                Resp = false;
            }

            //**************************************************************************************
            if (!this.FechaConexao()) { this.critica = this.critica; return false; }
            //**************************************************************************************

            return Resp;
            //**********
        }

        public string CarregaPaginaPrincipal()
        {
            string StrSql = "";
            int x,y=0;
            string StrTable = "";
   
            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  Produto.cd_peca, Produto.nm_produto, Produto.preco, Produto.qt_estoque ";
            StrSql = StrSql + " FROM    Produto, Principal ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = Principal.cd_produto";
            StrSql = StrSql + " AND     Principal.bl_ativo = 1";
            StrSql = StrSql + " ORDER   BY RAND()";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='600px' >";
            
            while (oDr.Read())
            {
                y = y + 1;
                
                if (y == 1 || y == 4 || y == 7 || y == 10)
                {
                    StrTable += "<tr>";
                }
                
                    StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                    StrTable += "<table border='0' style='valign:top'>";
                    StrTable += "<tr><td width='132px' height='132px' ><a href='produto.aspx?produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='100px' heigth='100px' border=0></a></td></tr>";
                    StrTable += "<tr><td><span>" + oDr[1].ToString().Trim() + "</span></td></tr>";
                    StrTable += "<tr><td><span>R$ " + oDr[2].ToString().Trim() + "</span></td></tr>";
                    if (Convert.ToInt32(oDr["qt_estoque"].ToString()) > 0)
                    {
                        StrTable += "<tr><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                    }
                    else 
                    {
                        string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                        StrTable += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                    }
                    StrTable += "</table>";
                    StrTable += "</td>";

            }
            StrTable += "</tr>";
            StrTable += "</table>";
           
            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }

        public string CarregaPaginaPromocoes()
        {
            string StrSql = "";
            int x, y = 0;
            string StrTable = "";
            
            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  Produto.cd_peca, Promocao.nm_promocao, Produto.preco, Produto.qt_estoque ";
            StrSql = StrSql + " FROM    Produto, Promocao ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = Promocao.cd_produto";
            StrSql = StrSql + " AND     Promocao.bl_ativo = 1";
            StrSql = StrSql + " ORDER   BY RAND() ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='600px' >";

            while (oDr.Read())
            {
                y = y + 1;

                if (y == 1 || y == 4 || y == 7 || y == 10)
                {
                    StrTable += "<tr>";
                }
                StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                StrTable += "<table border='0' style='valign:top'>";
                StrTable += "<tr><td width='132px' height='132px' ><a href='produto.aspx?produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='100px' heigth='100px' border=0></a></td></tr>";
                StrTable += "<tr><td><span>" + oDr[1].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span>R$ " + oDr[2].ToString().Trim() + "</span></td></tr>";
                if (Convert.ToInt32(oDr["qt_estoque"].ToString()) > 0)
                {
                    StrTable += "<tr><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                }
                else
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                    StrTable += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                }
                StrTable += "</table>";
                StrTable += "</td>";

            }
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }

        public string CarregaDadosBuscaSimples(string descricao)
        {
            string StrSql = "";
            int x, y = 0;
            string StrTable = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  Produto.cd_peca, Produto.nm_produto, Produto.preco, Produto.qt_estoque ";
            StrSql = StrSql + " FROM    Produto ";
            StrSql = StrSql + " WHERE   Produto.nm_produto  LIKE '%" + descricao.Trim() +"%'";
            StrSql = StrSql + " ORDER   BY RAND()";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                registrosbusca = true;
            }
            else
            {
                registrosbusca = false;
            }

            //**********
            oDr.Close();
            //**********

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='600px' >";

            while (oDr.Read())
            {
                y = y + 1;

                if (y == 1 || y == 4 || y == 7 || y == 10)
                {
                    StrTable += "<tr>";
                }
                StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                StrTable += "<table border='0' style='valign:top'>";
                StrTable += "<tr><td width='132px' height='132px' ><a href='produto.aspx?produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='100px' heigth='100px' border=0></a></td></tr>";
                StrTable += "<tr><td><span>" + oDr[1].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span>R$ " + oDr[2].ToString().Trim() + "</span></td></tr>";
                if (Convert.ToInt32(oDr["qt_estoque"].ToString()) > 0)
                {
                    StrTable += "<tr><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                }
                else
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                    StrTable += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                }
                StrTable += "</table>";
                StrTable += "</td>";

            }
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }

        public string CarregaDadosBuscaAvancada(string descricao, string tipo, string precoini, string precofim)
        {
            string StrSql = "";
            int x, y = 0;
            string StrTable = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  Produto.cd_peca, Produto.nm_produto, Produto.preco, Produto.qt_estoque ";
            StrSql = StrSql + " FROM    Produto ";
           
            if (tipo == "ber")
            {
                StrSql = StrSql + " , Bermuda ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Bermuda.cd_bermuda ";
             }
            if (tipo == "bol")
            {
                StrSql = StrSql + " , Bolsa ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Bolsa.cd_bolsa ";
             }

            if (tipo == "bon")
            {
                StrSql = StrSql + " , Bone ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Bone.cd_bone ";
            }

            if (tipo == "cal")
            {
                StrSql = StrSql + " , Calca ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Calca.cd_calca ";
            }

            if (tipo == "cam")
            {
                StrSql = StrSql + " , Camisa ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Camisa.cd_camisa ";
            }

            if (tipo == "car")
            {
                StrSql = StrSql + " , Carteira ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Carteira.cd_cart ";
             }

            if (tipo == "jaq")
            {
                StrSql = StrSql + " , Jaqueta ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Jaqueta.cd_jaqueta ";
             }

            if (tipo == "pra")
            {
                StrSql = StrSql + " , Prata ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Prata.cd_prata ";
             }

            if (tipo == "ten")
            {
                StrSql = StrSql + " , Tenis ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = Tenis.cd_tenis ";
            }

            if (tipo == "vic")
            {
                StrSql = StrSql + " , VicSecrets ";
                StrSql = StrSql + " WHERE 1 = 1 ";
                StrSql = StrSql + " AND   Produto.cd_peca = VicSecrets.cd_vicsec ";
            }

            if (tipo == "tod")
            {
                StrSql = StrSql + " WHERE 1 = 1 ";
            }

            StrSql = StrSql + " AND  Produto.nm_produto  LIKE '%" + descricao.Trim() + "%'";
            StrSql = StrSql + " AND  Produto.preco >= " + precoini;
            StrSql = StrSql + " AND  Produto.preco <= " + precofim;
            StrSql = StrSql + " ORDER   BY RAND()";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                registrosbusca = true;
            }
            else
            {
                registrosbusca = false;
            }

            //**********
            oDr.Close();
            //**********

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='600px' >";

            while (oDr.Read())
            {
                y = y + 1;

                if (y == 1 || y == 4 || y == 7 || y == 10)
                {
                    StrTable += "<tr>";
                }
                StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                StrTable += "<table border='0' style='valign:top'>";
                StrTable += "<tr><td width='132px' height='132px' ><a href='produto.aspx?produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='1o0px' heigth='1o0px' border=0></a></td></tr>";
                StrTable += "<tr><td><span>" + oDr[1].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span>R$ " + oDr[2].ToString().Trim() + "</span></td></tr>";
                if (Convert.ToInt32(oDr["qt_estoque"].ToString()) > 0)
                {
                    StrTable += "<tr><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                }
                else
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                    StrTable  += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                }
                StrTable += "</table>";
                StrTable += "</td>";

            }
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }

        public string CarregaInfoProduto(string produto)
        {
            string StrSql = "";
            string Sql = "";
            int x = 0;
            int y = 0;
            string StrTable = "";
            string CodigoProduto = "";
            int CodigoTipoProduto = 0;
            string NomeGenero = "";
            int QuantidadeEstoque = 0;
            int CodigoMarca = 0;
            string NomeMarca = "";
            string Tamanho = "";
            string Cor = "";
            int CodigoFragrancia = 0;
            string NomeFragrancia = "";
            int CodigoTipoCalca = 0;
            string NomeTipoCalca = "";
            int CodigoTipoPrata = 0;
            string NomeTipoPrata = "";
            bool Bolsas = false;
            bool Bones = false;
            bool Calcas = false;
            bool VictoriaSecrets = false;
            bool Camisas = false;
            bool Jaquetas = false;
            string Alca = "";
            string Fecho = "";
            string Estampa = "";
            string Cintura = "";
            string Volume = "";
            string Fechicle = "";
            string Gorro = "";
            string NomeEstampa = "";
            string Gola = "";
            string Manga = "";
            string BabyLook = "";
        
            
            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Bermuda ";
            StrSql = StrSql + " WHERE   Bermuda.cd_bermuda = '" + produto.ToString().Trim()+ "'" ;
            StrSql = StrSql + " ORDER   BY Bermuda.cd_bermuda ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********


            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Bolsa ";
            StrSql = StrSql + " WHERE   Bolsa.cd_bolsa = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Bolsa.cd_bolsa ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Bolsas = true;
                Alca = Convert.ToInt32(oDr["bl_alca"].ToString()) == 1 ? "SIM" : "NÃO";
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Bone ";
            StrSql = StrSql + " WHERE   Bone.cd_bone = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Bone.cd_bone ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Bones = true;
                Fecho = Convert.ToInt32(oDr["bl_fecho"].ToString()) == 1 ? "SIM" : "NÃO";
                Estampa = Convert.ToInt32(oDr["bl_estampa"].ToString()) == 1 ? "SIM" : "NÃO";
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Calca ";
            StrSql = StrSql + " WHERE   Calca.cd_calca = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Calca.cd_calca ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Calcas = true;
                Cintura = oDr["cintura"].ToString() == "A" ? "Alta" : "Baixa";
                CodigoTipoCalca = Convert.ToInt32(oDr["cd_tpcalca"].ToString());
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Camisa ";
            StrSql = StrSql + " WHERE   Camisa.cd_camisa = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Camisa.cd_camisa ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Camisas = true;
                Manga = oDr["manga"].ToString() == "C" ? "Curta" : (oDr["manga"].ToString() == "L" ? "Longa" : "");
                Gola = oDr["gola"].ToString() == "N" ? "Normal" : (oDr["gola"].ToString() == "C" ? "Cavada" : "Polo");
                BabyLook = Convert.ToInt32(oDr["bl_baby"].ToString()) == 1 ? "SIM" : "NÃO";
                Estampa = Convert.ToInt32(oDr["bl_estampa"].ToString()) == 1 ? "SIM" : "NÃO";
                NomeEstampa = oDr["estampa"].ToString() == "A" ? "Atrás" : (oDr["estampa"].ToString() == "F" ? "Frente" : (oDr["estampa"].ToString() == "D" ? "Frente e Atrás" : ""));
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString(); 
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Carteira ";
            StrSql = StrSql + " WHERE   Carteira.cd_cart = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Carteira.cd_cart ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Jaqueta ";
            StrSql = StrSql + " WHERE   Jaqueta.cd_jaqueta = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Jaqueta.cd_jaqueta ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Jaquetas = true;
                Gorro = Convert.ToInt32(oDr["bl_gorro"].ToString()) == 1 ? "SIM" : "NÃO";
                Fechicle = Convert.ToInt32(oDr["bl_fechic"].ToString()) == 1 ? "SIM" : "NÃO";
                Estampa = Convert.ToInt32(oDr["bl_estampa"].ToString()) == 1 ? "SIM" : "NÃO";
                NomeEstampa = oDr["estampa"].ToString() == "A" ? "Atrás" : (oDr["estampa"].ToString() == "F" ? "Frente" : (oDr["estampa"].ToString() == "D" ? "Frente e Atrás" : ""));
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Prata ";
            StrSql = StrSql + " WHERE   Prata.cd_prata = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Prata.cd_prata ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                CodigoTipoPrata = Convert.ToInt32(oDr["cd_tpprata"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Tenis ";
            StrSql = StrSql + " WHERE   Tenis.cd_tenis = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY Tenis.cd_tenis ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                Tamanho = oDr["tamanho"].ToString();
                Cor = oDr["cor"].ToString();
                CodigoMarca = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    VicSecrets ";
            StrSql = StrSql + " WHERE   VicSecrets.cd_vicsec = '" + produto.Trim() + "'";
            StrSql = StrSql + " ORDER   BY VicSecrets.cd_vicsec ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                VictoriaSecrets = true;
                Volume = oDr["volume"].ToString();
                CodigoFragrancia = Convert.ToInt32(oDr["cd_fragrancia"].ToString());
                CodigoMarca  = Convert.ToInt32(oDr["cd_marca"].ToString());
                CodigoTipoProduto = Convert.ToInt32(oDr["cd_tpprod"].ToString());
                CodigoProduto = oDr[0].ToString();
                Sql = StrSql;
            }

            //**********
            oDr.Close();
            //**********


            StrSql = "          SELECT  Produto.qt_estoque ";
            StrSql = StrSql + " FROM    Produto ";
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + CodigoProduto + "'";
            
            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                QuantidadeEstoque = Convert.ToInt32(oDr[0].ToString());
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  Marca.nm_marca ";
            StrSql = StrSql + " FROM    Marca ";
            StrSql = StrSql + " WHERE   Marca.cd_marca = " + CodigoMarca.ToString().Trim(); ;
            
            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                NomeMarca = oDr["nm_marca"].ToString();
            }

            //**********
            oDr.Close();
            //**********

            StrSql = "          SELECT  Genero.nm_genero ";
            StrSql = StrSql + " FROM    Genero, Tpprod ";
            StrSql = StrSql + " WHERE   Genero.cd_genero = Tpprod.cd_genero";
            StrSql = StrSql + " AND     Tpprod.cd_tpprod = " + CodigoTipoProduto.ToString().Trim(); ;

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                NomeGenero = oDr["nm_genero"].ToString();
            }


            //**********
            oDr.Close();
            //**********

            if (CodigoTipoCalca  > 0)
            {
                StrSql = "          SELECT  Tpcalca.nm_tpcalca ";
                StrSql = StrSql + " FROM    Tpcalca ";
                StrSql = StrSql + " WHERE   Tpcalca.cd_tpcalca = " + CodigoTipoCalca.ToString().Trim();

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    NomeTipoCalca = oDr["nm_tpcalca"].ToString();
                }

                //**********
                oDr.Close();
                //**********
            }

            if (CodigoTipoPrata > 0)
            {
                StrSql = "          SELECT  Tpprata.nm_tpprata ";
                StrSql = StrSql + " FROM    Tpprata ";
                StrSql = StrSql + " WHERE   Tpprata.cd_tpprata = " + CodigoTipoPrata.ToString().Trim();

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    NomeTipoPrata = oDr["nm_tpprata"].ToString();
                }

                //**********
                oDr.Close();
                //**********
            }

            if (CodigoFragrancia > 0)
            {
                StrSql = "          SELECT  Fragrancia.nm_fragrancia ";
                StrSql = StrSql + " FROM    Fragrancia ";
                StrSql = StrSql + " WHERE   Fragrancia.cd_fragrancia = " + CodigoFragrancia.ToString().Trim();

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************

                if (oDr.Read())
                {
                    NomeFragrancia = oDr["nm_fragrancia"].ToString();
                }

                //**********
                oDr.Close();
                //**********
            }

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = Sql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='500px' align='left'>";

            while (oDr.Read())
            {
                y = y + 1;

                if (y == 1 || y == 4 || y == 7 || y == 10)
                {
                    StrTable += "<tr>";
                }
                StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                StrTable += "<table border='0' style='valign:top;text-align:justify;margin-left:50px' align='left'>";
                if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\1\\" + oDr[0].ToString().Trim() + ".jpg"))
                {
                    StrTable += "<tr><td width='132px' height='132px' ><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='300px' heigth='300px'></td></tr>";
                }
                StrTable += "<tr><td align='center'><span class='campos'>" + oDr[2].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span class='campos'>Preço:&nbsp</span><span>R$ " + oDr["preco"].ToString().Trim() + "</span></td></tr>";
                if (QuantidadeEstoque > 0)
                {
                    
                    StrTable += "<tr align='center'><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                }
                else
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                    StrTable += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                }
                if (Convert.ToInt16(oDr["bl_frete"].ToString()) == 1)
                {
                    StrTable += "<tr><td align='center'><span class='campos' style:'color=gray'>FRETE GRÁTIS</span></td></tr>";
                }
                StrTable += "<tr><td><span class='campos'>Gênero:&nbsp</span><span>" + NomeGenero + "</span></td></tr>";
                if (NomeMarca != "")
                {
                    StrTable += "<tr><td><span class='campos'>Marca:&nbsp</span><span>" + NomeMarca + "</span></td></tr>";
                }
                if (Tamanho != "")
                {
                    StrTable += "<tr><td><span class='campos'>Tamanho:&nbsp</span><span>" + Tamanho + "</span></td></tr>";
                }
                if (Cor != "")
                {
                    StrTable += "<tr><td><span class='campos'>Cor:&nbsp</span><span>" + Cor + "</span></td></tr>";
                }
                if (NomeTipoPrata != "")
                {
                    StrTable += "<tr><td><span class='campos'>Tipo de Prata:&nbsp</span><span>" + NomeTipoPrata + "</span></td></tr>";
                }

                if (NomeTipoCalca != "")
                {
                    StrTable += "<tr><td><span class='campos'>Tipo de Calça:&nbsp</span><span>" + NomeTipoCalca + "</span></td></tr>";
                }

                if (NomeFragrancia != "")
                {
                    StrTable += "<tr><td><span class='campos'>Fragrância:&nbsp</span><span>" + NomeFragrancia + "</span></td></tr>";
                }
                if (Bolsas)
                {
                    StrTable += "<tr><td><span class='campos'>Possui Alça?&nbsp</span><span>" + Alca + "</span></td></tr>";
                }
                if (Bones)
                {
                    StrTable += "<tr><td><span class='campos'>Possui Fecho?&nbsp</span><span>" + Fecho + "</span></td></tr>";
                    StrTable += "<tr><td><span class='campos'>Possui Estampa?&nbsp</span><span>" + Estampa + "</span></td></tr>";
                }
                if (Calcas)
                {
                    StrTable += "<tr><td><span class='campos'>Cintura:&nbsp</span><span>" + Cintura + "</span></td></tr>";
                }
                if (Camisas)
                {
                    if (BabyLook != "")
                    {
                        StrTable += "<tr><td><span class='campos'>BabyLook?&nbsp</span><span>" + BabyLook + "</span></td></tr>";
                    }
                    if (Gola != "")
                    {
                        StrTable += "<tr><td><span class='campos'>Gola:&nbsp</span><span>" + Gola + "</span></td></tr>";
                    }
                    if (Manga != "")
                    {
                        StrTable += "<tr><td><span class='campos'>Manga:&nbsp</span><span>" + Manga + "</span></td></tr>";
                    }
                    if (NomeEstampa != "")
                    {
                        StrTable += "<tr><td><span class='campos'>Possui Estampa?&nbsp</span><span>" + Estampa + "</span>&nbsp<span class='campos'>&nbsp&nbspAonde?&nbsp</span><span>" + NomeEstampa + "</span></td></tr>";
                    }
                    else
                    {
                        StrTable += "<tr><td><span class='campos'>Possui Estampa?&nbsp</span><span>" + Estampa + "</span></td></tr>";
                    }
                }
                if (Jaquetas)
                {
                    StrTable += "<tr><td><span class='campos'>Possui Gorro?&nbsp</span><span>" + Gorro + "</span></td></tr>";
                    if (NomeEstampa != "")
                    {
                        StrTable += "<tr><td><span class='campos'>Possui Estampa?&nbsp</span><span>" + Estampa + "</span>&nbsp<span class='campos'>&nbsp&nbspAonde?&nbsp</span><span>" + NomeEstampa + "</span></td></tr>";
                    }
                    else
                    {
                        StrTable += "<tr><td><span class='campos'>Possui Estampa?&nbsp</span><span>" + Estampa + "</span></td></tr>";
                    }
                }
                if (VictoriaSecrets)
                {
                    StrTable += "<tr><td><span class='campos'>Volume:&nbsp</span><span>" + Volume + " ml.</span></td></tr>";
                }

                StrTable += "<tr><td><br><br><br><br></td></tr>";
                StrTable += "<tr><td><span class='campos'>Descrição:&nbsp</span><span>" + oDr["descricao"].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span class='campos'>Comentário:&nbsp</span><span>" + oDr["comentario"].ToString().Trim() + "</span></td></tr>";
                StrTable += "</table>";
                StrTable += "<table  valign='top' align='center'>";
                StrTable += "</td><td>";
                
                if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\2\\" + oDr[0].ToString().Trim() + ".jpg"))
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "fotoproduto.aspx?codigo=" + oDr[0].ToString().Trim() + "&page=2" + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=440px,Height=420px,help=no,edge=raised;" + '"' +")'";
                    StrTable += "<br><br><img " + link + " src='../Images/produtos/2/" + oDr[0].ToString().Trim() + ".jpg' width='80px' heigth='80px'><br><br>";
                }
                if (File.Exists("E:\\home\\luzonline\\web\\Images\\Produtos\\3\\" + oDr[0].ToString().Trim() + ".jpg"))
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "fotoproduto.aspx?codigo=" + oDr[0].ToString().Trim() + "&page=3" + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=440px,Height=420px,help=no,edge=raised;" + '"' + ")'";
                    StrTable += "<img " + link + " src='../Images/produtos/3/" + oDr[0].ToString().Trim() + ".jpg' width='80px' heigth='80px'>";
                }


            }
            StrTable += "</td></tr>";
            StrTable += "</table>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }

        public string CarregaInfoCarrinho(string produto, string campo)
        {
            string valor = "";
            string StrSql = "";
            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  * ";
            StrSql = StrSql + " FROM    Produto ";
            StrSql = StrSql + " WHERE   Produto.cd_peca = '" + produto.Trim() + "'" ;
            
            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                if (campo == "produto")
                {
                    valor = oDr["cd_peca"].ToString().Trim();
                }
                else
                {
                    if (campo == "nome")
                    {
                        valor = oDr["nm_produto"].ToString().Trim();
                    }
                    else
                    {
                        if (campo == "preco")
                        {
                            valor = oDr["preco"].ToString().Trim();
                        }
                        else
                        {
                            if (campo == "peso")
                            {
                                valor = oDr["peso"].ToString().Trim();
                            }

                        }

                    }
                }
            }
           
            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return valor;
        }

        public string CarregaPaginaProdutos(string p_produto, int p_genero)
        {
            string StrSql = "";
            int x, y = 0;
            string StrTable = "";

            //*****************
            this.AbreConexao();
            //*****************

            if (p_produto == "BER")
            {
                StrSql = "          SELECT  Bermuda.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Bermuda, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Bermuda.cd_bermuda = Produto.cd_peca";
                StrSql = StrSql + " AND     Bermuda.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "BOL")
            {
                StrSql = "          SELECT  Bolsa.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Bolsa, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Bolsa.cd_bolsa   = Produto.cd_peca";
                StrSql = StrSql + " AND     Bolsa.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "BON")
            {
                StrSql = "          SELECT  Bone.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Bone, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Bone.cd_bone = Produto.cd_peca";
                StrSql = StrSql + " AND     Bone.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "CAL")
            {
                StrSql = "          SELECT  Calca.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Calca, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Calca.cd_calca = Produto.cd_peca";
                StrSql = StrSql + " AND     Calca.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "CAM")
            {
                StrSql = "          SELECT  Camisa.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Camisa, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Camisa.cd_camisa = Produto.cd_peca";
                StrSql = StrSql + " AND     Camisa.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "CAR")
            {
                StrSql = "          SELECT  Carteira.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Carteira, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Carteira.cd_cart = Produto.cd_peca";
                StrSql = StrSql + " AND     Carteira.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "JAQ")
            {
                StrSql = "          SELECT  Jaqueta.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Jaqueta, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Jaqueta.cd_jaqueta = Produto.cd_peca";
                StrSql = StrSql + " AND     Jaqueta.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "PRA")
            {
                StrSql = "          SELECT  Prata.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Prata, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Prata.cd_prata = Produto.cd_peca";
                StrSql = StrSql + " AND     Prata.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "TEN")
            {
                StrSql = "          SELECT  Tenis.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    Tenis, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   Tenis.cd_tenis = Produto.cd_peca";
                StrSql = StrSql + " AND     Tenis.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }
            if (p_produto == "VIC")
            {
                StrSql = "          SELECT  VicSecrets.*, Produto.qt_estoque ";
                StrSql = StrSql + " FROM    VicSecrets, Produto, Tpprod ";
                StrSql = StrSql + " WHERE   VicSecrets.cd_vicsec = Produto.cd_peca";
                StrSql = StrSql + " AND     VicSecrets.cd_tpprod  = Tpprod.cd_tpprod";
                StrSql = StrSql + " AND     Tpprod.cd_genero   = " + p_genero.ToString();
                StrSql = StrSql + " ORDER   BY RAND() ";
            }

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                registros = true;
            }
            else
            {
                registros = false;
            }

            //**********
            oDr.Close();
            //**********

          
            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellspacing='0' cellpadding='0' width='600px' ><br/><br/>";

            while (oDr.Read())
            {
                y = y + 1;

                if (y % 4 == 0)
                {
                    StrTable += "<tr>";
                }
                StrTable += "<td style='font-weight: normal; color: black;' valign='top'>";
                StrTable += "<table border='0' style='valign:top'>";
                StrTable += "<tr><td width='132px' height='132px' ><a href='produto.aspx?produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/produtos/1/" + oDr[0].ToString().Trim() + ".jpg' width='100px' heigth='100px' border=0></a></td></tr>";
                StrTable += "<tr><td><span>" + oDr[2].ToString().Trim() + "</span></td></tr>";
                StrTable += "<tr><td><span>R$ " + oDr["preco"].ToString().Trim() + "</span></td></tr>";
                if (Convert.ToInt32(oDr["qt_estoque"].ToString()) > 0)
                {
                    StrTable += "<tr><td><a href='carrinho.aspx?acao=inserir&produto=" + oDr[0].ToString().Trim() + "'><img src='../Images/adicionar.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></a></td></tr>";
                }
                else
                {
                    string link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.open(" + '"' + "aviseme.aspx?codigo=" + oDr[0].ToString().Trim() + '"' + ",null," + '"' + "toolbars=no,status=no,center=yes,resizable=no,scrollbar=no,width=400px,Height=220px,help=no,edge=raised;" + '"' + ")'";
                    StrTable += "<tr align='center'><td><img " + link + "src='../Images/indisponivel.png' width='40px' heigth='40px' border=0 title='Adicionar ao carrinho.'></td></tr>";
                   
                }
                StrTable += "</table>";
                StrTable += "</td>";

            }
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
        }
        
        public string GridProduto(string Tabela, string StrCampos, string DesCampos, string CampoPK, string Condicao)
        {
            string StrSql = "";
            int x;
            string[] Campos, Desc;
            string StrTable = "";

            Campos = StrCampos.Split(',');
            Desc = DesCampos.Split(',');

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  " + StrCampos;
            StrSql = StrSql + " FROM    " + Tabela.ToString().Trim() + "   ";
            StrSql = StrSql + " WHERE   " + Condicao.ToString().Trim();

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<div style='width:100%; height:131px; overflow:scroll;'>";
            StrTable += "<table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='95%'>";
            StrTable += "<tr>";
            StrTable += "<td class='grid' height='17'>SEL</td>";
            for (x = 0; x < Campos.Length; x++)
            {
                StrTable += "<td nowrap class='grid' width='" + 100 / Campos.Length + "%'>";
                StrTable += "<div align='center'>" + Desc[x].ToString().Trim() + "</div>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";

            string CssUsado = "shape_grid_dois";

            while (oDr.Read())
            {
                if (CssUsado == "shape_grid_dois")
                {
                    CssUsado = "shape_grid_um";
                }
                else
                {
                    CssUsado = "shape_grid_dois";
                }

                StrTable += "<tr class='" + CssUsado.ToString().Trim() + "'>";
                StrTable += "<td><input type='checkbox' alt='" + CampoPK.ToString() + "' value='" + oDr[0].ToString().Trim() + "' onclick='procurar(this);'></td>";
                for (x = 0; x < Campos.Length; x++)
                {
                    StrTable += "<td style='font-weight: normal; color: darkblue;'>";
                    StrTable += oDr[x].ToString().Trim();
                    StrTable += "</td>";
                }
                StrTable += "</tr>";
            }

            StrTable += "</table></div>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public string GridFoto()
        {
            string StrSql = "";
            int x = 0;
            string StrTable = "";
            string PastaImagens = "";

            PastaImagens = "../Images/Bermudas/1/";
            StrSql = "          SELECT  Produto.cd_peca ";
            StrSql = StrSql + " FROM    Produto, Principal ";
            StrSql = StrSql + " WHERE   Produto.cd_produto = Principal.cd_produto";
            StrSql = StrSql + " AND     Principal.bl_ativo = 1";
            StrSql = StrSql + " ORDER   BY Principal.cd_produto ";

            //*****************
            this.AbreConexao();
            //*****************

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='95%'>";
            StrTable += "<tr><td align='center'>";

            while (oDr.Read())
            {
                if (!File.Exists(PastaImagens + oDr[0].ToString().Trim() + ".jpg"))
                {
                    continue;
                }

                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "<br><br>";
                }
                x++;

                StrTable += "<input type='radio' id='foto' name='foto' onclick='fazexclusaofoto(this);' value='" + oDr[0].ToString().Trim() + "'><img style='cursor: hand; cursor: pointer; height: 50px;' onclick='mostrafoto(this);' src='" + PastaImagens.Trim() + oDr[0].ToString().Trim() + oDr[1].ToString().Trim() + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            StrTable += "</td></tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public string GridFotoLoja(string Tabela, string Codigo, string LocalUrl)
        {
            string StrSql = "";
            int x = 0;
            string StrTable = "";
            string PastaImagens = "";

            PastaImagens = "../Imagens/Produtos/";
            StrSql = "          SELECT  cd_foto, extensao ";
            StrSql = StrSql + " FROM    AnuncioFoto ";
            StrSql = StrSql + " WHERE   cd_anuncio = " + Codigo.ToString().Trim();
            StrSql = StrSql + " ORDER   BY cd_foto ";

            //*****************
            this.AbreConexao();
            //*****************

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='95%'>";
            StrTable += "<tr><td align='center'>";

            while (oDr.Read())
            {
                if (!File.Exists(LocalUrl + oDr[0].ToString().Trim() + oDr[1].ToString().Trim()))
                {
                    continue;
                }

                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "<br><br>";
                }
                x++;

                StrTable += "<input type='radio' id='foto' name='foto' onclick='fazexclusaofoto(this);' value='" + oDr[0].ToString().Trim() + "'><img style='cursor: hand; cursor: pointer; height: 50px;' onclick='mostrafoto(this);' src='" + PastaImagens.Trim() + oDr[0].ToString().Trim() + oDr[1].ToString().Trim() + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            StrTable += "</td></tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public string GridFoto_P(string Tabela, string Codigo, string LocalUrl)
        {
            string StrSql = "";
            int x = 0;
            string StrTable = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  cd_foto, extensao ";
            StrSql = StrSql + " FROM    " + Tabela.ToString().Trim() + "   ";
            StrSql = StrSql + " WHERE   cd_acompanhante = " + Codigo.ToString().Trim();

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='95%'><tr>";
            StrTable += "<tr><td align='center'>";

            while (oDr.Read())
            {
                if (!File.Exists(LocalUrl + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim()))
                {
                    continue;
                }

                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "<br><br>";
                }
                x++;
                StrTable += "<input type='radio' id='foto' name='foto' onclick='fazexclusaofoto(this);' value='" + oDr[0].ToString().Trim() + "'><img style='cursor: hand; cursor: pointer; width: 50px;' onclick='mostrafoto(this);' src='../../Imagens/fotos/" + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim() + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            StrTable += "</td></tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public string GridSubMenu(string codigo)
        {
            if (codigo.Trim() == "")
            {
                codigo = "0";
            }

            string StrSql = "";
            string StrTable = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  SubMenu.cd_submenu, SubMenu.nm_submenu, SubMenu.bl_ativo ";
            StrSql = StrSql + " FROM    SubMenu   ";
            StrSql = StrSql + " WHERE   SubMenu.cd_menu = " + codigo.ToString();

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<div style='width:100%; height:131px; overflow:scroll;'><table class='shape_grid' border='0' cellspacing='1' cellpadding='1' width='95%'><tr>";
            StrTable += "<td class='titulojanela' height='17'>SEL</td>";
            StrTable += "<td nowrap class='titulojanela' width='33%'>";
            StrTable += "   <div align='center'>Código</div>";
            StrTable += "</td>";
            StrTable += "<td nowrap class='titulojanela' width='33%'>";
            StrTable += "   <div align='center'>SubMenu</div>";
            StrTable += "</td>";
            StrTable += "<td nowrap class='titulojanela' width='33%'>";
            StrTable += "   <div align='center'>Ativo?</div>";
            StrTable += "</td>";
            StrTable += "</tr>";

            string CssUsado = "shape_grid_dois";

            while (oDr.Read())
            {
                if (CssUsado == "shape_grid_dois")
                {
                    CssUsado = "shape_grid_um";
                }
                else
                {
                    CssUsado = "shape_grid_dois";
                }

                StrTable += "<tr class='" + CssUsado.ToString().Trim() + "'>";
                StrTable += "<td><input type='checkbox' alt='txtcd_submenu' value='" + oDr[0].ToString().Trim() + "' onclick='procurar(this);'></td>";
                StrTable += "<td style='font-weight: normal; color: darkblue;'>";
                StrTable += oDr[0].ToString().Trim();
                StrTable += "</td>";
                StrTable += "<td style='font-weight: normal; color: darkblue;'>";
                StrTable += oDr[1].ToString().Trim();
                StrTable += "</td>";
                StrTable += "<td style='font-weight: normal; color: darkblue;'>";
                StrTable += oDr[2].ToString().Trim();
                StrTable += "</td>";
                StrTable += "</tr>";
            }

            StrTable += "</table></div>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************
            return StrTable.ToString().Trim();
        }

        public string CarregaBusca()
        {
            string StrTable = "";

            StrTable += "<table cellpadding='0' cellspacing='0' border='0' style='width: 198px;'>";
            StrTable += "<tr>";
            StrTable += "   <td><iframe src='BuscaRapida.aspx' frameborder='0' width='198px' height='178px' scrolling='no'></iframe></td>";
            StrTable += "</tr>";
            StrTable += "</table>";

            return StrTable.ToString().Trim();
            //********************************
        }

        public string CarregaMenu(string modulo)
        {
            string StrSql = "";
            string StrTable = "";
            string seta = "";
            string link = "";
            string setalink = "onmouseover='javascript:mostrasubmenu(this,0);' onmouseout='javascript:mostrasubmenu(this,1);'";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT  Menu.cd_menu, Menu.nm_menu, Menu.url, Modulo.nm_modulo ";
            StrSql = StrSql + " FROM    Menu, Modulo   ";
            StrSql = StrSql + " WHERE   Menu.cd_modulo = Modulo.cd_modulo" ;
            StrSql = StrSql + " AND     Menu.cd_modulo = " + modulo.ToString().Trim();
            StrSql = StrSql + " AND     Menu.bl_ativo  = 1 ";
            StrSql = StrSql + " ORDER BY Menu.nm_menu ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            if (oDr.Read())
            {
                StrTable += "<table cellpadding='0' cellspacing='0' border='0' style='width: 140px;background-color: #fffacd;' >";
                StrTable += "<tr >";
                StrTable += "   <div class='menu' align='center'>" + oDr[3].ToString().Trim() + "</div>";
                StrTable += "</tr>";
            }

            oDr.Close();
            //**********

            StrSql = "          SELECT  Menu.cd_menu, Menu.nm_menu, Menu.url, Modulo.nm_modulo ";
            StrSql = StrSql + " FROM    Menu, Modulo   ";
            StrSql = StrSql + " WHERE   Menu.cd_modulo = Modulo.cd_modulo";
            StrSql = StrSql + " AND     Menu.cd_modulo = " + modulo.ToString().Trim();
            StrSql = StrSql + " AND     Menu.bl_ativo  = 1 ";
            StrSql = StrSql + " ORDER BY Menu.nm_menu ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************


            while (oDr.Read())
            {
                StrTable += "<tr>";
                if (oDr[2].ToString().Trim() == "")
                {
                    link = "name='" + oDr[0].ToString().Trim() + "' id='" + oDr[0].ToString().Trim() + "' src='~/images/seta.jpg' align='AbsMiddle' onmouseover='javascript:mudacor(this,0); mostrasubmenu(this,0);' onmouseout='javascript:mudacor(this,1); mostrasubmenu(this,1);'";
                }
                else
                {
                    link = " onmouseover='javascript:mudacor(this,0);' onmouseout='javascript:mudacor(this,1);'";
                    StrTable += "<tr onclick=chamatela('" + oDr[2].ToString().Trim() + "')>";
                }

                StrTable += "    <td colspan='2' nowrap style='background-color: #fffacd; height: 20px; width: 180px;' " + link.Trim() + ">";
                StrTable += "        <span class='LabelMenu' >" + oDr[1].ToString().Trim() + "</span>";
                StrTable += "    </td>";
                StrTable += "</tr>";  
            }
            

            StrTable += "<tr>";
            StrTable += "   <td colspan='2' style='height: 06px; width: 180px; background-color: #fffacd;'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td colspan='3' style='height: 05px; width: 180px; background-color: black;'></td>";
            StrTable += "</tr>";
            StrTable += "</table><br>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string CarregaSubMenu(string modulo)
        {
            string StrSql = "";
            string StrTable = "";
            string link = " onmouseover='javascript:mudacoropcao(this,0);' onmouseout='javascript:mudacoropcao(this,1);'";
            string cd_cod_ant = "";
            
            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT	SubMenu.cd_submenu, SubMenu.nm_submenu, SubMenu.cd_menu, SubMenu.url ";
            StrSql = StrSql + " FROM	Menu, SubMenu   ";
            StrSql = StrSql + " WHERE	Menu.cd_menu = SubMenu.cd_menu ";
            StrSql = StrSql + " AND     Menu.cd_modulo = " + modulo.ToString().Trim();
            StrSql = StrSql + " AND		Menu.bl_ativo		= 1 ";
            StrSql = StrSql + " AND		SubMenu.bl_ativo	= 1 ";
            StrSql = StrSql + " ORDER	BY SubMenu.cd_menu, SubMenu.nm_submenu ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            while (oDr.Read())
            {
                if (oDr[2].ToString().Trim() != cd_cod_ant.ToString().Trim())
                {
                    if (cd_cod_ant.ToString().Trim() != "")
                    {
                        StrTable += "<tr>";
                        StrTable += "   <td style='height:1px;'></td>";
                        StrTable += "</tr>";
                        StrTable += "</table></div>";
                    }
                    cd_cod_ant = oDr[2].ToString().Trim();


                    StrTable += "<div class='submenu' onmouseover='javascript:mostraopcao(this,0);' onmouseout='javascript:mostraopcao(this,1);' style=" + '"' + "filter: alpha(opacity=95); display: none;" + '"' + " id='SubMenu_" + oDr[2].ToString().Trim() + "' name='SubMenu_" + oDr[2].ToString().Trim() + "' style='position: absolute; float:right; left: 161px; top: 0px;'>";
                    StrTable += "   <table cellspacing='0' cellpadding='0' border='0' >";
	            }

                StrTable += "<tr>";
                StrTable += "   <td ></td>";
                StrTable += "</tr>";
                StrTable += "<tr style='height: 20px;' " + link.ToString() + " onclick=chamatela('" + oDr[3].ToString().Trim() + "')>";
                StrTable += "   <td style='width: 130px' nowrap><span >" + oDr[1].ToString().Trim() + "</span></td>";
                StrTable += "</tr>";
            }
            StrTable += "<tr>";
            StrTable += "   <td style='height:1px;'></td>";
            StrTable += "</tr>";
            StrTable += "</table></div>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazProdutoBusca(string UF, string TpAnuncio, string Marca, string Tipo, string Ano)
        {
            string StrSql = "";
            string StrTable = "";
            int x = 0, ver_codtp = 0, ver_marca = 0, ver_tipo = 0, ver_ano = 0;
            string link = "";
            string Localidade = "";
            bool bl_existe = false;

            try
            {
                ver_codtp = Convert.ToInt16(TpAnuncio);
            }
            catch (Exception Err)
            {
                ver_codtp = 0;
            }

            try
            {
                ver_marca = Convert.ToInt16(Marca);
            }
            catch (Exception Err)
            {
                ver_marca = 0;
            }

            try
            {
                ver_tipo = Convert.ToInt16(Tipo);
            }
            catch (Exception Err)
            {
                ver_tipo = 0;
            }

            try
            {
                ver_ano = Convert.ToInt16(Ano);
            }
            catch (Exception Err)
            {
                ver_ano = 0;
            }

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 16px;' align='center'>";
            StrTable += "<tr>";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = " SELECT	Anuncio.cd_anuncio, Anuncio.nm_cor, Marca.nm_marca, Anuncio.vl_anuncio, Tp_Veiculo.nm_tp_veiculo, Modelo.nm_modelo, CONCAT(TRIM(CAST(AnuncioFoto.cd_foto as char)), TRIM(AnuncioFoto.extensao)) as foto, Anuncio.aa_fabric, Anuncio.aa_modelo, ";
            StrSql = StrSql + " 		Cliente.con_cidade, Cliente.con_uf, Cliente.emp_cidade, Cliente.emp_uf ";
            StrSql = StrSql + " FROM 	Anuncio, AnuncioFoto, Tp_Veiculo, Modelo, Cliente, Marca ";
            StrSql = StrSql + " WHERE	Anuncio.cd_cliente		= Cliente.cd_cliente ";
            StrSql = StrSql + " AND		Anuncio.cd_anuncio		= AnuncioFoto.cd_anuncio ";
            StrSql = StrSql + " AND		Anuncio.cd_modelo		= Modelo.cd_modelo ";
            StrSql = StrSql + " AND		Marca.cd_tp_anuncio     = Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Marca.cd_marca			= Tp_Veiculo.cd_marca  ";
            StrSql = StrSql + " AND		Modelo.cd_tp_anuncio	= Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Modelo.cd_marca 	    = Tp_Veiculo.cd_marca ";
            StrSql = StrSql + " AND		Modelo.cd_tp_veiculo	= Tp_Veiculo.cd_tp_veiculo ";
            StrSql = StrSql + " AND   ((Cliente.emp_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.emp_uf          <> '')";
            StrSql = StrSql + " OR     (Cliente.con_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.con_uf          <> '')) ";
            StrSql = StrSql + " AND		AnuncioFoto.cd_foto	IN (SELECT 	MIN(AF.cd_foto) ";
            StrSql = StrSql + " 								FROM	Anuncio, AnuncioFoto AF ";
            StrSql = StrSql + " 							    WHERE	Anuncio.cd_anuncio	 = AF.cd_anuncio ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_ini_vis, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_fim_vis, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " 							    AND		Anuncio.bl_ativo	 =  1 ";
            StrSql = StrSql + " 							    GROUP	BY Anuncio.cd_anuncio) ";
            StrSql = StrSql + " AND     Marca.cd_tp_anuncio      =  " + ver_codtp.ToString().Trim();
            StrSql = StrSql + " AND     Marca.cd_marca           =  " + ver_marca.ToString().Trim();
            StrSql = StrSql + " AND     Tp_Veiculo.cd_tp_veiculo =  " + ver_tipo.ToString().Trim();
            StrSql = StrSql + " AND     Anuncio.aa_modelo        >= " + ver_ano.ToString().Trim();
            StrSql = StrSql + " ORDER   BY RAND() ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            while (oDr.Read())
            {
                bl_existe = true;
                Localidade = oDr["emp_uf"].ToString().Trim() != "" ? oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() : oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim();
                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'><br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center' valign='top'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 120px;'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' onclick='javascript:mostraDetalhes(" + oDr["cd_anuncio"].ToString().Trim() + ");' width='120px' src='Imagens/Produtos/" + oDr["foto"].ToString().Trim() + "' align='absmiddle' title='Clique para visualizar o anúncio' style='cursor: hand; cursor: pointer;'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "   <div class='InfoDescricao'>" + oDr["nm_tp_veiculo"].ToString().Trim() + " " + oDr["aa_fabric"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "/" + oDr["aa_modelo"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "<br>" + oDr["nm_modelo"].ToString().Trim() + "<br>" + Localidade.Trim() + "</div>";
                StrTable += "</td>";
            }

            if (!bl_existe)
            {
                StrTable += "<td style='width:100%;' align='center' colspan='4'>";
                StrTable += "   <div class='info_n_loja' align='center'><br><br><br><br>Nenhum veículo encontrado com as <br>características informadas.</div>";
                StrTable += "</td>";
            }

            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazQtdeProduto(string UF, string Qtde)
        {
            string StrSql = "";
            string StrTable = "";
            int x = 0, cod_qtde = 0;
            string link = "";
            string Localidade = "";
            bool bl_existe = false;

            try
            {
                cod_qtde = Convert.ToInt16(Qtde);
            }
            catch (Exception Err)
            {
                cod_qtde = 12;
            }

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 16px;' align='center'>";
            StrTable += "<tr>";


            //*****************
            this.AbreConexao();
            //*****************

            StrSql = " SELECT	Anuncio.cd_anuncio, Anuncio.nm_cor, Marca.nm_marca, Anuncio.vl_anuncio, Tp_Veiculo.nm_tp_veiculo, Modelo.nm_modelo, CONCAT(TRIM(CAST(AnuncioFoto.cd_foto as char)), TRIM(AnuncioFoto.extensao)) as foto, Anuncio.aa_fabric, Anuncio.aa_modelo, ";
            StrSql = StrSql + " 		Cliente.con_cidade, Cliente.con_uf, Cliente.emp_cidade, Cliente.emp_uf ";
            StrSql = StrSql + " FROM 	Anuncio, AnuncioFoto, Tp_Veiculo, Modelo, Cliente, Marca ";
            StrSql = StrSql + " WHERE	Anuncio.cd_cliente		= Cliente.cd_cliente ";
            StrSql = StrSql + " AND		Anuncio.cd_anuncio		= AnuncioFoto.cd_anuncio ";
            StrSql = StrSql + " AND		Anuncio.cd_modelo		= Modelo.cd_modelo ";

            StrSql = StrSql + " AND		Marca.cd_tp_anuncio     = Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Marca.cd_marca			= Tp_Veiculo.cd_marca  ";
            StrSql = StrSql + " AND		Modelo.cd_tp_anuncio	= Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Modelo.cd_marca 	    = Tp_Veiculo.cd_marca ";
            StrSql = StrSql + " AND		Modelo.cd_tp_veiculo	= Tp_Veiculo.cd_tp_veiculo ";
            
            StrSql = StrSql + " AND   ((Cliente.emp_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.emp_uf          <> '')";
            StrSql = StrSql + " OR     (Cliente.con_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.con_uf          <> '')) ";
            StrSql = StrSql + " AND		AnuncioFoto.cd_foto	IN (SELECT 	MIN(AF.cd_foto) ";
            StrSql = StrSql + " 								FROM	Anuncio, AnuncioFoto AF ";
            StrSql = StrSql + " 							    WHERE	Anuncio.cd_anuncio	 = AF.cd_anuncio ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_ini_vis, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_fim_vis, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " 							    AND		Anuncio.bl_ativo	 =  1 ";
            StrSql = StrSql + " 							    GROUP	BY Anuncio.cd_anuncio) ";
            StrSql = StrSql + " ORDER   BY RAND() LIMIT " + cod_qtde.ToString();

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = "SET DATEFORMAT MDY";
            oCmd.ExecuteNonQuery();
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            while (oDr.Read())
            {
                bl_existe = true;
                Localidade = oDr["emp_uf"].ToString().Trim() != "" ? oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() : oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim();
                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'><br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 120px;'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' onclick='javascript:mostraDetalhes(" + oDr["cd_anuncio"].ToString().Trim() + ");' width='120px' src='Imagens/Produtos/" + oDr["foto"].ToString().Trim() + "' align='absmiddle' title='Clique para visualizar o anúncio' style='cursor: hand; cursor: pointer;'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "   <div class='InfoDescricao'>" + oDr["nm_tp_veiculo"].ToString().Trim() + " " + oDr["aa_fabric"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "/" + oDr["aa_modelo"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "<br>" + oDr["nm_modelo"].ToString().Trim() + "<br>" + Localidade.Trim() + "</div>";
                StrTable += "</td>";
            }

            if (!bl_existe)
            {
                StrTable += "<td style='width:100%;' align='center' colspan='4'>";
                StrTable += "   &nbsp;";
                StrTable += "</td>";
            }

            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazProduto(string UF, string CodLoja)
        {
            string StrSql = "";
            string StrTable = "";
            int x = 0, cod_loja = 0;
            string link = "";
            string Localidade = "";
            bool bl_existe = false;

            try
            {
                cod_loja = Convert.ToInt16(CodLoja);
            }
            catch (Exception Err)
            {
                cod_loja = 0;
            }

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = " SELECT	Anuncio.cd_anuncio, Anuncio.nm_cor, Marca.nm_marca, Anuncio.vl_anuncio, Tp_Veiculo.nm_tp_veiculo, Modelo.nm_modelo, CONCAT(TRIM(CAST(AnuncioFoto.cd_foto as char)), TRIM(AnuncioFoto.extensao)) as foto, Anuncio.aa_fabric, Anuncio.aa_modelo, ";
            StrSql = StrSql + " 		Cliente.con_cidade, Cliente.con_uf, Cliente.emp_cidade, Cliente.emp_uf ";
            StrSql = StrSql + " FROM 	Anuncio, AnuncioFoto, Tp_Veiculo, Modelo, Cliente, Marca ";
            StrSql = StrSql + " WHERE	Anuncio.cd_cliente		= Cliente.cd_cliente ";
            StrSql = StrSql + " AND		Anuncio.cd_anuncio		= AnuncioFoto.cd_anuncio ";
            StrSql = StrSql + " AND		Anuncio.cd_modelo		= Modelo.cd_modelo ";

            StrSql = StrSql + " AND		Marca.cd_tp_anuncio     = Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Marca.cd_marca			= Tp_Veiculo.cd_marca  ";
            StrSql = StrSql + " AND		Modelo.cd_tp_anuncio	= Tp_Veiculo.cd_tp_anuncio ";
            StrSql = StrSql + " AND		Modelo.cd_marca 	    = Tp_Veiculo.cd_marca ";
            StrSql = StrSql + " AND		Modelo.cd_tp_veiculo	= Tp_Veiculo.cd_tp_veiculo ";

            StrSql = StrSql + " AND   ((Cliente.emp_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.emp_uf          <> '')";
            StrSql = StrSql + " OR     (Cliente.con_uf          Like '%" + UF.Trim() + "%' ";
            StrSql = StrSql + " AND     Cliente.con_uf          <> '')) ";
            StrSql = StrSql + " AND		AnuncioFoto.cd_foto	IN (SELECT 	MIN(AF.cd_foto) ";
            StrSql = StrSql + " 								FROM	Anuncio, AnuncioFoto AF ";
            StrSql = StrSql + " 							    WHERE	Anuncio.cd_anuncio	 = AF.cd_anuncio ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_ini_vis, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " 							    AND		STR_TO_DATE((CONCAT(Anuncio.dh_fim_vis, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " 							    AND		Anuncio.bl_ativo	 =  1 ";
            StrSql = StrSql + " 							    GROUP	BY Anuncio.cd_anuncio) ";
            StrSql = StrSql + (cod_loja == -1 ? " " : (" AND   Cliente.cd_cliente = " + cod_loja.ToString().Trim()));
            StrSql = StrSql + (cod_loja == -1 ? " ORDER   BY RAND() LIMIT 20 " : " ORDER   BY RAND() ");

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 16px;' align='center'>";
            StrTable += "<tr>";

            while (oDr.Read())
            {
                bl_existe = true;
                Localidade = oDr["emp_uf"].ToString().Trim() != "" ? oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() : oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim();
                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'>&nbsp;<br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center' valign='top'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 120px;'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' onclick='javascript:mostraDetalhes(" + oDr["cd_anuncio"].ToString().Trim() + ");' width='120px' src='Imagens/Produtos/" + oDr["foto"].ToString().Trim() + "' align='absmiddle' title='Clique para visualizar o anúncio' style='cursor: hand; cursor: pointer;'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "   <div class='InfoDescricao'>" + oDr["nm_tp_veiculo"].ToString().Trim() + " " + oDr["aa_fabric"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "/" + oDr["aa_modelo"].ToString().Trim().PadLeft(4, '0').Substring(2, 2) + "<br>" + oDr["nm_modelo"].ToString().Trim() + "<br>" + Localidade.Trim() + "</div>";
                StrTable += "</td>";
            }

            if (!bl_existe)
            {
                StrTable += "<td style='width:25%;' align='center'>";
                StrTable += "   &nbsp;";
                StrTable += "</td>";
                StrTable += "<td style='width:25%;' align='center'>";
                StrTable += "   &nbsp;";
                StrTable += "</td>";
                StrTable += "<td style='width:25%;' align='center'>";
                StrTable += "   &nbsp;";
                StrTable += "</td>";
                StrTable += "<td style='width:25%;' align='center'>";
                StrTable += "   &nbsp;";
                StrTable += "</td>";
            }

            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazLoja(string UF, string CodLoja, string LocalUrl)
        {
            string StrSql = "";
            string StrTable = "";
            string link = "";
            string Localidade = "";
            int x = 0, cod_loja = 0;

            try
            {
                cod_loja = Convert.ToInt16(CodLoja);
            }
            catch (Exception Err)
            {
                cod_loja = 0;
            }

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 16px;' align='center'>";
            StrTable += "<tr>";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = " SELECT	COUNT(Anuncio.cd_anuncio) as tt_anuncio, HotSite.cd_hotsite, ";
            StrSql = StrSql + " 		Cliente.cd_cliente, Cliente.emp_nome, Cliente.emp_cidade, Cliente.emp_uf, ";
            StrSql = StrSql + " 		Cliente.res_nome, Cliente.con_cidade, Cliente.con_uf ";
            StrSql = StrSql + " FROM	Anuncio, Cliente, HotSite ";
            StrSql = StrSql + " WHERE	Anuncio.cd_cliente	= Cliente.cd_cliente ";
            StrSql = StrSql + " AND		Cliente.cd_cliente	= HotSite.cd_cliente ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(Anuncio.dh_ini_vis, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(Anuncio.dh_fim_vis, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_ini_vig, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_fim_vig, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " AND		Anuncio.bl_ativo	 =  1  ";
            StrSql = StrSql + " AND		Cliente.cd_cliente	<>  " + cod_loja.ToString().Trim();
            StrSql = StrSql + " GROUP	BY HotSite.cd_hotsite, Cliente.cd_cliente, Cliente.emp_nome, Cliente.emp_cidade, Cliente.emp_uf, Cliente.res_nome, Cliente.con_cidade, Cliente.con_uf ";
            StrSql = StrSql + " ORDER   BY RAND() LIMIT 4 ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 0px;' align='center'>";
            StrTable += "<tr>";

            while (oDr.Read())
            {
                if (!File.Exists(LocalUrl + "\\Lojas\\" + oDr["cd_hotsite"].ToString().Trim() + ".jpg"))
                {
                    continue;
                }

                Localidade = oDr["emp_uf"].ToString().Trim() != "" ? oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() : oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim();
                link = "onclick='javascript:window.location.href = " + '"' + "loja.aspx?id=" + oDr["cd_cliente"].ToString().Trim() + '"' + ";'";
                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'><br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 120px;'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' width='120px' src='Imagens/Lojas/" + oDr["cd_hotsite"].ToString().Trim() + ".jpg' " + link + " align='absmiddle' title='Clique para visualizar a loja' style='cursor: hand; cursor: pointer;'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "   <div class='InfoDescricao'>" + (oDr["emp_nome"].ToString().Trim() != "" ? oDr["emp_nome"].ToString().Trim() : oDr["res_nome"].ToString().Trim()) + " [" + oDr["tt_anuncio"].ToString().Trim() + "]<br>" + Localidade.ToString().Trim() + "</div>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";
            StrTable += "</table>";

            //**********
            oDr.Close();
            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazTodasLoja(string UF, string LocalUrl)
        {
            string StrSql = "";
            string StrTable = "";
            string link = "";
            string Localidade = "";
            int x = 0, cod_loja = 0;

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 16px;' align='center'>";
            StrTable += "<tr>";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = " SELECT	COUNT(Anuncio.cd_anuncio) as tt_anuncio, HotSite.cd_hotsite, ";
            StrSql = StrSql + " 		Cliente.cd_cliente, Cliente.emp_nome, Cliente.emp_cidade, Cliente.emp_uf, ";
            StrSql = StrSql + " 		Cliente.res_nome, Cliente.con_cidade, Cliente.con_uf ";
            StrSql = StrSql + " FROM	Anuncio, Cliente, HotSite ";
            StrSql = StrSql + " WHERE	Anuncio.cd_cliente	= Cliente.cd_cliente ";
            StrSql = StrSql + " AND		Cliente.cd_cliente	= HotSite.cd_cliente ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(Anuncio.dh_ini_vis, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(Anuncio.dh_fim_vis, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_ini_vig, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_fim_vig, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";
            StrSql = StrSql + " AND		Anuncio.bl_ativo	 =  1  ";
            StrSql = StrSql + " GROUP	BY HotSite.cd_hotsite, Cliente.cd_cliente, Cliente.emp_nome, Cliente.emp_cidade, Cliente.emp_uf, Cliente.res_nome, Cliente.con_cidade, Cliente.con_uf ";
            StrSql = StrSql + " ORDER   BY RAND() LIMIT 4 ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px; margin-top: 0px;' align='center'>";
            StrTable += "<tr>";

            while (oDr.Read())
            {
                if (!File.Exists(LocalUrl + "\\Lojas\\" + oDr["cd_hotsite"].ToString().Trim() + ".jpg"))
                {
                    continue;
                }

                Localidade = oDr["emp_uf"].ToString().Trim() != "" ? oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() : oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim();
                link = "onclick='javascript:window.location.href = " + '"' + "loja.aspx?id=" + oDr["cd_cliente"].ToString().Trim() + '"' + ";'";
                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'><br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 120px;'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' width='120px' src='Imagens/Lojas/" + oDr["cd_hotsite"].ToString().Trim() + ".jpg' " + link + " align='absmiddle'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "   <div class='InfoDescricao'>" + (oDr["emp_nome"].ToString().Trim() != "" ? oDr["emp_nome"].ToString().Trim() : oDr["res_nome"].ToString().Trim()) + " [" + oDr["tt_anuncio"].ToString().Trim() + "]<br>" + Localidade.ToString().Trim() + "</div>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";
            StrTable += "</table>";

            //**********
            oDr.Close();
            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazInfoLoja(string CodLoja, int Acao)
        {
            string StrSql = "";
            string StrTable = "";
            string link = "";
            string nome_cliente = "Site Car";
            string desc_cliente = "";
            int x = 0, cod_loja = 0, cod_hotsite = 0;
            bool bl_existe = false;

            try
            {
                cod_loja = Convert.ToInt16(CodLoja);
            }
            catch (Exception Err)
            {
                cod_loja = 0;
            }

            //*****************
            this.AbreConexao();
            //*****************
            StrSql =          " SELECT	HotSite.cd_hotsite, Cliente.* ";
            StrSql = StrSql + " FROM	Cliente, HotSite ";
            StrSql = StrSql + " WHERE	Cliente.cd_cliente = HotSite.cd_cliente ";
            StrSql = StrSql + " AND 	Cliente.cd_cliente = " + cod_loja.ToString().Trim();
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_ini_vig, ' 00:00:00')), '%d/%m/%Y %H:%i:%s')	<= now() ";
            StrSql = StrSql + " AND		STR_TO_DATE((CONCAT(HotSite.dh_fim_vig, ' 23:43:59')), '%d/%m/%Y %H:%i:%s')	>= now() ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************
            if (oDr.Read())
            {
                bl_existe = true;
                if (oDr["tp_pessoa"].ToString().Trim() == "J")
                {
                    nome_cliente = oDr["emp_nome"].ToString().Trim() != "" ? oDr["emp_nome"].ToString().Trim() : oDr["res_nome"].ToString().Trim() + " - Site Car";
                    cod_hotsite = (int)oDr["cd_hotsite"];
                    desc_cliente = " Loja: " + oDr["emp_nome"].ToString().Trim() + "<br><br>";
                    desc_cliente += " Endereço: " + oDr["emp_endereco"].ToString().Trim() + ", " + oDr["emp_numero"].ToString().Trim() + "<br>";
                    desc_cliente += " Ref.: " + oDr["emp_complemento"].ToString().Trim() + "<br>";
                    desc_cliente += " Bairro: " + oDr["emp_bairro"].ToString().Trim() + "<br>";
                    desc_cliente += " Cidade/UF: " + oDr["emp_cidade"].ToString().Trim() + "/" + oDr["emp_uf"].ToString().Trim() + "<br>";
                    desc_cliente += " Telefone: (" + oDr["emp_ddd_telefone"].ToString().Trim() + ") " + oDr["emp_telefone"].ToString().Trim() + "<br>";
                    //desc_cliente += " Contato(s): " + oDr["res_nome"].ToString().Trim() + "<br>";
                }
                else
                {
                    nome_cliente = oDr["res_nome"].ToString().Trim() + " - Site Car";
                    cod_hotsite = (int)oDr["cd_hotsite"];
                    desc_cliente = " Proprietário: " + oDr["res_nome"].ToString().Trim() + "<br><br>";
                    desc_cliente += " Endereço: <span title='" + oDr["con_endereco"].ToString().Trim() + "'>" + (oDr["con_endereco"].ToString().Trim().Length > 20 ? (oDr["con_endereco"].ToString().Trim().Substring(0, 20) + "...") : oDr["con_endereco"].ToString().Trim()) + "</span>,<br>Nr: " + oDr["con_numero"].ToString().Trim() + "<br>";
                    desc_cliente += " Ref.: " + oDr["con_complemento"].ToString().Trim() + "<br>";
                    desc_cliente += " Bairro: " + oDr["con_bairro"].ToString().Trim() + "<br>";
                    desc_cliente += " Cidade/UF: " + oDr["con_cidade"].ToString().Trim() + "/" + oDr["con_uf"].ToString().Trim() + "<br>";
                    desc_cliente += " Telefone: (" + oDr["con_ddd_telefone"].ToString().Trim() + ") " + oDr["con_telefone"].ToString().Trim() + "<br>";
                    desc_cliente += " Celular: (" + oDr["con_ddd_celular"].ToString().Trim() + ") " + oDr["con_celular"].ToString().Trim() + "<br>";
                }
            }
            //**********
            oDr.Close();
            //******************
            this.FechaConexao();
            //******************

            if (Acao == 0)
            {
                return nome_cliente;
            }

            StrTable  = "<table border='0' cellpadding='0' cellspacing='0' width='100%' valign='top' style='margin-top: 16px;' align='center'>";
            if (bl_existe)
            {
                StrTable += "   <tr>";
                StrTable += "       <td nowrap align='center' colspan='2'>";
                StrTable += "           <img src='Imagens/Lojas/" + cod_hotsite.ToString().Trim() + "_b.jpg' border='0' align='center' /><br><br>";
                StrTable += "       </td>";
                StrTable += "   </tr>";
                StrTable += "   <tr>";
                StrTable += "       <td nowrap style='height: 30px; width: 50%;' align='center'>";
                StrTable += "           <div class='NaoEsqueca' style='color: black; text-align: left; margin-left: 30px;'>" + desc_cliente.Trim() + "</div>";
                StrTable += "       </td>";
                StrTable += "       <td nowrap style='height: 30px;' align='left'>";
                StrTable += "           <img class='ImagemLojaBrd' style='width: 250px;' src='Imagens/Lojas/" + cod_hotsite.ToString().Trim() + ".jpg' /><br><br>";
                StrTable += "       </td>";
                StrTable += "   </tr>";
                StrTable += "   <tr>";
                StrTable += "       <td nowrap style='height: 26px;' class='linha_info_site' colspan='2'>";
                StrTable += "           <div class='TituloTela'>Veículos da Loja</div>";
                StrTable += "       </td>";
                StrTable += "   </tr>";
            }
            else
            {
                StrTable += "   <tr>";
                StrTable += "       <td nowrap style='height: 100px; width: 100%;' align='center' colspan='2'>";
                StrTable += "           <div class='info_n_loja' align='center'>Loja não encontrada</div>";
                StrTable += "       </td>";
                StrTable += "   </tr>";
            }
            StrTable += "</table>";

            if (Acao == 0)
            {
                return nome_cliente;
            }


            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazModeloBuscaRapida(string infobanco, string camposel, Int16 codigo, string UF)
        {
            string StrSql = "";
            string StrTable = "";
            Int16 x = 0;
            string link = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT	AcompanhanteFoto.cd_foto, AcompanhanteFoto.extensao, Acompanhante.nm_acompanhante, Acompanhante.cd_acompanhante, Acompanhante.cidade, Acompanhante.uf, Acompanhante.dt_nascimento ";
            StrSql = StrSql + " FROM	Acompanhante, AcompanhanteFoto ";
            StrSql = StrSql + " WHERE	Acompanhante.cd_acompanhante	= AcompanhanteFoto.cd_acompanhante ";
            StrSql = StrSql + " AND		Acompanhante." + camposel + "   =   " + codigo.ToString().Trim();
            StrSql = StrSql + " AND		Acompanhante.bl_ativo			= 1 ";
            StrSql = StrSql + " AND		AcompanhanteFoto.cd_foto		IN    (	SELECT	MIN(AcFt.cd_foto)";
            StrSql = StrSql + " 	    								FROM	AcompanhanteFoto AcFt ";
            StrSql = StrSql + "     									WHERE	AcFt.cd_acompanhante	=  AcompanhanteFoto.cd_acompanhante )";
            StrSql = StrSql + " AND     Acompanhante.uf LIKE    '%" + UF.Trim() + "%'";
            StrSql = StrSql + " ORDER   BY RAND() ";

            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='96%' valign='top' style='margin-right: 10px;' align='center'>";
            StrTable += "<tr>";

            while (oDr.Read())
            {
                link = "style='cursor: hand; cursor: pointer;' onclick='javascript:window.location.href = " + '"' + "BuscaRapida.aspx?codmodelo=" + oDr[3].ToString().Trim() + '"' + ";'";

                if (x % 4 == 0 && x != 0)
                {
                    StrTable += "</tr>";
                    StrTable += "<tr><td colspan='4'><br><br><td></tr>";
                    StrTable += "<tr>";
                }
                x++;

                StrTable += "<td style='width: 25%;' align='center'>";
                StrTable += "   <table border='0' cellpadding='0' cellspacing='0' style='width: 140px; height: 187px;' class='ModuraModelo'>";
                StrTable += "       <tr>";
                StrTable += "           <td valign='middle' align='center'>";
                StrTable += "               <img class='ImagemBrd' width='120px' height='167px' src='Imagens/Fotos/" + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim() + "' " + link + " align='absmiddle' title='Clique para visualizar o anúncio' style='cursor: hand; cursor: pointer;'>";
                StrTable += "           </td>";
                StrTable += "       </tr>";
                StrTable += "   </table>";
                StrTable += "<br><span class='MostraModeloNome'>" + oDr[2].ToString().Trim() + "<br>" + Convert.ToString(DateTime.Now.Year - Convert.ToDateTime(oDr["dt_nascimento"].ToString().Trim()).Year).ToString().Trim() + " anos <br>" + oDr["cidade"].ToString().Trim() + "/" + oDr["uf"].ToString().Trim() + "</span>";
                StrTable += "</td>";
            }
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string TrazModeloBusca(Int16 codigo, string LocalUrl, string UF)
        {
            bool bl_prima = true;
            string nome = "";
            string StrSql = "";
            string StrTable = "";
            Int16 x = 0;
            string link = "";

            string nm_sexo = "";
            string nm_cabelos = "";
            string nm_coxas = "";
            string nm_fisico = "";
            string nm_labios = "";
            string nm_olhos = "";
            string nm_pele = "";
            string nm_pes = "";
            string nm_seios = "";
            string info_altura = "";
            string info_peso = "";
            string info_bumbum = "";
            string idade = "";

            string info_sexo_vaginal = "";
            string info_sexo_oral = "";
            string info_sexo_anal = "";
            string info_fantasia = "";
            string info_atende = "";
            string info_local = "";
            string info_horario = "";

            string vl_preco_1hr = "";
            string vl_preco_2hr = "";
            string vl_preco_noite = "";
            string info_pagamento = "";

            string info_estilo = "";
            string info_laser = "";
            string login_email = "";

            //*****************
            this.AbreConexao();
            //*****************

            StrSql = "          SELECT	AcompanhanteFoto.cd_foto, AcompanhanteFoto.extensao, Acompanhante.nm_acompanhante, Acompanhante.telefone, Acompanhante.observacao, Acompanhante.dt_nascimento, Acompanhante.info_bumbum, Acompanhante.info_altura, Acompanhante.info_peso, Acompanhante.info_estilo, Acompanhante.info_laser, ";
            StrSql = StrSql + " 		Sexo.nm_sexo, Cabelos.nm_cabelos, Coxas.nm_coxas, Fisico.nm_fisico, Labios.nm_labios, Olhos.nm_olhos, Pele.nm_pele, Pes.nm_pes, Seios.nm_seios, Acompanhante.vl_preco_1hr, Acompanhante.vl_preco_2hr, Acompanhante.vl_preco_noite, Acompanhante.info_pagamento, Acompanhante.login as login_email, ";
            StrSql = StrSql + " 		Acompanhante.info_sexo_vaginal, Acompanhante.info_sexo_oral, Acompanhante.info_sexo_anal, Acompanhante.info_fantasia, Acompanhante.info_atende, Acompanhante.info_local, Acompanhante.info_horario, Acompanhante.cidade, Acompanhante.uf ";
            StrSql = StrSql + " FROM	Acompanhante, AcompanhanteFoto, Sexo, Cabelos, Coxas, Fisico, Labios, Olhos, Pele, Pes, Seios ";
            StrSql = StrSql + " WHERE	Acompanhante.cd_acompanhante	= AcompanhanteFoto.cd_acompanhante ";
            StrSql = StrSql + " AND		Acompanhante.cd_sexo	        = Sexo.cd_sexo ";
            StrSql = StrSql + " AND		Acompanhante.cd_cabelos		    = Cabelos.cd_cabelos ";
            StrSql = StrSql + " AND		Acompanhante.cd_coxas	        = Coxas.cd_coxas ";
            StrSql = StrSql + " AND		Acompanhante.cd_fisico	        = Fisico.cd_fisico ";
            StrSql = StrSql + " AND		Acompanhante.cd_labios	        = Labios.cd_labios ";
            StrSql = StrSql + " AND		Acompanhante.cd_olhos	        = Olhos.cd_olhos ";
            StrSql = StrSql + " AND		Acompanhante.cd_pele	        = Pele.cd_pele ";
            StrSql = StrSql + " AND		Acompanhante.cd_pes		        = Pes.cd_pes ";
            StrSql = StrSql + " AND		Acompanhante.cd_seios	        = Seios.cd_seios ";
            StrSql = StrSql + " AND		Acompanhante.cd_acompanhante	=   " + codigo.ToString();
            StrSql = StrSql + " AND 	Acompanhante.bl_ativo			= 1 ";
            StrSql = StrSql + " AND     Acompanhante.uf LIKE    '%" + UF.Trim() + "%'";
            
            oCmd.Connection = this.oConn;
            //*************************************
            oCmd.CommandText = StrSql;
            oDr = oCmd.ExecuteReader();
            //*************************

            StrTable += "<table border='0' cellpadding='0' cellspacing='0' width='95%' valign='top' align='center'>";

            while (oDr.Read())
            {
                nome = oDr[2].ToString().Trim();
                nm_sexo = oDr["nm_sexo"].ToString().Trim();
                nm_cabelos = oDr["nm_cabelos"].ToString().Trim();
                nm_coxas = oDr["nm_coxas"].ToString().Trim();
                nm_fisico = oDr["nm_fisico"].ToString().Trim();
                nm_labios = oDr["nm_labios"].ToString().Trim();
                nm_olhos = oDr["nm_olhos"].ToString().Trim();
                nm_pele = oDr["nm_pele"].ToString().Trim();
                nm_pes = oDr["nm_pes"].ToString().Trim();
                nm_seios = oDr["nm_seios"].ToString().Trim();
                info_altura = oDr["info_altura"].ToString().Trim();
                info_peso = oDr["info_peso"].ToString().Trim();
                info_bumbum = oDr["info_bumbum"].ToString().Trim();
                idade = Convert.ToString(DateTime.Now.Year - Convert.ToDateTime(oDr["dt_nascimento"]).Year).ToString();

                info_sexo_vaginal = oDr["info_sexo_vaginal"].ToString().Trim();
                info_sexo_oral = oDr["info_sexo_oral"].ToString().Trim();
                info_sexo_anal = oDr["info_sexo_anal"].ToString().Trim();
                info_fantasia = oDr["info_fantasia"].ToString().Trim();
                info_atende = oDr["info_atende"].ToString().Trim();
                info_local = oDr["info_local"].ToString().Trim();
                info_horario = oDr["info_horario"].ToString().Trim();

                vl_preco_1hr = oDr["vl_preco_1hr"].ToString().Trim();
                vl_preco_2hr = oDr["vl_preco_2hr"].ToString().Trim();
                vl_preco_noite = oDr["vl_preco_noite"].ToString().Trim();
                info_pagamento = oDr["info_pagamento"].ToString().Trim();

                info_estilo = oDr["info_estilo"].ToString().Trim();
                info_laser = oDr["info_laser"].ToString().Trim();
                login_email = oDr["login_email"].ToString().Trim();

                if (bl_prima)
                {
                    StrTable += "<tr>";
                    StrTable += "   <td colspan='3' style='width: 100%;' align='center'>";
                    StrTable += "       <span class='BuscaModeloNomeTelefone'>";
                    StrTable += "           " + oDr[2].ToString().Trim() + "<br>" + oDr[3].ToString().Trim();
                    StrTable += "       </span>";
                    StrTable += "       <div class='BuscaModeloObservacao'>";
                    StrTable += "           <br>" + idade + " anos - " + oDr["cidade"].ToString().Trim() + "/" + oDr["uf"].ToString().Trim() + ". " + oDr[4].ToString().Trim() + "<br><br>";
                    StrTable += "       </div>";
                    StrTable += "   </td>";
                    StrTable += "</tr>";

                    StrTable += "<tr>";
                    StrTable += "   <td colspan='3' style='width: 100%; height: 455px; align='center'>";
                    StrTable += "       <table border='0' cellpadding='3' cellspacing='3' class='ModuraModelo'>";
                    StrTable += "           <tr>";
                    StrTable += "               <td valign='middle' align='center'>";
                    StrTable += "                   <img class='ImagemBrd' name='fotoprinc' id='fotoprinc' src='Imagens/Fotos/" + oDr[0].ToString().Trim() + oDr[1].ToString().Trim() + "' align='absmiddle' title='Clique para visualizar o anúncio'>";
                    StrTable += "               </td>";
                    StrTable += "           </tr>";
                    StrTable += "       </table>";
                    StrTable += "   </td>";
                    StrTable += "</tr>";

                    StrTable += "<tr>";
                    StrTable += "   <td colspan='3' align='center' nowrap>";
                    StrTable += "       <table border='0' align='center'>";
                    StrTable += "           <tr>";


                    StrTable += "<td id='td_avanca' name='td_avanca'>";
                    StrTable += "<image name='imgVolta' id='imgVolta' src='Layout/seta_volta_off.jpg' align='AbsMiddle' onmouseover='mudafigura(this, 1);' onmouseout='mudafigura(this, 0);' style='cursor: hand; cursor: pointer;' onclick='mudafotos(0);'>";
                    StrTable += "</td>";
                }
                bl_prima = false;

                if (!File.Exists(LocalUrl.ToString().Trim() + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim()))
                {
                    continue;
                }

                //if (x % 5 == 0 && x != 0)
                //{
                //    StrTable += "           </tr>";
                //    StrTable += "       </table>";
                //    StrTable += "   </td>";
                //    StrTable += "</tr>";
                //    StrTable += "<tr>";
                //    StrTable += "   <td colspan='3' align='center' nowrap>";
                //    StrTable += "       <table border='0' align='center'>";
                //    StrTable += "           <tr>";
                //}
                //x++;

                if (x > 5)
                {
                    StrTable += "                   <td id='td_" + x.ToString().Trim() + "' name='td_" + x.ToString().Trim() + "' style='display: none;'>";
                    StrTable += "                       <table border='0' cellpadding='2' cellspacing='2' class='ModuraModelo' style='height: 100px;'>";
                    StrTable += "                           <tr>";
                    StrTable += "                               <td valign='middle' align='center' nowrap><img onclick='javascript:mostrafoto(this);'  style='cursor: hand; cursor: pointer; width: 70px;' src='Imagens/Fotos/" + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim() + "'></td>";
                    StrTable += "                           </tr>";
                    StrTable += "                       </table>";
                    StrTable += "                   </td>";
                }
                else
                {
                    StrTable += "                   <td id='td_" + x.ToString().Trim() + "' name='td_" + x.ToString().Trim() + "'>";
                    StrTable += "                       <table border='0' cellpadding='2' cellspacing='2' class='ModuraModelo' style='height: 100px;'>";
                    StrTable += "                           <tr>";
                    StrTable += "                               <td valign='middle' align='center' nowrap><img onclick='javascript:mostrafoto(this);'  style='cursor: hand; cursor: pointer; width: 70px;' src='Imagens/Fotos/" + oDr[0].ToString().Trim() + "_p" + oDr[1].ToString().Trim() + "'></td>";
                    StrTable += "                           </tr>";
                    StrTable += "                       </table>";
                    StrTable += "                   </td>";
                }
            
                x++;
            }

            StrTable += "<td id='td_volta' name='td_volta'>";
            StrTable += "<image name='imgAvanca' id='imgAvanca' src='Layout/seta_avanca_off.jpg' align='AbsMiddle' onmouseover='mudafigura(this, 1);' onmouseout='mudafigura(this, 0);' style='cursor: hand; cursor: pointer;' onclick='mudafotos(1);'>";
            StrTable += "</td>";

            StrTable += "<td id='td_opcao' name='td_opcao' style='display: none;'>";
            StrTable += "   <table border='0' cellpadding='2' cellspacing='2' class='ModuraModelo' style='height: 100px;'>";
            StrTable += "       <tr>";
            StrTable += "           <td valign='middle' align='center' nowrap>";
            StrTable += "               <input type='text' id='txtqt_fotos' name='txtqt_fotos' value='" + x.ToString().Trim() + "'>";
            StrTable += "               <input type='text' id='txtpos_foto' name='txtpos_foto' value='0'>";
            StrTable += "           </td>";
            StrTable += "       </tr>";
            StrTable += "   </table>";
            StrTable += "</td>";

            if (bl_prima)
            {
                StrTable += "<tr>";
                StrTable += "   <td colspan='3' style='width: 100%;' align='center'>";
                StrTable += "       <span class='BuscaModeloNomeTelefone'>";
                StrTable += "           Modelo não disponível";
                StrTable += "       </span>";
                StrTable += "   </td>";
                StrTable += "</tr>";
                StrTable += "<tr>";
                StrTable += "   <td colspan='3' align='center' nowrap>";
                StrTable += "       <table align='center'>";
                StrTable += "           <tr>";
            }

            StrTable += "                   </tr>";
            StrTable += "               </table>";
            StrTable += "               <br>";
            StrTable += "           </td>";
            StrTable += "       </tr>";

            StrTable += "<tr>";
            StrTable += "   <td colspan='3' style='width: 30%;' align='center'></td>";
            StrTable += "<tr>";

            StrTable += "<tr>";
            StrTable += "   <td style='width: 30%;' align='center'>";
            StrTable += "       <table style='width: 179px;' border='0' bgcolor='#950A2B' cellpadding='0' cellspacing='0'>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_cima_modelo.jpg'>";
            StrTable += "                   <div class='InfoModeloTitulo'>";
            StrTable += "                       Perfil:";
            StrTable += "                   </div>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td style='width:173px;'>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Sexo: " + nm_sexo + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Idade: " + idade + " anos</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Cabelo: " + nm_cabelos.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Cor Pele: " + nm_pele.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Físico: " + nm_fisico.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Altura: " + info_altura.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Peso: " + info_peso.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Olhos: " + nm_olhos.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Lábios: " + nm_labios.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Seios: " + nm_seios.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Bumbum: " + info_bumbum.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Coxas: " + nm_coxas.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Pés: " + nm_pes.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Estilo: " + info_estilo.ToString() + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Lazer: " + info_laser.ToString() + "</div>";
            StrTable += "               </td>";
            StrTable += "               <td style='width: 6px; background-image: url(Layout/info_sombra_modelo.jpg);'></td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_baixo_modelo.jpg' align='absmiddle'>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "       </table>";
            StrTable += "   </td>";

            StrTable += "   <td style='width: 30%;' align='center' valign='top'>";
            StrTable += "       <table style='width: 179px;' border='0' bgcolor='#950A2B' cellpadding='0' cellspacing='0'>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_cima_modelo.jpg'>";
            StrTable += "                   <div class='InfoModeloTitulo'>";
            StrTable += "                       O que faço:";
            StrTable += "                   </div>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td style='width:173px;'>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Sexo Oral: " + info_sexo_oral + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Sexo Vaginal: " + info_sexo_vaginal + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Sexo Anal: " + info_sexo_anal + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Fantasias: " + info_fantasia + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Com quem: " + info_atende + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Disponibilidade: " + info_horario + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Local: " + info_local + "</div>";
            StrTable += "               </td>";
            StrTable += "               <td style='width: 6px; background-image: url(Layout/info_sombra_modelo.jpg);'></td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_baixo_modelo.jpg' align='absmiddle'>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "       </table>";
            StrTable += "   </td>";

            StrTable += "   <td style='width: 30%;' align='center' valign='top'>";
            StrTable += "       <table style='width: 179px;' border='0' bgcolor='#950A2B' cellpadding='0' cellspacing='0'>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_cima_modelo.jpg'>";
            StrTable += "                   <div class='InfoModeloTitulo'>";
            StrTable += "                       Quanto cobro:";
            StrTable += "                   </div>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td style='width:173px;'>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Por uma hora: " + vl_preco_1hr + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Por duas horas: " + vl_preco_2hr + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Por uma noite: " + vl_preco_noite + "</div>";
            StrTable += "                   <div class='InfoModeloDetalhes'>Pagamento: " + info_pagamento + "</div>";
            StrTable += "               </td>";
            StrTable += "               <td style='width: 6px; background-image: url(Layout/info_sombra_modelo.jpg);'></td>";
            StrTable += "           </tr>";
            StrTable += "           <tr>";
            StrTable += "               <td colspan='2'>";
            StrTable += "                   <img src='Layout/info_baixo_modelo.jpg' align='absmiddle'>";
            StrTable += "               </td>";
            StrTable += "           </tr>";
            StrTable += "       </table>";
            StrTable += "   </td>";
            StrTable += "</tr>";

            StrTable += "<tr>";
            StrTable += "   <td colspan='3' style='width: 100%;' align='center'>";
            StrTable += "       <br>";
            StrTable += "       <span class='BuscaModeloContato'>";
            StrTable += "           <br>Para contato online utilize o e-mail: " + login_email.ToString().Trim() + "@capixabacar.com.br.<br>";
            StrTable += "       </span>";
            StrTable += "       <span class='InfoViuSite'>";
            StrTable += "           Quando ligar para mim, não esqueça de dizer que me viu no noitebrasil.com.br.";
            StrTable += "       </span>";
            StrTable += "   </td>";
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string BuscaRapida()
        {
            string StrSql = "";
            string StrTable = "";
            string seta = "";
            string link = "";

            StrTable += "<table cellpadding='0' cellspacing='0' border='0' style='width: 169px; background-image: url(Layout/fundo.jpg);'>";
            StrTable += "<tr>";
            StrTable += "   <td colspan='2' style='height:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            link = " onmouseover='javascript:mudacor(this,0);' onmouseout='javascript:mudacor(this,1);'";

            StrTable += "<tr onclick=chamatela('buscarapida.aspx?buscar=1')>";
            StrTable += "    <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='height: 20px; width:143px' " + link.Trim() + ">";
            StrTable += "        <span class='info_menu'>Morenas</span>";
            StrTable += "    </td>";
            StrTable += "    <td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            StrTable += "<tr onclick=chamatela('buscarapida.aspx?buscar=2')>";
            StrTable += "    <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='height: 20px; width:143px' " + link.Trim() + ">";
            StrTable += "        <span class='info_menu'>Loiras</span>";
            StrTable += "    </td>";
            StrTable += "    <td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            StrTable += "<tr onclick=chamatela('buscarapida.aspx?buscar=3')>";
            StrTable += "    <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='height: 20px; width:143px' " + link.Trim() + ">";
            StrTable += "        <span class='info_menu'>Mulatas</span>";
            StrTable += "    </td>";
            StrTable += "    <td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            StrTable += "<tr onclick=chamatela('buscarapida.aspx?buscar=4')>";
            StrTable += "    <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='height: 20px; width:143px' " + link.Trim() + ">";
            StrTable += "        <span class='info_menu'>Negras</span>";
            StrTable += "    </td>";
            StrTable += "    <td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            StrTable += "<tr onclick=chamatela('buscarapida.aspx?buscar=5')>";
            StrTable += "    <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='height: 20px; width:143px' " + link.Trim() + ">";
            StrTable += "        <span class='info_menu'>Orientais</span>";
            StrTable += "    </td>";
            StrTable += "    <td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "    <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td>";
            StrTable += "   <td style='background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";

            StrTable += "<tr>";
            StrTable += "   <td colspan='3' style='height: 10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
            StrTable += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "<tr>";
            StrTable += "   <td colspan='4' style='height: 5px; background-image: url(Layout/sombra_horizontal.jpg);'></td>";
            StrTable += "</tr>";
            StrTable += "</table>";

            oDr.Close();
            //**********

            //******************
            this.FechaConexao();
            //******************

            return StrTable.ToString().Trim();
            //********************************
        }

        public string CarregaMenuInferior()
        {
            string info_menu = "";

            info_menu += "<div class='link_inferior'>Produto desenvolvido por Brubru Sistemas OnLine - Direitos 2009</div>";
            return info_menu;
        }

        public bool TrazSelecao(object DDL, string Tabela, string Valor)
        {
            string Selecao = "";
            bool Resp = true;
            string StrSql = "";
            DropDownList ListaMenu = (DropDownList)DDL;

            ListaMenu.Items.Clear();

            //*************************************************************************************
            if (!this.AbreConexao()) { this.critica = this.critica; return false; }
            //*************************************************************************************
            try
            {
                if (Tabela.ToString().ToUpper() == "CLIENTE")
                {
                    StrSql += " SELECT  cd_cliente, nm_cliente ";
                    StrSql += " FROM    Cliente ";
                }
                else
                {
                    StrSql += " SELECT  cd_" + Tabela.ToString().Trim() + ", nm_" + Tabela.ToString().Trim();
                    StrSql += " FROM    " + Tabela.ToString().Trim() + "   ";
                    StrSql += " ORDER   BY nm_" + Tabela.ToString().Trim();
                }

                //  ListaMenu.Items.Add(new ListItem(" -- " + Tabela.ToString().Trim() + " Disponíveis -- ", "0"));

                oCmd.Connection = this.oConn;
                //*************************************
                while (oDr.Read())
                {
                    if (Tabela.ToString().ToUpper() == "CLIENTE")
                    {
                        if (oDr[1].ToString().Trim() != "")
                        {
                            ListaMenu.Items.Add(new ListItem(oDr[3].ToString().Trim() + " - " + oDr[1].ToString().Trim(), (string)oDr[0].ToString().Trim()));
                        }
                        else
                        {
                            ListaMenu.Items.Add(new ListItem(oDr[3].ToString().Trim() + " - " + oDr[2].ToString().Trim(), (string)oDr[0].ToString().Trim()));
                        }
                    }
                    else 
                    {
                        ListaMenu.Items.Add(new ListItem(oDr[1].ToString().Trim(), (string)oDr[0].ToString().Trim()));
                    }
                }
                ListaMenu.Items.FindByValue(Valor).Selected = true;
                oDr.Close();
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
                Resp = false;
            }

            //**************************************************************************************
            if (!this.FechaConexao()) { this.critica = this.critica; return false; }
            //**************************************************************************************

            return Resp;
            //**********
        }

        public bool TrazModeloVeiculo(string TpAnuncio, object DDL, string CodMarca, string Valor)
        {
            string Selecao = "";
            bool Resp = true;
            string StrSql = "";
            DropDownList ListaMenu = (DropDownList)DDL;

            ListaMenu.Items.Clear();
            ListaMenu.Items.Add(new ListItem("---", "0"));

            //*************************************************************************************
            if (!this.AbreConexao()) { this.critica = this.critica; return false; }
            //*************************************************************************************
            try
            {
                StrSql += " SELECT  Modelo.cd_modelo, Modelo.nm_modelo, Tp_Veiculo.nm_tp_veiculo ";
                StrSql += " FROM    Modelo, Tp_Veiculo ";
                StrSql += " WHERE   Modelo.cd_tp_anuncio = Tp_Veiculo.cd_tp_anuncio ";
                StrSql += " AND     Modelo.cd_marca      = Tp_Veiculo.cd_marca ";
                StrSql += " AND     Modelo.cd_tp_veiculo = Tp_Veiculo.cd_tp_veiculo ";
                StrSql += " AND     Tp_Veiculo.cd_tp_anuncio = " + TpAnuncio.Trim();
                StrSql += " AND     Tp_Veiculo.cd_marca      = " + CodMarca.Trim();
                StrSql += " ORDER   BY Tp_Veiculo.nm_tp_veiculo, Modelo.nm_modelo ";

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                while (oDr.Read())
                {
                    ListaMenu.Items.Add(new ListItem(oDr[2].ToString().Trim() + " | " + oDr[1].ToString().Trim(), (string)oDr[0].ToString().Trim()));
                }
                ListaMenu.Items.FindByValue(Valor).Selected = true;
                oDr.Close();
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
                Resp = false;
            }

            //**************************************************************************************
            if (!this.FechaConexao()) { this.critica = this.critica; return false; }
            //**************************************************************************************

            return Resp;
            //**********
        }

        public string AtualizaBannerCliques(string cod_banner)
        {
            string StrSql = "";
            string StrUrl = "";

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.cd_banner = " + cod_banner.ToString().Trim();

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                if (oDr.Read())
                {
                    // oCmd.Connection = this.oConn;
                    oCmd.CommandText = " Update Banner Set nr_clique = nr_clique + 1 WHERE cd_banner = " + oDr["cd_banner"].ToString().Trim();
                    oCmd.ExecuteNonQuery();
                    //*********************

                    StrUrl = oDr["link_url"].ToString().Trim();
                }
                
                oDr.Close();
                //**********
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return StrUrl;
            //************
        }

        public string TrazLinkBanner(string codigo)
        { 
            string StrSql = "";
            string Link_URL = "default.aspx";

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.cd_banner    = " + codigo.ToString().Trim();

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                if (oDr.Read())
                {
                    Link_URL = oDr["link_url"].ToString().Trim();
                    oDr.Close();
                    //**********
                    oCmd.CommandText = " Update Banner Set nr_clique = nr_clique + 1 WHERE cd_banner = " + codigo.ToString().Trim();
                    oCmd.ExecuteNonQuery();
                    //*********************
                }
                else
                {
                    oDr.Close();
                    //**********
                }
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return Link_URL;
            //**************
        }

        public string CarregaBannerSuperior()
        {
            string StrSql = "";
            string StrBanner = "";
            Int16 cd_banner = 0;

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.cd_banner, Banner.extensao, Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.qt_combina >= Banner.qt_aparicao ";
                StrSql += " AND     Banner.tp_banner    = 0 ";
                StrSql += " AND     Banner.bl_ativo     = 1 ";
                StrSql += " ORDER   BY RAND() LIMIT 1 ";

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                if (oDr.Read())
                {
                    cd_banner = Convert.ToInt16(oDr["cd_banner"]);
                    if (oDr["link_url"].ToString().Trim() == "")
                    {
                        StrBanner += "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 468px; height: 60px;'>";
                    }
                    else
                    {
                        StrBanner += "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 468px; height: 60px; cursor: hand; cursor: pointer;' onclick='window.open(" + '"' + "AbreLink.aspx?link=" + oDr["cd_banner"].ToString().Trim() + '"' + ");'>";
                    }
                    oDr.Close();
                    //**********
                    oCmd.CommandText = " Update Banner Set qt_aparicao = qt_aparicao + 1 WHERE cd_banner = " + cd_banner.ToString().Trim();
                    oCmd.ExecuteNonQuery();
                    //*********************
                }
                else
                {
                    oDr.Close();
                    //**********
                }
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return StrBanner;
            //***************
        }

        public string CarregaBannerLateralEsquerdo()
        {
            string StrSql = "";
            string StrBanner = "";
            Int16 cd_banner = 0;

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.cd_banner, Banner.extensao, Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.qt_combina >= Banner.qt_aparicao ";
                StrSql += " AND     Banner.tp_banner    = 1 ";
                StrSql += " AND     Banner.bl_ativo     = 1 ";
                StrSql += " ORDER   BY RAND() LIMIT 1 ";

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                if (oDr.Read())
                {
                    StrBanner += "<table cellpadding='0' cellspacing='0' border='0' style='width: 169px; background-image: url(Layout/fundo.jpg);'>";
                    StrBanner += "<tr>    ";
                    StrBanner += "<td colspan='2' style='height:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                    StrBanner += "</tr> ";
                    StrBanner += "<tr> ";
                    StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td> ";
                    StrBanner += "<td style='background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                    StrBanner += "</tr> ";

                    cd_banner = Convert.ToInt16(oDr["cd_banner"]);

                    StrBanner += "<tr> ";
                    StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td align='left' style='height: 20px; width:198px'> ";
                    if (oDr["link_url"].ToString().Trim() == "")
                    {
                        StrBanner += "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 198px;'>";
                    }
                    else
                    {
                        StrBanner += "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 198px; cursor: hand; cursor: pointer;' onclick='window.open(" + '"' + "AbreLink.aspx?link=" + oDr["cd_banner"].ToString().Trim() + '"' + ");'>";
                    }
                    StrBanner += "</td> ";
                    StrBanner += "<td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                    StrBanner += "</tr> ";
                    StrBanner += "<tr> ";
                    StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td> ";
                    StrBanner += "<td style='background-image: url(Layout/fundo_menu.jpg);'></td> ";
                    StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                    StrBanner += "</tr> ";

                    StrBanner += "<tr>";
                    StrBanner += "   <td colspan='3' style='height: 10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
                    StrBanner += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
                    StrBanner += "</tr>";
                    StrBanner += "<tr>";
                    StrBanner += "   <td colspan='4' style='height: 5px; background-image: url(Layout/sombra_horizontal.jpg);'></td>";
                    StrBanner += "</tr>";
                    StrBanner += "</table>";

                    oDr.Close();
                    //**********
                    oCmd.CommandText = " Update Banner Set qt_aparicao = qt_aparicao + 1 WHERE cd_banner = " + cd_banner.ToString().Trim();
                    oCmd.ExecuteNonQuery();
                    //*********************
                }
                else
                {
                    oDr.Close();
                    //**********
                }
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return StrBanner;
            //***************
        }

        public string CarregaBannerLateralDireito()
        {
            string StrSql = "";
            string StrBanner = "";
            Int16 cd_banner = 0;

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.cd_banner, Banner.extensao, Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.qt_combina >= Banner.qt_aparicao ";
                StrSql += " AND     Banner.tp_banner    = 2 ";
                StrSql += " AND     Banner.bl_ativo     = 1 ";
                StrSql += " ORDER   BY RAND() LIMIT 5 ";

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = "SET DATEFORMAT MDY";
                oCmd.ExecuteNonQuery();
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                while (oDr.Read())
                {
                    cd_banner = Convert.ToInt16(oDr["cd_banner"]);
                    if (oDr["link_url"].ToString().Trim() == "")
                    {
                        StrBanner += "<img src='../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0'><br>";
                    }
                    else
                    {
                        StrBanner += "<img src='../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='cursor: hand; cursor: pointer;' onclick='window.open(" + '"' + "AbreLink.aspx?link=" + oDr["cd_banner"].ToString().Trim() + '"' + ");'><br>";
                    }
                    oCmdAux.Connection = this.oConn;
                    oCmdAux.CommandText = " Update Banner Set qt_aparicao = qt_aparicao + 1 WHERE cd_banner = " + cd_banner.ToString().Trim();
                    oCmdAux.ExecuteNonQuery();
                    //************************
                }
                oDr.Close();
                //**********
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return StrBanner;
            //***************
        }

        public string CarregaBannerPopup()
        {
            string StrSql = "";
            string StrBanner = "";
            Int16 cd_banner = 0;

            //*****************
            this.AbreConexao();
            //*****************

            try
            {
                StrSql += " SELECT  Banner.cd_banner, Banner.extensao, Banner.link_url ";
                StrSql += " FROM    Banner ";
                StrSql += " WHERE   Banner.qt_combina >= Banner.qt_aparicao ";
                StrSql += " AND     Banner.tp_banner    = 4 ";
                StrSql += " AND     Banner.bl_ativo     = 1 ";
                StrSql += " ORDER   BY RAND() LIMIT 1 ";

                oCmd.Connection = this.oConn;
                //*************************************
                oCmd.CommandText = StrSql;
                oDr = oCmd.ExecuteReader();
                //*************************
                if (oDr.Read())
                {
                    cd_banner = Convert.ToInt16(oDr["cd_banner"]);
                    StrBanner += "<script language='javascript'>";
                    if (oDr["link_url"].ToString().Trim() == "")
                    {
                        StrBanner += "window.open(" + '"' + "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 198px;'>" + '"' + ");";
                    }
                    else
                    {
                        StrBanner += "window.open(" + '"' + "<img src='../../../../Imagens/Banners/" + oDr["cd_banner"].ToString().Trim() + oDr["extensao"].ToString().Trim() + "' border='0' style='width: 198px; cursor: hand; cursor: pointer;' onclick='window.open(" + '"' + "AbreLink.aspx?link=" + oDr["cd_banner"].ToString().Trim() + '"' + ");'>" + '"' + ");";
                    }
                    StrBanner += "</script>";

                    oDr.Close();
                    //**********
                    oCmd.CommandText = " Update Banner Set qt_aparicao = qt_aparicao + 1 WHERE cd_banner = " + cd_banner.ToString().Trim();
                    oCmd.ExecuteNonQuery();
                    //*********************
                }
                else
                {
                    oDr.Close();
                    //**********
                }
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            //******************
            this.FechaConexao();
            //******************

            return StrBanner;
            //***************
        }

        public string CarregaBannerInferior()
        {
            return "";
        }

        public string CarregaUFs(string UF)
        {
            string StrBanner = "";

            try
            {
                StrBanner += "<table cellpadding='0' cellspacing='0' border='0' style='width: 169px; background-image: url(Layout/fundo.jpg);'>";
                StrBanner += "<tr>    ";
                StrBanner += "<td colspan='2' style='height:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                StrBanner += "</tr> ";
                StrBanner += "<tr> ";
                StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td> ";
                StrBanner += "<td style='background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                StrBanner += "</tr> ";
                StrBanner += "<tr> ";
                StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td align='left' style='height: 20px; width:198px'> ";
                StrBanner += "&nbsp;<select name='selUF' id='selUF' style='background-color: #A70D31; color: white;'>";
                StrBanner += "<option value='%'>Todas UF's</option>";
                //StrBanner += "<option value='AC'>AC</option>";
                //StrBanner += "<option value='AL'>AL</option>";
                //StrBanner += "<option value='AP'>AP</option>";
                //StrBanner += "<option value='AM'>AM</option>";
                StrBanner += "<option value='BA'>BA</option>";
                //StrBanner += "<option value='CE'>CE</option>";
                //StrBanner += "<option value='DF'>DF</option>";
                StrBanner += "<option value='ES'>ES</option>";
                //StrBanner += "<option value='GO'>GO</option>";
                //StrBanner += "<option value='MA'>MA</option>";
                StrBanner += "<option value='MG'>MG</option>";
                //StrBanner += "<option value='MS'>MS</option>";
                //StrBanner += "<option value='PA'>PA</option>";
                //StrBanner += "<option value='PR'>PR</option>";
                //StrBanner += "<option value='PB'>PB</option>";
                //StrBanner += "<option value='PI'>PI</option>";
                //StrBanner += "<option value='RS'>RS</option>";
                //StrBanner += "<option value='RO'>RO</option>";
                //StrBanner += "<option value='RR'>RR</option>";
                StrBanner += "<option value='SC'>SC</option>";
                StrBanner += "<option value='SP'>SP</option>";
                //StrBanner += "<option value='SE'>SE</option>";
                //StrBanner += "<option value='TO'>TO</option>";
                StrBanner += "</select>";
                StrBanner += "&nbsp;&nbsp;<input type='button' value='Ir...' style='width: 34px; font-family: verdana; font-size: 12px;' onclick='javascript:window.location.href = " + '"' + "default.aspx?ufsel=" + '"' + "+window.selUF.value;'>";

                StrBanner += "<script>selUF.value = '" + UF.Trim() + "'</script>";
               
                StrBanner += "</td> ";
                StrBanner += "<td align='center' style='width: 20px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                StrBanner += "</tr> ";
                StrBanner += "<tr> ";
                StrBanner += "<td style='width:10px; background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td colspan='1' style='height:1px; background-image: url(Layout/linha_menu.jpg);'></td> ";
                StrBanner += "<td style='background-image: url(Layout/fundo_menu.jpg);'></td> ";
                StrBanner += "<td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td> ";
                StrBanner += "</tr> ";

                StrBanner += "<tr>";
                StrBanner += "   <td colspan='3' style='height: 10px; background-image: url(Layout/fundo_menu.jpg);'></td>";
                StrBanner += "   <td style='width: 6px; background-image: url(Layout/sombra_vertical.jpg);'></td>";
                StrBanner += "</tr>";
                StrBanner += "<tr>";
                StrBanner += "   <td colspan='4' style='height: 5px; background-image: url(Layout/sombra_horizontal.jpg);'></td>";
                StrBanner += "</tr>";
                StrBanner += "</table>";
            }
            catch (Exception Err)
            {
                this.critica = Err.Message.ToString();
            }

            return StrBanner;
            //***************
        }

        public bool verificaEmail(string email)
        {
            if (email.Trim() == "")
            {
                this.criticaaux = "E-mail deve ser informado. Verifique.";
                return false;
            }
            else
            {
                if ((email.Length != 0) && ((email.IndexOf('@') < 1) || (email.IndexOf('.', email.IndexOf('@')) < 1)))
                {
                    this.criticaaux = "E-mail inválido. Verifique.";
                    return false;
                }
            }

            return true;
        }

        public bool validaData(string data)
        {
            DateTime TmpDate;

            if (data.ToString().Trim() == "")
            {
                this.criticaaux = "não foi informada.";
                return false;
            }
            else
            {
                try
                {
                    TmpDate = Convert.ToDateTime(data);
                }
                catch (Exception Err)
                {
                    this.criticaaux = "está inválida.";
                    return false;
                }
            }

            return true;
            //**********
        }
    }
}

