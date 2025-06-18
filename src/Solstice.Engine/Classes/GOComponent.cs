namespace Solstice.Engine.Classes;

public abstract class GOComponent
{
    /// <summary>
    /// Holds a reference to the gameobject that this component is attached to
    /// </summary>
    public GameObject Owner { get; internal set; }

    /// <summary>
    /// Gets the gameobject this script is attached to
    /// </summary>
    /// <returns>A reference to the gameobject that this script is attached to</returns>
    public GameObject GetOwner() => Owner;
}