using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>
{
    public int coins;
    public Text Coins;

    private void Start()
    {
        coins = 100;
        Coins.text = "Coins:" + coins.ToString();
    }
    private void Update()
    {
        Coins.text = "Coins:" + coins.ToString();
    }

}
