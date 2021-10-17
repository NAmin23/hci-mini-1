using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    // Start is called before the first frame update
    void Vibrat()

    {
        Handheld.Vibrate();
        Debug.Log("Other Vibrate script");
    }
}
