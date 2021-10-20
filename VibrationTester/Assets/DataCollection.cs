using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DataCollection : FullVibrate
{

    //[SerializeField] InputField feedback1;

    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdg8k-bpGPQ3LPOd7M6pYCioKyzJVepYseN6pVydS7aVvn5yw/formResponse";


    public void Send()
    {
        StartCoroutine(Post(pressCount.ToString()));
    }

    IEnumerator Post(string s1)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1637288442", s1);




        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

    }


}