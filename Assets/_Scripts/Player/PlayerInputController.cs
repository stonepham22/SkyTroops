using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public void OnTouchPosition(InputValue value)
    {
        Vector2 position = value.Get<Vector2>();

        Debug.Log($"Touch Position: {position}");
    }
}