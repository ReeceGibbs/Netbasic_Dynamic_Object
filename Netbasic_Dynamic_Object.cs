using System.Collections.Generic;
using System.Dynamic;

namespace Netbasic_Dynamic_Object
{
    class Netbasic_Dynamic_Object
    {

        //we create a dynamic object property
        public dynamic DynamicObject { get; set; }

        //we create a property that will store the DynamicObject type
        public string DynamicObjectType { get; set; }

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

        //a constructor that can be called to initialize a NDO of a certain type
        public Netbasic_Dynamic_Object(string ndoType)
        {

            //we switch on the ndoType
            switch (ndoType)
            {

                //we define the case where the type is "NDO_OBJECT"
                case "NDO_OBJECT":

                    //if the type is NDO_OBJECT, then we simply instantiate a new, empty dynamic object as we would with our default constructor 
                    DefaultNDOSetup();

                    //we break
                    break;

                //we define the case where the type is "NDO_ARRAY"
                case "NDO_ARRAY":

                    //if the type is NDO_ARRAY we want to instantiate our DynamicObject as a generic list
                    DynamicObject = new List<object>();

                    //we set our DynamicObjectType
                    DynamicObjectType = "NDO_ARRAY";

                    //we also initialize our currentIndex for iterative functionality
                    currentIndex = 0;
                    break;

                //we define the case where the type is "NDO_TRUE"
                case "NDO_TRUE":

                    //we instantiate our DynamicObject as a boolean with the value 'true'
                    DynamicObject = true;

                    //we set our DynamicObjectType
                    DynamicObjectType = "NDO_TRUE";

                    //we break;
                    break;

                //we define the case where the type is "NDO_FALSE"
                case "NDO_FALSE":

                    //we instantiate our DynamicObject as a boolean with the value 'false'
                    DynamicObject = false;

                    //we set our DynamicObjectType
                    DynamicObjectType = "NDO_FALSE";

                    //we break
                    break;

                //we define the case where the type is "NDO_NULL"
                //we also set the DynamicObject value to null by default if no valid type is passed
                case "NDO_NULL":
                default:

                    //we instantiate our DynamicObject as null
                    DynamicObject = null;

                    //we set our DynamicObjectType
                    DynamicObjectType = "NDO_NULL";

                    //we break
                    break;
            }
        }

        //a method that runs the default setup of an NDO object
        public void DefaultNDOSetup()
        {

            //we initialize our dynamic object as an expandoobject
            DynamicObject = new ExpandoObject();

            //we set our DynamicObjectType to NDO_OBJECT
            DynamicObjectType = "NDO_OBJECT";

            //we initalize our DynamicObjectProperties as our DynamicObject property cast to a dictionary interface so that we can implement reflection-like adding and removing of properties
            DynamicObjectProperties = (IDictionary<string, object>)DynamicObject;

            //we initalize our currentIndex to 0
            currentIndex = 0;
        }
    }
}
