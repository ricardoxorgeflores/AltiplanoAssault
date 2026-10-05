// Altiplano Assault - Constructor de la escena base "Pueblo Andino" (Nivel 1)
// Menu: Altiplano Assault > Construir escena Pueblo Andino
// Genera texturas, mallas y materiales propios del equipo y arma la escena
// con una jerarquia ordenada. Los modelos son PROVISIONALES (primitivas) y se
// reemplazan por los modelos definitivos (Asset Store / Blender / Mixamo).
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class ConstructorPuebloAndino
{
    const string Raiz = "Assets/AltiplanoAssault";
    const string RutaEscena = Raiz + "/Scenes/Nivel1_PuebloAndino.unity";
    const string DirMat = Raiz + "/Materials";
    const string DirTex = Raiz + "/Textures/Generadas";
    const string DirMallas = Raiz + "/Models/Generados";

    static readonly string[] Carpetas =
    {
        "Scenes", "Scripts/Editor", "Scripts/Runtime",
        "Materials", "Textures/Generadas", "Textures/Descargadas",
        "Models/Generados", "Models/Blender", "Models/AssetStore",
        "Prefabs/Personajes", "Prefabs/Enemigos", "Prefabs/Escenario", "Prefabs/Items",
        "Animations", "Audio/Musica", "Audio/SFX", "UI", "Art/Concepto"
    };

    static Mesh piramide, prisma;
    static System.Random azar;

    [InitializeOnLoadMethod]
    static void ConstruirAlAbrir()
    {
        if (Application.isBatchMode) return;
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (File.Exists(RutaEscena)) return;
            Construir();
        };
    }

    [MenuItem("Altiplano Assault/Crear carpetas del proyecto")]
    public static void CrearCarpetas()
    {
        foreach (var c in Carpetas) Directory.CreateDirectory(Raiz + "/" + c);
        AssetDatabase.Refresh();
    }

    [MenuItem("Altiplano Assault/Construir escena Pueblo Andino")]
    public static void Construir()
    {
        CrearCarpetas();
        azar = new System.Random(1825);
        piramide = MallaGuardada("Piramide", CrearPiramide);
        prisma = MallaGuardada("PrismaTecho", CrearPrisma);

        // ---------- Texturas generadas por el equipo ----------
        var tPiedra = Textura("T_Empedrado", 256, PixelEmpedrado, false);
        var tAdobe = Textura("T_Adobe", 256, PixelAdobe, false);
        var tTejas = Textura("T_Tejas", 256, PixelTejas, false);
        var tAguayo = Textura("T_Aguayo", 128, PixelAguayo, true);
        var tWiphala = Textura("T_Wiphala", 224, PixelWiphala, true);
        var tTierra = Textura("T_Tierra", 256, PixelTierra, false);

        // ---------- Materiales ----------
        var mEmpedrado = Mat("M_Empedrado", Color.white, 0.15f, 0, tPiedra, new Vector2(22, 2));
        var mTierra = Mat("M_TierraAltiplano", Color.white, 0.02f, 0, tTierra, new Vector2(30, 20));
        var mAdobe = Mat("M_Adobe", Color.white, 0.03f, 0, tAdobe, new Vector2(2, 1.5f));
        var mAdobeClaro = Mat("M_AdobeEncalado", new Color(0.95f, 0.88f, 0.76f), 0.03f, 0, tAdobe, new Vector2(2, 1.5f));
        var mIglesia = Mat("M_PiedraIglesia", new Color(0.86f, 0.72f, 0.55f), 0.05f, 0, tAdobe, new Vector2(3, 3));
        var mTejas = Mat("M_Tejas", Color.white, 0.1f, 0, tTejas, new Vector2(4, 2));
        var mMadera = Mat("M_MaderaOscura", new Color(0.30f, 0.18f, 0.10f), 0.15f);
        var mMaderaCaja = Mat("M_MaderaCaja", new Color(0.62f, 0.44f, 0.24f), 0.1f);
        var mPiedraMuro = Mat("M_PiedraMuro", new Color(0.50f, 0.47f, 0.44f), 0.1f, 0, tPiedra, new Vector2(1, 1));
        var mRoca = Mat("M_RocaMontana", new Color(0.33f, 0.26f, 0.33f), 0.05f);
        var mNieve = Mat("M_Nieve", new Color(0.96f, 0.97f, 1f), 0.4f);
        var mWiphala = Mat("M_Wiphala", Color.white, 0.05f, 0, tWiphala, Vector2.one);
        var mAguayo = Mat("M_Aguayo", Color.white, 0.05f, 0, tAguayo, new Vector2(1, 2));
        var mPiel = Mat("M_Piel", new Color(0.72f, 0.48f, 0.33f), 0.2f);
        var mPantalon = Mat("M_PantalonJugador", new Color(0.23f, 0.21f, 0.19f), 0.1f);
        var mChullo = Mat("M_Chullo", new Color(0.80f, 0.13f, 0.12f), 0.05f);
        var mMochila = Mat("M_Mochila", new Color(0.36f, 0.31f, 0.18f), 0.1f);
        var mMetal = Mat("M_MetalArma", new Color(0.13f, 0.13f, 0.15f), 0.6f, 0.8f);
        var mSoldado = Mat("M_UniformeEnemigo", new Color(0.20f, 0.26f, 0.17f), 0.1f);
        var mCasco = Mat("M_CascoEnemigo", new Color(0.12f, 0.15f, 0.11f), 0.35f, 0.2f);
        var mLana = Mat("M_LanaLlama", new Color(0.93f, 0.90f, 0.84f), 0.02f);
        var mOjoRojo = Mat("M_OjoRojoEmisivo", new Color(1f, 0.1f, 0.05f), 0.5f, 0, null, Vector2.one, new Color(3f, 0.2f, 0.1f));
        var mDrone = Mat("M_Drone", new Color(0.18f, 0.19f, 0.22f), 0.55f, 0.7f);
        var mBarrilAzul = Mat("M_BarrilAzul", new Color(0.12f, 0.30f, 0.55f), 0.45f, 0.5f);
        var mBarrilOxido = Mat("M_BarrilOxido", new Color(0.55f, 0.27f, 0.12f), 0.25f, 0.4f);
        var mSaco = Mat("M_SacoArena", new Color(0.66f, 0.58f, 0.40f), 0.02f);
        var mOro = Mat("M_MonedaOro", new Color(1f, 0.78f, 0.15f), 0.8f, 1f, null, Vector2.one, new Color(0.5f, 0.35f, 0.02f));
        var mBlanco = Mat("M_BotiquinBlanco", new Color(0.95f, 0.95f, 0.95f), 0.3f);
        var mRojo = Mat("M_RojoCruz", new Color(0.85f, 0.08f, 0.08f), 0.3f);
        var mMunicion = Mat("M_CajaMunicion", new Color(0.33f, 0.37f, 0.20f), 0.2f, 0.2f);
        var mHoja = Mat("M_Follaje", new Color(0.25f, 0.42f, 0.20f), 0.05f);
        var mTronco = Mat("M_Tronco", new Color(0.36f, 0.25f, 0.16f), 0.05f);
        var mFarol = Mat("M_FarolEmisivo", new Color(1f, 0.85f, 0.5f), 0.3f, 0, null, Vector2.one, new Color(2.5f, 1.7f, 0.6f));
        var mVentana = Mat("M_VentanaCalida", new Color(0.25f, 0.18f, 0.10f), 0.6f, 0, null, Vector2.one, new Color(0.9f, 0.5f, 0.12f));
        var mAgua = Mat("M_AguaFuente", new Color(0.25f, 0.55f, 0.70f), 0.9f);
        var mCielo = MatCielo();

        // ---------- Escena ----------
        var escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var gEscenario = Grupo("=== ESCENARIO ===", null);
        var gSuelo = Grupo("Suelo", gEscenario);
        var gEdificios = Grupo("Edificios", gEscenario);
        var gMontanas = Grupo("Montanas_Fondo", gEscenario);
        var gDecoracion = Grupo("Decoracion", gEscenario);
        var gPersonajes = Grupo("=== PERSONAJES ===", null);
        var gEnemigos = Grupo("Enemigos", gPersonajes);
        var gObstaculos = Grupo("=== OBSTACULOS ===", null);
        var gItems = Grupo("=== ITEMS ===", null);
        var gLuces = Grupo("=== ILUMINACION ===", null);
        var gCamaras = Grupo("=== CAMARAS ===", null);

        // Suelo: llanura, calle empedrada (carril de juego) y acera
        Prim(PrimitiveType.Cube, "Llanura_Altiplano", gSuelo, new Vector3(0, -0.6f, 30), new Vector3(260, 1, 160), mTierra);
        Prim(PrimitiveType.Cube, "Calle_Empedrada", gSuelo, new Vector3(0, -0.5f, 0), new Vector3(120, 1, 9), mEmpedrado);
        Prim(PrimitiveType.Cube, "Acera_Edificios", gSuelo, new Vector3(0, -0.4f, 6), new Vector3(120, 1, 3), mPiedraMuro);

        // Muro bajo de piedra en primer plano (como en el concepto)
        var gMuro = Grupo("Muro_PrimerPlano", gSuelo);
        for (int i = 0; i < 60; i++)
        {
            float x = -59 + i * 2f;
            float h = 0.7f + R(0.5f);
            Prim(PrimitiveType.Cube, "Piedra_" + i.ToString("00"), gMuro,
                new Vector3(x, h / 2 - 0.5f, -5.2f + R(0.25f)), new Vector3(1.95f, h, 1.1f), mPiedraMuro);
        }

        // Edificios: casas de adobe y la iglesia
        Casa("Casa_Adobe_01", gEdificios, new Vector3(-40, 0, 10), 8, 5, 6, mAdobe, mTejas, mMadera, mVentana);
        Casa("Casa_Adobe_02", gEdificios, new Vector3(-30, 0, 11), 9, 6.5f, 7, mAdobeClaro, mTejas, mMadera, mVentana);
        Casa("Casa_Adobe_03", gEdificios, new Vector3(-19, 0, 10), 7, 4.5f, 6, mAdobe, mTejas, mMadera, mVentana);
        Casa("Casa_Adobe_04", gEdificios, new Vector3(-9, 0, 11), 8, 6, 7, mAdobeClaro, mTejas, mMadera, mVentana);
        Casa("Casa_Adobe_05", gEdificios, new Vector3(34, 0, 10), 8, 5, 6, mAdobe, mTejas, mMadera, mVentana);
        Casa("Casa_Adobe_06", gEdificios, new Vector3(45, 0, 11), 9, 6.5f, 7, mAdobeClaro, mTejas, mMadera, mVentana);
        Casa("Casa_Fondo_01", gEdificios, new Vector3(-24, 0, 22), 10, 5, 6, mAdobe, mTejas, mMadera, mVentana);
        Casa("Casa_Fondo_02", gEdificios, new Vector3(2, 0, 24), 9, 5, 6, mAdobeClaro, mTejas, mMadera, mVentana);
        Casa("Casa_Fondo_03", gEdificios, new Vector3(40, 0, 23), 10, 5.5f, 6, mAdobe, mTejas, mMadera, mVentana);
        Iglesia(gEdificios, new Vector3(16, 0, 13), mIglesia, mTejas, mMadera, mVentana, mMetal, mWiphala);

        // Montanas nevadas de fondo
        Montana("Illimani", gMontanas, new Vector3(-10, -1, 95), 90, 52, mRoca, mNieve);
        Montana("Montana_Izq", gMontanas, new Vector3(-75, -1, 85), 70, 36, mRoca, mNieve);
        Montana("Montana_Der", gMontanas, new Vector3(60, -1, 90), 80, 42, mRoca, mNieve);
        Montana("Montana_Der_Lejana", gMontanas, new Vector3(110, -1, 100), 70, 30, mRoca, mNieve);
        Montana("Cerro_Cercano_Izq", gMontanas, new Vector3(-45, -1, 60), 45, 16, mRoca, null);
        Montana("Cerro_Cercano_Der", gMontanas, new Vector3(30, -1, 62), 50, 18, mRoca, null);

        // Decoracion: wiphalas, faroles, fuente, arboles
        Bandera("Wiphala_01", gDecoracion, new Vector3(-25, 0, 6), 6.5f, mMadera, mWiphala);
        Bandera("Wiphala_02", gDecoracion, new Vector3(-4, 0, 6), 7f, mMadera, mWiphala);
        Bandera("Wiphala_03", gDecoracion, new Vector3(28, 0, 6), 6.5f, mMadera, mWiphala);
        float[] xFaroles = { -35, -14, 6, 26, 40 };
        for (int i = 0; i < xFaroles.Length; i++)
            Farol("Farol_" + (i + 1).ToString("00"), gDecoracion, new Vector3(xFaroles[i], 0, 5.2f), mMetal, mFarol);
        Fuente(gDecoracion, new Vector3(3, 0, 8.5f), mPiedraMuro, mAgua);
        float[] xArboles = { -46, -14, 26, 50 };
        for (int i = 0; i < xArboles.Length; i++)
            Arbol("Arbol_" + (i + 1).ToString("00"), gDecoracion, new Vector3(xArboles[i], 0, 15 + R(3)), mTronco, mHoja);

        // Personaje principal (modelo provisional)
        var jugador = Humanoide("Jugador_Chullito", gPersonajes, new Vector3(-14, 0, 0), true, mAguayo, mPiel, mPantalon, mMetal);
        Vestuario_Chullito(jugador, mAguayo, mChullo, mMochila);

        // Enemigos (modelos provisionales)
        for (int i = 0; i < 3; i++)
        {
            var s = Humanoide("Soldado_Enemigo_" + (i + 1).ToString("00"), gEnemigos,
                new Vector3(4 + i * 7.5f, 0, i == 1 ? 1.2f : 0), false, mSoldado, mPiel, mSoldado, mMetal);
            Prim(PrimitiveType.Sphere, "Casco", s, new Vector3(0, 1.86f, 0), new Vector3(0.52f, 0.36f, 0.52f), mCasco);
            Prim(PrimitiveType.Cube, "Chaleco", s, new Vector3(0.02f, 1.2f, 0), new Vector3(0.52f, 0.5f, 0.66f), mCasco);
        }
        Llama("Llaminga_Salvaje", gEnemigos, new Vector3(-4, 0, 1.5f), mLana, mAguayo, mOjoRojo, mMadera);
        Dron("Drone_Vigilante", gEnemigos, new Vector3(12, 6.2f, 0), mDrone, mOjoRojo, mMetal);

        // Obstaculos
        Barricada(gObstaculos, new Vector3(1, 0, 0), mSaco);
        Prim(PrimitiveType.Cylinder, "Barril_Azul_01", gObstaculos, new Vector3(-8, 0.6f, -1), new Vector3(0.8f, 0.6f, 0.8f), mBarrilAzul);
        Prim(PrimitiveType.Cylinder, "Barril_Azul_02", gObstaculos, new Vector3(-7, 0.6f, -1.6f), new Vector3(0.8f, 0.6f, 0.8f), mBarrilAzul);
        Prim(PrimitiveType.Cylinder, "Barril_Oxido_01", gObstaculos, new Vector3(22, 0.6f, 1.5f), new Vector3(0.8f, 0.6f, 0.8f), mBarrilOxido);
        Prim(PrimitiveType.Cube, "Caja_Madera_01", gObstaculos, new Vector3(8, 0.5f, -1.5f), Vector3.one, mMaderaCaja);
        Prim(PrimitiveType.Cube, "Caja_Madera_02", gObstaculos, new Vector3(9.1f, 0.5f, -1.4f), Vector3.one, mMaderaCaja);
        Prim(PrimitiveType.Cube, "Caja_Madera_03", gObstaculos, new Vector3(8.5f, 1.5f, -1.45f), Vector3.one, mMaderaCaja, new Vector3(0, 18, 0));
        Prim(PrimitiveType.Cube, "Caja_Madera_04", gObstaculos, new Vector3(27, 0.5f, 0.5f), Vector3.one, mMaderaCaja, new Vector3(0, 30, 0));

        // Items
        for (int i = 0; i < 4; i++)
        {
            var m = Prim(PrimitiveType.Cylinder, "Moneda_" + (i + 1).ToString("00"), gItems,
                new Vector3(-10 + i * 1.3f, 1.5f + (i == 1 || i == 2 ? 0.5f : 0), 0), new Vector3(0.55f, 0.04f, 0.55f), mOro, new Vector3(90, 0, 0));
            m.gameObject.AddComponent<RotarConstante>().gradosPorSegundo = new Vector3(0, 0, 140);
        }
        var botiquin = Grupo("Botiquin", gItems); botiquin.position = new Vector3(18, 0.35f, -1);
        Prim(PrimitiveType.Cube, "Caja", botiquin, Vector3.zero, new Vector3(0.7f, 0.5f, 0.5f), mBlanco);
        Prim(PrimitiveType.Cube, "Cruz_H", botiquin, new Vector3(0, 0, -0.26f), new Vector3(0.36f, 0.12f, 0.02f), mRojo);
        Prim(PrimitiveType.Cube, "Cruz_V", botiquin, new Vector3(0, 0, -0.26f), new Vector3(0.12f, 0.36f, 0.02f), mRojo);
        Prim(PrimitiveType.Cube, "Caja_Municion", gItems, new Vector3(-1, 0.25f, -1.6f), new Vector3(0.8f, 0.4f, 0.45f), mMunicion);

        // Iluminacion y ambiente (atardecer andino)
        var sol = new GameObject("Sol_Atardecer").AddComponent<Light>();
        sol.transform.SetParent(gLuces, false);
        sol.type = LightType.Directional;
        sol.color = new Color(1f, 0.84f, 0.66f);
        sol.intensity = 1.5f;
        sol.shadows = LightShadows.Soft;
        sol.transform.rotation = Quaternion.Euler(32, -38, 0);
        RenderSettings.skybox = mCielo;
        RenderSettings.sun = sol;
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.62f, 0.56f, 0.70f);
        RenderSettings.ambientEquatorColor = new Color(0.78f, 0.58f, 0.46f);
        RenderSettings.ambientGroundColor = new Color(0.30f, 0.24f, 0.20f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.90f, 0.70f, 0.60f);
        RenderSettings.fogStartDistance = 70;
        RenderSettings.fogEndDistance = 380;

        // Camaras: lateral 2.5D (principal) y vista general del pueblo
        var cam = new GameObject("Camara_Principal_2.5D").AddComponent<Camera>();
        cam.transform.SetParent(gCamaras, false);
        cam.tag = "MainCamera";
        cam.transform.position = new Vector3(-3, 4.0f, -16);
        cam.transform.rotation = Quaternion.Euler(6, 0, 0);
        cam.fieldOfView = 42;
        cam.farClipPlane = 400;
        cam.gameObject.AddComponent<AudioListener>();
        var cam2 = new GameObject("Camara_VistaGeneral").AddComponent<Camera>();
        cam2.transform.SetParent(gCamaras, false);
        cam2.transform.position = new Vector3(-30, 14, -30);
        cam2.transform.rotation = Quaternion.Euler(18, 38, 0);
        cam2.fieldOfView = 50;
        cam2.farClipPlane = 400;
        cam2.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(escena, RutaEscena);
        var lista = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!lista.Exists(s => s.path == RutaEscena)) lista.Insert(0, new EditorBuildSettingsScene(RutaEscena, true));
        EditorBuildSettings.scenes = lista.ToArray();
        AssetDatabase.SaveAssets();
        Debug.Log("[Altiplano Assault] Escena Pueblo Andino construida: " + RutaEscena);
    }

    // ================= Constructores de objetos =================

    static void Casa(string nombre, Transform padre, Vector3 pos, float w, float h, float d,
        Material muro, Material tejas, Material madera, Material ventana)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Cube, "Muros", g, new Vector3(0, h / 2, 0), new Vector3(w, h, d), muro);
        Malla("Techo", prisma, g, new Vector3(0, h, 0), new Vector3(w + 0.8f, h * 0.38f, d + 0.8f), tejas);
        float f = -d / 2 - 0.03f;
        Prim(PrimitiveType.Cube, "Puerta", g, new Vector3(-w * 0.2f, 1.1f, f), new Vector3(1.2f, 2.2f, 0.12f), madera);
        Prim(PrimitiveType.Cube, "Dintel", g, new Vector3(-w * 0.2f, 2.3f, f), new Vector3(1.6f, 0.2f, 0.2f), madera);
        Prim(PrimitiveType.Cube, "Ventana_01", g, new Vector3(w * 0.22f, 1.7f, f), new Vector3(1.1f, 1.1f, 0.1f), ventana);
        Prim(PrimitiveType.Cube, "Marco_01", g, new Vector3(w * 0.22f, 1.1f, f), new Vector3(1.4f, 0.14f, 0.24f), madera);
        if (h > 5.5f)
        {
            Prim(PrimitiveType.Cube, "Ventana_02", g, new Vector3(-w * 0.2f, h - 1.6f, f), new Vector3(1f, 1.1f, 0.1f), ventana);
            Prim(PrimitiveType.Cube, "Ventana_03", g, new Vector3(w * 0.22f, h - 1.6f, f), new Vector3(1f, 1.1f, 0.1f), ventana);
            Prim(PrimitiveType.Cube, "Balcon", g, new Vector3(0, h - 2.35f, f - 0.35f), new Vector3(w * 0.8f, 0.14f, 0.8f), madera);
            Prim(PrimitiveType.Cube, "Baranda", g, new Vector3(0, h - 1.85f, f - 0.72f), new Vector3(w * 0.8f, 0.1f, 0.08f), madera);
        }
    }

    static void Iglesia(Transform padre, Vector3 pos, Material muro, Material tejas, Material madera,
        Material ventana, Material metal, Material wiphala)
    {
        var g = Grupo("Iglesia_Colonial", padre); g.position = pos;
        Prim(PrimitiveType.Cube, "Nave", g, new Vector3(2, 4.5f, 0), new Vector3(11, 9, 9), muro);
        Malla("Techo_Nave", prisma, g, new Vector3(2, 9, 0), new Vector3(11.8f, 3, 9.8f), tejas);
        Prim(PrimitiveType.Cube, "Torre", g, new Vector3(-5.5f, 7, -1), new Vector3(4.4f, 14, 4.4f), muro);
        Prim(PrimitiveType.Cube, "Cornisa", g, new Vector3(-5.5f, 14.1f, -1), new Vector3(5, 0.4f, 5), muro);
        float[] dx = { -1.6f, 1.6f, -1.6f, 1.6f }; float[] dz = { -1.6f, -1.6f, 1.6f, 1.6f };
        for (int i = 0; i < 4; i++)
            Prim(PrimitiveType.Cube, "Pilar_Campanario_" + (i + 1), g, new Vector3(-5.5f + dx[i], 15.6f, -1 + dz[i]), new Vector3(0.8f, 2.8f, 0.8f), muro);
        Prim(PrimitiveType.Sphere, "Campana", g, new Vector3(-5.5f, 15.4f, -1), new Vector3(1.3f, 1.5f, 1.3f), metal);
        Malla("Techo_Torre", piramide, g, new Vector3(-5.5f, 17, -1), new Vector3(5, 3.2f, 5), tejas);
        Prim(PrimitiveType.Cube, "Cruz_V", g, new Vector3(-5.5f, 21, -1), new Vector3(0.18f, 1.8f, 0.18f), metal);
        Prim(PrimitiveType.Cube, "Cruz_H", g, new Vector3(-5.5f, 21.3f, -1), new Vector3(0.9f, 0.18f, 0.18f), metal);
        Prim(PrimitiveType.Cube, "Porton", g, new Vector3(2, 1.9f, -4.56f), new Vector3(2.6f, 3.8f, 0.2f), madera);
        Prim(PrimitiveType.Cylinder, "Arco_Porton", g, new Vector3(2, 3.8f, -4.56f), new Vector3(2.6f, 0.1f, 2.6f), madera, new Vector3(90, 0, 0));
        Prim(PrimitiveType.Cylinder, "Roseton", g, new Vector3(2, 7, -4.52f), new Vector3(1.6f, 0.06f, 1.6f), ventana, new Vector3(90, 0, 0));
        Prim(PrimitiveType.Cube, "Ventana_Torre", g, new Vector3(-5.5f, 9, -3.24f), new Vector3(0.9f, 1.8f, 0.1f), ventana);
        Prim(PrimitiveType.Cube, "Escalinata_01", g, new Vector3(2, 0.15f, -5.4f), new Vector3(6, 0.3f, 1.6f), muro);
        Prim(PrimitiveType.Cube, "Escalinata_02", g, new Vector3(2, 0.35f, -5.0f), new Vector3(5, 0.3f, 0.9f), muro);
        Prim(PrimitiveType.Cube, "Asta_Fachada", g, new Vector3(6.6f, 6.5f, -4.8f), new Vector3(0.08f, 3, 0.08f), madera, new Vector3(0, 0, -25));
        Prim(PrimitiveType.Cube, "Wiphala_Fachada", g, new Vector3(7.9f, 6.6f, -4.85f), new Vector3(1.6f, 1.6f, 0.03f), wiphala, new Vector3(0, 0, -25));
    }

    static void Montana(string nombre, Transform padre, Vector3 pos, float ancho, float alto, Material roca, Material nieve)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        g.rotation = Quaternion.Euler(0, 20 + R(50), 0);
        Malla("Roca", piramide, g, Vector3.zero, new Vector3(ancho, alto, ancho * 0.8f), roca);
        if (nieve != null)
            Malla("Nieve", piramide, g, new Vector3(0, alto * 0.58f, 0), new Vector3(ancho * 0.44f, alto * 0.43f, ancho * 0.36f), nieve);
    }

    static void Bandera(string nombre, Transform padre, Vector3 pos, float alto, Material asta, Material tela)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Cylinder, "Asta", g, new Vector3(0, alto / 2, 0), new Vector3(0.1f, alto / 2, 0.1f), asta);
        Prim(PrimitiveType.Cube, "Tela", g, new Vector3(1.05f, alto - 1f, 0), new Vector3(2, 2, 0.03f), tela, new Vector3(0, 0, -4));
    }

    static void Farol(string nombre, Transform padre, Vector3 pos, Material metal, Material foco)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Cylinder, "Poste", g, new Vector3(0, 1.7f, 0), new Vector3(0.1f, 1.7f, 0.1f), metal);
        Prim(PrimitiveType.Cube, "Brazo", g, new Vector3(0, 3.4f, -0.3f), new Vector3(0.08f, 0.08f, 0.7f), metal);
        Prim(PrimitiveType.Sphere, "Foco", g, new Vector3(0, 3.2f, -0.6f), Vector3.one * 0.32f, foco);
        var l = new GameObject("Luz").AddComponent<Light>();
        l.transform.SetParent(g, false);
        l.transform.localPosition = new Vector3(0, 3.1f, -0.7f);
        l.type = LightType.Point; l.color = new Color(1f, 0.75f, 0.42f); l.range = 9; l.intensity = 6;
    }

    static void Fuente(Transform padre, Vector3 pos, Material piedra, Material agua)
    {
        var g = Grupo("Fuente_Plaza", padre); g.position = pos;
        Prim(PrimitiveType.Cylinder, "Base", g, new Vector3(0, 0.3f, 0), new Vector3(3.2f, 0.3f, 3.2f), piedra);
        Prim(PrimitiveType.Cylinder, "Agua", g, new Vector3(0, 0.56f, 0), new Vector3(2.8f, 0.05f, 2.8f), agua);
        Prim(PrimitiveType.Cylinder, "Columna", g, new Vector3(0, 1.2f, 0), new Vector3(0.4f, 0.9f, 0.4f), piedra);
        Prim(PrimitiveType.Cylinder, "Plato", g, new Vector3(0, 2.1f, 0), new Vector3(1.5f, 0.08f, 1.5f), piedra);
    }

    static void Arbol(string nombre, Transform padre, Vector3 pos, Material tronco, Material hoja)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Cylinder, "Tronco", g, new Vector3(0, 2, 0), new Vector3(0.45f, 2, 0.45f), tronco);
        Prim(PrimitiveType.Sphere, "Copa_01", g, new Vector3(0, 5, 0), new Vector3(3.6f, 3.2f, 3.6f), hoja);
        Prim(PrimitiveType.Sphere, "Copa_02", g, new Vector3(1, 4.2f, 0.4f), new Vector3(2.4f, 2.2f, 2.4f), hoja);
        Prim(PrimitiveType.Sphere, "Copa_03", g, new Vector3(-1, 4.4f, -0.3f), new Vector3(2.4f, 2.2f, 2.4f), hoja);
    }

    // Humanoide provisional. Se construye mirando a +X; si miraDerecha es false gira 180.
    static Transform Humanoide(string nombre, Transform padre, Vector3 pos, bool miraDerecha,
        Material torso, Material piel, Material pantalon, Material arma)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        g.rotation = Quaternion.Euler(0, miraDerecha ? 0 : 180, 0);
        Prim(PrimitiveType.Cube, "Pierna_Izq", g, new Vector3(0.12f, 0.42f, 0.16f), new Vector3(0.26f, 0.84f, 0.24f), pantalon, new Vector3(0, 0, 12));
        Prim(PrimitiveType.Cube, "Pierna_Der", g, new Vector3(-0.12f, 0.42f, -0.16f), new Vector3(0.26f, 0.84f, 0.24f), pantalon, new Vector3(0, 0, -12));
        Prim(PrimitiveType.Cube, "Torso", g, new Vector3(0, 1.18f, 0), new Vector3(0.44f, 0.72f, 0.6f), torso);
        Prim(PrimitiveType.Sphere, "Cabeza", g, new Vector3(0.02f, 1.74f, 0), Vector3.one * 0.42f, piel);
        Prim(PrimitiveType.Cube, "Brazo_Izq", g, new Vector3(0.22f, 1.22f, 0.38f), new Vector3(0.6f, 0.17f, 0.17f), torso, new Vector3(0, 0, -12));
        Prim(PrimitiveType.Cube, "Brazo_Der", g, new Vector3(0.14f, 1.16f, -0.38f), new Vector3(0.5f, 0.17f, 0.17f), torso, new Vector3(0, 0, -22));
        Prim(PrimitiveType.Cube, "Fusil", g, new Vector3(0.52f, 1.2f, -0.2f), new Vector3(1.05f, 0.1f, 0.08f), arma);
        Prim(PrimitiveType.Cube, "Fusil_Cargador", g, new Vector3(0.48f, 1.08f, -0.2f), new Vector3(0.1f, 0.24f, 0.07f), arma, new Vector3(0, 0, 12));
        return g;
    }

    static void Vestuario_Chullito(Transform g, Material aguayo, Material chullo, Material mochila)
    {
        Malla("Poncho", piramide, g, new Vector3(0, 0.82f, 0), new Vector3(1.0f, 0.82f, 1.05f), aguayo);
        Malla("Chullo", piramide, g, new Vector3(0.02f, 1.82f, 0), new Vector3(0.46f, 0.36f, 0.46f), chullo);
        Prim(PrimitiveType.Cube, "Chullo_Orejera_Izq", g, new Vector3(0.02f, 1.68f, 0.21f), new Vector3(0.2f, 0.32f, 0.05f), chullo);
        Prim(PrimitiveType.Cube, "Chullo_Orejera_Der", g, new Vector3(0.02f, 1.68f, -0.21f), new Vector3(0.2f, 0.32f, 0.05f), chullo);
        Prim(PrimitiveType.Sphere, "Chullo_Pompon", g, new Vector3(0.02f, 2.2f, 0), Vector3.one * 0.12f, aguayo);
        Prim(PrimitiveType.Cube, "Mochila", g, new Vector3(-0.36f, 1.2f, 0), new Vector3(0.3f, 0.62f, 0.5f), mochila);
    }

    static void Llama(string nombre, Transform padre, Vector3 pos, Material lana, Material aguayo, Material ojo, Material pezuna)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Capsule, "Cuerpo", g, new Vector3(0, 1.15f, 0), new Vector3(0.8f, 0.85f, 0.75f), lana, new Vector3(0, 0, 90));
        Prim(PrimitiveType.Cube, "Manta_Aguayo", g, new Vector3(-0.05f, 1.5f, 0), new Vector3(0.75f, 0.14f, 0.82f), aguayo);
        Prim(PrimitiveType.Capsule, "Cuello", g, new Vector3(0.78f, 1.85f, 0), new Vector3(0.34f, 0.6f, 0.34f), lana, new Vector3(0, 0, -18));
        Prim(PrimitiveType.Cube, "Cabeza", g, new Vector3(1.1f, 2.42f, 0), new Vector3(0.5f, 0.3f, 0.3f), lana);
        Prim(PrimitiveType.Cube, "Oreja_Izq", g, new Vector3(0.92f, 2.68f, 0.1f), new Vector3(0.08f, 0.26f, 0.08f), lana);
        Prim(PrimitiveType.Cube, "Oreja_Der", g, new Vector3(0.92f, 2.68f, -0.1f), new Vector3(0.08f, 0.26f, 0.08f), lana);
        Prim(PrimitiveType.Sphere, "Ojo_Izq", g, new Vector3(1.16f, 2.48f, 0.15f), Vector3.one * 0.09f, ojo);
        Prim(PrimitiveType.Sphere, "Ojo_Der", g, new Vector3(1.16f, 2.48f, -0.15f), Vector3.one * 0.09f, ojo);
        float[] lx = { 0.55f, 0.55f, -0.55f, -0.55f }; float[] lz = { 0.2f, -0.2f, 0.2f, -0.2f };
        for (int i = 0; i < 4; i++)
        {
            Prim(PrimitiveType.Cube, "Pata_" + (i + 1), g, new Vector3(lx[i], 0.45f, lz[i]), new Vector3(0.18f, 0.9f, 0.18f), lana);
            Prim(PrimitiveType.Cube, "Pezuna_" + (i + 1), g, new Vector3(lx[i], 0.06f, lz[i]), new Vector3(0.2f, 0.12f, 0.2f), pezuna);
        }
        Prim(PrimitiveType.Sphere, "Cola", g, new Vector3(-0.92f, 1.3f, 0), Vector3.one * 0.28f, lana);
    }

    static void Dron(string nombre, Transform padre, Vector3 pos, Material cuerpo, Material ojo, Material metal)
    {
        var g = Grupo(nombre, padre); g.position = pos;
        Prim(PrimitiveType.Cube, "Cuerpo", g, Vector3.zero, new Vector3(0.9f, 0.3f, 0.7f), cuerpo);
        Prim(PrimitiveType.Sphere, "Ojo_Sensor", g, new Vector3(-0.42f, -0.05f, 0), Vector3.one * 0.26f, ojo);
        Prim(PrimitiveType.Cube, "Canon", g, new Vector3(-0.3f, -0.26f, 0), new Vector3(0.6f, 0.1f, 0.1f), metal);
        float[] ax = { 0.75f, 0.75f, -0.75f, -0.75f }; float[] az = { 0.65f, -0.65f, 0.65f, -0.65f };
        for (int i = 0; i < 4; i++)
        {
            Prim(PrimitiveType.Cube, "Brazo_" + (i + 1), g, new Vector3(ax[i] / 2, 0.05f, az[i] / 2), new Vector3(0.9f, 0.07f, 0.07f), metal,
                new Vector3(0, Mathf.Atan2(-az[i], ax[i]) * Mathf.Rad2Deg, 0));
            var r = Prim(PrimitiveType.Cylinder, "Rotor_" + (i + 1), g, new Vector3(ax[i], 0.16f, az[i]), new Vector3(0.7f, 0.015f, 0.7f), metal);
            r.gameObject.AddComponent<RotarConstante>().gradosPorSegundo = new Vector3(0, 900, 0);
        }
        var l = new GameObject("Luz_Roja").AddComponent<Light>();
        l.transform.SetParent(g, false);
        l.transform.localPosition = new Vector3(-0.6f, -0.2f, 0);
        l.type = LightType.Point; l.color = new Color(1f, 0.15f, 0.1f); l.range = 7; l.intensity = 8;
    }

    static void Barricada(Transform padre, Vector3 pos, Material saco)
    {
        var g = Grupo("Barricada_Sacos", padre); g.position = pos;
        int n = 0;
        for (int fila = 0; fila < 3; fila++)
            for (int i = 0; i < 4 - fila; i++)
                Prim(PrimitiveType.Capsule, "Saco_" + (++n).ToString("00"), g,
                    new Vector3(0, 0.2f + fila * 0.36f, -1.5f + i * 0.95f + fila * 0.47f),
                    new Vector3(0.45f, 0.5f, 0.6f), saco, new Vector3(90, R(10) - 5, 0));
    }

    // ================= Utilidades =================

    static float R(float max) { return (float)azar.NextDouble() * max; }

    static Transform Grupo(string nombre, Transform padre)
    {
        var g = new GameObject(nombre).transform;
        if (padre != null) g.SetParent(padre, false);
        return g;
    }

    static Transform Prim(PrimitiveType tipo, string nombre, Transform padre, Vector3 pos, Vector3 escala, Material mat, Vector3? rot = null)
    {
        var go = GameObject.CreatePrimitive(tipo);
        go.name = nombre;
        go.transform.SetParent(padre, false);
        go.transform.localPosition = pos;
        go.transform.localScale = escala;
        if (rot.HasValue) go.transform.localRotation = Quaternion.Euler(rot.Value);
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        return go.transform;
    }

    static Transform Malla(string nombre, Mesh malla, Transform padre, Vector3 pos, Vector3 escala, Material mat)
    {
        var go = new GameObject(nombre);
        go.transform.SetParent(padre, false);
        go.transform.localPosition = pos;
        go.transform.localScale = escala;
        go.AddComponent<MeshFilter>().sharedMesh = malla;
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        return go.transform;
    }

    static Shader ShaderLit()
    {
        var rp = GraphicsSettings.currentRenderPipeline;
        Shader s = rp != null ? rp.defaultShader : null;
        if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
        if (s == null) s = Shader.Find("Standard");
        return s;
    }

    static Material Mat(string nombre, Color color, float suavidad, float metal = 0, Texture2D tex = null,
        Vector2? mosaico = null, Color? emision = null)
    {
        string ruta = DirMat + "/" + nombre + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(ruta);
        if (m == null)
        {
            m = new Material(ShaderLit());
            AssetDatabase.CreateAsset(m, ruta);
        }
        m.color = color;
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", suavidad);
        if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", suavidad);
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metal);
        if (tex != null)
        {
            m.mainTexture = tex;
            m.mainTextureScale = mosaico ?? Vector2.one;
        }
        if (emision.HasValue && m.HasProperty("_EmissionColor"))
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", emision.Value);
            m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
        }
        EditorUtility.SetDirty(m);
        return m;
    }

    static Material MatCielo()
    {
        string ruta = DirMat + "/M_Cielo_Atardecer.mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(ruta);
        if (m == null)
        {
            var s = Shader.Find("Skybox/Procedural");
            if (s == null) return RenderSettings.skybox;
            m = new Material(s);
            AssetDatabase.CreateAsset(m, ruta);
        }
        m.SetColor("_SkyTint", new Color(0.42f, 0.52f, 0.86f));
        m.SetColor("_GroundColor", new Color(0.86f, 0.62f, 0.50f));
        m.SetFloat("_AtmosphereThickness", 0.95f);
        m.SetFloat("_Exposure", 1.25f);
        m.SetFloat("_SunSize", 0.06f);
        EditorUtility.SetDirty(m);
        return m;
    }

    delegate Color FuncionPixel(int x, int y, int n);

    static Texture2D Textura(string nombre, int n, FuncionPixel f, bool pixelada)
    {
        string ruta = DirTex + "/" + nombre + ".png";
        if (!File.Exists(ruta))
        {
            var t = new Texture2D(n, n, TextureFormat.RGBA32, false);
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                    t.SetPixel(x, y, f(x, y, n));
            t.Apply();
            File.WriteAllBytes(ruta, t.EncodeToPNG());
            Object.DestroyImmediate(t);
            AssetDatabase.ImportAsset(ruta);
            var imp = AssetImporter.GetAtPath(ruta) as TextureImporter;
            if (imp != null && pixelada)
            {
                imp.filterMode = FilterMode.Point;
                imp.textureCompression = TextureImporterCompression.Uncompressed;
                imp.SaveAndReimport();
            }
        }
        return AssetDatabase.LoadAssetAtPath<Texture2D>(ruta);
    }

    static float Ruido(float x, float y, float escala)
    {
        return Mathf.PerlinNoise(x * escala + 13.7f, y * escala + 71.3f);
    }

    static Color PixelAdobe(int x, int y, int n)
    {
        float v = 0.6f * Ruido(x, y, 0.03f) + 0.3f * Ruido(x, y, 0.11f) + 0.1f * Ruido(x, y, 0.5f);
        var c = Color.Lerp(new Color(0.62f, 0.42f, 0.27f), new Color(0.84f, 0.63f, 0.43f), v);
        int fila = y / 32;
        bool junta = (y % 32) < 2 || ((x + (fila % 2) * 32) % 64) < 2;
        if (junta) c *= 0.86f;
        if (Ruido(x, y, 0.9f) > 0.78f) c = Color.Lerp(c, new Color(0.90f, 0.78f, 0.50f), 0.5f);
        c.a = 1; return c;
    }

    static Color PixelTierra(int x, int y, int n)
    {
        float v = 0.6f * Ruido(x, y, 0.02f) + 0.3f * Ruido(x, y, 0.09f) + 0.1f * Ruido(x, y, 0.6f);
        var c = Color.Lerp(new Color(0.55f, 0.42f, 0.27f), new Color(0.78f, 0.64f, 0.40f), v);
        if (Ruido(x + 300, y, 0.07f) > 0.66f) c = Color.Lerp(c, new Color(0.62f, 0.60f, 0.30f), 0.55f);
        c.a = 1; return c;
    }

    static Color PixelEmpedrado(int x, int y, int n)
    {
        const int celdas = 8;
        float u = (float)x / n * celdas, v = (float)y / n * celdas;
        int cx = Mathf.FloorToInt(u), cy = Mathf.FloorToInt(v);
        float d1 = 9, d2 = 9; int id = 0;
        for (int j = -1; j <= 1; j++)
            for (int i = -1; i <= 1; i++)
            {
                int gx = cx + i, gy = cy + j;
                int wx = ((gx % celdas) + celdas) % celdas, wy = ((gy % celdas) + celdas) % celdas;
                float px = gx + 0.5f + 0.35f * Mathf.Sin(wx * 12.9898f + wy * 78.233f);
                float py = gy + 0.5f + 0.35f * Mathf.Sin(wx * 39.346f + wy * 11.135f);
                float d = (px - u) * (px - u) + (py - v) * (py - v);
                if (d < d1) { d2 = d1; d1 = d; id = wx * 7 + wy * 13; }
                else if (d < d2) d2 = d;
            }
        float borde = Mathf.Sqrt(d2) - Mathf.Sqrt(d1);
        float tono = 0.5f + 0.5f * Mathf.Sin(id * 1.7f);
        var c = Color.Lerp(new Color(0.42f, 0.40f, 0.38f), new Color(0.66f, 0.60f, 0.54f), tono);
        c *= 0.85f + 0.3f * Ruido(x, y, 0.2f);
        if (borde < 0.12f) c = new Color(0.20f, 0.17f, 0.15f);
        c.a = 1; return c;
    }

    static Color PixelTejas(int x, int y, int n)
    {
        int fila = y / 32;
        float fx = ((x + (fila % 2) * 16) % 32) / 32f;
        float curva = Mathf.Sin(fx * Mathf.PI);
        float sombra = 0.6f + 0.4f * ((y % 32) / 32f);
        var c = Color.Lerp(new Color(0.48f, 0.17f, 0.10f), new Color(0.80f, 0.36f, 0.20f), curva) * sombra;
        c *= 0.9f + 0.2f * Ruido(x, y, 0.15f);
        c.a = 1; return c;
    }

    static readonly Color[] ColoresAguayo =
    {
        new Color(0.78f, 0.10f, 0.12f), new Color(0.78f, 0.10f, 0.12f), new Color(0.10f, 0.10f, 0.12f),
        new Color(0.96f, 0.72f, 0.10f), new Color(0.78f, 0.10f, 0.12f), new Color(0.12f, 0.50f, 0.30f),
        new Color(0.95f, 0.95f, 0.90f), new Color(0.85f, 0.15f, 0.50f), new Color(0.78f, 0.10f, 0.12f),
        new Color(0.15f, 0.30f, 0.70f), new Color(0.96f, 0.50f, 0.10f), new Color(0.78f, 0.10f, 0.12f),
        new Color(0.78f, 0.10f, 0.12f), new Color(0.10f, 0.10f, 0.12f), new Color(0.96f, 0.72f, 0.10f),
        new Color(0.78f, 0.10f, 0.12f)
    };

    static Color PixelAguayo(int x, int y, int n)
    {
        return ColoresAguayo[(y * ColoresAguayo.Length / n) % ColoresAguayo.Length];
    }

    static readonly Color[] ColoresWiphala =
    {
        new Color(1f, 1f, 1f), new Color(1f, 0.87f, 0f), new Color(1f, 0.5f, 0f), new Color(0.89f, 0.07f, 0.09f),
        new Color(0.47f, 0.18f, 0.55f), new Color(0.05f, 0.30f, 0.70f), new Color(0.10f, 0.60f, 0.23f)
    };

    static Color PixelWiphala(int x, int y, int n)
    {
        int col = x * 7 / n;
        int fila = 6 - (y * 7 / n); // fila 0 = arriba
        return ColoresWiphala[(((col - fila) % 7) + 7) % 7];
    }

    // ---------- Mallas propias ----------

    static Mesh MallaGuardada(string nombre, System.Func<Mesh> crear)
    {
        string ruta = DirMallas + "/" + nombre + ".asset";
        var m = AssetDatabase.LoadAssetAtPath<Mesh>(ruta);
        if (m != null) return m;
        m = crear();
        m.name = nombre;
        AssetDatabase.CreateAsset(m, ruta);
        return m;
    }

    static Mesh DesdeTriangulos(List<Vector3> t, Vector3 centro)
    {
        var v = new List<Vector3>(); var uv = new List<Vector2>(); var idx = new List<int>();
        for (int i = 0; i < t.Count; i += 3)
        {
            Vector3 a = t[i], b = t[i + 1], c = t[i + 2];
            Vector3 normal = Vector3.Cross(b - a, c - a);
            if (Vector3.Dot(normal, (a + b + c) / 3f - centro) < 0) { var tmp = b; b = c; c = tmp; }
            int k = v.Count;
            v.Add(a); v.Add(b); v.Add(c);
            uv.Add(new Vector2(a.x + a.z + 0.5f, a.y)); uv.Add(new Vector2(b.x + b.z + 0.5f, b.y)); uv.Add(new Vector2(c.x + c.z + 0.5f, c.y));
            idx.Add(k); idx.Add(k + 1); idx.Add(k + 2);
        }
        var m = new Mesh();
        m.SetVertices(v); m.SetUVs(0, uv); m.SetTriangles(idx, 0);
        m.RecalculateNormals(); m.RecalculateBounds();
        return m;
    }

    // Piramide de base 1x1 (y=0) y vertice en y=1
    static Mesh CrearPiramide()
    {
        Vector3 p0 = new Vector3(-0.5f, 0, -0.5f), p1 = new Vector3(0.5f, 0, -0.5f),
                p2 = new Vector3(0.5f, 0, 0.5f), p3 = new Vector3(-0.5f, 0, 0.5f), a = new Vector3(0, 1, 0);
        var t = new List<Vector3> { p0, p1, a, p1, p2, a, p2, p3, a, p3, p0, a, p0, p1, p2, p0, p2, p3 };
        return DesdeTriangulos(t, new Vector3(0, 0.3f, 0));
    }

    // Prisma triangular (techo a dos aguas): base 1x1, cumbrera a lo largo de X en y=1
    static Mesh CrearPrisma()
    {
        Vector3 f0 = new Vector3(-0.5f, 0, -0.5f), f1 = new Vector3(0.5f, 0, -0.5f),
                b0 = new Vector3(-0.5f, 0, 0.5f), b1 = new Vector3(0.5f, 0, 0.5f),
                c0 = new Vector3(-0.5f, 1, 0), c1 = new Vector3(0.5f, 1, 0);
        var t = new List<Vector3>
        {
            f0, f1, c1, f0, c1, c0,   // faldon frontal
            b0, b1, c1, b0, c1, c0,   // faldon trasero
            f0, b0, c0, f1, b1, c1,   // hastiales
            f0, f1, b1, f0, b1, b0    // base
        };
        return DesdeTriangulos(t, new Vector3(0, 0.3f, 0));
    }
}
