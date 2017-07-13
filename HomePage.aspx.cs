using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using PASABAHCE_URETIM_TAKIP.pasabahce;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

namespace PASABAHCE_URETIM_TAKIP
{
    public partial class HomePage : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            CacheUtils.sendNoCache(Response, Session);
            var errMsg = "";
            var ret = 0;

            #region checkWebConfig

            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }

            if (WebConfigUtils.checkWebConfig(Response, Session, ref webConfig) == false)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }

            #endregion

            #region checkIfLoggedIn
            errMsg = "";
            var ws = new PASABAHCE_WEB_SERVICE();
            var USER_INFO = new CLS_USER_INFO();
            ws.init_CLS_USER_INFO(ref USER_INFO);

            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, Session, Response, ref USER_INFO, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (USER_INFO.USER_NAME == "")
            {
                Response.Redirect("Account/LoginForm.aspx", true);
                return;
            }
            var USER_INFO_LIST = new CLS_USER_INFO();
            ret = ws.get_USER(Global.SYSTEM_CODE, USER_INFO.USER_ID, USER_INFO.SIFRE, ref USER_INFO_LIST, ref errMsg);
            int USER_TYPE = Convert.ToInt32(USER_INFO_LIST.USER_TYPE);
            Session["USER_TYPE"] = USER_TYPE;
            #endregion   
            
            #region taslak files
            errMsg = "";
            var html_taslak = "";
            //CMS++
            pasabahce.CLS_LANG_TEXT[] TEXT = null;
            ret = ws.getLangText(webConfig.SQLServerConnectionString, USER_INFO, USER_INFO.LANG, ref TEXT, ref errMsg);
            StringBuilder JS_LANG_TEXTS = new StringBuilder();
            for (int i = 0; i < TEXT.Length; i++)
            {
                JS_LANG_TEXTS.Append("LANG_TEXT['" + TEXT[i].LANG_TEXT_ID + "'] = \"" + TEXT[i].LANG_TEXT_CONTENT + "\";" + (char)13 + (char)10);
            }
            if (ret < 0)
            {
                Session["information_msg"] = errMsg;
                Response.Redirect("Information.aspx");
                return;
            }
            //CMS--
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/Layout.html", ref html_taslak, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html_menu_bar = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/MenuBar.html", ref html_menu_bar, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html_middle_area = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/FormLayout.html", ref html_middle_area,
                ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Home.html", ref html, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
 
            #endregion

            html_taslak = html_taslak.Replace("<!--[!USER_NAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            string sayfa_baslik="";
            if (USER_INFO.LANG == "TR")
            { sayfa_baslik = "Ana Sayfa"; }
            else if(USER_INFO.LANG=="EN")
            { sayfa_baslik = "Home Page"; }
            else if(USER_INFO.LANG=="BG")
            { sayfa_baslik = ""; }
            PageContentHelper.prepareMenuBar(USER_INFO, ref html_menu_bar);
            //CMS
            html_taslak = html_taslak.Replace("<!--[!JS_LANG_TEXTS!]-->", JS_LANG_TEXTS.ToString());
            html_taslak = html_taslak.Replace("<!--[!MENU_BAR!]-->", html_menu_bar);
            html_middle_area = html_middle_area.Replace("<!--[!PAGE_CONTENT!]-->", html);
            html_taslak = html_taslak.Replace("<!--[!MIDDLE_AREA!]-->", html_middle_area);
            html_taslak = html_taslak.Replace("<!--[!header_text!]-->", sayfa_baslik);
            sendHtml(USER_INFO, webConfig, USER_INFO.LANG, html_taslak);
        }

        [WebMethod(EnableSession = true)]
        public static int[] Menus()
        
      {
            var errMsg = "";
            var ret = 0;
            var ws = new PASABAHCE_WEB_SERVICE();
            var USER_INFO = new CLS_USER_INFO();
            ws.init_CLS_USER_INFO(ref USER_INFO);
            int[] PAGE_ID_CALL_LIST= null;
           int USER_TYPE= Convert.ToInt32(HttpContext.Current.Session["USER_TYPE"].ToString());
           ret = ws.get_PAGE_ID_CALL_LIST(Global.SYSTEM_CODE, USER_TYPE, ref PAGE_ID_CALL_LIST, ref errMsg);
            return PAGE_ID_CALL_LIST;
        }


        public void sendHtml(CLS_USER_INFO USER_INFO, WebConfig webConfig, string Lang, string html)
        {

            var errMsg = "";
            //CMS+++
            var ret = CMSUtils.getCMSHTML(USER_INFO, webConfig, Lang, ref html, ref errMsg);
            if (ret < 0)
            {
                sendError(ret, errMsg, errMsg);
                return;
            }
            //CMS---
            Response.Write(html);
        }

        public string getFormDataString(string s)
        {
            if (s == null)
            {
                return ("");
            }
            if (s == "")
            {
                return ("");
            }
            return (s);
        }

        public string getFormDataStringWithEliminateZero(string s)
        {
            var ss = getFormDataString(s);
            if (ss == "0")
            {
                return ("");
            }
            return (ss);
        }

        public void sendResponse(int status, string msg, string xmlResponse)
        {
            Response.Write("<gns><status>" + status + "</status><msg>" + msg + "</msg>" + xmlResponse + "</gns>");
        }

        public void sendError(int status, string msg, string detailed_msg)
        {
            Response.Write("<gns><status>" + status + "</status><msg>" + msg + "</msg><detailed_msg>" + detailed_msg +
                           "</detailed_msg></gns>");
        }

        public void sendSuccess()
        {
            Response.Write("<gns><status>0</status><msg>OK</msg><detailed_msg>OK</detailed_msg></gns>");
        }
    }
}
