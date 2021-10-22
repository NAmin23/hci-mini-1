using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FullVibrate : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

    //This instance is involved in shake detection
    #region Instance
    private static Accelerometer instance;
    protected bool shake;
    public static Accelerometer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Accelerometer>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned Accelerometer", typeof(Accelerometer)).GetComponent<Accelerometer>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    //This region has enable emoji functions
    #region

    public GameObject angry;
    public GameObject like;
    public GameObject yay;
    public GameObject love;
    public GameObject haha;
    public GameObject sad;

    void enableAngry()
    {
        angry.SetActive(true);
        like.SetActive(false);
        yay.SetActive(false);
        love.SetActive(false);
        sad.SetActive(false);
        haha.SetActive(false);
    }

    void enableLike()
    {
        angry.SetActive(false);
        like.SetActive(true);
        yay.SetActive(false);
        love.SetActive(false);
        sad.SetActive(false);
        haha.SetActive(false);
    }

    void enableYay()
    {
        angry.SetActive(false);
        like.SetActive(false);
        yay.SetActive(true);
        love.SetActive(false);
        sad.SetActive(false);
        haha.SetActive(false);
    }

    void enableLove()
    {
        angry.SetActive(false);
        like.SetActive(false);
        yay.SetActive(false);
        love.SetActive(true);
        sad.SetActive(false);
        haha.SetActive(false);
    }

    void enableSad()
    {
        angry.SetActive(false);
        like.SetActive(false);
        yay.SetActive(false);
        love.SetActive(false);
        sad.SetActive(true);
        haha.SetActive(false);
    }
    void enableHaha()
    {
        angry.SetActive(false);
        like.SetActive(false);
        yay.SetActive(false);
        love.SetActive(false);
        sad.SetActive(false);
        haha.SetActive(true);
    }

    void disableAll()
    {
        angry.SetActive(false);
        like.SetActive(false);
        yay.SetActive(false);
        love.SetActive(false);
        sad.SetActive(false);
        haha.SetActive(false);
    }

    #endregion


    //This region does shake detection
    #region
    [Header("Shake Detection")]
    public Action OnShake;
    [SerializeField] private float shakeDetectionThreshold = 3.0f;
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float lowPassfilterFactor;
    private Vector3 lowPassValue;
    #endregion


    public Button yourButton;
    public AudioSource audioData;
    public AudioSource sendData;
    public AudioSource eraseData;

    private int i = 0;
    protected int pressCount = 0;
    private int finalCount;
    private bool pressed;

    private float timeOfLastPress = 0.0f;
    protected string Information = "";

    void Start()
    {
        disableAll();

        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        DontDestroyOnLoad(this.gameObject);

        lowPassfilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassfilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        // Shake Detection - Sets pressCount to zero
        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            reset();
            eraseData.Play();
            Information = Information + " shake at" + Time.time.ToString();
        }

        switch (pressCount)
        {
            case 0:
                disableAll();
                break;
            case 1:
                enableLike();
                break;
            case 2:
                enableLove();
                break;
            case 3:
                enableHaha();
                break;
            case 4:
                enableYay();
                break;
            case 5:
                enableSad();
                break;
            case 6:
                enableAngry();
                break;
            default:
                enableAngry();
                break;
        }

        if (pressed == true)
        {
            i++;
        }
        else if(pressed == false)
        {
            if(i > 60) //Send message if the users presses for over 2 seconds
            {
                finalCount = pressCount - 1;
                Debug.Log("Final number:" + finalCount);
                Debug.Log(i);
                sendData.Play();
                reset();
            }
            i = 0;
        }
    }

    void incrementCount()
    {
        if(pressCount < 6)
        {
            pressCount++;
        }
    }

    void reset()
    {
        pressCount = 0;
        timeOfLastPress = 0.0f;
    }

    void TaskOnClick()
    {
        //Runs on RELEASE, not on PRESS

        if(Time.time - timeOfLastPress > 5f) //If its been more than 5 seconds, record as accidental press
        {
            reset();
            Information = Information + " reset at " + Time.time.ToString();
        }
        Information = Information + " " + pressCount.ToString();

        Handheld.Vibrate();
        audioData.Play();
        incrementCount();

        timeOfLastPress = Time.time;
        pressed = false;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
}
