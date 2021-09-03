using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bet : Singleton<Bet>
{

    public int[] BetValue = { 1, 2, 5, 10 };

    int i=0;
    public Text BetNumber;

    public int multiplier;

    private void Start()
    {

        BetNumber.text = BetValue[0].ToString();
        multiplier = BetValue[0];
    }

    public void plusButton()
    {
        AudioManager.Instance.Play("plus");
        if (i >= BetValue.Length-1)
        {
            i = 0;
            BetNumber.text = BetValue[i].ToString();

        }
        else
        {
            i++;
            BetNumber.text = BetValue[i].ToString();
        }

        multiplier = BetValue[i];

    }

    public void minusButton()
    {
        AudioManager.Instance.Play("minus");
        if (i < 1)
        {
            i = BetValue.Length - 1;
            BetNumber.text = BetValue[i].ToString();
        }
        else
        {
            i--;
            BetNumber.text = BetValue[i].ToString();
        }
        multiplier = BetValue[i];

    }

}
