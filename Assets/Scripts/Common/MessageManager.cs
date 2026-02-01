using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        Instance = this;
    }
    public void Messge(string message, int time = 0)
    {
        text.text = message;
        text.enabled = true;
        StopAllCoroutines();
        if(time > 0)
        {
            StartCoroutine(Off(time));
        }
    }
    public void MessageOff()
    {
        text.enabled = false;
    }
    IEnumerator Off(int time)
    {
        yield return new WaitForSeconds(time);
        MessageOff();
    }
}
