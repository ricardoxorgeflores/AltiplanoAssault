# Guía de trabajo con Git

## Primera vez

1. Aceptar la invitación al repositorio en GitHub.
2. GitHub Desktop → **File → Clone repository** → `AltiplanoAssault`.
3. Unity Hub → **Añadir** → carpeta clonada → abrir con Unity **6000.6.3f1**.

## Flujo de cada tarea

1. `Fetch origin` y actualizar `main`.
2. Cambiar a tu rama: `escena/pueblo-andino`, `recursos/modelos-3d` o `ambientacion/materiales-luz`.
3. Trabajar en Unity. Guardar escena y proyecto.
4. Commit con mensaje claro: `F2: agrega modelo Mixamo de Chullito`.
5. `Push origin` y abrir Pull Request hacia `main`. Otro integrante lo revisa.
6. Mover la tarjeta en GitHub Projects y actualizar `docs/DISTRIBUCION_TAREAS.md`.

## Reglas para evitar conflictos

- **Solo una persona edita `Nivel1_PuebloAndino.unity` a la vez** (avisar en el grupo). Los demás trabajan en prefabs, materiales o una escena de prueba propia.
- Subir siempre los archivos `.meta` junto con su recurso.
- No subir `Library/`, `Temp/` ni `Logs/` (ya están en `.gitignore`).
- Archivos de más de 100 MB no entran en GitHub: comprimir texturas o usar Git LFS.

## Tablero (GitHub Projects)

Columnas: **Pendiente → En desarrollo → Finalizada**. Una tarjeta por tarea (R1…R8, F1…F6, A1…A6), asignada a su responsable.
