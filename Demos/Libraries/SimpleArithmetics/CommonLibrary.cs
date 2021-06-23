using System;

namespace Lib
{
    public abstract class CommonLibrary
    {
        public virtual string Name { get; set; }
        //new private object[] arguments;
        public CommonLibraryCore service;

        protected CommonLibrary()
        {
            this.service = new CommonLibraryCore(this);
        }


        //private override object[] arguments;
        public void setArguments(params object[] args)
        {
            this.service.clearArguments();
            if(args!=null && args.Length == 1 && args[0] is Array)
            {
                // due to restriction of single[] to object[] direct cast
                this.service.__arguments = new object[((Array)args[0]).Length];
                for (var i = 0; i < ((Array)args[0]).Length; i++) {
                    this.service.__arguments[i] = ((Array)args[0]).GetValue(i);
                }
            } else
                this.service.__arguments = args;
        }

        public T[] getArgumentsOfType<T>(bool preserveIndexes = true)
        {
            var result = new T[0] { };
            if (this.service.__arguments == null) return null;
            foreach (object arg in this.service.__arguments)
            {
                if (preserveIndexes) { 
                    Array.Resize(ref result, result.Length + 1); 
                }

                try {
                    if (!preserveIndexes) Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = (T)arg;
                } catch (Exception e) { }
                /*if (arg is T || arg as T != null)
                {
                    if (!preserveIndexes) Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = (T)arg;
                }*/
            }
            return (result.Length > 0) ? result : null;
        }

        public T InvokeFunctionByName<T>(string methodName) where T : new()
        {
            if (methodName == null || methodName.Replace(" ", "") == "") return default(T);
            foreach (var mtd in this.GetType().GetMethods())
            {
                if (mtd.Name == methodName)
                {
                    try
                    {
                        return ((T)mtd.Invoke(this, null));
                    }
                    catch (Exception e) { return default(T); }
                }
            }
            return default(T);
        }
}

    public class CommonLibraryCore
    {
        public abstract class CLAttribute:Attribute
        {
            public abstract string Text
            { get; }
        }

        public object[] __arguments;
        private CommonLibrary __CommonLibrary;
        public void clearArguments()
        {
            this.__arguments = new object[0];
        }

        public CommonLibraryCore(CommonLibrary commonLibrary)
        {
            __CommonLibrary = commonLibrary;
        }

        public string[] listAvailableFunctions()
        {
            string[] res = new string[0];
            var mtds = __CommonLibrary.GetType().GetMethods();
            foreach (var mtd in mtds)
            {
                if (GetAttribute<Image>(mtd) != null)
                {
                    Array.Resize<string>(ref res, res.Length + 1);
                    res[res.Length - 1] = mtd.Name /*+ ": " + GetAttribute<Image>(mtd).Text*/;
                }
            }

            return res;
        }

        public string getFunctionDescByFName(string methodName)
        {
            return getFunctionAttributeByFName<Description>(methodName);
        }

        public string getFunctionImageByFName(string methodName)
        {
            return getFunctionAttributeByFName<Image>(methodName);
        }

        private string getFunctionAttributeByFName<T>(string methodName) where T:CLAttribute
        {
            if (methodName == null || methodName.Replace(" ", "") == "") return "";
            foreach (var mtd in __CommonLibrary.GetType().GetMethods())
            {
                    if (mtd.Name == methodName) return GetAttribute<T>(mtd).Text;
            }

            return "";
        }

        [AttributeUsage(AttributeTargets.Method)]
         public class Image : CLAttribute
        {
            public Image(string Image)
            {
                this.img = Image;
            }
            override public string Text
            {
                get
                {
                    return this.img;
                }
            }
            private string img;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Description : CLAttribute
        {
            public Description(string Description)
            {
                this.desc = Description;
            }
            override public string Text
            {
                get
                {
                    return this.desc;
                }
            }
            private string desc;
        }

        public static T GetAttribute<T>(System.Reflection.MemberInfo t) where T : Attribute
        {
            foreach (object attribute in Attribute.GetCustomAttributes(t, true))
            {
                if (attribute is T)
                {
                    return (attribute as T);
                }
            }
            return null;
        }
    }

}
