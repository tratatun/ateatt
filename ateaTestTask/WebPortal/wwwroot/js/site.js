$(function() {
    var applicationInfos = {};

    $("datagrid").dxDataGrid({
        dataSource: "Home/GetDataAsync?computerName=ttn",
        columns: ["publisherName","applicationInfos"]
    });

});