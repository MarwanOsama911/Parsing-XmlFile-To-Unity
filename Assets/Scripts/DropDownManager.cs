using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownManager : MonoBehaviour
{
    public ParseXML parseXML;

    void Start()
    {
        var dropDown = GetComponent<Dropdown>();
        dropDown.onValueChanged.AddListener(delegate { DropDOwnItemSelected(dropDown); });
    }

    private void DropDOwnItemSelected(Dropdown dropDown)
    {
        int index = dropDown.value;
        if (dropDown.options[index].text == "Choose model") { return; }
        parseXML.FindItemWithID(dropDown.options[index].text);
    }

}
