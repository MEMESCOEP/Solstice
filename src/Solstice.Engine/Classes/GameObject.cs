using Solstice.Common.Classes;

namespace Solstice.Engine.Classes;

public abstract class GameObject
{
    /// <summary>
    /// The name of the current gameobject
    /// </summary>
    public string Name { get; set; } = "New gameobject";
    
    /// <summary>
    /// Describes how the object is positioned, rotated, and scaled in 3D space
    /// </summary>
    Transform Transform { get; set; }
    
    /// <summary>
    /// Stores a list of components that are attached to this gameobject
    /// </summary>
    public List<GOComponent> Components { get; } =  new List<GOComponent>();
    
    /// <summary>
    /// Adds a component to the current gameobject
    /// </summary>
    /// <param name="component">The component to add</param>
    public void AddComponent(GOComponent Component)
    {
        Component.Owner = this;
        Components.Add(Component);
    }

    /// <summary>
    /// Gets one or more components of a specific type
    /// </summary>
    /// <typeparam name="T">The component type to get</typeparam>
    /// <returns>One or more components that match the provided type</returns>
    public T GetComponent<T>() where T : GOComponent
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Used to set up the component
    /// </summary>
    public abstract void Setup();

    /// <summary>
    /// Called once when the object enters the scene
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Called every frame once the object enters the tree
    /// </summary>
    /// <param name="DeltaTime"></param>
    public abstract void Update(float DeltaTime);
}