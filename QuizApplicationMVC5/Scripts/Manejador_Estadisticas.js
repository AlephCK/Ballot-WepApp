var ctx = document.getElementById('visualizar_estadistica');
var estadistica_seleccionada;
var myChart;
var i_choices;
var c_choices;

function mostrar_estadistica(estadistica_seleccionada) {

    i_choices = parseFloat($("#I_Choices").val());
    c_choices = parseFloat($("#C_Choices").val());

    if (typeof myChart !== 'undefined')
        myChart.destroy();

    myChart = new Chart(ctx, {
        type: estadistica_seleccionada,
        data: {
            labels: ['Incorrectas', 'Correctas'],
            datasets: [{
                label: 'Número de Selecciones',
                data: [i_choices, c_choices],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });

}

$(document).ready(function () {
    estadistica_seleccionada = $("#seleccion_estadistica").val();
    mostrar_estadistica(estadistica_seleccionada);
});

$("#seleccion_estadistica").change(function () {
    estadistica_seleccionada = $("#seleccion_estadistica").val();
    mostrar_estadistica(estadistica_seleccionada);
});