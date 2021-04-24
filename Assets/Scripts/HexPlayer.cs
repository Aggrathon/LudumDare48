using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class HexPlayer : MonoBehaviour
{
    public GridWorld world;

    Vector3Int tilemapPos;


    void Start()
    {
        SnapToTile();
    }

    [ContextMenu("Snap to tile")]
    void SnapToTile()
    {
        tilemapPos = world.GetTile(transform.position);
        transform.position = world.SnapToTile(tilemapPos);
    }

    void TryEnterTile(Vector3Int pos)
    {
        (int energy, int food, var interaction) = world.GetTileCost(pos);
        if (tilemapPos != pos)
        {
            //TODO: Check energy requirements
            tilemapPos = pos;
            transform.position = world.SnapToTile(tilemapPos);
            // TODO: energy and food cost and some interactions
        }
        else
        {
            // TODO: food cost, restore energy and all interactions
        }
    }

    bool CanEnterTile(Vector3Int pos)
    {
        int energy = world.GetTileCost(pos).Item1;
        // TODO: check energy requirements
        return true;
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
