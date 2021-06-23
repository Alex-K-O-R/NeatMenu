using System;

namespace Lib
{
    public abstract class CommonLibrary
    {
        //new private object[] arguments;
        public CommonLibraryCore service = new CommonLibraryCore();

        //private override object[] arguments;
        public void setArguments(params object[] args)
        {
            this.service.clearArguments();
            this.service.__arguments = args;
        }

        public T[] getArgumentsOfType<T>(bool preserveIndexes = true)
        {
            var result = new T[0] { };
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
    }

    public class CommonLibraryCore
    {
        public object[] __arguments;
        public void clearArguments()
        {
            this.__arguments = new object[0];
        }

        public string[] list()
        {
            string[] res = new string[0];

            foreach (var mtd in this.GetType().GetMethods())
            {
                if (GetAttribute<Image>(mtd) != null)
                {
                    Array.Resize<string>(ref res, res.Length + 1);
                    res[res.Length - 1] = GetAttribute<Image>(mtd).Text;
                }
            }

            return res;
        }

        public string desc(string methodImage)
        {
            if (methodImage == null || methodImage.Replace(" ", "") == "") return "";
            foreach (var mtd in this.GetType().GetMethods())
            {
                if (GetAttribute<Image>(mtd) != null)
                    if (GetAttribute<Image>(mtd).Text == methodImage) return GetAttribute<Description>(mtd).Text;
            }

            return "";
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Image : Attribute
        {
            public Image(string Image)
            {
                this.img = Image;
            }
            public string Text
            {
                get
                {
                    return this.img;
                }
            }
            private string img;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Description : Attribute
        {
            public Description(string Description)
            {
                this.desc = Description;
            }
            public string Text
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
