/**
 * Script de Validación de Catálogos Base
 * Verifica que los datos documentados en 02-Base-de-Datos.md coincidan con la realidad
 */

const puppeteer = require('puppeteer');

const BASE_URL = 'https://adminproyectos.entersys.mx';

// Datos esperados según documentación
const EXPECTED_CATALOGS = {
    Prioridad: [
        { id: 1, descripcion: 'Alta' },
        { id: 2, descripcion: 'Media' },
        { id: 3, descripcion: 'Baja' }
    ],
    PCN: [
        { id: 1, descripcion: 'Consultoras' },
        { id: 2, descripcion: 'Líderes' },
        { id: 3, descripcion: 'GNs/GDs' },
        { id: 4, descripcion: 'Gerentes Regionales' },
        { id: 5, descripcion: 'Equipo Natura' }
    ],
    Audiencia: [
        { id: 1, descripcion: 'Consultoras' },
        { id: 2, descripcion: 'Líderes' },
        { id: 3, descripcion: 'GNs' },
        { id: 4, descripcion: 'GDs' },
        { id: 5, descripcion: 'Gerentes' },
        { id: 6, descripcion: 'Equipo Interno' }
    ],
    Formato: [
        { id: 1, descripcion: 'Kit Digital' },
        { id: 2, descripcion: 'Post' },
        { id: 3, descripcion: 'Stories' },
        { id: 4, descripcion: 'Video' },
        { id: 5, descripcion: 'Banner' },
        { id: 6, descripcion: 'Flyer' },
        { id: 7, descripcion: 'Presentación' },
        { id: 8, descripcion: 'Plantilla' },
        { id: 9, descripcion: 'Diseño de Mailing' }
    ],
    EstatusMateriales: [
        { id: 1, descripcion: 'En Revisión' },
        { id: 2, descripcion: 'En Producción' },
        { id: 3, descripcion: 'Falta Información' },
        { id: 4, descripcion: 'Aprobado' },
        { id: 5, descripcion: 'Entregado' },
        { id: 6, descripcion: 'Programado' }
    ]
};

async function validateCatalogs() {
    console.log('╔' + '═'.repeat(68) + '╗');
    console.log('║' + ' '.repeat(15) + 'VALIDACIÓN DE CATÁLOGOS BASE' + ' '.repeat(25) + '║');
    console.log('╚' + '═'.repeat(68) + '╝\n');

    const browser = await puppeteer.launch({
        headless: false,
        args: ['--no-sandbox', '--disable-setuid-sandbox'],
        defaultViewport: { width: 1920, height: 1080 }
    });

    try {
        const page = await browser.newPage();

        // Login como administrador para acceder a catálogos
        console.log('🔐 Iniciando sesión como Administrador...');
        console.log('⚠️  Por favor ingresa las credenciales manualmente en el navegador que se abrirá.\n');

        await page.goto(`${BASE_URL}/Login`, { waitUntil: 'networkidle2' });

        // Esperar a que el usuario haga login manualmente
        console.log('⏳ Esperando login manual...');
        await page.waitForFunction(() => {
            return !window.location.href.includes('/Login');
        }, { timeout: 120000 }); // 2 minutos para login manual

        console.log('✅ Login completado\n');

        // Navegar a catálogos
        await page.goto(`${BASE_URL}/Catalogos`, { waitUntil: 'networkidle2' });
        await page.waitForTimeout(2000);

        // Verificar cada catálogo
        for (const [catalogName, expectedData] of Object.entries(EXPECTED_CATALOGS)) {
            console.log(`\n📋 Validando catálogo: ${catalogName}`);
            console.log('─'.repeat(70));

            // Obtener datos del catálogo desde la página
            const actualData = await page.evaluate((name) => {
                const catalogKey = `cat${name}`;
                if (appViewModel && appViewModel[catalogKey]) {
                    return appViewModel[catalogKey]().map(item => ({
                        id: item.id,
                        descripcion: item.descripcion
                    }));
                }
                return null;
            }, catalogName);

            if (!actualData) {
                console.log(`❌ No se pudo obtener datos de ${catalogName}`);
                continue;
            }

            console.log(`   Registros esperados: ${expectedData.length}`);
            console.log(`   Registros encontrados: ${actualData.length}`);

            // Verificar cantidad
            if (actualData.length !== expectedData.length) {
                console.log(`   ⚠️  ADVERTENCIA: Cantidad de registros no coincide`);
            }

            // Verificar cada registro
            let matches = 0;
            let mismatches = 0;

            for (const expected of expectedData) {
                const actual = actualData.find(a => a.id === expected.id);

                if (!actual) {
                    console.log(`   ❌ Falta registro ID ${expected.id}: ${expected.descripcion}`);
                    mismatches++;
                } else if (actual.descripcion !== expected.descripcion) {
                    console.log(`   ⚠️  ID ${expected.id}: Esperado "${expected.descripcion}", Encontrado "${actual.descripcion}"`);
                    mismatches++;
                } else {
                    matches++;
                }
            }

            // Verificar registros adicionales
            for (const actual of actualData) {
                const expected = expectedData.find(e => e.id === actual.id);
                if (!expected) {
                    console.log(`   ➕ Registro adicional ID ${actual.id}: ${actual.descripcion}`);
                }
            }

            if (matches === expectedData.length && actualData.length === expectedData.length) {
                console.log(`   ✅ Catálogo ${catalogName} validado correctamente (${matches}/${expectedData.length})`);
            } else {
                console.log(`   ⚠️  Catálogo ${catalogName} tiene inconsistencias (${matches}/${expectedData.length} correctos, ${mismatches} errores)`);
            }
        }

        console.log('\n' + '═'.repeat(70));
        console.log('✅ Validación de catálogos completada');

    } catch (error) {
        console.error('\n❌ Error durante la validación:', error.message);
    } finally {
        await browser.close();
    }
}

// Ejecutar
validateCatalogs().catch(console.error);
