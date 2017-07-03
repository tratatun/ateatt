$(function () {
    var applicationInfos = new DevExpress.data.DataSource ({
        load: function () {
            var def = $.Deferred();
            $.getJSON("/Home/GetAppsInfoAsync", {})
                .done(function (result) {
                    if (result.statusCode != 0) {
                        def.reject("Data Loading Error: " + result.description);
                    } else {
                        def.resolve(result.applicationInfoList);
                        $("#clientcomp-header").html("Client computer: " + result.clientComputerName);
                        $("#lastupdated-header").html("Last Updated: " + result.lastUpdatedString);
                    }
                });
            return def.promise();
        }
    });

    var publishers = new DevExpress.data.DataSource({
        load: function () {
            var d = new $.Deferred();
            $.getJSON('/Home/GetPublishersAsync')
                .done(function (result) {
                    if (result) {
                        if (result.statusCode != 0) {
                            d.reject("Data Loading Error: " + result.description);
                        } else {
                            $("#clientcomp-header").html("Client computer: " + result.clientComputerName);
                            $("#lastupdated-header").html("Last Updated: " + result.lastUpdatedString);
                            d.resolve(result.publishers);
                        }
                    }
                });
            return d.promise();
        }
    });

    $("#button-refresh").dxButton({
        text: "Update",
        type: "success",
        width:"100%",
        onClick: function (ev) {
            DevExpress.ui.notify("Updating...");
            var dataGrid = $('#datagrid').dxDataGrid('instance');
            if (dataGrid) {
                dataGrid.refresh();
            }


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

    var viewPort = $(window).width();

    $("#piechart").dxPieChart({
        size: {
            width: viewPort < 400 ? viewPort - 30 : 700
        },
        dataSource: publishers,
        series: {
            argumentField: "publisherName",
            valueField: "numberOfApplications",
            label: {
                visible: true,
                connector: {
                    visible: true,
                    width: 1
                },
                customizeText: function(point) {
                    return point.valueText + " apps";
                }
            },
            smallValuesGrouping: {
                mode: "smallValueThreshold",
                threshold: 2
            }
        },
        title: "Publishers Chart",
        legend: {
            visible: true, 
            horizontalAlignment: viewPort < 400 ? 'center' : 'left',
            

        },
        onPointClick: function (e) {
            var point = e.target;

            toggleVisibility(point);
        },
        onLegendClick: function (e) {
            var arg = e.target;

            toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
        }

    });

    function toggleVisibility(item) {
        if (item.isVisible()) {
            item.hide();
        } else {
            item.show();
        }
    }
});
