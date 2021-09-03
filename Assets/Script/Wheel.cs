using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
    private int randomvalue;
    private float timeInterval;
    private bool coroutineAllowed;
    private int finalAngle;

    [SerializeField]
    private Text winText;


    // Use this for initialization
    private void Start()
    {
        coroutineAllowed = true;

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && coroutineAllowed)
            StartCoroutine(Spin());
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Spin());
        }
    }
    private IEnumerator Spin()
    {
        coroutineAllowed = false;
        randomvalue = Random.Range(20, 30);
        timeInterval = 0.1f;


        for (int i = 0; i < randomvalue; i++)
        {

            transform.Rotate(0, 0, 22.5f);
            if (i > Mathf.RoundToInt(randomvalue * 0.5f))
                timeInterval = 0.2f;
            if (i > Mathf.RoundToInt(randomvalue * 0.85f))
                timeInterval = 0.4f;
            yield return new WaitForSeconds(timeInterval);
        }
        if (Mathf.RoundToInt(transform.eulerAngles.z) % 45 != 0)
            transform.Rotate(0, 0, 22.5f);
        finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);

        switch (finalAngle)
        {
            case 0:
                winText.text = "You win 1";
                break;
            case 45:
                winText.text = "You win 2";
                break;
            case 90:
                winText.text = "You win 3";
                break;
            case 135:
                winText.text = "You win 4";
                break;
            case 180:
                winText.text = "You win 5";
                break;
            case 225:
                winText.text = "You win 6";
                break;
            case 270:
                winText.text = "You win 7";
                break;
            case 315:
                winText.text = "You win 8";
                break;


        }
        coroutineAllowed = true;
    }
}