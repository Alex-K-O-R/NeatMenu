using Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.Utilities.Menu.PageContollers
{
    class ByePage : PageController
    {
        public ByePage(object subj = null) : base(subj)
        {
        }

        public override Action<int, object> ElementOutputController => null;

        public override void drawBottom()
        {
            return ;
        }

        public override void drawTop()
        {
            Conn.Line("Demo's ended. Press any key to close the application.");
        }

        public override void InputActionDescriber(CInputter Inputceptor)
        {
            Inputceptor.cycleContinueMarker = false;
        }
    }


}
