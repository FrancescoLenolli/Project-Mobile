using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Formatter
{
    public static string FormatValue(long value)
    {
        string formattedValue = "";

        string convertedValue = value.ToString();

        switch (convertedValue.Length)
        {
            case 1:
                formattedValue = convertedValue != "0" ? string.Format(convertedValue.Substring(0, 1) + " M") : string.Format(convertedValue.Substring(0, 1));
                break;
            case 2:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " M");
                break;
            case 3:
                formattedValue = string.Format(convertedValue.Substring(0, 3) + " M");
                break;
            case 4:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " B");
                break;
            case 5:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " B");
                break;
            case 6:
                formattedValue = string.Format(convertedValue.Substring(0,3) + " B");
                break;
            case 7:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " T");
                break;
            case 8:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " T");
                break;
            case 9:
                formattedValue = string.Format(convertedValue.Substring(0, 3) + " T");
                break;
            case 10:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " Quad.");
                break;
            case 11:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " Quad.");
                break;
            case 12:
                formattedValue = string.Format(convertedValue.Substring(0, 3) + " Quad.");
                break;
            case 13:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " Quint.");
                break;
            case 14:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " Quint.");
                break;
            case 15:
                formattedValue = string.Format(convertedValue.Substring(0, 3) + " Quint.");
                break;
            case 16:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " Sixt.");
                break;
            case 17:
                formattedValue = string.Format(convertedValue.Substring(0, 2) + " Sixt.");
                break;
            case 18:
                formattedValue = string.Format(convertedValue.Substring(0, 3) + " Sixt.");
                break;
            case 19:
                formattedValue = string.Format(convertedValue.Substring(0, 1) + " Sept.");
                break;
            default:
                formattedValue = convertedValue;
                break;
        }

        return formattedValue;
    }
}
