let tablaData;
let tablaDataAgrupados;
let idAlerta;
$(document).ready(function () {

  //  obtenerFrecuencia();
 
    tablaData = $('#tbAlerta').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": './ListaAgrupadaEtiqueta',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Ingreso", "data": "primerControl" },
            { title: "Etiqueta", "data": "etiqueta" },
            { title: "Cantidad", "data": "cantidadAlertas" },
            { title: "Ultima actualizacion", "data": "ultimoControl" },
            {
                title: "Estado", "data": "estado", render: function (data, type, row) {
                    let est = "";

                    switch (data) {
                        case 'Activo':
                            est = `<span class="badge bg-success">${data}</span>`
                            break;
                        case 'Inactivo':
                            est = `<span class="badge bg-secondary">${data}</span>`
                            break;

                        default:
                            est = `<span class="badge bg-info">${data}</span>`
                    }
                  
                    return est;
                }
            },
            {
                title: "Operaciones", "data": "etiqueta", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbActivarAgrupado('${data}');"><i class="fas fas fa-rotate-left"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbDesactivarAgrupado('${data}');"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

});

function tbDesactivarAgrupado(etiq){

    Swal.fire({
        text: "Desea anular seguimiento de monitoreo?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Alerta/DesactivarAgrupadas?etiqueta=${etiq}`, {
                method: "POST",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {

                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "Anulado!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo anular. Error en la base de datos",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo anular. Error de servidor",
                    icon: "warning"
                });
            })
        }
    });
}

function tbActivarAgrupado(etiq){

    Swal.fire({
        text: "Desea volver a activar el seguimiento de monitoreo?",
        icon: "success",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Alerta/ActivarAgrupadas?etiqueta=${etiq}`, {
                method: "POST",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {

                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "Anulado!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo Activar. Error en la base de datos",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo anular. Error de servidor",
                    icon: "warning"
                });
            })
        }
    });
}


