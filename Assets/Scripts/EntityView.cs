using UnityEngine;
using Logic = Packages.com.terallite.gamelogic.Runtime;

// Links logical game entities to their Unity GameObject representations.
// Synchronizes position, rotation, and health between model and view.
// Acts as the view component in the MVC architecture.
public class EntityView : MonoBehaviour
{
    private Logic.IDurable _associatedDurableEntity;
    private Logic.Entity _associatedLogicalEntity;

    private HealthBar _healthBar;

    public Logic.Entity AssociatedLogicalEntity
    {
        get => _associatedLogicalEntity;
        internal set
        {
            if (_associatedLogicalEntity != null)
            {
                _associatedLogicalEntity.Despawned -= OnLogicalEntityDespawned;
            }
            _associatedLogicalEntity = value;
            _associatedDurableEntity = _associatedLogicalEntity as Logic.IDurable;
            _associatedLogicalEntity.Despawned += OnLogicalEntityDespawned;
        }
    }

    private void Awake()
    {
        _healthBar = gameObject.GetComponent<HealthBar>();
    }

    private void Start()
    {
        ApplyPositionAndHeading();
    }

    private void Update()
    {
        ApplyPositionAndHeading();
        if (_healthBar && _associatedDurableEntity != null)
        {
            _healthBar.FilledRatio = _associatedDurableEntity.RelativeDurability;
        }
    }

    internal void OnLogicalEntityDespawned(Logic.Entity _)
    {
        Destroy(gameObject);
    }

    private void ApplyPositionAndHeading()
    {
        transform.position = AssociatedLogicalEntity.Position.ToUnityVector3();
        transform.forward = AssociatedLogicalEntity.Heading.ToUnityVector3();
    }
}