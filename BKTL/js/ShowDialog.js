
//----------------------------------------------Dialog-------------------------------------------------------------------------------
function ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, scrollingType) {
    if ($("#dialog" + dialogName).length == 0)
        $(document.body).append(
                    "<div id='dialog" + dialogName + "'>"
                       + "<iframe id='frame" + dialogName + "' name='frame" + dialogName + "' frameborder='0' width='100%' scrolling='" + scrollingType + "' style='overflow: hidden;" +
                            "overflow-x: hidden; overflow-y: hidden; height: 100%; width: 100%; position: absolute;" +
                            "top: 0px; left: 0px; right: 0px; bottom: 0px'></iframe>" +
                    "</div>");
    $("#dialog" + dialogName).dialog({
        dialogClass: dialogStyle,
        resizable: false,
        modal: true,

        open: function (type, data) {
            $(this).parent().appendTo("form");
            $("#frame" + dialogName).attr("src", url);
        },
        width: dialogWidth,
        height: dialogHeight,
        title: dialogTitle,
        closeOnEscape: true
    });
}
function ShowDialogDemo() {
    var dialogName = "Text";
    var dialogWidth = $(window).width() - 750;
    var dialogHeight = $(window).height() - 650;
    var dialogStyle = "DynamicDialogStyle";
    var url = "HTMLPage2.html";
    var dialogTitle = "Text";
    ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, 'yes');
}
function ShowDialogDemoControl() {
    var dialogName = "Demo";
    var dialogWidth = $(window).width() - 30;
    var dialogHeight = $(window).height() - 30;
    var dialogStyle = "DynamicDialogStyle";
    var url = "Default4.html";
    var dialogTitle = "Chi tiết chức năng nhiệm vụ của các thành phần có trên website";
    ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, 'yes');
}
function ShowEditDialog() {
    var dialogName = "DemoBeta";
    var dialogWidth = 615;
    var dialogHeight = 210;
    var dialogStyle = "DynamicDialogStyle";
    var url = "Add.html";
    var dialogTitle = "Thêm thông tin khách hàng";
    ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, 'yes');
}

function ShowEditDialogfont14() {
    var dialogName = "DemoBeta";
    var dialogWidth = 800;
    var dialogHeight = 245;
    var dialogStyle = "DynamicDialogStyle";
    var url = "AddCustomer.html";
    var dialogTitle = "Thêm thông tin khách hàng";
    ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, 'yes');
}

function ShowInfoDialog() {
    var dialogName = "Demo";
    var dialogWidth = $(window).width() - 820;
    var dialogHeight = $(window).height() - 650;
    var dialogStyle = "DynamicDialogStyle";
    var url = "HTMLPage5.html";
    var dialogTitle = "Thông tin cá nhân";
    ShowDialog(dialogName, dialogWidth, dialogHeight, dialogStyle, url, dialogTitle, 'yes');
}
//-------------------------------Confirm---------------------------------------------------------------------------
//typeConfirm = true là gọi doPostBack, != 1 là gọi hàm tiếp theo trên Client
// funtionName hàm được gọi trên Client tiếp theo nếu typeConfirm != 1
//yesNo = true/false nút đồng ý hay hủy
var arrParameterConfirm;
function bkav_confirm_check(idBtn, typeConfirm, yesNo, functionName, isInsideiframe, isHref) {
    if (typeConfirm == true && idBtn != '') {
        if (yesNo) {
            if (isHref) {
                window.location.href = $(idBtn).attr('href');

            } else {
                __doPostBack($(idBtn).attr('id'), '');
                bkav_alert_close();
            }

        }
        else {
            bkav_alert_close();
        }
    }
    else {
        if (yesNo) {
            setTimeout(function () {
                var functionNameMain = window[functionName];
                functionNameMain.apply(functionNameMain, arrParameterConfirm);
                bkav_alert_close(isInsideiframe);
            }, 300);
        }
        else {
            bkav_alert_close(isInsideiframe);
        }

    }
}

//VD:   bkav_confirm('', false, true,false 'warning','Thông báo','Bạn chắc chắn bẩm ký và gửi','SignSend()')
function bkav_confirm(idBtn, typeConfirm, isInsideiframe, isHref, typeAlert, title, content, functionName, arrParameter, width, isHasIcon, bkav_confirm_button1, bkav_confirm_button2) {
    arrParameterConfirm = arrParameter;
    width = (width == undefined) ? 428 : width;
    var imgtypeurl = '';
    var htmlcontent = '';

    if (bkav_confirm_button1 == "" || bkav_confirm_button1 == undefined)
        bkav_confirm_button1 = "Có";
    if (bkav_confirm_button2 == "" || bkav_confirm_button2 == undefined)
        bkav_confirm_button2 = "Không";

    if (idBtn == '') {
        btnGroup = "<div style='float:right;margin-top:10px'>" +
         "<button class='btn btn-primary' onclick=bkav_confirm_check(\'\'," + typeConfirm + "," + true + ",\'" + functionName + "\'," + isInsideiframe + "," + isHref + ")>" + bkav_confirm_button1 + "</button>" +
         " <button class='btn btn-close' onclick='bkav_alert_close()'>" + bkav_confirm_button2 + "</button></div>";
    }
    else {
        idBtn = $(idBtn).attr('id');
        btnGroup = "<div style='float:right;margin-top:10px'>" +
           "<button class='btn btn-primary' onclick=bkav_confirm_check(" + idBtn + "," + typeConfirm + "," + true + ",\'" + functionName + "\'," + isInsideiframe + "," + isHref + ",\'\')>" + bkav_confirm_button1 + "</button>" +
           "<button class='btn btn-close' onclick='bkav_alert_close()'>" + bkav_confirm_button2 + "</button></div>";
    }
    if (title == '') { title = 'Thông báo'; }
    if (typeAlert == 'html') {
        htmlcontent = " <div class='row' style='margin-top:10px;margin-right:0px'>" +
               " <div class='col-xs-12 col-md-12'>" +
                    content +
                "</div></div>";
    }
    else {
        switch (typeAlert) {
            case "error": imgtypeurl = '/img/icon-loi.png'; break;
            case "success": imgtypeurl = '/img/icon-thanh-cong.png'; break;
            case "warning": imgtypeurl = '/img/icon-thong-bao.png'; break;
        }

        var htmlicon = "<div style='width:60px;float:left'>" +
                            "  <img src='" + imgtypeurl + "' class='img' width='50px' style='margin: 0px 10px 0px 0px;'/>" +
                         " </div>";
        var styleContent = ("style='width:" + (width - 100) + "px;float:right'");
        if (isHasIcon != undefined && !isHasIcon) {
            htmlicon = "";
            styleContent = "";
        }

        htmlcontent = "<div style='margin:10px 5px;text-align: justify;'>" +
                          "<div style='min-height:38px'>" +
                               htmlicon +
                               " <div " + styleContent + ">" +
                                    content +
                                "</div>" +
                            "</div>" +
                            btnGroup
        "</div>";
    }
    if ($('#bkav_alert_dialog').length > 0) {
        $('#bkav_alert_dialog').html(htmlcontent);
    } else {
        $(document.body).append("<div class='container' id='bkav_alert_dialog' style='margin-bottom: 10px'>" +
            htmlcontent + "</div>");
    }
    $("#bkav_alert_dialog").dialog({
        resizable: false,
        modal: true,
        title: title,
        minWidth: 300,
        width: width,
        closeOnEscape: true
    });
    return false;
}
function bkav_alert_close(isInsideiFrame) {
    if (isInsideiFrame) {
        window.location.reload();
        $("#bkav_alert_dialog").dialog("close");
    }
    else {
        $("#bkav_alert_dialog").dialog("close");
    }
}