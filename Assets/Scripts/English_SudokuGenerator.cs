using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class English_SudokuGenerator
{
    public static void CreateSudokuObject(out English_SudokuObject finalObject,out English_SudokuObject gameObject)
    {
        _finalSudokuObject = null;
        English_SudokuObject english_SudokuObject =new English_SudokuObject();
        CreateRandomGroups(english_SudokuObject);
        if (TryToSolve(english_SudokuObject))
        {
            english_SudokuObject = _finalSudokuObject;
        }
        else
        {
            throw new System.Exception("Bir þeyler ters gitti");
        }
        finalObject= english_SudokuObject;
        gameObject=RemoveSomeRandomNumbers(english_SudokuObject);
    }

    private static English_SudokuObject RemoveSomeRandomNumbers(English_SudokuObject english_SudokuObject)
    {
        English_SudokuObject newSudokuObject = new English_SudokuObject();
        newSudokuObject.Values = (int[,])english_SudokuObject.Values.Clone();
        List<Tuple<int, int>> values = GetValues();
        int EndValueIndex = 10;
        if (EnglishGameSettings.EasyMiddleHard_Number == 1) { EndValueIndex = 60; }
        if (EnglishGameSettings.EasyMiddleHard_Number == 2) { EndValueIndex = 45; }
        if (EnglishGameSettings.EasyMiddleHard_Number == 3) { EndValueIndex = 30; }
        bool isFinish=false;
        while(!isFinish)
        {
            int index = UnityEngine.Random.Range(0, values.Count);
            var searchedIndex = values[index];
            English_SudokuObject nextSudokuObject = new English_SudokuObject();
            nextSudokuObject.Values = (int[,])newSudokuObject.Values.Clone();
            nextSudokuObject.Values[searchedIndex.Item1,searchedIndex.Item2] = 0;

            if (TryToSolve(nextSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }
            values.RemoveAt(index);

            if (values.Count < EndValueIndex)
            {
                isFinish = true;
            }
        }
        return newSudokuObject;
    }
    private static List<Tuple<int, int>> GetValues()
    {
        List<Tuple<int, int>> values = new List <Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                values.Add(new Tuple<int, int>(i, j));
            }
        }
        return values;
    }

    private static English_SudokuObject _finalSudokuObject;
    private static bool TryToSolve(English_SudokuObject english_SudokuObject, bool OnlyOne=false) 
    {
        //find empty field which can be filled
        if(HasEmptyFieldsToFill(english_SudokuObject,out int row,out int column, OnlyOne= false))
        {
            List<int> possibleValues = GetPossibleValues(english_SudokuObject, row, column);
            foreach(var possibleValue in possibleValues)
            {
                English_SudokuObject nextSudokuObject =new English_SudokuObject();
                nextSudokuObject.Values = (int[,]) english_SudokuObject.Values.Clone();
                nextSudokuObject.Values[row,column]=possibleValue;
                if (TryToSolve(nextSudokuObject,OnlyOne))
                {
                    return true;
                }
            }
        }

        //Has sudokuobject empty fields
        if (HasEmptyFields(english_SudokuObject))
        {
            return false;
        }
        _finalSudokuObject = english_SudokuObject; 
        return true;
        //finish
    }
    private static bool HasEmptyFields(English_SudokuObject english_SudokuObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (english_SudokuObject.Values[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private static List<int> GetPossibleValues(English_SudokuObject english_SudokuObject, int row, int column)
    {
        List<int> possibleValues = new List<int>();
        for (int value = 1;  value < 10;  value ++)
        {
            if(english_SudokuObject.IsPossibleNumberInPosition(value,row,column))
            {
                possibleValues.Add(value);
            }
        }
        return possibleValues;
    }
    private static bool HasEmptyFieldsToFill(
        English_SudokuObject english_SudokuObject, out int row, out int column, bool OnlyOne = false)
    {
        row=0; column = 0;
        int amountOfPossibleValues = 10;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (english_SudokuObject.Values[i,j]==0)
                {
                    int currentAmount = GetPossibleAmountOfValues(english_SudokuObject, i, j);
                    if (currentAmount!=0)
                    {
                        if (currentAmount < amountOfPossibleValues)
                        {
                            amountOfPossibleValues = currentAmount;
                            row = i;
                            column = j;
                        }
                    }
                }
            }
        }
        if(OnlyOne)
        {
            if (amountOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (amountOfPossibleValues == 10)
        {
            return false;
        }
        return true;
    }
    private static int GetPossibleAmountOfValues(English_SudokuObject english_SudokuObject, int row, int column)
    {
        int amount = 0;
        for (int value = 1; value < 10; value++)
        {
            if (english_SudokuObject.IsPossibleNumberInPosition(value, row, column))
            {
                amount++;
            }
        }
        return amount;
    }
    public static void CreateRandomGroups(English_SudokuObject english_SudokuObject)
    {
        List<int> Values = new List<int>() { 0, 1, 2 };
        int index=UnityEngine.Random.Range(0,Values.Count);
        InsertRandomGoup(english_SudokuObject,1+Values[index]); 
        Values.RemoveAt(index);

        index=UnityEngine.Random.Range(0,Values.Count);
        InsertRandomGoup(english_SudokuObject,4+Values[index]);
        Values.RemoveAt(index);

        index=UnityEngine.Random.Range(0,Values.Count);
        InsertRandomGoup(english_SudokuObject,7+Values[index]);
    }
    public static void InsertRandomGoup(English_SudokuObject english_SudokuObject,int group)
    {
        english_SudokuObject.GetGroupIndex(group, out int startRow, out int startColumn);
        List<int> Values = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9 };  
        for (int row = startRow; row < startRow+3; row++)
        {
            for (int column = startColumn; column < startColumn + 3; column++) 
            {
                int index =UnityEngine.Random.Range(0, Values.Count);
                english_SudokuObject.Values[row,column]=Values[index];
                Values.RemoveAt(index);
            }
        }
    }
}
