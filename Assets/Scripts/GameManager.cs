using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logic = Packages.com.terallite.gamelogic.Runtime;


public class GameManager : MonoBehaviour
{
    private const float TurnDuration = 2f;

    [field: SerializeField] public GameObject Laser { get; private set; }
    [field: SerializeField] public GameObject Fighter { get; private set; }
    [field: SerializeField] public GameObject EnemyLaser { get; private set; }
    [field: SerializeField] public GameObject EnemyFighter { get; private set; }
    [field: SerializeField] public GameObject SceneWorldGrid { get; private set; }
    [field: SerializeField] public UserInteractionManager UserInteractionManager { get; private set; } = new();
    [field: SerializeField] public TextMeshProUGUI EndTurnButtonText { get; private set; }

    [field: SerializeField] public Button EndTurnButton { get; private set; }
    private readonly Logic.World _world = new();
    private Transform _player;
    private State _state = State.Standby;

    private float _turnStartTime;
    private Dictionary<Logic.EntityType, Dictionary<Logic.EntityTeam, GameObject>> EntityViewPrefabs { get; } = new();

    private void Awake()
    {
        EntityViewPrefabs[Logic.EntityType.Fighter] = new Dictionary<Logic.EntityTeam, GameObject>
        {
            { Logic.EntityTeam.Ally, Fighter },
            { Logic.EntityTeam.Enemy, EnemyFighter }
        };
        EntityViewPrefabs[Logic.EntityType.Laser] = new Dictionary<Logic.EntityTeam, GameObject>
        {
            { Logic.EntityTeam.Ally, Laser },
            { Logic.EntityTeam.Enemy, EnemyLaser }
        };

        _world.Spawned += WorldOnSpawned;
        _world.Won += OnWin;
        _world.Lost += OnLose;
        UserInteractionManager.Init();
    }

    private void Start()
    {
        _world.Begin();

        SceneWorldGrid.GetComponent<WorldGrid>().Interacted += UserInteractionManager.OnInteraction;
        SceneWorldGrid.GetComponent<WorldGrid>().CursorMoved += UserInteractionManager.OnCursorMoved;
    }

    public void Update()
    {
        if (_state == State.RunTurn)
        {
            _world.Tick(Time.deltaTime);
            if (Time.time >= _turnStartTime + TurnDuration)
            {
                _state = State.Standby;
                SceneWorldGrid.SetActive(true);
                EndTurnButtonText.text = "End Turn";
                EndTurnButton.interactable = true;
            }
        }
    }

    private void OnWin()
    {
        EndTurnButton.gameObject.SetActive(false);
        UserInteractionManager.OnWin();
    }

    private void OnLose()
    {
        EndTurnButton.gameObject.SetActive(false);
        UserInteractionManager.OnLose();
    }

    private void WorldOnSpawned(object sender, Logic.Entity entity)
    {
        // TODO: Introduce an object pool to avoid creating garbage for the GC that will eventually result in 
        //       frame spikes if there are enough game objects added and removed from the scene.
        var newViewGameObject = Instantiate(EntityViewPrefabs[entity.Type][entity.Team]);
        var newEntityViewComponent = newViewGameObject.AddComponent<EntityView>();
        newEntityViewComponent.AssociatedLogicalEntity = entity;
        entity.Despawned += newEntityViewComponent.OnLogicalEntityDespawned;
        entity.AssociatedObject = newViewGameObject;
    }

    public void OnTurnEndClicked()
    {
        if (_state == State.Standby)
        {
            _state = State.RunTurn;
            _world.EndTurn();
            _turnStartTime = Time.time;
            SceneWorldGrid.SetActive(false);
            EndTurnButton.interactable = false;
            EndTurnButtonText.text = "Wait...";
        }
    }

    private enum State
    {
        Standby,
        RunTurn,
        Win,
        Lose
    }
}