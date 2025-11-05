$(document).ready(function () {
    fetch(`campor/Home/Dashboard`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        const data = responseJson.data;

       // alert("entre")

        $("#htotalControlCampo").text(data.totalControlCampo);
        $("#htotalAlertasActivas").text(data.totalAlertasActivas);
        $("#htotalControlesProceso").text(data.totalControlesProceso);
        $("#htotalAntecedentes").text(data.totalAntecedentes);
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias." ,
            icon: "warning"
        });
    })
})