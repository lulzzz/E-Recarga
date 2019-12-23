function DrawOverviewPieChart (id, data) {
    var chart = new CanvasJS.Chart("overviewchart__" + id, {
        animationEnabled: true,
            title: {
            text: "Marcações/Hora"
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    chart.render();
}


function DrawProfitPieChart(id, data) {
    var chart = new CanvasJS.Chart("monthlyProfitChart__" + id, {
        animationEnabled: true,
        title: {
            text: "Lucro/Dia"
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    chart.render();
}