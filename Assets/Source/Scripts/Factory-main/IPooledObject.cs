public interface IPooledObject
{
    void ReturnToPool();
    void Initialize();
    void SetParentPool<T>(IPool<T> parent) where T : IPooledObject;
}