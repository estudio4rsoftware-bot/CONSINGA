let tablaData;
let idEmpresaEditar = 0;
let _rut, _nombre = "";

$(document).ready(function () {
    
    tablaData = $('#tbEmpresa').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "./Empresa/Lista",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Codigo", "data": "idEmpresa" },
            { title: "RUT", "data": "rut" },
            { title: "Nombre", "data": "nombre" },
            { title: "Mail", "data": "mail" },

            {
                title: "", "data": "idEmpresa", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1" onclick="tbEditarEmpresa(${data});"><i class="fas fa-pen-to-square"></i></button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

function tbEditarEmpresa(id) {
   
    fetch(`/Empresa/Obtener?IdEmpresa=${id}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        
        if (responseJson.data.idEmpresa != 0) {
            const empresa = responseJson.data;
            $("#txtRut").val(empresa.rut);
            $("#txtNombre").val(empresa.nombre);
            $("#txtDireccion").val(empresa.direccion);
            $("#txtMail").val(empresa.mail);
           // $("#txtContacto").val(empresa.contacto);
            $("#txtUsuarioSNIG").val(empresa.usuarioSnig);
            $("#txtClaveSNIG").val(empresa.claveSnig);
            $('#mdEmpresa').modal('show');
        } else {
            Swal.fire({
                title: "Error!",
                text: "No se encontraron coincidencias.",
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

//$("#btnNuevoEstudiante").on("click", function () {
//    idEstudianteEditar = 0;
//    $("#txtNombres").val("");
//    $("#txtApellidos").val("");

//    $('#mdEstudiante').modal('show');
//})

//function tbEliminarEstudiante(id) {

//    Swal.fire({
//        text: "Desea eliminar el estudiante?",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Si, continuar",
//        cancelButtonText: "No, volver"
//    }).then((result) => {
//        if (result.isConfirmed) {

//            fetch(`/Estudiante/Eliminar?IdEstudiante=${id}`, {
//                method: "DELETE",
//                headers: { 'Content-Type': 'application/json;charset=utf-8' }
//            }).then(response => {
//                return response.ok ? response.json() : Promise.reject(response);
//            }).then(responseJson => {
//                if (responseJson.data == 1) {
//                    Swal.fire({
//                        title: "Eliminado!",
//                        text: "El estudiante fue eliminado.",
//                        icon: "success"
//                    });
//                    tablaData.ajax.reload();
//                } else {
//                    Swal.fire({
//                        title: "Error!",
//                        text: "No se pudo eliminar.",
//                        icon: "warning"
//                    });
//                }
//            }).catch((error) => {
//                Swal.fire({
//                    title: "Error!",
//                    text: "No se pudo eliminar.",
//                    icon: "warning"
//                });
//            })
//        }
//    });
//}

$("#btnGuardar").on("click", function () {


    if ($("#txtNombre").val() == "" ||
        $("#txtRUT").val().trim() == ""
    ) {
        Swal.fire({
            title: "Error!",
            text: "Faltan completar datos.",
            icon: "warning"
        });
        return;
    }

    const objeto = {
        IdEmpresa: idEmpresaEditar,
        RUT: $("#txtRUT").val(),
        Nombre: $("#txtNombre").val(),
        Direccion: $("#txtDireccion").val(),
        Contacto: $("#txtContacto").val(),
        UsuarioSnig: $("#txtUsuarioSnig").val(),
        ClaveSnig: $("#txtClaveSnig").val()
    }

    if (idEmpresaEditar != 0) {

        fetch(`/Empresa/Editar`, {
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
                $('#mdEmpresa').modal('hide');
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
        fetch(`/Empresa/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: "Empresa registrado!",
                    icon: "success"
                });
                $('#mdEmpresa').modal('hide');
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