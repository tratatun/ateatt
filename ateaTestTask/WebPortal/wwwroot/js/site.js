$(function() {
    var applicationInfos = new DevExpress.data.DataSource ({
        load: function(options) {
            var def = $.Deferred();
            $.getJSON("Home/GetDataAsync?computerName=ttn", {})
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

    $("#datagrid").dxDataGrid({
        dataSource: applicationInfos,
        columns: [
            {
                dataField: "displayName",
                
            }, {
                dataField: "publisherName",
                
            }, {
                dataField: "installDateString", 
                caption:"Install Date",
                dataType:"date"

            }, {
                dataField: "displayVersion",
            }
        ],
        paging: {
            pageSize: 12
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [8, 12, 20]
        }
    });
});