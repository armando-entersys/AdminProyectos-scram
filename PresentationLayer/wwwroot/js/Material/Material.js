var estatusActivo = { IdEstatus: true, Estatus: "Activo" };
var estatusInactivo = { IdEstatus: false, Estatus: "Inactivo" };

var catEstatus = [estatusActivo, estatusInactivo];

function AppViewModel() {
    var self = this;

    // Observables principales
    self.registros = ko.observableArray([]);
    self.registrosHistorico = ko.observableArray([]);

    self.catEstatusMateriales = ko.observableArray([]);
    self.EstatusMateriales = ko.observable();

    self.catEstatusMaterialesFiltro = ko.observableArray([]);
    self.EstatusMaterialesFiltro = ko.observable();

    self.tituloModal = ko.observable("");

    self.revision = ko.observable(0);
    self.produccion = ko.observable(0);
    self.faltaInfo = ko.observable(0);
    self.aprobado = ko.observable(0);
    self.programado = ko.observable(0);
    self.entregado = ko.observable(0);
    self.inicioCiclo = ko.observable(0);
    self.noCompartio = ko.observable(0);
    self.linksReferencias = ko.observable("");
    self.idBrief = ko.observable(0);
    self.rutaArchivo = ko.observable("");
    self.nombreBrief = ko.observable("");
    self.descripcionBrief = ko.observable("");
    self.objetivoBrief = ko.observable("");
    self.objetivoNegocioBrief = ko.observable("");
    self.dirigidoABrief = ko.observable("");
    self.comentarioBrief = ko.observable("");
    self.tipoBrief = ko.observable("");
    self.estatusBrief = ko.observable("");
    self.fechaEntregaBrief = ko.observable("");

    // Observables para información del material
    self.mensajeMaterial = ko.observable("");
    self.pcnMaterial = ko.observable("");
    self.formatoMaterial = ko.observable("");
    self.estatusMaterial = ko.observable("");
    self.audienciaMaterial = ko.observable("");
    self.responsableMaterial = ko.observable("");
    self.areaMaterial = ko.observable("");
    self.fechaEntregaMaterial = ko.observable("");
    self.fechaPublicacion = ko.observable("");
    self.fechaPublicacionLiberada = ko.observable(false);
    self.fechaPublicacionMaterial = ko.observable("");

    // Computed para normalizar URLs agregando http:// si no tiene protocolo
    self.linksReferenciasNormalizadas = ko.computed(function() {
        var links = self.linksReferencias();
        if (!links) return '';

        // Si el link no empieza con http:// o https://, agregarlo
        if (links && !links.match(/^https?:\/\//i)) {
            return 'http://' + links;
        }
        return links;
    });

    // Observables para filtros
    // Inicializar filtroNombre con el valor de la URL si existe
    self.filtroNombre = ko.observable(typeof FiltroNombreInicial !== 'undefined' && FiltroNombreInicial ? FiltroNombreInicial : "");
    self.filtroNombreProyecto = ko.observable("");
    self.filtroArea = ko.observable("");
    self.filtroResponsable = ko.observable("");
    self.filtroFechaEntrega = ko.observable("");
    self.filtroFechaInicio = ko.observable("");
    self.filtroFechaFin = ko.observable("");

    // Fecha mínima para selección
    const today = new Date();
    self.minDate = today.toISOString().split('T')[0];

    // Otros observables
    self.envioCorreo = ko.observable("No");
    self.id = ko.observable();
    self.Comentario = ko.observable("");
    self.fechaEntrega = ko.observable("");

    // Autocompletado
    self.buscarUsuario = ko.observable("");
    self.resultadosBusqueda = ko.observableArray([]);
    self.registrosUsuariosCorreo = ko.observableArray([]);

    // Paginación
    self.pageSize = ko.observable(8);
    self.currentPage = ko.observable(1);
    self.totalRegistros = ko.observable(0);

    // Función para verificar si el rol actual está permitido
    self.isRoleVisible = function (allowedRoles) {
        return allowedRoles.includes(RolId); // Asegúrate de que `RolId` esté definido en tu código
    };

    // Custom binding handler para evitar el reset del cursor en áreas de texto
    ko.bindingHandlers.textareaCursor = {
        update: function (element, valueAccessor) {
            var value = ko.unwrap(valueAccessor());
            var cursorPosition = element.selectionStart;
            element.value = value;
            setTimeout(function () {
                element.setSelectionRange(cursorPosition, cursorPosition);
            }, 0);
        }
    };

    // Función helper para obtener los PCNs como string
    self.getPCNsString = function(material) {
        if (material.materialPCNs && material.materialPCNs.length > 0) {
            return material.materialPCNs.map(function(mp) {
                return mp.pcn ? mp.pcn.descripcion : '';
            }).filter(function(desc) {
                return desc !== '';
            }).join(', ');
        }
        return 'N/A';
    };

    // Función helper para obtener las Audiencias como string
    self.getAudienciasString = function(material) {
        if (material.materialAudiencias && material.materialAudiencias.length > 0) {
            return material.materialAudiencias.map(function(ma) {
                return ma.audiencia ? ma.audiencia.descripcion : '';
            }).filter(function(desc) {
                return desc !== '';
            }).join(', ');
        }
        return 'N/A';
    };

    // Computados
    self.registrosFiltrados = ko.computed(function () {
        var filtroNombreProyecto = self.filtroNombreProyecto().toLowerCase();
        var filtroNombre = self.filtroNombre().toLowerCase();
        var filtroArea = self.filtroArea().toLowerCase();
        var filtroResponsable = self.filtroResponsable().toLowerCase();
        var filtroEstatus = self.EstatusMaterialesFiltro(); // Obtener el estatus seleccionado

        return ko.utils.arrayFilter(self.registros(), function (registro) {
            var nombreProyecto = (registro.brief?.nombre || "").toLowerCase();
            var nombre = (registro.nombre || "").toLowerCase();
            var area = (registro.area || "").toLowerCase();
            var responsable = (registro.responsable || "").toLowerCase();
            var fechaEntrega = new Date(registro.fechaEntrega);

            var cumpleFiltroNombre = !filtroNombre || nombre.includes(filtroNombre);
            var cumpleFiltroProyecto = !filtroNombreProyecto || nombreProyecto.includes(filtroNombreProyecto);
            var cumpleFiltroArea = !filtroArea || area.includes(filtroArea);
            var cumpleFiltroResponsable = !filtroResponsable || responsable.includes(filtroResponsable);
            // Verifica el estatus
            var cumpleFiltroEstatus = !filtroEstatus || registro.estatusMaterialId === filtroEstatus.id;

            let cumpleFechas = true;
            if (self.filtroFechaInicio()) {
                cumpleFechas = cumpleFechas && fechaEntrega >= new Date(self.filtroFechaInicio());
            }
            if (self.filtroFechaFin()) {
                cumpleFechas = cumpleFechas && fechaEntrega <= new Date(self.filtroFechaFin());
            }

            return cumpleFiltroNombre && cumpleFiltroProyecto && cumpleFiltroArea && cumpleFiltroResponsable && cumpleFiltroEstatus && cumpleFechas;
        });
    });

    self.paginatedRegistros = ko.computed(function () {
        var startIndex = (self.currentPage() - 1) * self.pageSize();
        return self.registrosFiltrados().slice(startIndex, startIndex + self.pageSize());
    });

    self.totalPages = ko.computed(function () {
        return Math.ceil(self.registrosFiltrados().length / self.pageSize());
    });

    // Cambiar página
    self.goToPage = function (page) {
        if (page >= 1 && page <= self.totalPages()) {
            self.currentPage(page);
        }
    };

    // Inicializar datos
    self.inicializar = function () {
        $.get("/Materiales/ObtenerMateriales")
            .then(function (d) {
                console.log("Datos de materiales:", d.datos); // Verifica la estructura de datos
                self.registros(d.datos);

                return $.get("/Materiales/ObtenerConteoEstatusMateriales");
            })
            .then(function (d) {
                self.revision(d.datos.revision);
                self.produccion(d.datos.produccion);
                self.faltaInfo(d.datos.faltaInfo);
                self.aprobado(d.datos.aprobado);
                self.programado(d.datos.programado);
                self.entregado(d.datos.entregado);
                return $.get("/Materiales/ObtenerEstatusMateriales");
            })
            .then(function (d) {
                var estatusMateriales = d.datos;

                // Reporte 2: Filtrar estados por rol
                if (typeof RolId !== 'undefined') {
                    if (RolId === 1 || RolId === '1') {
                        // Administrador: Ve TODOS los estados
                        estatusMateriales = d.datos;
                    } else if (RolId === 3 || RolId === '3') {
                        // Producción: Solo ve estados específicos
                        //   - En diseño (2)
                        //   - En revisión (3)
                        //   - Listo para publicación (4)
                        //   - En producción (5)
                        estatusMateriales = d.datos.filter(function(estatus) {
                            return estatus.id === 2 || estatus.id === 3 || estatus.id === 4 || estatus.id === 5;
                        });
                    } else {
                        // Solicitante (RolId = 2): NO ve "En producción" (ID=5) ni "Listo para publicación" (ID=4)
                        estatusMateriales = d.datos.filter(function(estatus) {
                            return estatus.id !== 4 && estatus.id !== 5;
                        });
                    }
                }

                self.catEstatusMateriales(estatusMateriales);
                self.catEstatusMaterialesFiltro(d.datos);
            })

            .catch(function (error) {
                console.error("Error al inicializar los datos:", error);
            });
    };

    // Editar material
    self.Editar = function (material) {
        self.tituloModal("Editar Material: " + material.nombre);
        self.Comentario("");

        self.id(material.id);
        self.fechaEntrega(new Date(material.fechaEntrega).toISOString().split('T')[0]);
        self.registrosUsuariosCorreo.removeAll();
        self.idBrief(material.brief.id);
        self.nombreBrief(material.brief.nombre);

        if (material.brief.linksReferencias == "undefined") {
            self.linksReferencias("");
        }
        else {
            self.linksReferencias(material.brief.linksReferencias);
        }

        if (material.brief.rutaArchivo == "undefined" || !material.brief.rutaArchivo) {
            self.rutaArchivo("");
        }
        else {
            self.rutaArchivo(material.brief.rutaArchivo);
        }

        // Poblar información adicional del brief
        self.descripcionBrief(material.brief.descripcion || "");
        self.objetivoBrief(material.brief.objetivo || "");
        self.objetivoNegocioBrief(material.brief.objetivoNegocio || "");
        self.dirigidoABrief(material.brief.dirigidoA || "");
        self.comentarioBrief(material.brief.comentario || "");
        self.tipoBrief(material.brief.tipoBrief?.descripcion || "N/A");
        self.estatusBrief(material.brief.estatusBrief?.descripcion || "N/A");
        self.fechaEntregaBrief(material.brief.fechaEntrega ? new Date(material.brief.fechaEntrega).toLocaleDateString('es-MX') : "");

        // Poblar información del material
        self.mensajeMaterial(material.mensaje || "");
        self.pcnMaterial(self.getPCNsString(material));
        self.formatoMaterial(material.formato?.descripcion || "N/A");
        self.estatusMaterial(material.estatusMaterial?.descripcion || "N/A");
        self.audienciaMaterial(self.getAudienciasString(material));
        self.responsableMaterial(material.responsable || "");
        self.areaMaterial(material.area || "");
        self.fechaEntregaMaterial(material.fechaEntrega ? new Date(material.fechaEntrega).toLocaleDateString('es-MX') : "");

        // Poblar información de fecha de publicación
        self.fechaPublicacion(material.fechaPublicacion ? new Date(material.fechaPublicacion).toISOString().split('T')[0] : "");
        self.fechaPublicacionLiberada(material.fechaPublicacionLiberada || false);
        self.fechaPublicacionMaterial(material.fechaPublicacion ? new Date(material.fechaPublicacion).toLocaleDateString('es-MX') : "");
        var EstatusMateriales = self.catEstatusMateriales().find(function (r) {
            return r.id === material.estatusMaterialId;
        });

        // Si el estatus actual del material no está en la lista del usuario (ej: RolId=2 con estatus 4 o 5),
        // agregar temporalmente ese estatus para que pueda agregar comentarios
        if (!EstatusMateriales && material.estatusMaterial) {
            var estatusActual = {
                id: material.estatusMaterialId,
                descripcion: material.estatusMaterial.descripcion
            };
            self.catEstatusMateriales.push(estatusActual);
            EstatusMateriales = estatusActual;
        }

        self.EstatusMateriales(EstatusMateriales);

        // Usar Promise.resolve para asegurar que siempre devuelve una promesa
        Promise.resolve($.get("/Materiales/ObtenerHistorial/" + material.id))
            .then(function (d) {
                self.registrosHistorico(d.datos);
                $("#divEdicion").modal("show");

                // Esperar a que el modal esté completamente abierto y el editor esté listo
                $('#divEdicion').one('shown.bs.modal', function () {
                    // Esperar a que TinyMCE esté completamente inicializado
                    var checkEditor = setInterval(function() {
                        var editor = tinymce.get('myComentario');
                        if (editor && editor.initialized) {
                            clearInterval(checkEditor);
                            $('.tox-statusbar__branding').hide();
                            editor.setContent('');
                            editor.focus();
                            }
                    }, 50);

                    // Timeout de seguridad
                    setTimeout(function() {
                        clearInterval(checkEditor);
                    }, 3000);
                });
            })
            .catch(function (error) {
                console.error("Error al obtener el historial:", error);
                alert("Error al cargar el historial del material.");
            });
    };

    // Guardar comentario
    self.GuardarComentario = function () {
        // Obtener el valor del editor TinyMCE y establecerlo en el observable `Comentario`
        var comentarioContenido = tinymce.get('myComentario').getContent();
        self.Comentario(comentarioContenido);

        // Verificar si todos los campos requeridos están presentes
        if (!self.id() || !self.Comentario() || !self.EstatusMateriales() || !self.fechaEntrega()) {
            alert("Por favor, completa todos los campos requeridos antes de guardar.");
            return;
        }

        // Validar que se hayan agregado participantes si se va a enviar correo
        if (self.envioCorreo() !== "No" && self.registrosUsuariosCorreo().length === 0) {
            alert("Debe agregar al menos un participante para enviar notificaciones por correo.");
            return;
        }

        // Construir el objeto `HistorialMaterial`
        var historialMaterial = {
            MaterialId: self.id(),
            Comentarios: self.Comentario(),
            FechaEntrega: self.fechaEntrega(),
            EstatusMaterialId: self.EstatusMateriales().id,
            FechaPublicacion: self.fechaPublicacion() || null,
            FechaPublicacionLiberada: self.fechaPublicacionLiberada() === "true" || self.fechaPublicacionLiberada() === true
        };

        // Construir la solicitud completa
        var historialMaterialRequest = {
            HistorialMaterial: historialMaterial,
            EnvioCorreo: self.envioCorreo() !== "No",
            Usuarios: self.registrosUsuariosCorreo()
        };

        // Cerrar el modal antes de ejecutar la solicitud
        $("#divEdicion").modal("hide");

        // Iniciar el loader manualmente (por si ajaxSetup no lo detecta)
        $('#loader-overlay').removeClass('d-none');

        // Enviar la solicitud
        $.ajax({
            url: "/Materiales/AgregarHistorialMaterial",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(historialMaterialRequest),
            success: function (response) {
                // Mostrar mensaje de éxito y refrescar los datos
                showAlertModal(response.mensaje);

                self.inicializar();
            },
            error: function (xhr, status, error) {
                console.error("Error al guardar el comentario:", xhr.responseText || error);
                alert("Error al guardar el comentario. Revisa los datos y vuelve a intentar.");
            },
            complete: function () {
                // Ocultar el loader cuando la solicitud termina
                $('#loader-overlay').addClass('d-none');
            }
        });
    };



    // Buscar usuarios con autocompletar
    self.buscarUsuarios = function () {
        if (self.buscarUsuario().length < 3) {
            self.resultadosBusqueda([]); // Limpia resultados si menos de 3 caracteres
            return;
        }

        // Construir el objeto para enviar
        var usuarioRequest = {
            Nombre: self.buscarUsuario()
        };

        // Enviar la solicitud AJAX
        $.ajax({
            url: "/Usuarios/BuscarAllUsuarios",
            type: "POST",
            contentType: "application/json", // Asegúrate de que el Content-Type sea JSON
            data: JSON.stringify(usuarioRequest), // Convertir el objeto a JSON
            success: function (response) {
                self.resultadosBusqueda(response.datos); // Actualizar los resultados con los datos obtenidos
            },
            error: function (xhr, status, error) {
                console.error("Error al buscar usuarios:", xhr.responseText || error);
                alert("Error al buscar usuarios. Revisa los datos y vuelve a intentar.");
            }
        });
    };


    // Seleccionar usuario
    self.seleccionarUsuario = function (usuario) {
        self.registrosUsuariosCorreo.push(usuario);
        self.buscarUsuario("");
        self.resultadosBusqueda([]);

        // Enviar notificación al participante agregado
        if (self.id()) {
            var notificacionRequest = {
                MaterialId: self.id(),
                ParticipanteId: usuario.id
            };

            $.ajax({
                url: "/Materiales/NotificarParticipante",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(notificacionRequest),
                success: function (response) {
                    if (response.exito) {
                        console.log("Notificación enviada al participante:", usuario.nombre);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al enviar notificación:", xhr.responseText || error);
                }
            });
        }
    };

    // Eliminar participante
    self.EliminarParticipante = function (usuario) {
        self.registrosUsuariosCorreo.remove(usuario);
    };

    // Método para setear filtros desde el QueryString
    self.setFiltroFromQueryString = function () {
        const params = new URLSearchParams(window.location.search);
        const filtro = params.get("filtroNombre");
        if (filtro) {
            self.filtroNombre(filtro);
        }
    };
    self.exportarExcel = function () {
        // Obtener los datos filtrados
        var data = self.registrosFiltrados().map(function (registro) {
            return {
                "Nombre de Material": registro.nombre || "",
                "Mensaje": registro.mensaje || "",
                "PCN": self.getPCNsString(registro),
                "Formato": registro.formato?.descripcion || "",
                "Estatus": registro.estatusMaterial?.descripcion || "",
                "Nombre del Proyecto": registro.brief?.nombre || "",
                "Audiencia": self.getAudienciasString(registro),
                "Responsable": registro.responsable || "",
                "Área": registro.area || "",
                "Fecha de Entrega": registro.fechaEntrega ? new Date(registro.fechaEntrega).toLocaleDateString('es-MX') : ""
            };
        });

        // Verificar si hay datos para exportar
        if (data.length === 0) {
            alert("No hay datos para exportar.");
            return;
        }

        // Crear una hoja de trabajo
        var worksheet = XLSX.utils.json_to_sheet(data);

        // Crear un libro de trabajo
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Materiales Filtrados");

        // Generar el archivo Excel y descargarlo
        XLSX.writeFile(workbook, "MaterialesFiltrados.xlsx");
    };

    // Inicializar al cargar
    self.inicializar();
}

// Activar Knockout.js
var appViewModel = new AppViewModel();
ko.applyBindings(appViewModel);
appViewModel.setFiltroFromQueryString();
