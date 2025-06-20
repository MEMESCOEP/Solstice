using Solstice.Engine.Components;

namespace Solstice.Engine.Classes;

public class Scene
{
    public List<GameObject> GameObjects;

    public Scene()
    {
        GameObjects = new List<GameObject>();
        GameObjects.Add(new GameObject("Camera") { Components = { new CameraComponent() } });
    }
    
    public void AddGameObject(GameObject gameObject)
    {
        GameObjects.Add(gameObject);
    }
    
    public void RemoveGameObject(GameObject gameObject)
    {
        GameObjects.Remove(gameObject);
    }
    
    public void Update(float deltaTime)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(deltaTime);
        }
    }
    
    public void Start()
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Start();
        }
    }
    
    public void Setup()
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Setup();
        }
    }
    
    public GameObject? GetGameObject(string name)
    {
        return GameObjects.FirstOrDefault(go => go.Name == name);
    }
    
    public List<GameObject> GetGameObjectsByType<T>() where T : Component
    {
        return GameObjects.Where(go => go.GetComponent<T>() != null).ToList();
    }
}