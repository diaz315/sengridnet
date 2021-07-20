function refrescar() {
    fetch('/Home/refrescar')
    .then(function (resultado) {
        return resultado.json();
    })
    .then(function (data) {
        $("#enproceso").html(data.enproceso);
        $("#enviados").html(data.enviados);
        $("#restantes").html(data.restantes);
    })
    .catch(function (error) {
        alert('Hubo un problema con la petición Fetch:' + error.message);
    });
}

$(document).ready(function () {
    refrescar();

    $("#procesar").click(function () {
        fetch('/Home/procesar')
        .then(function () {
            //console.log("Bien")
        })
        .catch(function (error) {
            alert('Hubo un problema con la petición Fetch:' + error.message);
        });
    })

    $("#refrescar").click(function () {
        refrescar();
    })
});