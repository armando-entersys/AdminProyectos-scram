const puppeteer = require('puppeteer');

(async () => {
    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });

    const page = await browser.newPage();

    // Capturar errores de consola
    page.on('console', msg => {
        const type = msg.type();
        const text = msg.text();
        console.log(`[CONSOLE ${type}]: ${text}`);
    });

    // Capturar errores de JavaScript
    page.on('pageerror', error => {
        console.error('[PAGE ERROR]:', error.message);
    });

    // Capturar fallos de peticiones
    page.on('requestfailed', request => {
        console.error('[REQUEST FAILED]:', request.url(), request.failure().errorText);
    });

    // Capturar respuestas con error
    page.on('response', response => {
        if (response.status() >= 400) {
            console.error(`[${response.status()}]: ${response.url()}`);
        }
    });

    try {
        console.log('Navegando a https://adminproyectos.entersys.mx/Calendario...');
        await page.goto('https://adminproyectos.entersys.mx/Calendario', {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        console.log('\n=== Página cargada ===\n');

        // Esperar un poco más para ver si hay errores asíncronos
        await new Promise(resolve => setTimeout(resolve, 3000));

        // Verificar si hay algún elemento de error visible
        const errorElements = await page.evaluate(() => {
            const errors = [];
            // Buscar alertas o mensajes de error
            const alerts = document.querySelectorAll('.alert, .error, [class*="error"]');
            alerts.forEach(el => {
                if (el.textContent.trim()) {
                    errors.push(el.textContent.trim());
                }
            });
            return errors;
        });

        if (errorElements.length > 0) {
            console.log('\n=== Elementos de error encontrados ===');
            errorElements.forEach(err => console.log(err));
        }

        // Tomar screenshot
        await page.screenshot({ path: 'calendario-error.png', fullPage: true });
        console.log('\n=== Screenshot guardado: calendario-error.png ===');

    } catch (error) {
        console.error('Error durante la navegación:', error.message);
    } finally {
        await browser.close();
    }
})();
