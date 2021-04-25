using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MoveButton : MonoBehaviour
{

    public Sprite timeSprite;
    public Sprite energySprite;
    public Sprite foodSprite;

    Button button;
    System.Action callback;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        callback?.Invoke();
    }

    public void Enable(CustomTile.TileInfo ti, bool rest, bool enabled, System.Action callback)
    {
        this.callback = callback;
        int index = 1;
        Transform layout = transform.GetChild(1);
        if (rest)
        {
            Image img = layout.GetChild(index).GetComponent<Image>();
            img.sprite = timeSprite;
            img.gameObject.SetActive(true);
            index++;
        }
        else
        {
            for (int i = 0; i < ti.energy; i++)
            {
                Image img = layout.GetChild(index).GetComponent<Image>();
                img.sprite = energySprite;
                img.gameObject.SetActive(true);
                index++;
            }
        }
        for (int i = 0; i < ti.food; i++)
        {
            Image img = layout.GetChild(index).GetComponent<Image>();
            img.sprite = foodSprite;
            img.gameObject.SetActive(true);
            index++;
        }
        for (; index < transform.GetChild(1).childCount; index++)
        {
            transform.GetChild(1).GetChild(index).gameObject.SetActive(false);
        }
        button.interactable = enabled;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ti.GetTitle();
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
