using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    void Reset()
    {
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MoveDown();
    }
    void MoveDown()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + Vector2.down * _moveSpeed * Time.fixedDeltaTime);
    }
}
