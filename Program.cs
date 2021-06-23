using CTI.Utilities.Menu;
using CTI.Utilities.Menu.PageContollers;

/*
  Made by Alex K.
     */
namespace Main
{
    class Program
	{
        /**
         * This is a demo of the console Menu class (look for description in ./BasicImages/Menu.cs).
         * SimpleArithmeticsMenu is based on Menu class.
         * 
         * TODO: make a friendship with linux when netcore great Console Redesign will be 90% finished.
         */

		public static void Main(string[] args)
		{
			// SimpleArithmetics module Demo
			var lib1 = new SimpleArithmetics.Main();

            var Menu = new SimpleArithmeticsMenu(lib1);

            // Page: default
            PageControllerOptions MenuActionResult = Menu.Draw(
                new IndexPage(lib1).SetOptions(
                    new IndexPageOptions(0)
                )
            );

            while (Menu.Continue())
            {
                // Page: from main
                if (MenuActionResult?.getSubj<IndexPageResult>() != null)
                {
                    switch (MenuActionResult.getSubj<IndexPageResult>().next_step)
                    {
                        case IndexPageResult.SELECTED_OPTION.EXIT:
                            MenuActionResult = Menu.Draw(
                                new ByePage()
                            );
                            Menu.breakDrawCycle();
                        break;

                        case IndexPageResult.SELECTED_OPTION.ARGS_INPUT:
                            var libArgs = lib1.getArgumentsOfType<float>();
                            MenuActionResult = Menu.Draw(
                                new ArgumentsInputPage().SetOptions(
                                        new ArgumentsInputPageOptions()
                                        {
                                            inputString = (libArgs != null ? string.Join(", ", libArgs) : "")
                                        }
                                    )
                            );
                        break;
                        
                        case IndexPageResult.SELECTED_OPTION.FUNCTION_PAGE:
                            if (MenuActionResult.getSubj<IndexPageResult>().s_param != null)
                            {
                                MenuActionResult = Menu.Draw(
                                new FunctionDescPage(lib1).SetOptions(
                                        new FunctionDescOptions()
                                        {
                                            methodName = MenuActionResult.getSubj<IndexPageResult>().s_param
                                        }
                                    )
                                );
                            }
                        break;
                    }
                }

                // Page: from input arguments
                if (MenuActionResult?.getSubj<ArgumentsInputPageResult>() != null)
                {
                    switch (MenuActionResult.getSubj<ArgumentsInputPageResult>().inputResult)
                    {
                        case ArgumentsInputPageResult.INPUT_RESULT.OK:
                            if (MenuActionResult.getSubj<ArgumentsInputPageResult>()?.inputArguments?.Count > 0) {
                                lib1.setArguments(MenuActionResult.getSubj<ArgumentsInputPageResult>().inputArguments.ToArray());
                            }
                            MenuActionResult = Menu.Draw(
                                new IndexPage(lib1).SetOptions(
                                    new IndexPageOptions(0)
                                )
                            );
                            break;

                        case ArgumentsInputPageResult.INPUT_RESULT.ERROR:
                            MenuActionResult = Menu.Draw(
                                new ArgumentsInputPage().SetOptions(
                                        new ArgumentsInputPageOptions() { 
                                            inputString = MenuActionResult.getSubj<ArgumentsInputPageResult>()?.inputString
                                            , inputResult = MenuActionResult.getSubj<ArgumentsInputPageResult>()?.inputResult
                                        }
                                    )
                            );
                            break;
                    }
                }


                if (MenuActionResult?.getSubj<FunctionDescResult>() != null)
                {
                    if (MenuActionResult.getSubj<FunctionDescResult>().Return)
                        MenuActionResult = Menu.Draw(
                            new IndexPage(lib1).SetOptions(
                                new IndexPageOptions(0)
                            )
                        );
                    else
                        if (MenuActionResult.getSubj<FunctionDescResult>().Invoke) { 
                            double result;
                            if (lib1.getArgumentsOfType<float>() != null)
                            result = lib1.InvokeFunctionByName<double>(MenuActionResult.getSubj<FunctionDescResult>().methodName);
                            else result = default;
                            
                            MenuActionResult = Menu.Draw(
                                    new FunctionDescPage(lib1).SetOptions(
                                    new FunctionDescOptions()
                                    {
                                        methodName = MenuActionResult.getSubj<FunctionDescResult>().methodName
                                        , InvokeResult = result
                                    }
                                )
                            );
                        }
                }
            }
            return;
		}
	}
}
