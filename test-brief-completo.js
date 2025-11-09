const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');

const BASE_URL = 'https://adminproyectos.entersys.mx';
const SCREENSHOTS_DIR = path.join(__dirname, 'test-screenshots');

if (!fs.existsSync(SCREENSHOTS_DIR)) {
    fs.mkdirSync(SCREENSHOTS_DIR);
}

const colors = {
    reset: '\x1b[0m',
    green: '\x1b[32m',
    red: '\x1b[31m',
    yellow: '\x1b[33m',
    cyan: '\x1b[36m',
    bright: '\x1b[1m',
    blue: '\x1b[34m'
};

function log(message, color = 'reset') {
    console.log(`${colors[color]}${message}${colors.reset}`);
}

function section(title) {
    console.log('\n' + '='.repeat(70));
    log(title, 'bright');
    console.log('='.repeat(70) + '\n');
}

(async () => {
    const browser = await puppeteer.launch({
        headless: false,
        args: ['--start-maximized'],
        defaultViewport: null
    });

    const page = await browser.newPage();
    let testResults = {
        login: false,
        navegacion: false,
        edicionBrief: false,
        creacionBrief: false
    };

    // Capturar logs de consola
    const consoleLogs = [];
    page.on('console', msg => {
        const text = msg.text();
        consoleLogs.push(text);
        if (text.includes('error') || text.includes('Error')) {
            log(`[BROWSER ERROR] ${text}`, 'red');
        }
    });

    // Capturar errores de red
    page.on('response', response => {
        if (response.status() >= 400) {
            log(`❌ HTTP ${response.status()}: ${response.url()}`, 'red');
        }
    });

    try {
        section('PRUEBA COMPLETA: EDITAR Y CREAR BRIEF');

        // ================================================================
        // PASO 1: Login
        // ================================================================
        section('PASO 1: Autenticación');

        await page.goto(`${BASE_URL}/Login/Index`, { waitUntil: 'networkidle2' });
        await page.waitForSelector('input[type="email"]');
        await page.type('input[type="email"]', 'ajcortest@gmail.com');
        await page.type('input[type="password"]', 'Operaciones.2025');

        const loginButton = await page.$('button[type="submit"]');
        if (loginButton) {
            await loginButton.click();
        }

        await page.waitForNavigation({ timeout: 10000 });
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-01-login.png'), fullPage: true });
        log('✅ Login exitoso', 'green');
        testResults.login = true;

        await new Promise(r => setTimeout(r, 2000));

        // ================================================================
        // PASO 2: Ir a vista de lista de Briefs (no Kanban)
        // ================================================================
        section('PASO 2: Navegando a vista de lista de Briefs');

        // Intentar navegar directamente a la vista de lista
        let paginaBriefsLista = false;

        // Método 1: Buscar botón "Briefs" en el menú superior
        try {
            const botonBriefs = await page.evaluate(() => {
                const botones = Array.from(document.querySelectorAll('button, a'));
                const botonBrief = botones.find(btn => {
                    const texto = btn.textContent.toLowerCase();
                    return texto.includes('brief') && !texto.includes('material');
                });
                if (botonBrief) {
                    botonBrief.click();
                    return true;
                }
                return false;
            });

            if (botonBriefs) {
                await new Promise(r => setTimeout(r, 2000));
                paginaBriefsLista = true;
                log('✅ Navegación por botón Brief en menú superior', 'green');
            }
        } catch (e) {
            log('⚠️  Método 1 fallido, intentando método 2...', 'yellow');
        }

        // Método 2: Navegar al sidebar y buscar el enlace
        if (!paginaBriefsLista) {
            try {
                const menuLateral = await page.$('a[href="/Brief/Index"]');
                if (menuLateral) {
                    await menuLateral.click();
                    await new Promise(r => setTimeout(r, 2000));
                    paginaBriefsLista = true;
                    log('✅ Navegación por menú lateral', 'green');
                }
            } catch (e) {
                log('⚠️  Método 2 fallido, intentando método 3...', 'yellow');
            }
        }

        // Verificar si estamos en IndexAdmin (Kanban) y necesitamos cambiar de vista
        const esVistaKanban = await page.evaluate(() => {
            return document.body.innerText.includes('Gestión de Proyectos') ||
                   document.body.innerText.includes('En revisión');
        });

        if (esVistaKanban) {
            log('ℹ️  Estamos en vista Kanban (IndexAdmin)', 'cyan');
            log('ℹ️  Buscando forma de cambiar a vista de lista...', 'cyan');

            // Intentar ir directamente por URL
            await page.goto(`${BASE_URL}/Brief/Index`, { waitUntil: 'networkidle2' });
            await new Promise(r => setTimeout(r, 2000));
        }

        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-02-briefs-page.png'), fullPage: true });
        testResults.navegacion = true;

        // ================================================================
        // PASO 3: EDITAR UN BRIEF EXISTENTE
        // ================================================================
        section('PASO 3: Editando un Brief existente');

        // Buscar el primer brief en la tabla/lista
        const briefParaEditar = await page.evaluate(() => {
            // Buscar botón de editar (puede tener diferentes textos)
            const botonesEditar = Array.from(document.querySelectorAll('button, a'));
            const botonEditar = botonesEditar.find(btn => {
                const texto = btn.textContent.toLowerCase();
                const atributos = btn.getAttribute('data-bind') || '';
                return texto.includes('editar') ||
                       texto.includes('edit') ||
                       atributos.includes('Editar');
            });

            if (botonEditar) {
                botonEditar.click();
                return true;
            }

            // Si no hay botón de editar, buscar una tarjeta de brief
            const tarjetas = document.querySelectorAll('[data-bind*="Ver Detalles"], button');
            for (const tarjeta of tarjetas) {
                if (tarjeta.textContent.includes('Ver Detalles')) {
                    tarjeta.click();
                    return true;
                }
            }

            return false;
        });

        if (briefParaEditar) {
            await new Promise(r => setTimeout(r, 2000));
            await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-03-brief-editar-modal.png'), fullPage: true });

            // Verificar que el modal de edición esté abierto
            const modalEdicionAbierto = await page.evaluate(() => {
                const modal = document.getElementById('divEdicion');
                return modal && (modal.classList.contains('show') || modal.style.display !== 'none');
            });

            if (modalEdicionAbierto) {
                log('✅ Modal de edición abierto', 'green');

                // Modificar el nombre del brief
                const nombreActual = await page.evaluate(() => {
                    const input = document.getElementById('nombre');
                    return input ? input.value : '';
                });

                log(`ℹ️  Nombre actual: "${nombreActual}"`, 'cyan');

                // Agregar timestamp al nombre
                const nuevoNombre = `${nombreActual} [EDITADO ${Date.now()}]`;

                await page.evaluate((nombre) => {
                    const input = document.getElementById('nombre');
                    if (input) {
                        input.value = nombre;
                        input.dispatchEvent(new Event('input'));
                        input.dispatchEvent(new Event('change'));
                    }
                }, nuevoNombre);

                log(`✅ Nuevo nombre: "${nuevoNombre}"`, 'green');

                await new Promise(r => setTimeout(r, 500));
                await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-04-brief-editado.png'), fullPage: true });

                // Guardar cambios
                consoleLogs.length = 0;

                const botonGuardar = await page.evaluate(() => {
                    const botones = Array.from(document.querySelectorAll('button'));
                    const guardar = botones.find(btn => btn.textContent.includes('Guardar'));
                    if (guardar) {
                        guardar.click();
                        return true;
                    }
                    return false;
                });

                if (botonGuardar) {
                    log('✅ Clic en Guardar', 'green');
                    await new Promise(r => setTimeout(r, 3000));

                    // Verificar errores
                    const errores = consoleLogs.filter(log =>
                        log.includes('error') ||
                        log.includes('Error') ||
                        log.includes('válido: false')
                    );

                    if (errores.length === 0) {
                        log('✅ Brief editado sin errores', 'green');
                        testResults.edicionBrief = true;
                    } else {
                        log('❌ Errores al editar brief:', 'red');
                        errores.forEach(e => log(`   ${e}`, 'red'));
                    }

                    await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-05-despues-editar.png'), fullPage: true });
                } else {
                    log('❌ No se encontró botón Guardar', 'red');
                }
            } else {
                log('❌ Modal de edición no se abrió', 'red');
            }
        } else {
            log('⚠️  No se encontró brief para editar', 'yellow');
            log('ℹ️  Continuando con creación de brief nuevo...', 'cyan');
        }

        // ================================================================
        // PASO 4: CREAR UN NUEVO BRIEF
        // ================================================================
        section('PASO 4: Creando un nuevo Brief');

        // Asegurarnos de estar en la página correcta
        await page.goto(`${BASE_URL}/Brief/Index`, { waitUntil: 'networkidle2' });
        await new Promise(r => setTimeout(r, 2000));

        // Buscar botón "Agregar" o "Nuevo Brief"
        const botonAgregar = await page.evaluate(() => {
            const botones = Array.from(document.querySelectorAll('button'));
            const botonAgregar = botones.find(btn => {
                const texto = btn.textContent.toLowerCase();
                const atributos = btn.getAttribute('data-bind') || '';
                return texto.includes('agregar') ||
                       texto.includes('nuevo') ||
                       atributos.includes('Agregar');
            });

            if (botonAgregar) {
                // Scroll al elemento
                botonAgregar.scrollIntoView({ behavior: 'smooth', block: 'center' });
                setTimeout(() => botonAgregar.click(), 500);
                return true;
            }
            return false;
        });

        if (botonAgregar) {
            await new Promise(r => setTimeout(r, 2000));
            log('✅ Botón Agregar clickeado', 'green');

            // Verificar que el modal esté abierto
            const modalNuevoAbierto = await page.evaluate(() => {
                const modal = document.getElementById('divEdicion');
                return modal && (modal.classList.contains('show') || modal.style.display !== 'none');
            });

            if (modalNuevoAbierto) {
                log('✅ Modal de nuevo Brief abierto', 'green');
                await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-06-modal-nuevo.png'), fullPage: true });

                // Llenar el formulario
                const timestamp = Date.now();
                const briefData = {
                    nombre: `Prueba Automatizada ${timestamp}`,
                    descripcion: 'Este es un brief creado mediante prueba automatizada para validar la funcionalidad completa del sistema',
                    objetivo: 'Validar creación de briefs',
                    dirigidoA: 'Equipo de Desarrollo',
                    fechaEntrega: '2025-12-31'
                };

                log('ℹ️  Llenando formulario...', 'cyan');

                // Llenar cada campo
                await page.evaluate((data) => {
                    const campos = {
                        nombre: document.getElementById('nombre'),
                        descripcion: document.getElementById('descripcion'),
                        objetivo: document.getElementById('objetivo'),
                        dirigidoA: document.getElementById('dirigidoA'),
                        fechaEntrega: document.getElementById('fechaEntrega'),
                        tipoBrief: document.getElementById('tipoBrief')
                    };

                    if (campos.nombre) {
                        campos.nombre.value = data.nombre;
                        campos.nombre.dispatchEvent(new Event('input'));
                        campos.nombre.dispatchEvent(new Event('change'));
                    }

                    if (campos.descripcion) {
                        campos.descripcion.value = data.descripcion;
                        campos.descripcion.dispatchEvent(new Event('input'));
                        campos.descripcion.dispatchEvent(new Event('change'));
                    }

                    if (campos.objetivo) {
                        campos.objetivo.value = data.objetivo;
                        campos.objetivo.dispatchEvent(new Event('input'));
                        campos.objetivo.dispatchEvent(new Event('change'));
                    }

                    if (campos.dirigidoA) {
                        campos.dirigidoA.value = data.dirigidoA;
                        campos.dirigidoA.dispatchEvent(new Event('input'));
                        campos.dirigidoA.dispatchEvent(new Event('change'));
                    }

                    if (campos.fechaEntrega) {
                        campos.fechaEntrega.value = data.fechaEntrega;
                        campos.fechaEntrega.dispatchEvent(new Event('input'));
                        campos.fechaEntrega.dispatchEvent(new Event('change'));
                    }

                    if (campos.tipoBrief && campos.tipoBrief.options.length > 1) {
                        campos.tipoBrief.selectedIndex = 1;
                        campos.tipoBrief.dispatchEvent(new Event('change'));
                    }
                }, briefData);

                await new Promise(r => setTimeout(r, 1000));

                log(`✅ Nombre: ${briefData.nombre}`, 'green');
                log(`✅ Descripción: ${briefData.descripcion}`, 'green');
                log(`✅ Objetivo: ${briefData.objetivo}`, 'green');
                log(`✅ Dirigido A: ${briefData.dirigidoA}`, 'green');
                log(`✅ Fecha: ${briefData.fechaEntrega}`, 'green');
                log(`✅ Tipo de Brief: Seleccionado`, 'green');

                await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-07-formulario-lleno.png'), fullPage: true });

                // Guardar
                consoleLogs.length = 0;

                const guardado = await page.evaluate(() => {
                    const botones = Array.from(document.querySelectorAll('button'));
                    const guardar = botones.find(btn => btn.textContent.includes('Guardar'));
                    if (guardar) {
                        guardar.click();
                        return true;
                    }
                    return false;
                });

                if (guardado) {
                    log('✅ Clic en Guardar', 'green');
                    await new Promise(r => setTimeout(r, 3000));

                    // Verificar errores
                    const errores = consoleLogs.filter(log =>
                        log.includes('error') ||
                        log.includes('Error') ||
                        log.includes('válido: false') ||
                        log.includes('campos obligatorios')
                    );

                    if (errores.length === 0) {
                        log('✅ ¡Brief creado exitosamente!', 'green');
                        testResults.creacionBrief = true;
                    } else {
                        log('❌ Errores al crear brief:', 'red');
                        errores.forEach(e => log(`   ${e}`, 'red'));

                        log('\n🔍 Logs de validación:', 'yellow');
                        consoleLogs.forEach(log => {
                            if (log.includes('válido:')) {
                                console.log(`   ${log}`);
                            }
                        });
                    }

                    await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-08-despues-crear.png'), fullPage: true });
                } else {
                    log('❌ No se encontró botón Guardar', 'red');
                }
            } else {
                log('❌ Modal de nuevo Brief no se abrió', 'red');
            }
        } else {
            log('❌ No se encontró botón Agregar', 'red');
        }

        // ================================================================
        // RESUMEN FINAL
        // ================================================================
        section('RESUMEN DE PRUEBAS');

        const resultados = [
            { nombre: 'Login', resultado: testResults.login },
            { nombre: 'Navegación', resultado: testResults.navegacion },
            { nombre: 'Edición de Brief', resultado: testResults.edicionBrief },
            { nombre: 'Creación de Brief', resultado: testResults.creacionBrief }
        ];

        console.log('');
        resultados.forEach(test => {
            const icono = test.resultado ? '✅' : '❌';
            const color = test.resultado ? 'green' : 'red';
            log(`${icono} ${test.nombre}`, color);
        });

        const totalExitosos = resultados.filter(t => t.resultado).length;
        const totalPruebas = resultados.length;
        const porcentaje = ((totalExitosos / totalPruebas) * 100).toFixed(0);

        console.log('');
        log(`Pruebas exitosas: ${totalExitosos}/${totalPruebas} (${porcentaje}%)`,
            totalExitosos === totalPruebas ? 'green' : 'yellow');

        if (totalExitosos === totalPruebas) {
            log('\n🎉 ¡TODAS LAS PRUEBAS PASARON! 🎉', 'green');
        }

        log(`\nScreenshots guardados en: ${SCREENSHOTS_DIR}`, 'cyan');
        log('\nPresiona Ctrl+C para cerrar', 'yellow');

        // Mantener abierto
        await new Promise(() => {});

    } catch (e) {
        log(`\n❌ ERROR GENERAL: ${e.message}`, 'red');
        console.error(e);
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'test-error-general.png'), fullPage: true });
        process.exit(1);
    }
})();
