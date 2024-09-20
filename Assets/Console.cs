using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Console : MonoBehaviour
{
    public bool active;
    public TMP_InputField inputField;
    public TextMeshProUGUI history;
    public GameObject panel;
    void Start()
    {
        
    }
    public abstract class IConsoleCommand
    {
        public string[] parameters;
        public string name;
        public abstract void Init(params string[] vs);
        public abstract string On();
        public IConsoleCommand(params string[] vs)
        {
            Init(vs);
        }
    }
    public sealed class SetSpell : IConsoleCommand
    {
        public SetSpell(params string[] vs) : base(vs)
        {
        }

        public override void Init(params string[] vs)
        {
            name = "SetSpell";
            parameters = vs;
        }

        public override string On()
        {
            try
            {
                MagIntegerField.mif.SetText(parameters[0]);
            }
            catch(System.Exception ex)
            {
                return ex.Message+": "+"Не найден первый параметр";
            }
            return "Заклинание изменено.";
        }
    }
    public void ExtractParameter(string command,List<string> cutcoms,ref string sum, ref int i)
    {
        bool read=false;
        
        for (; i < command.Length; i++)
        {
            if (command[i] == '>')
            {
                read = false;
                
            }
            if (read)
            {
                sum += command[i];
            }
            if (command[i] == '<')
            {
                read = true;
            }
        }
    }
    public void CaretRunner(string command,List<string> cutcoms,ref string sum,ref int i)
    {
        
        char s = command[i];
        if (command[i]!= ' '& command[i] != '<')
        {
            sum += s;
        }
        else
        {
            if (command[i] == '<') ExtractParameter(command, cutcoms, ref sum, ref i);
            cutcoms.Add(sum);
            sum = "";
        }
    }
   
    public void SendCommand(string command)
    {
        List<string> cutcoms = new();
        string sum = "";
        for(int i = 0; i < command.Length; i++)
        {
            CaretRunner(command, cutcoms,ref sum,ref i);
        }
        
        IConsoleCommand ca=null;
        switch (cutcoms[0])
        {
            case "SetSpell":
                ca = new SetSpell(cutcoms[1]);               
                break;
        }
        
        history.text+= ca.On()+'\n';

    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if(active)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }
        panel.SetActive(active);

        if(active)
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                SendCommand(inputField.text);

            }
                    
        
    }
}
