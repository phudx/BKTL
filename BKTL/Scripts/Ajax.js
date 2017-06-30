var objPara = {
    url: "",
    data: "",
    async: true,
    idName: "",
    functionName: ""
};
function AjaxPostRequest(url, parameters, functName, successCallback) {
    AjaxRequest(url, parameters, 'POST', true, functName, successCallback);
}

function AjaxPostRequestSync(url, parameters, functName, successCallback) {
    AjaxRequest(url, parameters, 'POST', false, functName, successCallback);
}
function AjaxRequest(url, parameters, type, async, functName, successCallback) {
    $.ajax({
        url: url == undefined ? "" : url,
        data: parameters == undefined ? "" : JSON.stringify(parameters),
        type: type == undefined ? 'POST' : type,
        async: async == undefined ? true : async,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var result = $.parseJSON(response.d);
            successCallback(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            ShowMessage(functName + " - Error: " + xhr.responseText);
        }
    });
}

function AjaxLoadHTMLByFunct(objPara, functStatusTrue) {
    $.ajax({
        type: "POST",
        url: objPara.url,
        data: objPara.data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: objPara.async,
        success: function (response) {
            var result = $.parseJSON(response.d);
            if (result.Status) {
                $('#' + objPara.idName).html(result.Message);
                functStatusTrue();
            }
            else {
                ShowMessage(result.Message, 350);
            }
        },
        error: function (xhr, status, error) {
            ShowMessage(objPara.functionName + " - Error: " + xhr.responseText);
        }
    });
}

function AjaxLoadHTML(objPara) {
    $.ajax({
        type: "POST",
        url: objPara.url,
        data: objPara.data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: objPara.async,
        success: function (response) {
            var result = $.parseJSON(response.d);
            if (result.Status) {
                $('#' + objPara.idName).html(result.Message);
            }
            else {
                ShowMessage(result.Message, 350);
            }
        },
        error: function (xhr, status, error) {
            ShowMessage(objPara.functionName + " - Error: " + xhr.responseText);
        }
    });
}

function AjaxLoadHistory(objPara) {
    $.ajax({
        type: "POST",
        url: objPara.url,
        data: objPara.data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: objPara.async,
        success: function (response) {
            var result = $.parseJSON(response.d);
            if (result.Status) {
                $('#' + objPara.idName).html(result.Message);
                $('#errorHistory').html("");
            }
            else {
                $('#' + objPara.idName).html("");
                $('#errorHistory').html(result.Message);
            }
        },
        error: function (xhr, status, error) {
            ShowMessage(objPara.functionName + " - Error: " + xhr.responseText);
        }
    });
}
function CheckAjaxRequest(objPara) {
    $.ajax({
        type: "POST",
        url: objPara.url,
        data: objPara.data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: objPara.async,
        success: function (response) {
            var result = $.parseJSON(response.d);
            if (result.Status) {
                if (result.Message == '') return;
                else ShowMessage(result.Message);
            }
            else ShowMessage(result.Message);
        },
        error: function (xhr, status, error) {
            ShowMessage(objPara.functionName + " - Error: " + xhr.responseText);
        }
    });
}

function AjaxInsertOrUpdate(objPara, functStatusTrue) {
    $.ajax({
        type: "POST",
        url: objPara.url,
        data: objPara.data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: objPara.async,
        success: function (response) {
            var result = $.parseJSON(response.d);
            if (result.Status) {
                if (result.Message == '')
                    functStatusTrue();
                else {
                    ShowMessage(result.Message);
                }
            }
            else {
                ShowMessage(result.Message);
            }
        },
        error: function (xhr, status, error) {
            ShowMessage(objPara.functionName + " - Error: " + xhr.responseText);
        }
    });
}
function EncodeBase(str) {
    var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
    // Encode the String
    return Base64.encode(str);
}
function DecodeBase(str) {
    var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
    // Decode the String
    return Base64.decode(str);
}
function RemoveBad(strTemp) {
    strTemp = strTemp.replace(/\<|\>|\"|\'|\%|\;|\(|\)|\&|\+|\-/g, "");
    return strTemp;
}
function ajaxindicatorstart(text) {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading" style="display:none"><div><img src="../Images/ajax_loader.gif"><div>' + text + '</div></div><div class="bg"></div></div>');
    }

    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });

    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });

    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'
    });
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(10);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(10);
    jQuery('body').css('cursor', 'default');
}
jQuery(document).ajaxStart(function () {
    //show ajax indicator
    ajaxindicatorstart('Đang lấy dữ liệu xin vui lòng chờ...');
}).ajaxStop(function () {
    //hide ajax indicator
    ajaxindicatorstop();
});



