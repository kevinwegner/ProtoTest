
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Throwable : Item
{
    List<IImpactBehaviour> behaviour = new List<IImpactBehaviour>();
    
    public Throwable()
    {

    }
    public Throwable(string n, string t, IImpactBehaviour b)
    {
        name = n;
        type = t;
        behaviour.Add(b);
    }
    public Throwable(string n, string t, List<IImpactBehaviour> b)
    {
        name = n;
        type = t;
        behaviour = b;
    }

    public void Use()
    {
        GameData data = Application.GetData();

        if (!data.combat)
        {
            return;
        }

        for (int i = 0; i < behaviour.Count; i++)
        {
            behaviour[i].Execute();
        }
    }

    public void AddBehaviour(IImpactBehaviour b)
    {
        behaviour.Add(b);
    }
}