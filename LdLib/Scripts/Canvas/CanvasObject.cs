namespace LdLib;

public abstract class CanvasObject : IDisposable
{
    internal static List<CanvasObject> All = new();

    protected CanvasObject()
    {
        All.Add(this);
    }

    public void Dispose()
    {
        Destroy();
    }

    protected virtual void Update()
    {
    }

    internal void UpdateInternal()
    {
        Update();
    }

    public void Destroy()
    {
        All.Remove(this);
    }
}