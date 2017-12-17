function addRecord() {
    var record = $("#recordLocator").val();
    var passenger = $("#passenger").val();
    var fileId = readCookie('fileId');
    $.ajax({
        type: "Post",
        url: "SaveRecord",
        data: { FileId: fileId, RecordLocator:record, Passengers:[passenger]},
        success: function (data) {            
            $("#addResult").html('record added');
        },
        error: function (error) {
            // handle error
        },
    });
}
$("#recordLocator").on('change', function () {
    var ok = $("#recordLocator").val().length == 6;
    if (ok)
        $("#addRecord").removeAttr("disabled")
    else
        $("#addRecord").attr("disabled","disabled")
  
});
$("#addRecord").on('click', function () {
    addRecord();
});

function searchRecord(id) {
    $.ajax({
        type: "Get",
        url: "SearchRecords?fileId=" + id +"&keyword=" + $("#search").val(),

        success: function (data) {
            console.log(data);
            var div = '';
            data.forEach(function (x) {
                div += '<div><strong>' + x.RecordLocator + '</strong><br />';
                x.Passengers.forEach(function (y) {
                    div = div + y + '<br />';
                });
            });
            $("#recordResult").html(div);
        },
        error: function (error) {
            // handle error
        },
    });
}
var id = readCookie('fileId');

$("#searchRecord").on('click', function () {
    if(id)
        searchRecord(id);
});
$("#downloadRecord").on('click', function () {
    if (id) {
        window.open("DownloadFile?fileId=" + id);
    }
});

function getRecord(id) {
    $.ajax({
        type: "Get",
        url: "GetRecords?fileId="+ id,
       
        success: function (data) {
            console.log(data);
            var div = '';
            data.forEach(function (x) {
                div += '<div><strong>' + x.RecordLocator + '</strong><br />';
                x.Passengers.forEach(function (y) {
                    div = div + y + '<br />';
                });                
            });
            $("#recordResult").html(div);
        },
        error: function (error) {
            // handle error
        },        
    });
}


if (id) {
    getRecord(id);
}
    
