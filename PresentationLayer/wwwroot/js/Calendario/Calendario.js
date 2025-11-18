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
                // Usar la fecha que se está mostrando en el evento
                self.fechaEntrega(info.event.extendedProps.fechaMostrada);
                self.nombre(info.event.title);
                self.responsable(info.event.extendedProps.responsable);
                self.area(info.event.extendedProps.area);
                $("#divEdicion").modal("show");
            },
            eventContent: function (arg) {
                let customHtml = `
                <div class="fc-event-title-container">
                    <div class="fc-event-title">${arg.event.title}</div>
                    <div class="fc-event-subtitle">
                        Tipo: ${arg.event.extendedProps.tipoTexto}<br>
                        Fecha: ${arg.event.extendedProps.fechaMostrada}<br>
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
                    // Determinar qué fecha y color usar según si la fecha de publicación está liberada
                    var usarFechaPublicacion = item.fechaPublicacion && item.fechaPublicacionLiberada;

                    var fecha, tipoEvento, colorFondo, colorBorde, fechaParaProps;

                    if (usarFechaPublicacion) {
                        // Si la fecha de publicación está liberada: ROJO claro y mostrar fecha de publicación
                        fecha = new Date(item.fechaPublicacion);
                        tipoEvento = 'Publicación';
                        colorFondo = '#FF7979';  // Rojo claro
                        colorBorde = '#FF7979';
                        fechaParaProps = new Date(item.fechaPublicacion).toISOString().split('T')[0];
                    } else {
                        // Si NO está liberada o no existe: MORADO y mostrar fecha de entrega
                        fecha = new Date(item.fechaEntrega);
                        tipoEvento = 'Entrega';
                        colorFondo = '#A564DC';  // Morado
                        colorBorde = '#A564DC';
                        fechaParaProps = new Date(item.fechaEntrega).toISOString().split('T')[0];
                    }

                    var fechaFormateada = fecha.toISOString().split('T')[0];
                    var fechaFin = new Date(fecha);
                    fechaFin.setDate(fechaFin.getDate() + 1);

                    events.push({
                        title: item.nombre,
                        start: fechaFormateada,
                        end: fechaFin,
                        allDay: true,
                        backgroundColor: colorFondo,
                        borderColor: colorBorde,
                        extendedProps: {
                            fechaEntrega: item.fechaEntrega ? new Date(item.fechaEntrega).toISOString().split('T')[0] : '',
                            fechaPublicacion: item.fechaPublicacion ? new Date(item.fechaPublicacion).toISOString().split('T')[0] : '',
                            fechaMostrada: fechaParaProps,
                            responsable: item.responsable,
                            area: item.area,
                            tipo: usarFechaPublicacion ? 'publicacion' : 'entrega',
                            tipoTexto: tipoEvento
                        }
                    });
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
