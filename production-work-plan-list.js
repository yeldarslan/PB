var s_KALIP_NO="";
var s_BAS_TARIH="";
var s_HAT_KOD = "";
var s_HAT_TANIM = "";
var s_URETIM_TIPI = "";
var s_URET_BAS_TAR = "";

var params = {
   
    filt1: '',
    filt2: '',
    filt3:'',
};
$(document).ready(function () {
    if (typeof ($.cookie("URET_BAS_TAR")) === "undefined") {
        document.cookie = 'URET_BAS_TAR' + "=" + '';
    }
    if (typeof ($.cookie("HAT_KOD")) === "undefined") {
        document.cookie = 'HAT_KOD' + "=" + '';
    }
    if (typeof ($.cookie("HAT_TANIM")) === "undefined") {
        document.cookie = 'HAT_TANIM' + "=" + '';
    }
    if (typeof ($.cookie("ACIKLAMA")) === "undefined") {
        document.cookie = 'ACIKLAMA' + "=" + '';
    }


    if ($.cookie("KALIP_NO")) {
      
        params.filt1 = $.cookie("KALIP_NO");
        params.filt2 = $.cookie("URETIM_TIPI");
        params.filt3 = $.cookie("URET_BAS_TAR");
    } else {
  
    }
    var grid = $("#grid").kendoGrid({

        dataSource: {
            batch: true,
            transport: {
                prefix: "",
                read: {
                    url: "../../../ProductionWorkPlan/ProductionWorkPlanList.aspx/GetProductionWorkPlanList",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: params,
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                }

            },
            sortable: true,
            schema: {
                data: "Data",
                total: "Total",
                errors: "Errors"
            },
            schema: {
                data: function (d) {
                    var kendoData = $.parseJSON(d.d);
                    return $.parseJSON(kendoData.Data);
                },
                total: function (d) {
                    var kendoData = $.parseJSON(d.d);
                    return kendoData.Total;
                },
                model: {
                    id: "KALIP_NO",
                    subId: "URETIM_TIPI",
                    thirdId: "URET_BAS_TAR",
                    fourthId: "",
                    fifthId: "HAT_KODU",
                    fields: {
                        "MANDT": { type: "string" },
                        "ISLETME_KOD": { type: "string" },
                        "HAT_KODU": { type: "string" },
                        "URET_BAS_TAR": { type: "string" },
                        "KALIP_NO": { type: "string" },
                        "HAT_TURU_TANIM": { type: "string" },
                        "DIL_NO": { type: "number" },
                    }
                }
            }
        },
        sortable: true,
        resizable: true,
        change: onChange,
       dataBound: function (e) {
            var grid = e.sender;
            grid.select(grid.tbody.find('tr:first'));
        },
        selectable: "multiple",
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "Kalıp No"
                }
            }
        },
        pageable: {
            refresh: true,
            buttonCount: 5,
            pageSize: 10
        },
        columns: [
            {
                field: "HAT_TURU_TANIM", filterable: false,
                title: LANG_TEXT['js_hat_türü_tanım'],
                width: 170,
            },
            {
                field: "KALIP_NO",
                title: LANG_TEXT['js_kalıp_no'],
                width: 90,
                filterable: {
                    ui: titleFilter
                }
            }, {
                field: "URET_BAS_TAR",
                title: LANG_TEXT['js_pln_min_trh'],
                template: function (dataItem) {
                    return (dataItem.URET_BAS_TAR.substring(6, 8) + "." + dataItem.URET_BAS_TAR.substring(4, 6)+"."+dataItem.URET_BAS_TAR.substring(0, 4));
                  },
                filterable: false,
                width: 100,
            },

        ],
        toolbar: [
                     { template: "<input type='text' id='kalipNo' />" },
                     {
                         template: "<input type='button' class='k-button' value='" + LANG_TEXT['js_filtrele'] + "' onclick='filter()' />",
                         imageclass: "k-icon k-i-pencil"
                     },
                     {
                         template: "&nbsp;&nbsp;&nbsp;<input type='button' class='k-button' value='" + LANG_TEXT['js_iş_planı'] + "' onclick='createWorkPlan()' />",
                         imageclass: "k-icon k-i-pencil"
                     },
                     {
                         template: "&nbsp;&nbsp;&nbsp;<input type='button' class='k-button' value='" + LANG_TEXT['js_aktar'] + "'  id='aktar' name='aktar' onclick='aktar()' disabled/>",
                         imageclass: "k-icon k-i-pencil"
                     },
                     { template: "	&nbsp;	&nbsp;	&nbsp;<label>Ürt. Baş. Tar.: " + $.cookie("URET_BAS_TAR") + "</label>" },
                     { template: "	&nbsp;	&nbsp;	&nbsp;<label>Hat Kod: " + $.cookie("HAT_KOD") + "</label>" },
                     { template: "	&nbsp;	&nbsp;	&nbsp;<label>Hat Tanım: " + $.cookie("HAT_TANIM") + "</label>" },
                     { template: "	&nbsp;	&nbsp;	&nbsp;<label>Açıklama: " + $.cookie("ACIKLAMA") + "</label>" },
                     {
                         template: "&nbsp;&nbsp;&nbsp;<input type='button' class='k-button' id='iptal' name='iptal' value='" + LANG_TEXT['js_seçilenleri_iptal_et'] + "'  onclick='clearAll()' disabled />",
                         imageclass: "k-icon k-i-pencil"
                     },

        ],
    }).data("kendoGrid");
 if ($.cookie("KALIP_NO")) {
        document.getElementById("iptal").disabled = false;
    } else {
        document.getElementById("iptal").disabled = true;
    }
    
});
function onChange(arg) {
   
    var grid = $("#grid").data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select());
    if (!selectedItem.KALIP_NO || selectedItem.KALIP_NO === "") {
        KALIP_NO = $.cookie("KALIP_NO");       
    }
    else {
              
    }
   
    var gridDet = $("#grid-det").data("kendoGrid");
    var newData = new kendo.data.DataSource({
        batch: true,
        transport: {
            prefix: "",
            read: {
                url: "../../../ProductionWorkPlanDet/ProductionWorkPlanDetList.aspx/GetProductionWorkPlanDetList",
               data: { KALIP_NO: selectedItem.KALIP_NO, URETIM_TIPI: selectedItem.URETIM_TIPI, URET_BAS_TAR: selectedItem.URET_BAS_TAR, BAS_TARIH:selectedItem.BAS_TARIH, HAT_KODU:selectedItem.HAT_KODU },
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            },
          
            parameterMap: function (data, operation) {
                if (operation != "read") {
                } else {
                    return kendo.stringify(data);
                }
            }
        },
        pageable: {
            refresh: true,
            buttonCount: 5,
            pageSize: 10
        },
        schema: {
            data: function (d) {
                var kendoData = $.parseJSON(d.d);
                return $.parseJSON(kendoData.Data);
            
            },
            total: function (d) {
                var kendoData = $.parseJSON(d.d);
                return kendoData.Total;
            },
            model: {
                id: "KALIP_NO",
                subId: "URETIM_TIPI",
                thirdId: "URET_BAS_TAR",
                fourthId:"BAS_TARIH",
                fifthId:"HAT_KODU",
                fields: {
                    "MANDT": { type: "string" },
                    "ISLETME_KOD": { type: "string" },
                    "HAT_KODU": { type: "string" },
                    "CHARG": { type: "string" },
                    "URETIM_SIP_NO": { type: "string" },
                    "ITEM_NO": { type: "string" },
                    "ITEM_NO_DGR": { type: "string" },
                    "VERSIYON_KOD": { type: "string" },
                    "URET_BAS_TAR": { type: "string" },
                    "URET_BIT_TAR": { type: "string" },
                    "SAT_SIPARIS_NO": { type: "string" },
                    "SAT_SIP_SIRA_NO": { type: "string" },
                    "PLAN_MIKTAR": { type: "number" },
                    "KALIP_NO": { type: "string" },
                    "BIRIM_KOD": { type: "string" },
                    "DIL_NO": { type: "number" },
                    "KISA_TANIM": { type: "string" },
                    "ACIKLAMA": { type: "string" }
               }
            }
        }
    });


    gridDet.setDataSource(newData);
}

$("a.k-button").hide();
$("a.k-button-icontext").hide();
$("a.k-grid-Düzenle").hide();

function createWorkPlan() {  
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(grid.select());  
    var queryString = "?id=" + dataItem.id;
    if (dataItem[dataItem.subId]) {
        queryString = queryString + "&subId=" + dataItem[dataItem.subId];
    }
    if (dataItem[dataItem.thirdId]) {
        queryString = queryString + "&thirdId=" + dataItem[dataItem.thirdId];
    }

    if (dataItem[dataItem.fourthId]) {
        queryString = queryString + "&fourthId=" + dataItem[dataItem.fourthId];
    }
    if (dataItem[dataItem.fifthId]) {
        queryString = queryString + "&fifthId=" + dataItem[dataItem.fifthId];
    }
    if (dataItem[dataItem.sixthId]) {
        queryString = queryString + "&sixthId=" + dataItem[dataItem.sixthId];
    }
    console.log("queryString: " + queryString);
    var url = "../../../ProductionWorkPlan/ProductionWorkPlanEdit.aspx" + queryString;
    console.log("editRecord url " + url);
    window.location = url;
}


function aktar() {
    var checkCount = jQuery(".chk_aktar:checked").length;
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(grid.select());
    
    if ($.cookie("CHARG") == null || $.cookie("CHARG") == "")
    { $.notify("Parti kodu boş olamaz."); }

        $.ajax({
            type: "POST",
            url: "../../../ProductionWorkPlan/ProductionWorkPlanCreate.aspx/Create",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (msg) {
                if (msg.d == "0") {              
                    $.notify(LANG_TEXT['js_üretim_iş_planı_başarılı'], "successs");
                    setTimeout(clearAll, 2000);

                } else {
                    alert("gelenler: "+msg.d);
                    $.notify(LANG_TEXT['js_üretim_iş_planı_başarısız'] + msg.d, "error");
                }
            }
        });
       
}

function kick_off() {
    
    window.location.href = "/ProductionWorkPlan/ProductionWorkPlanList.aspx";
}

function titleFilter(element) {   //filtre

}

$(document).keyup(function (e) {
    if ($("#kalipNo").is(':focus')) {
        if (e.keyCode === 13) {
            filter();
        }
    }
});


function filter() {

    params.filt1 = (document.getElementById("kalipNo").value.trim()).toUpperCase();
    refreshGrid();
}

function clearAll() {
    params.filt1 = "";
    params.filt2 = "";
    params.filt3 = "";
    s_KALIP_NO = "";
    s_BAS_TARIH = "";
    s_HAT_KOD = "";

    document.cookie = 'HAT_KOD =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'HAT_TANIM =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'KALIP_NO =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'BAS_TARIH =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'URETIM_SIP_NO =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'ITEM_NO =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'ITEM_NO_DGR =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'VERSIYON_KOD =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'URETIM_TIPI =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'SAT_SIPARIS_NO =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'SAT_SIP_SIRA_NO =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'PLAN_MIKTAR =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'ITEM_NO_DGR =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'BIRIM_KOD =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'URET_BAS_TAR =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'URET_BIT_TAR =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'CHARG =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'ACIKLAMA =; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    
    document.getElementById("iptal").disabled = true;
    kick_off();
}

function refreshGrid() {
    $("#grid").data("kendoGrid").dataSource.read();
    $('#grid').data("kendoGrid").refresh();
}

function toDate(value) {
    var gun, ay, yil;
    var yil = Convert.ToString(value).Substring(0, 3);
    return yil;
}
