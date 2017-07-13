using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using PASABAHCE_URETIM_TAKIP.pasabahce;
using System.IO;
using System.Data;
using System.Collections;

namespace PASABAHCE_URETIM_TAKIP.ProductionReport
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)   {
            CacheUtils.sendNoCache(Response, Session);
            var errMsg = "";
            var ret = 0;
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (WebConfigUtils.checkWebConfig(Response, Session, ref webConfig) == false) {
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
            if (ret < 0){
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (userInfo.USER_ID == "") {
                Response.Redirect("~/Account/LoginForm.aspx", true);
                return;
            }
            int PAGE_ID;
            string PAGE_NAME = "Reports";
            pasabahce.CLS_PAGES[] PAGES_LIST = null;
            ret = ws.get_PAGE_ID(Global.SYSTEM_CODE, PAGE_NAME, ref PAGES_LIST, ref errMsg);
            PAGE_ID = PAGES_LIST[0].PAGE_ID;
            pasabahce.CLS_PERMISSION[] PERMISSION_LIST = null;
            ret = ws.get_PAGE_CALL(Global.SYSTEM_CODE, PAGE_ID, userInfo.USER_TYPE, ref PERMISSION_LIST, ref errMsg);
            if (ret < 0)  {
                MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return;
            }
            if (PERMISSION_LIST[0].READ_PERMISSION == "N") {
                Response.Redirect("/Permission.aspx", true);
                return;
            }
            #endregion
            string BAS_TARIH = Request["tmp_BAS_TARIH"];
            string BIT_TARIH = Request["tmp_BIT_TARIH"];
            string MAKINA_KOD1 = Request["tmp_MAKINA_KOD1"];
            string MAKINA_KOD2 = Request["tmp_MAKINA_KOD2"];
            string HAT_TURU = Request["tmp_HAT_TURU"];
            string RAPOR_TUR = Request["tmp_RAPOR_TUR"];
            string VK_BAS_TARIH = Request["vk_BAS_TARIH"];
            string VK_BIT_TARIH = Request["vk_BIT_TARIH"];
            string VK_HAT_KOD1 = Request["vk_HAT_KOD1"];
            string VK_HAT_KOD2 = Request["vk_HAT_KOD2"];
            string VK_HAT_TURU = Request["vk_HAT_TURU"];
            string VK_RAPOR_TUR = Request["vk_RAPOR_TUR"];
            string VK_KALIP_NO = Request["vk_KALIP_NO"];
            string CMC_BAS_TARIH = Request["cmc_BAS_TARIH"];
            string CMC_BIT_TARIH = Request["cmc_BIT_TARIH"];
            string CMC_HAT_KOD1 = Request["cmc_HAT_KOD1"];
            string CMC_HAT_TURU = Request["cmc_HAT_TURU"];
            string CMC_RAPOR_TUR = Request["cmc_RAPOR_TUR"];
            string CMC_KALIP_NO = Request["cmc_KALIP_NO"];
            string PP_BAS_TARIH = Request["pp_BAS_TARIH"];
            string PP_BIT_TARIH = Request["pp_BIT_TARIH"];
            string PP_RAPOR_TUR = Request["pp_RAPOR_TUR"];
            string GK_BAS_TARIH = Request["gk_BAS_TARIH"];
            string GK_BIT_TARIH = Request["gk_BIT_TARIH"];
            string GK_HAT_KOD1 = Request["gk_HAT_KOD1"];
            string GK_RAPOR_TUR = Request["gk_RAPOR_TUR"];
            string MST_BAS_TARIH = Request["mst_BAS_TARIH"];
            string MST_BIT_TARIH = Request["mst_BIT_TARIH"];
            string MST_HAT_KOD1 = Request["mst_HAT_KOD1"];
            string MST_HAT_TURU = Request["mst_HAT_TURU"];
            string MST_RAPOR_TUR = Request["mst_RAPOR_TUR"];
            string DB_BAS_TARIH = Request["db_BAS_TARIH"];
            string DB_BIT_TARIH = Request["db_BIT_TARIH"];
            string DB_HAT_KOD1 = Request["db_HAT_KOD1"];
            string DB_HAT_TURU = Request["db_HAT_TURU"];
            string DB_RAPOR_TUR = Request["db_RAPOR_TUR"];
            string DB_ANA_DURUS = Request["db_ANA_DURUS"];
            string DP_BAS_TARIH = Request["dp_BAS_TARIH"];
            string DP_BIT_TARIH = Request["dp_BIT_TARIH"];
            string DP_HAT_KOD1 = Request["dp_HAT_KOD1"];
            string DP_HAT_TURU = Request["dp_HAT_TURU"];
            string DP_RAPOR_TUR = Request["dp_RAPOR_TUR"];
            string DP_KALIP_NO = Request["dp_KALIP_NO"];
            string kk_BAS_TARIH = Request["kk_BAS_TARIH"];
            string kk_BIT_TARIH = Request["kk_BIT_TARIH"];
            string kk_HAT_TURU = Request["kk_HAT_TURU"];
            string kk_MAKINA_KOD1 = Request["kk_MAKINA_KOD1"];
            string kk_KALIP_NO = Request["kk_KALIP_NO"];
            string kk_RAPOR_TUR = Request["kk_RAPOR_TUR"];
            string dc_ITEM_NO = Request["dc_ITEM_NO"];
            string dc_ITEM_TANIM = Request["dc_ITEM_TANIM"];
            string dc_SIPARIS_NO = Request["dc_SIPARIS_N0"];
            string dc_IHT_BAS_TAR = Request["dc_IHT_BAS_TAR"];
            string dc_IHT_BIT_TAR = Request["dc_IHT_BIT_TAR"];
            string dc_PRO_BAS_TAR = Request["dc_PRO_BAS_TAR"];
            string dc_PRO_BIT_TAR = Request["dc_PRO_BIT_TAR"];
            string dc_SEVK_BAS_TAR = Request["dc_SEVK_BAS_TAR"];
            string dc_SEVK_BIT_TAR = Request["dc_SEVK_BIT_TAR"];
            string dc_RAPOR_TUR = Request["dc_RAPOR_TUR"];
            if (RAPOR_TUR == "1")  {
                string retS = UretimCreate(BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU);
            }
            else if (RAPOR_TUR == "2")  {
                string retS = BekleyenRedlerCreate(BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU);
            }
            else if (RAPOR_TUR == "3") {
                string retS = YenidenAyirmalarCreate(BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2);
            }
            else if (RAPOR_TUR == "4")  {
                string retS = PaletSayisiCreate(BAS_TARIH, BIT_TARIH, MAKINA_KOD1);
            }
            else if (RAPOR_TUR == "5") {
                string retS = UrunGrubuBazindaCreate(BAS_TARIH, BIT_TARIH, HAT_TURU);
            }
            else if (RAPOR_TUR == "6")  {
                string retS = HatBazindaUretimlerSumCreate(BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU);
            }
            else if (RAPOR_TUR == "7"){
                string retS = UrunGrubuCreate(BAS_TARIH, BIT_TARIH, HAT_TURU);
            }
            if (VK_RAPOR_TUR == "1")  {
                string retS = HatKalipItemSipVardiyaBazindaVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            else if (VK_RAPOR_TUR == "2")   {
                string retS = HatKalipVardiyaBazindaVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            else if (VK_RAPOR_TUR == "3") {
                string retS = HatKalipItemSipBazindaVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            else if (VK_RAPOR_TUR == "4")  {
                string retS = OkeyIlaveVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            else if (VK_RAPOR_TUR == "5")   {
                string retS = OkeyRedVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            else if (VK_RAPOR_TUR == "6")  {
                string retS = YenidenAyirmaVK(VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_HAT_TURU, VK_KALIP_NO);
            }
            if (CMC_RAPOR_TUR == "1")  {
                string retS = KampanyaKalipDegisimiCMC(CMC_BAS_TARIH, CMC_BIT_TARIH, CMC_HAT_KOD1, CMC_HAT_TURU, CMC_KALIP_NO);
            }
            if (PP_RAPOR_TUR == "1") {
                string retS = AlinmayanPaletlerUretimPP(PP_BAS_TARIH, PP_BIT_TARIH);
            }
            if (PP_RAPOR_TUR == "2") {
                string retS = AlinmayanPaletlerFabrikaPP(PP_BAS_TARIH, PP_BIT_TARIH);
            }
            if (PP_RAPOR_TUR == "3") {
                string retS = AlinmayanPaletlerTumuPP(PP_BAS_TARIH, PP_BIT_TARIH);
            }
            if (GK_RAPOR_TUR == "1"){
                string retS = GunlukKaliteGK(GK_BAS_TARIH, GK_BIT_TARIH, GK_HAT_KOD1);
            }
            if (MST_RAPOR_TUR == "1")  {
                string retS = MakinaDurusMST(MST_BAS_TARIH, MST_BIT_TARIH, MST_HAT_KOD1, MST_HAT_TURU);
            }
            if (DB_RAPOR_TUR == "1"){
                string retS = DepMakinaDurusDB(DB_BAS_TARIH, DB_BIT_TARIH, DB_HAT_KOD1, DB_HAT_TURU, DB_ANA_DURUS);
            }
            if (DP_RAPOR_TUR == "1") {
                string retS = ImalatDP(DP_BAS_TARIH, DP_BIT_TARIH, DP_HAT_KOD1, DP_HAT_TURU, DP_KALIP_NO);
            }
            if (kk_RAPOR_TUR == "1") {
                string retS = KalipDegisim(kk_BAS_TARIH, kk_BIT_TARIH, kk_HAT_TURU, kk_MAKINA_KOD1, kk_KALIP_NO);
            }
            if (dc_RAPOR_TUR == "1"){
                string retS = DcSipAmb(dc_ITEM_NO, dc_ITEM_TANIM, dc_SIPARIS_NO, dc_IHT_BAS_TAR, dc_IHT_BIT_TAR, dc_PRO_BAS_TAR, dc_PRO_BIT_TAR, dc_SEVK_BAS_TAR, dc_SEVK_BIT_TAR);
            }
        }
        [WebMethod(EnableSession = true)]
        public static string UretimCreate(string BAS_TARIH, string BIT_TARIH, string MAKINA_KOD1, string MAKINA_KOD2, string HAT_TURU) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==
                false)  {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == ""){
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string URETIM_SIP_NO;
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\taslak_hat_bazinda_uretim2.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\taslak_hat_bazinda_uretim_row.xml";
            try    {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatBazindaUretimler_ROW> Report_HatBazindaUretimler_ROWS = new List<CLS_Report_HatBazindaUretimler_ROW>();
            var tmpRows = Report_HatBazindaUretimler_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_HAT_BAZINDA_URETIMLER_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++)     {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatBazindaUretimler_ROW row = tmpRows[i];
                URETIM_SIP_NO = row.URETIM_SIP_NO;
                html_table_row = html_table_row.Replace("[!HAT!]", row.HAT);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_TIP!]", row.PALET_TIP);
                html_table_row = html_table_row.Replace("[!TANIM!]", row.TANIM);
                html_table_row = html_table_row.Replace("[!URETIM_SIP_NO!]", row.URETIM_SIP_NO);
                html_table_row = html_table_row.Replace("[!PLAN_MIKTAR!]", row.PLAN_MIKTAR);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!TANIM_1!]", row.TANIM_1);
                html_table_row = html_table_row.Replace("[!OKEY!]", row.OKEY);
                html_table_row = html_table_row.Replace("[!ILAVE!]", row.ILAVE);
                html_table_row = html_table_row.Replace("[!TOPLAM!]", row.TOPLAM);
                html_table_row = html_table_row.Replace("[!RED!]", row.RED);
                html_table_row = html_table_row.Replace("[!BEKLEYEN!]", row.BEKLEYEN);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_Üretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string BekleyenRedlerCreate(string BAS_TARIH, string BIT_TARIH, string MAKINA_KOD1, string MAKINA_KOD2, string HAT_TURU)  {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false)   {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\taslak_bekleyen_redler.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\taslak_bekleyen_redler_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_BekleyenRedler_ROW> Report_BekleyenRedler_ROWS = new List<CLS_Report_BekleyenRedler_ROW>();
            var tmpRows = Report_BekleyenRedler_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_BEKLEYEN_REDLER_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_BekleyenRedler_ROW row = tmpRows[i];
                degerler = row.ISLEM_TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].ToString();
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!SIRKET_NO!]", row.SIRKET_NO);
                html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", row.ISLETME_KOD);
                html_table_row = html_table_row.Replace("[!HAT_KOD!]", row.HAT_KOD);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row.MAKINA_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!TANIM!]", row.TANIM);
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO);
                html_table_row = html_table_row.Replace("[!ADET!]", row.ADET);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_BekleyenRedler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string YenidenAyirmalarCreate(string BAS_TARIH, string BIT_TARIH, string MAKINA_KOD1, string MAKINA_KOD2)  {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false){
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
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")  {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\yeniden_ayirmalar.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\yeniden_ayirmalar_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_YenidenAyirmalar_ROW> Report_YenidenAyirmalar_ROWS = new List<CLS_Report_YenidenAyirmalar_ROW>();
            var tmpRows = Report_YenidenAyirmalar_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_YENIDEN_AYIRMALAR_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_YenidenAyirmalar_ROW row = tmpRows[i];
                degerler = row.ISLEM_TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].ToString();
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                html_table_row = html_table_row.Replace("[!SIRKET_NO!]", row.SIRKET_NO);
                html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", row.ISLETME_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO);
                html_table_row = html_table_row.Replace("[!ADET!]", row.ADET);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_YenidenAyirmalar.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string PaletSayisiCreate(string BAS_TARIH, string BIT_TARIH, string MAKINA_KOD1) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false)  {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")    {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\taslak_palet_sayisi.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\taslak_palet_sayisi_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)    {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_PaletSayisi_ROW> Report_PaletSayisi_ROWS = new List<CLS_Report_PaletSayisi_ROW>();
            var tmpRows = Report_PaletSayisi_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_PALET_SAYISI_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, MAKINA_KOD1, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_PaletSayisi_ROW row = tmpRows[i];
                degerler = row.ISLEM_TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].ToString();
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                html_table_row = html_table_row.Replace("[!SIRKET_NO!]", row.SIRKET_NO);
                html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", row.ISLETME_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO);
                html_table_row = html_table_row.Replace("[!ILAVE_ADET!]", row.ILAVE_ADET);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_PaletSayisi.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string UrunGrubuBazindaCreate(string BAS_TARIH, string BIT_TARIH, string HAT_TURU)   {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false)    {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")   {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\urun_grubu_bazinda_kalip_urun.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\urun_grubu_bazinda_kalip_urun_row.xml";
            try   {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_UrunGrubuBazindaUretimler_ROW> Report_UrunGrubuBazindaUretimler_ROWS = new List<CLS_Report_UrunGrubuBazindaUretimler_ROW>();
            var tmpRows = Report_UrunGrubuBazindaUretimler_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_URUN_GRUBU_BAZINDA_URETIMLER_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)
            {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++)  {
                string html_table_row = html_taslak_table_row;
                CLS_Report_UrunGrubuBazindaUretimler_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HIYERARSI!]", row.HIYERARSI);
                html_table_row = html_table_row.Replace("[!TANIM!]", row.TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!TANIM_1!]", row.TANIM_1);
                html_table_row = html_table_row.Replace("[!OKEY!]", row.OKEY);
                html_table_row = html_table_row.Replace("[!ILAVE!]", row.ILAVE);
                html_table_row = html_table_row.Replace("[!RED!]", row.RED);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_UrunGrubuBazindaUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string HatBazindaUretimlerSumCreate(string BAS_TARIH, string BIT_TARIH, string MAKINA_KOD1, string MAKINA_KOD2, string HAT_TURU) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==
                false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")  {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_bazinda_uretimler_sum.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_bazinda_uretimler_sum_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatBazindaUretimlerSum_ROW> Report_HatBazindaUretimlerSum_ROWS = new List<CLS_Report_HatBazindaUretimlerSum_ROW>();
            var tmpRows = Report_HatBazindaUretimlerSum_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_HAT_BAZINDA_URETIMLER_SUM_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, MAKINA_KOD1, MAKINA_KOD2, HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatBazindaUretimlerSum_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT!]", row.HAT);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!OKEY!]", row.OKEY);
                html_table_row = html_table_row.Replace("[!ILAVE!]", row.ILAVE);
                html_table_row = html_table_row.Replace("[!TOPLAM!]", row.TOPLAM);
                html_table_row = html_table_row.Replace("[!RED!]", row.RED);
                html_table_row = html_table_row.Replace("[!BEKLEYEN!]", row.BEKLEYEN);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatBazindaUretimlerSum.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string UrunGrubuCreate(string BAS_TARIH, string BIT_TARIH, string HAT_TURU){
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")  {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\urun_grubu.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\urun_grubu_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)    {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_UrunGrubuBazındaUretimlerSum_ROW> Report_UrunGrubuBazındaUretimlerSum_ROWS = new List<CLS_Report_UrunGrubuBazındaUretimlerSum_ROW>();
            var tmpRows = Report_UrunGrubuBazındaUretimlerSum_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_URUN_GRUBU_BAZINDA_URETIMLER_SUM_LIST(SYSTEM_CODE, USER_INFO, BAS_TARIH, BIT_TARIH, HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)    {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_UrunGrubuBazındaUretimlerSum_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT!]", row.HIYERARSI);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.TANIM);
                html_table_row = html_table_row.Replace("[!OKEY!]", row.OKEY);
                html_table_row = html_table_row.Replace("[!ILAVE!]", row.ILAVE);
                html_table_row = html_table_row.Replace("[!TOPLAM!]", row.TOPLAM);
                html_table_row = html_table_row.Replace("[!RED!]", row.RED);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_UrunGrubuBazindaUretimlerSum.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string HatKalipItemSipVardiyaBazindaVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_kalip_item_sip_vard_bazinda_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_kalip_item_sip_vard_bazinda_vk_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatKalipItemSipVardiyaBazindaVK_ROW> Report_HatKalipItemSipVardiyaBazindaVK_ROWS = new List<CLS_Report_HatKalipItemSipVardiyaBazindaVK_ROW>();
            var tmpRows = Report_HatKalipItemSipVardiyaBazindaVK_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_HatKalipItemSipVardiyaBazindaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatKalipItemSipVardiyaBazindaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", row.ISLEM_TARIH);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!OKEY_1!]", row.OKEY_1);
                html_table_row = html_table_row.Replace("[!RED_1!]", row.RED_1);
                html_table_row = html_table_row.Replace("[!ILAVE_1!]", row.ILAVE_1);
                html_table_row = html_table_row.Replace("[!OKEY_2!]", row.OKEY_2);
                html_table_row = html_table_row.Replace("[!RED_2!]", row.RED_2);
                html_table_row = html_table_row.Replace("[!ILAVE_2!]", row.ILAVE_2);
                html_table_row = html_table_row.Replace("[!OKEY_3!]", row.OKEY_3);
                html_table_row = html_table_row.Replace("[!RED_3!]", row.RED_3);
                html_table_row = html_table_row.Replace("[!ILAVE_3!]", row.ILAVE_3);
                html_table_row = html_table_row.Replace("[!TOP_OKEY!]", row.TOP_OKEY);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_row = html_table_row.Replace("[!TOP_ILAVE!]", row.TOP_ILAVE);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatKalipItemVardiyaBazindaUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string HatKalipVardiyaBazindaVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO){
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false){
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_kalip_vard_bazinda_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_kalip_vard_bazinda_vk_row.xml";
            try   {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatKalipVardiyaBazindaVK_ROW> Report_HatKalipVardiyaBazindaVK_ROWS = new List<CLS_Report_HatKalipVardiyaBazindaVK_ROW>();
            var tmpRows = Report_HatKalipVardiyaBazindaVK_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_HatKalipVardiyaBazindaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatKalipVardiyaBazindaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!HAT_KOD!]", row.HAT_KOD);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!OKEY1!]", row.OKEY1);
                html_table_row = html_table_row.Replace("[!RED1!]", row.RED1);
                html_table_row = html_table_row.Replace("[!OKEY2!]", row.OKEY2);
                html_table_row = html_table_row.Replace("[!RED2!]", row.RED2);
                html_table_row = html_table_row.Replace("[!OKEY3!]", row.OKEY3);
                html_table_row = html_table_row.Replace("[!RED3!]", row.RED3);
                html_table_row = html_table_row.Replace("[!TOP_OKEY!]", row.TOP_OKEY);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatKalipVardiyaBazindaUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string HatKalipItemSipBazindaVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == ""){
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_kalip_item_sip_bazinda_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_kalip_item_sip_bazinda_vk_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)   {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatKalipItemBazindaVK_ROW> Report_HatKalipItemBazindaVK_ROWS = new List<CLS_Report_HatKalipItemBazindaVK_ROW>();
            var tmpRows = Report_HatKalipItemBazindaVK_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_HatKalipItemBazindaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatKalipItemBazindaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!ITEM_TANIM!]", row.ITEM_TANIM);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!TOP_OKEY!]", row.TOP_OKEY);
                html_table_row = html_table_row.Replace("[!TOP_ILAVE!]", row.TOP_ILAVE);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_row = html_table_row.Replace("[!BEKRED!]", row.BEKRED);
                html_table_row = html_table_row.Replace("[!TOP!]", row.TOP);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatKalipItemSipBazindaUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string OkeyIlaveVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO)    {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false)  {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_kalip_bazinda_okey_ilave_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_kalip_bazinda_okey_ilave_vk_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)   {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatKalipOkeyIlaveBazindaVK_ROW> Report_HatKalipOkeyIlaveBazindaVK_ROWS = new List<CLS_Report_HatKalipOkeyIlaveBazindaVK_ROW>();
            var tmpRows = Report_HatKalipOkeyIlaveBazindaVK_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_HatKalipOkeyIlaveBazindaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatKalipOkeyIlaveBazindaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TOP_OKEY!]", row.TOP_OKEY);
                html_table_row = html_table_row.Replace("[!TOP_ILAVE!]", row.TOP_ILAVE);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_row = html_table_row.Replace("[!BEKRED!]", row.BEKRED);
                html_table_row = html_table_row.Replace("[!AGIRLIK!]", row.AGIRLIK);
                html_table_row = html_table_row.Replace("[!NET_TON!]", row.NET_TON);
                html_table_row = html_table_row.Replace("[!TOP!]", row.TOP);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatKalipBazindaOkeyIlaveUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string OkeyRedVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO) {   var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\hat_kalip_bazinda_okey_red_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\hat_kalip_bazinda_okey_red_vk_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_HatKalipOkeyRedBazindaVK_ROW> Report_HatKalipOkeyRedBazindaVK_ROWS = new List<CLS_Report_HatKalipOkeyRedBazindaVK_ROW>();
            var tmpRows = Report_HatKalipOkeyRedBazindaVK_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_HatKalipOkeyRedBazindaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_HatKalipOkeyRedBazindaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TOP_OKEY!]", row.TOP_OKEY);
                html_table_row = html_table_row.Replace("[!TOP_ILAVE!]", row.TOP_ILAVE);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_row = html_table_row.Replace("[!BEKRED!]", row.BEKRED);
                html_table_row = html_table_row.Replace("[!TOP!]", row.TOP);
                html_table_row = html_table_row.Replace("[!RED_YUZDE!]", row.TOP);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_HatKalipBazindaOkeyRedUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string YenidenAyirmaVK(string VK_BAS_TARIH, string VK_BIT_TARIH, string VK_HAT_KOD1, string VK_HAT_KOD2, string VK_HAT_TURU, string VK_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false) {
                return "";  }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)     {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\yeniden_ayirma_vk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\yeniden_ayirma_vk_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_YenidenAyirmaVK_ROW> Report_YenidenAyirmaVK_ROWS = new List<CLS_Report_YenidenAyirmaVK_ROW>();
            var tmpRows = Report_YenidenAyirmaVK_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_YenidenAyirmaVK_LIST(SYSTEM_CODE, USER_INFO, VK_BAS_TARIH, VK_BIT_TARIH, VK_HAT_KOD1, VK_HAT_KOD2, VK_KALIP_NO, VK_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_YenidenAyirmaVK_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", row.ISLEM_TARIH);
                html_table_row = html_table_row.Replace("[!TOP_ILAVE!]", row.TOP_ILAVE);
                html_table_row = html_table_row.Replace("[!TOP_RED!]", row.TOP_RED);
                html_table_row = html_table_row.Replace("[!BEKRED!]", row.BEKRED);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", VK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", VK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_YenidenAyirmaUretimler.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string KampanyaKalipDegisimiCMC(string CMC_BAS_TARIH, string CMC_BIT_TARIH, string CMC_HAT_KOD1, string CMC_HAT_TURU, string CMC_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false)   {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")    {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\kampanya_kalip_degisim_cmc.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\kampanya_kalip_degisim_cmc_row.xml";
            try   {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_KampanyaKalipDegisim_ROW> Report_KampanyaKalipDegisim_ROWS = new List<CLS_Report_KampanyaKalipDegisim_ROW>();
            var tmpRows = Report_KampanyaKalipDegisim_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_KampanyaKalipDegisim_LIST(SYSTEM_CODE, USER_INFO, CMC_BAS_TARIH, CMC_BIT_TARIH, CMC_HAT_KOD1, CMC_KALIP_NO, CMC_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_KampanyaKalipDegisim_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!SIRKET_NO!]", row.SIRKET_NO.ToString());
                html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", row.ISLETME_KOD);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!HAT_KOD!]", row.HAT_KOD);
                html_table_row = html_table_row.Replace("[!BAS_TARIH!]", row.BAS_TARIH);
                html_table_row = html_table_row.Replace("[!BIT_TARIH!]", row.BIT_TARIH);
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!MAKINA_NO!]", row.MAKINA_NO);
                html_table_row = html_table_row.Replace("[!CEKIS_SURE!]", row.CEKIS_SURE.ToString());
                html_table_row = html_table_row.Replace("[!DURUS!]", row.DURUS.ToString());
                html_table_row = html_table_row.Replace("[!DEVIR!]", row.DEVIR.ToString());
                html_table_row = html_table_row.Replace("[!LEGAL_DURUS!]", row.LEGAL_DURUS.ToString());
                html_table_row = html_table_row.Replace("[!OKEY!]", row.OKEY.ToString());
                html_table_row = html_table_row.Replace("[!RED!]", row.RED.ToString());
                html_table_row = html_table_row.Replace("[!ILAVE!]", row.ILAVE.ToString());
                html_table_row = html_table_row.Replace("[!STANDART_ADET!]", row.STANDART_ADET.ToString());
                html_table_row = html_table_row.Replace("[!HEDEF_ADET!]", row.HEDEF_ADET.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_GRAM!]", row.BRUT_GRAM.ToString());
                html_table_row = html_table_row.Replace("[!NET_GRAM!]", row.NET_GRAM.ToString());
                html_table_row = html_table_row.Replace("[!CALISMA_ORANI!]", row.CALISMA_ORANI.ToString());
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", CMC_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", CMC_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_KampanyaKalipDegisim.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string AlinmayanPaletlerUretimPP(string PP_BAS_TARIH, string PP_BIT_TARIH) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")  {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\alinmayan_paletler_uretim_pp.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\alinmayan_paletler_uretim_pp_row.xml";
            try   {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_AlinmayanPaletlerUretim_ROW> Report_AlinmayanPaletlerUretim_ROWS = new List<CLS_Report_AlinmayanPaletlerUretim_ROW>();
            var tmpRows = Report_AlinmayanPaletlerUretim_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_AlinmayanPaletlerUretim_LIST(SYSTEM_CODE, USER_INFO, PP_BAS_TARIH, PP_BIT_TARIH, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string[] degerler2;
            string[] saat_sil2;
            string gun2, ay2, yil2, tarih2;
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++)  {
                string html_table_row = html_taslak_table_row;
                CLS_Report_AlinmayanPaletlerUretim_ROW row = tmpRows[i];
                degerler = row.TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].Substring(0, 4);
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                degerler2 = row.ISLEM_TARIH.Split('/');
                ay2 = degerler2[0].ToString();
                gun2 = degerler2[1].ToString();
                yil2 = degerler2[2].Substring(0, 4);
                tarih2 = gun2 + "." + ay2 + "." + yil2;
                saat_sil2 = tarih2.Split(' ');
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row.MAKINA_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_TIP!]", row.PALET_TIP.ToString());
                html_table_row = html_table_row.Replace("[!PALET_ADET!]", row.PALET_ADET.ToString());
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO.ToString());
                html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", row.MAKINA_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_ADET!]", row.ISLEM_ADET.ToString());
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil2[0]);
                html_table_row = html_table_row.Replace("[!ISLEM_TUR!]", row.ISLEM_TUR.ToString());
                html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", row.VARDIYA_NO.ToString());
                html_table_row = html_table_row.Replace("[!URETIM_ADET!]", row.URETIM_ADET);
                html_table_row = html_table_row.Replace("[!PARTI!]", row.PARTI);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", PP_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", PP_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_AlinmayanPaletlerUretim.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string AlinmayanPaletlerFabrikaPP(string PP_BAS_TARIH, string PP_BIT_TARIH)   {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==
                false)   {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
               ref USER_INFO, ref errMsg);
            if (ret < 0)   {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "")  {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\alinmayan_paletler_fabrika_pp.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\alinmayan_paletler_fabrika_pp_row.xml";
            try     {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)  {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_AlinmayanPaletlerFabrikaIci_ROW> Report_AlinmayanPaletlerFabrikaIci_ROWS = new List<CLS_Report_AlinmayanPaletlerFabrikaIci_ROW>();
            var tmpRows = Report_AlinmayanPaletlerFabrikaIci_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_AlinmayanPaletlerFabikaIci_LIST(SYSTEM_CODE, USER_INFO, PP_BAS_TARIH, PP_BIT_TARIH, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string[] degerler2;
            string[] saat_sil2;
            string gun2, ay2, yil2, tarih2;
            string html = html_taslak;
            string html_table_rows = "";
            for (int i = 0; i < tmpRows.Length; i++)   {
                string html_table_row = html_taslak_table_row;
                CLS_Report_AlinmayanPaletlerFabrikaIci_ROW row = tmpRows[i];
                degerler = row.TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].Substring(0, 4);
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                degerler2 = row.ISLEM_TARIH.Split('/');
                ay2 = degerler2[0].ToString();
                gun2 = degerler2[1].ToString();
                yil2 = degerler2[2].Substring(0, 4);
                tarih2 = gun2 + "." + ay2 + "." + yil2;
                saat_sil2 = tarih2.Split(' ');
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row.MAKINA_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_TIP!]", row.PALET_TIP.ToString());
                html_table_row = html_table_row.Replace("[!PALET_ADET!]", row.PALET_ADET.ToString());
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO.ToString());
                html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", row.MAKINA_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_ADET!]", row.ISLEM_ADET.ToString());
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil2[0]);
                html_table_row = html_table_row.Replace("[!ISLEM_TUR!]", row.ISLEM_TUR.ToString());
                html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", row.VARDIYA_NO.ToString());
                html_table_row = html_table_row.Replace("[!URETIM_ADET!]", row.URETIM_ADET);
                html_table_row = html_table_row.Replace("[!PARTI!]", row.PARTI);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", PP_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", PP_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_AlinmayanPaletlerFabrika.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string AlinmayanPaletlerTumuPP(string PP_BAS_TARIH, string PP_BIT_TARIH)   {
            var errMsg = "";
            var ret = 0;
            var ret2 = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==  false)   {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            ret2 = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
               ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\alinmayan_paletler_pp.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\alinmayan_paletler_pp_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex){
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_AlinmayanPaletlerUretim_ROW> Report_AlinmayanPaletlerUretim_ROWS = new List<CLS_Report_AlinmayanPaletlerUretim_ROW>();
            var tmpRows = Report_AlinmayanPaletlerUretim_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_AlinmayanPaletlerUretim_LIST(SYSTEM_CODE, USER_INFO, PP_BAS_TARIH, PP_BIT_TARIH, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            List<CLS_Report_AlinmayanPaletlerFabrikaIci_ROW> Report_AlinmayanPaletlerFabrikaIci_ROWS = new List<CLS_Report_AlinmayanPaletlerFabrikaIci_ROW>();
            var tmpRows2 = Report_AlinmayanPaletlerFabrikaIci_ROWS.ToArray();
            ret2 = ws.get_REPORT_CLS_Report_AlinmayanPaletlerFabikaIci_LIST(SYSTEM_CODE, USER_INFO, PP_BAS_TARIH, PP_BIT_TARIH, Global.Max_Row_Count, ref tmpRows2, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string[] degerler2;
            string[] saat_sil2;
            string gun2, ay2, yil2, tarih2;
            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_AlinmayanPaletlerUretim_ROW row = tmpRows[i];
                degerler = row.TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].Substring(0, 4);
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                degerler2 = row.ISLEM_TARIH.Split('/');
                ay2 = degerler2[0].ToString();
                gun2 = degerler2[1].ToString();
                yil2 = degerler2[2].Substring(0, 4);
                tarih2 = gun2 + "." + ay2 + "." + yil2;
                saat_sil2 = tarih2.Split(' ');
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row.HAT_NO);
                html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row.MAKINA_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_TIP!]", row.PALET_TIP.ToString());
                html_table_row = html_table_row.Replace("[!PALET_ADET!]", row.PALET_ADET.ToString());
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row.PALET_NO.ToString());
                html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", row.MAKINA_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_ADET!]", row.ISLEM_ADET.ToString());
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", saat_sil2[0]);
                html_table_row = html_table_row.Replace("[!ISLEM_TUR!]", row.ISLEM_TUR.ToString());
                html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", row.VARDIYA_NO.ToString());
                html_table_row = html_table_row.Replace("[!URETIM_ADET!]", row.URETIM_ADET);
                html_table_row = html_table_row.Replace("[!PARTI!]", row.PARTI);
                html_table_rows += html_table_row;
            }
            for (int i = 0; i < tmpRows2.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_AlinmayanPaletlerFabrikaIci_ROW row2 = tmpRows2[i];
                html_table_row = html_table_row.Replace("[!HAT_NO!]", row2.HAT_NO);
                html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row2.MAKINA_TANIM);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row2.KALIP_NO);
                html_table_row = html_table_row.Replace("[!TARIH!]", row2.TARIH);
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row2.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!SIPARIS_SIRA_NO!]", row2.SIPARIS_SIRA_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row2.ITEM_NO);
                html_table_row = html_table_row.Replace("[!PALET_TIP!]", row2.PALET_TIP.ToString());
                html_table_row = html_table_row.Replace("[!PALET_ADET!]", row2.PALET_ADET.ToString());
                html_table_row = html_table_row.Replace("[!PALET_NO!]", row2.PALET_NO.ToString());
                html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", row2.MAKINA_KOD);
                html_table_row = html_table_row.Replace("[!ISLEM_ADET!]", row2.ISLEM_ADET.ToString());
                html_table_row = html_table_row.Replace("[!ISLEM_TARIH!]", row2.ISLEM_TARIH);
                html_table_row = html_table_row.Replace("[!ISLEM_TUR!]", row2.ISLEM_TUR.ToString());
                html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", row2.VARDIYA_NO.ToString());
                html_table_row = html_table_row.Replace("[!URETIM_ADET!]", row2.URETIM_ADET);
                html_table_row = html_table_row.Replace("[!PARTI!]", row2.PARTI);
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9 + tmpRows2.Length).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", PP_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", PP_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_AlinmayanPaletlerTümü.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string GunlukKaliteGK(string GK_BAS_TARIH, string GK_BIT_TARIH, string GK_HAT_KOD1)  {
            var uzunluk = 0;
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false){
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == ""){
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\gunluk_kalite_gk.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\gunluk_kalite_gk_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_GunlukKalite_ROW> Report_GunlukKalite_ROWS = new List<CLS_Report_GunlukKalite_ROW>();
            var tmpRows = Report_GunlukKalite_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_GunlukKalite_LIST(SYSTEM_CODE, USER_INFO, GK_BAS_TARIH, GK_BIT_TARIH, GK_HAT_KOD1, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            double kalite_kaybı1;
            double kalite_kaybı2;
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            string[] degerler2;
            string[] saat_sil2;
            string gun2, ay2, yil2;
            string html = html_taslak;
            string html_table_rows = "";
            string karislastirSirketNo = "";
            string karislastirIsletmeKod = "";
            string karislastirTarih = "";
            string karislastirHatKod = "";
            string karislastirHatTanim = "";
            string karislastirMakinaKod = "";
            string karislastirMakinaTanim = "";
            string karislastirVardiyaNo = "";
            string karislastirKalipNo = "";
            string karislastirHataliUrunAdedi = "";
            string karislastirToplamNumune = "";
            string karislastirBırakilanAdet = "";
            string karislastirIskarta = "";
            string karislastirHataliUrunSayisi = "";
            string karislastirNumuneAdedi = "";
            string karislastirKaliteKaybı = "";
            string karislastirKaliteKaybı2 = "";
            for (int i = 0; i < tmpRows.Length; i++){
                CLS_Report_GunlukKalite_ROW row = tmpRows[i];
                degerler = row.TARIH.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].Substring(0, 4);
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                kalite_kaybı2 = 0.0f;
                kalite_kaybı1 = 0.0f;
                List<CLS_Report_GunlukKaliteHata_ROW> Report_GunlukKaliteHata_ROWS = new List<CLS_Report_GunlukKaliteHata_ROW>();
                var tmpRows2 = Report_GunlukKaliteHata_ROWS.ToArray();
                degerler2 = row.TARIH.Split('/');
                if (degerler2[0].Length == 1) { ay2 = "0" + degerler2[0].ToString(); }
                else { ay2 = degerler2[0].ToString(); }
                if (degerler2[1].Length == 1) { gun2 = "0" + degerler2[1].ToString(); }
                else { gun2 = degerler2[1].ToString(); }
                yil2 = degerler2[2].ToString();
                saat_sil2 = yil2.Split(' ');
                string son_tarih = gun2 + "." + ay2 + "." + saat_sil2[0];
                int vardiya_no = Convert.ToInt32(row.VARDIYA_NO);
                ret = ws.get_REPORT_CLS_Report_GunlukKaliteHata_LIST(SYSTEM_CODE, USER_INFO, son_tarih, row.HAT_KOD, row.MAKINA_KOD, vardiya_no, row.KALIP_NO, Global.Max_Row_Count, ref tmpRows2, ref errMsg);
                uzunluk += tmpRows2.Length;
                for (int k = 0; k < tmpRows2.Length; k++){
                    string html_table_row = html_taslak_table_row;
                    CLS_Report_GunlukKaliteHata_ROW row2 = tmpRows2[k];
                    html_table_row = html_table_row.Replace("[!HATA_TIP_NO!]", row2.HATA_TIP_NO.ToString());
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!SIRKET_NO!]", ""); }
                    html_table_row = html_table_row.Replace("[!SIRKET_NO!]", row.SIRKET_NO.ToString());
                    karislastirSirketNo = row.SIRKET_NO.ToString();
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", ""); }
                    html_table_row = html_table_row.Replace("[!ISLETME_KOD!]", row.ISLETME_KOD);
                    karislastirIsletmeKod = row.ISLETME_KOD;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!TARIH!]", ""); }
                    html_table_row = html_table_row.Replace("[!TARIH!]", saat_sil[0]);
                    karislastirTarih = row.TARIH;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!HAT_KOD!]", ""); }
                    html_table_row = html_table_row.Replace("[!HAT_KOD!]", row.HAT_KOD);
                    karislastirHatKod = row.HAT_KOD;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!HAT_TANIM!]", ""); }
                    html_table_row = html_table_row.Replace("[!HAT_TANIM!]", row.HAT_TANIM);
                    karislastirHatTanim = row.HAT_TANIM;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", ""); }
                    html_table_row = html_table_row.Replace("[!MAKINA_KOD!]", row.MAKINA_KOD);
                    karislastirMakinaKod = row.MAKINA_KOD;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", ""); }
                    html_table_row = html_table_row.Replace("[!MAKINA_TANIM!]", row.MAKINA_TANIM);
                    karislastirMakinaTanim = row.MAKINA_TANIM;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!KALIP_NO!]", ""); }
                    html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                    karislastirKalipNo = row.KALIP_NO;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString())
                    { html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", ""); }
                    html_table_row = html_table_row.Replace("[!VARDIYA_NO!]", row.VARDIYA_NO.ToString());
                    karislastirVardiyaNo = row.VARDIYA_NO.ToString();
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString() && karislastirBırakilanAdet == row.BIRAKILAN_ADET.ToString())
                    { html_table_row = html_table_row.Replace("[!BIRAKILAN_ADET!]", ""); }
                    html_table_row = html_table_row.Replace("[!BIRAKILAN_ADET!]", row.BIRAKILAN_ADET.ToString());
                    karislastirBırakilanAdet = row.BIRAKILAN_ADET.ToString();
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString() && karislastirIskarta == row.ISKARTA_YUZDE.ToString())
                    { html_table_row = html_table_row.Replace("[!ISKARTA!]", ""); }
                    html_table_row = html_table_row.Replace("[!ISKARTA!]", row.ISKARTA_YUZDE.ToString());
                    karislastirIskarta = row.ISKARTA_YUZDE.ToString();
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString() && karislastirHataliUrunSayisi == row.HATALI_URUN_ADEDI.ToString())
                    { html_table_row = html_table_row.Replace("[!HATALI_URUN_SAYISI!]", ""); }
                    html_table_row = html_table_row.Replace("[!HATALI_URUN_SAYISI!]", row.HATALI_URUN_ADEDI.ToString());
                    karislastirHataliUrunSayisi = row.HATALI_URUN_ADEDI.ToString();
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString() && karislastirNumuneAdedi == row.TOPLAM_NUMUNE.ToString())
                    { html_table_row = html_table_row.Replace("[!NUMUNE_ADEDI!]", ""); }
                    html_table_row = html_table_row.Replace("[!NUMUNE_ADEDI!]", row.TOPLAM_NUMUNE.ToString());
                    karislastirNumuneAdedi = row.TOPLAM_NUMUNE.ToString();
                    kalite_kaybı1 = (Convert.ToDouble(row.HATALI_URUN_ADEDI) / Convert.ToDouble(row.TOPLAM_NUMUNE)) * 100;
                    if (karislastirSirketNo == row.SIRKET_NO.ToString() && karislastirIsletmeKod == row.ISLETME_KOD.ToString() && karislastirTarih == row.TARIH && karislastirHatKod == row.HAT_KOD && karislastirMakinaKod == row.MAKINA_KOD && karislastirKalipNo == row.KALIP_NO && karislastirVardiyaNo == row.VARDIYA_NO.ToString() && karislastirKaliteKaybı == kalite_kaybı1.ToString())
                    { html_table_row = html_table_row.Replace("[!KALITE_KAYBI1!]", ""); }
                    html_table_row = html_table_row.Replace("[!KALITE_KAYBI1!]", kalite_kaybı1.ToString());
                    karislastirKaliteKaybı = kalite_kaybı1.ToString();
                    html_table_row = html_table_row.Replace("[!HATA_TIP_NO!]", row.HATA_TIP_NO.ToString());
                    html_table_row = html_table_row.Replace("[!HATA_NO!]", row2.HATA_NO.ToString());
                    html_table_row = html_table_row.Replace("[!HATA_SAYISI!]", row2.HATA_SAYISI.ToString());
                    kalite_kaybı2 = (Convert.ToDouble(row2.HATA_SAYISI) / Convert.ToDouble(row.TOPLAM_NUMUNE)) * 100;
                    html_table_row = html_table_row.Replace("[!KALITE_KAYBI2!]", kalite_kaybı2.ToString());
                    karislastirKaliteKaybı2 = kalite_kaybı2.ToString();
                    html_table_row = html_table_row.Replace("[!HATA_TIP_TANIM!]", row2.HATA_TIP_TANIM.ToString());
                    html_table_row = html_table_row.Replace("[!HATA_TANIM!]", row2.HATA_TANIM.ToString());
                    html_table_rows += html_table_row;
                }
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (uzunluk + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", GK_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", GK_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_GunlukKalite.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string MakinaDurusMST(string MST_BAS_TARIH, string MST_BIT_TARIH, string MST_HAT_KOD1, string MST_HAT_TURU){
            var errMsg = "";
            var ret = 0;
            var ret2 = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response, ref USER_INFO, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\makina_durus_mst.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\makina_durus_mst_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex){
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_MakinaDurus_ROW> Report_MakinaDurus_ROWS = new List<CLS_Report_MakinaDurus_ROW>();
            List<CLS_Report_MakinaDurus_ROW> Report_MakinaDurus_ROWS2 = new List<CLS_Report_MakinaDurus_ROW>();
            var tmpRows = Report_MakinaDurus_ROWS.ToArray();
            var tmpRowsSatir = Report_MakinaDurus_ROWS.ToArray();
            ret = ws.get_REPORT_EXCEL_MAKINA_DURUS_SURE_LIST_ORDER(SYSTEM_CODE, USER_INFO, MST_BAS_TARIH, MST_BIT_TARIH, MST_HAT_KOD1, MST_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            string[] detTanim = new string[tmpRows.Length];
            for (int i = 0; i < tmpRows.Length; i++) {
                detTanim[0] = tmpRows[0].DET_TANIM + "_SURE";
                int pos = Array.IndexOf(detTanim, tmpRows[i].DET_TANIM + "_SURE");
                if (pos == -1 && i != 0)
                { detTanim[i] = tmpRows[i].DET_TANIM + "_SURE"; }
            }
            detTanim = detTanim.Where(val => !string.IsNullOrEmpty(val)).ToArray();
            string rowField = "";
            string rowHeader = "<Row ss:AutoFitHeight=\"0\">\n" +
                               "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!HAT_KOD!]</Data></Cell>\n" +
                               "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!IMALAT_DEG_ADET!]</Data></Cell>\n" +
                               "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!ARIZA_DEG_ADET!]</Data></Cell>\n";
            for (int i = 0; i < detTanim.Length; i++) {
                rowField += "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!" + detTanim[i] + "!]</Data></Cell>\n";
            }
            string rowFooter = "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!TOPLAM!]</Data></Cell>\n" +"</Row>";
            string rowFile = rowHeader + rowField + rowFooter;
            string html = html_taslak;
            string html_table_rows = "";
            int TOPLAM = 0;
            int count = 0;
            string hatKod = "";
            int j = 0;
            tmpRowsSatir = tmpRows;
            CLS_Report_MakinaDurus_ROW row2 = new CLS_Report_MakinaDurus_ROW();
            var Report_MakinaDurus_ROW_list = new Hashtable();
            string html_table_row = rowFile;
            int döngüSayisi = tmpRows.Length - 1;
            string[,] multipleData;
            multipleData = new string[100, 300];
            Hashtable singleData = new Hashtable();
            int sayac = 0;
            for (int i = 0; i < tmpRows.Length; i = j){
                sayac = i;
                hatKod = tmpRows[i].HAT_NO;
                j = i;
                int countAynı = 0;
                CLS_Report_MakinaDurus_ROW row = tmpRows[i];
                CLS_Report_MakinaDurus_ROW rowSatir = tmpRowsSatir[i];
                while (hatKod == tmpRowsSatir[j].HAT_NO && i != döngüSayisi + 1) {
                    hatKod = tmpRows[j].HAT_NO;
                    countAynı++;
                    multipleData[j, 0] = tmpRows[j].HAT_NO;
                    multipleData[j, 1] = tmpRows[j].IMALAT_DEG_ADET;
                    multipleData[j, 2] = tmpRows[j].ARIZA_DEG_ADET;
                    multipleData[j, 3] = tmpRows[j].DET_TANIM + "_SURE";
                    multipleData[j, 4] = tmpRows[j].DURUS_SURE;
                    if (j == döngüSayisi) { j++; break; }
                    if (j != döngüSayisi) { j++; }
                    if (i != döngüSayisi) { i++; }
                }
                int toplam = 0;
                if (sayac == 0) {
                    singleData.Add("HAT_KOD", multipleData[0, 0]);
                    singleData.Add("IMALAT_DEG_ADET", multipleData[0, 1]);
                    singleData.Add("ARIZA_DEG_ADET", multipleData[0, 2]);
                    for (int m = 0; m < detTanim.Length; m++){
                        singleData.Add(detTanim[m], multipleData[m, 4]);
                    }
                    for (int k = 0; k < countAynı; k++) {
                        toplam += Convert.ToInt32(multipleData[k, 4]);
                    }
                    singleData.Add("TOPLAM", toplam.ToString());
                }
                else {
                    singleData.Add("HAT_KOD", multipleData[sayac, 0]);
                    singleData.Add("IMALAT_DEG_ADET", multipleData[sayac, 1]);
                    singleData.Add("ARIZA_DEG_ADET", multipleData[sayac, 2]);
                    int n = 0;
                    for (int m = 0; m < detTanim.Length; m++)
                    {
                        if (countAynı == 1) {
                            if (detTanim[m] == multipleData[sayac, 3]) singleData.Add(detTanim[m], multipleData[sayac, 4]);
                        }
                        if (countAynı != 1){
                            for (int k = 0; k < countAynı; k++){
                                if (detTanim[m] == multipleData[sayac + k, 3]) singleData.Add(detTanim[m], multipleData[sayac + k, 4]);
                            }
                        }
                    }
                    for (int k = sayac; k < (sayac + countAynı); k++) {
                        toplam += Convert.ToInt32(multipleData[k, 4]);
                    }
                    singleData.Add("TOPLAM", toplam.ToString());
                }
                html_table_row = html_table_row.Replace("[!HAT_KOD!]", (string)singleData["HAT_KOD"]);
                html_table_row = html_table_row.Replace("[!IMALAT_DEG_ADET!]", (string)singleData["IMALAT_DEG_ADET"]);
                html_table_row = html_table_row.Replace("[!ARIZA_DEG_ADET!]", (string)singleData["ARIZA_DEG_ADET"]);
                 for (int a = 0; a < detTanim.Length; a++) {
                    html_table_row = html_table_row.Replace("[!" + detTanim[a] + "!]", (string)singleData[detTanim[a]]);
                }
                html_table_row = html_table_row.Replace("[!TOPLAM!]", (string)singleData["TOPLAM"]);
                count++;
                html_table_rows += html_table_row;
                singleData.Clear();
                html_table_row = rowFile;
                for (int a = 0; a < 100; a++){
                    for (int b = 0; b < 300; b++) {
                        multipleData[a, b] = "";
                    }
                }
            }
            string baslikField = "";
            string baslikHeader = "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">HAT_KOD</Data></Cell>\n" +
                                  "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">IMALAT_DEG_ADET</Data></Cell>\n" +
                                   "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">ARIZA_DEG_ADET</Data></Cell>\n";
            for (int i = 0; i < detTanim.Length; i++){
                baslikField += "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">" + detTanim[i] + "</Data></Cell>\n";
            }
            string baslikFooter = "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">TOPLAM</Data></Cell>";
            string baslik = baslikHeader + baslikField + baslikFooter;
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (count + 9).ToString());
            html = html.Replace("<!--[!ROW!]-->", baslik);
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", MST_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", MST_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_MakinaDurus.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
            }
        [WebMethod(EnableSession = true)]
        public static string DepMakinaDurusDB(string DB_BAS_TARIH, string DB_BIT_TARIH, string DB_HAT_KOD1, string DB_HAT_TURU, string DB_ANA_DURUS) {
            var errMsg = "";
            var ret = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false)  {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\dep_makina_durus_mst.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\dep_makina_durus_mst_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_DepMakDurus_ROW> Report_DepMakDurus_ROWS = new List<CLS_Report_DepMakDurus_ROW>();
            List<CLS_Report_DepMakDurus_ROW> Report_DepMakDurus_ROWS2 = new List<CLS_Report_DepMakDurus_ROW>();
            var tmpRows = Report_DepMakDurus_ROWS.ToArray();
            var tmpRowsSatir = Report_DepMakDurus_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_DepMakDurus_LIST(SYSTEM_CODE, USER_INFO, DB_BAS_TARIH, DB_BIT_TARIH, DB_ANA_DURUS, DB_HAT_TURU, DB_HAT_KOD1, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }

            string[] detTanim = new string[tmpRows.Length];
            for (int i = 0; i < tmpRows.Length; i++) {
                if (tmpRows[i].DET_TANIM == "ARIZA") {
                    if (tmpRows[0].DET_TANIM == "ARIZA") detTanim[0] = tmpRows[0].DET_TANIM + "_" + tmpRows[0].DEPARTMAN_NO + "_SURE";
                    int pos = Array.IndexOf(detTanim, tmpRows[i].DET_TANIM + "_" + tmpRows[i].DEPARTMAN_NO + "_SURE");
                    if (pos == -1 && i != 0) {
                        detTanim[i] = tmpRows[i].DET_TANIM + "_" + tmpRows[i].DEPARTMAN_NO + "_SURE";
                    }
                }
            }
            detTanim = detTanim.Where(val => !string.IsNullOrEmpty(val)).ToArray();
            string rowField = "";
            string rowHeader = "<Row ss:AutoFitHeight=\"0\">\n" +
                               "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!HAT_KOD!]</Data></Cell>\n";
            for (int i = 0; i < detTanim.Length; i++){
                rowField += "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!" + detTanim[i] + "!]</Data></Cell>\n";
            }
            string rowFooter = "<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">[!TOPLAM!]</Data></Cell>\n" +
                               "</Row>";
            string rowFile = rowHeader + rowField + rowFooter;
            string html = html_taslak;
            string html_table_rows = "";
            int count = 0;
            string hatKod = "";
            int j = 0;
            tmpRowsSatir = tmpRows;
            CLS_Report_DepMakDurus_ROW row2 = new CLS_Report_DepMakDurus_ROW();
            var Report_MakinaDurus_ROW_list = new Hashtable();
            string html_table_row = rowFile;
            int döngüSayisi = tmpRows.Length - 1;
            string[,] multipleData;
            multipleData = new string[100, 300];
            Hashtable singleData = new Hashtable();
            int sayac = 0;
            for (int i = 0; i < tmpRows.Length; i = j) {
                sayac = i;
                hatKod = tmpRows[i].HAT_NO;
                j = i;
                int countAynı = 0;
                CLS_Report_DepMakDurus_ROW row = tmpRows[i];
                CLS_Report_DepMakDurus_ROW rowSatir = tmpRowsSatir[i];
                while (hatKod == tmpRowsSatir[j].HAT_NO && i != döngüSayisi + 1) {
                    hatKod = tmpRows[j].HAT_NO;
                    if (tmpRowsSatir[j].DET_TANIM == "ARIZA"){
                        multipleData[j, 0] = tmpRows[j].HAT_NO;
                        multipleData[j, 1] = tmpRows[j].DET_TANIM + "_" + tmpRows[j].DEPARTMAN_NO + "_SURE";
                        multipleData[j, 2] = tmpRows[j].DURUS_SURE.ToString();
                        countAynı++;
                        sayac = j;
                    }
                    if (j == döngüSayisi) { j++; break; }
                    if (j != döngüSayisi) { j++; }
                    if (i != döngüSayisi) { i++; }
                }
                int toplam = 0;
                if (sayac == 1) {
                    if (multipleData[0, 2] != null) {
                        singleData.Add("HAT_KOD", multipleData[0, 0]);
                        for (int m = 0; m < detTanim.Length; m++){
                            singleData.Add(detTanim[m], multipleData[m, 2]);
                        }
                        for (int k = 0; k < countAynı; k++){
                            toplam += Convert.ToInt32(multipleData[k, 2]);
                        }
                        singleData.Add("TOPLAM", toplam.ToString());
                    }
                }
                else{
                    singleData.Add("HAT_KOD", multipleData[sayac, 0]);
                    int n = 0;
                    for (int m = 0; m < detTanim.Length; m++)   {
                        if (countAynı == 1)  {
                            if (detTanim[m] == multipleData[sayac, 1]) singleData.Add(multipleData[sayac, 1], multipleData[sayac, 2]);
                        }
                        if (countAynı != 1) {
                            for (int k = sayac; k >= 0; k--){
                                if (detTanim[m] == multipleData[k, 1]) singleData.Add(detTanim[m], multipleData[k, 2]);
                            }
                        }
                    }
                    for (int k = (sayac - countAynı); k < j; k++)   {
                        if (multipleData[k, 2] != "") toplam += Convert.ToInt32(multipleData[k, 2]);
                    }
                    singleData.Add("TOPLAM", toplam.ToString());
                }
                if (singleData["HAT_KOD"] != ""){
                    html_table_row = html_table_row.Replace("[!HAT_KOD!]", (string)singleData["HAT_KOD"]);
                    for (int a = 0; a < detTanim.Length; a++) {
                        html_table_row = html_table_row.Replace("[!" + detTanim[a] + "!]", (string)singleData[detTanim[a]]);
                    }
                    html_table_row = html_table_row.Replace("[!TOPLAM!]", (string)singleData["TOPLAM"]);
                    count++;
                    html_table_rows += html_table_row;
                }
                singleData.Clear();
                html_table_row = rowFile;
                for (int a = 0; a < 100; a++){
                    for (int b = 0; b < 300; b++) {
                        multipleData[a, b] = "";
                    }
                }
            }
            string baslikField = "";
            string baslikHeader = "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">HAT_KOD</Data></Cell>\n";
            for (int i = 0; i < detTanim.Length; i++) {
                baslikField += "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">" + detTanim[i] + "</Data></Cell>\n";
            }
            string baslikFooter = "<Cell ss:StyleID=\"s17\"><Data ss:Type=\"String\">TOPLAM</Data></Cell>";
            string baslik = baslikHeader + baslikField + baslikFooter;
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (count + 9).ToString());
            html = html.Replace("<!--[!ROW!]-->", baslik);
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", DB_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", DB_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_DepMakinaDurus.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public static string ImalatDP(string DP_BAS_TARIH, string DP_BIT_TARIH, string DP_HAT_KOD1, string DP_HAT_TURU, string DP_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var ret2 = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
               return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false) {
                //MessageUtils.sendInformationMessage(Session, Response, errMsg);
                return "";
            }

            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\imalat_dp.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\imalat_dp_row.xml";
            try  {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex) {
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_Imalat_ROW> Report_Imalat_ROWS = new List<CLS_Report_Imalat_ROW>();
            var tmpRows = Report_Imalat_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_Imalat_LIST(SYSTEM_CODE, USER_INFO, DP_BAS_TARIH, DP_BIT_TARIH, DP_KALIP_NO, DP_HAT_KOD1, DP_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            for (int i = 0; i < tmpRows.Length; i++){
                string html_table_row = html_taslak_table_row;
                CLS_Report_Imalat_ROW row = tmpRows[i];
                degerler = row.DATE1.Split('/');
                ay = degerler[0].ToString();
                gun = degerler[1].ToString();
                yil = degerler[2].ToString();
                tarih = gun + "." + ay + "." + yil;
                saat_sil = tarih.Split(' ');
                html_table_row = html_table_row.Replace("[!HAT!]", row.HAT_KOD);
                html_table_row = html_table_row.Replace("[!TARIH!]", saat_sil[0]);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!CEKIS_SURE!]", row.CEKIS_SURE.ToString());
                html_table_row = html_table_row.Replace("[!DEVIR!]", row.DEVIR.ToString());
                html_table_row = html_table_row.Replace("[!LEGAL_DURUS!]", row.LEGAL_DURUS.ToString());
                html_table_row = html_table_row.Replace("[!KAYIP_ADET!]", row.KAYIP_ADET.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_ADET!]", row.BRUT_ADET.ToString());
                html_table_row = html_table_row.Replace("[!GERCEK_BRUT!]", row.GERCEK_BRUT.ToString());
                html_table_row = html_table_row.Replace("[!PAKETLENEN!]", row.PAKETLENEN.ToString());
                html_table_row = html_table_row.Replace("[!AYRILAN_ADET!]", row.AYRILAN_ADET.ToString());
                html_table_row = html_table_row.Replace("[!RED_ADET!]", row.RED_ADET.ToString());
                html_table_row = html_table_row.Replace("[!NET_ADET!]", row.NET_ADET.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_GRAM!]", row.BRUT_GRAM.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_C!]", row.BRUT_C.ToString());
                html_table_row = html_table_row.Replace("[!NET_GRAM!]", row.NET_GRAM.ToString());
                html_table_row = html_table_row.Replace("[!NET_C!]", row.NET_C.ToString());
                html_table_row = html_table_row.Replace("[!DURUS_SURE!]", row.DURUS.ToString());
                html_table_row = html_table_row.Replace("[!IML_DG_SURE!]", row.IM_DEG_SUR.ToString());
                html_table_row = html_table_row.Replace("[!VERIM!]", row.VERIM.ToString());
                html_table_row = html_table_row.Replace("[!AG_RANDIMAN!]", row.AG_RANDIMAN.ToString());
                html_table_row = html_table_row.Replace("[!FIRIN_CEKIS!]", row.FIRIN_CEKIS.ToString());
                html_table_row = html_table_row.Replace("[!STANDART_ADET!]", row.STANDART_ADET.ToString());
                html_table_row = html_table_row.Replace("[!HEDEF_ADET!]", row.HEDEF_ADET.ToString());
                html_table_row = html_table_row.Replace("[!KAPE!]", row.KAPE.ToString());
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", DP_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", DP_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_Imalat.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }

        [WebMethod(EnableSession = true)]
        public static string KalipDegisim(string kk_BAS_TARIH, string kk_BIT_TARIH, string kk_HAT_TURU, string kk_MAKINA_KOD1, string kk_KALIP_NO) {
            var errMsg = "";
            var ret = 0;
            var ret2 = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            if ( WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) == false) {
                return "";
            }

            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0)  {
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == "") {
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\kalip_degisim.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\kalip_degisim_row.xml";
            try {
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();

            }
            catch (Exception ex){
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_KalipDegisim_ROW> KalipDegisim_ROWS = new List<CLS_Report_KalipDegisim_ROW>();
            var tmpRows = KalipDegisim_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_KalıpDegisim_LIST(SYSTEM_CODE, USER_INFO, kk_BAS_TARIH, kk_BIT_TARIH, kk_KALIP_NO, kk_MAKINA_KOD1, kk_HAT_TURU, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;
            for (int i = 0; i < tmpRows.Length; i++){
                string html_table_row = html_taslak_table_row;
                CLS_Report_KalipDegisim_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!HAT_KOD!]", row.HAT_KOD);
                html_table_row = html_table_row.Replace("[!MAKINA!]", row.MAKINA_NO);
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO);
                html_table_row = html_table_row.Replace("[!BAS_TARIH!]", row.BAS_TARIH);
                html_table_row = html_table_row.Replace("[!BIT_TARIH!]", row.BIT_TARIH);
                html_table_row = html_table_row.Replace("[!CALISILAN_GUN!]", row.CALISMA_ORANI.ToString());
                html_table_row = html_table_row.Replace("[!KAYIP_SURE!]", row.KAYIP.ToString());
                html_table_row = html_table_row.Replace("[!LEGAL_DURUS!]", row.LEGAL_DURUS.ToString());
                html_table_row = html_table_row.Replace("[!GERCEK_BRUT!]", row.GER_BRUT2.ToString());
                html_table_row = html_table_row.Replace("[!NET_ADET!]", row.NET_PAKETLENEN.ToString());
                html_table_row = html_table_row.Replace("[!STANDART_ADET!]", row.STANDART_ADET.ToString());
                html_table_row = html_table_row.Replace("[!HEDEF_ADET!]", row.HEDEF_ADET.ToString());
                html_table_row = html_table_row.Replace("[!NET_C!]", row.NET_CEKIS.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_C!]", row.BRUT_CEKIS.ToString());
                html_table_row = html_table_row.Replace("[!AYRILAN_ADET!]", row.AYRILAN_ADET.ToString());
                html_table_row = html_table_row.Replace("[!RED_ADET!]", row.RED_ADET.ToString());
                html_table_row = html_table_row.Replace("[!ORT_BRUT_PAKETLENEN!]", row.ORTALAMA_BRUT.ToString());
                html_table_row = html_table_row.Replace("[!ORT_NET_PAKETLENEN!]", row.ORTALAMA_NET.ToString());
                html_table_row = html_table_row.Replace("[!VERIM!]", row.VERIM.ToString());
                html_table_row = html_table_row.Replace("[!DEVIR!]", row.DEVIR.ToString());
                html_table_row = html_table_row.Replace("[!AG_RANDIMAN!]", row.AGIRLIK_RANDIMAN.ToString());
                html_table_row = html_table_row.Replace("[!FIRIN_CEKIS!]", row.FIRIN_CEKIS.ToString());
                html_table_row = html_table_row.Replace("[!STD%!]", row.STANDART_YUZDE.ToString());
                html_table_row = html_table_row.Replace("[!HEDEF%!]", row.HEDEF_YUZDE.ToString());
                html_table_row = html_table_row.Replace("[!BRUT_GRAM!]", row.BRUT_GRAM.ToString());
                html_table_row = html_table_row.Replace("[!NET_GRAM!]", row.NET_GRAM.ToString());
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", kk_BAS_TARIH);
            html = html.Replace("<!--[!BIT_TARIH!]-->", kk_BIT_TARIH);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_KalipDegisim.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";

        }
        [WebMethod(EnableSession = true)]
        public static string DcSipAmb(string dc_ITEM_NO, string dc_ITEM_TANIM, string dc_SIPARIS_NO, string dc_IHT_BAS_TAR, string dc_IHT_BIT_TAR, string dc_PRO_BAS_TAR, string dc_PRO_BIT_TAR, string dc_SEVK_BAS_TAR, string dc_SEVK_BIT_TAR) {
            var errMsg = "";
            var ret = 0;
            var ret2 = 0;
            var SYSTEM_CODE = Global.SYSTEM_CODE;
            pasabahce.PASABAHCE_WEB_SERVICE ws = new pasabahce.PASABAHCE_WEB_SERVICE();
            #region checkWebConfig
            var webConfig = new WebConfig();
            ret = WebConfigUtils.readWebConfig(ref webConfig, ref errMsg);
            if (ret < 0)  {   
            return ret.ToString() + " " + errMsg;
            }
            if (WebConfigUtils.checkWebConfig(HttpContext.Current.Response, HttpContext.Current.Session, ref webConfig) ==false) {
                return "";
            }
            #endregion
            #region checkIfLoggedIn
            errMsg = "";
            var USER_INFO = new CLS_USER_INFO();
            ret = LoginUtils.checkIfLoggedIn(ws, webConfig, HttpContext.Current.Session, HttpContext.Current.Response,
                ref USER_INFO, ref errMsg);
            if (ret < 0){
                return ret.ToString() + " " + errMsg;
            }
            if (USER_INFO.USER_ID == ""){
                return ret.ToString() + " " + errMsg;
            }
            #endregion
            string html_taslak = "";
            string filename_taslak = webConfig.WebRootPath + "Xml\\dc_sip_amb.xml";
            string html_taslak_table_row = "";
            string filename_table_row = webConfig.WebRootPath + "Xml\\dc_sip_amb_row.xml";
            try{
                StreamReader sr = null;
                sr = File.OpenText(filename_taslak);
                html_taslak = sr.ReadToEnd();
                sr.Close();
                sr = File.OpenText(filename_table_row);
                html_taslak_table_row = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex){
                return ex.Message.ToString();
            }
            html_taslak = html_taslak.Replace("<!--[!USERNAME!]-->", USER_INFO.USER_NAME + "&nbsp;&nbsp;&nbsp;");
            errMsg = "";
            List<CLS_Report_DcSipAmb_ROW> DcSipAmb_ROWS = new List<CLS_Report_DcSipAmb_ROW>();
            var tmpRows = DcSipAmb_ROWS.ToArray();
            ret = ws.get_REPORT_CLS_Report_DcSipAmb_LIST(SYSTEM_CODE, USER_INFO, dc_ITEM_NO, dc_ITEM_TANIM, dc_SIPARIS_NO, dc_IHT_BAS_TAR, dc_IHT_BIT_TAR, dc_PRO_BAS_TAR, dc_PRO_BIT_TAR, dc_SEVK_BAS_TAR, dc_SEVK_BIT_TAR, Global.Max_Row_Count, ref tmpRows, ref errMsg);
            if (ret < 0) {
                return ret.ToString() + " " + errMsg;
            }
            string html = html_taslak;
            string html_table_rows = "";
            string[] degerler;
            string[] saat_sil;
            string gun, ay, yil, tarih;

            for (int i = 0; i < tmpRows.Length; i++) {
                string html_table_row = html_taslak_table_row;
                CLS_Report_DcSipAmb_ROW row = tmpRows[i];
                html_table_row = html_table_row.Replace("[!SIPARIS_NO!]", row.SIPARIS_NO);
                html_table_row = html_table_row.Replace("[!TARIH!]", row.TARIH);
                html_table_row = html_table_row.Replace("[!MUSTERI_NO!]", row.MUSTERI_N0);
                html_table_row = html_table_row.Replace("[!MUS_TANIM!]", row.MUS_TANIM);
                html_table_row = html_table_row.Replace("[!SIRA!]", row.SIRA_NO);
                html_table_row = html_table_row.Replace("[!ITEM_NO!]", row.ITEM_NO);
                html_table_row = html_table_row.Replace("[!ASIM_ORAN!]", row.ASIM_ORAN.ToString());
                html_table_row = html_table_row.Replace("[!BIRIM_ICAD!]", row.BIRIM_ICAD.ToString());
                html_table_row = html_table_row.Replace("[!KOLI_ICAD!]", row.KOLI_ICAD.ToString());
                html_table_row = html_table_row.Replace("[!PROFORMA_TERMIN_TARIHI!]", row.PROFORMA_TERMIN_TARIHI.ToString());
                html_table_row = html_table_row.Replace("[!DURUM!]", row.DURUM_NO.ToString());
                html_table_row = html_table_row.Replace("[!ALT_SIRA_NO!]", row.ALT_SIRA_NO.ToString());
                html_table_row = html_table_row.Replace("[!TEDARIK_NO!]", row.TEDARIK_NO.ToString());
                html_table_row = html_table_row.Replace("[!FAB!]", row.FABRIKA_NO.ToString());
                html_table_row = html_table_row.Replace("[!MIKTAR!]", row.MIKTAR.ToString());
                html_table_row = html_table_row.Replace("[!DOVIZ_KOD!]", row.DOVIZ_KOD.ToString());
                html_table_row = html_table_row.Replace("[!SATIS_SEKLI_KOD!]", row.SATIS_SEKLI_KOD.ToString());
                html_table_row = html_table_row.Replace("[!BIRIM_FIYAT_FOB_USD!]", row.BIRIM_FIYAT_FOR_USD.ToString());
                html_table_row = html_table_row.Replace("[!FAB_SEVK_TARIHI!]", row.FAB_SEVK_TARIH.ToString());
                html_table_row = html_table_row.Replace("[!KALIP_NO!]", row.KALIP_NO.ToString());
                html_table_row = html_table_row.Replace("[!ITEM_TANIM!]", row.ITEM_TANIM.ToString());
                html_table_row = html_table_row.Replace("[!MALZEME!]", row.MALZEME.ToString());
                html_table_row = html_table_row.Replace("[!MIKTAR2!]", row.MIKTAR2.ToString());
                html_table_row = html_table_row.Replace("[!MLZ_TANIM!]", row.MLZ_TANIM.ToString());
                html_table_rows += html_table_row;
            }
            DateTime dt = DateTime.Today;
            string now = String.Format("{0:dd.MM.yyyy}", dt);
            html = html.Replace("[!ROW_COUNT!]", (tmpRows.Length + 9).ToString());
            html = html.Replace("<!--[!ROWS!]-->", html_table_rows);
            html = html.Replace("<!--[!BAS_TARIH!]-->", dc_IHT_BAS_TAR);
            html = html.Replace("<!--[!BIT_TARIH!]-->", dc_IHT_BIT_TAR);
            html = html.Replace("<!--[!USER!]-->", USER_INFO.USER_ID);
            html = html.Replace("<!--[!TARIH!]-->", now);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=Report_DcSipAmb.xls");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return "0";
        }

        public void sendNoCache()   {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
            Session.Timeout = 60;
        }
        public string getFormDataString(string s)  {
            if (s == null){
                return ("");
            }
            if (s == "") {
                return ("");
            }
            return (s);
        }
        public string getFormDataStringWithEliminateZero(string s) {
            string ss = getFormDataString(s);
            if (ss == "0"){
                return ("");
            }
            return (ss);
        }

        public void sendResponse(int status, string msg, string xmlResponse)  {
            Response.Write("<gns><status>" + status.ToString() + "</status><msg>" + msg + "</msg>" + xmlResponse + "</gns>");
        }
        public void sendError(int status, string msg, string detailed_msg){
            Response.Write("<gns><status>" + status.ToString() + "</status><msg>" + msg + "</msg><detailed_msg>" + detailed_msg + "</detailed_msg></gns>");
        }
        public void sendSuccess()   {
            Response.Write("<gns><status>0</status><msg>OK</msg><detailed_msg>OK</detailed_msg></gns>");
        }
        public void sendHtml(CLS_USER_INFO userInfo, WebConfig webConfig, string Lang, string html) {
            var errMsg = "";
            //CMS+++
            var ret = CMSUtils.getCMSHTML(userInfo, webConfig, Lang, ref html, ref errMsg);
            if (ret < 0) {
                sendError(ret, errMsg, errMsg);
                return;
            }
            //CMS---
            Response.Write(html);
        }
    }
}
