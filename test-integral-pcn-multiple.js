const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');

// Configuración
const BASE_URL = 'https://adminproyectos.entersys.mx';
const SCREENSHOTS_DIR = path.join(__dirname, 'test-screenshots');

// Crear directorio de screenshots si no existe
if (!fs.existsSync(SCREENSHOTS_DIR)) {
    fs.mkdirSync(SCREENSHOTS_DIR);
}

// Colores para consola
const colors = {
    reset: '\x1b[0m',
    bright: '\x1b[1m',
    green: '\x1b[32m',
    red: '\x1b[31m',
    yellow: '\x1b[33m',
    blue: '\x1b[34m',
    cyan: '\x1b[36m'
};

function log(message, color = 'reset') {
    console.log(`${colors[color]}${message}${colors.reset}`);
}

function success(message) {
    log(`✅ ${message}`, 'green');
}

function error(message) {
    log(`❌ ${message}`, 'red');
}

function info(message) {
    log(`ℹ️  ${message}`, 'cyan');
}

function warning(message) {
    log(`⚠️  ${message}`, 'yellow');
}

function section(message) {
    log(`\n${'='.repeat(60)}`, 'bright');
    log(message, 'bright');
    log('='.repeat(60), 'bright');
}

async function screenshot(page, name) {
    const filepath = path.join(SCREENSHOTS_DIR, `${name}.png`);
    await page.screenshot({ path: filepath, fullPage: true });
    info(`Screenshot guardado: ${name}.png`);
}

async function waitAndClick(page, selector, description) {
    try {
        await page.waitForSelector(selector, { timeout: 5000 });
        await page.click(selector);
        success(`Clic en: ${description}`);
        return true;
    } catch (e) {
        error(`No se pudo hacer clic en: ${description} (${selector})`);
        return false;
    }
}

async function typeText(page, selector, text, description) {
    try {
        await page.waitForSelector(selector, { timeout: 5000 });
        await page.type(selector, text);
        success(`Texto ingresado en: ${description}`);
        return true;
    } catch (e) {
        error(`No se pudo ingresar texto en: ${description} (${selector})`);
        return false;
    }
}

(async () => {
    const browser = await puppeteer.launch({
        headless: false,
        args: ['--start-maximized'],
        defaultViewport: null
    });

    const page = await browser.newPage();
    let testsPassed = 0;
    let testsFailed = 0;

    // Capturar logs de consola
    const consoleLogs = [];
    page.on('console', msg => {
        const text = msg.text();
        consoleLogs.push(text);
        if (text.includes('ERROR') || text.includes('error') || text.includes('Error')) {
            warning(`BROWSER ERROR: ${text}`);
        }
    });

    // Capturar errores de red
    page.on('response', response => {
        if (response.status() >= 400) {
            error(`HTTP ${response.status()}: ${response.url()}`);
        }
    });

    try {
        section('PRUEBA INTEGRAL - ADMINPROYECTOS (PCN MÚLTIPLE)');

        // ================================================================
        // TEST 1: Cargar página de login
        // ================================================================
        section('TEST 1: Cargar página de login');
        await page.goto(`${BASE_URL}/Login/Index`, { waitUntil: 'networkidle2' });
        await screenshot(page, '01-login-page');

        const loginFormExists = await page.$('input[type="email"]') !== null;
        if (loginFormExists) {
            success('Página de login cargada correctamente');
            testsPassed++;
        } else {
            error('Formulario de login no encontrado');
            testsFailed++;
        }

        // ================================================================
        // TEST 2: Login
        // ================================================================
        section('TEST 2: Autenticación de usuario');

        try {
            // Ingresar email
            await page.waitForSelector('input[type="email"]', { timeout: 5000 });
            await page.type('input[type="email"]', 'ajcortest@gmail.com');
            success('Email ingresado');

            // Ingresar password
            await page.waitForSelector('input[type="password"]', { timeout: 5000 });
            await page.type('input[type="password"]', 'Operaciones.2025');
            success('Password ingresado');

            // Click en botón de login
            const loginButton = await page.$('button[type="submit"]');
            if (loginButton) {
                await loginButton.click();
                success('Clic en botón de login');
            }

            // Esperar navegación
            await page.waitForNavigation({ timeout: 10000 });
            success('Login exitoso');
            testsPassed++;
            await screenshot(page, '02-after-login');
        } catch (e) {
            error(`Error en login: ${e.message}`);
            testsFailed++;
            await screenshot(page, '02-login-error');
        }

        // Esperar un momento para que cargue el dashboard
        await new Promise(r => setTimeout(r, 2000));

        // ================================================================
        // TEST 3: Verificar Home Dashboard
        // ================================================================
        section('TEST 3: Verificar Dashboard Principal');

        // Buscar contadores del dashboard
        const dashboardLoaded = await page.evaluate(() => {
            const checks = {
                hasProyectos: document.body.innerText.includes('Proyectos') || document.body.innerText.includes('Total'),
                hasMateriales: document.body.innerText.includes('Materiales'),
                noErrors: !document.body.innerText.includes('Uncaught') && !document.body.innerText.includes('TypeError')
            };
            return checks;
        });

        if (dashboardLoaded.noErrors) {
            success('Dashboard cargado sin errores JavaScript');
            testsPassed++;
        } else {
            error('Dashboard tiene errores JavaScript');
            testsFailed++;
        }

        await screenshot(page, '03-dashboard');

        // ================================================================
        // TEST 4: Navegar a Briefs
        // ================================================================
        section('TEST 4: Navegar a sección de Briefs');

        const briefsLinkClicked = await waitAndClick(page, 'a[href="/Brief/Index"]', 'Menú Briefs');

        if (briefsLinkClicked) {
            await new Promise(r => setTimeout(r, 2000));
            await screenshot(page, '04-briefs-index');
            testsPassed++;
        } else {
            testsFailed++;
        }

        // ================================================================
        // TEST 5: Abrir modal "Agregar Brief"
        // ================================================================
        section('TEST 5: Abrir formulario de nuevo Brief');

        const addButtonClicked = await waitAndClick(page, 'button[data-bind*="Agregar"]', 'Botón Agregar Brief');

        if (addButtonClicked) {
            await new Promise(r => setTimeout(r, 1000));
            await screenshot(page, '05-modal-agregar-brief');

            // Verificar que el modal esté visible
            const modalVisible = await page.evaluate(() => {
                const modal = document.getElementById('divEdicion');
                return modal && modal.classList.contains('show');
            });

            if (modalVisible) {
                success('Modal de Brief abierto correctamente');
                testsPassed++;
            } else {
                error('Modal no se abrió');
                testsFailed++;
            }
        } else {
            testsFailed++;
        }

        // ================================================================
        // TEST 6: Llenar formulario de Brief
        // ================================================================
        section('TEST 6: Llenar formulario de Brief');

        const timestamp = Date.now();
        const briefData = {
            nombre: `Brief Prueba ${timestamp}`,
            descripcion: 'Este es un brief de prueba automatizada para validar la funcionalidad',
            objetivo: 'Validar funcionalidad del sistema',
            dirigidoA: 'Equipo de desarrollo',
            fechaEntrega: '2025-12-31'
        };

        let formFilled = true;
        formFilled &= await typeText(page, '#nombre', briefData.nombre, 'Nombre');
        formFilled &= await typeText(page, '#descripcion', briefData.descripcion, 'Descripción');
        formFilled &= await typeText(page, '#objetivo', briefData.objetivo, 'Objetivo');
        formFilled &= await typeText(page, '#dirigidoA', briefData.dirigidoA, 'Dirigido A');

        // Fecha de entrega
        await page.waitForSelector('#fechaEntrega');
        await page.evaluate((fecha) => {
            document.getElementById('fechaEntrega').value = fecha;
        }, briefData.fechaEntrega);
        success('Fecha de entrega configurada');

        // Seleccionar Tipo de Brief
        await page.waitForSelector('#tipoBrief');
        await page.select('#tipoBrief', '1'); // Seleccionar primer tipo
        success('Tipo de Brief seleccionado');

        await screenshot(page, '06-formulario-lleno');

        if (formFilled) {
            testsPassed++;
        } else {
            testsFailed++;
        }

        // ================================================================
        // TEST 7: Guardar Brief y verificar validación
        // ================================================================
        section('TEST 7: Guardar Brief');

        // Limpiar logs anteriores
        consoleLogs.length = 0;

        const saveButtonClicked = await waitAndClick(page, 'button[data-bind*="Guardar"]', 'Botón Guardar');

        if (saveButtonClicked) {
            await new Promise(r => setTimeout(r, 2000));

            // Verificar si hay errores de validación en consola
            const hasValidationErrors = consoleLogs.some(log =>
                log.includes('Errores de validación') ||
                log.includes('campos obligatorios')
            );

            if (hasValidationErrors) {
                error('Error de validación al guardar Brief');
                warning('Logs de validación:');
                consoleLogs.forEach(log => {
                    if (log.includes('válido:') || log.includes('Errores')) {
                        console.log(`  ${log}`);
                    }
                });
                testsFailed++;
            } else {
                success('Brief guardado sin errores de validación');
                testsPassed++;
            }

            await screenshot(page, '07-despues-guardar-brief');
        } else {
            testsFailed++;
        }

        // ================================================================
        // TEST 8: Navegar a IndexAdmin (Gestión de Materiales con PCN Múltiple)
        // ================================================================
        section('TEST 8: Navegar a Gestión de Materiales (IndexAdmin)');

        await page.goto(`${BASE_URL}/Brief/IndexAdmin`, { waitUntil: 'networkidle2' });
        await new Promise(r => setTimeout(r, 2000));
        await screenshot(page, '08-index-admin');

        const adminPageLoaded = await page.evaluate(() => {
            return document.body.innerText.includes('Material') ||
                   document.body.innerText.includes('PCN');
        });

        if (adminPageLoaded) {
            success('Página IndexAdmin cargada');
            testsPassed++;
        } else {
            error('Página IndexAdmin no cargó correctamente');
            testsFailed++;
        }

        // ================================================================
        // TEST 9: Seleccionar un Brief para agregar materiales
        // ================================================================
        section('TEST 9: Seleccionar Brief para agregar materiales');

        // Buscar el primer brief disponible
        const briefSelected = await page.evaluate(() => {
            const briefCards = document.querySelectorAll('[data-bind*="nombreBrief"]');
            if (briefCards.length > 0) {
                briefCards[0].click();
                return true;
            }
            return false;
        });

        if (briefSelected) {
            success('Brief seleccionado');
            await new Promise(r => setTimeout(r, 1000));
            testsPassed++;
        } else {
            warning('No se encontraron briefs para seleccionar');
            info('Intentando crear un brief primero...');
            testsFailed++;
        }

        await screenshot(page, '09-brief-seleccionado');

        // ================================================================
        // TEST 10: Abrir modal de Material con PCN Múltiple
        // ================================================================
        section('TEST 10: Abrir formulario de Material (PCN Múltiple)');

        const addMaterialClicked = await waitAndClick(page, 'button[data-bind*="GuardarMaterial"]', 'Botón Agregar Material');

        if (addMaterialClicked) {
            await new Promise(r => setTimeout(r, 1000));
            await screenshot(page, '10-modal-material');
            testsPassed++;
        } else {
            // Intentar con selector alternativo
            info('Intentando selector alternativo para agregar material...');
            const altButtonClicked = await page.evaluate(() => {
                const buttons = Array.from(document.querySelectorAll('button'));
                const addButton = buttons.find(btn => btn.textContent.includes('Agregar') || btn.textContent.includes('Material'));
                if (addButton) {
                    addButton.click();
                    return true;
                }
                return false;
            });

            if (altButtonClicked) {
                await new Promise(r => setTimeout(r, 1000));
                success('Modal de material abierto (selector alternativo)');
                testsPassed++;
            } else {
                testsFailed++;
            }
        }

        // ================================================================
        // TEST 11: PRUEBA CRÍTICA - Seleccionar múltiples PCN (Checkboxes)
        // ================================================================
        section('TEST 11: CRÍTICO - Seleccionar múltiples PCN');

        const pcnCheckboxesFound = await page.evaluate(() => {
            const checkboxes = document.querySelectorAll('input[type="checkbox"][id^="pcn_"]');
            return checkboxes.length;
        });

        if (pcnCheckboxesFound > 0) {
            success(`Encontrados ${pcnCheckboxesFound} checkboxes de PCN`);

            // Seleccionar los primeros 2 PCN
            const pcnsSelected = await page.evaluate(() => {
                const checkboxes = document.querySelectorAll('input[type="checkbox"][id^="pcn_"]');
                let selected = 0;
                for (let i = 0; i < Math.min(2, checkboxes.length); i++) {
                    checkboxes[i].click();
                    if (checkboxes[i].checked) {
                        selected++;
                    }
                }
                return selected;
            });

            if (pcnsSelected >= 2) {
                success(`${pcnsSelected} PCN seleccionados correctamente`);
                testsPassed++;
            } else {
                error(`Solo se seleccionaron ${pcnsSelected} PCN`);
                testsFailed++;
            }

            await screenshot(page, '11-pcn-multiples-seleccionados');
        } else {
            error('No se encontraron checkboxes de PCN - LA FUNCIONALIDAD PCN MÚLTIPLE NO ESTÁ FUNCIONANDO');
            testsFailed++;
        }

        // ================================================================
        // TEST 12: Llenar datos del Material
        // ================================================================
        section('TEST 12: Llenar formulario de Material');

        const materialData = {
            nombre: `Material Prueba ${timestamp}`,
            mensaje: 'Mensaje de prueba automatizada',
            ciclo: '2025',
            responsable: 'Equipo QA',
            area: 'Desarrollo'
        };

        // Intentar llenar campos del material
        let materialFormFilled = true;

        try {
            await page.waitForSelector('input[data-bind*="nombreMaterial"]', { timeout: 3000 });
            await page.type('input[data-bind*="nombreMaterial"]', materialData.nombre);
            success('Nombre del material ingresado');
        } catch (e) {
            warning('Campo nombre no encontrado o no accesible');
            materialFormFilled = false;
        }

        // Seleccionar prioridad
        try {
            const prioridadSelect = await page.$('select[data-bind*="prioridad"]');
            if (prioridadSelect) {
                await page.evaluate(() => {
                    const select = document.querySelector('select[data-bind*="prioridad"]');
                    if (select && select.options.length > 1) {
                        select.selectedIndex = 1;
                        select.dispatchEvent(new Event('change'));
                    }
                });
                success('Prioridad seleccionada');
            }
        } catch (e) {
            warning('No se pudo seleccionar prioridad');
        }

        // Seleccionar audiencia
        try {
            const audienciaSelect = await page.$('select[data-bind*="audiencia"]');
            if (audienciaSelect) {
                await page.evaluate(() => {
                    const select = document.querySelector('select[data-bind*="audiencia"]');
                    if (select && select.options.length > 1) {
                        select.selectedIndex = 1;
                        select.dispatchEvent(new Event('change'));
                    }
                });
                success('Audiencia seleccionada');
            }
        } catch (e) {
            warning('No se pudo seleccionar audiencia');
        }

        // Seleccionar formato
        try {
            const formatoSelect = await page.$('select[data-bind*="formato"]');
            if (formatoSelect) {
                await page.evaluate(() => {
                    const select = document.querySelector('select[data-bind*="formato"]');
                    if (select && select.options.length > 1) {
                        select.selectedIndex = 1;
                        select.dispatchEvent(new Event('change'));
                    }
                });
                success('Formato seleccionado');
            }
        } catch (e) {
            warning('No se pudo seleccionar formato');
        }

        await screenshot(page, '12-material-formulario-lleno');

        if (materialFormFilled) {
            testsPassed++;
        } else {
            testsFailed++;
        }

        // ================================================================
        // TEST 13: Guardar Material con múltiples PCN
        // ================================================================
        section('TEST 13: CRÍTICO - Guardar Material con múltiples PCN');

        consoleLogs.length = 0;

        try {
            const saveButton = await page.$('button[data-bind*="GuardarMaterial"]');
            if (saveButton) {
                await saveButton.click();
                success('Clic en guardar material');

                await new Promise(r => setTimeout(r, 3000));

                // Verificar si hubo errores
                const hasErrors = consoleLogs.some(log =>
                    log.includes('error') ||
                    log.includes('Error') ||
                    log.includes('500')
                );

                if (hasErrors) {
                    error('Error al guardar material con múltiples PCN');
                    error('Logs relevantes:');
                    consoleLogs.forEach(log => {
                        if (log.includes('error') || log.includes('Error') || log.includes('500')) {
                            console.log(`  ${log}`);
                        }
                    });
                    testsFailed++;
                } else {
                    success('Material guardado sin errores aparentes');
                    testsPassed++;
                }

                await screenshot(page, '13-despues-guardar-material');
            }
        } catch (e) {
            error(`Error al intentar guardar material: ${e.message}`);
            testsFailed++;
        }

        // ================================================================
        // TEST 14: Verificar tabla de Materiales
        // ================================================================
        section('TEST 14: Verificar que los materiales se muestren correctamente');

        await page.goto(`${BASE_URL}/Materiales/Index`, { waitUntil: 'networkidle2' });
        await new Promise(r => setTimeout(r, 3000));
        await screenshot(page, '14-materiales-index');

        const materialesTableExists = await page.evaluate(() => {
            const table = document.querySelector('table');
            const hasPCNColumn = document.body.innerText.includes('PCN');
            const hasRows = table && table.querySelectorAll('tbody tr').length > 0;
            return { hasPCNColumn, hasRows };
        });

        if (materialesTableExists.hasPCNColumn) {
            success('Columna PCN existe en la tabla de materiales');
            testsPassed++;
        } else {
            error('Columna PCN NO encontrada en la tabla');
            testsFailed++;
        }

        if (materialesTableExists.hasRows) {
            success('Tabla contiene registros de materiales');
            testsPassed++;
        } else {
            warning('Tabla de materiales está vacía');
        }

        // ================================================================
        // TEST 15: Verificar serialización JSON (sin ciclos)
        // ================================================================
        section('TEST 15: Verificar que no haya errores de serialización JSON');

        const hasSerializationErrors = consoleLogs.some(log =>
            log.includes('object cycle') ||
            log.includes('circular') ||
            log.includes('depth')
        );

        if (!hasSerializationErrors) {
            success('No se detectaron errores de serialización JSON (ciclos)');
            testsPassed++;
        } else {
            error('Se detectaron errores de serialización JSON');
            testsFailed++;
        }

        // ================================================================
        // RESUMEN FINAL
        // ================================================================
        section('RESUMEN DE PRUEBAS');

        const totalTests = testsPassed + testsFailed;
        const successRate = ((testsPassed / totalTests) * 100).toFixed(2);

        console.log('');
        log(`Total de pruebas: ${totalTests}`, 'bright');
        log(`✅ Exitosas: ${testsPassed}`, 'green');
        log(`❌ Fallidas: ${testsFailed}`, 'red');
        log(`📊 Tasa de éxito: ${successRate}%`, testsFailed === 0 ? 'green' : 'yellow');
        console.log('');

        if (testsFailed === 0) {
            success('🎉 ¡TODAS LAS PRUEBAS PASARON! 🎉');
        } else {
            warning(`⚠️  ${testsFailed} prueba(s) fallaron. Revisa los screenshots y logs.`);
        }

        info(`Screenshots guardados en: ${SCREENSHOTS_DIR}`);

        console.log('');
        info('Presiona Ctrl+C para cerrar el navegador y salir');

        // Mantener el navegador abierto
        await new Promise(() => {});

    } catch (e) {
        error(`Error general en pruebas: ${e.message}`);
        console.error(e);
        await screenshot(page, 'error-general');
    }
})();
