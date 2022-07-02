
Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    var today = new Date(this.getFullYear(), this.getMonth(), this.getDate());
    var dayOfYear = ((today - onejan + 1) / 86400000);
    return Math.ceil(dayOfYear / 7)
};
$(document).ready(function () {

    var minute = new Date().getMinutes(),
        nextRefresh = (30 - (minute % 30)) * 60 * 1000;

    setTimeout(function () { location.reload(); }, nextRefresh);

    //$(document).on('click', '.Weeklysummary', function () {
    //    var flag = $(this).attr('aria-expanded');
    //    if (flag == "true") {
           
    //    }

    //});

    ////$(document).on('click', '.GlobalSales', function () {
    ////    var flag = $(this).attr('aria-expanded');
    ////    if (flag == "true") {
    ////        $('.GlobalSalesdata').load('/home/globalsales', function () {

    ////        });
    ////    }

    ////});

    //$(document).on('click', '.Visualization', function () {
    //    var flag = $(this).attr('aria-expanded');
    //    if (flag == "true") {
    //        $('.Visualizationdata').load('/home/visualization', function () {

    //        });
    //    }

    //});
    //$(document).on('click', '.BillingOffice', function () {
    //    var flag = $(this).attr('aria-expanded');
    //    if (flag == "true") {
    //        $('.BillingOfficedata').load('/home/billingoffice', function () {

    //        });
    //    }

    //});



    ToDatatable();

    var labelsarray = [];
    var seriesarray = [];
    $.ajax({
        type: "POST",
        url: "/home/ClientVisualizationByCountry",
        //data: "",
        dataType: "JSON",
        success: function (data) {

            $.each(data.data, function (index, obj) {

                labelsarray.push(obj.country);
                seriesarray.push(obj.total);

            });

            var data = {
                labels: labelsarray,//['Jan', 'Feb', 'Mar', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                series: [
                    seriesarray
                    //[5, 4, 3, 7, 5, 10, 3, 4, 8, 10, 6, 8]
                ]
            };

            var options = {
                seriesBarDistance: 30,
                //width: 300,
                height: 200
            };

            var responsiveOptions = [
                ['screen and (min-width: 641px) and (max-width: 1024px)', {
                    seriesBarDistance: 20,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value;
                        }
                    }
                }],
                ['screen and (max-width: 640px)', {
                    seriesBarDistance: 10,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value[0];
                        }
                    }
                }]
            ];

            new Chartist.Bar('.clientsBarChart', data, options, responsiveOptions);
        },
        error: function () {
            alert("error");
        }
    });

    var labelsarray1 = [];
    var seriesarray1 = [];
    $.ajax({
        type: "POST",
        url: "/home/JobsVisualizationByCountry",
        //data: "",
        dataType: "JSON",
        success: function (data) {

            $.each(data.data, function (index, obj) {

                labelsarray1.push(obj.country);
                seriesarray1.push(obj.total);

            });

            var data = {
                labels: labelsarray1,//['Jan', 'Feb', 'Mar', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                series: [
                    seriesarray1
                    //[5, 4, 3, 7, 5, 10, 3, 4, 8, 10, 6, 8]
                ]
            };

            var options = {
                seriesBarDistance: 30,
                //width: 300,
                height: 200
            };

            var responsiveOptions = [
                ['screen and (min-width: 641px) and (max-width: 1024px)', {
                    seriesBarDistance: 20,
                    //distributeSeries: true,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value;
                        }
                    }
                }],
                ['screen and (max-width: 640px)', {
                    seriesBarDistance: 10,
                    // distributeSeries: true,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value[0];
                        }
                    }
                }]
            ];

            new Chartist.Bar('.jobsBarChart', data, options, responsiveOptions);
        },
        error: function () {
            alert("error");
        }
    });

    var labelsarray2 = [];
    var seriesarray2 = [];
    $.ajax({
        type: "POST",
        url: "/home/ProductVisualizationByCountry",
        //data: "",
        dataType: "JSON",
        success: function (data) {

            $.each(data.data, function (index, obj) {

                labelsarray2.push(index);
                seriesarray2.push(obj);

            });

            var data = {
                labels: labelsarray2,//['Jan', 'Feb', 'Mar', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                series: [
                    seriesarray2
                    //[5, 4, 3, 7, 5, 10, 3, 4, 8, 10, 6, 8]
                ]
            };

            var options = {
                seriesBarDistance: 30,
                //width: 300,
                height: 200
            };

            var responsiveOptions = [
                ['screen and (min-width: 641px) and (max-width: 1024px)', {
                    seriesBarDistance: 20,
                    //distributeSeries: true,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value;
                        }
                    }
                }],
                ['screen and (max-width: 640px)', {
                    seriesBarDistance: 10,
                    // distributeSeries: true,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value[0];
                        }
                    }
                }]
            ];

            new Chartist.Bar('.productionBarChart', data, options, responsiveOptions);
        },
        error: function () {
            alert("error");
        }
    });

    //var labelsarray2 = [];
    //var seriesarray2 = [];
    var data = {
        labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        series: [
            [5000, 4000, 3000, 7000, 5000, 10000, 3000, 4000, 8000, 1000, 6000, 8000]
        ]
    };

    var options = {
        seriesBarDistance: 30,
        //width: 300,
        height: 200
    };

    var responsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
            seriesBarDistance: 20,
            //distributeSeries: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value;
                }
            }
        }],
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 10,
            // distributeSeries: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];

    new Chartist.Bar('.weeklyBarChart', data, options, responsiveOptions);

});
function dateConvert(dateobj, format) {
    var year = dateobj.getFullYear();
    var month = ("0" + (dateobj.getMonth() + 1)).slice(-2);
    var date = ("0" + dateobj.getDate()).slice(-2);
    var hours = ("0" + dateobj.getHours()).slice(-2);
    var minutes = ("0" + dateobj.getMinutes()).slice(-2);
    var seconds = ("0" + dateobj.getSeconds()).slice(-2);
    var day = dateobj.getDay();
    var months = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
    var dates = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var converted_date = "";

    switch (format) {
        case "YYYY-MM-DD":
            converted_date = year + "-" + month + "-" + date;
            break;
        case "YYYY-MMM-DD DDD":
            converted_date = dates[parseInt(day)] + ", " + months[parseInt(month) - 1] + " " + date + ", " + year;
            break;
    }

    return converted_date;
}

function ToDatatable() {

    var url = '/home/GetWeeklySummaryList';

    var table = $('#WeeklySummaryTable').DataTable({
        "processing": true,
        "bServerSide": true,
        "bSort": true,
        "sAjaxSource": url,
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
        ],
        "iDisplayLength": 10,
        "bLengthChange": true,
        "bDestroy": true,
        "sEmptyTable": "Loading data from server",
        "searching": true,
        "paging": true,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": true,
        "columnDefs": [
            { width: '20%', targets: 0 }
        ],
        "fnServerData": function (sSource, aoData, fnCallback) {
            $.ajax({
                "dataType": 'json',
                "type": "POST",
                "url": sSource,
                "data": aoData,
                "success": fnCallback
            });
        },
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            $(nRow).attr('id', aData[0]) // or whatever you choose to set as the id
        },
        //"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            var data = data;

            var totalcol8 = 0;
            data.forEach(x => totalcol8 += parseFloat(x[7]))

            var totalcol9 = 0;
            data.forEach(x => totalcol9 += parseFloat(x[8]))

            var totalcol10 = 0;
            data.forEach(x => totalcol10 += parseFloat(x[9]))

            var totalcol11 = 0;
            data.forEach(x => totalcol11 += parseFloat(x[10]))

            var totalcol12 = 0;
            data.forEach(x => totalcol12 += parseFloat(x[11]))
            // Update footer

            var today = new Date();
            var weekno = today.getWeek();
            $(api.column(0).footer()).html(
                "Week " + weekno + " " + dateConvert(today, "YYYY-MMM-DD DDD")
            );

            $(api.column(7).footer()).html(
                totalcol8.toFixed(0)
            );
            $(api.column(8).footer()).html(
                totalcol9.toFixed(0)
            );
            $(api.column(9).footer()).html(
                '$' + totalcol10.toFixed(2)
            );
            $(api.column(10).footer()).html(
                '$' + totalcol11.toFixed(2)
            );
            $(api.column(11).footer()).html(
                '$' + totalcol12.toFixed(2)
            );


        },


        //},
        "aoColumns": [
            {
                "width": "5%",
                "name": "InvoiceID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[0]
                }
            },
            {
                "width": "5%",
                "name": "InvoiceDate",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[1]
                }
            },
            {
                "width": "10%",
                "name": "Notes",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "width": "5%",
                "name": "Date Paid",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[3]
                }
            },
            {
                "width": "5%",
                "name": "Engagement",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "width": "10%",
                "name": "Billing Conatct",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "width": "5%",
                "name": "Product",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "width": "5%",
                "name": "document",
                "orderable": false,
                "class": "text-center",
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "width": "5%",
                "name": "pages",
                "orderable": false,
                "class": "text-center",
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },
            {
                "width": "5%",
                "name": "Billed",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[9]).toFixed(2);
                }
            },
            {
                "width": "10%",
                "name": "netwithhst",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[10]).toFixed(2);
                }
            },
            {
                "width": "10%",
                "name": "hstamount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[11]).toFixed(2);
                }
            },
            {
                "width": "5%",
                "name": "currency",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[12]
                }
            },
            {
                "width": "10%",
                "name": "ForeginCurrencyValue",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[13]).toFixed(2);
                }
            },
        ]
    });
}