using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(PlayerAudio))]
public class HexPlayer : MonoBehaviour
{
    public GridWorld world;
    public MoveButton moveUpButton;
    public MoveButton moveLeftButton;
    public MoveButton moveRightButton;
    public MoveButton moveStayButton;

    public float speed = 3.0f;

    Vector3Int tilemapPos;
    Inventory inventory;
    PlayerAudio playerAudio;

    public enum State
    {
        Ready,
        Moving,
        Waiting,
    }

    State state;
    Vector3 target;
    CustomTile.Interaction queuedInteraction;

    public State CurrentState { get { return state; } }
    public PlayerAudio CurrentAudio { get { return playerAudio; } }


    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerAudio = GetComponent<PlayerAudio>();
        SnapToTile();
        state = State.Ready;
        queuedInteraction = CustomTile.Interaction.None;
        ShowMoveButtons();
    }

    private void Update()
    {
        if (state == State.Moving)
        {
            var move = target - transform.position;
            float dist = speed * Time.deltaTime;
            if (move.sqrMagnitude < dist * dist)
            {
                transform.position = target;
                if (queuedInteraction != CustomTile.Interaction.None)
                {
                    InteractWithTile(queuedInteraction);
                    queuedInteraction = CustomTile.Interaction.None;
                }
                else
                {
                    ShowMoveButtons();
                    state = State.Ready;
                }
            }
            else
            {
                transform.position += move.normalized * dist;
            }
        }
    }

    [ContextMenu("Snap to tile")]
    void SnapToTile()
    {
        tilemapPos = world.GetTile(transform.position);
        transform.position = world.SnapToTile(tilemapPos);
    }

    void TryEnterTile(Vector3Int pos)
    {
        if (state != State.Ready)
            return;
        var ti = world.GetTileInfo(pos);
        if (tilemapPos != pos)
        {
            if (inventory.CanEnterTile(ti))
            {
                tilemapPos = pos;
                target = world.SnapToTile(tilemapPos);
                state = State.Moving;
                HideMoveButtons();
                playerAudio.PlaySteps();
                inventory.energy.value -= ti.energy;
                inventory.food.value -= ti.food;
                queuedInteraction = Random.value < ti.encounterChance ? ti.interaction : CustomTile.Interaction.None;
            }
            else
            {
                playerAudio.PlayError();
            }
        }
        else
        {
            inventory.time--;
            inventory.food.value -= ti.food;
            inventory.energy.Refill();
            InteractWithTile(ti.interaction);
        }
        if (inventory.CheckDead())
        {
            state = State.Waiting;
            HideMoveButtons();
        }
        inventory.UpdateUI();
    }

    void InteractWithTile(CustomTile.Interaction interaction)
    {
        HideMoveButtons();
        state = State.Waiting;
        // TODO: Handle Interaction
        state = State.Ready;
        ShowMoveButtons();
    }

    void ShowMoveButtons()
    {
        var ti = world.GetTileInfo(world.TopOf(tilemapPos));
        moveUpButton.Enable(ti, false, inventory.CanEnterTile(ti), GoUp);
        ti = world.GetTileInfo(world.LeftOf(tilemapPos));
        moveLeftButton.Enable(ti, false, inventory.CanEnterTile(ti), GoLeft);
        ti = world.GetTileInfo(world.RightOf(tilemapPos));
        moveRightButton.Enable(ti, false, inventory.CanEnterTile(ti), GoRight);
        moveStayButton.Enable(world.GetTileInfo(tilemapPos), true, true, Stay);
    }

    void HideMoveButtons()
    {
        moveUpButton.Disable();
        moveLeftButton.Disable();
        moveRightButton.Disable();
        moveStayButton.Disable();
    }


    public void OnUp(InputAction.CallbackContext input)
    {
        if (input.performed)
            GoUp();
    }

    public void GoUp()
    {
        TryEnterTile(world.TopOf(tilemapPos));
    }

    public void OnDown(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            Stay();
        }
    }

    public void Stay()
    {
        TryEnterTile(tilemapPos);
    }

    public void OnLeft(InputAction.CallbackContext input)
    {
        if (input.performed)
            GoLeft();
    }
    public void GoLeft()
    {
        TryEnterTile(world.LeftOf(transform.position));
    }

    public void OnRight(InputAction.CallbackContext input)
    {
        if (input.performed)
            GoRight();
    }

    public void GoRight()
    {
        TryEnterTile(world.RightOf(transform.position));
    }
}
