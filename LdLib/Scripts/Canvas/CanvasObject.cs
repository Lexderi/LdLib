namespace LdLib;

/// <summary>
/// An object that gets updated each frame by the canvas
/// THESE OBJECTS HAVE TO BE DESTROYED MANUALLY
/// </summary>
public abstract class CanvasObject
{
    internal static List<CanvasObject> All = new();

    protected CanvasObject()
    {
        All.Add(this);
    }

    /// <summary>
    /// This will be called every frame
    /// </summary>
    protected virtual void Update()
    {
    }

    internal void UpdateInternal()
    {
        Update();
    }

    /// <summary>
    /// The object won't be updated anymore and allows to be fully removed
    /// </summary>
    public void Destroy()
    {
        All.Remove(this);
    }
}