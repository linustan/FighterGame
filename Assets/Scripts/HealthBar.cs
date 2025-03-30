using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [field: SerializeField] public GameObject HealthBarPrefab { get; private set; }

    // How much of the bar is filled (0f - 1f)
    private float _filledRatio;

    private GameObject _healthBar;
    private Camera _mainCamera;

    public float FilledRatio
    {
        set => _healthBar.transform.localScale = new Vector2(value * 5f, 5f);
    }

    private void Start()
    {
        _healthBar = Instantiate(HealthBarPrefab, FindAnyObjectByType<Canvas>().gameObject.transform);
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        _healthBar.transform.position = _mainCamera.WorldToScreenPoint(gameObject.transform.position) + Vector3.down * 60f;
    }

    private void OnDestroy()
    {
        Destroy(_healthBar);
    }
}