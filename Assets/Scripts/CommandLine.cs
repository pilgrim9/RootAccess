using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CommandLine : MonoBehaviour
{
    private Dictionary<string, string> commands = new Dictionary<string, string>()
    {
        { "enter", nameof(MoveTo) },
        { "back", nameof(Back) },
        { "install", nameof(Install) },
        { "cut", nameof(Cut)},
        { "paste", nameof(Paste)},
        { "paste", nameof(Download)},
    };
    
    // Start is called before the first frame update
    public void InputCommand(string input)
    {
        string[] parameters = input.Split(" ");
        string command = parameters[0];
        
        if (command.Contains(command))
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(command);
            theMethod.Invoke(this, parameters);
        }
    }

    private void MoveTo(string[] parameters)
    {
        
    }
    
    private void Install(string[] parameters)
    {
        
    }
    
    private void Cut(string[] parameters)
    {
        
    }
    private void Paste(string[] parameters)
    {
        
    }
    private void Download(string[] parameters)
    {
        
    }
    private void Back(string[] parameters)
    {
        
    }
}
