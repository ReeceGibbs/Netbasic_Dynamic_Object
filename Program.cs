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

            testObject.DynamicObject.Add(new KeyValuePair<string, object>("UDO_STRING", "PENIS"));
            testObject.DynamicObject.Add(new KeyValuePair<string, object>("UDO_NUMBER", 7));
            testObject.DynamicObject.Add(new KeyValuePair<string, object>("UDO_ARRAY", new object[] { "test", "hello" }));

            object value;
            string type;

            Netbasic_Dynamic_Object_Controller.NDOArrayGetItem(testObject, 2, out value, out type);

            Console.WriteLine();
        }
    }
}
