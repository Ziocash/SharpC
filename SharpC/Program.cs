using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static System.Console;

namespace SharpC
{
    public class Program
    {
        //    [DllImport("libupoalglib")]
        //    static extern double upo_ht_sepchain_capacity();

        public static void Main(string[] args)
        {
            string officialString = string.Empty;
            unsafe
            {
                char** h = stackalloc char*[256];
                int length = 0;
                fixed (char* str = "string in pointer")
                {
                    h[0] = str;
                    length = StringLength(str);
                }
                char* temp = h[0];
                Write("Unmanaged string: ");
                for(int i = 0; i < length; i++)
                {
                    Write("{0}", *(temp++));
                }
                WriteLine();
                officialString = Marshal.PtrToStringAuto((IntPtr)h[0]);
            }
            WriteLine("Managed string: {0}", officialString);
        }

        public static unsafe int StringLength(char *str)
        {
            if(str == null)
                return 0;

            int length = 0;
            char *p = str;
            while (*p != '\0')
            {
                length++;
                p++;
            }

            return length;
        }
    }
}