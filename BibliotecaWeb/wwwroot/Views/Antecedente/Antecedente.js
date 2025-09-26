let tablaData;
let idAntecedenteEditar;
$(document).ready(function () {
    tablaData = $('#tbAntecedente').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "./Antecedente/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Fecha", "data": "fechaIngreso" },
            { title: "Documento", "data": "documento" },
            { title: "Estado", "data": "estado" },
            {
                title: "Estado", "data": "estado", render: function (data, type, row) {
                    let etiqueta = "";
                   
                    switch (data) {
                        case 'Pendiente':
                            etiqueta = `<span class="badge bg-warning">${data}</span>`
                            break;
                        case 'En proceso':
                            etiqueta = `<span class="badge bg-primary">${data}</span>`
                            break;
                        case 'Anulado':
                            etiqueta = `<span class="badge bg-danger">${data}</span>`
                            break;
                        default:
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                    }
                 
                    return etiqueta;
                }
            },
            {
                title: "", "data": "idAntecedente", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarAntecedente(${data});"><i class="fas fa-pen-to-square"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbAnularAntecedente(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function tbEditarAntecedente(id) {
   
    fetch(`./Antecedente/Obtener?IdAntecedente=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        
        if (responseJson.data.idAntecedente != 0) {
           
            const antecedente = responseJson.data;
           
            idAntecedenteEditar = antecedente.idAntecedente;
            $("#txtEstado").val(antecedente.estado);
            $("#txtDocumento").val(antecedente.documento);
            $("#txtResultadoBCU").val(antecedente.resultadoBCU);
            $("#txtResultadoDGI").val(antecedente.resultadoDGI);
            $("#txtResultadoClearing").val(antecedente.resultadoClearing);
            $("#txtResultadoOtro").val(antecedente.resultadoOtro);
            $('#mdAntecedente').modal('show');
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se pudo cargar el antecedentes.",
                icon: "warning"
            });
        }
    }).catch((error) => {
     
        Swal.fire({
            title: "Error!",
            text: "Excepcion en la obtencion del Antecedente.",
            icon: "warning"
        });
    })

}

$("#btnNuevoAntecedente").on("click", function () {
    idUsuarioEditar = 0;
    $("#txtDocumento").val("");
    $("#txtEstado").val("Pendiente");
    $("#txtResultadoBCU").val("Pendiente");
    $("#txtResultadoDGI").val("Pendiente");
    $("#txtResultadoClearing").val("Pendiente");
    $("#txtResultadoOtro").val("Pendiente");


    $('#mdAntecedente').modal('show');
})
/*
function tbEliminarAntecedente(id) {

    Swal.fire({
        text: "Desea eliminar el usuario?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Usuario/Eliminar?IdUsuario=${id}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        title: "Eliminado!",
                        text: "El usuario fue eliminado.",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo eliminar.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo eliminar.",
                    icon: "warning"
                });
            })
        }
    });
}
*/


$("#btnGuardar").on("click", function () {


    if ($("#txtDocumento").val() == "") {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos. Debe ingresar el documento", 
            icon: "warning"
        });
        return;
    }


    const objeto = {
        IdAntecedente: idAntecedenteEditar,
        Documento: $("#txtDocumento").val().trim(),
        EmpresaId: "1",
        UsuarioId:"1"
    }
  

    if (idAntecedenteEditar != 0) {

        fetch(`./Antecedente/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Se guardaron los cambios!",
                    icon: "success"
                });
                $('#mdUsuario').modal('hide');
                tablaData.ajax.reload();
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.data,
                    icon: "warning"
                });
            }
        }).catch((error) => {
            Swal.fire({
                title: "Error!",
                text: "No se pudo editar.",
                icon: "warning"
            });
        })
    } else {
      
        fetch(`./Antecedente/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Antecedente registrado!",
                    icon: "success"
                });
                $('#mdAntecedente').modal('hide');
                tablaData.ajax.reload();
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.data,
                    icon: "warning"
                });
            }
        }).catch((error) => {
            Swal.fire({
                title: "Error!",
                text: "No se pudo registrar.",
                icon: "warning"
            });
        })
    }
});


function tbAnularAntecedente(id) {

    Swal.fire({
        text: "Desea anular el Antecedente?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`./Antecedente/Anular?IdAlerta=${id}`, {
                method: "GET",
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