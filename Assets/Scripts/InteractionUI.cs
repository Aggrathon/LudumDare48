using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Transform options;

    int index;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ClearOptions(string title)
    {
        for (int i = 0; i < options.childCount; i++)
        {
            options.GetChild(i).gameObject.SetActive(false);
        }
        this.title.text = title;
        index = 0;
    }

    public void AddOption(string description, bool enabled, UnityAction callback)
    {
        Transform tr;
        if (index == options.childCount)
        {
            tr = Instantiate<Transform>(options.GetChild(0), options.position, options.rotation, options);
        }
        else
        {
            tr = options.GetChild(index);
        }
        index++;
        var button = tr.GetComponent<Button>();
        button.interactable = enabled;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(callback);
        button.onClick.AddListener(Hide);
        var text = tr.GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
        tr.gameObject.SetActive(true);
    }

    public void StartEncounter(HexPlayer player, CustomTile.TileInfo ti)
    {
        Hide();
        ClearOptions(ti.GetTitle());
        switch (ti.interaction)
        {
            case CustomTile.Interaction.None:
                player.GiveControl();
                return;
            case CustomTile.Interaction.Road:
                AddOption("Keep to yourself", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Fields:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Forest:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.OldGrowth:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Hills:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Rocks:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Mountain:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Snow:
                AddOption("Continue Exploring", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Settlement:
                AddOption("Leave", true, player.GiveControl);
                break;
            case CustomTile.Interaction.LoggingCamp:
                AddOption("Leave", true, player.GiveControl);
                break;
            case CustomTile.Interaction.ElfVillage:
                AddOption("Leave", true, player.GiveControl);
                break;
            case CustomTile.Interaction.DwarfMine:
                AddOption("Leave", true, player.GiveControl);
                break;
            case CustomTile.Interaction.Flee:
                player.CurrentInventory.outro.Show("<b>Game Over:</b>\n\nYou decided not explore, and instead just ran away!");
                return;
            default:
                Debug.LogException(new System.Exception("Missing switch statement"));
                return;
        }
        Show();
    }
}
