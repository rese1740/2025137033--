using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public Text textComponent;
    public string fullText;    
    public float typingSpeed = 0.1f;

    private void Awake()
    {
        if (textComponent != null)
        {
            StartCoroutine(TypeText());
        }
    }

    // 타자기 효과 Coroutine
    IEnumerator TypeText()
    {
        textComponent.text = ""; 

        foreach (char letter in fullText) 
        {
            textComponent.text += letter;  
            yield return new WaitForSeconds(typingSpeed);  
        }
    }
}
