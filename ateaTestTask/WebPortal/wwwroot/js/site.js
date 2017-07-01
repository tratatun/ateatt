$(function() {
    var applicationInfos = {};
    $.ajax({
        url: "Home/GetDataAsync",
        contentType: "application/json",
        data: { computerName: "ttn" },
        success: function(data) {
            $("datagrid").dxDataGrid({
                dataSource: data.publishers,
                columns: ["publisherName", "applicationInfos"]
            });
        },
        error:function(err) {
            $("datagrid").dxDataGrid({
                columns: ["publisherName", "applicationInfos"]
            });
        }
    });
    

});