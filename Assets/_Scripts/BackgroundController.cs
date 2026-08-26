using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 2f;
    [SerializeField] private float _resetPositionY = -20f;
    private Vector3 _startPosition;
    private SpriteRenderer _spriteRenderer;

    void Reset()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FitWidthToCamera();
    }

    void Awake()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down*_scrollSpeed*Time.deltaTime);
        if(transform.position.y<_resetPositionY)
        {
            transform.position = _startPosition;
        }
    }
    private void FitWidthToCamera()
    {
    Camera mainCamera = Camera.main;

    // Chiều cao camera trong World Units
    float cameraHeight = mainCamera.orthographicSize * 2f;

    // Chiều rộng camera
    float cameraWidth = cameraHeight * mainCamera.aspect;

    // Chiều rộng hiện tại của Background
    float backgroundWidth = _spriteRenderer.bounds.size.x;

    // Tỷ lệ scale cần thiết
    float scaleFactor = cameraWidth / backgroundWidth;

    // Chỉ scale theo chiều ngang
    Vector3 scale = transform.localScale;
    scale.x *= scaleFactor;

    transform.localScale = scale;
    }

}
