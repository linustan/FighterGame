// State machine for handling user input in a structured way.
// This could be turned into a hierarchical state machine for a more complex game.

using System;
using TMPro;
using UnityEngine;
using Logic = Packages.com.terallite.gamelogic.Runtime;
using Object = UnityEngine.Object;

// Manages player interactions through a state machine pattern.
// Handles selection of units, movement orders, and targeting.
// Demonstrates clean separation between input handling and game logic.
[Serializable]
public class UserInteractionManager
{
    [field: SerializeField] public TextMeshProUGUI InstructionsText { get; private set; }
    [field: SerializeField] public GameObject DestinationIndicator { get; private set; }
    [field: SerializeField] public GameObject TargetIndicator { get; private set; }

    private State _currentState;
    private GameObject _destinationIndicator;
    private GameObject _selectedFighter;
    private GameObject _targetIndicator;

    public void Init()
    {
        DestinationIndicator.SetActive(false);
        _destinationIndicator = Object.Instantiate(DestinationIndicator);
        TargetIndicator.SetActive(false);
        _targetIndicator = Object.Instantiate(TargetIndicator);
    }

    public void Tick(float deltaTime) { }

    public void OnWin()
    {
        InstructionsText.text = "Congratulations, You Won!";
    }

    public void OnLose()
    {
        InstructionsText.text = "Game Over!";
    }

    public void OnInteraction(Vector3 interactionPoint, GameObject hitEntity)
    {
        switch (_currentState)
        {
            case State.Idle:
                OnIdleInteraction(interactionPoint, hitEntity);
                break;
            case State.DraggingDestination:
                OnDraggingDestinationInteraction(interactionPoint, hitEntity);
                break;
            case State.DraggingLaserAttackTarget:
                OnDraggingLaserAttackTargetInteraction(interactionPoint, hitEntity);
                break;
        }
    }

    public void OnCursorMoved(Vector3 interactionPoint, GameObject hitEntity)
    {
        switch (_currentState)
        {
            case State.DraggingDestination:
                OnDraggingDestinationCursorMoved(interactionPoint, hitEntity);
                break;
            case State.DraggingLaserAttackTarget:
                OnDraggingLaserAttackTargetCursorMoved(interactionPoint, hitEntity);
                break;
        }
    }

    private void OnIdleInteraction(Vector3 interactionPoint, GameObject hitEntity)
    {
        var entityView = hitEntity.gameObject.GetComponent<EntityView>();
        if (!entityView || entityView.AssociatedLogicalEntity.Team != Logic.EntityTeam.Ally) return;
        _currentState = State.DraggingDestination;
        InstructionsText.text = "Click to set Fighter destination";
        Cursor.visible = false;
        _selectedFighter = hitEntity;
    }
    private void OnDraggingDestinationCursorMoved(Vector3 interactionPoint, GameObject hitEntity)
    {
        // TODO: Refactor 2.5D UI interactions to work in a fully 2D space.
        _destinationIndicator.transform.position = interactionPoint + Vector3.up;
        _destinationIndicator.transform.forward = (interactionPoint - _selectedFighter.transform.position).normalized;
        if (!_destinationIndicator.activeSelf)
        {
            _destinationIndicator.SetActive(true);
        }
    }

    private void OnDraggingDestinationInteraction(Vector3 interactionPoint, GameObject hitEntity)
    {
        _destinationIndicator.SetActive(false);
        if (_selectedFighter.GetComponent<EntityView>().AssociatedLogicalEntity is Logic.Fighter fighter)
        {
            fighter.Destination = new Logic.Vector2(_destinationIndicator.transform.position.x, _destinationIndicator.transform.position.z);
        }
        _currentState = State.DraggingLaserAttackTarget;
        InstructionsText.text = "Click to set laser target";
    }

    private void OnDraggingLaserAttackTargetCursorMoved(Vector3 interactionPoint, GameObject hitEntity)
    {
        // TODO: Refactor 2.5D UI interactions to work in a fully 2D space.
        //       Also think of a good way to unify similar interaction code.
        _targetIndicator.transform.position = interactionPoint + Vector3.up;
        _targetIndicator.transform.forward = (interactionPoint - _selectedFighter.transform.position).normalized;
        if (!_targetIndicator.activeSelf)
        {
            _targetIndicator.SetActive(true);
        }
    }

    private void OnDraggingLaserAttackTargetInteraction(Vector3 interactionPoint, GameObject hitEntity)
    {
        _targetIndicator.SetActive(false);
        if (_selectedFighter.GetComponent<EntityView>().AssociatedLogicalEntity is Logic.Fighter fighter)
        {
            fighter.LaserTarget = new Logic.Vector2(_targetIndicator.transform.position.x, _targetIndicator.transform.position.z);
        }
        _selectedFighter = null;
        Cursor.visible = true;
        _currentState = State.Idle;
        InstructionsText.text = "Select a blue Fighter or click End Turn";
    }

    private enum State
    {
        Idle,
        Locked,
        FighterSelected,
        DraggingDestination,
        DraggingLaserAttackTarget
    }
}