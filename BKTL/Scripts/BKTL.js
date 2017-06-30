function keyCodeNumber(id) {
    $("#" + id).keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}
function ConvertMoney(strMoney) {
    return strMoney == "" ? 0 : parseFloat(strMoney.replace(/,/g, ''))
}
function ConvertStringMoneyToNumber(number) {
    return number.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + " VNĐ";
}
function ConvertStringToDate(strDate) {//dd/MM/YYYY
    var arrBirthDate = strDate.split('/');
    return new Date(arrBirthDate[2] + '/' + arrBirthDate[1] + '/' + arrBirthDate[0]);
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
String.prototype.reverse = function () { return this.split("").reverse().join(""); }
function reformatText(input) {
    var x = input.value;
    x = x.replace(/,/g, ""); // Strip out all commas
    x = x.reverse();
    x = x.replace(/.../g, function (e) {
        return e + ",";
    }); // Insert new commas
    x = x.reverse();
    x = x.replace(/^,/, ""); // Remove leading comma
    input.value = x;
}
//Hàm để xuất Excel
function ExportExcel(tableNames, headerbdColor, fileName) {
    LoadExcel();
    if (tableNames.trim() === "") {
        alert("Không có bảng được chọn");
        return;
    }
    if (headerbdColor.trim() === "") {
        headerbdColor = "#87AFC6";
    }
    var export_data = "";
    var arrTableNames = tableNames.split("|");
    if (arrTableNames.length > 0) {
        for (var i = 0 ; i < arrTableNames.length ; i++) {
            export_data += "<table border='2px'><tr bgcolor='" + headerbdColor + "'>";
            objTable = document.getElementById(arrTableNames[i]); // table to export
            if (objTable === undefined) {
                alert("Table not found!");
                return;
            }
            for (var j = 0 ; j < objTable.rows.length ; j++) {
                export_data += objTable.rows[j].innerHTML + "</tr>";
            }
            export_data += "</table>";
        }
        export_data = export_data.replace(/<A[^>]*>|<\/A>/g, "");
        export_data = export_data.replace(/<img[^>]*>/gi, "");
        export_data = export_data.replace(/<input[^>]*>|<\/input>/gi, "");
    }
    else {
        alert("No table supplied to export data!");
        return;
    }
    if (window.navigator.userAgent.indexOf("MSIE ") > 0 || !!window.navigator.userAgent.match(/Trident.*rv\:11\./)) {
        exportIF.document.open("txt/html", "replace");
        exportIF.document.write(export_data);
        exportIF.document.close();
        exportIF.focus();
        sa = exportIF.document.execCommand("SaveAs", true, fileName + ".xls");
    }
    else //other browsers : Chrome/FireFox (Supported Data URIs)
    {
        var d = $.datepicker.formatDate('dd/mm/yy', new Date());
        a = document.createElement("a");
        a.download = fileName + '(' + d + ')';
        a.href = 'data:application/vnd.ms-excel,' + encodeURIComponent(export_data);
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
    //return (sa);
}