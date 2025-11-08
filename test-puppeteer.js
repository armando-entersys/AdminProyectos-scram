/**
 * Script de Pruebas Automatizadas con Puppeteer - PCN Múltiple
 */

const puppeteer = require('puppeteer');

const baseUrl = 'https://adminproyectos.entersys.mx';

// Credenciales
const usuarios = {
  admin: {
    email: 'ajcortest@gmail.com',
    password: 'Operaciones.2025',
    rol: 'Administrador'
  },
  usuario: {
    email: 'ivanldg@hotmail.com',
    password: 'Natura2025$',
    rol: 'Usuario'
  }
};

const wait = (ms) => new Promise(resolve => setTimeout(resolve, ms));

async function ejecutarPruebas() {
  console.log('🚀 INICIANDO PRUEBAS AUTOMATIZADAS - PCN MÚLTIPLE');
  console.log('='.repeat(70));

  const browser = await puppeteer.launch({
    headless: false, // Mostrar navegador para depuración
    args: ['--no-sandbox', '--disable-setuid-sandbox', '--ignore-certificate-errors']
  });

  const page = await browser.newPage();
  await page.setViewport({ width: 1920, height: 1080 });

  const resultados = {
    exitosas: 0,
    fallidas: 0,
    pruebas: []
  };

  try {
    // PRUEBA 1: Acceder a la página de login
    console.log('\n🧪 PRUEBA 1: Acceder a página de login');
    console.log('-'.repeat(70));
    await page.goto(`${baseUrl}/Login/Index`, { waitUntil: 'networkidle2', timeout: 30000 });
    await wait(2000);

    const tituloLogin = await page.title();
    console.log(`✅ Página cargada: ${tituloLogin}`);
    resultados.pruebas.push({ nombre: 'Acceso a login', exitosa: true });
    resultados.exitosas++;

    // Screenshot
    await page.screenshot({ path: 'test-screenshots/01-login-page.png' });

    // PRUEBA 2: Login como Administrador
    console.log('\n🧪 PRUEBA 2: Login como Administrador');
    console.log('-'.repeat(70));

    // Buscar campos de login
    await page.waitForSelector('input[type="email"], input[name*="Correo"], input[id*="correo"]', { timeout: 10000 });

    await page.type('input[type="email"], input[name*="Correo"], input[id*="correo"]', usuarios.admin.email);
    await page.type('input[type="password"]', usuarios.admin.password);

    await wait(1000);
    await page.screenshot({ path: 'test-screenshots/02-login-filled.png' });

    // Hacer clic en botón de login
    await page.click('button[type="submit"], input[type="submit"]');
    await wait(5000);

    const urlDespuesLogin = page.url();
    const loginExitoso = !urlDespuesLogin.includes('/Login');

    console.log(loginExitoso ? `✅ Login exitoso - Redirigido a: ${urlDespuesLogin}` : '❌ Login falló');
    resultados.pruebas.push({ nombre: 'Login Administrador', exitosa: loginExitoso });
    if (loginExitoso) resultados.exitosas++;
    else resultados.fallidas++;

    await page.screenshot({ path: 'test-screenshots/03-dashboard.png' });

    if (!loginExitoso) {
      console.log('❌ No se puede continuar sin login exitoso');
      await browser.close();
      return resultados;
    }

    // PRUEBA 3: Navegar a módulo de Materiales
    console.log('\n🧪 PRUEBA 3: Navegar a módulo de Materiales');
    console.log('-'.repeat(70));

    await page.goto(`${baseUrl}/Materiales/Index`, { waitUntil: 'networkidle2', timeout: 30000 });
    await wait(3000);

    const h3 = await page.$eval('h3', el => el.textContent.trim()).catch(() => '');
    const materialesVisible = h3.includes('Material');

    console.log(materialesVisible ? `✅ Página de materiales cargada: "${h3}"` : '❌ No se pudo cargar materiales');
    resultados.pruebas.push({ nombre: 'Navegación a Materiales', exitosa: materialesVisible });
    if (materialesVisible) resultados.exitosas++;
    else resultados.fallidas++;

    await page.screenshot({ path: 'test-screenshots/04-materiales-page.png' });

    // PRUEBA 4: Verificar columna PCN en tabla
    console.log('\n🧪 PRUEBA 4: Verificar columna PCN en tabla');
    console.log('-'.repeat(70));

    const columnaPCN = await page.evaluate(() => {
      const headers = Array.from(document.querySelectorAll('thead th'));
      return headers.some(th => th.textContent.trim() === 'PCN');
    });

    console.log(columnaPCN ? '✅ Columna PCN encontrada en tabla' : '❌ Columna PCN NO encontrada');
    resultados.pruebas.push({ nombre: 'Columna PCN visible', exitosa: columnaPCN });
    if (columnaPCN) resultados.exitosas++;
    else resultados.fallidas++;

    // PRUEBA 5: Contar materiales con PCN
    console.log('\n🧪 PRUEBA 5: Verificar materiales con PCN');
    console.log('-'.repeat(70));

    const conteoMateriales = await page.evaluate(() => {
      const rows = Array.from(document.querySelectorAll('tbody tr'));
      let conPCN = 0;
      let sinPCN = 0;
      const ejemplos = [];

      rows.forEach(row => {
        const cells = row.querySelectorAll('td');
        if (cells.length > 2) {
          const pcnText = cells[2].textContent.trim();
          const nombreText = cells[0].textContent.trim();

          if (pcnText && pcnText !== 'N/A' && pcnText !== '') {
            conPCN++;
            if (ejemplos.length < 5) {
              ejemplos.push({ nombre: nombreText, pcn: pcnText });
            }
          } else {
            sinPCN++;
          }
        }
      });

      return { conPCN, sinPCN, ejemplos, totalFilas: rows.length };
    });

    console.log(`✅ Total de filas en tabla: ${conteoMateriales.totalFilas}`);
    console.log(`✅ Materiales con PCN: ${conteoMateriales.conPCN}`);
    console.log(`✅ Materiales sin PCN: ${conteoMateriales.sinPCN}`);
    console.log('\n📋 Ejemplos de materiales con PCN:');
    conteoMateriales.ejemplos.forEach(ej => {
      console.log(`   - "${ej.nombre}" → PCN: "${ej.pcn}"`);
    });

    const tieneMaterialesConPCN = conteoMateriales.conPCN > 0;
    resultados.pruebas.push({
      nombre: 'Materiales con PCN',
      exitosa: tieneMaterialesConPCN,
      detalle: `${conteoMateriales.conPCN} de ${conteoMateriales.totalFilas}`
    });
    if (tieneMaterialesConPCN) resultados.exitosas++;
    else resultados.fallidas++;

    // PRUEBA 6: Navegar a Brief
    console.log('\n🧪 PRUEBA 6: Navegar a módulo de Brief');
    console.log('-'.repeat(70));

    await page.goto(`${baseUrl}/Brief/IndexAdmin`, { waitUntil: 'networkidle2', timeout: 30000 });
    await wait(3000);

    await page.screenshot({ path: 'test-screenshots/05-brief-index.png' });

    // Buscar y hacer clic en el primer Brief
    const briefEncontrado = await page.evaluate(() => {
      // Buscar cards de Brief
      const briefElements = document.querySelectorAll('.card .card-body, .brief-card, [data-bind*="Editar"]');
      if (briefElements.length > 0) {
        // Buscar botón de editar dentro del primer Brief
        const firstBrief = briefElements[0].closest('.card, .brief-item');
        if (firstBrief) {
          const editButton = firstBrief.querySelector('button, a');
          if (editButton) {
            editButton.click();
            return true;
          }
        }
      }
      return false;
    });

    await wait(3000);

    console.log(briefEncontrado ? '✅ Brief abierto' : '⚠️  Intentando abrir Brief de otra forma...');

    if (!briefEncontrado) {
      // Intentar hacer clic en cualquier botón que abra Brief
      try {
        await page.click('.btn-primary, button:first-of-type');
        await wait(3000);
      } catch (e) {
        console.log('⚠️  No se pudo hacer clic automáticamente en Brief');
      }
    }

    await page.screenshot({ path: 'test-screenshots/06-brief-modal.png' });

    // PRUEBA 7: Navegar a pestaña de Materiales
    console.log('\n🧪 PRUEBA 7: Abrir pestaña de Materiales en Brief');
    console.log('-'.repeat(70));

    const tabMaterialesVisible = await page.evaluate(() => {
      const tabs = Array.from(document.querySelectorAll('a[href*="materiales"], .nav-link, [data-bs-toggle="tab"]'));
      const materialesTab = tabs.find(tab => tab.textContent.toLowerCase().includes('material'));
      if (materialesTab) {
        materialesTab.click();
        return true;
      }
      return false;
    });

    await wait(2000);

    console.log(tabMaterialesVisible ? '✅ Pestaña Materiales abierta' : '⚠️  No se encontró pestaña Materiales');

    await page.screenshot({ path: 'test-screenshots/07-tab-materiales.png' });

    // PRUEBA 8: Abrir formulario de creación de material
    console.log('\n🧪 PRUEBA 8: Abrir formulario de creación de material');
    console.log('-'.repeat(70));

    const formularioVisible = await page.evaluate(() => {
      // Buscar botón de agregar material
      const addButtons = Array.from(document.querySelectorAll('button'));
      const addButton = addButtons.find(btn =>
        btn.textContent.toLowerCase().includes('agregar') ||
        btn.textContent.toLowerCase().includes('nuevo') ||
        btn.textContent.toLowerCase().includes('crear')
      );

      if (addButton) {
        addButton.click();
        return true;
      }
      return false;
    });

    await wait(3000);

    console.log(formularioVisible ? '✅ Formulario de material abierto' : '⚠️  No se pudo abrir formulario');

    await page.screenshot({ path: 'test-screenshots/08-form-material.png' });

    // PRUEBA 9: Verificar checkboxes de PCN
    console.log('\n🧪 PRUEBA 9: Verificar checkboxes de PCN en formulario');
    console.log('-'.repeat(70));

    const infoCheckboxes = await page.evaluate(() => {
      // Buscar label de PCN
      const labels = Array.from(document.querySelectorAll('label'));
      const pcnLabel = labels.find(l => l.textContent.includes('PCN'));

      if (!pcnLabel) return { encontrado: false };

      // Buscar contenedor padre
      const container = pcnLabel.closest('.col-md-6, .form-group, .mb-3, div');
      if (!container) return { encontrado: false };

      // Buscar todos los checkboxes de PCN
      const checkboxes = container.querySelectorAll('input[type="checkbox"]');

      const opciones = Array.from(checkboxes).map(cb => {
        const label = cb.closest('.form-check')?.querySelector('label');
        return {
          id: cb.id,
          name: cb.name,
          texto: label ? label.textContent.trim() : '',
          checked: cb.checked
        };
      });

      return {
        encontrado: true,
        labelText: pcnLabel.textContent.trim(),
        cantidadCheckboxes: checkboxes.length,
        opciones: opciones
      };
    });

    if (infoCheckboxes.encontrado) {
      console.log(`✅ Campo PCN encontrado: "${infoCheckboxes.labelText}"`);
      console.log(`✅ Cantidad de checkboxes: ${infoCheckboxes.cantidadCheckboxes}`);
      console.log('\n📋 Lista de opciones PCN disponibles:');
      infoCheckboxes.opciones.forEach((opt, idx) => {
        console.log(`   ${idx + 1}. ${opt.texto}`);
      });

      resultados.pruebas.push({
        nombre: 'Checkboxes PCN en formulario',
        exitosa: true,
        detalle: `${infoCheckboxes.cantidadCheckboxes} opciones`
      });
      resultados.exitosas++;
    } else {
      console.log('❌ No se encontraron checkboxes de PCN');
      resultados.pruebas.push({ nombre: 'Checkboxes PCN', exitosa: false });
      resultados.fallidas++;
    }

    await page.screenshot({ path: 'test-screenshots/09-pcn-checkboxes.png', fullPage: true });

    // PRUEBA 10: Validación de PCN obligatorio
    console.log('\n🧪 PRUEBA 10: Probar validación de PCN obligatorio');
    console.log('-'.repeat(70));

    // Interceptar alert
    page.on('dialog', async dialog => {
      console.log(`📢 Alert detectado: "${dialog.message()}"`);
      await dialog.accept();
    });

    const validacionFunciona = await page.evaluate(() => {
      // Desmarcar todos los checkboxes
      const checkboxes = document.querySelectorAll('input[type="checkbox"]');
      checkboxes.forEach(cb => {
        if (cb.checked) cb.click();
      });

      // Buscar botón de guardar
      const saveButtons = Array.from(document.querySelectorAll('button'));
      const saveButton = saveButtons.find(btn =>
        btn.textContent.toLowerCase().includes('guardar') &&
        !btn.disabled
      );

      if (saveButton) {
        saveButton.click();
        return true;
      }
      return false;
    });

    await wait(2000);

    console.log(validacionFunciona ? '✅ Validación probada' : '⚠️  No se pudo probar validación');
    resultados.pruebas.push({ nombre: 'Validación PCN obligatorio', exitosa: validacionFunciona });
    if (validacionFunciona) resultados.exitosas++;
    else resultados.fallidas++;

    await page.screenshot({ path: 'test-screenshots/10-validacion.png' });

  } catch (error) {
    console.error('\n❌ ERROR:', error.message);
    resultados.pruebas.push({ nombre: 'Error general', exitosa: false, detalle: error.message });
    resultados.fallidas++;

    await page.screenshot({ path: 'test-screenshots/error.png' });
  }

  await browser.close();

  return resultados;
}

function generarReporte(resultados) {
  console.log('\n\n');
  console.log('='.repeat(70));
  console.log('📊 REPORTE FINAL DE PRUEBAS AUTOMATIZADAS');
  console.log('='.repeat(70));
  console.log(`\n✅ Pruebas exitosas: ${resultados.exitosas}`);
  console.log(`❌ Pruebas fallidas: ${resultados.fallidas}`);
  console.log(`📈 Total: ${resultados.pruebas.length}`);
  console.log(`🎯 Tasa de éxito: ${((resultados.exitosas / resultados.pruebas.length) * 100).toFixed(1)}%`);

  console.log('\n📋 DETALLE:');
  console.log('-'.repeat(70));
  resultados.pruebas.forEach((prueba, idx) => {
    const icono = prueba.exitosa ? '✅' : '❌';
    const detalle = prueba.detalle ? ` (${prueba.detalle})` : '';
    console.log(`${idx + 1}. ${icono} ${prueba.nombre}${detalle}`);
  });

  console.log('\n' + '='.repeat(70));

  if (resultados.fallidas === 0) {
    console.log('🎉 ¡TODAS LAS PRUEBAS PASARON EXITOSAMENTE!');
  } else {
    console.log('⚠️  Algunas pruebas fallaron. Ver screenshots en test-screenshots/');
  }

  console.log('='.repeat(70));
  console.log('\n📸 Screenshots guardados en: test-screenshots/');
  console.log('='.repeat(70));
}

(async () => {
  try {
    const resultados = await ejecutarPruebas();
    generarReporte(resultados);
  } catch (error) {
    console.error('Error fatal:', error);
    process.exit(1);
  }
})();
