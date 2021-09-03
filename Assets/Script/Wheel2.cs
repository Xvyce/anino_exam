using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wheel2 : Singleton<Wheel2>
{
    [System.Serializable]
    public class WeightedValue
    {
        public int Prize; // The Reward Multiplier
        public float Weight; // The Chance of getting that multiplier

        public WeightedValue(int value, float weight)
        {
            Prize = value;
            Weight = weight;
        }
    }

    public List<WeightedValue> PricesWithWeights;

    //Adjustable in inspector
    public float SpinDuration = 5;
    public int maxSpin;
    public int minSpin;

    private readonly List<int> _weightedList = new List<int>();

    //Wheel
    private bool _spinning;
    private float _anglePerItem;

    //UI
    public Text winText;
    public Button PlayButton;
    bool StartSpin;

    private void Start()
    {
        AudioManager.Instance.Play("bgm");
        _spinning = false;
        _anglePerItem = 360f / PricesWithWeights.Count;

        // first fill the randomResults accordingly to the given wheights
        foreach (var kvp in PricesWithWeights)
        {
            // add kvp.Key to the list kvp.value times
            for (var i = 0; i < kvp.Weight; i++)
            {
                _weightedList.Add(kvp.Prize);
            }
        }
    }


    public void StartSpinButton()
    {
        AudioManager.Instance.Play("play");
        StartSpin = true;
        PlayButton.interactable = false;
        CoinManager.Instance.coins -= Bet.Instance.multiplier;
        winText.text = "Winnings: 0";
    }

    public int GetRandomNumber()
    {
        // get a random inxed from 0 to 99
        var randomIndex = Random.Range(0, _weightedList.Count);
        return _weightedList[randomIndex];
    }

    private void Update()
    {
        if (_spinning || !StartSpin) return;

        // how often should the wheel spin before coming to hold
        var randomTime = Random.Range(minSpin, maxSpin);
        var itemNumber = GetRandomNumber();

        var itemIndex = PricesWithWeights.FindIndex(w => w.Prize == itemNumber);
        var itemNumberAngle = itemIndex * _anglePerItem;
        var currentAngle = transform.eulerAngles.z;
        // reset/clamp currentAngle to a value 0-360 since itemNumberAngle will be in this range
        while (currentAngle >= 360)
        {
            currentAngle -= 360;
        }
        while (currentAngle < 0)
        {
            currentAngle += 360;
        }

        //Rotation
        var targetAngle = -(itemNumberAngle + 360f * randomTime);

        Debug.Log($"Will spin {randomTime } times before ending at {itemNumber} with an angle of {itemNumberAngle}", this);
        Debug.Log($"The odds for this were {PricesWithWeights[itemIndex].Weight / 100f:P} !");

        //Start the Wheel
        if (StartSpin == true)
        {
            AudioManager.Instance.StopPlaying("bgm");
            AudioManager.Instance.Play("wheelBgm");
            StartCoroutine(SpinTheWheel(currentAngle, targetAngle, randomTime * SpinDuration, itemNumber));
            StartSpin = false;
        }
    }

    // spins the wheel from the given fromAngle until the given toAngle within withinSeconds seconds
    private IEnumerator SpinTheWheel(float fromAngle, float toAngle, float withinSeconds, int result)
    {
        _spinning = true;

        var passedTime = 0f;
        while (passedTime < withinSeconds)
        {
            //has an ease in and ease out effect
            var lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / withinSeconds)));

            transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(fromAngle, toAngle, lerpFactor));
            passedTime += Time.deltaTime;

            yield return null;
        }
        AudioManager.Instance.StopPlaying("wheelBgm");
        AudioManager.Instance.Play("win");
        
        transform.eulerAngles = new Vector3(0.0f, 0.0f, toAngle);
        _spinning = false;

        PlayButton.interactable = true;
        Debug.Log("Prize: " + result);
        //Rewards to be displayed
        winText.text = "Winnings:" + result * Bet.Instance.multiplier + "Coins";
        CoinManager.Instance.coins += result * Bet.Instance.multiplier;
        Debug.Log("You got a grand total of" + CoinManager.Instance.coins);
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.Play("bgm");
    }
}


