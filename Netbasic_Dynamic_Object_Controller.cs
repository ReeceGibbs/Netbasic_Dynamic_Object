using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netbasic_Dynamic_Object
{

    /// <summary>
    /// the controller class for our Netbasic_Dynamic_Object
    /// these static methods will manipulate the Netbasic_Dynamic_Objects passed to them
    /// </summary>
    class Netbasic_Dynamic_Object_Controller
    {

        //a public method that can be called to check the data type of on object
        public static string GetNDODataType(object value)
        {

            //we begin a try/catch block
            try
            {

                //we switch on the value datatype
                switch (value.GetType().Name)
                {

                    //we define the String case
                    case "String":

                        //if the value is of type string then we return "NDO_STRING"
                        return "NDO_STRING";

                    //we define the Int32 case
                    //we define the Int64 case
                    //we define the Double case
                    case "Int32":
                    case "Int64":
                    case "Double":

                        //if the value is of type int then we return "NDO_NUMBER"
                        return "NDO_NUMBER";

                    //we define the Boolean case
                    case "Boolean":

                        //if the value is true we return "NDO_TRUE" else we return "NDO_FALSE"
                        return (bool)value ? "NDO_TRUE" : "NDO_FALSE";

                    //we define the Netbasic_Dynamic_Object case
                    case "Netbasic_Dynamic_Object":

                        //if the type is "Netbasic_Dynamic_Object" then we return "NDO_OBJECT"
                        return "NDO_OBJECT";

                    //we define the List case
                    case "List`1":
                    case "JArray":

                        //if the type is "List`1" then we return "NDO_ARRAY"
                        return "NDO_ARRAY";

                    //our default is to throw an exception
                    default:

                        //we throw a new exception
                        throw new Exception(value.GetType().Name + " is not a valid NDO data type.");
                }
            }
            catch (NullReferenceException)
            {

                //if we catch a NullReferenceException it means that our value is null so we return "NDO_NULL"
                return "NDO_NULL";
            }
        }

        //a public method that can be called to create a Netbasic_Dynamic_Object using a JSON or XML string
        public static bool NDORead(string inputString, byte inputType, out Netbasic_Dynamic_Object ndoHandle)
        {

            //we initialize our ndoHandle to a new, default Netbasic_Dynamic_Object
            ndoHandle = new Netbasic_Dynamic_Object();

            //we define a switch statement to check whether the inputString is of type XML or of type JSON
            switch (inputType)
            {

                //we check to see if the inputType is 0
                case 0:

                    //we begin a try/catch to catch any potential errors
                    try
                    {

                        //if the inputType is 0, then the inputString is of type JSON
                        //we deserialize the JSON string to a dictionary interface object 
                        IDictionary<string, object> rawProperties = JsonConvert.DeserializeObject<IDictionary<string, object>>(inputString);

                        //we now iterate through the rawProperties and give our values NDO datatypes and add them to our ndoHandle
                        foreach (KeyValuePair<string, object> rawProperty in rawProperties)
                        {

                            //we call the NDOSetProperty method
                            NDOSetProperty(ndoHandle, rawProperty.Key, rawProperty.Value, false);
                        }
                    }
                    catch (Exception e)
                    {

                        //if anything goes wrong when deserializing our JSON string, we throw an error 
                        Console.WriteLine(e.Message);

                        //and return false to indicate that we could not successfully create our Netbasic_Dynamic_Object
                        return false;
                    }

                    //now that we have created our Netbasic_Dynamic_Object from our JSON string we break out of the switch statement
                    break;
                case 1:

                    //if the inputType is 1 then we are working with an XML string *to be implemented in the future*
                    break;
                default:

                    //if we are passed an input type that is not valid, then we want to display an error message and return false
                    Console.WriteLine("Invalid input type in NDORead method call.");

                    return false;
            }

            //we return true to indicate that everything went smoothly
            return true;
        }

        //a public method that can be called to serialize a Netbasic_Dynamic_Object to JSON or XML
        public static bool NDOWrite(Netbasic_Dynamic_Object ndoHandle, byte outputType, out string outputString)
        {

            //we initialize our outputString
            outputString = "";

            //we use a switch statement to determine whether we want our output string to be in JSON or XML format
            switch (outputType)
            {

                //we check to see if the outputType is 0
                case 0:

                    //we begin a try/catch block so that we can handle any potential errors
                    try
                    {

                        //if the output type is 0 then we want to serialize our Netbasic_Dynamic_Object to JSON
                        //we initialize a temporary dictionary that will contain our raw properties without the NDO types
                        Dictionary<string, object> rawProperties = new Dictionary<string, object>();

                        //we iterate through the properties in our ndoHandle
                        foreach (KeyValuePair<string, object> ndoProperty in ndoHandle.DynamicObjectProperties)
                        {

                            //we extract the key value pair from within the rawProperty
                            KeyValuePair<string, object> ndoPropertyValue = (KeyValuePair<string, object>)ndoProperty.Value;

                            //we add the value of each key value pair to rawProperties
                            rawProperties.Add(ndoProperty.Key, ndoPropertyValue.Value);
                        }

                        //we set the outputString value to the serialized IDictionary
                        outputString = JsonConvert.SerializeObject(rawProperties);
                    }
                    catch (Exception e)
                    {
                        //if there are any issues we want to output the error message but not t hrow the error so we can still return false
                        Console.WriteLine(e.Message);

                        //we return false to indicate that the serialization has not been successful
                        return false;
                    }

                    //now that we have serialized our object we exit the switch statement
                    break;

                //we check to see if the outputType is 1
                case 1:
                    
                    //if the outputType is 1 then we need to serialize our dynamic object to XML *to be implemented in the future*
                    break;
                default:

                    //if we are passed an output type that is not valid, then we want to display an error message and return false
                    Console.WriteLine("Invalid output type in NDOWrite method call.");

                    return false;
            }

            //we return true to indicate that everything completed successfully
            return true;
        }

        //a method that can be called to set the value of a dynamic object property
        public static bool NDOSetProperty(Netbasic_Dynamic_Object ndoHandle, string propName, object value, bool castToNum)
        {

            //we check to see if the user wants to cast the value to a number
            if (castToNum)
            {

                //we initialize an int variable that will be assigned the result of the strings conversion to an int if it is successful
                int tempIntValue;

                //we begin a try/catch
                try
                {

                    //we now check to see if the name of the property exists
                    if (ndoHandle.DynamicObjectProperties.ContainsKey(propName))
                    {

                        //if the property exists, then we want to update the value of the property
                        //we set the value to an int if we can convert it to one, else we set the value to a double
                        //we use tryparse for the first condition because we want the control flow to pass to the else statement in the event we cannot convert to int which is quite likely
                        ndoHandle.DynamicObjectProperties[propName] = (Int32.TryParse(value.ToString(), out tempIntValue)) ? new KeyValuePair<string, object>("NDO_NUMBER", tempIntValue) : new KeyValuePair<string, object>("NDO_NUMBER", Convert.ToDouble(value.ToString()));
                    }
                    else
                    {

                        //if the property does not yet exist, then we want to add the property to our object
                        ndoHandle.DynamicObjectProperties.Add(propName, (Int32.TryParse(value.ToString(), out tempIntValue)) ? new KeyValuePair<string, object>("NDO_NUMBER", tempIntValue) : new KeyValuePair<string, object>("NDO_NUMBER", Convert.ToDouble(value.ToString())));
                    }
                }
                catch (Exception e)
                {

                    //if we cant convert to neither an int nor a double, we want to display an error message and return false
                    Console.WriteLine(e.Message);

                    return false;
                }
            }
            else
            {

                //we begin a try/catch
                try
                {

                    //we now check to see if the name of the property exists
                    if (ndoHandle.DynamicObjectProperties.ContainsKey(propName))
                    {

                        //if the property exists, then we want to update the value of the property
                        ndoHandle.DynamicObjectProperties[propName] = new KeyValuePair<string, object>(GetNDODataType(value), value);
                    }
                    else
                    {

                        //if the property does not yet exist, then we want to add the property to our object
                        ndoHandle.DynamicObjectProperties.Add(propName, new KeyValuePair<string, object>(GetNDODataType(value), value));
                    }
                }
                catch (Exception e)
                {

                    //if for some reason we can't set the value we want to display an error message and return false
                    Console.WriteLine(e.Message);

                    return false;
                }
            }

            //if we have gotten to this point, then nothing has gone wrong so we return true to indicate that this method was run successfully
            return true;
        }

        //a public method that can be called to return the value of a property of our dynamic object given the property name
        public static bool NDOGetProperty(Netbasic_Dynamic_Object ndoHandle, string propName, out object value, out string valueType)
        {

            //we begin a try/catch block
            try
            {

                //we get the key value pair that is the value of the property
                KeyValuePair<string, object> propKeyValuePair = (KeyValuePair<string, object>)ndoHandle.DynamicObjectProperties[propName];

                //we set the out value_type argument to the key value pair key
                valueType = propKeyValuePair.Key;

                //we set the out value argument to the key value pair value
                value = propKeyValuePair.Value;

            }
            catch (Exception e)
            {

                //if we cannot reference the property given the propName then we want to display an error message and return false
                Console.WriteLine(e.Message);

                //we set the out value argument to a new object
                value = new object();

                //we set the out value_type argument to an empty string
                valueType = "";

                //we return false to indicate that something went wrong
                return false;
            }

            //we return true to indicate that the processes of the method were carried out successfully
            return true;
        }

        //a public method that can be called to delete a property from the property list of a dynamic object
        public static bool NDODeleteProperty(Netbasic_Dynamic_Object ndoHandle, string propName)
        {

            //we begin a try/catch
            try
            {

                //we check to see if the property exists in the property list, if it doesn't we throw an error
                if (!ndoHandle.DynamicObjectProperties.ContainsKey(propName)) { throw new Exception($"Property [{propName}] not found in call to NDODeleteProperty." ); }

                //we remove a property from our ndoHandle's list of properties given the propName
                ndoHandle.DynamicObjectProperties.Remove(propName);
            }
            catch (Exception e)
            {

                //if we cannot find the property given the propName we display an error message and return false
                Console.WriteLine(e.Message);

                return false;
            }

            //we return true here because everything has gone smoothly
            return true;
        }

        //a public method that can be called to return a list of all dynamic object property names
        public static bool NDOGetPropertyNames(Netbasic_Dynamic_Object ndoHandle, out List<string> propKeys)
        {

            //we begin a try/catch block
            try
            {

                //if there are not any properties in our property list then we throw an exception
                if (ndoHandle.DynamicObjectProperties.Count == 0) { throw new Exception("Empty property list in call to NDOGetPropertyNames"); }

                //we set propKeys to a list of keys of our property dictionary
                propKeys = ndoHandle.DynamicObjectProperties.Keys.ToList();
            }
            catch (Exception e)
            {

                //if for some reason there is an issue, we want to display an error message and return false
                Console.WriteLine(e.Message);

                //we our out propKeys to an empty list
                propKeys = new List<string>();

                return false;
            }

            //since everything has gone well, we return true
            return true;
        }

        //a public method that can be called to return the value in a property list given the currentPropertyIndex prop of our dynamic object
        public static bool NDOGetNextProperty(Netbasic_Dynamic_Object ndoHandle, out string propName, out object propValue, out string propType)
        {

            //we begin a try/catch block
            try
            {

                //we set our propName to the key of our property given the currentPropertyIndex property
                propName = ndoHandle.DynamicObjectProperties.ElementAt(ndoHandle.currentPropertyIndex).Key;

                //we set our propValue to the valur of our property given the currentPropertyIndex property
                KeyValuePair<string, object> propKeyValuePair = (KeyValuePair<string, object>)ndoHandle.DynamicObjectProperties.ElementAt(ndoHandle.currentPropertyIndex).Value;

                //we set our propType to the key of our key value pair
                propType = propKeyValuePair.Key;

                //we set our propValue to the value of our key value pair
                propValue = propKeyValuePair.Value;

                //we increment our currentPropertyIndex
                ndoHandle.currentPropertyIndex++;
            }
            catch (ArgumentOutOfRangeException e)
            {

                //if we are referencing an element out of range then we want to let display an error message, set the currentPropertyIndex back to zero and return false
                Console.WriteLine(e.Message);

                propName = "";

                propValue = new object();

                propType = "";

                ndoHandle.currentPropertyIndex = 0;

                return false;
            }
            catch (Exception e)
            {

                //if there are any other issues, then we want to display an error message and return false
                Console.WriteLine(e.Message);

                propName = "";

                propValue = new object();

                propType = "";

                return false;
            }

            //because everything has completed successfully, we return true
            return true;
        }

        //a public method that can be called to instantiate a Netbasic_Dynamic_Object of a given type
        public static bool NDOCreate(string ndoType, out Netbasic_Dynamic_Object ndoHandle)
        {

            //we instantiate a Netbasic_Dynamic_Object by calling our constructor that allows us to do this
            ndoHandle = new Netbasic_Dynamic_Object(ndoType);

            //we return true
            return true;
        }

        //a public method that can be called to get the type of a Netbasic_Dynamic_Object
        public static bool NDOGetType(Netbasic_Dynamic_Object ndoHandle, out string type)
        {

            //we set the value of our out argument type to our the DynamicObjectType of our ndoHandle
            type = ndoHandle.DynamicObjectType;

            //we return true
            return true;
        }

        //a public method that can be called to test against the type of a Netbasic_Dynamic_Object
        public static bool NDOIsTypeOf(Netbasic_Dynamic_Object ndoHandle, string type)
        {

            //if the type argument matches the DynamicObjectType then we return true, else false
            return (ndoHandle.DynamicObjectType == type) ? true : false;
        }

        //a public function that can be called to return the size of a NDO_ARRAY type NetbasicDynamicObject
        public static bool NDOGetArraySize(Netbasic_Dynamic_Object ndoHandle, out int size)
        {

            //we begin a try/catch
            try
            {

                //we set our size out argument's value to the count of our DynamicObject
                size = ndoHandle.DynamicObject.Count;
            }
            catch (Exception e)
            {

                //if there is an error, e.g. the count prop does not exist because the ndoHandle is not an NDO_ARRAY then we display an error message and return false
                Console.WriteLine(e.Message);

                //we set the size to 0
                size = 0;

                return false;
            }
            return true;
        }

        //a public method that can be called to get an item in a NDO_ARRAY given the index
        public static bool NDOArrayGetItem(Netbasic_Dynamic_Object ndoHandle, int index, out object value, out string type)
        {

            //we begin a try/catch block
            try
            {

                //we create a temp key value pair
                KeyValuePair<string, object> itemKeyValuePair = ndoHandle.DynamicObject[index - 1];

                //we set the out type argument to the key of the item
                type = itemKeyValuePair.Key;

                //we set our out value argument to the value of the element at the index - 1 in our ndoHandle.DynamicObject
                value = itemKeyValuePair.Value;
            }
            catch (Exception e)
            {

                //if there is an issue, we display an error message, set our out value to an empty object and return false
                Console.WriteLine(e.Message);

                type = "";

                value = new object();

                return false;
            }

            //since the element was set successfully, we return true
            return true;
        }
    }
}
