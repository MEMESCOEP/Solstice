using Solstice.Engine.Components;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Classes;

public class Scene
{
    public List<GameObject> GameObjects;

    public Scene()
    {
        GameObjects = new List<GameObject>();
        AddGameObject(new GameObject("Camera") { Components = { new CameraComponent() } });
    }
    
    public void AddGameObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            throw new ArgumentNullException(nameof(gameObject), "GameObject cannot be null");
        }
        
        if (GameObjects.Any(go => go.Name == gameObject.Name))
        {
            throw new InvalidOperationException($"A GameObject with the name '{gameObject.Name}' already exists in the scene.");
        }
        
        foreach (var component in gameObject.Components)
        {
            component.Owner = gameObject;
        }
        
        GameObjects.Add(gameObject);
    }
    
    public void RemoveGameObject(GameObject gameObject)
    {
        GameObjects.Remove(gameObject);
    }
    
    public void Update(IWindow window)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(window);
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

    public void Render(IGraphics graphics)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Render(graphics);
        }
        
        graphics.Cameras = GetGameObjectsByType<CameraComponent>()
            .Select(c => c.GetComponent<CameraComponent>()!.Camera)
            .ToList();
        
        graphics.Render(); // Actually render the scene
    }
}