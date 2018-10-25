function getitem() {
    var data = {
        id: document.getElementById('ReceiptID').value,
    }
    $.ajax({
        url: '/Receipt_Note/getReceiptDetail/',
        type: 'GET',
        dataType: 'json',
        data: data,
        success: function (data) {
            var result = JSON.parse(data);
            var rows = '';
            var total = 0;
            var count = 999;
            $("#list_item tbody").html('');
            $.each(result, function (i, item) {
                rows += "<tr>   <input type=\"hidden\" id=\"" + count + "\" value=\"" + item.ProductID + "\" />"
                rows += "<td>" + item.ProductID + "</td>"
                rows += "<td>" + item.ProductPrice + " $</td>"
                rows += "<td>" + item.Quality + "</td>"
                rows += "<td>" + item.ProductPrice * item.Quality + " $</td>"
                rows += "<td>  <input type=\"submit\" value=\"Delete\" class=\"btn btn-danger\" onclick=\"DeleteItem(" + count + ");\" />  </td>"
                rows += "</tr>";
                $("#list_item tbody").html(rows);
                total += item.Quality * item.ProductPrice;
                count++;
            });
            var string = "<label class=\"text-info\">Total: " + total + " $</label>"
            $('#total').html(string);
        },
    });
}
function AddNewItemReceipt(a) {
    if (parseInt(document.getElementById('Qua' + a).value) < 0) {
        toastr["warning"]("Quality is not valid", "Errors !!");
        return false;
    }
    var data = {
        receiptnote_id: document.getElementById('ReceiptID').value,
        product_id: document.getElementById(a).value,
        quality: document.getElementById('Qua' + a).value,
    }
    $.ajax({
        url: '/Receipt_Note/AddItem/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            toastr["success"]("Success", "Notification !!");
            getitem();
        },
    });
}
function DeleteItem(a) {
    var data = {
        receiptnote_id: document.getElementById('ReceiptID').value,
        product_id: document.getElementById(a).value,
    }
    $.ajax({
        url: '/Receipt_Note/DeleteItem/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            getitem();
        },
    });
}

function myFunction(a) {
    var img = document.getElementById('IMG' + a);
    var name = document.getElementById('Name' + a);
    var data = {
        product_id: document.getElementById(a).value,
        quality: 1
    }

    $.ajax({
        url: '/Cart/AddItem/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            //Show data in cart mini top right.
            getData();
            //Show dialog notifice        
            toastr.options = {
                "closeButton": true,
                "debug": true,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr["success"]("You have added 1 " + name.value + " <br/> <div align=\"center\"> <img src=\" " + img.value + " \"  width=\"80\" height=\"80\" > </div>", "Congratulation !!");
        },
    });
}
function addnewItem(a) {
    var quality = document.getElementById('Qua' + a);
    var newvalue = quality.value = parseInt(quality.value, 10) + 1;
    changItems(a);
}
function subtractionItem(a) {
    var quality = document.getElementById('Qua' + a);
    if (parseInt(quality.value, 10) > 0)
        var newvalue = quality.value = parseInt(quality.value, 10) - 1;
    changItems(a);
}
function changItems(a) {
    var quality = document.getElementById('Qua' + a);
    var pid = document.getElementById('PID' + a);
    var qualityCheck = document.getElementById('QuaOf' + a);
    if (parseInt(quality.value) < 0 || parseInt(quality.value) > parseInt( qualityCheck.value) ) {
        toastr.options = {
            "closeButton": true,
            "debug": true,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "2000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        toastr["error"]("Quality of product isn't valid.", "Warning !!");
        return false;
    }
    var newvalue = quality.value;
    var data = {
        product_id: pid.value,
        quality: newvalue
    }
    $.ajax({
        url: '/Cart/ChangeItem/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            //Show data in cart mini top right.
            getData();
            //Show dialog notifice  
            ChangeData();
        },
    });
}
function updateMess(a) {
    var data = {
        id: a,
    }
    $.ajax({
        url: '/Admin/Home/updateMess/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {

        },
    });
}
function ChangeData() {
    $.ajax({
        url: '/Cart/getData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var result = JSON.parse(data);
            var rows = '';
            var total = 0;
            $("#totalgood1").html('');
            $.each(result, function (i, item) {
                total += item.Quality * item.ProductPrice;
            });
            rows += "<li> Total money: <span> " + parseFloat(Math.round(total * 100) / 100).toFixed(2) + " $</span></li>";
            rows += "<li>Sales <span> # $</span></li>";
            rows += "<li>Value Added Tax: <span> # $</span></li>";
            rows += "<li>Total Service Charges: <span> # $</span></li>";
            rows += "<li>Total to payment: <span>" + parseFloat(Math.round(total * 100) / 100).toFixed(2) + " $</span></li>";
            $('#totalgood1').html(rows);
        },
    });
}
function getData() {
    $.ajax({
        url: '/Cart/getData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var result = JSON.parse(data);
            var rows = '';
            var total = 0;
            $("#listData tbody").html('');
            $.each(result, function (i, item) {
                rows += "<tr>"
                rows += "<td><img src=" + item.ProductImages + " width=" + 20 + " height=" + 20 + "/></td>"
                rows += "<td>" + item.ProductPrice + " $</td>"
                rows += "<td>" + item.Quality + "</td>"
                rows += "</tr>";
                $("#listData tbody").html(rows);
                total += item.Quality * item.ProductPrice;
            });
            var string = "<label class=\"text-info\">Total: " + parseFloat(Math.round(total * 100) / 100).toFixed(2) + " $</label>"
            $('#total').html(string);
        },
    });
}
function checkPassword() {
    var password = $('#password').val();
    var repassword = $('#repassword').val();
    if (!($.trim(repassword) == $.trim(password))) {
        document.getElementById('classidrepassword').className += ' has-error';
    }
    else {
        document.getElementById('classidrepassword').className = 'control-group';
    }
}
function RemoveItem(a) {
    var pid = document.getElementById('PID' + a);
    var data = {
        product_id: pid.value,
    }
    $.ajax({
        url: '/Cart/RemoveItem/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            //Show data in cart mini top right.
            getData();
            //Show dialog notifice  
            ChangeData();
            $('.' + pid.value).fadeOut('slow', function (c) {
                $('.' + pid.value).remove();
            });
        },
    });
}

function searchProduct(a) {
    var data = {
        key: document.getElementById(a).value,
    }
    $.ajax({
        url: '/Receipt_Note/SearchProduct/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            var result = JSON.parse(data);
            var rows = '';
            var count = 100;
            $("#listProduct tbody").html('');
            $.each(result, function (i, item) {
                rows += "<tr> <input type=\"hidden\" id= " + count + "  value=\"" + item.ProductID + "\" />"
                rows += "<td class=\"center\">" + item.ProductID + "</td>"
                rows += "<td class=\"center\">" + item.ProductName + " </td>"
                rows += "<td class=\"center\">" + item.Quality + "</td>"
                rows += "<td class=\"center\"> <input id=\"Qua" + count + "\" size=\"1\" style=\"font-size:25px;\" value=\"\" /> </td>"
                rows += "<td class=\"center\">  <input type=\"submit\" value=\"Additem\" class=\"btn btn-info\" onclick=\"AddNewItemReceipt(" + count + ");\" />  </td>"
                rows += "</tr>";
                $("#listProduct tbody").html(rows);
                count++;
            });
        },
    });
}
function getTotalRevenue(idType, idListUser, idStart, idEnd, idQuality, idSort) {
    var data = {
        key: idListUser,
        start: idStart,
        end: idEnd,
        quality: idQuality,
        sort: idSort,
    }
    $.ajax({
        url: '/Statistical/getInformationRevenue/',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            $("#TotalSell").html('');
            $("#TotalBuy").html('');
            $("#Revenue").html('');
            $("#TotalOrder").html('');
            var rows1 = "<label class=\"text-success\"> Total Buy: " + data.TotalBuy + " $ </label>";
            var rows2 = "<label class=\"text-success\"> Total Sell: " + data.TotalSell + " $ </label>";
            var rows3 = "<label class=\"text-success\"> Total Revenue: " + data.Revenue + " $ </label>";
            var rows4 = "<label class=\"text-success\"> Total Order: " + data.TotalOrder + " </label> <hr/>";
            $("#TotalSell").html(rows1);
            $("#TotalBuy").html(rows2);
            $("#Revenue").html(rows3);
            $("#TotalOrder").html(rows4);
        },
    });
}
function drawGraph(idType, idListUser, idStart, idEnd, idListProduct, idQuality, idSort) {
    if (idType == 1) {
        var data = {
            key: idListUser,
            start: idStart,
            end: idEnd,
            quality: idQuality,
            sort: idSort,
        }
        $.ajax({
            url: '/Statistical/getRevenue/',
            type: 'POST',
            dataType: 'json',
            data: data,
            success: function (data) {
                var title = "Statistical revenue";
                $("#titleStatistical").html('');
                $("#titleStatistical").html(title);
                getTotalRevenue(idType, idListUser, idStart, idEnd, idQuality, idSort);
                $("#statistical").empty();
                graphArea2 = Morris.Area({
                    element: 'statistical',
                    padding: 10,
                    behaveLikeLine: true,
                    gridEnabled: false,
                    gridLineColor: '#dddddd',
                    axes: true,
                    resize: true,
                    smooth: true,
                    pointSize: 0,
                    lineWidth: 0,
                    fillOpacity: 0.85,
                    data: data,
                    lineColors: ['#eb6f6f'],
                    xkey: 'label',
                    redraw: true,
                    ykeys: ['value'],
                    labels: ['Revenue in day'],
                    pointSize: 2,
                    hideHover: 'auto',
                    resize: true
                });
            },
        });
    }
    else {
        var data = {
            key: idListProduct,
            start: idStart,
            end: idEnd,
            quality: idQuality,
            sort: idSort,
        }
        $.ajax({
            url: '/Statistical/getProductHot/',
            type: 'POST',
            dataType: 'json',
            data: data,
            success: function (data) {
                var title = "Statistical products were sell";
                $("#titleStatistical").html('');
                $("#titleStatistical").html(title);
                $("#TotalSell").html('');
                $("#TotalBuy").html('');
                $("#Revenue").html('');
                $("#TotalOrder").html('');
                $("#statistical").empty();
                graphArea2 = Morris.Bar({
                    element: 'statistical',
                    padding: 10,
                    behaveLikeLine: true,
                    gridEnabled: false,
                    gridLineColor: '#dddddd',
                    axes: true,
                    resize: true,
                    smooth: true,
                    pointSize: 0,
                    lineWidth: 0,
                    fillOpacity: 0.85,
                    data: data,
                    lineColors: ['#eb6f6f'],
                    xkey: 'label',
                    redraw: true,
                    ykeys: ['value'],
                    labels: ['Quality product'],
                    hideHover: 'auto',
                    resize: true
                });
            },
        });
    }
}
function inputDraw() {
    var idType = document.getElementById('idType').value;
    var idQuality = document.getElementById('idQuality').value;
    var idStart = document.getElementById('idStart').value;
    var idEnd = document.getElementById('idEnd').value;
    var idListUser = document.getElementById('idListUser').value;
    var idListProduct = document.getElementById('idListProduct').value;
    var idSort = document.getElementById('idSort').value;
    if (idStart == "" || idEnd == "") {
        idStart = "1/1/1990";
        idEnd = "1/1/2020";
    }
    drawGraph(idType, idListUser, idStart, idEnd, idListProduct, idQuality, idSort);
}
function checkValidation(a) {
    if (document.getElementById(a).value == "") {
        document.getElementById('class' + a).className += " has-error";
        return false;
    }
    else {
        document.getElementById('class' + a).className = "control-group";
    }
}
function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!re.test(email))
        document.getElementById('classemail').className += " has-error";
    else
        document.getElementById('classemail').className = "control-group";
}