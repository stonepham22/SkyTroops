using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] private float _hp = 10;
    public void DeductHp(float hp)
    {
        _hp -= hp;
    }

}
