using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class Colors
{
    public static string CommandColor = "#3AB741";
    public static string FolderColor = "#E200FF";
    public static string FileColor = "#E3A10E";
    public static string DownloadColor = "#30BABF";

    public static string Wrap(string input, string color)
    {
        return "<color="+color+">"+input+"</color>";
    }

    public static string ReplaceCollection(string input, IEnumerable<string> collection, string color)
    {
        foreach (var substring in collection)
        {
            input = input.Replace(substring, Colors.Wrap(substring, color), StringComparison.OrdinalIgnoreCase);
        }
        return input;
    }
}
