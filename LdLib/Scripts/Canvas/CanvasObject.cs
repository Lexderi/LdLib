using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LdLib
{
    public abstract class CanvasObject: IDisposable
    {
        internal static List<CanvasObject> All = new();

        protected CanvasObject()
        {
            All.Add(this);
        }

        protected virtual void Update() {}

        internal void UpdateInternal()
        {
            Update();
        }

        public void Destroy()
        {
            All.Remove(this);
        }
        public void Dispose()
        {
            Destroy();
        }
    }
}
