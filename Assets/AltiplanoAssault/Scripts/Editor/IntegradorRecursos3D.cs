// Altiplano Assault - flujo asistido para los recursos 3D de Luis Fernando.
// Los modelos externos deben conservar su licencia y sus archivos .meta.
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IntegradorRecursos3D
{
    const string Raiz = "Assets/AltiplanoAssault";
    const string Escena = Raiz + "/Scenes/Nivel1_PuebloAndino.unity";
    const string ModelosMixamo = Raiz + "/Models/AssetStore/Chullito";
    const string ModelosBlender = Raiz + "/Models/Blender";
    const string Animaciones = Raiz + "/Animations";
    const string Controlador = Animaciones + "/AC_Chullito.controller";

    [MenuItem("Altiplano Assault/Recursos 3D/1 - Validar archivos de Luis Fernando")]
    public static void ValidarArchivos()
    {
        CrearCarpetas();
        var fbxMixamo = BuscarFbx(ModelosMixamo);
        var fbxBlender = BuscarFbx(ModelosBlender);
        var modelo = BuscarModeloBase(fbxMixamo);
        var idle = BuscarClip(fbxMixamo, "idle");
        var run = BuscarClip(fbxMixamo, "run");
        var accesorio = BuscarModelo(fbxBlender, "chullo", "poncho");

        string informe =
            "Modelo de personaje: " + Estado(modelo != null) + "\n" +
            "Animacion Rifle Idle: " + Estado(idle != null) + "\n" +
            "Animacion Rifle Run: " + Estado(run != null) + "\n" +
            "Chullo o poncho de Blender: " + Estado(accesorio != null) + "\n\n" +
            "Mixamo: " + ModelosMixamo + "\n" +
            "Blender: " + ModelosBlender;

        Debug.Log("[Recursos 3D] " + informe.Replace("\n", " | "));
        EditorUtility.DisplayDialog("Validacion de recursos 3D", informe, "Aceptar");
    }

    [MenuItem("Altiplano Assault/Recursos 3D/2 - Configurar FBX como Humanoid")]
    public static void ConfigurarFbxHumanoid()
    {
        CrearCarpetas();
        int actualizados = 0;
        foreach (string ruta in BuscarFbx(ModelosMixamo))
        {
            var importador = AssetImporter.GetAtPath(ruta) as ModelImporter;
            if (importador == null || importador.animationType == ModelImporterAnimationType.Human) continue;
            importador.animationType = ModelImporterAnimationType.Human;
            importador.SaveAndReimport();
            actualizados++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[Recursos 3D] FBX Humanoid actualizados: {actualizados}.");
    }

    [MenuItem("Altiplano Assault/Recursos 3D/3 - Integrar Chullito en la escena")]
    public static void IntegrarChullito()
    {
        CrearCarpetas();
        ConfigurarFbxHumanoid();

        var fbxMixamo = BuscarFbx(ModelosMixamo);
        var modelo = BuscarModeloBase(fbxMixamo);
        if (modelo == null)
        {
            EditorUtility.DisplayDialog("Falta el personaje",
                "Copia el FBX del personaje en " + ModelosMixamo +
                " y usa un nombre que incluya Chullito o Character.", "Aceptar");
            return;
        }

        if (!AbrirEscenaObjetivo()) return;
        if (BuscarEnEscena("Jugador_Chullito_Modelo") != null)
        {
            EditorUtility.DisplayDialog("Chullito ya integrado",
                "Ya existe Jugador_Chullito_Modelo. No se creo un duplicado.", "Aceptar");
            return;
        }

        var grupo = BuscarEnEscena("=== PERSONAJES ===");
        var provisional = BuscarEnEscena("Jugador_Chullito");
        if (grupo == null || provisional == null)
        {
            EditorUtility.DisplayDialog("Escena incompatible",
                "No se encontraron === PERSONAJES === y Jugador_Chullito.", "Aceptar");
            return;
        }

        var instancia = PrefabUtility.InstantiatePrefab(modelo, SceneManager.GetActiveScene()) as GameObject;
        if (instancia == null)
        {
            EditorUtility.DisplayDialog("Error", "Unity no pudo instanciar el FBX del personaje.", "Aceptar");
            return;
        }

        Undo.RegisterCreatedObjectUndo(instancia, "Integrar Chullito");
        instancia.name = "Jugador_Chullito_Modelo";
        instancia.transform.SetParent(grupo.transform, false);
        instancia.transform.localPosition = new Vector3(-14f, 0f, 0f);
        instancia.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

        var animator = instancia.GetComponent<Animator>() ?? instancia.AddComponent<Animator>();
        animator.runtimeAnimatorController = CrearControlador(fbxMixamo);
        AdjuntarAccesorio(instancia);

        Undo.RecordObject(provisional, "Desactivar Jugador Chullito provisional");
        provisional.SetActive(false);
        EditorUtility.SetDirty(provisional);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Selection.activeGameObject = instancia;

        EditorUtility.DisplayDialog("Chullito integrado",
            "Se creo Jugador_Chullito_Modelo en (-14, 0, 0), rotacion Y 90, y se desactivo el provisional. " +
            "Revisa escala, hueso de la cabeza, materiales y transiciones antes de tomar la captura.", "Aceptar");
    }

    [MenuItem("Altiplano Assault/Recursos 3D/4 - Reemplazar provisional seleccionado por FBX...")]
    public static void ReemplazarProvisionalSeleccionado()
    {
        var provisional = Selection.activeGameObject;
        if (provisional == null || !provisional.scene.IsValid())
        {
            EditorUtility.DisplayDialog("Seleccion requerida",
                "Selecciona en la Jerarquia el prop o enemigo provisional que quieres reemplazar.", "Aceptar");
            return;
        }

        string archivo = EditorUtility.OpenFilePanel("Seleccionar FBX importado", Application.dataPath, "fbx");
        if (string.IsNullOrEmpty(archivo)) return;
        string assets = NormalizarRuta(Application.dataPath);
        string normalizada = NormalizarRuta(archivo);
        if (!normalizada.StartsWith(assets + "/", StringComparison.OrdinalIgnoreCase))
        {
            EditorUtility.DisplayDialog("FBX fuera del proyecto",
                "Primero copia el recurso dentro de Assets/AltiplanoAssault/Models/AssetStore.", "Aceptar");
            return;
        }

        string ruta = "Assets" + normalizada.Substring(assets.Length);
        var modelo = AssetDatabase.LoadAssetAtPath<GameObject>(ruta);
        if (modelo == null)
        {
            EditorUtility.DisplayDialog("Modelo no valido", "Unity no pudo cargar ese FBX.", "Aceptar");
            return;
        }

        var instancia = PrefabUtility.InstantiatePrefab(modelo, provisional.scene) as GameObject;
        if (instancia == null) return;
        Undo.RegisterCreatedObjectUndo(instancia, "Reemplazar modelo provisional");
        instancia.name = provisional.name + "_Modelo";
        instancia.transform.SetParent(provisional.transform.parent, false);
        instancia.transform.localPosition = provisional.transform.localPosition;
        instancia.transform.localRotation = provisional.transform.localRotation;
        instancia.transform.localScale = Vector3.one;

        Undo.RecordObject(provisional, "Desactivar modelo provisional");
        provisional.SetActive(false);
        EditorUtility.SetDirty(provisional);
        EditorSceneManager.MarkSceneDirty(provisional.scene);
        EditorSceneManager.SaveScene(provisional.scene);
        Selection.activeGameObject = instancia;

        EditorUtility.DisplayDialog("Recurso integrado",
            "Se desactivo " + provisional.name + " y se creo " + instancia.name +
            ". Ajusta la escala y comprueba la licencia antes del commit.", "Aceptar");
    }

    static RuntimeAnimatorController CrearControlador(string[] rutasFbx)
    {
        var controlador = AssetDatabase.LoadAssetAtPath<AnimatorController>(Controlador);
        if (controlador == null)
            controlador = AnimatorController.CreateAnimatorControllerAtPath(Controlador);

        if (!controlador.parameters.Any(p => p.name == "Velocidad"))
            controlador.AddParameter("Velocidad", AnimatorControllerParameterType.Float);

        var maquina = controlador.layers[0].stateMachine;
        var idleClip = BuscarClip(rutasFbx, "idle");
        var runClip = BuscarClip(rutasFbx, "run");
        var idle = EstadoAnimacion(maquina, "Rifle Idle", idleClip);
        var run = EstadoAnimacion(maquina, "Rifle Run", runClip);

        if (idle != null) maquina.defaultState = idle;
        if (idle != null && run != null && !idle.transitions.Any(t => t.destinationState == run))
        {
            var haciaRun = idle.AddTransition(run);
            haciaRun.hasExitTime = false;
            haciaRun.duration = 0.15f;
            haciaRun.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Velocidad");
            var haciaIdle = run.AddTransition(idle);
            haciaIdle.hasExitTime = false;
            haciaIdle.duration = 0.15f;
            haciaIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Velocidad");
        }

        EditorUtility.SetDirty(controlador);
        AssetDatabase.SaveAssets();
        return controlador;
    }

    static AnimatorState EstadoAnimacion(AnimatorStateMachine maquina, string nombre, AnimationClip clip)
    {
        var existente = maquina.states.Select(s => s.state).FirstOrDefault(s => s.name == nombre);
        if (clip == null) return existente;
        var estado = existente ?? maquina.AddState(nombre);
        estado.motion = clip;
        return estado;
    }

    static void AdjuntarAccesorio(GameObject personaje)
    {
        var rutas = BuscarFbx(ModelosBlender);
        var accesorio = BuscarModelo(rutas, "chullo", "poncho");
        if (accesorio == null) return;

        var animator = personaje.GetComponent<Animator>();
        Transform padre = animator != null && animator.isHuman
            ? animator.GetBoneTransform(HumanBodyBones.Head)
            : personaje.transform;
        if (padre == null) padre = personaje.transform;

        var instancia = PrefabUtility.InstantiatePrefab(accesorio, personaje.scene) as GameObject;
        if (instancia == null) return;
        Undo.RegisterCreatedObjectUndo(instancia, "Adjuntar chullo o poncho");
        instancia.name = accesorio.name + "_Blender";
        instancia.transform.SetParent(padre, false);
        instancia.transform.localPosition = Vector3.zero;
        instancia.transform.localRotation = Quaternion.identity;
        instancia.transform.localScale = Vector3.one;
    }

    static GameObject BuscarModeloBase(string[] rutas)
    {
        string[] preferidos = { "chullito", "character", "personaje" };
        foreach (string palabra in preferidos)
        {
            var modelo = BuscarModelo(rutas.Where(r => !EsAnimacion(r)).ToArray(), palabra);
            if (modelo != null) return modelo;
        }
        string primera = rutas.FirstOrDefault(r => !EsAnimacion(r));
        return primera == null ? null : AssetDatabase.LoadAssetAtPath<GameObject>(primera);
    }

    static GameObject BuscarModelo(string[] rutas, params string[] palabras)
    {
        string ruta = rutas.FirstOrDefault(r => palabras.Any(p =>
            Path.GetFileNameWithoutExtension(r).IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0));
        return ruta == null ? null : AssetDatabase.LoadAssetAtPath<GameObject>(ruta);
    }

    static AnimationClip BuscarClip(string[] rutas, string palabra)
    {
        foreach (string ruta in rutas.Where(r => Path.GetFileNameWithoutExtension(r)
                     .IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0))
        {
            var clip = AssetDatabase.LoadAllAssetsAtPath(ruta).OfType<AnimationClip>()
                .FirstOrDefault(c => !c.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase));
            if (clip != null) return clip;
        }
        return null;
    }

    static bool EsAnimacion(string ruta)
    {
        string n = Path.GetFileNameWithoutExtension(ruta);
        return n.IndexOf("idle", StringComparison.OrdinalIgnoreCase) >= 0 ||
               n.IndexOf("run", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    static string[] BuscarFbx(string carpeta)
    {
        if (!AssetDatabase.IsValidFolder(carpeta)) return Array.Empty<string>();
        return AssetDatabase.FindAssets("t:Model", new[] { carpeta })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(r => string.Equals(Path.GetExtension(r), ".fbx", StringComparison.OrdinalIgnoreCase))
            .OrderBy(r => r, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    static bool AbrirEscenaObjetivo()
    {
        if (SceneManager.GetActiveScene().path == Escena) return true;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return false;
        EditorSceneManager.OpenScene(Escena, OpenSceneMode.Single);
        return true;
    }

    static GameObject BuscarEnEscena(string nombre)
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go => go.scene.IsValid() && go.name == nombre);
    }

    static void CrearCarpetas()
    {
        Directory.CreateDirectory(ModelosMixamo);
        Directory.CreateDirectory(ModelosBlender);
        Directory.CreateDirectory(Animaciones);
        AssetDatabase.Refresh();
    }

    static string Estado(bool encontrado) => encontrado ? "OK" : "FALTA";
    static string NormalizarRuta(string ruta) => ruta.Replace('\\', '/').TrimEnd('/');
}
