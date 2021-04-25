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
        gameObject.SetActive(false);
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
        button.onClick.AddListener(Hide);
        button.onClick.AddListener(callback);
        var text = tr.GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
        tr.gameObject.SetActive(true);
    }

    public void StartEncounter(HexPlayer player, CustomTile.TileInfo ti)
    {
        ClearOptions(ti.GetTitle());
        switch (ti.interaction)
        {
            case CustomTile.Interaction.None:
                player.GiveControl();
                return;
            case CustomTile.Interaction.Road:
                AddOption("Greet strangers", true, () =>
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            ClearOptions("Strangers");
                            AddOption("They ignore you", true, player.GiveControl);
                            break;
                        case 1:
                            ClearOptions("Strangers");
                            AddOption("You tell them about your adventure and they give you <sprite name=Food><sprite name=Food><sprite name=Food>", true, () =>
                            {
                                player.CurrentInventory.food.Refill(3);
                                player.GiveControl();
                            });
                            break;
                        case 2:
                            ClearOptions("Strangers");
                            AddOption("They offer to trade 5<sprite name=Food> for a <sprite name=Gold>", player.CurrentInventory.gold.value > 0, () =>
                            {
                                player.CurrentInventory.gold.value--;
                                player.CurrentInventory.food.Refill(5);
                                player.GiveControl();
                            });
                            AddOption("Decline", true, player.GiveControl);
                            break;
                        case 3:
                            ClearOptions("Robbers");
                            AddOption("They demand half your <sprite name=Gold>", true, () =>
                            {
                                player.CurrentInventory.gold.value /= 2;
                                player.GiveControl();
                            });
                            AddOption("Try to fight them, loose <sprite name=Health><sprite name=Health>", true, () =>
                            {
                                player.CurrentInventory.health.value -= 2;
                                player.GiveControl();
                            });
                            AddOption("Try to fight them with your <sprite name=Bow>, loose <sprite name=Health>", player.CurrentInventory.bow.value > 0, () =>
                            {
                                player.CurrentInventory.health.value -= 1;
                                player.CurrentInventory.bow.value -= 1;
                                player.GiveControl();
                            });
                            AddOption("Try to fight them with your <sprite name=Dagger>, loose <sprite name=Health>", player.CurrentInventory.dagger.value > 0, () =>
                            {
                                player.CurrentInventory.health.value -= 1;
                                player.CurrentInventory.dagger.value -= 1;
                                player.GiveControl();
                            });
                            AddOption("Try to fight them with your <sprite name=Axe>, loose <sprite name=Health>", player.CurrentInventory.axe.value > 0, () =>
                            {
                                player.CurrentInventory.health.value -= 1;
                                player.CurrentInventory.axe.value -= 1;
                                player.GiveControl();
                            });
                            break;
                    }
                    Show();
                });
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
