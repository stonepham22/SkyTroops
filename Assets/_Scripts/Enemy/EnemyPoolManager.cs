using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private IObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: CreateObject,
            actionOnGet:OnGet,
            actionOnRelease: OnRelease,
            collectionCheck: true,
            defaultCapacity: 1,
            maxSize: int.MaxValue
        );
    }
    private GameObject CreateObject()
    {
        return Instantiate(_prefab, transform);
    }
    private void OnGet(GameObject instance)
    {
        instance.SetActive(true);
    }
    private void OnRelease(GameObject instance)
    {
        instance.SetActive(false);
    }
    private void OnDestroyPoolObject(GameObject instance)
    {
        Destroy(instance);
    }
    public GameObject Get()
    {
        return _pool.Get();
    }
    public void Release(GameObject instance)
    {
        _pool.Release(instance);
    }
}
