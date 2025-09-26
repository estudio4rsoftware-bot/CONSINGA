let tablaData;

$(document).ready(function (){
    obtenerFrecuencia();
    tablaData = $('#tbControl').DataTable({
        responsive: true,
       // order: [[0, 'desc']],
        scrollX: true,
        "ajax": {
            "url": "./ControlCampo/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //  { title: "Id", "data": "idControlCampo" },
            
            {
                title: "", "data": "nombre", width: "5%", render: function (data, type, row) {
                    return `<i class="fa fa-user" title="${data}"></i>`

                }
            },
            {
                title: "", "data": "idControlCampo", width: "20%", render: function (data, type, row) {
                    let disabled = row.estado == "Finalizado" ? "" : "disabled";
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="DescargarLectura(${data});"><i class="fa fa-download" title="Descargar lectura de ganado"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-success me-1" onclick="Descargar(${data});"><i class="fas fa-file-download" title="Descargar resultado Control de Campo"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-warning me-1" onclick="Comentarios(${data});"><i class="fa-solid fa-cloud-arrow-up" title="Visualizar Control de Campo"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-danger me-1"  onclick="Anular(${data});"><i class="fas fa-file-circle-minus" title="Anular Control de Campor"></i></button>` +
                        `<button type="button" class="btn btn-sm btn-outline-info me-1"  onclick="Clonar(${data});"><i class="fa fa-clone" title="Repetir Control de Campo"></i></button>`
                }
            },
            { title: "Fecha Control", "data": "fechaCreacion" },
            { title: "Nombre", "data": "nombre" },
          // { title: "Usuario", "data": "" },
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
                            etiqueta = `<span class="badge bg-secondary">${data}</span>`
                            break;
                        case 'Fallo':
                            etiqueta = `<span class="badge bg-danger">${data}</span>`
                            break;
                        default:
                            etiqueta = `<span class="badge bg-success">${data}</span>`
                    }
                    return etiqueta;
                }
            }
            
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function Anular(id) {

     alert("La anulación del CC esta en etapa de desarrollo");
}

function Comentarios(id) {

  //  alert("Los comentarios del CC esta en etapa de desarrollo");

   // let id = 0;

    fetch(`./ControlCampo/Obtener?IdControlCampo=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.idControlCampo != 0) {
           // id = responseJson.data.idControlCampo;
          
            $("#txtVerFecha").val(responseJson.data.fechaCreacion);
            $("#txtVerEstado").val(responseJson.data.estado);
            $("#txtVerNombre").val(responseJson.data.nombre);
            $("#txtVerComentarios").val(responseJson.data.observaciones);
            $("#txtVerIdControl").val(responseJson.data.idControlCampo);

            $('#mdControlVisualizar').modal('show');
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })



}
function Descargar(id) {
    
    // alert(control.ResultadoBase64);
    // alert("Mensaje" + control.observaciones);
    fetch(`./ControlCampo/Obtener?IdControlCampo=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        const control = responseJson.data;
        if (control.archivoResultado != "") {
            const resultado = control.archivoResultado;
            const nombre_resultado = control.nombre + "_resultado.xlsx";
            const link = document.createElement('a');
            link.href = `data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,${resultado}`;
            link.download = nombre_resultado; //'Resultado.xlsx';  Cambia el nombre y la extensión según sea necesario
            document.body.appendChild(link);
            link.click();
           // document.body.removeChild(link);
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se ha podido cargar el Excel.",
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
function DescargarLectura(id) {

    // alert(control.ResultadoBase64);
    // alert("Mensaje" + control.observaciones);
    fetch(`./ControlCampo/Obtener?IdControlCampo=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        const control = responseJson.data;
        if (control.archivo != "") {
          
            const resultado = control.archivo;
            const tipoDeArchivo = detectarTipoArchivo(resultado); // 'resultado' es tu cadena Base64
            //console.log(`El archivo parece ser: ${tipoDeArchivo}`);
           
            const nombre_resultado = control.nombre+ "_lectura";
            const link = document.createElement('a');
            //link.href = `data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,${resultado}`;
            link.href = `data:text/plain;base64,${resultado}`;
            link.download = nombre_resultado; //'Resultado.xlsx';  Cambia el nombre y la extensión según sea necesario
            document.body.appendChild(link);
            link.click();
            // document.body.removeChild(link);
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se ha podido cargar el Excel.",
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

    function detectarTipoArchivo(base64String) {
        // Decodificar el Base64
       
        const decodedString = atob(base64String);

        // Verificar si el archivo parece un CSV
        if (decodedString.includes(",") && decodedString.includes("\n")) {
            return "csv"; // Si contiene comas y saltos de línea, probablemente sea un CSV
        }

        // Si no tiene comas, podría ser texto plano (TXT)
        if (decodedString.match(/[a-zA-Z0-9\s]+/)) {
            return "txt"; // Si tiene letras, números y espacios, podría ser un TXT
        }

        // Si no puedes determinarlo, devuelves un valor por defecto
        return "desconocido";
    }
}

$("#btnNuevoControl").on("click", function () {
    $('#mdControl').modal('show');
})


$("#btnGuardar").on("click", function () {
   
    if ($("#txtArchivo").val() == "") {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    const fileInput = document.getElementById('txtArchivo');
  
    const file = fileInput.files[0];
    
    const checkbox = document.getElementById('boolAlerta');

    // Inicializamos la variable 'alerta'
    let alerta = 'No';

    let oFrecuencia=0;

    let nombre; 
    nombre = $("#txtNombre").val(); 

    // Comprobamos si el checkbox está seleccionado
   /* if (checkbox.checked) {
        alerta = 'Si';
    } else {
        alerta = 'No';
        oFrecuencia = 0;
    }*/

    if (alerta == 'Si' && oFrecuencia == 0) {
        Swal.fire({
            title: "Error!",
            text: "Si desea realizar seguimiento de Alertas a la caravanas, debe seleccionar Frecuencia.",
            icon: "warning"
        });
        return;
    }

    if (file) {
        const reader = new FileReader();
      
        reader.onload = function (event) {
           const contenido = event.target.result;
            const base = btoa(contenido);
         
            const objeto = {
                Archivo: base,
                ConAlerta: 'No',
                FrecuenciaAlerta: oFrecuencia,
                Nombre:nombre
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
                        text: "Control de campo registrado!",
                        icon: "success"
                    });
                    $('#mdControl').modal('hide');
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
    } else {
        alert("No se ha seleccionado ningún archivo.");
    }
});


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
                dropdownParent: $('#mdControl'),
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


function Clonar(id) {
    Swal.fire({
        text: "¿Desea repetir el control de campo?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/ControlCampo/Clonar?IdControlCampo=${id}`, {
                method: "POST",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == "") {
                    Swal.fire({
                        text: "Repetición del Control de Campo realizado con exito!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo repetir el control de campo.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo repetir el control de campo.",
                    icon: "warning"
                });
            })
        }
    });
}




/*
function devolverPrestamo(id) {
    Swal.fire({
        text: "Desea registrar la devolucion?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Prestamo/Devolver?IdPrestamo=${id}`, {
                method: "GET",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == 1) {
                    Swal.fire({
                        text: "Devuelto!",
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo devolver.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo devolver.",
                    icon: "warning"
                });
            })
        }
    });
}

function anularPrestamo(id) {

    Swal.fire({
        text: "Desea anular el prestamo?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/Prestamo/Anular?IdPrestamo=${id}`, {
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
                        text: "No se pudo anular.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo anular.",
                    icon: "warning"
                });
            })
        }
    });
}
*/
