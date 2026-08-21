using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D planeCollider;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Boundary")]
    [SerializeField] private float screenPadding = 0.1f;

    [SerializeField] private bool isTouching;
    private Vector2 currentTouchPosition;
    private Vector2 previousTouchPosition;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (planeCollider == null)
            planeCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!isTouching)
            return;

        bool mousePressed = Mouse.current != null && Mouse.current.leftButton.isPressed;
        bool touchPressed = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed;

        if (!mousePressed && !touchPressed)
            EndTouch();
    }

    // =========================================================
    // INPUT
    // =========================================================

    /// <summary>
    /// Được Player Input gọi khi TouchPosition thay đổi.
    /// Player Input phải dùng Behavior = Send Messages.
    /// </summary>
    public void OnTouchPosition(InputValue value)
    {
        currentTouchPosition = value.Get<Vector2>();

        if (!isTouching)
            return;

        MovePlane();
    }

    /// <summary>
    /// Được Player Input gọi khi người chơi nhấn / nhả màn hình.
    /// </summary>
    public void OnPress(InputValue value)
    {
        bool pressed = value.Get<float>() > 0.5f;

        if (pressed)
        {
            if (IsPointerOverPlane())
                StartTouch();
        }
        else
        {
            EndTouch();
        }
    }

    // =========================================================
    // TOUCH
    // =========================================================

    private bool IsPointerOverPlane()
    {
        if (mainCamera == null)
            return false;

        float distanceFromCamera =
            Mathf.Abs(transform.position.z - mainCamera.transform.position.z);

        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(
            new Vector3(currentTouchPosition.x, currentTouchPosition.y, distanceFromCamera)
        );

        if (planeCollider != null)
            return planeCollider.OverlapPoint(worldPoint);

        return spriteRenderer != null && spriteRenderer.bounds.Contains(worldPoint);
    }

    private void StartTouch()
    {
        isTouching = true;

        // Bắt đầu từ vị trí hiện tại của ngón tay.
        // Không để Plane bị giật ngay khi chạm.
        previousTouchPosition = currentTouchPosition;
    }

    private void EndTouch()
    {
        isTouching = false;
    }

    // =========================================================
    // MOVEMENT
    // =========================================================

    private void MovePlane()
    {
        Vector2 delta = currentTouchPosition - previousTouchPosition;

        if (delta.sqrMagnitude <= 0f)
            return;

        Vector3 worldDelta = ScreenDeltaToWorldDelta(delta);

        previousTouchPosition = currentTouchPosition;

        worldDelta *= moveSpeed;

        transform.position += worldDelta;

        ClampPosition();
    }

    /// <summary>
    /// Chuyển khoảng cách di chuyển trên màn hình
    /// thành khoảng cách trong World Space.
    /// </summary>
    private Vector3 ScreenDeltaToWorldDelta(Vector2 screenDelta)
    {
        float distanceFromCamera =
            Mathf.Abs(transform.position.z - mainCamera.transform.position.z);

        Vector3 currentWorldPosition =
            mainCamera.ScreenToWorldPoint(
                new Vector3(
                    currentTouchPosition.x,
                    currentTouchPosition.y,
                    distanceFromCamera
                )
            );

        Vector3 previousWorldPosition =
            mainCamera.ScreenToWorldPoint(
                new Vector3(
                    previousTouchPosition.x,
                    previousTouchPosition.y,
                    distanceFromCamera
                )
            );

        return currentWorldPosition - previousWorldPosition;
    }

    // =========================================================
    // BOUNDARY
    // =========================================================

    private void ClampPosition()
    {
        Vector3 position = transform.position;

        Vector3 minScreen =
            mainCamera.ViewportToWorldPoint(
                new Vector3(0f, 0f, Mathf.Abs(transform.position.z - mainCamera.transform.position.z))
            );

        Vector3 maxScreen =
            mainCamera.ViewportToWorldPoint(
                new Vector3(1f, 1f, Mathf.Abs(transform.position.z - mainCamera.transform.position.z))
            );

        float halfWidth = 0f;
        float halfHeight = 0f;

        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.extents.x;
            halfHeight = spriteRenderer.bounds.extents.y;
        }

        position.x = Mathf.Clamp(
            position.x,
            minScreen.x + halfWidth + screenPadding,
            maxScreen.x - halfWidth - screenPadding
        );

        position.y = Mathf.Clamp(
            position.y,
            minScreen.y + halfHeight + screenPadding,
            maxScreen.y - halfHeight - screenPadding
        );

        transform.position = position;
    }
}