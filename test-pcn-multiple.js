/**
 * Script de Pruebas Automatizadas - PCN Múltiple
 * Utiliza Chrome DevTools para probar todos los flujos
 */

const baseUrl = 'https://adminproyectos.entersys.mx';

// Credenciales de prueba
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

// Función de espera
const wait = (ms) => new Promise(resolve => setTimeout(resolve, ms));

// Función para hacer login
async function login(email, password) {
  console.log(`\n🔐 Iniciando sesión con: ${email}`);

  await page.goto(`${baseUrl}/Login/Index`);
  await wait(2000);

  // Llenar formulario de login
  await page.evaluate((email, password) => {
    const emailInput = document.querySelector('input[name="Correo"], input[type="email"], #Correo');
    const passwordInput = document.querySelector('input[name="Contrasena"], input[type="password"], #Contrasena');

    if (emailInput) emailInput.value = email;
    if (passwordInput) passwordInput.value = password;
  }, email, password);

  await wait(1000);

  // Hacer clic en el botón de login
  await page.evaluate(() => {
    const loginButton = document.querySelector('button[type="submit"], input[type="submit"], button:contains("Iniciar")');
    if (loginButton) loginButton.click();
  });

  await wait(3000);

  // Verificar que el login fue exitoso
  const currentUrl = await page.evaluate(() => window.location.href);
  const isLoggedIn = !currentUrl.includes('/Login');

  console.log(isLoggedIn ? '✅ Login exitoso' : '❌ Login falló');
  return isLoggedIn;
}

// Función para navegar al módulo de Materiales
async function navegarAMateriales() {
  console.log('\n📄 Navegando al módulo de Materiales...');

  await page.goto(`${baseUrl}/Materiales/Index`);
  await wait(3000);

  // Verificar que la página cargó correctamente
  const titulo = await page.evaluate(() => {
    const h3 = document.querySelector('h3');
    return h3 ? h3.textContent.trim() : '';
  });

  console.log(`✅ Página cargada: ${titulo}`);
  return titulo.includes('Gestión de Material');
}

// Función para verificar que la columna PCN existe
async function verificarColumnaPCN() {
  console.log('\n🔍 Verificando columna PCN en tabla...');

  const tienePCN = await page.evaluate(() => {
    const headers = Array.from(document.querySelectorAll('th'));
    return headers.some(th => th.textContent.trim() === 'PCN');
  });

  console.log(tienePCN ? '✅ Columna PCN encontrada' : '❌ Columna PCN no encontrada');
  return tienePCN;
}

// Función para contar materiales con PCN
async function contarMaterialesConPCN() {
  console.log('\n📊 Contando materiales con PCN...');

  const resultado = await page.evaluate(() => {
    const rows = Array.from(document.querySelectorAll('tbody tr'));
    let conPCN = 0;
    let sinPCN = 0;
    const ejemplos = [];

    rows.forEach((row, index) => {
      const cells = row.querySelectorAll('td');
      if (cells.length > 2) {
        const pcnCell = cells[2]; // Columna PCN es la 3ra (índice 2)
        const pcnText = pcnCell.textContent.trim();
        const nombreCell = cells[0];
        const nombreText = nombreCell.textContent.trim();

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

    return { conPCN, sinPCN, ejemplos };
  });

  console.log(`✅ Materiales con PCN: ${resultado.conPCN}`);
  console.log(`✅ Materiales sin PCN: ${resultado.sinPCN}`);
  console.log('\n📋 Ejemplos de materiales con PCN:');
  resultado.ejemplos.forEach(ej => {
    console.log(`   - "${ej.nombre}" → PCN: "${ej.pcn}"`);
  });

  return resultado;
}

// Función para navegar a un Brief
async function navegarABrief() {
  console.log('\n📁 Navegando al módulo de Brief...');

  await page.goto(`${baseUrl}/Brief/IndexAdmin`);
  await wait(3000);

  // Hacer clic en el primer Brief disponible
  const briefClicked = await page.evaluate(() => {
    const briefCards = document.querySelectorAll('.card, .brief-item, [data-bind*="click"][data-bind*="Editar"]');
    if (briefCards.length > 0) {
      briefCards[0].click();
      return true;
    }

    // Intentar con botones de edición
    const editButtons = document.querySelectorAll('button[data-bind*="Editar"], .btn-editar');
    if (editButtons.length > 0) {
      editButtons[0].click();
      return true;
    }

    return false;
  });

  await wait(2000);

  console.log(briefClicked ? '✅ Brief abierto' : '❌ No se pudo abrir Brief');
  return briefClicked;
}

// Función para abrir el modal de creación de material
async function abrirModalMaterial() {
  console.log('\n➕ Abriendo modal de creación de material...');

  // Hacer clic en la pestaña de Materiales
  await page.evaluate(() => {
    const tabs = Array.from(document.querySelectorAll('a[href*="materiales"], .nav-link, [data-bs-toggle="tab"]'));
    const materialesTab = tabs.find(tab => tab.textContent.toLowerCase().includes('material'));
    if (materialesTab) materialesTab.click();
  });

  await wait(1000);

  // Hacer clic en el botón de agregar material
  const modalAbierto = await page.evaluate(() => {
    const addButtons = document.querySelectorAll('button[data-bind*="AgregarMaterial"], button:contains("Agregar Material"), .btn-agregar-material');
    if (addButtons.length > 0) {
      addButtons[0].click();
      return true;
    }
    return false;
  });

  await wait(2000);

  console.log(modalAbierto ? '✅ Modal abierto' : '❌ No se pudo abrir modal');
  return modalAbierto;
}

// Función para verificar checkboxes de PCN
async function verificarCheckboxesPCN() {
  console.log('\n☑️  Verificando checkboxes de PCN...');

  const resultado = await page.evaluate(() => {
    // Buscar el contenedor de PCN
    const labels = Array.from(document.querySelectorAll('label'));
    const pcnLabel = labels.find(l => l.textContent.includes('PCN'));

    if (!pcnLabel) return { encontrado: false };

    // Buscar checkboxes de PCN
    const parent = pcnLabel.closest('.col-md-6, .form-group, div');
    if (!parent) return { encontrado: false };

    const checkboxes = parent.querySelectorAll('input[type="checkbox"]');
    const pcnOptions = Array.from(checkboxes).map(cb => {
      const label = cb.closest('.form-check')?.querySelector('label');
      return {
        id: cb.id,
        texto: label ? label.textContent.trim() : '',
        checked: cb.checked
      };
    });

    return {
      encontrado: true,
      cantidadOpciones: checkboxes.length,
      opciones: pcnOptions,
      labelText: pcnLabel.textContent.trim()
    };
  });

  if (resultado.encontrado) {
    console.log(`✅ Campo PCN encontrado: "${resultado.labelText}"`);
    console.log(`✅ Cantidad de opciones (checkboxes): ${resultado.cantidadOpciones}`);
    console.log('\n📋 Primeras 5 opciones de PCN:');
    resultado.opciones.slice(0, 5).forEach(opt => {
      console.log(`   - ${opt.texto}`);
    });
  } else {
    console.log('❌ No se encontraron checkboxes de PCN');
  }

  return resultado;
}

// Función para intentar crear material sin seleccionar PCN
async function probarValidacionPCN() {
  console.log('\n⚠️  Probando validación de PCN obligatorio...');

  await wait(1000);

  // Intentar guardar sin seleccionar PCN
  const alertMostrado = await page.evaluate(() => {
    // Primero desmarcar todos los checkboxes de PCN si hay alguno marcado
    const checkboxes = document.querySelectorAll('input[type="checkbox"][data-bind*="pcn"]');
    checkboxes.forEach(cb => cb.checked = false);

    // Interceptar alert
    let alertMessage = '';
    const originalAlert = window.alert;
    window.alert = function(msg) {
      alertMessage = msg;
      return true;
    };

    // Hacer clic en guardar
    const saveButtons = document.querySelectorAll('button[data-bind*="GuardarMaterial"], button:contains("Guardar")');
    if (saveButtons.length > 0) {
      saveButtons[0].click();
    }

    // Restaurar alert original
    setTimeout(() => {
      window.alert = originalAlert;
    }, 100);

    return alertMessage;
  });

  await wait(1000);

  if (alertMostrado.toLowerCase().includes('pcn')) {
    console.log(`✅ Validación funcionando: "${alertMostrado}"`);
    return true;
  } else {
    console.log('❌ Validación de PCN no detectada');
    return false;
  }
}

// Función principal de pruebas
async function ejecutarPruebas() {
  console.log('🚀 INICIANDO PRUEBAS AUTOMATIZADAS - PCN MÚLTIPLE');
  console.log('=' .repeat(60));

  const resultados = {
    exitosas: 0,
    fallidas: 0,
    pruebas: []
  };

  try {
    // PRUEBA 1: Login con Administrador
    console.log('\n\n🧪 PRUEBA 1: Login como Administrador');
    console.log('-'.repeat(60));
    const loginAdmin = await login(usuarios.admin.email, usuarios.admin.password);
    resultados.pruebas.push({ nombre: 'Login Administrador', exitosa: loginAdmin });
    if (loginAdmin) resultados.exitosas++;
    else resultados.fallidas++;

    if (!loginAdmin) {
      console.log('❌ No se pudo continuar sin login exitoso');
      return resultados;
    }

    // PRUEBA 2: Navegar a Materiales
    console.log('\n\n🧪 PRUEBA 2: Navegación al módulo de Materiales');
    console.log('-'.repeat(60));
    const navegacionMateriales = await navegarAMateriales();
    resultados.pruebas.push({ nombre: 'Navegación a Materiales', exitosa: navegacionMateriales });
    if (navegacionMateriales) resultados.exitosas++;
    else resultados.fallidas++;

    // PRUEBA 3: Verificar columna PCN
    console.log('\n\n🧪 PRUEBA 3: Verificación de columna PCN en tabla');
    console.log('-'.repeat(60));
    const columnaPCN = await verificarColumnaPCN();
    resultados.pruebas.push({ nombre: 'Columna PCN en tabla', exitosa: columnaPCN });
    if (columnaPCN) resultados.exitosas++;
    else resultados.fallidas++;

    // PRUEBA 4: Contar materiales con PCN
    console.log('\n\n🧪 PRUEBA 4: Verificación de materiales con PCN');
    console.log('-'.repeat(60));
    const conteo = await contarMaterialesConPCN();
    const tieneMaterialesConPCN = conteo.conPCN > 0;
    resultados.pruebas.push({
      nombre: 'Materiales con PCN visible',
      exitosa: tieneMaterialesConPCN,
      detalle: `${conteo.conPCN} materiales con PCN encontrados`
    });
    if (tieneMaterialesConPCN) resultados.exitosas++;
    else resultados.fallidas++;

    // PRUEBA 5: Navegar a Brief
    console.log('\n\n🧪 PRUEBA 5: Abrir Brief para crear material');
    console.log('-'.repeat(60));
    const briefAbierto = await navegarABrief();
    resultados.pruebas.push({ nombre: 'Abrir Brief', exitosa: briefAbierto });
    if (briefAbierto) resultados.exitosas++;
    else resultados.fallidas++;

    if (briefAbierto) {
      // PRUEBA 6: Abrir modal de material
      console.log('\n\n🧪 PRUEBA 6: Abrir modal de creación de material');
      console.log('-'.repeat(60));
      const modalAbierto = await abrirModalMaterial();
      resultados.pruebas.push({ nombre: 'Abrir modal de material', exitosa: modalAbierto });
      if (modalAbierto) resultados.exitosas++;
      else resultados.fallidas++;

      if (modalAbierto) {
        // PRUEBA 7: Verificar checkboxes de PCN
        console.log('\n\n🧪 PRUEBA 7: Verificar checkboxes de PCN en formulario');
        console.log('-'.repeat(60));
        const checkboxes = await verificarCheckboxesPCN();
        resultados.pruebas.push({
          nombre: 'Checkboxes de PCN',
          exitosa: checkboxes.encontrado,
          detalle: checkboxes.encontrado ? `${checkboxes.cantidadOpciones} opciones disponibles` : ''
        });
        if (checkboxes.encontrado) resultados.exitosas++;
        else resultados.fallidas++;

        // PRUEBA 8: Validación de PCN obligatorio
        console.log('\n\n🧪 PRUEBA 8: Validación de PCN obligatorio');
        console.log('-'.repeat(60));
        const validacion = await probarValidacionPCN();
        resultados.pruebas.push({ nombre: 'Validación PCN obligatorio', exitosa: validacion });
        if (validacion) resultados.exitosas++;
        else resultados.fallidas++;
      }
    }

    // PRUEBA 9: Logout y login con Usuario
    console.log('\n\n🧪 PRUEBA 9: Logout y login como Usuario');
    console.log('-'.repeat(60));
    await page.goto(`${baseUrl}/Login/Logout`);
    await wait(2000);
    const loginUsuario = await login(usuarios.usuario.email, usuarios.usuario.password);
    resultados.pruebas.push({ nombre: 'Login Usuario', exitosa: loginUsuario });
    if (loginUsuario) resultados.exitosas++;
    else resultados.fallidas++;

    if (loginUsuario) {
      // PRUEBA 10: Verificar acceso a materiales como usuario
      console.log('\n\n🧪 PRUEBA 10: Acceso a materiales como Usuario');
      console.log('-'.repeat(60));
      const accesoUsuario = await navegarAMateriales();
      resultados.pruebas.push({ nombre: 'Acceso a Materiales (Usuario)', exitosa: accesoUsuario });
      if (accesoUsuario) resultados.exitosas++;
      else resultados.fallidas++;

      if (accesoUsuario) {
        // Verificar columna PCN también para usuario
        const columnaPCNUsuario = await verificarColumnaPCN();
        resultados.pruebas.push({ nombre: 'Columna PCN visible (Usuario)', exitosa: columnaPCNUsuario });
        if (columnaPCNUsuario) resultados.exitosas++;
        else resultados.fallidas++;
      }
    }

  } catch (error) {
    console.error('\n❌ ERROR EN PRUEBAS:', error);
    resultados.pruebas.push({ nombre: 'Error general', exitosa: false, detalle: error.message });
    resultados.fallidas++;
  }

  return resultados;
}

// Función para generar reporte final
function generarReporte(resultados) {
  console.log('\n\n');
  console.log('='.repeat(60));
  console.log('📊 REPORTE FINAL DE PRUEBAS');
  console.log('='.repeat(60));
  console.log(`\n✅ Pruebas exitosas: ${resultados.exitosas}`);
  console.log(`❌ Pruebas fallidas: ${resultados.fallidas}`);
  console.log(`📈 Total de pruebas: ${resultados.pruebas.length}`);
  console.log(`🎯 Tasa de éxito: ${((resultados.exitosas / resultados.pruebas.length) * 100).toFixed(1)}%`);

  console.log('\n📋 DETALLE DE PRUEBAS:');
  console.log('-'.repeat(60));
  resultados.pruebas.forEach((prueba, index) => {
    const icono = prueba.exitosa ? '✅' : '❌';
    const detalle = prueba.detalle ? ` (${prueba.detalle})` : '';
    console.log(`${index + 1}. ${icono} ${prueba.nombre}${detalle}`);
  });

  console.log('\n' + '='.repeat(60));

  if (resultados.fallidas === 0) {
    console.log('🎉 ¡TODAS LAS PRUEBAS PASARON EXITOSAMENTE!');
  } else {
    console.log('⚠️  Algunas pruebas fallaron. Revisar los detalles arriba.');
  }

  console.log('='.repeat(60));
}

// Ejecutar pruebas
(async () => {
  const resultados = await ejecutarPruebas();
  generarReporte(resultados);
})();
