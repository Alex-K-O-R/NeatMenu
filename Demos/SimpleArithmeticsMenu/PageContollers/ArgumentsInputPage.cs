using Modules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static CTI.Utilities.Menu.PageContollers.ArgumentsInputPageResult;

namespace CTI.Utilities.Menu.PageContollers
{
    class ArgumentsInputPage : PageController
    {
        public ArgumentsInputPage(object subj = null) : base(subj)
        {
        }

        public override Action<int, object> ElementOutputController => null;

        public override void drawBottom()
        {
        }

        public override void drawTop()
        {
            Conn.Line("Library arguments input.");
            Conn.Line();
            if (this.getOptions<ArgumentsInputPageOptions>().inputResult != INPUT_RESULT.ERROR) { 
                Conn.Line("Current library arguments are [" +"!yel "+ getOptions<ArgumentsInputPageOptions>().inputString + " !!]");
            } else
            {
                Conn.Line("!dred INPUT ERROR. Incorrect string: " + getOptions<ArgumentsInputPageOptions>().inputString+ " !!");
            }
            Conn.Line();
            Conn.Line("Input float numbers with !red ,!! separator and press Enter to save.");
            Conn.Line("Press !red r!! to return to previous menu.");
        }

        public override void InputActionDescriber(CInputter Inputceptor)
        {
            if (Inputceptor.CheckKeyPressed(ConsoleKey.R)) {
                this.Result = new ArgumentsInputPageResult() { inputResult = INPUT_RESULT.OK, inputArguments = null };
            }

            if (Inputceptor.GetLastInputStringResult()!=null)
            {
                var RegEx = new Regex(@"(\d+[.]?[\d]*)+");
                var mR = RegEx.Matches(Inputceptor.GetLastInputStringResult());
                List<float> result = new List<float>();
                Match match;
                for (int i = 0; i < mR.Count; i++)
                {
                    match = mR[i];
                    float tmp;
                    if (float.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out tmp)) result.Add(tmp);
                }

                if (result.Count > 0) { 
                    this.Result =
                        new ArgumentsInputPageResult() { inputResult = INPUT_RESULT.OK, inputArguments = result };
                } else
                    this.Result =
                        new ArgumentsInputPageResult() { inputResult = INPUT_RESULT.ERROR, inputString = Inputceptor.GetLastInputStringResult() };
            }
        }
    }

    class ArgumentsInputPageOptions : PageControllerOptions 
    {
        public INPUT_RESULT? inputResult = default;
        public string inputString;
    }

    class ArgumentsInputPageResult : PageControllerOptions
    {
        public enum INPUT_RESULT { OK, ERROR };
        public INPUT_RESULT? inputResult;
        public List<float> inputArguments;
        public string inputString;
    }
}
