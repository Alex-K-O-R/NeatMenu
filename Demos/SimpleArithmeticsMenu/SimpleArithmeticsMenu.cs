using Modules;
using System;

public class SimpleArithmeticsMenu : CTI.Utilities.Menu.NeatMenu
{
    private object subj;

    public SimpleArithmeticsMenu(object subj)
    {
        this.subj = subj;
    }

    public override void drawHeader()
    {
        /*
         To avoid AmbiguousMatchException, I would rather say
            objectToCheck.GetType().GetMethods().Count(m => m.Name == method) > 0
         */
        var libName = subj.GetType().Module/*.GetProperty("Name")*/;
        if (libName != null)
        {
            Conn.Line("!Yellow " + libName + " !!");
            Conn.Line();
        }
    }
}

