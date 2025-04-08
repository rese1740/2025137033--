using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public Text textComponent; // UI Text 컴포넌트
    public string fullText;    // 전체 텍스트
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
