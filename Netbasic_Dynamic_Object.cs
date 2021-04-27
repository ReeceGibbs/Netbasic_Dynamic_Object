using System.Collections.Generic;
using System.Dynamic;

namespace Netbasic_Dynamic_Object
{
    public class Netbasic_Dynamic_Object
    {

        //we create a dynamic object property
        public dynamic DynamicObject { get; set; }

        //we create a property that will store the DynamicObject type
        public object DynamicObjectType { get; set; }

        //we create a dictionary interface property so that we can add and remove properties from our DynamicObject at runtime with string/object references like we would with reflection
        public IDictionary<string, object> DynamicObjectProperties { get; set; }

        //we create a property that will track the current index of the property we are referencing in an iteration
        public int currentIndex { get; set; }

        //our Netbasic_Dynamic_Object default constructor
        public Netbasic_Dynamic_Object()
        {

            //we run our default setup
            DefaultNDOSetup();
        }

        //a method that runs the default setup of an NDO object
        public void DefaultNDOSetup()
        {

            //we initialize our dynamic object as an expandoobject
            DynamicObject = new ExpandoObject();

            //we set our DynamicObjectType to NDO_OBJECT
            DynamicObjectType = Netbasic_Dynamic_Object_Controller.NDO_OBJECT;

            //we initalize our DynamicObjectProperties as our DynamicObject property cast to a dictionary interface so that we can implement reflection-like adding and removing of properties
            DynamicObjectProperties = (IDictionary<string, object>)DynamicObject;

            //we initalize our currentIndex to 0
            currentIndex = 0;
        }
    }
}
