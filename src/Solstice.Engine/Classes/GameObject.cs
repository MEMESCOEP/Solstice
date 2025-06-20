using Solstice.Common.Classes;
using Solstice.Engine.Interfaces;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Classes;

public class GameObject
{
    /// <summary>
    /// The name of the current gameobject
    /// </summary>
    public string Name { get; set; } = "New gameobject";
    
    /// <summary>
    /// Describes how the object is positioned, rotated, and scaled in 3D space
    /// </summary>
    public Transform Transform { get; set; }
    
    /// <summary>
    /// Stores a list of components that are attached to this gameobject
    /// </summary>
    public List<Component> Components { get; } =  new List<Component>();

    public bool Enabled { get; set; } = true;

    public GameObject()
    {
        Transform = new Transform();
    }

    public GameObject(string name)
    {
        Name = name;
        Transform = new Transform();
    }
    
    /// <summary>
    /// Adds a component to the current gameobject
    /// </summary>
    /// <param name="component">The component to add</param>
    public void AddComponent(Component Component)
    {
        Component.Owner = this;
        Components.Add(Component);
    }

    /// <summary>
    /// Gets one or more components of a specific type
    /// </summary>
    /// <typeparam name="T">The component type to get</typeparam>
    /// <returns>One or more components that match the provided type</returns>
    public T? GetComponent<T>() where T : Component
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Used to set up the component
    /// </summary>
    public void Setup()
    {
        foreach (var component in Components)
        {
            component.Setup();
        }
    }

    /// <summary>
    /// Called once when the object enters the scene
    /// </summary>
    public void Start()
    {
        foreach (var component in Components)
        {
            component.Start();
        }
    }

    /// <summary>
    /// Called every frame once the object enters the tree
    /// </summary>
    public void Update(IWindow window)
    {
        foreach (var component in Components)
        {
            component.Update(window);
        }
    }

    public void Render(IGraphics graphics)
    {
        foreach (var component in Components)
        {
            if (component is IRenderable renderable)
            {
                renderable.Render(graphics);
            }
        }
    }
}