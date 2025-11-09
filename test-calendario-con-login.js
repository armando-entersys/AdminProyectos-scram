const puppeteer = require('puppeteer');

(async () => {
    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });

    const page = await browser.newPage();

    const consoleMessages = [];
    const pageErrors = [];
    const requestFailures = [];
    const responseErrors = [];

    // Capturar errores de consola
    page.on('console', msg => {
        const type = msg.type();
        const text = msg.text();
        consoleMessages.push({ type, text });
    });

    // Capturar errores de JavaScript
    page.on('pageerror', error => {
        pageErrors.push(error.message);
    });

    // Capturar fallos de peticiones
    page.on('requestfailed', request => {
        requestFailures.push({
            url: request.url(),
            error: request.failure().errorText
        });
    });

    // Capturar respuestas con error
    page.on('response', response => {
        if (response.status() >= 400) {
            responseErrors.push({
                status: response.status(),
                url: response.url()
            });
        }
    });

    try {
        console.log('=== PASO 1: Navegando al login ===');
        await page.goto('https://adminproyectos.entersys.mx/Account/Login', {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        console.log('=== PASO 2: Ingresando credenciales (demo) ===');
        // Usar credenciales de prueba - ajusta según tu configuración
        await page.type('input[name="Email"]', 'admin@test.com');
        await page.type('input[name="Password"]', 'Admin123$');

        console.log('=== PASO 3: Haciendo clic en el botón de login ===');
        await Promise.all([
            page.waitForNavigation({ waitUntil: 'networkidle0', timeout: 30000 }),
            page.click('button[type="submit"]')
        ]);

        console.log('=== PASO 4: Navegando al Calendario ===');
        await page.goto('https://adminproyectos.entersys.mx/Calendario', {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        console.log('=== PASO 5: Esperando 3 segundos para capturar errores asíncronos ===');
        await new Promise(resolve => setTimeout(resolve, 3000));

        // Tomar screenshot
        await page.screenshot({ path: 'calendario-logged-in.png', fullPage: true });
        console.log('Screenshot guardado: calendario-logged-in.png');

        // Mostrar resultados
        console.log('\n========================================');
        console.log('RESULTADOS DE LA PRUEBA');
        console.log('========================================\n');

        if (consoleMessages.length > 0) {
            console.log('=== MENSAJES DE CONSOLA ===');
            consoleMessages.forEach(msg => {
                if (msg.type === 'error' || msg.type === 'warning') {
                    console.log(`[${msg.type.toUpperCase()}]: ${msg.text}`);
                }
            });
        }

        if (pageErrors.length > 0) {
            console.log('\n=== ERRORES DE JAVASCRIPT ===');
            pageErrors.forEach(err => console.log(`ERROR: ${err}`));
        }

        if (requestFailures.length > 0) {
            console.log('\n=== PETICIONES FALLIDAS ===');
            requestFailures.forEach(fail => {
                console.log(`URL: ${fail.url}`);
                console.log(`Error: ${fail.error}\n`);
            });
        }

        if (responseErrors.length > 0) {
            console.log('\n=== RESPUESTAS CON ERROR (4xx/5xx) ===');
            responseErrors.forEach(err => {
                console.log(`[${err.status}]: ${err.url}`);
            });
        }

        if (consoleMessages.length === 0 && pageErrors.length === 0 &&
            requestFailures.length === 0 && responseErrors.length === 0) {
            console.log('✅ No se encontraron errores');
        }

    } catch (error) {
        console.error('\n❌ ERROR DURANTE LA EJECUCIÓN:', error.message);
        await page.screenshot({ path: 'calendario-error-critical.png', fullPage: true });
    } finally {
        await browser.close();
    }
})();
