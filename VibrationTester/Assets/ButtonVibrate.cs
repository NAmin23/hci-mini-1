using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonVibrate : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

    public Button yourButton;
    public AudioSource audioData;
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        //Debug.Log("It's working");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");
        Handheld.Vibrate();
        audioData.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Down.");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " Was Released.");
    }

}
