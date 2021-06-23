using System;
using Modules;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

/*
  Made by Alex K.
     */
namespace CTI.Utilities.Menu
{
    /**
     * NeatMenu class is an interactive suit for some object that is *subj*.
     * 
     * NeatMenu uses PageController siblings to operate and has an integrated Paging ability for enumerables.
     * The default button combinations to use a page control are Alt + [-,+].
     * 
     * NeatMenu has an inbound input interceptor that allows to operate via keyboard :-) AND provides a delayed controller 
     * input conversion which is a cool thing (IMHO).
     * 
     * Only one thing to remember: STORE AND REUSE PAGE CONTROLLERS IN VARIABLES carefully, it's easy to forget something.
     */
    public class NeatMenu
    {
        private bool cycleContinueMarker = true;
        public virtual ConsoleModifiers defaultPageNavigationButtonAndCMDModifier { get { return ConsoleModifiers.Alt; } }
        public virtual ConsoleKey defaultGoFullscreenButton { get { return ConsoleKey.F; } }
        private CInputter Inputceptor;

        private byte refreshSleep = 175;

        // Fullscreen part
        private bool FullscreennIsOn = false;
        private Dictionary<char, int> LastConsoleSize = Conn.GetCurrentConsoleSize();

        public NeatMenu()
        {
            Inputceptor = new CInputter();
            Inputceptor.Clear();
            Inputceptor.startGet();
            Console.CursorVisible = false;
        }

        public class SwitchPageResult : PageControllerOptions
        {
            /*            protected int? subj;*/

            public SwitchPageResult(int? subj) { }
        }
        private string IsMyBirthDay = "[09011990]";
        public PageControllerOptions Draw(PageController pController = null)
        {
            if (Inputceptor.CheckKeyPressed(defaultGoFullscreenButton, defaultPageNavigationButtonAndCMDModifier))
            {
                if (FullscreennIsOn)
                {
                    Conn.SetConsoleSize(LastConsoleSize); FullscreennIsOn = false;
                }
                else
                {
                    LastConsoleSize = Conn.GetCurrentConsoleSize();
                    Conn.SwitchFullScreen(); FullscreennIsOn = true;
                }
            }

            Inputceptor.Clear();
            while (Inputceptor.cycleContinueMarker && !pController.HasResult())
            {
                Conn.Clear();
                this.drawHeader();
                if (pController != null)
                {
                    pController.drawTop();
                    if (pController.subjEnumerationGetter != null)
                    {
                        var meanBodyParts = pController.subjEnumerationGetter.Invoke();
                        if (meanBodyParts != null)
                        {
                            var Opts = pController.getOptions<SwitchPageControllerOptions>();
                            if (Opts != null)
                            {
                                if (Inputceptor.CheckKeyPressed(ConsoleKey.OemMinus, defaultPageNavigationButtonAndCMDModifier) && Opts.index_start >= Opts.max_records_per_page)
                                { Opts.index_start = (int)(Opts.index_start - Opts.max_records_per_page); }


                                if (Inputceptor.CheckKeyPressed(ConsoleKey.OemPlus, defaultPageNavigationButtonAndCMDModifier) && (meanBodyParts.Length / ((Math.Floor((double)Opts.index_start / Opts.max_records_per_page) + 1) * Opts.max_records_per_page) > 1))
                                { Opts.index_start = (int)(Opts.index_start + Opts.max_records_per_page); }

                                for (var i = 0; i < Opts.max_records_per_page; i++)
                                {
                                    if (Opts.index_start + i > meanBodyParts.Length - 1) break;
                                    pController.ElementOutputController(Opts.index_start + i, meanBodyParts.GetValue(Opts.index_start + i));
                                }

                                NeatMenu.useNavigationBlock(Opts.max_records_per_page, Opts.index_start, meanBodyParts.Length);
                            }
                            else
                            {
                                var i = 0;
                                foreach (var elem in meanBodyParts) pController.ElementOutputController(i, meanBodyParts.GetValue(i++));
                            }
                        }
                        else Conn.Line("!yel No records found.!!");
                    }
                    pController.drawBottom();
                }
                else
                {
                    Conn.Line("Connected.");
                }

                Conn.Line(Conn.GetBottomLineNum());
                //Conn.Line(String.Format("!gre input is:!! {0}, ({1})", Inputceptor.last_input_string, Inputceptor.GetLastInputStringResult()));
                Conn.Line(String.Format("!gre input is:!! {0}", Inputceptor.GetCurrentInputBuffer()));

                /*                if (Inputceptor.CheckKeyPressed(ConsoleKey.OemPlus))
                                {
                                    Console.WriteLine("You pressed PLUS");
                                }

                                if (Inputceptor.CheckKeyPressed(ConsoleKey.OemMinus))
                                {
                                    Console.WriteLine("You pressed minus");
                                }

                                if (Inputceptor.CheckKeyPressed(ConsoleKey.A))
                                {
                                    Console.WriteLine("You pressed AAAA!!");
                                }*/
                while (!Inputceptor.refreshRequiredOnce) Thread.Sleep(refreshSleep);
                if (pController != null)
                {
                    pController.InputActionDescriber(Inputceptor);
                    if (pController.HasResult()) return pController.getResult();
                }
                else this.Draw(pController);
            }

            return null;
        }

        /** TODO: subject name output */
        public virtual void drawHeader()
        {
        }


        public virtual void drawFooterMain()
        {
        }



        public void breakDrawCycle()
        {
            this.cycleContinueMarker = false;
            Inputceptor.cycleContinueMarker = false;
        }

        public bool Continue()
        {
            return this.cycleContinueMarker;
        }


        /**
         * //left direction result - false, right direction result - true
         * null - something is chosen
         */
        public static int? useNavigationBlock(int max_records_per_page = 10, int index_start = 1, int count = 100)
        {
            Conn.Line();
            if (count != 0 && count >= max_records_per_page)
            {
                if (count / ((Math.Floor((double)index_start / max_records_per_page) + 1) * max_records_per_page) > 1) Conn.Line("!gre +!!. Next page");
                if (index_start >= max_records_per_page) Conn.Line("!gre -!!. Previous page");
                Conn.Line("");
                Conn.Line("Page " + (Math.Floor((double)index_start / max_records_per_page) + 1).ToString() +
                          "/" + Math.Ceiling((double)count / max_records_per_page).ToString());
            }
            //else Conn.Line("1 page total.");
            return null;
        }
    }


    public abstract class PageController
    {
        protected object subj;
        public PageController(object subj = null)
        {
            this.subj = subj;
        }
        public T getSubj<T>()
        {
            try
            {
                return (T)subj;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        private PageControllerOptions Options;
        private PageControllerOptions _Result = null;
        public PageControllerOptions Result { get { return _Result; } set { this._Result = value; hasResult = true; } }
        private bool hasResult = false;

        // Method that draws a block for a single element of Collection
        public abstract Action<int, object> ElementOutputController { get; }

        // Method that is used to get a Collection from Subj (some Object for menu to work with)
        public virtual Func<object[]> subjEnumerationGetter { get { return null; } }


        /* unused due to c# limitations for class and struct
        * public Func<T[]> GetSubjectEnum<T>(*//*Func<object[]> subjEnumerationGetter = null*//*) where T : new()
        {
            return subjEnumerationGetter as Func<T[]>;
        }*/

        public PageControllerOptions getResult()
        {
            return Result;
        }

        public bool HasResult()
        {
            return this.hasResult;
        }

        public bool IsOverride()
        {
            return base.GetType().GetMethod("subjEnumerationGetter").GetBaseDefinition().DeclaringType != this.GetType().GetMethod("subjEnumerationGetter").GetBaseDefinition().DeclaringType.DeclaringType;
        }

        public T getOptions<T>() where T : class
        {
            return Options.getSubj<T>();
        }

        public PageController SetOptions(PageControllerOptions options)
        {
            this.Options = options;
            return this;
        }


        // Body of action describer - main part of PageController when you're up to interact with human input
        // It's executed after first Draw() cycle, right before refresh
        public abstract void InputActionDescriber(CInputter Inputceptor);

        // Method is used as basic to draw a section of Menu or to output something before Collection
        public abstract void drawTop();
        // Method is used to output something after Collection
        public abstract void drawBottom();
    }

    public class CInputter
    {
        private ConsoleKeyInfo? last_input_result;
        private string last_input_string = null;
        public bool cycleContinueMarker = true;
        private bool refreshRequired = false;
        private string inputIsFinishedOrEnterWasPressed = null;

        public bool refreshRequiredOnce
        {
            get
            {
                if (refreshRequired) { refreshRequired = false; return true; }
                return refreshRequired;
            }
        }

        public void Clear()
        {
            this.last_input_string = null;
            this.refreshRequired = false;
            this.last_input_result = null;
            this.inputIsFinishedOrEnterWasPressed = null;
            this.cycleContinueMarker = true;
        }

        public bool CheckKeyPressed(ConsoleKey? pressed = null, ConsoleModifiers mdfrKey = default)
        {
            //char? current = Conn.CheckKeyPressed(pressed, mdfrKey);
            if (this.last_input_result != null && this.last_input_result.Value.Key == pressed)
            {
                if (mdfrKey == default || mdfrKey != default && (this.last_input_result.Value.Modifiers & (ConsoleModifiers)mdfrKey) != 0)
                {
                    this.last_input_string = this.last_input_string.Substring(0, this.last_input_string.Length - 1);
                    return true;
                }
            }

            return false;
        }


        public bool CheckInputString(string typed = null, bool caseSensitive = false)
        {
            if (this.inputIsFinishedOrEnterWasPressed != null && typed != null)
            {
                bool tmp = false;
                if (caseSensitive)
                {
                    tmp = typed.ToLower() == this.inputIsFinishedOrEnterWasPressed.ToLower();
                }
                else
                {
                    tmp = typed == this.inputIsFinishedOrEnterWasPressed;
                }

                if (tmp)
                {
                    inputIsFinishedOrEnterWasPressed = null;
                    return true;
                }
            }

            return false;
        }

        public string GetLastInputStringResult()
        {
            return inputIsFinishedOrEnterWasPressed;
        }

        public string GetCurrentInputBuffer()
        {
            return this.last_input_string;
        }

        public char? GetLastInputCharSymbolResult()
        {
            if (this.last_input_result.HasValue) return last_input_result.Value.KeyChar; else return null;
        }

        public async void startGet()
        {
            while (cycleContinueMarker)
            {
                await Task.Run(() => this.last_input_result = Conn.WaitConsoleKey().Result);
                if (this.last_input_result != null)
                {
                    switch (this.last_input_result.Value.Key)
                    {
                        case ConsoleKey.Enter:
                            {
                                inputIsFinishedOrEnterWasPressed = last_input_string;
                                last_input_string = null;
                            }
                            break;
                        case ConsoleKey.Backspace:
                            {
                                if (this.last_input_string.Length > 0)
                                    last_input_string = this.last_input_string.Substring(0, this.last_input_string.Length - 1);
                            }
                            break;
                        default:
                            last_input_string += this.last_input_result.Value.KeyChar;
                            break;
                    }

                }
                if (this.last_input_result.Value != default) refreshRequired = true;
            }
        }
    }

    public class PageControllerOptions
    {
        public T getSubj<T>() where T : class
        {
            return this as T;
        }
    }

    abstract class SwitchPageControllerOptions : PageControllerOptions
    {
        public int index_start;
        public abstract int max_records_per_page { get; set; }


        public SwitchPageControllerOptions(int index_start = 0)
        {
            this.index_start = index_start;
        }

        public SwitchPageControllerOptions ChangeMaxRecordsPerPage(int i)
        {
            max_records_per_page = i; return this;
        }
    }
}
