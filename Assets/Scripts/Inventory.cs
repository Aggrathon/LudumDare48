using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [System.Serializable]
    public struct Capacity
    {
        public int value;
        public int max;

        public void Refill()
        {
            value = max;
        }
        public void Refill(int amount)
        {
            value = Mathf.Min(max, value + amount);
        }

        public void Consume(int amount)
        {
            value -= amount;
        }
    }

    public Capacity food;
    public Capacity energy;
    public Capacity health;
    public Capacity gold;
    public Capacity bandage;
    public bool bow;
    public bool axe;
    public bool dagger;


    public bool IsDead()
    {
        return food.value < 0 || health.value <= 0;
    }
}
