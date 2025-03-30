using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public delegate void InteractionHandler(Vector3 interactionPoint, GameObject hitEntity);
    private readonly RaycastHit[] _raycastResults = new RaycastHit[1];
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        foreach (var layerName in new[] { "Markers", "GoodGuys", "Grid" })
        {
            var layerMask = 1 << LayerMask.NameToLayer(layerName);
            var ray = _camera!.ScreenPointToRay(Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, _raycastResults, Mathf.Infinity, layerMask) > 0)
            {
                var hit = _raycastResults[0];
                CursorMoved?.Invoke(hit.point, hit.collider.gameObject);
                if (Input.GetMouseButtonDown(0))
                {
                    Interacted?.Invoke(hit.point, hit.collider.gameObject);
                }
                return;
            }
        }
    }

    public event InteractionHandler Interacted;
    public event InteractionHandler CursorMoved;
}