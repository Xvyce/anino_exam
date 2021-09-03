using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class Wheel2 : Singleton<Wheel2>
    {
    [System.Serializable]
    public class WeightedValue
    {
        public int Value;
        public float Weight;

        public WeightedValue(int value, float weight)
        {
            Value = value;
            Weight = weight;
        }
    }

    public List<WeightedValue> PricesWithWeights;

    // seconds one complete rotation shall take
    // adjust in the Inspector

    public float SpinDuration = 5;
        public int maxSpin;
        public int minSpin;

        // you can't assign this directly since you want it weighted
        private readonly List<int> _weightedList = new List<int>();

        private bool _spinning;
        private float _anglePerItem;

        public Text winText;

        public Button PlayButton;
        bool StartSpin;

        private void Start()
        {
            _spinning = false;
            _anglePerItem = 360f / PricesWithWeights.Count;

            // first fill the randomResults accordingly to the given wheights
            foreach (var kvp in PricesWithWeights)
            {
                // add kvp.Key to the list kvp.value times
                for (var i = 0; i < kvp.Weight; i++)
                {
                    _weightedList.Add(kvp.Value);
                }
            }
        }


        public void StartSpinButton()
        {
            StartSpin = true;
            PlayButton.interactable = false;
            CoinManager.Instance.coins -= Bet.Instance.multiplier;
            winText.text = "Winnings: 0";
        }

        public int GetRandomNumber()
        {
            // get a random inxed from 0 to 99
            var randomIndex = Random.Range(0, _weightedList.Count);
            // get the according value
            return _weightedList[randomIndex];
        }

        private void Update()
        {
            // spinning is less expensive to check so do it first
            if (_spinning || !StartSpin) return;

            // how often should the wheel spin before coming to hold
            // 1 is quite boring so I would say at least 2 -> always at least one full rotation
            var randomTime = Random.Range(minSpin, maxSpin);
            // What you had
            //itemNumber = Random.Range(0, prize.Count); 
            // returns a random index .. not the actual value at this index
            var itemNumber = GetRandomNumber();

            // find the original index of the selected random value
            // and its angle by multiplying by anglePerItem
            var itemIndex = PricesWithWeights.FindIndex(w => w.Value == itemNumber);
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

            // Now we can compose the actual total target rotation
            // depends on your setup ofcourse .. For my example below I will use it negative (rotation clockwise) like
            var targetAngle = -(itemNumberAngle + 360f * randomTime);

            Debug.Log($"Will spin {randomTime } times before ending at {itemNumber} with an angle of {itemNumberAngle}", this);
            Debug.Log($"The odds for this were {PricesWithWeights[itemIndex].Weight / 100f:P} !");

            // now pass it all on
            if (StartSpin == true)
            {
                StartCoroutine(SpinTheWheel(currentAngle, targetAngle, randomTime * SpinDuration, itemNumber));
                StartSpin = false;
            }
        }


        // spins the wheel from the given fromAngle until the given toAngle within withinSeconds seconds
        // using an eased in and eased out rotation
        private IEnumerator SpinTheWheel(float fromAngle, float toAngle, float withinSeconds, int result)
        {
            _spinning = true;

            var passedTime = 0f;
            while (passedTime < withinSeconds)
            {
                // here you can use any mathematical curve for easing the animation
                // in this case Smoothstep uses a simple ease-in and ease-out
                // so the rotation starts slow, reaches a maximum in the middle and ends slow
                // you could also e.g. use SmoothDamp to start fast and only end slow
                // and you can stack them to amplify their effect
                var lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / withinSeconds)));

                transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(fromAngle, toAngle, lerpFactor));
                passedTime += Time.deltaTime;

                yield return null;
            }

            transform.eulerAngles = new Vector3(0.0f, 0.0f, toAngle);
            _spinning = false;
            PlayButton.interactable = true;
            Debug.Log("Prize: " + result);
            winText.text = "Winnings:" + result * Bet.Instance.multiplier + "Coins";
            CoinManager.Instance.coins += result * Bet.Instance.multiplier;
            Debug.Log("You got a grand total of" + CoinManager.Instance.coins);
        }
    }


