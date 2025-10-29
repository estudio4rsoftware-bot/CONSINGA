let tablaData;
let idSelectPaisMdPerfil = "#cboPais";
let idSelectTipoDocumentoMdPerfil = "#cboTipoDocumento";
let idSelectPaisMdPerfilClonar = "#cboClonarPais";
let idSelectTipoDocumentoMdPerfilClonar = "#cboClonarTipoDocumento";


$(document).ready(function () {

    obtenerPais(idSelectPaisMdPerfil);
    $("#cboTipoDocumento").empty().append($("<option>").val(0).text("Seleccionar"));

    tablaData = $('#tbControl').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "./PerfilCrediticio/ListPerfilCrediticioBySigningUser",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                title: "", "data": "nombre", width: "5%", render: function (data, type, row) {
                    return `<i class="fa fa-user" title="${data}"></i>`
                }
            },
            {
                title: "", "data": "idPerfilCrediticio", width: "20%", render: function (data, type, row) {
                    let disabled = row.estado == "Finalizado" ? "" : "disabled";
                    return `<button type="button" class="btn btn-sm btn-outline-success me-1" onclick="Descargar(${data});"><i class="fas fa-file-download" title="Descargar resultado del análisis"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-warning me-1" onclick="VerResultado(${data});"><i class="fa-solid fa-cloud-arrow-up" title="Ver resultado de análisis"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1"  onclick="Eliminar(${data});"><i class="fas fa-file-circle-minus" title="Eliminar análisis"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-info me-1"  onclick="Clonar(${data});"><i class="fa fa-clone" title="Repetir análisis"></i></button>`
                }
            },
            {
                title: "Fecha Control", "data": "fechaCreacion", render: function (data, type, row) {
                    if (!data) return "";
                    let fecha = new Date(data);
                    return fecha.toLocaleString();
                }
            },
            { title: "Nombre", "data": "nombre" },
            { title: "Numero", "data": "numero" },
            {
                title: "Estado", "data": "estado", render: function (data, type, row) {
                    let etiqueta = "";
                    switch (data) {
                        case 'Pendiente':
                            etiqueta = `<span class="badge bg-warning">${data}</span>`
                            break;
                        case 'Procesando':
                            etiqueta = `<span class="badge bg-primary">${data}</span>`
                            break;
                        case 'Fallo':
                            etiqueta = `<span class="badge bg-danger">${data}</span>`
                            break;
                        default:
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                    }
                    return etiqueta;
                }
            },
            {
                title: "Semaforo", "data": "semaforo", render: function (data, type, row) {
                    let labelSemaforo = "";
                    switch (data) {
                        case 'Verde':
                            labelSemaforo = `<span class="badge bg-success">${data}</span>`
                            break;
                        case 'Amarillo':
                            labelSemaforo = `<span class="badge bg-warning">${data}</span>`
                            break;
                        case 'Rojo':
                            labelSemaforo = `<span class="badge bg-danger">${data}</span>`
                            break;
                        case 'No asignado':
                            labelSemaforo = `<span class="badge bg-dark">${data}</span>`
                            break;
                        default:
                            labelSemaforo = `<span class="badge bg-info">${data}</span>`
                    }
                    return labelSemaforo;
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function Descargar(idPerfilCrediticio) {
    alert("La descarga de los análisis esta en etapa de desarrollo");
}

function VerResultado(idPerfilCrediticio) {

    $('#mdPerfilVerResultado').modal('show');
    $('#txtVerNombre').val("");
    $('#cboVerSemaforo').val("Seleccionar");
    $('#txtVerObservaciones').val("");

    fetch(`./PerfilCrediticio/ListaCalificacion?idPerfilCrediticio=${idPerfilCrediticio}`, {
        method: "GET"
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data) {

            $('#txtVerNombre').val(responseJson.data.nombre);
            $('#hdnIdPerfilCrediticio').val(responseJson.data.idPerfilCrediticio);

            if (responseJson.data.semaforo != "Pendiente")
                $('#cboVerSemaforo').val(responseJson.data.semaforo);

            if (responseJson.data.observaciones != "Pendiente")
                $('#txtVerObservaciones').val(responseJson.data.observaciones);

            if ($.fn.DataTable.isDataTable('#tbCalificacionInstitucion')) {
                $('#tbCalificacionInstitucion').DataTable().clear().destroy();
            }

            $('#tbCalificacionInstitucion').DataTable({
                responsive: true,
                autoWidth: true,
                scrollX: true,
                data: responseJson.data.calificaciones,
                "columns": [
                    { title: "Nombre del Banco", "data": "banco" },
                    { title: "Calificación", "data": "calificacion" }
                ],
                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
                },
                searching: false,
                lengthChange: false,
                pageLength: 5,
                ordering: false,
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

function Eliminar(idPerfilCrediticio) {
    Swal.fire({
        text: "¿Esta seguro de eliminar el análisis?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`./PerfilCrediticio/Eliminar?idPerfilCrediticio=${idPerfilCrediticio}`, {
                method: "POST",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "El análisis fue eliminado correctamente!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else if (responseJson.data == 0) {
                    Swal.fire({
                        title: "Error!",
                        text: "No se encontro el análisis seleccionado.",
                        icon: "warning"
                    });
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo eliminar el análisis seleccionado.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo completar la operación.",
                    icon: "warning"
                });
            })
        }
    });
}

function Clonar(idPerfilCrediticio) {
    $("#txtClonarPeriodo").val("");
    $('#mdPerfilClonar').modal('show');
    obtenerPerfilCrediticio(idPerfilCrediticio);
}

$("#btnNuevoAnalisis").on("click", function () {
    $('#mdPerfil').modal('show');
    limpiarCampos();
})

$('#txtPeriodo').on('input', function () {
    // Remove any non-digit characters
    this.value = this.value.replace(/\D/g, '');
});

$('#txtClonarPeriodo').on('input', function () {
    // Remove any non-digit characters
    this.value = this.value.replace(/\D/g, '');
});


$("#cboPais").on("change", function () {
    var selectedValue = $(this).val();
    obtenerTiposDocumento(selectedValue, idSelectTipoDocumentoMdPerfil);
})

$("#btnGuardar").on("click", function () {

    var txtPeriodo = $("#txtPeriodo").val();
    if (txtPeriodo == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar un periodo/fecha.",
            icon: "warning"
        });
        return;
    }

    // Regular expression to validate the format YYYYMM
    var regex = /^(19|20)\d{2}(0[1-9]|1[0-2])$/;

    if (!regex.test(txtPeriodo)) {
        Swal.fire({
            title: "Error!",
            text: "El formato de fecha es incorrecto. Use AAAAMM.",
            icon: "warning"
        });
        return;
    }

    var pais = $("#cboPais").val();
    if (pais == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un pais.",
            icon: "warning"
        });
        return;
    }

    var tipoDocumento = $("#cboTipoDocumento").val();
    if (tipoDocumento == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un tipo de documento.",
            icon: "warning"
        });
        return;
    }

    var txtNumero = $("#txtNumero").val();
    if (txtNumero == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar un numero.",
            icon: "warning"
        });
        return;
    }

    const perfil = {
        periodo: txtPeriodo,
        numero: txtNumero,
        idTipoDocumento: tipoDocumento,
        idPais: pais
    }

    fetch(`./PerfilCrediticio/Guardar`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(perfil)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == "") {
            Swal.fire({
                text: "Perfil crediticio registrado!",
                icon: "success"
            });
            limpiarCampos();
            $('#mdPerfil').modal('hide');
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
});

$("#btnVerGuardar").on("click", function () {

    var idPerfil = $("#hdnIdPerfilCrediticio").val();    

    var semaforo = $("#cboVerSemaforo").val();
    if (semaforo == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un valor para el campo semaforo.",
            icon: "warning"
        });
        return;
    }

    var observaciones = $("#txtVerObservaciones").val();
    if (observaciones == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar las observaciones para completar el análisis.",
            icon: "warning"
        });
        return;
    }

    const perfilActualizado = {
        idPerfilCrediticio: idPerfil,
        semaforo: semaforo,
        observaciones: observaciones
    }

    fetch(`./PerfilCrediticio/Actualizar`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(perfilActualizado)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == 1) {
            Swal.fire({
                text: "Análisis actualizado!",
                icon: "success"
            });
            $('#mdPerfilVerResultado').modal('hide');
            tablaData.ajax.reload();
        } else if (responseJson.data == 0) {
            Swal.fire({
                title: "Error!",
                text: "Ningun registro fue actualizado.",
                icon: "warning"
            });
        }
        else {
            Swal.fire({
                title: "Error!",
                text: "Occurrio un error y no se pudo actualizar el análisis.",
                icon: "warning"
            })
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se pudo actualizar la información.",
            icon: "warning"
        });
    })
});

function obtenerPais(idSelect) {
    return fetch(`./PerfilCrediticio/GetPaises`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            $(idSelect).append($("<option>").val("").text("Seleccionar"));
            responseJson.data.forEach((item) => {
                $(idSelect).append($("<option>").val(item.idPais).text(item.nombre));
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

function obtenerTiposDocumento(idPais, idSelect) {
    return fetch(`./PerfilCrediticio/GetTiposDocumento?idPais=${idPais}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        $(idSelect).empty().append($("<option>").val("").text("Seleccionar"));
        if (responseJson.data.length > 0) {
            responseJson.data.forEach((item) => {
                $(idSelect).append($("<option>").val(item.idTipo).text(item.descripcion));
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

function obtenerPerfilCrediticio(idPerfilCrediticio) {
    fetch(`./PerfilCrediticio/Obtener?idPerfilCrediticio=${idPerfilCrediticio}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idPerfilCrediticio != 0) {

            obtenerPais(idSelectPaisMdPerfilClonar).then(() => {
                $("#cboClonarPais").val(responseJson.data.idPais);
            });

            obtenerTiposDocumento(responseJson.data.idPais, idSelectTipoDocumentoMdPerfilClonar).then(() => {
                $("#cboClonarTipoDocumento").val(responseJson.data.idTipoDocumento);
            });

            $("#txtClonarNumero").val(responseJson.data.numero);
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias. Error de Servidor",
            icon: "warning"
        });
    })
}

$("#btnClonar").on("click", function () {

    var txtPeriodo = $("#txtClonarPeriodo").val();
    if (txtPeriodo == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar un periodo/fecha.",
            icon: "warning"
        });
        return;
    }

    // Regular expression to validate the format YYYYMM
    var regex = /^(19|20)\d{2}(0[1-9]|1[0-2])$/;

    if (!regex.test(txtPeriodo)) {
        Swal.fire({
            title: "Error!",
            text: "El formato de fecha es incorrecto. Use AAAAMM.",
            icon: "warning"
        });
        return;
    }

    var pais = $("#cboClonarPais").val();
    if (pais == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un pais.",
            icon: "warning"
        });
        return;
    }

    var tipoDocumento = $("#cboClonarTipoDocumento").val();
    if (tipoDocumento == 0) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un tipo de documento.",
            icon: "warning"
        });
        return;
    }

    var txtNumero = $("#txtClonarNumero").val();
    if (txtNumero == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar un numero.",
            icon: "warning"
        });
        return;
    }

    const perfil = {
        periodo: txtPeriodo,
        numero: txtNumero,
        idTipoDocumento: tipoDocumento,
        idPais: pais
    }

    fetch(`./PerfilCrediticio/Guardar`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(perfil)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == "") {
            Swal.fire({
                text: "Análisis clonado!",
                icon: "success"
            });
            $('#mdPerfilClonar').modal('hide');
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
            text: "No se pudo clonar el análisis.",
            icon: "warning"
        });
    })
});

function limpiarCampos() {
    $("#txtPeriodo").val("");
    $("#cboPais").val("");
    $("#cboTipoDocumento").empty().append($("<option>").val(0).text("Seleccionar"));
    $("#txtNumero").val("");
}