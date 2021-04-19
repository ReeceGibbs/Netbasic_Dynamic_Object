using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netbasic_Dynamic_Object
{
    class Program
    {
        static void Main(string[] args)
        {

            Netbasic_Dynamic_Object testObject;

            Netbasic_Dynamic_Object_Controller.NDOCreate("NDO_ARRAY", out testObject);

            Netbasic_Dynamic_Object_Controller.NDOArrayAppendItem(testObject, "test");
            Netbasic_Dynamic_Object_Controller.NDOArrayAppendItem(testObject, true);
            Netbasic_Dynamic_Object_Controller.NDOArrayAppendItem(testObject, 44.5);
            Netbasic_Dynamic_Object_Controller.NDOArrayAppendItem(testObject, new List<object>() { "test", "4" });

            Netbasic_Dynamic_Object_Controller.NDOArrayDeleteItem(testObject, 5);

            Console.WriteLine();
        }
    }
}
