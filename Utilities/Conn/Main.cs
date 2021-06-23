using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Modules
{
    /**
     * Functions here have a key modifiers; till now key modifiers are ugly in linux-with-netcore and there are tasks 
     * for netcore improvals within Console Redesign big task
     */
    public static class Conn
    {   
        // For posting a keyboard event
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // For getting a virtual key code from a character
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern uint VkKeyScan(char ch);

        const int SWP_NOSIZE = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr MyConsole = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);


        public static OSPlatform GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            throw new Exception("Cannot determine operating system!");
        }
        private static bool checkColorDefinition(ref string src, ref int i)
        {
            int j = src.IndexOf(" ", i);
            string tmp;
            if ((j - i - 1) > 0) tmp = src.Substring(i + 1, j - i - 1);
                else return false;

            var abc = tmp.ToCharArray();
            bool found = true;

            switch (tmp.ToLower())
            {
                case "red": { Console.ForegroundColor = ConsoleColor.Red; break; }
                case "dred": { Console.ForegroundColor = ConsoleColor.DarkRed; break; }
                case "black": { Console.ForegroundColor = ConsoleColor.Black; break; }
                case "blue": { Console.ForegroundColor = ConsoleColor.Blue; break; }
                case "cyan": { Console.ForegroundColor = ConsoleColor.Cyan; break; }
                case "dblue": { Console.ForegroundColor = ConsoleColor.DarkBlue; break; }
                case "dcyan": { Console.ForegroundColor = ConsoleColor.DarkCyan; break; }
                case "dgray": { Console.ForegroundColor = ConsoleColor.DarkGray; break; }
                case "dgreen": { Console.ForegroundColor = ConsoleColor.DarkGreen; break; }
                case "dmagenta": { Console.ForegroundColor = ConsoleColor.DarkMagenta; break; }
                case "dyellow": { Console.ForegroundColor = ConsoleColor.DarkYellow; break; }
                case "gray": { Console.ForegroundColor = ConsoleColor.Gray; break; }
                case "green": { Console.ForegroundColor = ConsoleColor.Green; break; }
                case "magenta": { Console.ForegroundColor = ConsoleColor.Magenta; break; }
                case "white": { Console.ForegroundColor = ConsoleColor.White; break; }
                case "yellow": { Console.ForegroundColor = ConsoleColor.Yellow; break; }
                case "bla": { Console.ForegroundColor = ConsoleColor.Black; break; }
                case "blu": { Console.ForegroundColor = ConsoleColor.Blue; break; }
                case "cya": { Console.ForegroundColor = ConsoleColor.Cyan; break; }
                case "dblu": { Console.ForegroundColor = ConsoleColor.DarkBlue; break; }
                case "dcya": { Console.ForegroundColor = ConsoleColor.DarkCyan; break; }
                case "dgra": { Console.ForegroundColor = ConsoleColor.DarkGray; break; }
                case "dgre": { Console.ForegroundColor = ConsoleColor.DarkGreen; break; }
                case "dmag": { Console.ForegroundColor = ConsoleColor.DarkMagenta; break; }
                case "dyel": { Console.ForegroundColor = ConsoleColor.DarkYellow; break; }
                case "gra": { Console.ForegroundColor = ConsoleColor.Gray; break; }
                case "gre": { Console.ForegroundColor = ConsoleColor.Green; break; }
                case "mag": { Console.ForegroundColor = ConsoleColor.Magenta; break; }
                case "whi": { Console.ForegroundColor = ConsoleColor.White; break; }
                case "yel": { Console.ForegroundColor = ConsoleColor.Yellow; break; }
                //case "red": {Console.ForegroundColor = ConsoleColor; break;}

                default: { found = false; break; }
            }

            if (found) i = i + tmp.Length + 1;
            return found;
        }

        public static void Line(string src = "")
        {
            Chars(src);
            Console.WriteLine();
        }

        public static void Line(string src = "", params object[] stringFormatArguments)
        {
            src = string.Format(src, stringFormatArguments);
            Chars(src);
        }

        public static void Line(int num)
        {
            for (int i = 0; i < num; i++) Console.WriteLine();
        }

        public static byte GetBottomLineNum()
        {
            int OS_offset = 2;
            /*if (GetOperatingSystem() == OSPlatform.Linux) OS_offset = 4;*/
            return (byte)(Console.WindowHeight - Console.CursorTop - OS_offset);
        }

        public static System.Collections.Generic.Dictionary<char, int> GetMaxFullscreenConsoleSize()
        {
            return new System.Collections.Generic.Dictionary<char, int>() { { 'h', Console.LargestWindowHeight }, { 'w', Console.LargestWindowWidth } };
        }

        public static System.Collections.Generic.Dictionary<char, int> GetCurrentConsoleSize()
        {
            return new System.Collections.Generic.Dictionary<char, int>() { { 'h', Console.WindowHeight }, { 'w', Console.WindowWidth } };
        }

        public static System.Collections.Generic.Dictionary<char, int> SwitchFullScreen()
        {
            var previous = GetCurrentConsoleSize();
            if (GetOperatingSystem() == OSPlatform.Windows) { 
                SetWindowPos(MyConsole, 0, 0, 0, 0, 0, SWP_NOSIZE);
            }
            SetConsoleSize(GetMaxFullscreenConsoleSize());
            return previous;
        }

        public static void SetConsoleSize(System.Collections.Generic.Dictionary<char, int> sizeWH)
        {
            if (sizeWH != null)
            {
                int w, h;
                if(sizeWH.TryGetValue('w', out w)
                 && sizeWH.TryGetValue('h', out h)) {
                    Console.WindowHeight = h;
                    Console.WindowWidth = w;
                }
            }
        }

        public static void Chars(string src)
        {
            if (src != null)
            {
                int i; bool found = false;
                int lmo = src.Length - 1;
                for (i = 0; i < src.Length; i++)
                {
                    if ((src[i] == '!') && (i < lmo))
                    {
                        if (src[i + 1] == '!')
                        {
                            if (found)
                            {
                                Console.ResetColor();
                                i = i + 1;
                            }
                            else Console.Write(src[i]);
                        }
                        else
                        {
                            found = Conn.checkColorDefinition(ref src, ref i);
                            if (found) i = i + 1;
                            Console.Write(src[i]);

                        }
                    }
                    else
                    {
                        Console.Write(src[i]);
                    }
                }
            }
        }





        public static char? CheckKeyPressed(ConsoleKey? pressed = null, byte mdfrKey = 0)
        {
            ConsoleKeyInfo k;
            k = Console.ReadKey(true);
            //Conn.Line(((((int)ConsoleModifiers.Control) == 4).ToString())); is true;

            if 
                (((k.Key == pressed) && (pressed != null) && (mdfrKey == 0 || mdfrKey != 0 && (k.Modifiers & (ConsoleModifiers)mdfrKey) != 0)) 
                ||
                (pressed == null && (mdfrKey != 0 && (mdfrKey == 0 || (k.Modifiers & (ConsoleModifiers)mdfrKey) != 0))))
                return k.KeyChar;

            char kch = k.KeyChar;
            uint tmp = VkKeyScan(kch);
            keybd_event((byte)tmp,
                0,
                0,
                0);

            // Let the key up
            keybd_event((byte)tmp, 0, 2, 0);

            // Simulating a Alt+Tab keystroke
            //keybd_event(VK_MENU, 0xb8, 0, 0); //Alt Press
            //keybd_event(VK_TAB, 0x8f, 0, 0); // Tab Press
            //keybd_event(VK_TAB, 0x8f, KEYEVENTF_KEYUP, 0); // Tab Release
            //keybd_event(VK_MENU, 0xb8, KEYEVENTF_KEYUP, 0); // Alt Release
            return null;
        }

        public static bool WaitKeyPressed(ConsoleKey pressed, byte mdfrKey = 0)
        {
            //Console.Read();
            if (CheckKeyPressed(pressed, mdfrKey) != null) return true;
            else return false;
        }




        public static string InputException(ConsoleKey[] exceptionKeys, ref ConsoleKey? exceptionThatWasCatched, bool inputMask = false)
        {
            string Result = "";
            ConsoleKeyInfo k = new ConsoleKeyInfo();

            do
            {
                k = Console.ReadKey(true);
                if (Array.IndexOf(exceptionKeys, k.Key) != -1)
                {
                    exceptionThatWasCatched = k.Key;
                    return "";
                }


                if (k.Key != ConsoleKey.Backspace && k.Key != ConsoleKey.Enter)
                {
                    Result += k.KeyChar;
                    if (inputMask) Console.Write("*"); else Console.Write(k.KeyChar);
                }
                else
                {
                    if (k.Key == ConsoleKey.Backspace && Result.Length > 0)
                    {
                        Result = Result.Substring(0, (Result.Length - 1));
                        Console.Write("\b \b");
                    }
                }

            } while (k.Key != ConsoleKey.Enter);

            return Result;
        }

        public static void Clear()
        {
            /*var clearString = TerminalFormatStrings.Instance.Clear;
            if (clearString.Equal("\E[H\E[J - a standard value", StringComparison.Ordinal)
            {
                clearString = "\E3J" + clearString;
            }

            WriteStdoutAnsiString(clearString);*/
            Console.Clear();
            if (GetOperatingSystem() == OSPlatform.Linux)
            { 
                Console.Write("\x1b[3J");
            }
        }

        public static char AwaitAnyKeyPressNoActions()
        {
            return Console.ReadKey(true).KeyChar;
        }


        public static async Task<ConsoleKeyInfo?> WaitConsoleKey()
        {
            try
            {
                ConsoleKeyInfo key = default;
                await Task.Run(() => key = Console.ReadKey(true));
                return key;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
