using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenState : MonoBehaviour
{
    [SerializeField] private InputField m_NameInputField;
    [SerializeField] private Button m_NextButton;

    private void OnDisable()
    {
        m_NameInputField.onValueChanged.RemoveListener(OnNameInputFieldValueChange);
    }

    private void OnEnable()
    {
        m_NextButton.interactable = false;
        m_NameInputField.onValueChanged.AddListener(OnNameInputFieldValueChange);
    }

    private void OnNameInputFieldValueChange(string value)
    {
        if (value.Length >= 3)
        {
            m_NextButton.interactable = true;
        }
        else
        {
            m_NextButton.interactable = false;
        }
    }

    private void Update()
    {
    }
}
