using Lib;
using Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.Utilities.Menu.PageContollers
{
    class IndexPage : PageController
    {
        public IndexPage(object subj = null) : base(subj)
        {
        }

        public override Action<int, object> ElementOutputController =>
         (i, x) => {
                Conn.Line(String.Format("!red {0}.!! {1}: {2}.", (1 + (int)i), x, getSubj<CommonLibrary>().service.getFunctionImageByFName((string)x)));
         };

        public override Func<object[]> subjEnumerationGetter =>
            () => getSubj<Lib.CommonLibrary>()?.service.listAvailableFunctions();


        public override void drawBottom()
        {
          /*  Conn.Line();
            Conn.Line(@"Press *a* to input library arguments");*/

            Conn.Line();
            Conn.Line("Press !green a!! button to set arguments");
            Conn.Line();
            Conn.Line("Input !yellow exit!! to exit menu");
        }

        public override void drawTop()
        {
            /*
             * Removed till now due to ugly key modifiers in linux-with-netcore; there are sub-tasks 
             * for netcore improvals within Console Redesign big task
             */
            Conn.Line("Methods list. Use !Red 1, ..., 9!! buttons to select method, !gre Alt!! + [!green +, -!!] to navigate.");
            Conn.Line();
        }

        public override void InputActionDescriber(CInputter Inputceptor)
        {
            if (Inputceptor.CheckKeyPressed(ConsoleKey.A))
            {
                this.Result =
                        new IndexPageResult() { next_step = IndexPageResult.SELECTED_OPTION.ARGS_INPUT };

            }
            if (Inputceptor.GetLastInputCharSymbolResult().Value >= (char) ConsoleKey.D1 
                && Inputceptor.GetLastInputCharSymbolResult().Value <= (char)ConsoleKey.D9)
            {
                var selected_index = Char.GetNumericValue(Inputceptor.GetLastInputCharSymbolResult().Value) - 1;
                var tmp = subjEnumerationGetter.Invoke();
                if (tmp != null && selected_index < tmp.Length && selected_index >-1)
                    this.Result =
                            new IndexPageResult() {
                                next_step = IndexPageResult.SELECTED_OPTION.FUNCTION_PAGE
                                , s_param = tmp.GetValue((int)selected_index).ToString()

                        };
            }

            if(Inputceptor.CheckInputString("exit"))
            {
                this.Result =
                        new IndexPageResult() { next_step = IndexPageResult.SELECTED_OPTION.EXIT };

            }
        }
    }

    class IndexPageOptions : SwitchPageControllerOptions 
    { 
        public IndexPageOptions(int index_start = 0, object subj = null) { }

        public override int max_records_per_page { get => 2; set => ChangeMaxRecordsPerPage(value); }
    }

    class IndexPageResult : PageControllerOptions
    {
        public enum SELECTED_OPTION { FUNCTION_PAGE, ARGS_INPUT, EXIT };

        public SELECTED_OPTION next_step;
        public int i_param;
        public string s_param;
    }
}
