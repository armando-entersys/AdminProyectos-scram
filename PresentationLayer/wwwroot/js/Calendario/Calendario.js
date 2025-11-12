function AppViewModel() {
    var self = this;

    self.registros = ko.observableArray([]);
    self.id = ko.observable(0);
    self.fechaEntrega = ko.observable("");
    self.nombre = ko.observable("");
    self.responsable = ko.observable("");
    self.area = ko.observable("");

    // Initialize the calendar
    self.inicializarCalendario = function () {
        var calendarEl = document.getElementById('calendar');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'es', // Idioma español
            initialView: 'dayGridMonth', // Vista inicial
            headerToolbar: {
                left: 'prev,next today', 
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            events: self.registros(), // Cargar eventos
            eventDisplay: 'block',
            displayEventTime: false,
            eventClick: function (info) {
                if (info.event.extendedProps.tipo === 'entrega') {
                    self.fechaEntrega(info.event.extendedProps.fechaEntrega);
                } else {
                    self.fechaEntrega(info.event.extendedProps.fechaPublicacion);
                }
                self.nombre(info.event.title);
                self.responsable(info.event.extendedProps.responsable);
                self.area(info.event.extendedProps.area);
                $("#divEdicion").modal("show");
            },
            eventContent: function (arg) {
                var fecha = arg.event.extendedProps.tipo === 'entrega'
                    ? arg.event.extendedProps.fechaEntrega
                    : arg.event.extendedProps.fechaPublicacion;
                var tipoTexto = arg.event.extendedProps.tipo === 'entrega'
                    ? 'Entrega'
                    : 'Publicación';

                let customHtml = `
                <div class="fc-event-title-container">
                    <div class="fc-event-title">${arg.event.title}</div>
                    <div class="fc-event-subtitle">
                        Tipo: ${tipoTexto}<br>
                        Fecha: ${fecha}<br>
                        Responsable: ${arg.event.extendedProps.responsable}<br>
                        Área: ${arg.event.extendedProps.area}
                    </div>
                </div>
            `;
                return { html: customHtml };
            }
        });
        calendar.render();
    };


    // Fetch project data and load into the calendar
    self.inicializar = function () {
        $.ajax({
            url: "/Materiales/ObtenerMateriales",
            type: "GET",
            contentType: "application/json",
            success: function (d) {
                var events = [];

                d.datos.forEach(function (item) {
                    // Agregar fecha de entrega (azul - color por defecto)
                    var fechaFormateada = new Date(item.fechaEntrega).toISOString().split('T')[0];
                    var fechaFin = new Date(item.fechaEntrega);
                    fechaFin.setDate(fechaFin.getDate() + 1);
                    events.push({
                        title: item.nombre + " (Entrega)",
                        start: fechaFormateada,
                        end: fechaFin,
                        allDay: true,
                        backgroundColor: '#3788d8', // Azul para entregas
                        borderColor: '#3788d8',
                        extendedProps: {
                            fechaEntrega: fechaFormateada,
                            responsable: item.responsable,
                            area: item.area,
                            tipo: 'entrega'
                        }
                    });

                    // Agregar fecha de publicación si existe y está liberada (o si es Admin)
                    // RolId está disponible globalmente si se pasa desde la vista
                    var esAdmin = typeof RolId !== 'undefined' && RolId === 1;
                    if (item.fechaPublicacion && (item.fechaPublicacionLiberada || esAdmin)) {
                        var fechaPublicacion = new Date(item.fechaPublicacion).toISOString().split('T')[0];
                        var fechaFinPublicacion = new Date(item.fechaPublicacion);
                        fechaFinPublicacion.setDate(fechaFinPublicacion.getDate() + 1);
                        events.push({
                            title: item.nombre + " (Publicación)",
                            start: fechaPublicacion,
                            end: fechaFinPublicacion,
                            allDay: true,
                            backgroundColor: '#D9534F', // Rojo para publicaciones
                            borderColor: '#D9534F',
                            extendedProps: {
                                fechaPublicacion: fechaPublicacion,
                                responsable: item.responsable,
                                area: item.area,
                                tipo: 'publicacion'
                            }
                        });
                    }
                });

                self.registros(events); // Actualiza los datos
                self.inicializarCalendario(); // Renderiza el calendario
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener los datos: ", error);
                alert("Error al obtener los datos: " + xhr.responseText);
            }
        });
    };

    self.inicializar();
}

// Activate Knockout.js bindings
ko.applyBindings(new AppViewModel());
