using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryResult
{
    public int Category;
    public bool IsUserContent;
}

public class CategoryCodes : MonoBehaviour
{
    public const int Category_MejoraPersonal = 0;
    public const int Category_MejoraMundo = 1;
    public const int Category_Autoconocimiento = 2;
    public const int Category_Trascendencia = 3;
    public const int Category_Agobios = 4; 

    public static string EncodeCategoryPrefix(int Category, bool IsUserContent)
    {
        string result = "";
        switch(Category)
        {
            case Category_MejoraPersonal:
                result = IsUserContent ? "-1:0:" : "0:0:";
                break;
            case Category_MejoraMundo:
                result = IsUserContent ? "-1:1:" : "0:1:";
                break;
            case Category_Autoconocimiento:
                result = IsUserContent ? "-2:0:" : "1:0:";
                break;
            case Category_Trascendencia:
                result = IsUserContent ? "-3:0:" : "2:0:";
                break;
            case Category_Agobios:
                result = IsUserContent ? "-4:0:" : "3:0:";
                break;
        }
        return result;
    }

    public static CategoryResult DecodeCategoryPrefix(string prefix)
    {
        CategoryResult result = new CategoryResult();
        string[] fields = prefix.Split(':');
        int Field0;
        int Field1;
        int.TryParse(fields[0], out Field0);
        int.TryParse(fields[1], out Field1);
        result.IsUserContent = Field0 < 0;
        int NormalizedCategory = Field0 < 0 ? -(1 + Field0) : Field0;
        switch(NormalizedCategory)
        {
            case 0:
                if(Field1 == 0)
                {
                    result.Category = Category_MejoraPersonal;
                }
                else
                {
                    result.Category = Category_MejoraMundo;
                }
                break;
            case 1:
                result.Category = Category_Autoconocimiento;
                break;
            case 2:
                result.Category = Category_Trascendencia;
                break;
            case 3:
                result.Category = Category_Agobios;
                break;
        }
        return result;
    }
}
