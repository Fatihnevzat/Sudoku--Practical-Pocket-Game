using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Game1 : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject SudokuFieldPanel;
    public GameObject FieldPrefab;
    public GameObject ControllPanel;
    public GameObject ControllPrefab;
    public Button InformationButton;
    public Button BackButton;
    void Start()
    {
        CreateFieldPrefabs();
        CreateControllPrefabs();
        CreateSudokuObject();
    }


    public void ClickOn_Finish()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                FieldPrefabObject fieldObject=
                    _FieldPrefabObjectDic[new Tuple<int, int>(row, column)];

                if (fieldObject.IsChangeAble)
                {
                    if (_finalObject.Values[row, column] == fieldObject.Number)
                    {
                        fieldObject.ChangeColorToGreen();
                    }
                    else
                    {
                        fieldObject.ChangeColorToRed();
                    }
                }
            }
        }
    }
    public void ClickOn_BackButton()
    {
        SceneManager.LoadScene("GameMenu");
    }

    private English_SudokuObject _gameObject;
    private English_SudokuObject _finalObject;
    private void CreateSudokuObject()
    {
        English_SudokuGenerator.CreateSudokuObject(
            out English_SudokuObject finalObject,out English_SudokuObject gameObject);
        _gameObject = gameObject;
        _finalObject = finalObject;

        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var currentValue = _gameObject.Values[row, column];
                if(currentValue != 0)
                {
                    FieldPrefabObject fieldObject = _FieldPrefabObjectDic[new Tuple<int, int>(row, column)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.IsChangeAble = false;
                }
            }
        }
    }

    private bool IsInformationButtonActive=false;
    public void ClickOn_InformationButton()
    {
        Debug.Log($"Click on InformationButton");
        if(IsInformationButtonActive)
        {
            IsInformationButtonActive = false;
            InformationButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        }
        else
        {
            IsInformationButtonActive=true;
            InformationButton.GetComponent<Image>().color = new Color(0.7f, 0.99f, 0.99f);

        }
    }

    private Dictionary<Tuple<int, int>, FieldPrefabObject> _FieldPrefabObjectDic =
        new Dictionary<Tuple<int, int>, FieldPrefabObject>();
    private void CreateFieldPrefabs()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                GameObject instance = GameObject.Instantiate(FieldPrefab, SudokuFieldPanel.transform);

                FieldPrefabObject englishfieldPrefabObject = new FieldPrefabObject(instance, row, column);
                _FieldPrefabObjectDic.Add(new Tuple<int, int>(row, column), englishfieldPrefabObject);

                instance.GetComponent<Button>().onClick.AddListener(() => Onclick_FieldPrefab(englishfieldPrefabObject));
            }
        }
    }
    private void CreateControllPrefabs()
    {
        
        for (int i = 1; i < 10; i++)
        {
            
            GameObject instance = GameObject.Instantiate(ControllPrefab, ControllPanel.transform);
            Debug.Log(instance.name);
           
            instance.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            English_ControllPrefabObject english_ControllPrefabObject = new English_ControllPrefabObject();
            english_ControllPrefabObject.Number=i;
            instance.GetComponent<Button>().onClick.AddListener(() => ClickOn_ControllPrefab(english_ControllPrefabObject));

        }
    }
    private void ClickOn_ControllPrefab(English_ControllPrefabObject english_ControllPrefabObject)
    {
        Debug.Log($"Click on ControllPrefab: {english_ControllPrefabObject.Number}");
        if(_currentHoverendFieldPrefab !=null)
        {
            if(IsInformationButtonActive)
            {
                _currentHoverendFieldPrefab.SetSmallNumber(english_ControllPrefabObject.Number);
            }
            else 
            {         
                _currentHoverendFieldPrefab.SetNumber(english_ControllPrefabObject.Number);
            }
        }
    }

    private FieldPrefabObject _currentHoverendFieldPrefab;
    private void Onclick_FieldPrefab(FieldPrefabObject fieldPrefabObject)
    {
        Debug.Log($"Clicked on Prefab Row {fieldPrefabObject.Row},Column:{fieldPrefabObject.Column}");
        if (fieldPrefabObject.IsChangeAble)
        {
            if (_currentHoverendFieldPrefab != null)
            {
                _currentHoverendFieldPrefab.UnsetHoverMode();
            }
            _currentHoverendFieldPrefab = fieldPrefabObject;
            fieldPrefabObject.SetHoverMode();
        }
    }
}
