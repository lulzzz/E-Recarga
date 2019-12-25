﻿function DrawOverviewPieChart (id, data) {
    var chart = new CanvasJS.Chart("overviewchart__" + id, {
        animationEnabled: true,
        title: {
            text: "Marcações/Hora",
            fontSize: 22,
            fontColor: "#606060",
            fontFamily: "Calibri",
            fontWeight: "bold",
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}


function DrawProfitPieChart(id, data) {
    var chart = new CanvasJS.Chart("monthlyProfitChart__" + id, {
        animationEnabled: true,
        title: {
            text: "Lucro/Dia da semana",
            fontSize: 22,
            fontColor: "#606060",
            fontFamily: "Calibri",
            fontWeight: "bold",
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}

function DrawRevenueChart(data) {
    var chart = new CanvasJS.Chart("globalRevenueChart", {
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            text: "Rendimento Médio Mensal",
            fontSize: 24,
            fontColor: "#606060",
            fontFamily: "Calibri",
            fontWeight:"bold"
        },
        animationEnabled: true,
        axisX: {
            labelFontColor: "#717171",
            labelFontSize: 16,
            lineColor: "#a2a2a2",
            tickColor: "#a2a2a2",
        },
        axisY: {
            gridThickness: 0,
            includeZero: false,
            labelFontColor: "#717171",
            labelFontSize: 16,
            lineColor: "#a2a2a2",
            prefix: "€",
            tickColor: "#a2a2a2",

            title: "Rendimento"
        },
        toolTip: {
            borderThickness: 0,
            cornerRadius: 0,
            fontStyle: "normal"
        },
        data: [
            {
                toolTipContent: "{y}",
                color: "#393f63",
                markerSize: 0,
                type: "splineArea",
                yValueFormatString: "€###,###.##",
                markerSize: 5,
                color: "rgba(33,33,242,0.72)",
                dataPoints: data,
            }
        ]
    });

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}

function DrawDaysOverviewPieChart(id, data) {
    var chart = new CanvasJS.Chart(id, {
        animationEnabled: true,
        title: {
            text: "Lucro /dia da semana",
            fontSize: 24,
            fontColor: "#606060",
            fontFamily: "Calibri",
            fontWeight: "bold"
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "€##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}

function DrawPodUsagePieChart(id, data) {
    var chart = new CanvasJS.Chart(id, {
        animationEnabled: true,
        title: {
            text: "Detalhes dos Postos"
        },
        data: [{
            type: "pie",
            startAngle: 240,
            yValueFormatString: "##0.00",
            indexLabel: "{label} {y}",
            dataPoints: data,
        }]
    });

    showDefaultText(chart, "Dados Inexistentes");

    chart.render();
}

function showDefaultText(chart, text) {

    var isEmpty = !(chart.options.data[0].dataPoints && chart.options.data[0].dataPoints.length > 0);

    if (!chart.options.subtitles)
        (chart.options.subtitles = []);

    if (isEmpty)
        chart.options.subtitles.push({
            text: text,
            fontSize: 18,
            fontColor: "#606060",
            fontFamily: "Calibri",
            verticalAlign: 'center',
        });
    else
        (chart.options.subtitles = []);
}