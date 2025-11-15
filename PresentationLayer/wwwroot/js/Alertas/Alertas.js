    var estatusActivo = { IdEstatus: true, Estatus: "Activo" };
var estatusInactivo = { IdEstatus: false, Estatus: "Inactivo" };

var catEstatus = [estatusActivo, estatusInactivo];
function AppViewModel() {
    var self = this;

    self.registros = ko.observableArray();

    self.catTiposAlerta = ko.observableArray();
    self.TiposAlerta = ko.observable().extend({ required: true });
 

 

    // Función para formatear fecha
    self.formatearFecha = function (fecha) {
        if (!fecha) return '';
        var date = new Date(fecha);
        var dia = String(date.getDate()).padStart(2, '0');
        var mes = String(date.getMonth() + 1).padStart(2, '0');
        var anio = date.getFullYear();
        return dia + '/' + mes + '/' + anio;
    };

    self.inicializar = function () {
        $.ajax({
            url: "/Alertas/ObtenerAlertas", // URL del método GetAll en tu API
            type: "GET",
            contentType: "application/json",
            success: function (d) {
                self.registros.removeAll();
                // Ordenar por fechaCreacion en forma descendente (más reciente primero)
                var alertasOrdenadas = d.datos.sort(function(a, b) {
                    return new Date(b.fechaCreacion) - new Date(a.fechaCreacion);
                });
                self.registros.push.apply(self.registros, alertasOrdenadas);
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener los datos: ", error);
                alert("Error al obtener los datos: " + xhr.responseText);
            }
        });

    }
    self.inicializar();

   

    self.Editar = function (alerta) {
        $.ajax({
            url: "/Alertas/ActualizarAlerta/" + alerta.id, // URL del método GetAll en tu API
            type: "GET",
            contentType: "application/json",
            success: function (d) {
                // Redirigir a la página especificada en alerta.accion
                if (alerta.accion) {
                    window.location.href = alerta.accion;
                }
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener los datos: ", error);
                alert("Error al obtener los datos: " + xhr.responseText);
            }
        });
    }
   
}

// Activar Knockout.js
ko.applyBindings(new AppViewModel());