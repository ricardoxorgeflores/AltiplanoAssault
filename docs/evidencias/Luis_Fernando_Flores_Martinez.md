# Evidencia individual — Flores Martinez Luis Fernando

**Responsabilidad:** Búsqueda, selección y modificación de recursos 3D

## Descripción de lo realizado

Se analizo la escena base y se preparo un flujo de Editor para validar FBX de Mixamo y Blender, configurar el rig Humanoid, crear el controlador de Rifle Idle/Run, colocar el personaje en la posicion indicada y desactivar los modelos provisionales sin borrarlos. La integracion visual queda pendiente hasta incorporar los FBX, verificar sus licencias y tomar las capturas en las herramientas correspondientes.

## Verificacion en Unity

El 05/10/2026 se abrio e importo el proyecto con Unity `6000.6.3f1 (45d8eee7de74)` y URP `17.6.0`. La compilacion termino sin errores de C# y el validador de recursos se ejecuto correctamente en modo automatico. El resultado fue `FALTA` para el personaje, Rifle Idle, Rifle Run y el accesorio de Blender; por ese motivo las tareas F2-F6 permanecen pendientes y no se reemplazo ningun objeto provisional.

## Capturas

Guardar las imágenes en `docs/evidencias/capturas/` y enlazarlas aquí.

| Captura | Qué muestra |
|---|---|
| `capturas/LuisFernando_01_mixamo.png` | Personaje elegido en Mixamo (pendiente) |
| `capturas/LuisFernando_02_importacion_unity.png` | FBX importados y validacion en Unity (pendiente) |
| `capturas/LuisFernando_03_blender.png` | Modelado del chullo o poncho (pendiente) |
| `capturas/LuisFernando_04_original_vs_modificado.png` | Personaje original y modificado (pendiente) |
| `capturas/LuisFernando_05_resultado_escena.png` | Resultado final en Nivel 1 (pendiente) |

## Resultado incorporado al proyecto

| Archivo o carpeta | Commit |
|---|---|
| `Assets/AltiplanoAssault/Scripts/Editor/IntegradorRecursos3D.cs` | Este commit de `recursos/modelos-3d` |
| `docs/FLUJO_RECURSOS_3D_LUIS_FERNANDO.md` | Este commit de `recursos/modelos-3d` |
