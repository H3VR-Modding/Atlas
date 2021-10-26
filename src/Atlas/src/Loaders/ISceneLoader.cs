using System.Collections;

namespace Atlas.Loaders
{
    public interface ISceneLoader
    {
        public void Awake();
        public IEnumerator Start();
    }
}