using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Classes;

public abstract class Component
{
    /// <summary>
    /// Holds a reference to the gameobject that this component is attached to
    /// </summary>
    public GameObject Owner { get; internal set; }
    
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets the gameobject this script is attached to
    /// </summary>
    /// <returns>A reference to the gameobject that this script is attached to</returns>
    public GameObject GetOwner() => Owner;
    
    /// <summary>
    /// Used to set up the component
    /// </summary>
    public abstract void Setup();
    
    /// <summary>
    /// Called every frame once the object enters the tree
    /// </summary>
    public abstract void Update(IWindow window);
    
    /// <summary>
    /// Called once when the object enters the scene
    /// </summary>
    public abstract void Start();
}