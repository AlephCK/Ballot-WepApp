if ($('#tabla').length) {

    $('#tabla').DataTable({
        dom: "<'row'<'col-sm-1.5'l><'col-sm-7 text-center'B><'col-sm-3.5'f>>" +
"<'row'<'col-sm-12'tr>>" +
"<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
        {
            extend: 'copy',
            text: 'Copiar'
        },
        {
            extend: 'excel',
            text: 'Excel'
        },
        {
            extend: 'pdf',
            text: 'PDF'
        }
        ],
        lengthMenu: [[-1, 10, 25, 50, 100], ["Todos", 10, 25, 50, 100]],
        "pageLength": 10,
        language: {
            processing: "Procesando",
            search: "Buscar:",
            lengthMenu: "Ver _MENU_ Filas",
            info: "_END_ de _TOTAL_ elementos",
            infoFiltered: "(Filtro de _MAX_ entradas en total)",
            infoPostFix: "",
            loadingRecords: "Cargando datos...",
            zeroRecords: "No se encontraron datos",
            emptyTable: "No hay datos disponibles",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            }
        }
    });
}