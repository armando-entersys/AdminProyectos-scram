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
    bright: '\x1b[1m'
};

function log(message, color = 'reset') {
    console.log(`${colors[color]}${message}${colors.reset}`);
}

(async () => {
    const browser = await puppeteer.launch({
        headless: false,
        args: ['--start-maximized'],
        defaultViewport: null
    });

    const page = await browser.newPage();

    // Capturar logs de consola
    const consoleLogs = [];
    page.on('console', msg => {
        const text = msg.text();
        consoleLogs.push(text);
        console.log(`[BROWSER] ${text}`);
    });

    // Capturar errores de red
    page.on('response', response => {
        if (response.status() >= 400) {
            log(`❌ HTTP ${response.status()}: ${response.url()}`, 'red');
        }
    });

    try {
        log('\n=== PRUEBA: CREACIÓN DE BRIEF ===\n', 'bright');

        // ================================================================
        // PASO 1: Login
        // ================================================================
        log('PASO 1: Autenticación...', 'cyan');
        await page.goto(`${BASE_URL}/Login/Index`, { waitUntil: 'networkidle2' });

        await page.waitForSelector('input[type="email"]');
        await page.type('input[type="email"]', 'ajcortest@gmail.com');

        await page.waitForSelector('input[type="password"]');
        await page.type('input[type="password"]', 'Operaciones.2025');

        const loginButton = await page.$('button[type="submit"]');
        if (loginButton) {
            await loginButton.click();
        }

        await page.waitForNavigation({ timeout: 10000 });
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-01-login.png'), fullPage: true });
        log('✅ Login exitoso\n', 'green');

        await new Promise(r => setTimeout(r, 2000));

        // ================================================================
        // PASO 2: Navegar a Briefs
        // ================================================================
        log('PASO 2: Navegando a sección de Briefs...', 'cyan');

        // Intentar varios métodos para llegar a Briefs
        let navegacionExitosa = false;

        // Método 1: Buscar en el menú lateral
        try {
            const menuBrief = await page.$('a[href*="/Brief/Index"]');
            if (menuBrief) {
                await menuBrief.click();
                await page.waitForNavigation({ timeout: 5000 });
                navegacionExitosa = true;
                log('✅ Navegación por menú lateral', 'green');
            }
        } catch (e) {
            log('⚠️  No se encontró menú lateral, intentando otro método...', 'yellow');
        }

        // Método 2: Navegar directamente
        if (!navegacionExitosa) {
            await page.goto(`${BASE_URL}/Brief/Index`, { waitUntil: 'networkidle2' });
            log('✅ Navegación directa a /Brief/Index', 'green');
        }

        await new Promise(r => setTimeout(r, 2000));
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-02-index.png'), fullPage: true });
        log('✅ Página de Briefs cargada\n', 'green');

        // ================================================================
        // PASO 3: Abrir modal de nuevo Brief
        // ================================================================
        log('PASO 3: Abriendo formulario de nuevo Brief...', 'cyan');

        // Buscar botón de agregar
        const botonesAgregar = await page.$$('button');
        let botonEncontrado = false;

        for (const boton of botonesAgregar) {
            const texto = await page.evaluate(el => el.textContent, boton);
            if (texto.includes('Agregar') || texto.includes('Nuevo')) {
                await boton.click();
                botonEncontrado = true;
                log(`✅ Clic en botón: "${texto.trim()}"`, 'green');
                break;
            }
        }

        if (!botonEncontrado) {
            log('❌ No se encontró botón de agregar', 'red');
            throw new Error('No se encontró botón de agregar Brief');
        }

        await new Promise(r => setTimeout(r, 1000));

        // Verificar que el modal esté visible
        const modalVisible = await page.evaluate(() => {
            const modal = document.getElementById('divEdicion');
            return modal && (modal.classList.contains('show') || modal.style.display !== 'none');
        });

        if (modalVisible) {
            log('✅ Modal de Brief abierto correctamente\n', 'green');
        } else {
            log('❌ Modal no se abrió', 'red');
            throw new Error('Modal no visible');
        }

        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-03-modal-vacio.png'), fullPage: true });

        // ================================================================
        // PASO 4: Llenar formulario
        // ================================================================
        log('PASO 4: Llenando formulario del Brief...', 'cyan');

        const timestamp = new Date().getTime();
        const briefData = {
            nombre: `Brief Prueba ${timestamp}`,
            descripcion: 'Este es un brief de prueba automatizada para validar la funcionalidad del sistema',
            objetivo: 'Validar creación de briefs',
            dirigidoA: 'Equipo de desarrollo y QA',
            fechaEntrega: '2025-12-31'
        };

        // Nombre
        const nombreInput = await page.$('#nombre');
        if (nombreInput) {
            await nombreInput.click({ clickCount: 3 }); // Seleccionar todo
            await nombreInput.type(briefData.nombre);
            log(`✅ Nombre: ${briefData.nombre}`, 'green');
        }

        // Descripción
        const descripcionInput = await page.$('#descripcion');
        if (descripcionInput) {
            await descripcionInput.click({ clickCount: 3 });
            await descripcionInput.type(briefData.descripcion);
            log(`✅ Descripción: ${briefData.descripcion}`, 'green');
        }

        // Objetivo
        const objetivoInput = await page.$('#objetivo');
        if (objetivoInput) {
            await objetivoInput.click({ clickCount: 3 });
            await objetivoInput.type(briefData.objetivo);
            log(`✅ Objetivo: ${briefData.objetivo}`, 'green');
        }

        // Dirigido A
        const dirigidoInput = await page.$('#dirigidoA');
        if (dirigidoInput) {
            await dirigidoInput.click({ clickCount: 3 });
            await dirigidoInput.type(briefData.dirigidoA);
            log(`✅ Dirigido A: ${briefData.dirigidoA}`, 'green');
        }

        // Fecha de entrega
        await page.evaluate((fecha) => {
            const input = document.getElementById('fechaEntrega');
            if (input) {
                input.value = fecha;
                input.dispatchEvent(new Event('change'));
            }
        }, briefData.fechaEntrega);
        log(`✅ Fecha de Entrega: ${briefData.fechaEntrega}`, 'green');

        // Tipo de Brief
        const tipoSelect = await page.$('#tipoBrief');
        if (tipoSelect) {
            await page.evaluate(() => {
                const select = document.getElementById('tipoBrief');
                if (select && select.options.length > 1) {
                    select.selectedIndex = 1;
                    select.dispatchEvent(new Event('change'));
                }
            });

            const tipoSeleccionado = await page.evaluate(() => {
                const select = document.getElementById('tipoBrief');
                return select.options[select.selectedIndex].text;
            });
            log(`✅ Tipo de Brief: ${tipoSeleccionado}`, 'green');
        }

        await new Promise(r => setTimeout(r, 500));
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-04-formulario-lleno.png'), fullPage: true });
        log('✅ Formulario completado\n', 'green');

        // ================================================================
        // PASO 5: Guardar Brief
        // ================================================================
        log('PASO 5: Guardando Brief...', 'cyan');

        consoleLogs.length = 0; // Limpiar logs anteriores

        // Buscar botón Guardar
        const botonesGuardar = await page.$$('button');
        let botonGuardarEncontrado = false;

        for (const boton of botonesGuardar) {
            const texto = await page.evaluate(el => el.textContent, boton);
            if (texto.includes('Guardar')) {
                await boton.click();
                botonGuardarEncontrado = true;
                log(`✅ Clic en botón: "${texto.trim()}"`, 'green');
                break;
            }
        }

        if (!botonGuardarEncontrado) {
            log('❌ No se encontró botón Guardar', 'red');
            throw new Error('Botón Guardar no encontrado');
        }

        await new Promise(r => setTimeout(r, 3000));

        // ================================================================
        // PASO 6: Verificar resultado
        // ================================================================
        log('\nPASO 6: Verificando resultado...', 'cyan');

        // Revisar logs de consola
        const erroresValidacion = consoleLogs.filter(log =>
            log.includes('Errores de validación') ||
            log.includes('campos obligatorios') ||
            log.includes('válido: false')
        );

        const erroresServidor = consoleLogs.filter(log =>
            log.includes('error') ||
            log.includes('Error') ||
            log.includes('500')
        );

        if (erroresValidacion.length > 0) {
            log('\n❌ ERRORES DE VALIDACIÓN DETECTADOS:', 'red');
            erroresValidacion.forEach(err => console.log(`   ${err}`));
            log('\n🔍 Logs de validación detallados:', 'yellow');
            consoleLogs.forEach(log => {
                if (log.includes('válido:') || log.includes('Errores')) {
                    console.log(`   ${log}`);
                }
            });
        } else if (erroresServidor.length > 0) {
            log('\n❌ ERRORES DEL SERVIDOR DETECTADOS:', 'red');
            erroresServidor.forEach(err => console.log(`   ${err}`));
        } else {
            log('\n✅ ¡NO SE DETECTARON ERRORES!', 'green');
            log('✅ Brief guardado exitosamente', 'green');
        }

        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-05-resultado.png'), fullPage: true });

        // ================================================================
        // PASO 7: Verificar que el Brief aparece en la lista
        // ================================================================
        log('\nPASO 7: Verificando que el Brief aparece en la lista...', 'cyan');

        await new Promise(r => setTimeout(r, 2000));

        const briefEnLista = await page.evaluate((nombreBrief) => {
            const textoCompleto = document.body.innerText;
            return textoCompleto.includes(nombreBrief);
        }, briefData.nombre);

        if (briefEnLista) {
            log(`✅ Brief "${briefData.nombre}" encontrado en la lista`, 'green');
        } else {
            log(`⚠️  Brief no encontrado visualmente en la página`, 'yellow');
            log('   (Esto podría ser normal si la página necesita recarga)', 'yellow');
        }

        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-06-lista-final.png'), fullPage: true });

        // ================================================================
        // RESUMEN
        // ================================================================
        log('\n' + '='.repeat(60), 'bright');
        log('RESUMEN DE LA PRUEBA', 'bright');
        log('='.repeat(60), 'bright');
        log(`\nDatos del Brief creado:`, 'cyan');
        log(`  • Nombre: ${briefData.nombre}`);
        log(`  • Descripción: ${briefData.descripcion}`);
        log(`  • Objetivo: ${briefData.objetivo}`);
        log(`  • Dirigido A: ${briefData.dirigidoA}`);
        log(`  • Fecha Entrega: ${briefData.fechaEntrega}`);

        log(`\nResultado:`, 'cyan');
        if (erroresValidacion.length === 0 && erroresServidor.length === 0) {
            log('  ✅ PRUEBA EXITOSA - Brief creado sin errores', 'green');
        } else {
            log('  ❌ PRUEBA FALLIDA - Se detectaron errores', 'red');
        }

        log(`\nScreenshots guardados en: ${SCREENSHOTS_DIR}`, 'cyan');
        log('  • brief-01-login.png');
        log('  • brief-02-index.png');
        log('  • brief-03-modal-vacio.png');
        log('  • brief-04-formulario-lleno.png');
        log('  • brief-05-resultado.png');
        log('  • brief-06-lista-final.png');

        log('\n' + '='.repeat(60) + '\n', 'bright');
        log('Presiona Ctrl+C para cerrar el navegador', 'yellow');

        // Mantener navegador abierto
        await new Promise(() => {});

    } catch (e) {
        log(`\n❌ ERROR: ${e.message}`, 'red');
        console.error(e);
        await page.screenshot({ path: path.join(SCREENSHOTS_DIR, 'brief-error.png'), fullPage: true });
        log(`\nScreenshot del error guardado: brief-error.png`, 'yellow');
        process.exit(1);
    }
})();
