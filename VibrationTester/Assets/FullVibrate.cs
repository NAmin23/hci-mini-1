using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FullVibrate : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

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

    [Header("Shake Detection")]
    public Action OnShake;
    [SerializeField] private float shakeDetectionThreshold = 2.0f;
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float lowPassfilterFactor;
    private Vector3 lowPassValue;

    public Button yourButton;
    public AudioSource audioData;
    public AudioSource sendData;
    public AudioSource eraseData;

    private int i = 0;
    protected int pressCount = 0;
    private int finalCount;
    private bool pressed;

    void Start()
    {
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
            pressCount = 0;
            eraseData.Play();
        }

        if (pressed == true)
        {
            i++;
        }
        else if(pressed == false)
        {
            if(i > 120) //Send message if the users presses for over 2 seconds
            {
                finalCount = pressCount - 1;
                Debug.Log("Final number:" + finalCount);
                Debug.Log(i);
                sendData.Play();
            }
            i = 0;
        }
    }

    void TaskOnClick()
    {
        //Runs on RELEASE, not on PRESS
        Handheld.Vibrate();

        audioData.Play();

        pressCount++;
        Debug.Log(pressCount);
        pressed = false;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        Debug.Log(pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " Was Released.");
    }
    
}
