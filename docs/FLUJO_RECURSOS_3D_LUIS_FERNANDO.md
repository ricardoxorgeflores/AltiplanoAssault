# Flujo de recursos 3D - Luis Fernando

Esta guía completa la parte que sí puede automatizarse sin redistribuir modelos de terceros. No marques una tarea como finalizada ni hagas el commit definitivo hasta tener los FBX, sus licencias y las capturas.

## 1. Antes de modificar la escena

1. Avisa al equipo que editarás `Nivel1_PuebloAndino.unity`.
2. Actualiza la rama desde `main` y abre el proyecto con Unity `6000.6.3f1`.
3. Abre la escena y toma la captura **antes** en la vista Game.

## 2. Mixamo

1. Inicia sesión en Mixamo con tu cuenta de Adobe y elige un personaje. Toma `LuisFernando_01_mixamo.png` mostrando la ventana completa.
2. Descarga el personaje y las animaciones **Rifle Idle** y **Rifle Run** como `FBX for Unity`, con `Skin: With Skin` cuando corresponda.
3. Usa estos nombres para que el integrador los detecte:
   - `Chullito_Character.fbx`
   - `Chullito_Rifle_Idle.fbx`
   - `Chullito_Rifle_Run.fbx`
4. Copia los tres archivos a `Assets/AltiplanoAssault/Models/AssetStore/Chullito/`.
5. En Unity ejecuta `Altiplano Assault > Recursos 3D > 1 - Validar archivos de Luis Fernando` y toma `LuisFernando_02_importacion_unity.png`.

## 3. Chullo o poncho de Blender

1. Modela un chullo sencillo (cono, dos orejeras y pompón) o un poncho. Toma `LuisFernando_03_blender.png` con Blender completo.
2. Exporta como FBX a `Assets/AltiplanoAssault/Models/Blender/Chullo_LuisFernando.fbx` o `Poncho_LuisFernando.fbx`.
3. En Unity asigna `M_Chullo` o `M_Aguayo`. El integrador intenta adjuntarlo al hueso Head; revisa posición, rotación y escala.

## 4. Integración automática

1. Ejecuta `2 - Configurar FBX como Humanoid`.
2. Ejecuta `3 - Integrar Chullito en la escena`.
3. Revisa que `Jugador_Chullito_Modelo` esté en X `-14`, Y `0`, Z `0`, con rotación Y `90`.
4. Confirma que `Jugador_Chullito` esté desactivado, no borrado.
5. En `AC_Chullito`, revisa los estados `Rifle Idle` y `Rifle Run`. La transición usa el parámetro float `Velocidad`.
6. Ajusta materiales y escala del accesorio. Toma `LuisFernando_04_original_vs_modificado.png` y `LuisFernando_05_resultado_escena.png`.

## 5. Prop o enemigo opcional

1. Elige un modelo gratuito y guarda una captura de su página con nombre, enlace y licencia visibles.
2. Copia el FBX a `Assets/AltiplanoAssault/Models/AssetStore/`.
3. Selecciona en la Jerarquía el objeto provisional.
4. Ejecuta `4 - Reemplazar provisional seleccionado por FBX...` y elige el FBX ya importado.
5. El integrador desactiva el provisional y conserva posición y rotación. Ajusta la escala del modelo nuevo.

## 6. Evidencia y cierre

1. Guarda las capturas en `docs/evidencias/capturas/` y enlázalas desde `docs/evidencias/Luis_Fernando_Flores_Martinez.md`.
2. Completa `docs/RECURSOS.md` con nombre, fuente, enlace, licencia y modificación de cada recurso.
3. Solo después cambia F2-F6 a `Finalizada` y agrega al registro el commit definitivo.
4. Comprueba que cada FBX y captura tenga su `.meta`, que ningún archivo supere 100 MB y que Unity no muestre errores.
5. Haz el commit, sube `recursos/modelos-3d` y abre el Pull Request contra `main`.
