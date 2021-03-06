﻿var ctx = document.getElementById('visualizar_estadistica');
var estadistica_seleccionada;
var myChart;
var i_choices;
var c_choices;
var list_id;

$(function () {

    $("#seleccion_pregunta").SumoSelect({
        placeholder: 'Seleccione una o varias Preguntas',
        captionFormat: '{0} Preguntas seleccionadas',
        captionFormatAllSelected: '{0} Preguntas seleccionadas',
        locale: ['OK', 'Cancelar', 'Seleccionar Todas'],
        selectAll: true
    });

    $('li.opt').click(function () {

        list_id = [];

        getSeleccionadas();

        ConseguirDatos();
    });
});

function getSeleccionadas()
{
    list_id = [];

    var seleccion = $('#seleccion_pregunta :selected');

    list_id = $.map(seleccion, function (option) {
        return option.value;
    });
}

function ConseguirDatos()
{
    console.log(list_id);

    $.ajax({
        type: "GET",
        url: 'ObtenerDatos',
        data: { data: JSON.stringify(list_id) },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            c_choices = result.c_result;
            i_choices = result.i_result;

            estadistica_seleccionada = $("#seleccion_estadistica").val();

            mostrar_estadistica(estadistica_seleccionada, i_choices, c_choices);

        },
        error: function () {
            console.log("Error while inserting data");
        }
    });
}

function mostrar_estadistica(estadistica_seleccionada, i_choices, c_choices) {

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

    $('p.select-all').css("height", "33px");

    $('.sumo_seleccion').css("width", "275px");

    $("#visualizar_estadistica").hide();

    $('p.select-all').click();

    $("#visualizar_estadistica").show();

});

$("#seleccion_estadistica").change(function () {
    estadistica_seleccionada = $("#seleccion_estadistica").val();
    mostrar_estadistica(estadistica_seleccionada);
});