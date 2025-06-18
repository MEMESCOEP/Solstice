namespace Solstice.Engine.Classes;

public class Scene
{
    public List<GameObject> GameObjects;

    public Scene()
    {
        GameObjects = new List<GameObject>();
        GameObjects.Add(new GOCamera());
    }
}