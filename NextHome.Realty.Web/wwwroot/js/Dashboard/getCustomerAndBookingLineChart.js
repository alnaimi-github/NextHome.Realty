
$(document).ready(function () {
    loadCustomerAndBookingLineChart();
});

function loadCustomerAndBookingLineChart() {
    $(".chart.spinner").show();

    $.ajax({
        url: "/Dashboard/GetMemberAndBookingLineChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
         
            loadLineChart("newMembersAndBookingsLineChart", data);

            $(".chart.spinner").hide();
        }

    })
}


function loadLineChart(id, data) {
    var chartColors = getChartColorsArray(id);
    var options = {
        series: data.series,
        colors: chartColors,
        chart: {
            height: 350,
            type: 'line'
        },
        stroke: {
            curve: 'smooth',
            width:2
        },
        markers: {
            size: 3,
            strokewidth: 0,
            hover: {
                size: 7
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#ddd"
                }
            }
        },
        xaxis: {
            labels: {
                style: {
                    colors: "#fff"
                }
            }
        },
        lengend: {
            labels: {
                colors: "#fff"
            }
        }
    };
    var chart = new ApexCharts(document.querySelector("#" + id), options);
        chart.render();
}