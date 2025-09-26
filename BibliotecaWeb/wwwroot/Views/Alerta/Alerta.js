let tablaData;
let tablaDataAgrupados;
let idAlerta;
$(document).ready(function () {

    obtenerFrecuencia();
 
    tablaData = $('#tbAlerta').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "./Alerta/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Caravana", "data": "nroCaravana" },
            { title: "Propiedad", "data": "propiedad" },
            { title: "Tenencia", "data": "tenencia" },
            { title: "Ultimo control", "data": "fechaUltimoControl" },
            {
                title: "Cambio", "data": "cambio", render: function (data, type, row) {
                    let etiqueta = "";

                    switch (data) {
                        case 'No':
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                            break;
                        case 'Si':
                            etiqueta = `<span class="badge bg-danger">${data}</span>`
                            break;

                        default:
                            etiqueta = `<span class="badge bg-secondary">${data}</span>`
                    }

                    return etiqueta;
                }
            },
            {
                title: "Estado", "data": "estado", render: function (data, type, row) {
                    let etiqueta = "";

                    switch (data) {
                        case 'Activo':
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                            break;
                        case 'Inactivo':
                            etiqueta = `<span class="badge bg-secondary">${data}</span>`
                            break;

                        default:
                            etiqueta = `<span class="badge bg-info">${data}</span>`
                    }

                    return etiqueta;
                }
            },
            {
                title: "Operaciones", "data": "idAlerta", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbVerComentario(${data});"><i class="fas fa-pen-to-square"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1" onclick="tbAnularAlerta(${data});"><i class="fas fa-trash"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
   

});

function tbDesactivarAgrupado(etiqueta) {
    alert("Desactivar el grupo de alerta está en etapa de desarrollo. Etiqueta:" )
}

function tbActivarAgrupado(etiqueta) {
    alert("Desactivar el grupo de alerta está en etapa de desarrollo. Etiqueta: " )
}


$("#btnNuevaCaravana").on("click", function () {
    // alert("prueba");
    idAlerta = 0;
    $('#mdAlerta').modal('show');
});


$("#btnGuardar").on("click", function () {

  
    if ($("#txtNroCaravana").val() == "" && $("#fileLecturaCaravana").val() == "" ) {
        Swal.fire({
            title: "Error!",
            text: "Debes cargar un archivo o ingresar un valor en el nro caravana.",
            icon: "warning"
        });
        return;
    }

    if ($("#txtNroCaravana").val() != "" && $("#fileLecturaCaravana").val() != "") {
        Swal.fire({
            title: "Error!",
            text: "Debes seleccionar un tipo, unica caravana o por archivo de lectura.",
            icon: "warning"
        });
        return;
    }

    var frecuencia = $("#cboFrecuencia").val();
    if (frecuencia == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar una frecuencia.",
            icon: "warning"
        });
        return;
    }

    var txtEtiqueta = $("#txtEtiqueta").val();
    if (txtEtiqueta =="") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar una etiqueta.",
            icon: "warning"
        });
        return;
    }


    if ($("#txtNroCaravana").val() == "") {
        const fileInput = document.getElementById('fileLecturaCaravana');

        const file = fileInput.files[0];

        if (file) {


            const reader = new FileReader();

            reader.onload = function (event) {
                const contenido = event.target.result;
                const base = btoa(contenido);

                const objeto = {
                    Archivo: base,
                    ConAlerta: 'Si',
                    FrecuenciaAlerta: frecuencia,
                    Nombre: txtEtiqueta
                }


                fetch(`./ControlCampo/Guardar`, {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json;charset=utf-8' },
                    body: JSON.stringify(objeto)
                }).then(response => {
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(responseJson => {
                    if (responseJson.data == "") {
                        Swal.fire({
                            text: "Alertas registradas! Una vez corrido el proceso de Control de Campo, podrá visualizar el monitoreo de caravanas ",
                            icon: "success"
                        });
                        $('#mdAlerta').modal('hide');
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
                // Aquí puedes hacer lo que quieras con la cadena Base64, como enviarla a un servidor o mostrarla en la consola.
            };

            reader.readAsBinaryString(file);
        } 


    } else {
       
        const objeto = {
            nroCaravana: $("#txtNroCaravana").val(),
            etiqueta: txtEtiqueta,
            oFrecuenciaAlerta: frecuencia
        }


        fetch(`./Alerta/Guardar`, {
               method: "POST",
               headers: { 'Content-Type': 'application/json;charset=utf-8' },
               body: JSON.stringify(objeto)
           }).then(response => {
               return response.ok ? response.json() : Promise.reject(response);
           }).then(responseJson => {
               if (responseJson.data == "") {
                   Swal.fire({
                       text: "Caravana registrada!",
                       icon: "success"
                   });
                   $('#mdAlerta').modal('hide');
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



function tbVerComentario(id) {
   
    fetch(`./Alerta/Obtener?idAlerta=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.IdAlerta != 0) {
            const alerta = responseJson.data;


            $("#txtNroCaravanaVer").val(alerta.nroCaravana)
            $("#txtFechaInicioVer").val(alerta.fechaIngreso)
            $("#txtFechaFinVer").val(alerta.fechaFin)
        
            $("#txtTenenciaActualVer").val(alerta.tenencia);
            $("#txtTenenciaAnteriorVer").val(alerta.tenenciaAnterior);

            $("#txtLugarFisicoActualVer").val(alerta.lugarFisico);
            $("#txtLugarFisicoAnteriorVer").val(alerta.lugarFisicoAnterior);

            $("#txtPropiedadActualVer").val(alerta.propiedad);
            $("#txtPropiedadAnteriorVer").val(alerta.propiedadAnterior);
            

            $('#mdVerAlerta').modal('show');
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se encontró la alerta en la base de datos.",
                icon: "warning"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })


}




function tbAnularAlerta(id) {

    Swal.fire({
        text: "Desea anular Alerta?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
         
            fetch(`./Alerta/Anular?IdAlerta=${id}`, {
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


function obtenerFrecuencia() {
    fetch(`./FrecuenciaAlerta/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            $("#cboFrecuencia").append($("<option>").val("").text(""));
            responseJson.data.forEach((item) => {
                $("#cboFrecuencia").append($("<option>").val(item.idFrecuenciaAlerta).text(item.textoCombo));
            });
            $('#cboFrecuencia').select2({
                theme: 'bootstrap-5',
                dropdownParent: $('#mdAlerta'),
                placeholder: "Seleccionar"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias. Error de Servidor",
            icon: "warning"
        });
    })
}


