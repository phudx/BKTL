function ShowMessage(title, content, width) {
    $(document.body).append("<div id='DialogID' style='display: none'><p id='DialogContent'></p></div>");
    if (width == 'undefined' || width == null)
        width = 550;
    if (title == 'undefined' || title == null)
        title = "Thông báo";
    $("#DialogContent").html(content);
    $('#DialogID').dialog({
        title: "Thông báo",
        autoOpen: true,
        resizable: false,
        modal: true,
        height: 'auto',
        width: width,
        buttons: [{
            text: "Đóng",
            'class': "button-close btn btn-primary",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
}
function ShowForm(idDialog, autoOpen, pTitle, pWidth, pHeight, functName, functUpdate, obj) {
    if (pWidth == 'undefined' || pWidth == null)
        pWidth = 550;
    if (pHeight == 'undefined' || pHeight == null)
        pWidth = 'auto';
    $("#" + idDialog).dialog({
        title: pTitle,
        autoOpen: autoOpen,
        resizable: false,
        modal: true,
        overflow: 'auto',
        height: pHeight,
        width: pWidth,
        minHeight: 10,
        buttons: [{
            text: functName,
            'class': "button-update",
            click: function () {
                functUpdate(obj);
                $(this).dialog("close");
            }
        },
        {
            text: "Đóng",
            'class': "button-close",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
}
