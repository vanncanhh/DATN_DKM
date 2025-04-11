﻿
// SCRIPT FOR PAGE : DEVICE
// Thanh lý thiết bị 
var Confim1 = function () {
    var chkArray = [];
    var status = 0;
    / look for all checkboes that have a class 'chk' attached to it and check if it was checked /
    var table = $('#example').dataTable()
    table.$('input[type="checkbox"]').each(function () {
        if (this.checked) {
            chkArray.push($(this).val());
            if ($(this).data("status") == 1) {
                status = 1;
            }
            if ($(this).data("status") == 2) {
                status = 2;
            }
            if ($(this).data("status") == 3) {
                status = 3;
            }
        }
    });

    / we join the array separated by the comma /
    var selected;
    selected = chkArray.join(',');
    / check if there is selected checkboxes, by default the length is 1 as it contains one single comma /
    if (selected.length > 0) {
        if (status == 1) {
            alert("Có những thiết bị đang trong dự án không thể thanh lý!");
        }
        else if (status == 2) {
            alert("Có những thiết bị đã được thanh lý!");
        }
        else if (status == 3) {
            alert("Có những thiết bị đang sửa chữa không thể thanh lý!");
        }
        else {
            $("#hiddenId1").val(chkArray);
            $("#Liquidation").modal('show');
            $('#btnLiquidation').click(function () {
                $("#loaderDiv").show();
                var dvId = $("#hiddenId1").val();
                $.ajax({
                    type: "POST",
                    url: "/Device/LiquidationDevice",
                    data: { Id: dvId },
                    success: function (result) {
                        if (result) {
                            $("#loaderDiv").hide();
                            $("#Liquidation").modal("hide");
                            location.reload();
                        } else {

                            $("#loaderDiv").hide();
                            $("#Liquidation").modal("hide");
                            alert("Thanh lý thiết bị lỗi.Tồn tại thiết bị con nằm trong thiết bị cha");
                        }
                    }
                })
            });
        }
    }
}

//Trả thiết bị 
var ConfimReturn = function () {
    var chkArray = [];
    / look for all checkboes that have a class 'chk' attached to it and check if it was checked /
    var table = $('#example').dataTable()
    table.$('input[type="checkbox"]').each(function () {
        if (this.checked) {
            chkArray.push($(this).val());
        }
    });
    / we join the array separated by the comma /
    if (chkArray.length > 0) {
        $("#hiddenId1").val(chkArray);
        $("#Returnproject").modal('show');
        $('#btnreturndevice').click(function () {
            $("#loaderDiv").show();
            var dvId = $("#hiddenId1").val();
            $.ajax({
                type: "POST",
                url: "/Device/ReturnDeviceProject",
                data: { Id: dvId },
                success: function (result) {
                    if (result) {
                        $("#loaderDiv").hide();
                        $("#Returnproject").modal("hide");
                        location.reload();
                    } else {

                        $("#loaderDiv").hide();
                        $("#Returnproject").modal("hide");
                        alert("Trả thiết bị lỗi. Tồn tại thiết bị con nằm trong thết bị cha");
                    }
                }
            })
        })
    }
}

//Xóa thiết bị 
var ConfirmDelete1 = function () {
    var chkArray = [];
    var status = 0;
    / look for all checkboes that have a class 'chk' attached to it and check if it was checked /
    $(".check:checked").each(function () {
        chkArray.push($(this).val());
        if ($(this).data("status") == 1) {
            status = 1;
        }
        if ($(this).data("status") == 3) {
            status = 3;
        }
    });
    / we join the array separated by the comma /
    var selected;
    selected = chkArray.join(',');
    / check if there is selected checkboxes, by default the length is 1 as it contains one single comma /
    if (selected.length > 0) {
        if (status == 1) {
            alert("Có những thiết bị đang trong dự án không thể xóa!");
        }
        else if (status == 3) {
            alert("Có những thiết bị đang sửa chữa không thể xóa!");
        }
        else {
            $("#hiddenId1").val(chkArray);
            $("#myModaldelete1").modal('show');
            $('#btnContinueDelete1').click(function () {
                $("#loaderDiv").show();
                var dvId = $("#hiddenId1").val();
                $.ajax({
                    type: "POST",
                    url: "/Device/DeleteDevice",
                    data: { Id: dvId },
                    success: function (result) {
                        if (result) {
                            $("#loaderDiv").hide();
                            $("#myModaldelete1").modal("hide");
                            location.reload();
                        } else {

                            $("#loaderDiv").hide();
                            $("#myModaldelete1").modal("hide");
                            alert("Xóa thiết bị lỗi. Tồn tại thiết bị con nằm trong thiết bị khác");
                        }
                    }
                })
            });
        }
    }
}

// Script Chạy trc khi load HTML
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tabledvdiv tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    var table = $('#example').dataTable({
        "bFilter": false,
        "bLengthChange": false,
        "iDisplayLength": 25,
        "oLanguage": {
            "sInfo": "Hiển thị từ _START_ đến _END_ của _TOTAL_ thiết bị",
            "oPaginate": {
                "sPrevious": "Trước",
                "sNext": "Tiếp",
            }
        },
        buttons: [
          'print'
        ],
        "aoColumnDefs": [
            { "aTargets": [0, 10, 11], bSortable: false },
        ]
    });
    $("#checkAll1").click(function () {
        var rows = table.$('tr', { search: 'applied' });
        if ($('input:checked', rows).length == rows.length) {
            $('input[type="checkbox"]', rows).prop('checked', false);
        }
        else {
            $('input[type="checkbox"]', rows).prop('checked', true);
        }
    });
    $("body").on("change", "input", function () {
        var rows = table.$('tr', { search: 'applied' });
    });
});
//function fnExcelReport() {
//    var TypeOfDevice = $('#TypeOfDevice').val();
//    var Status = $('#Status').val();
//    var Guarantee = $('#Guarantee').val();
//    var ProjectDKC = $('#ProjectDKC').val();
//    var DeviceCode = $('#DeviceCode').val();
//    $.ajax({
//        url: "/Device/ExportToExcel",
//        data: {
//            TypeOfDevice: TypeOfDevice,
//            Status: Status,
//            Guarantee: Guarantee,
//            Project: ProjectDKC,
//            DeviceCode: DeviceCode
//        },
//        success: function (response) {
//            response = response.replace("DeviceCode", "Mã Thiết Bị ");
//            response = response.replace("DeviceName", "Tên Thiết Bị");
//            response = response.replace("TypeName", "Tên Loại");
//            response = response.replace("PriceOne", "Giá");
//            response = response.replace("FullName", "Tên Người Dùng");
//            //  response = response.replace("Configuration", "Cấu Hình");

//            response = response.replace("Name", "Nhà Cung Cấp");

//            response = response.replace("ProjectSymbol", "Mã Dự Án");
//            response = response.replace("Status", "Trạng Thái");
//            //  response = response.replace(/&lt;p&gt;/gi, "");
//            //   response = response.replace(/&lt;\/p&gt;/gi, " ");
//            // response = response.replace(/&nbsp;/gi, " ");
//            var ua = window.navigator.userAgent;
//            var msie = ua.indexOf("MSIE ");
//            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:s11\./))      // If Internet Explorer
//            {
//                txtArea1.document.open("txt/html", "replace");
//                txtArea1.document.write(response);
//                txtArea1.document.close();
//                txtArea1.focus();
//                sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
//            }
//            else                 //other browser not tested on IE 11
//            {
//                console.log(response);
//                var a = 1;
//                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(response));
//            }
//        }
//    })
//}

// In Table 
function printDiv() {
    var divToPrint = document.getElementById('example');
    newWin = window.open("");
    newWin.document.write(divToPrint.outerHTML);
    newWin.print();
    newWin.close();
}

// Cấu hình Tạo Barcode (In mã vạch cho thiết bị )
$('#ConfimprintImg').click(function () {
    var chkArray = [], IdArray = [];
    var table = $('#example').dataTable();

    table.$('input[type="checkbox"]').each(function () {
        if (this.checked) {
            chkArray.push($(this).data("code").trim());
            IdArray.push($(this).val());
        }
    });

    if (chkArray.length > 0) {
        $("#hiddenId").val(IdArray);
        $("#DeviceBarCode").val(chkArray);

        $.ajax({
            type: "POST",
            url: "/Device/GenerateBarCodeDevice",
            data: {
                dvcode: $("#DeviceBarCode").val(),
                dvid: $("#hiddenId").val()
            },
            async: false,
            success: function (response) {
                var data = response.list;
                var listdv = response.listdv;
                var _url = '<body>';

                for (var i = 0; i < data.length; i++) {
                    var DeviceName = listdv[i].deviceName || 'N/A';
                    var DeviceModel = listdv[i].typeName || 'N/A';
                    var DeviceCode = listdv[i].deviceCode || 'N/A';
                    var Price = listdv[i].priceOne || 'N/A';
                    var Manufacturer = listdv[i].name || 'N/A';
                    var rawDateAdded = listdv[i].createdDate;
                    var DateAdded = rawDateAdded ? dateFormat(new Date(rawDateAdded)) : 'N/A';
                    var url = data[i];

                    _url += '<div style="padding-top:23px;margin-right:30px;overflow:hidden;display:inline-block;">' +
                        '<table border="1" style="display:inline-block;width:250px;height:170px;">' +
                        '<tbody>' +
                        '<tr style="font-weight:bold;text-align:center;border:none;"><td>Tem thông tin thiết bị</td></tr>' +
                        '<tr style="border:none;"><td style="padding:3px;font-size:12px;">Tên thiết bị: ' + DeviceName + '</td></tr>' +
                        '<tr style="border:none;"><td>' +
                        '<table style="width:100%;"><tbody><tr>' +
                        '<td style="width:150px;border-right:1px solid black;padding-top:0px;font-size:12px;">' +
                        'Loại thiết bị: ' + DeviceModel + '<br><hr style="margin:1px;">' +
                        'Mã code: ' + DeviceCode + '<br><hr style="margin:1px;">' +
                        'Giá: ' + Price + '<br><hr style="margin:1px;">' +
                        'Nhà sản xuất: ' + Manufacturer + '<br><hr style="margin:1px;">' +
                        'Ngày nhập: ' + DateAdded + '</td>' +
                        '<td style="width:100px;border-width:1px;"><img src="' + url + '" style="width:90px;height:90px;"></td>' +
                        '</tr></tbody></table></td></tr></tbody></table></div>';
                }

                _url += '</body>';

                function dateFormat(d) {
                    return ((d.getDate() + "").padStart(2, "0")) + "/" + ((d.getMonth() + 1) + "").padStart(2, "0") + "/" + d.getFullYear();
                }

                var print = _url.replace(/null|undefined/gi, "N/A");
                var win = window.open("");
                win.document.write('<html><head><title>Print</title><style>#DetailBarCode label{font-family:sans-serif;font-size:x-small;} #DetailBarCode1 label{font-family:sans-serif;font-size:xx-small;}</style>');
                win.document.write('</head><body>' + print + '</body></html>');
                win.onload();
                win.focus();
            }
        });
    }
});
