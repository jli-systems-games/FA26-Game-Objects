using System;
using UnityEngine;
using TMPro;

public class EnumSwitchCase : MonoBehaviour
{
    public enum TonalMessages
    {
        NeutralMessage,
        HappyMessage,
        TiredMessage,
    }

    public TonalMessages messageTypes;
    public TMP_Text displayMessage;

    private void Start()
    {
        switch (messageTypes)
        {
            case TonalMessages.NeutralMessage:
                displayMessage.text = "This is a neutral message";
                break;
            case TonalMessages.HappyMessage:
                displayMessage.text = "This is a happy message";
                break;
            case TonalMessages.TiredMessage:
                displayMessage.text = "This is a tired message";
                break;
        }
    }

    private void Update()
    {
    }
}
