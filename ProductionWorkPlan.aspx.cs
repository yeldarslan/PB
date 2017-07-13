using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using PASABAHCE_URETIM_TAKIP.pasabahce;
namespace PASABAHCE_URETIM_TAKIP.ProductionWorkPlan
{
    public partial class ProductionWorkPlanList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
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

            var systemCode = Global.SYSTEM_CODE;
            var ws = new PASABAHCE_WEB_SERVICE();

            #region checkIfLoggedIn

            errMsg = "";
            var userInfo = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, Session, Response, ref userInfo, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (userInfo.USER_ID == "")
            {
                Response.Redirect("~/Account/LoginForm.aspx", true);
                return;
            }

            int PAGE_ID;
            string PAGE_NAME = "ProductionWorkPlanList";
            pasabahce.CLS_PAGES[] PAGES_LIST = null;
            ret = ws.get_PAGE_ID(Global.SYSTEM_CODE, PAGE_NAME, ref PAGES_LIST, ref errMsg);

            PAGE_ID = PAGES_LIST[0].PAGE_ID;
            pasabahce.CLS_PERMISSION[] PERMISSION_LIST = null;
            ret = ws.get_PAGE_CALL(Global.SYSTEM_CODE, PAGE_ID, userInfo.USER_TYPE, ref PERMISSION_LIST, ref errMsg);

            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (PERMISSION_LIST[0].READ_PERMISSION == "N")
            {
                Response.Redirect("/Permission.aspx", true);
                return;
            }


            #endregion
            pasabahce.CLS_LANG_TEXT[] TEXT = null;
            ret = ws.getLangText(webConfig.SQLServerConnectionString, userInfo, userInfo.LANG, ref TEXT, ref errMsg);
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

            #region taslak files

            errMsg = "";
            var html_taslak = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/Layout.html", ref html_taslak, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html_taslak_menu_bar = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/MenuBar.html", ref html_taslak_menu_bar,
                ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html_taslak_middle_area = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Layout/FormLayout.html", ref html_taslak_middle_area,
                ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            errMsg = "";
            var html_taslak_content = "";
            ret = FileUtils.getFile(webConfig.WebRootPath, "Pages/Views/ProductionWorkPlan/ProductionWorkPlanList.html",
                ref html_taslak_content, ref errMsg);



            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;


            }

            #endregion

            html_taslak = html_taslak.Replace("<!--[!USER_NAME!]-->", userInfo.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            PageContentHelper.prepareMenuBar(userInfo, ref html_taslak_menu_bar);
            html_taslak = html_taslak.Replace("<!--[!MENU_BAR!]-->", html_taslak_menu_bar);
            html_taslak = html_taslak.Replace("<!--[!JS_LANG_TEXTS!]-->", JS_LANG_TEXTS.ToString());
            html_taslak_middle_area = html_taslak_middle_area.Replace("<!--[!PAGE_CONTENT!]-->", html_taslak_content);
            html_taslak = html_taslak.Replace("<!--[!MIDDLE_AREA!]-->", html_taslak_middle_area);
            html_taslak = html_taslak.Replace("<!--[!header_text!]-->", "Üretim İş Planı");
            sendHtml(userInfo, webConfig, userInfo.LANG, html_taslak);
        }

        public class KendoDataModel
        {
            public int Total { get; set; }
            public string Data { get; set; }
        }

        public void sendHtml(CLS_USER_INFO userInfo, WebConfig webConfig, string Lang, string html)
        {
            var errMsg = "";
            //CMS+++
            var ret = CMSUtils.getCMSHTML(userInfo, webConfig, Lang, ref html, ref errMsg);
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

        [WebMethod(EnableSession = true)]
        public static string RemoveProductionWorkPlan(string KALIP_NO)
        {
            string errMsg = "";

            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig

            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)
            {
                //MessageUtils.sendInformationMessage(Session, Response, errMsg);
                //return "";
                return ret.ToString() + " " + errMsg;
            }

            if (
                WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==
                false)
            {
                //MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return "";
            }

            #endregion

            #region checkIfLoggedIn

            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)
            {
                //MessageUtils.sendInformationMessage(Session, Response, errMsg);
                //return "";
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")
            {
                //Response.Redirect("~/Account/LoginForm.aspx", true);
                //return "";
                return ret.ToString() + " " + errMsg;
            }

            #endregion

            pasabahce.CLS_ZPP_URT_PLAN ZPP_URT_PLAN = new pasabahce.CLS_ZPP_URT_PLAN();
            ws.init_CLS_ZPP_URT_PLAN(ref ZPP_URT_PLAN);
            ZPP_URT_PLAN.KALIP_NO = KALIP_NO;
            if (ret < 0)
            {
                return "-1";
            }
            return "0";
        }


        [WebMethod(EnableSession = true)]
        public static string GetProductionWorkPlanList(string filt1, string filt2, string filt3)
        {
            var errMsg = "";
            var ret = 0;
            var systemCode = Global.SYSTEM_CODE;
            var ws = new PASABAHCE_WEB_SERVICE();

            #region checkWebConfig

            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(HttpContext.Current.Session, HttpContext.Current.Response, errMsg);
                return "";
            }

            if (
                WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==
                false)
            {
                MessageUtils.sendInformationMessage(HttpContext.Current.Session, HttpContext.Current.Response, errMsg);
                return "";
            }

            #endregion

            #region checkIfLoggedIn

            errMsg = "";
            var userInfo = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref userInfo, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(HttpContext.Current.Session, HttpContext.Current.Response, errMsg);
                return "";
            }
            if (userInfo.USER_ID == "")
            {
                HttpContext.Current.Response.Redirect("~/Account/LoginForm.aspx", true);
                return "";
            }

            #endregion

            pasabahce.CLS_ZPP_URT_PLAN_FILTER prLineFilter = new pasabahce.CLS_ZPP_URT_PLAN_FILTER();
            ws.init_CLS_ZPP_URT_PLAN_FILTER(ref prLineFilter);
            if (filt1 != "")
            {
                prLineFilter.KALIP_NO = filt1;
            }
            if (filt2 != "")
            {
                prLineFilter.URETIM_TIPI = Int32.Parse(filt2);
            }
            if (filt3 != "")
            {
                prLineFilter.URET_BAS_TAR = filt3;
            }
            pasabahce.CLS_ZPP_URT_PLAN_SORT prLineSort = new pasabahce.CLS_ZPP_URT_PLAN_SORT();
            ws.init_CLS_ZPP_URT_PLAN_SORT(ref prLineSort);
            CLS_ZPP_URT_PLAN[] prHatList = null;
            userInfo.MANDT = "400";
            ret = ws.GET_ZPP_URT_PLAN_LIST(systemCode, userInfo, Global.Max_Row_Count, prLineFilter, prLineSort, ref prHatList, ref errMsg);
            if (ret < 0)
            {
                MessageUtils.sendInformationMessage(HttpContext.Current.Session, HttpContext.Current.Response, errMsg);
                return "";
            }
            if (filt1 != "")
            {
                var temp = prHatList.Where(c => c.KALIP_NO == filt1);
                prHatList = temp.ToArray();
            }
            var json_output = JsonConvert.SerializeObject(prHatList);
            var data = new KendoDataModel();
            data.Data = json_output;
            data.Total = prHatList.Count();
            var jsonData = JsonConvert.SerializeObject(data);
            return (jsonData);
        }
    }
}

