const puppeteer = require('puppeteer');

(async () => {
  const browser = await puppeteer.launch({
    headless: false,
    args: ['--start-maximized']
  });

  const page = await browser.newPage();

  // Capturar logs de consola
  page.on('console', msg => {
    console.log('BROWSER LOG:', msg.text());
  });

  // Navegar a la página de briefs
  await page.goto('https://adminproyectos.entersys.mx/Brief/Index', {
    waitUntil: 'networkidle2'
  });

  console.log('✅ Página cargada');
  console.log('Por favor:');
  console.log('1. Inicia sesión si es necesario');
  console.log('2. Haz clic en "Agregar Brief"');
  console.log('3. Llena el formulario');
  console.log('4. Haz clic en "Guardar"');
  console.log('5. Revisa los logs en esta consola');
  console.log('\nPresiona Ctrl+C para salir cuando termines');

  // Mantener el navegador abierto
  await new Promise(() => {});
})();
