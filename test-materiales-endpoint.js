const puppeteer = require('puppeteer');

(async () => {
    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });

    const page = await browser.newPage();

    const errors = [];
    const responses = [];

    // Capturar errores
    page.on('pageerror', error => {
        errors.push({ type: 'pageerror', message: error.message });
    });

    page.on('requestfailed', request => {
        errors.push({
            type: 'requestfailed',
            url: request.url(),
            error: request.failure().errorText
        });
    });

    page.on('response', response => {
        const url = response.url();
        if (url.includes('ObtenerMateriales') || url.includes('Calendario')) {
            responses.push({
                url: url,
                status: response.status(),
                statusText: response.statusText()
            });
        }
    });

    try {
        console.log('=== TEST 1: Verificando endpoint directo ===\n');

        // Primero hacer login
        console.log('Navegando al home (debería redirigir a login)...');
        await page.goto('https://adminproyectos.entersys.mx/', {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        // Obtener cookies si existen (para simular sesión activa)
        const cookies = await page.cookies();
        console.log(`Cookies encontradas: ${cookies.length}`);

        // Intentar acceder al calendario
        console.log('\n=== TEST 2: Accediendo al Calendario ===\n');
        await page.goto('https://adminproyectos.entersys.mx/Calendario', {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        await new Promise(resolve => setTimeout(resolve, 2000));

        // Mostrar resultados
        console.log('\n========================================');
        console.log('RESULTADOS');
        console.log('========================================\n');

        if (responses.length > 0) {
            console.log('=== RESPUESTAS DE ENDPOINTS ===');
            responses.forEach(resp => {
                const statusEmoji = resp.status === 200 ? '✅' : '❌';
                console.log(`${statusEmoji} [${resp.status}] ${resp.url}`);
            });
        }

        if (errors.length > 0) {
            console.log('\n=== ERRORES DETECTADOS ===');
            errors.forEach(err => {
                console.log(`❌ ${err.type}: ${err.message || err.error}`);
                if (err.url) console.log(`   URL: ${err.url}`);
            });
        }

        // Verificar si hay errores 500 en ObtenerMateriales
        const materialErrors = responses.filter(r =>
            r.url.includes('ObtenerMateriales') && r.status >= 500
        );

        if (materialErrors.length === 0 && responses.some(r => r.url.includes('ObtenerMateriales'))) {
            console.log('\n✅ ÉXITO: El endpoint ObtenerMateriales responde correctamente');
        } else if (materialErrors.length > 0) {
            console.log('\n❌ ERROR: El endpoint ObtenerMateriales sigue devolviendo 500');
        } else {
            console.log('\n⚠️  ADVERTENCIA: No se detectaron peticiones a ObtenerMateriales (posiblemente no autenticado)');
        }

        await page.screenshot({ path: 'test-materiales-result.png', fullPage: true });
        console.log('\nScreenshot guardado: test-materiales-result.png');

    } catch (error) {
        console.error('\n❌ ERROR DURANTE LA PRUEBA:', error.message);
    } finally {
        await browser.close();
    }
})();
