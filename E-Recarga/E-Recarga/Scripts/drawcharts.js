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

    showDefaultText(chart, "dados inexistentes");

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

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}


function DrawRevenueChart(data) {
    var chart = new CanvasJS.Chart("globalRevenueChart", {
		animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            text: "Rendimento Anual"
        },
		axisX: {
			labelFontColor: "#717171",
			labelFontSize: 16,
			lineColor: "#a2a2a2",
			minimum: new Date("1 Jan 2015"),
			tickColor: "#a2a2a2",
			valueFormatString: "MMM YYYY"
		},
		axisY: {
			gridThickness: 0,
			includeZero: false,
			labelFontColor: "#717171",
			labelFontSize: 16,
			lineColor: "#a2a2a2",
			prefix: "$",
			tickColor: "#a2a2a2"
		},
		toolTip: {
			borderThickness: 0,
			cornerRadius: 0,
			fontStyle: "normal"
		},
		data: [
			{
				color: "#393f63",
				markerSize: 0,
				type: "spline",
				yValueFormatString: "$###,###.##",
				dataPoints: data,
			}
		]
    });

    showDefaultText(chart,"dados inexistentes");

    chart.render();

}

function DrawRevenueChart(data) {
    var chart = new CanvasJS.Chart("globalRevenueChart", {
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            text: "Rendimento Médio Mensal"
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

            title: "Lucro Bruto"
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
            text: "Lucro por dias"
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

    showDefaultText(chart, "dados inexistentes");

    chart.render();
}

function showDefaultText(chart, text) {

    var isEmpty = !(chart.options.data[0].dataPoints && chart.options.data[0].dataPoints.length > 0);

    if (!chart.options.subtitles)
        (chart.options.subtitles = []);

    if (isEmpty)
        chart.options.subtitles.push({
            text: text,
            verticalAlign: 'center',
        });
    else
        (chart.options.subtitles = []);
}