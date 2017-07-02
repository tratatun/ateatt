$(function () {
    var publishers = [];
    var applicationInfos = new DevExpress.data.DataSource ({
        load: function () {
            var def = $.Deferred();
            $.getJSON("/Home/GetAppsInfoAsync?computerId=2", {})
                .done(function (result) {
                    if (result.statusCode != 0) {
                        def.reject("Data Loading Error: " + result.description);
                    } else {
                        def.resolve(result.applicationInfoList);

                    }
                });
            return def.promise();
        }
    });

    $("#button-clientcomputer").dxButton({
        text: "Update",
        type: "success",
        width:"40%",
        onClick: function (ev) {
            DevExpress.ui.notify("Updating...");
            var dataGrid = $('#datagrid').dxDataGrid('instance');
            dataGrid.refresh();
        }
    });

    $("#datagrid").dxDataGrid({
        dataSource: applicationInfos,
        columnHidingEnabled: true,
        columns: [
            {
                dataField: "displayName",
                width: '40%'
            }, {
                dataField: "publisherName",
                width: 230
            }, {
                dataField: "installDateString", 
                caption:"Install Date",
                dataType: "date",
                width: 100
            }, {
                dataField: "displayVersion",
                width: 100
            }
        ],
        paging: {
            pageSize: 12
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [12, 24]
        }
    });

    $("#piechart").dxPieChart({
        size: {
            width:600
        },
        dataSource: publishers,
        series: [
            {
                argumentField: "publisherName",
                valueField: "installDateString",
                label: {
                    visible: true,
                    connector: {
                        visible: true,
                        width: 1
                    }
                }
            }
        ],
        title: "Publishers",
    });

});
