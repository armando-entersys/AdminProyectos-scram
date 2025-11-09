/**
 * Script para verificar errores en ELMAH
 */

const puppeteer = require('puppeteer');

async function checkElmahErrors() {
    console.log('🔍 Verificando errores en ELMAH...\n');

    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });

    try {
        const page = await browser.newPage();

        // Navegar a ELMAH
        console.log('📡 Conectando a https://adminproyectos.entersys.mx/elmah...');
        await page.goto('https://adminproyectos.entersys.mx/elmah', {
            waitUntil: 'networkidle2',
            timeout: 30000
        });

        // Esperar a que cargue el contenido
        await new Promise(resolve => setTimeout(resolve, 2000));

        // Verificar si hay tabla de errores
        const hasErrorTable = await page.evaluate(() => {
            const table = document.querySelector('table');
            return !!table;
        });

        if (!hasErrorTable) {
            console.log('✅ No se encontró tabla de errores - ELMAH está limpio o vacío\n');
            await browser.close();
            return;
        }

        // Obtener los errores
        const errors = await page.evaluate(() => {
            const rows = Array.from(document.querySelectorAll('table tr'));

            // Saltar el header
            return rows.slice(1).map(row => {
                const cells = Array.from(row.querySelectorAll('td'));
                if (cells.length === 0) return null;

                return {
                    host: cells[0]?.textContent?.trim() || '',
                    code: cells[1]?.textContent?.trim() || '',
                    type: cells[2]?.textContent?.trim() || '',
                    error: cells[3]?.textContent?.trim() || '',
                    user: cells[4]?.textContent?.trim() || '',
                    date: cells[5]?.textContent?.trim() || '',
                    time: cells[6]?.textContent?.trim() || ''
                };
            }).filter(e => e !== null);
        });

        if (errors.length === 0) {
            console.log('✅ No hay errores registrados en ELMAH\n');
        } else {
            console.log(`⚠️  Se encontraron ${errors.length} error(es) en ELMAH:\n`);
            console.log('═'.repeat(120));

            errors.forEach((error, index) => {
                console.log(`\n${index + 1}. Error ${error.code || 'N/A'}`);
                console.log(`   Tipo: ${error.type}`);
                console.log(`   Mensaje: ${error.error}`);
                console.log(`   Usuario: ${error.user || 'N/A'}`);
                console.log(`   Fecha: ${error.date} ${error.time}`);
                console.log(`   Host: ${error.host}`);
                console.log('─'.repeat(120));
            });

            // Resumen por tipo de error
            console.log('\n📊 Resumen por Tipo de Error:\n');
            const errorTypes = errors.reduce((acc, err) => {
                acc[err.type] = (acc[err.type] || 0) + 1;
                return acc;
            }, {});

            Object.entries(errorTypes).forEach(([type, count]) => {
                console.log(`   ${type}: ${count} ocurrencia(s)`);
            });
        }

        // Tomar screenshot
        await page.screenshot({
            path: 'elmah-screenshot.png',
            fullPage: true
        });
        console.log('\n📸 Screenshot guardado en: elmah-screenshot.png');

    } catch (error) {
        console.error('\n❌ Error al verificar ELMAH:', error.message);
    } finally {
        await browser.close();
    }
}

checkElmahErrors().catch(console.error);
