using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FieldPrefabObject
{
    private int _row;
    private int _column;
    private GameObject _instance;


    public FieldPrefabObject(GameObject instance, int row,int column)
    {
        _instance = instance;
        Row = row;
        Column = column;
    }
    public bool IsChangeAble = true;

    public void ChangeColorToGreen()
    {
        _instance.GetComponent<UnityEngine.UI.Image>().color = Color.green;
    }
    public void ChangeColorToRed() 
    {
        _instance.GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
    public bool TryGetTextByName(string name, out TextMeshProUGUI text)
    {
        text = null;
        TextMeshProUGUI[] texts = _instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var currentText in texts)
        {
            if (currentText.name.Equals (name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }

    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }

    public void SetHoverMode()
    {

        _instance.GetComponent<UnityEngine.UI.Image>().color = new Color(0.7f,0.99f,0.99f);
    }
    public void UnsetHoverMode()
    {
        _instance.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f);
    }

    public int Number;
    public void SetNumber(int number)
    {
        Number = number;
        if (TryGetTextByName("Value", out TextMeshProUGUI text))
        {
            text.text=number.ToString();
            for (int i = 1; i < 10; i++)
            {
                if (TryGetTextByName($"Number_{i}", out TextMeshProUGUI textNumber))
                {
                    textNumber.text = "";
                }
            }
        }
    }
    public void SetSmallNumber(int number)
    {
        if(TryGetTextByName($"Number_{number}",out TextMeshProUGUI text))
        {
            text.text = number.ToString();
            if (TryGetTextByName("Value", out TextMeshProUGUI textValue))
            {
                textValue.text = "";
            }
        }
    }
}
