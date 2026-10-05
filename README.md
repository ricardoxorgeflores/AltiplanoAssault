# Altiplano Assault — Leyendas que nunca mueren

Shooter andino 2.5D hecho en Unity. Un joven **chullito** recorre pueblos, yungas y ruinas del altiplano boliviano para detener al Kari Kari, al Condenado y a la Chola Sin Cabeza.

**Práctica actual:** primera implementación visual — escena base **Nivel 1: Pueblo Andino**.

## Integrantes

| Integrante | Responsabilidad | Rama |
|---|---|---|
| Flores Vargas Ricardo Jorge | Construcción de la escena en Unity y organización del proyecto | `escena/pueblo-andino` |
| Flores Martinez Luis Fernando | Búsqueda, selección y modificación de recursos 3D | `recursos/modelos-3d` |
| Osco Nina Luis Ángel | Materiales, texturas, iluminación y ambientación | `ambientacion/materiales-luz` |

Detalle de tareas y estado: [docs/DISTRIBUCION_TAREAS.md](docs/DISTRIBUCION_TAREAS.md)

## Cómo abrir el proyecto

1. Unity Hub → **Añadir** → carpeta del repositorio. Versión del editor: **6000.6.3f1** (URP).
2. Abrir `Assets/AltiplanoAssault/Scenes/Nivel1_PuebloAndino.unity`.
3. Para regenerar la escena base: menú **Altiplano Assault → Construir escena Pueblo Andino**.

## Organización

```
Assets/AltiplanoAssault/
  Scenes/        Escenas del juego
  Scripts/       Editor (constructor de escena) y Runtime
  Materials/     Materiales M_*
  Textures/      Generadas (propias) y Descargadas
  Models/        Generados, Blender, AssetStore
  Prefabs/       Personajes, Enemigos, Escenario, Items
  Animations/ Audio/ UI/ Art/
docs/            Tareas, recursos, guía de git y evidencias
```

Convención de nombres: `M_` materiales, `T_` texturas, objetos de escena en `Nombre_Descriptivo_01`.

## Documentos

- [Distribución de tareas](docs/DISTRIBUCION_TAREAS.md)
- [Recursos utilizados y procedencia](docs/RECURSOS.md)
- [Guía de trabajo con Git](docs/GUIA_GIT.md)
- [Evidencias individuales](docs/evidencias/)
