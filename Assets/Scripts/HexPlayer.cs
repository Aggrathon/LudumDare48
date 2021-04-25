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

    public float speed = 3.0f;

    Vector3Int tilemapPos;
    Inventory inventory;
    PlayerAudio playerAudio;

    enum State
    {
        Ready,
        Moving,
        Waiting,
    }

    State state;
    Vector3 target;
    CustomTile.Interaction queuedInteraction;


    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerAudio = GetComponent<PlayerAudio>();
        SnapToTile();
        state = State.Ready;
        queuedInteraction = CustomTile.Interaction.None;
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
                    InteractWithTile(queuedInteraction, false);
                    queuedInteraction = CustomTile.Interaction.None;
                }
                else
                {
                    // TODO: Show move UI
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
            if (inventory.energy.value >= ti.energy && inventory.food.value >= ti.food)
            {
                tilemapPos = pos;
                target = world.SnapToTile(tilemapPos);
                state = State.Moving;
                playerAudio.PlaySteps();
                inventory.energy.value -= ti.energy;
                inventory.food.value -= ti.food;
                queuedInteraction = ti.interaction;
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
            InteractWithTile(ti.interaction, true);
        }
        if (inventory.CheckDead())
            state = State.Waiting;
        inventory.UpdateUI();
    }

    void InteractWithTile(CustomTile.Interaction interaction, bool forced)
    {
        state = State.Waiting;
        state = State.Ready;
        // TODO: Show move UI
    }

    bool CanEnterTile(Vector3Int pos)
    {
        var ti = world.GetTileInfo(pos);
        return inventory.energy.value >= ti.energy && inventory.food.value >= ti.food;
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
