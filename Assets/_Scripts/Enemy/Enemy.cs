using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _damage = 1;
    [SerializeField] private EnemyPoolManager _enemyPoolManager;

    void Reset()
    {
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MoveDown();
        CheckReleasePool();
    }

    void MoveDown()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + Vector2.down * _moveSpeed * Time.fixedDeltaTime);
    }

    void CheckReleasePool()
    {
        if(transform.position.y>-6f) return;
        ReleasePool();
    }

    void ReleasePool()
    {
        _enemyPoolManager.Release(transform.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerDamageReceiver>().DeductHp(_damage);
            _enemyPoolManager.Release(transform.gameObject);
        }
    }

}
