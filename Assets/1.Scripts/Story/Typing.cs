using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public Text textComponent; // UI Text ������Ʈ
    public string fullText;    // ��ü �ؽ�Ʈ
    public float typingSpeed = 0.1f;

    private void Awake()
    {
        if (textComponent != null)
        {
            StartCoroutine(TypeText());
        }
    }

    // Ÿ�ڱ� ȿ�� Coroutine
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
