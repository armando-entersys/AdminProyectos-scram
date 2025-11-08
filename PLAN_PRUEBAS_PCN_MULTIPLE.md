# Plan de Pruebas - PCN Múltiple
## Implementación Completada en Servidor

**URL de la aplicación:** https://adminproyectos.entersys.mx
**Fecha de implementación:** 2025-11-08
**Commits desplegados:** 228a721, 37970bc, a05f258

---

## ✅ Estado de la Implementación

### Backend
- ✅ Entidad MaterialPCN creada
- ✅ Relación muchos-a-muchos configurada
- ✅ Método InsertMaterialConPCNs implementado
- ✅ Todas las consultas actualizadas

### Frontend
- ✅ Checkboxes para selección múltiple
- ✅ Validación de al menos un PCN
- ✅ Vista de materiales con columna PCN
- ✅ Exportación a Excel incluye PCNs

### Base de Datos
- ✅ Tabla MaterialPCN creada
- ✅ 24 registros migrados exitosamente
- ✅ Columna PCNId eliminada de Materiales

---

## 📋 Plan de Pruebas Manuales

### 1. PRUEBA DE LOGIN Y ACCESO
**Objetivo:** Verificar que la aplicación está accesible

#### Pasos:
1. Abrir navegador e ir a: https://adminproyectos.entersys.mx
2. Verificar que la página de login carga correctamente
3. Iniciar sesión con credenciales válidas
4. Verificar acceso al dashboard

#### Resultado Esperado:
- ✅ Página de login visible
- ✅ Login exitoso
- ✅ Dashboard carga sin errores
- ✅ Menú de navegación visible

---

### 2. PRUEBA DE VISUALIZACIÓN DE MATERIALES EXISTENTES
**Objetivo:** Verificar que los materiales migrados muestran sus PCNs correctamente

#### Pasos:
1. Ir al módulo de **Materiales** (menú lateral)
2. Observar la tabla de materiales
3. Verificar que existe la columna **PCN**
4. Verificar que los 24 materiales existentes muestran sus PCNs

#### Resultado Esperado:
- ✅ Columna "PCN" visible en la tabla
- ✅ Materiales muestran el nombre del PCN (ej: "Mi Negocio", "WhatsApp Estrategia")
- ✅ No hay errores en consola del navegador (F12)
- ✅ Los PCNs se muestran como texto separado por comas si hay múltiples

#### Materiales de Referencia a Verificar:
| Material ID | Nombre | PCN Esperado |
|------------|--------|--------------|
| 4 | test material promo oct | Mi Negocio |
| 7 | t1 | WhatsApp Estrategia |
| 12 | Promo otoño | WhatsApp Estrategia |
| 13 | Reel podcast Historias que inspiran | SMS |

---

### 3. PRUEBA DE CREACIÓN DE BRIEF Y NAVEGACIÓN
**Objetivo:** Acceder al modal de creación de materiales

#### Pasos:
1. Ir al módulo de **Brief** (menú lateral)
2. Seleccionar un Brief existente de la lista
3. Hacer clic en el Brief para abrir el modal de detalles
4. Navegar a la pestaña **"Materiales"**
5. Hacer clic en el botón **"Agregar Material"** o similar

#### Resultado Esperado:
- ✅ Modal de Brief se abre correctamente
- ✅ Pestaña "Materiales" es accesible
- ✅ Formulario de creación de material se muestra

---

### 4. PRUEBA DE SELECCIÓN MÚLTIPLE DE PCN
**Objetivo:** Verificar que la UI permite seleccionar múltiples PCNs

#### Pasos:
1. En el formulario de creación de material, localizar el campo **"PCN"**
2. Verificar que muestra checkboxes en lugar de un dropdown
3. Verificar que el label dice **"PCN (Seleccione uno o más)"**
4. Intentar NO seleccionar ningún PCN y hacer clic en "Guardar"
5. Verificar mensaje de validación

#### Resultado Esperado:
- ✅ Campo PCN muestra lista de checkboxes
- ✅ Todos los PCNs disponibles están listados (20 opciones):
  - Mi Negocio
  - Facebook - Consultoría de Belleza Natura y Avon
  - Instagram - Consultoría de Belleza Natura y Avon
  - WhatsApp Estrategia
  - WhatsApp GNs
  - SMS
  - Instagram - Natura México
  - Facebook - Natura México
  - WhatsApp Consultor
  - Mailing
  - Instagram - Avon México
  - Facebook - Avon México
  - Espacios tiendas Natura
  - Revista
  - Revista digital
  - Sitio web Natura CF
  - Canal YouTube Escuela Natura y Avon
  - WhatsApp Líder
  - Mensaje IVR
  - Linktree
- ✅ Los checkboxes están dentro de un contenedor con scroll (max-height: 150px)
- ✅ Alert aparece: "Debe seleccionar al menos un PCN"
- ✅ El formulario NO se envía sin PCN seleccionado

---

### 5. PRUEBA DE CREACIÓN DE MATERIAL CON 1 PCN
**Objetivo:** Crear un material seleccionando un solo PCN

#### Pasos:
1. Llenar el formulario de material:
   - **Nombre:** "Prueba PCN Único - [Fecha/Hora]"
   - **Mensaje:** "Material de prueba con un solo PCN"
   - **Prioridad:** Seleccionar cualquiera
   - **Ciclo:** "2024"
   - **PCN:** ✅ Seleccionar SOLO "Mi Negocio"
   - **Audiencia:** Seleccionar cualquiera
   - **Formato:** Seleccionar cualquiera
   - **Fecha Entrega:** Seleccionar fecha futura
   - **Responsable:** "Usuario Prueba"
   - **Área:** "QA Testing"
2. Hacer clic en **"Guardar"**
3. Esperar confirmación
4. Ir a la lista de materiales
5. Buscar el material recién creado

#### Resultado Esperado:
- ✅ Material se crea exitosamente
- ✅ Mensaje de confirmación aparece
- ✅ Material aparece en la lista
- ✅ Columna PCN muestra: "Mi Negocio"
- ✅ No hay errores en consola (F12)
- ✅ No hay errores en logs del servidor

---

### 6. PRUEBA DE CREACIÓN DE MATERIAL CON MÚLTIPLES PCN
**Objetivo:** Crear un material seleccionando varios PCNs

#### Pasos:
1. Agregar otro material:
   - **Nombre:** "Prueba PCN Múltiple - [Fecha/Hora]"
   - **Mensaje:** "Material de prueba con múltiples PCNs"
   - **Prioridad:** Seleccionar cualquiera
   - **Ciclo:** "2024"
   - **PCN:** ✅ Seleccionar TRES opciones:
     - WhatsApp Estrategia
     - Instagram - Natura México
     - Facebook - Natura México
   - **Audiencia:** Seleccionar cualquiera
   - **Formato:** Seleccionar cualquiera
   - **Fecha Entrega:** Seleccionar fecha futura
   - **Responsable:** "Usuario Prueba"
   - **Área:** "QA Testing"
2. Hacer clic en **"Guardar"**
3. Esperar confirmación
4. Ir a la lista de materiales
5. Buscar el material recién creado

#### Resultado Esperado:
- ✅ Material se crea exitosamente
- ✅ Material aparece en la lista
- ✅ Columna PCN muestra: "WhatsApp Estrategia, Instagram - Natura México, Facebook - Natura México"
- ✅ Los PCNs están separados por comas
- ✅ No hay errores en consola
- ✅ No hay errores en servidor

---

### 7. PRUEBA DE VERIFICACIÓN EN BASE DE DATOS
**Objetivo:** Verificar que los datos se guardaron correctamente en BD

#### Pasos (requiere acceso SSH al servidor):
```bash
# Conectar al servidor
gcloud compute ssh dev-server --zone=us-central1-c

# Ejecutar consulta SQL
docker exec -i natura-adminproyectos-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Operaciones.2025' -C -Q "
USE AdminProyectosNaturaDB;
SELECT TOP 5
    m.Id,
    m.Nombre,
    STRING_AGG(p.Descripcion, ', ') AS PCNs
FROM Materiales m
LEFT JOIN MaterialPCN mp ON m.Id = mp.MaterialId
LEFT JOIN PCN p ON mp.PCNId = p.Id
WHERE m.Nombre LIKE '%Prueba PCN%'
GROUP BY m.Id, m.Nombre
ORDER BY m.Id DESC;
"
```

#### Resultado Esperado:
- ✅ Los dos materiales de prueba aparecen en la consulta
- ✅ El primer material muestra 1 PCN
- ✅ El segundo material muestra 3 PCNs
- ✅ Los nombres de PCN coinciden con lo seleccionado

---

### 8. PRUEBA DE FILTROS Y BÚSQUEDA
**Objetivo:** Verificar que los filtros de materiales funcionan correctamente

#### Pasos:
1. En la lista de materiales, usar el campo de búsqueda por **Nombre**
2. Buscar: "Prueba PCN"
3. Verificar resultados
4. Probar filtros por:
   - Área: "QA Testing"
   - Responsable: "Usuario Prueba"

#### Resultado Esperado:
- ✅ Filtros funcionan correctamente
- ✅ Se muestran solo los materiales que coinciden
- ✅ La columna PCN sigue mostrando los valores correctos

---

### 9. PRUEBA DE EXPORTACIÓN A EXCEL
**Objetivo:** Verificar que la exportación incluye la columna PCN

#### Pasos:
1. En la lista de materiales, aplicar un filtro para incluir los materiales de prueba
2. Hacer clic en el botón **"Exportar a Excel"**
3. Abrir el archivo descargado (.xlsx)
4. Verificar las columnas

#### Resultado Esperado:
- ✅ Archivo Excel se descarga correctamente
- ✅ Existe columna **"PCN"** en el Excel
- ✅ Los valores de PCN están correctamente poblados
- ✅ Para materiales con múltiples PCNs, se muestran separados por comas
- ✅ El orden de columnas incluye PCN después de "Mensaje"

#### Columnas esperadas en Excel:
1. Nombre de Material
2. Mensaje
3. **PCN** ← Nueva columna
4. Formato
5. Estatus
6. Nombre del Proyecto
7. Audiencia
8. Responsable
9. Área
10. Fecha de Entrega

---

### 10. PRUEBA DE EDICIÓN/VER MATERIAL
**Objetivo:** Verificar que al ver materiales se muestran correctamente

#### Pasos:
1. En la lista de materiales, hacer clic en **"Ver"** en uno de los materiales de prueba
2. Verificar que el modal se abre
3. Observar la información del material

#### Resultado Esperado:
- ✅ Modal se abre sin errores
- ✅ Información del Brief (proyecto) se muestra correctamente
- ✅ No hay errores de JavaScript en consola
- ✅ El material se puede visualizar correctamente

---

### 11. PRUEBA DE COMPATIBILIDAD CON MATERIALES ANTIGUOS
**Objetivo:** Verificar que materiales sin PCN no causan errores

#### Pasos:
1. Si existen materiales muy antiguos en la lista (anteriores a la migración)
2. Verificar que se muestran sin errores
3. Verificar que la columna PCN muestra "N/A" o está vacía

#### Resultado Esperado:
- ✅ No hay errores al mostrar materiales sin PCN
- ✅ La columna PCN maneja correctamente valores nulos
- ✅ Se muestra "N/A" cuando no hay PCNs asociados

---

### 12. PRUEBA DE RENDIMIENTO Y CARGA
**Objetivo:** Verificar que no hay degradación de rendimiento

#### Pasos:
1. Abrir consola del navegador (F12) → pestaña Network
2. Recargar la página de materiales
3. Observar tiempos de carga
4. Verificar las llamadas AJAX a la API

#### Resultado Esperado:
- ✅ Página de materiales carga en menos de 3 segundos
- ✅ Llamadas AJAX responden en menos de 2 segundos
- ✅ No hay errores 500 en las llamadas
- ✅ Los datos se cargan correctamente

---

### 13. PRUEBA DE NAVEGADORES
**Objetivo:** Verificar compatibilidad cross-browser

#### Pasos:
Repetir pruebas principales en:
1. Google Chrome (último)
2. Microsoft Edge (último)
3. Firefox (último)
4. Safari (si disponible)

#### Resultado Esperado:
- ✅ Checkboxes de PCN se renderizan correctamente en todos los navegadores
- ✅ Funcionalidad de selección múltiple funciona
- ✅ Estilos CSS se aplican correctamente
- ✅ No hay errores de JavaScript específicos del navegador

---

### 14. PRUEBA DE RESPONSIVE DESIGN
**Objetivo:** Verificar que la UI funciona en diferentes resoluciones

#### Pasos:
1. Abrir DevTools (F12) y activar modo responsive
2. Probar con resoluciones:
   - Desktop: 1920x1080
   - Laptop: 1366x768
   - Tablet: 768x1024
   - Mobile: 375x667
3. Verificar que el área de checkboxes de PCN se adapta

#### Resultado Esperado:
- ✅ Los checkboxes de PCN son accesibles en todas las resoluciones
- ✅ El scroll funciona correctamente en dispositivos pequeños
- ✅ Los labels son legibles
- ✅ La tabla de materiales es responsive

---

### 15. PRUEBA DE VALIDACIONES
**Objetivo:** Verificar todas las validaciones del formulario

#### Pasos:
1. Intentar crear material sin completar campos obligatorios
2. Verificar cada validación:
   - Sin nombre
   - Sin PCN seleccionado
   - Sin prioridad
   - Sin formato
   - Sin audiencia
   - Sin fecha de entrega

#### Resultado Esperado:
- ✅ Validación "Debe seleccionar al menos un PCN" funciona
- ✅ Otras validaciones siguen funcionando
- ✅ Mensajes de error son claros
- ✅ Formulario no se envía con datos inválidos

---

## 🐛 Registro de Errores Encontrados

Si encuentras algún error durante las pruebas, regístralo aquí:

### Error #1
- **Descripción:**
- **Pasos para reproducir:**
- **Resultado esperado:**
- **Resultado actual:**
- **Navegador/Versión:**
- **Screenshots:**
- **Logs de consola:**

### Error #2
- **Descripción:**
- **Pasos para reproducir:**
- **Resultado esperado:**
- **Resultado actual:**
- **Navegador/Versión:**
- **Screenshots:**
- **Logs de consola:**

---

## 📊 Resumen de Pruebas

| # | Prueba | Estado | Notas |
|---|--------|--------|-------|
| 1 | Login y Acceso | ⬜ | |
| 2 | Visualización Materiales | ⬜ | |
| 3 | Navegación Brief | ⬜ | |
| 4 | Selección Múltiple UI | ⬜ | |
| 5 | Crear Material 1 PCN | ⬜ | |
| 6 | Crear Material Múltiple PCN | ⬜ | |
| 7 | Verificación BD | ⬜ | |
| 8 | Filtros y Búsqueda | ⬜ | |
| 9 | Exportación Excel | ⬜ | |
| 10 | Ver Material | ⬜ | |
| 11 | Compatibilidad Antiguos | ⬜ | |
| 12 | Rendimiento | ⬜ | |
| 13 | Cross-browser | ⬜ | |
| 14 | Responsive | ⬜ | |
| 15 | Validaciones | ⬜ | |

**Leyenda:**
- ⬜ Pendiente
- ✅ Pasó
- ❌ Falló
- ⚠️ Pasó con observaciones

---

## 📝 Información Técnica de Referencia

### Endpoints de API
- **GET** `/Materiales/ObtenerMateriales` - Obtiene lista de materiales
- **GET** `/Brief/GetAllPCN` - Obtiene catálogo de PCNs
- **POST** `/Brief/CreateMaterial` - Crea nuevo material con PCNs

### Estructura de Request para Crear Material
```json
{
  "BriefId": 1,
  "Nombre": "Nombre del material",
  "Mensaje": "Mensaje del material",
  "PrioridadId": 1,
  "Ciclo": "2024",
  "PCNIds": [1, 4, 7],  // Array de IDs de PCN
  "AudienciaId": 1,
  "FormatoId": 1,
  "FechaEntrega": "2025-12-31",
  "Responsable": "Nombre",
  "Area": "Área"
}
```

### Tablas de Base de Datos Afectadas
- `Materiales` - Ya no tiene columna PCNId
- `MaterialPCN` - Nueva tabla intermedia
- `PCN` - Catálogo de PCNs (20 registros)

### Archivos Modificados en Última Implementación
1. `EntityLayer/Concrete/MaterialPCN.cs` (nuevo)
2. `EntityLayer/Concrete/Material.cs` (modificado)
3. `EntityLayer/Concrete/PCN.cs` (modificado)
4. `DataAccessLayer/Context/DataAccesContext.cs` (modificado)
5. `DataAccessLayer/Repositories/BriefRepository.cs` (modificado)
6. `PresentationLayer/Controllers/BriefController.cs` (modificado)
7. `PresentationLayer/Models/CreateMaterialRequest.cs` (nuevo)
8. `PresentationLayer/Views/Brief/IndexAdmin.cshtml` (modificado)
9. `PresentationLayer/Views/Materiales/Index.cshtml` (modificado)
10. `PresentationLayer/wwwroot/js/Brief/BriefAdmin.js` (modificado)
11. `PresentationLayer/wwwroot/js/Material/Material.js` (modificado)
12. `MIGRACION_PCN_MULTIPLE.sql` (nuevo)

---

## ✅ Checklist Final de Verificación

Antes de dar por completada la implementación:

- [ ] Todas las pruebas principales pasaron (1-15)
- [ ] No hay errores críticos en consola del navegador
- [ ] No hay errores en logs del servidor
- [ ] Exportación a Excel funciona correctamente
- [ ] Materiales existentes muestran sus PCNs correctamente
- [ ] Se pueden crear materiales con 1 PCN
- [ ] Se pueden crear materiales con múltiples PCNs
- [ ] Validaciones funcionan correctamente
- [ ] Performance es aceptable (< 3 segundos carga inicial)
- [ ] Responsive design funciona en mobile y tablet
- [ ] Cross-browser testing completado (mínimo Chrome + Edge)
- [ ] Base de datos tiene datos consistentes
- [ ] Documentación actualizada

---

## 📞 Soporte

Si encuentras problemas durante las pruebas:
- Revisar logs del servidor: `docker logs natura-adminproyectos-web --tail 100`
- Revisar logs de SQL Server: `docker logs natura-adminproyectos-sqlserver --tail 100`
- Verificar estado de contenedores: `docker ps`

**Fin del Plan de Pruebas**
