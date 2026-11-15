using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

// Builds the whole Roll-a-Ball scene for you.
// It runs once automatically the first time the project opens.
// You can run it again from the menu: Tools > Roll-a-Ball > Build Scene.
[InitializeOnLoad]
public static class RollABallSetup
{
    const string ScenePath = "Assets/Scenes/RollABall.unity";

    static RollABallSetup()
    {
        EditorApplication.delayCall += () =>
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !File.Exists(ScenePath))
            {
                BuildScene();
            }
        };
    }

    [MenuItem("Tools/Roll-a-Ball/Build Scene")]
    public static void BuildScene()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Materials");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        Material groundMat = MakeMaterial("Ground", new Color(0.30f, 0.55f, 0.35f));
        Material wallMat = MakeMaterial("Wall", new Color(0.55f, 0.55f, 0.60f));
        Material playerMat = MakeMaterial("Player", new Color(0.20f, 0.45f, 0.95f));
        Material coinMat = MakeMaterial("Coin", new Color(1.00f, 0.80f, 0.10f));
        Material obstacleMat = MakeMaterial("Obstacle", new Color(0.90f, 0.20f, 0.20f));

        // Ground: a plane is 10 x 10 units, scale 2 makes it 20 x 20.
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(2f, 1f, 2f);
        ground.GetComponent<Renderer>().sharedMaterial = groundMat;

        // Four walls so the ball cannot fall off.
        GameObject walls = new GameObject("Walls");
        MakeWall(walls, wallMat, new Vector3(0f, 0.5f, 10.25f), new Vector3(21f, 1f, 0.5f));
        MakeWall(walls, wallMat, new Vector3(0f, 0.5f, -10.25f), new Vector3(21f, 1f, 0.5f));
        MakeWall(walls, wallMat, new Vector3(10.25f, 0.5f, 0f), new Vector3(0.5f, 1f, 21f));
        MakeWall(walls, wallMat, new Vector3(-10.25f, 0.5f, 0f), new Vector3(0.5f, 1f, 21f));

        // Player ball.
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 0.5f, -7f);
        player.GetComponent<Renderer>().sharedMaterial = playerMat;
        player.AddComponent<Rigidbody>();
        player.AddComponent<PlayerController>();

        // Coins.
        GameObject coins = new GameObject("Coins");
        Vector3[] coinSpots =
        {
            new Vector3(-7f, 0.6f, -7f), new Vector3(7f, 0.6f, -7f),
            new Vector3(-4f, 0.6f, -2f), new Vector3(4f, 0.6f, -2f),
            new Vector3(0f, 0.6f, 1f),
            new Vector3(-7f, 0.6f, 4f), new Vector3(7f, 0.6f, 4f),
            new Vector3(-3f, 0.6f, 8f), new Vector3(3f, 0.6f, 8f),
            new Vector3(0f, 0.6f, 5f),
        };
        foreach (Vector3 spot in coinSpots)
        {
            GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Coin";
            coin.transform.SetParent(coins.transform);
            coin.transform.position = spot;
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            coin.transform.localScale = new Vector3(0.7f, 0.05f, 0.7f);
            coin.GetComponent<Renderer>().sharedMaterial = coinMat;
            coin.GetComponent<Collider>().isTrigger = true;
            coin.AddComponent<Coin>();
        }

        // Obstacles: two that stand still, two that slide.
        GameObject obstacles = new GameObject("Obstacles");
        MakeObstacle(obstacles, obstacleMat, new Vector3(-4f, 0.5f, -5f), 0f);
        MakeObstacle(obstacles, obstacleMat, new Vector3(4f, 0.5f, 6.5f), 0f);
        MakeObstacle(obstacles, obstacleMat, new Vector3(0f, 0.5f, -0.5f), 5f);
        MakeObstacle(obstacles, obstacleMat, new Vector3(0f, 0.5f, 3f), 6f);

        // Game manager.
        new GameObject("GameManager").AddComponent<GameManager>();

        // Camera follows the ball.
        Camera cam = Camera.main;
        if (cam != null)
        {
            CameraFollow follow = cam.gameObject.AddComponent<CameraFollow>();
            follow.target = player.transform;
            cam.transform.position = player.transform.position + follow.offset;
            cam.transform.LookAt(player.transform);
        }

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Roll-a-Ball scene is ready. Press Play!");
    }

    static void MakeWall(GameObject parent, Material mat, Vector3 position, Vector3 size)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.SetParent(parent.transform);
        wall.transform.position = position;
        wall.transform.localScale = size;
        wall.GetComponent<Renderer>().sharedMaterial = mat;
    }

    static void MakeObstacle(GameObject parent, Material mat, Vector3 position, float moveDistance)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = moveDistance > 0f ? "Moving Obstacle" : "Obstacle";
        box.transform.SetParent(parent.transform);
        box.transform.position = position;
        box.GetComponent<Renderer>().sharedMaterial = mat;

        Obstacle obstacle = box.AddComponent<Obstacle>();
        obstacle.moveDistance = moveDistance;

        if (moveDistance > 0f)
        {
            // A kinematic rigidbody lets a script move the box while physics still works.
            Rigidbody rb = box.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    static Material MakeMaterial(string name, Color color)
    {
        // Pick the shader that matches the project's render pipeline.
        Shader shader = null;
        if (GraphicsSettings.defaultRenderPipeline != null)
        {
            shader = Shader.Find("Universal Render Pipeline/Lit");
        }
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material mat = new Material(shader);
        mat.color = color;
        AssetDatabase.CreateAsset(mat, "Assets/Materials/" + name + ".mat");
        return mat;
    }
}
