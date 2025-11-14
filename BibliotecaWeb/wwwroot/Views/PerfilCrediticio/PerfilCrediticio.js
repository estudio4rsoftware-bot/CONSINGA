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
                title: "Acciones", "data": "idPerfilCrediticio", width: "20%", render: function (data, type, row) {
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
                    return fecha.toLocaleDateString();
                }
            },
            { title: "Nombre", "data": "nombre" },
            {
                title: "Fecha vencimiento", "data": "fechaVencimiento", render: function (data, type, row) {
                    if (!data) return "";
                    let fecha = new Date(data);
                    return fecha.toLocaleDateString();
                }
            },
            { title: "Operacion asociada", "data": "operacionAsociada" },
            { title: "Monto asociado", "data": "montoAsociado" },
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

    fetch(`./PerfilCrediticio/Descargar?idPerfilCrediticio=${idPerfilCrediticio}`, {
        method: "GET"
    }).then(response => {
        return response.ok
            ? response.json()
            : response.json().then(err => Promise.reject(err));
    }).then(responseJson => {
        if (responseJson.imagen) {

            // Convert to data URL
            var dataUrl = "data:image/jpg;base64," + responseJson.imagen;

            // Create a download link and trigger it
            var link = document.createElement('a');
            link.href = dataUrl;

            const d = new Date();
            const dateStr = `${d.getDate().toString().padStart(2, "0")}${(d.getMonth() + 1).toString().padStart(2, "0")}${d.getFullYear()}`;

            link.download = 'PerfilCrediticioDetalle_' + responseJson.nombre + '_' + dateStr + '.jpg';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
        else {
            Swal.fire({
                title: "Error!",
                text: "No hay imagenes disponibles para descargar.",
                icon: "warning"
            });
        }
    }).catch((error) => {
        const msg = error?.message || "No se encontraron coincidencias. Error de Servidor.";
        Swal.fire({
            title: "Error!",
            text: msg,
            icon: "warning"
        });
    })
}

function VerResultado(idPerfilCrediticio) {

    fetch(`./PerfilCrediticio/ListaCalificacion?idPerfilCrediticio=${idPerfilCrediticio}`, {
        method: "GET"
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data) {
            $('#mdPerfilVerResultado').modal('show');
            loadControlsInModalVerResultado(responseJson.data);
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

$('#txtPlazoNegocio').on('input', function () {
    // Remove any non-digit characters
    this.value = this.value.replace(/\D/g, '');
});

$('#txtNoOperacionAsociado').on('input', function () {
    // Remove any non-digit characters
    this.value = this.value.replace(/\D/g, '');
});

$('#txtValorAsociado').on('input', function () {
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

    var plazoNegocio = $("#txtPlazoNegocio").val();
    if (plazoNegocio == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar el plazo del negocio.",
            icon: "warning"
        });
        return;
    }

    var numeroOperacion = $("#txtNoOperacionAsociado").val();
    if (numeroOperacion == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar el número de operación asociado.",
            icon: "warning"
        });
        return;
    }

    var valorAsociado = $("#txtValorAsociado").val();
    if (valorAsociado == "") {
        Swal.fire({
            title: "Error!",
            text: "Debe ingresar el valor asociado.",
            icon: "warning"
        });
        return;
    }

    var semaforo = document.querySelector('input[name="inlineRadioOptions"]:checked')?.value || 0;
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
        plazoNegocio: plazoNegocio,
        numeroOperacion: numeroOperacion,
        valorAsociado: valorAsociado,
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

function loadControlsInModalVerResultado(data) {

    loadPopovers();
    $('#txtVerNombre').val("");
    $('#cboVerSemaforo').val("Seleccionar");
    $('#txtVerObservaciones').val("");

    $('#txtVerNombre').val(data.nombre);
    $('#hdnIdPerfilCrediticio').val(data.idPerfilCrediticio);
    $('#txtPlazoNegocio').val(data.plazoNegocio);
    $('#txtNoOperacionAsociado').val(data.numeroOperacion);
    $('#txtValorAsociado').val(data.valorAsociado);

    document.querySelectorAll('input[name="inlineRadioOptions"]').forEach(radio => {
        radio.checked = false;
    });

    if (data.semaforo != "Pendiente") {
        var savedRadio = document.querySelector(`input[name="inlineRadioOptions"][value="${data.semaforo}"]`);
        savedRadio.checked = true;
    }

    if (data.observaciones != "Pendiente")
        $('#txtVerObservaciones').val(data.observaciones);

    // Clear any previous rows in the table
    const tableBody = document.getElementById('CalificacionTableBody');
    tableBody.innerHTML = '';

    // Check if there are records
    if (data.calificaciones && data.calificaciones.length > 0) {

        // Insert rows into the table
        data.calificaciones.forEach(calificacion => {
            const row = document.createElement('tr');
            row.innerHTML = `
                        <td>${calificacion.banco}</td>
                        <td>${calificacion.calificacion}</td>
                    `;
            tableBody.appendChild(row);
        });
    } else {
        // If no records, show "No records" row
        const noRecordsRow = document.createElement('tr');
        noRecordsRow.innerHTML = `
                                <td colspan="2" class="text-center">No records</td>
                            `;
        tableBody.appendChild(noRecordsRow);
    }
}

function loadPopovers() {

    $('#popoverIconPlazo').popover({
        trigger: 'hover',  // Show the popover on hover
        html: true,  // Allow HTML content inside the popover
        placement: 'right',  // Place the popover to the right of the icon
        content: `
        <p>Si estás analizando el otorgamiento de un crédito, puedes identificar el plazo asociado a un negocio para organizar los vencimientos.</p>
        <p>El dato ingresado se mostrará en la <b>columna "Fecha de vencimiento"</b> dentro del módulo Perfil Crediticio.</p>
        <p>Si no corresponde o no cuentas con el dato, indica "0" en la casilla.</p>
      `,
        title: "Disclaimer"
    });

    $('#popoverIconNoOperacion').popover({
        trigger: 'hover',  // Show the popover on hover
        html: true,  // Allow HTML content inside the popover
        placement: 'right',  // Place the popover to the right of the icon
        content: `
        <p>Si estás analizando el otorgamiento de un crédito, puedes identificar el plazo asociado a un negocio para organizar los vencimientos.</p>
        <p>El dato ingresado se mostrará en la <b>columna "Operación asociada"</b> dentro del módulo Perfil Crediticio.</p>
        <p>Si no corresponde o no cuentas con el dato, indica "0" en la casilla.</p>
      `,
        title: "Disclaimer"
    });

    $('#popoverIconValorAsociado').popover({
        trigger: 'hover',  // Show the popover on hover
        html: true,  // Allow HTML content inside the popover
        placement: 'right',  // Place the popover to the right of the icon
        content: `
        <p>Si estás analizando el otorgamiento de un crédito, puedes identificar el plazo asociado a un negocio para organizar los vencimientos.</p>
        <p>El dato ingresado se mostrará en la <b>columna "Monto asociado"</b> dentro del módulo Perfil Crediticio.</p>
        <p>Si no corresponde o no cuentas con el dato, indica "0" en la casilla.</p>
      `,
        title: "Disclaimer"
    });
}