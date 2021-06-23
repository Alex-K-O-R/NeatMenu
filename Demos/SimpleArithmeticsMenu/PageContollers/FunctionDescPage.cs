using Lib;
using Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.Utilities.Menu.PageContollers
{
    class FunctionDescPage : PageController
    {
        public FunctionDescPage(object subj = null) : base(subj)
        {
        }

        public override Action<int, object> ElementOutputController => null;

        public override void drawBottom()
        {
            if (getOptions<FunctionDescOptions>().InvokeResult!=null) {
                Conn.Line(Conn.GetBottomLineNum() - 3);
                if ((double)getOptions<FunctionDescOptions>().InvokeResult != default)
                    Conn.Line("!gre Result: " + (double)getOptions<FunctionDescOptions>().InvokeResult + "!!");
                    else
                    Conn.Line("!dred Result:  Impossible to execute. Incorrect arguments.!!");
            }
        }

        public override void drawTop()
        {
            var libArgs = getSubj<CommonLibrary>().getArgumentsOfType<float>();
            Conn.Line("Library arguments are: !yel [" + (libArgs != null ? string.Join(", ", libArgs) : "") + "] !!");
            Conn.Line();
            Conn.Line("Method !gre {0}!!.", getOptions<FunctionDescOptions>().methodName);
            Conn.Line();
            Conn.Line(getSubj<CommonLibrary>().service.getFunctionDescByFName(getOptions<FunctionDescOptions>().methodName));
            Conn.Line();
            Conn.Line("!red e.!! Execute method;");
            Conn.Line("!red 0.!! Return to previous menu.");
        }

        public override void InputActionDescriber(CInputter Inputceptor)
        {
            if (Inputceptor.CheckKeyPressed(ConsoleKey.E))
            {
                this.Result =
                        new FunctionDescResult() { methodName = getOptions<FunctionDescOptions>().methodName, Invoke = true };
            }

            if (Inputceptor.CheckKeyPressed(ConsoleKey.D0))
            {
                this.Result =
                        new FunctionDescResult() { Return = true };

            }
        }
    }

    class FunctionDescOptions : PageControllerOptions 
    {
            public double? InvokeResult = null;
            public string methodName;
    }

    class FunctionDescResult : PageControllerOptions
    {
        public bool Return = false;
        public bool Invoke = false;
        public string methodName;
    }
}
