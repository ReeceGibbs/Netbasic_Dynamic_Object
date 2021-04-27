using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Netbasic_Dynamic_Object
{

    /// <summary>
    /// the controller class for our Netbasic_Dynamic_Object
    /// these static methods will manipulate the Netbasic_Dynamic_Objects passed to them
    /// </summary>
    public class Netbasic_Dynamic_Object_Controller
    {

        //here we need to initialize all of the possible static NDO header options with their default values
        //NDO status codes
        public static object NDO_SUCCESS { get; set; } = 0;
        public static object NDO_ERROR { get; set; } = -1;
        public static object NDO_INVALIDHANDLE { get; set; } = -2;

        //NDO value types
        public static object NDO_FALSE { get; set; } = 0;
        public static object NDO_TRUE { get; set; } = 1;
        public static object NDO_NULL { get; set; } = 2;
        public static object NDO_NUMBER { get; set; } = 3;
        public static object NDO_STRING { get; set; } = 4;
        public static object NDO_ARRAY { get; set; } = 5;
        public static object NDO_OBJECT { get; set; } = 6;

        //NDO error codes
        public static object NDOERROR_EXCEPTION { get; set; } = 0;
        public static object NDOERROR_OUTOFMEMORY { get; set; } = 1;
        public static object NDOERROR_INVALIDJSON { get; set; } = 2;
        public static object NDOERROR_NOTSTANDALONE { get; set; } = 3;
        public static object NDOERROR_NOTFOUND { get; set; } = 4;
        public static object NDOERROR_INVALIDNAME { get; set; } = 5;
        public static object NDOERROR_NOTANOBJECT { get; set; } = 6;
        public static object NDOERROR_NOTANARRAY { get; set; } = 7;
        public static object NDOERROR_INVALIDINDEX { get; set; } = 8;
        public static object NDOERROR_OUTOFBOUND { get; set; } = 9;
        public static object NDOERROR_INVALIDOPTION { get; set; } = 10;
        public static object NDOERROR_INVALIDOPTIONVALUE { get; set; } = 11;
        public static object NDOERROR_OPTIONNOTSET { get; set; } = 12;
        public static object NDOERROR_INVALIDFORMAT { get; set; } = 13;
        public static object NDOERROR_INVALIDVALUE { get; set; } = 14;
        public static object NDOERROR_INVALIDTYPE { get; set; } = 15;
        public static object NDOERROR_REFERENCECYCLE { get; set; } = 16;
        public static object NDOERROR_INVALIDXML { get; set; } = 17;
        public static object NDOERROR_DOMAPIFAILURE { get; set; } = 18;

        //NDO flags
        public static object NDOFORMAT_JSON { get; set; } = 0;
        public static object NDOFORMAT_XML { get; set; } = 1;

        //NDO options
        public static object NDOOPTION_NDO2XML_XMLSTYLE { get; set; } = 0;
        public static object NDOOPTION_NDO2XML_NAMESPACE { get; set; } = 1;
        public static object NDOOPTION_OUTPUTMODE { get; set; } = 2;
        public static object NDOOPTION_XML2NDO_INFERNUMBER { get; set; } = 3;
        public static object NDOOPTION_XML2NDO_INFERBOOLEAN { get; set; } = 4;
        public static object NDOOPTION_XML2NDO_INFERNULL { get; set; } = 5;
        public static object NDOOPTION_XML2NDO_EMPTY2NULL { get; set; } = 6;
        public static object NDOOPTION_XML2NDO_TRIMWHITESPACE { get; set; } = 7;
        public static object NDOOPTION_XML2NDO_CASEINSENSITIVE { get; set; } = 8;
        public static object NDOOPTION_XML2NDO_KEEPROOT { get; set; } = 9;
        public static object NDOOPTION_NDO2XML_ROOTNAME { get; set; } = 10;
        public static object NDOOPTION_NDO2XML_NULL2EMPTY { get; set; } = 11;
        public static object NDOOPTION_NDO2XML_NAMESPACEPREFIX { get; set; } = 12;

        //NDO option values
        public static object NDO_XMLSTYLE_ATTR { get; set; } = "ATTRIBUTE";
        public static object NDO_XMLSTYLE_ELEM { get; set; } = "ELEMENT";
        public static object NDO_OUTPUT_COMPACT { get; set; } = "COMPACT";
        public static object NDO_OUTPUT_FORMATTED { get; set; } = "FORMATTED";
        public static object NDO_OPTION_ON { get; set; } = "ON";
        public static object NDO_OPTION_OFF { get; set; } = "OFF";

        //the last error that occured
        public static KeyValuePair<object, string> LAST_ERROR = new KeyValuePair<object, string>();

        //a public method that can be called to check the data type of on object
        public static object GetNDODataType(object value)
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
                        return NDO_STRING;

                    //we define the Int32 case
                    //we define the Int64 case
                    //we define the Double case
                    case "Int32":
                    case "Int64":
                    case "Double":

                        //if the value is of type int then we return "NDO_NUMBER"
                        return NDO_NUMBER;

                    //we define the Boolean case
                    case "Boolean":

                        //if the value is true we return "NDO_TRUE" else we return "NDO_FALSE"
                        return (bool)value ? NDO_TRUE : NDO_FALSE;

                    //we define the Netbasic_Dynamic_Object case
                    case "Netbasic_Dynamic_Object":

                        //if the type is "Netbasic_Dynamic_Object" then we return "NDO_OBJECT"
                        return NDO_OBJECT;

                    //we define the List case
                    case "List`1":
                    case "JArray":

                        //if the type is "List`1" then we return "NDO_ARRAY"
                        return NDO_ARRAY;

                    //our default is to throw an exception
                    default:

                        //if anything goes wrong when deserializing our JSON string, we throw an error 
                        throw new Exception("Invalid data type.");
                }
            }
            catch (NullReferenceException)
            {

                //if we catch a NullReferenceException it means that our value is null so we return "NDO_NULL"
                return NDO_NULL;
            }
        }

        //a public method that can be called to set the value of NDO options
        public static object NDOSetOption(string option, object value)
        {

            //we begin a try/catch
            try
            {

                //we initalize a type of this class
                Type controllerType = typeof(Netbasic_Dynamic_Object_Controller);

                //we reference our option using the option argument
                PropertyInfo ndoOption = controllerType.GetProperty(option);

                //we set the value of our property to the value passed as long as it is of type string or int
                ndoOption.SetValue(null, value.GetType().Name == "String" || value.GetType().Name == "Int32" || value.GetType().Name == "Int64" ? value : ndoOption.GetValue(null));
            }
            catch (Exception e)
            {

                //if there is an issue, we want to keep track of it
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDJSON, e.Message);

                return NDO_ERROR;
            }

            //we return success
            return NDO_SUCCESS;
        }

        //a public method that can be called to get the value of an NDO option
        public static object NDOGetOption(string option, out object value)
        {

            //we begin a try/catch
            try
            {

                //we initialize a type of this class
                Type controllerType = typeof(Netbasic_Dynamic_Object_Controller);

                //we reference our option using the option argument
                PropertyInfo ndoOption = controllerType.GetProperty(option);

                //we set our out value to the value of our ndoOption
                value = ndoOption.GetValue(null);
            }
            catch (Exception e)
            {

                //if there is an issue, we want to keep track of it
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDJSON, e.Message);

                value = null;

                return NDO_ERROR;
            }

            //success
            return NDO_SUCCESS;
        }

        //a public method that can be called to create a Netbasic_Dynamic_Object using a JSON or XML string
        public static object NDORead(string inputString, object inputType, out Netbasic_Dynamic_Object ndoHandle)
        {

            //we initialize our ndoHandle to a new, default Netbasic_Dynamic_Object
            ndoHandle = new Netbasic_Dynamic_Object();

            if (inputType.Equals(NDOFORMAT_JSON))
            {

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
                    LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDJSON, e.Message);

                    //and return false to indicate that we could not successfully create our Netbasic_Dynamic_Object
                    return NDO_ERROR;
                }
            }
            else if (inputType.Equals(NDOFORMAT_XML))
            {

                //if the inputType is 1 then we are working with an XML string *to be implemented in the future*
            }
            else
            {

                //if we are passed an output type that is not valid, then we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDFORMAT, "Invalid input type in NDORead function.");

                return NDO_ERROR;
            }

            //we return true to indicate that everything went smoothly
            return NDO_SUCCESS;
        }

        //a public method that can be called to serialize a Netbasic_Dynamic_Object to JSON or XML
        public static object NDOWrite(Netbasic_Dynamic_Object ndoHandle, object outputType, out string outputString)
        {

            //we initialize our outputString
            outputString = "";
            
            if (outputType.Equals(NDOFORMAT_JSON))
            {

                //we begin a try/catch block so that we can handle any potential errors
                try
                {

                    //we need to check what type our ndoHandle is
                    if (ndoHandle.DynamicObjectType.Equals(NDO_OBJECT))
                    {
                        //if the output type is 0 then we want to serialize our Netbasic_Dynamic_Object to JSON
                        //we initialize a temporary dictionary that will contain our raw properties without the NDO types
                        Dictionary<string, object> rawProperties = new Dictionary<string, object>();

                        //we iterate through the properties in our ndoHandle
                        foreach (KeyValuePair<string, object> ndoProperty in ndoHandle.DynamicObjectProperties)
                        {

                            //we extract the key value pair from within the rawProperty
                            KeyValuePair<object, object> ndoPropertyValue = (KeyValuePair<object, object>)ndoProperty.Value;

                            //we add the value of each key value pair to rawProperties
                            rawProperties.Add(ndoProperty.Key, ndoPropertyValue.Value);
                        }

                        //we set the outputString value to the serialized IDictionary
                        outputString = JsonConvert.SerializeObject(rawProperties);
                    }
                    else if (ndoHandle.DynamicObjectType.Equals(NDO_ARRAY))
                    {

                        //if this is an NDO_ARRAY, we just want to serialize the values
                        List<object> elementValues = new List<object>();

                        //we iterate through all of the NDO_ARRAY elements
                        foreach (KeyValuePair<object, object> element in ndoHandle.DynamicObject)
                        {

                            //we append the value of each element to our elementValues list
                            elementValues.Add(element.Value);
                        }

                        //we now serialize the list and set the result to our outputstring
                        outputString = JsonConvert.SerializeObject(elementValues);
                    }
                    else 
                    {

                        //if it's any other type we throw an error
                        throw new Exception("Invalid NDO type in NDOWrite function.");
                    }
                }
                catch (Exception e)
                {
                    //if there are any issues we want to output the error message but not throw the error so we can still return false
                    LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDFORMAT, e.Message);

                    //we return false to indicate that the serialization has not been successful
                    return NDO_ERROR;
                }
            }
            else if (outputType.Equals(NDOFORMAT_XML))
            {

                //if the outputType is 1 then we need to serialize our dynamic object to XML *to be implemented in the future*
            }
            else
            {

                //if we are passed an output type that is not valid, then we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDFORMAT, "Invalid output type in NDOWrite function.");

                return NDO_ERROR;
            }

            //we return true to indicate that everything completed successfully
            return NDO_SUCCESS;
        }

        //a method that can be called to set the value of a dynamic object property
        public static object NDOSetProperty(Netbasic_Dynamic_Object ndoHandle, string propName, object value, bool castToNum)
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
                        ndoHandle.DynamicObjectProperties[propName] = (Int32.TryParse(value.ToString(), out tempIntValue)) ? new KeyValuePair<object, object>(NDO_NUMBER, tempIntValue) : new KeyValuePair<object, object>(NDO_NUMBER, Convert.ToDouble(value.ToString()));
                    }
                    else
                    {

                        //if the property does not yet exist, then we want to add the property to our object
                        ndoHandle.DynamicObjectProperties.Add(propName, (Int32.TryParse(value.ToString(), out tempIntValue)) ? new KeyValuePair<object, object>(NDO_NUMBER, tempIntValue) : new KeyValuePair<object, object>(NDO_NUMBER, Convert.ToDouble(value.ToString())));
                    }
                }
                catch (Exception e)
                {

                    //if anything goes wrong when casting our value then we throw an error
                    LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDFORMAT, e.Message);

                    return NDO_ERROR;
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
                        ndoHandle.DynamicObjectProperties[propName] = new KeyValuePair<object, object>(GetNDODataType(value), value);
                    }
                    else
                    {

                        //if the property does not yet exist, then we want to add the property to our object
                        ndoHandle.DynamicObjectProperties.Add(propName, new KeyValuePair<object, object>(GetNDODataType(value), value));
                    }
                }
                catch (Exception e)
                {

                    //if for some reason we can't set the value we want to display an error message and return false
                    LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDTYPE, e.Message);

                    return NDO_ERROR;
                }
            }

            //if we have gotten to this point, then nothing has gone wrong so we return true to indicate that this method was run successfully
            return NDO_SUCCESS;
        }

        //a public method that can be called to return the value of a property of our dynamic object given the property name
        public static object NDOGetProperty(Netbasic_Dynamic_Object ndoHandle, string propName, out object value, out object valueType)
        {

            //we begin a try/catch block
            try
            {

                //we get the key value pair that is the value of the property
                KeyValuePair<object, object> propKeyValuePair = (KeyValuePair<object, object>)ndoHandle.DynamicObjectProperties[propName];

                //we set the out value_type argument to the key value pair key
                valueType = propKeyValuePair.Key;

                //we set the out value argument to the key value pair value
                value = propKeyValuePair.Value;

            }
            catch (Exception e)
            {

                //if we cannot reference the property given the propName then we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDVALUE, e.Message);

                //we set the out value argument to a new object
                value = new object();

                //we set the out value_type argument to an empty string
                valueType = -1;

                //we return false to indicate that something went wrong
                return NDO_ERROR;
            }

            //we return true to indicate that the processes of the method were carried out successfully
            return NDO_SUCCESS;
        }

        //a public method that can be called to delete a property from the property list of a dynamic object
        public static object NDODeleteProperty(Netbasic_Dynamic_Object ndoHandle, string propName)
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
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_INVALIDVALUE, e.Message);

                return NDO_ERROR;
            }

            //we return true here because everything has gone smoothly
            return NDO_SUCCESS;
        }

        //a public method that can be called to return a list of all dynamic object property names
        public static object NDOGetPropertyNames(Netbasic_Dynamic_Object ndoHandle, out List<string> propKeys)
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
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_NOTANOBJECT, e.Message);

                //we our out propKeys to an empty list
                propKeys = null;

                return NDO_ERROR;
            }

            //since everything has gone well, we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to return the value in a property list given the currentIndex prop of our dynamic object
        public static object NDOGetNextProperty(Netbasic_Dynamic_Object ndoHandle, out string propName, out object propValue, out object propType)
        {

            //we begin a try/catch block
            try
            {

                //we set our propName to the key of our property given the currentIndex property
                propName = ndoHandle.DynamicObjectProperties.ElementAt(ndoHandle.currentIndex).Key;

                //we set our propValue to the valur of our property given the currentIndex property
                KeyValuePair<object, object> propKeyValuePair = (KeyValuePair<object, object>)ndoHandle.DynamicObjectProperties.ElementAt(ndoHandle.currentIndex).Value;

                //we set our propType to the key of our key value pair
                propType = propKeyValuePair.Key;

                //we set our propValue to the value of our key value pair
                propValue = propKeyValuePair.Value;

                //we increment our currentIndex
                ndoHandle.currentIndex++;
            }
            catch (ArgumentOutOfRangeException e)
            {

                //if we are referencing an element out of range then we want to let display an error message, set the currentIndex back to zero and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_OUTOFBOUND, e.Message);

                propName = "";

                propValue = new object();

                propType = -1;

                ndoHandle.currentIndex = 0;

                return NDO_ERROR;
            }
            catch (Exception e)
            {

                //if there are any other issues, then we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                propName = "";

                propValue = new object();

                propType = -1;

                return NDO_ERROR;
            }

            //because everything has completed successfully, we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to instantiate a Netbasic_Dynamic_Object of a given type
        public static object NDOCreate(object ndoType, out Netbasic_Dynamic_Object ndoHandle)
        {
 
            //we define the case where the type is "NDO_OBJECT"
            if (ndoType.Equals(NDO_OBJECT))
            {
                //if the type is NDO_OBJECT, then we simply instantiate a new, empty dynamic object as we would with our default constructor 
                ndoHandle = new Netbasic_Dynamic_Object();

                //we return success
                return NDO_SUCCESS;

            }
            else if (ndoType.Equals(NDO_ARRAY))
            {
                //we instantiate a new ndo
                ndoHandle = new Netbasic_Dynamic_Object();

                //if the type is NDO_ARRAY we want to instantiate our DynamicObject as a generic list
                ndoHandle.DynamicObject = new List<object>();

                //we set our DynamicObjectType
                ndoHandle.DynamicObjectType = NDO_ARRAY;

                //we also initialize our currentIndex for iterative functionality
                ndoHandle.currentIndex = 0;

                //we return success
                return NDO_SUCCESS;
            }
            else if (ndoType.Equals(NDO_TRUE))
            {

                //we instantiate a new ndo
                ndoHandle = new Netbasic_Dynamic_Object();

                //we instantiate our DynamicObject as a boolean with the value 'true'
                ndoHandle.DynamicObject = true;

                //we set our DynamicObjectType
                ndoHandle.DynamicObjectType = NDO_TRUE;

                //we return success
                return NDO_SUCCESS;
            }
            else if (ndoType.Equals(NDO_FALSE))
            {

                //we instantiate a new ndo
                ndoHandle = new Netbasic_Dynamic_Object();

                //we instantiate our DynamicObject as a boolean with the value 'false'
                ndoHandle.DynamicObject = false;

                //we set our DynamicObjectType
                ndoHandle.DynamicObjectType = NDO_FALSE;

                //we return success
                return NDO_SUCCESS;
            }
            else
            {

                //we instantiate a new ndo
                ndoHandle = new Netbasic_Dynamic_Object();

                //we instantiate our DynamicObject as null
                ndoHandle.DynamicObject = null;

                //we set our DynamicObjectType
                ndoHandle.DynamicObjectType = NDO_NULL;

                //we return success
                return NDO_SUCCESS;
            }
        }

        //a public method that can be called to get the type of a Netbasic_Dynamic_Object
        public static object NDOGetType(Netbasic_Dynamic_Object ndoHandle, out object type)
        {

            //we set the value of our out argument type to our the DynamicObjectType of our ndoHandle
            type = ndoHandle.DynamicObjectType;

            //we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to test against the type of a Netbasic_Dynamic_Object
        public static object NDOIsTypeOf(Netbasic_Dynamic_Object ndoHandle, object type)
        {

            //if the type argument matches the DynamicObjectType then we return true, else false
            return (ndoHandle.DynamicObjectType.Equals(type)) ? NDO_TRUE : NDO_FALSE;
        }

        //a public function that can be called to return the size of a NDO_ARRAY type NetbasicDynamicObject
        public static object NDOArrayGetSize(Netbasic_Dynamic_Object ndoHandle, out int size)
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
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_NOTANARRAY, e.Message);

                //we set the size to 0
                size = 0;

                return NDO_ERROR;
            }

            //success
            return NDO_SUCCESS;
        }

        //a public method that can be called to set the value of an element in an NDO_ARRAY
        public static object NDOArraySetItem(Netbasic_Dynamic_Object ndoHandle, int index, object value)
        {

            //we begin a try/catch
            try
            {
                
                //we check to see if the index is greater than the size of the NDO_ARRAY
                if (index > ndoHandle.DynamicObject.Count)
                {

                    //we initialize an int with our padding size
                    int padding = (index - 1) - ndoHandle.DynamicObject.Count;

                    //we loop from 0 to the padding size we calculate
                    for (int i = 0; i < padding; i++)
                    {

                        //we call our append method to add NDO_NULLS to the NDO_ARRAY
                        NDOArrayAppendItem(ndoHandle, null);
                    }

                    //now we append our value to end of our NDO_ARRAY
                    NDOArrayAppendItem(ndoHandle, value);
                }
                else
                {

                    //if the index is less than the count of the NDO_ARRAY then we want to set the value at the index to the new value passed here
                    ndoHandle.DynamicObject[index - 1] = new KeyValuePair<object, object>(GetNDODataType(value), value);
                }
            }
            catch (Exception e)
            {

                //if for some reason we can't set the value we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                return NDO_ERROR;
            }

            //if everything goes smoothly, we want to return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to get an item in a NDO_ARRAY given the index
        public static object NDOArrayGetItem(Netbasic_Dynamic_Object ndoHandle, int index, out object value, out object type)
        {

            //we begin a try/catch block
            try
            {

                //we create a temp key value pair
                KeyValuePair<object, object> itemKeyValuePair = ndoHandle.DynamicObject[index - 1];

                //we set the out type argument to the key of the item
                type = itemKeyValuePair.Key;

                //we set our out value argument to the value of the element at the index - 1 in our ndoHandle.DynamicObject
                value = itemKeyValuePair.Value;
            }
            catch (Exception e)
            {

                //if there is an issue, we display an error message, set our out value to an empty object and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                type = -1;

                value = new object();

                return NDO_ERROR;
            }

            //since the element was set successfully, we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to insert an element into an NDO_ARRAY
        public static object NDOArrayInsertItem(Netbasic_Dynamic_Object ndoHandle, int index, object value)
        {

            //we begin a try/catch
            try
            {

                //we check to see if the index is greater than the size of the NDO_ARRAY
                if (index > ndoHandle.DynamicObject.Count)
                {

                    //we initialize an int with our padding size
                    int padding = (index - 1) - ndoHandle.DynamicObject.Count;

                    //we loop from 0 to the padding size we calculate
                    for (int i = 0; i < padding; i++)
                    {

                        //we call our append method to add NDO_NULLS to the NDO_ARRAY
                        NDOArrayAppendItem(ndoHandle, null);
                    }

                    //now we append our value to end of our NDO_ARRAY
                    NDOArrayAppendItem(ndoHandle, value);
                }
                else
                {

                    //if the index is less than the count of the NDO_ARRAY then we want to insert the value at the index passed to this method
                    ndoHandle.DynamicObject.Insert(index - 1, new KeyValuePair<object, object>(GetNDODataType(value), value));
                }
            }
            catch (Exception e)
            {

                //if for some reason we can't set the value we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                return NDO_ERROR;
            }

            //if everything goes smoothly, we want to return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to get the next available element in an NDO_ARRAY based on the currentIndex prop
        public static object NDOArrayGetNextItem(Netbasic_Dynamic_Object ndoHandle, out object value, out object type)
        {

            //we begin a try/catch block
            try
            {

                //we set our value to the value of our NDO_ARRAY element given the currentIndex property
                KeyValuePair<object, object> elementKeyValuePair = (KeyValuePair<object, object>)ndoHandle.DynamicObject[ndoHandle.currentIndex];

                //we set our type to the key of our key value pair
                type = elementKeyValuePair.Key;

                //we set our value to the value of our key value pair
                value = elementKeyValuePair.Value;

                //we increment our currentIndex
                ndoHandle.currentIndex++;
            }
            catch (ArgumentOutOfRangeException e)
            {

                //if we are referencing an element out of range then we want to let display an error message, set the currentIndex back to zero and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_OUTOFBOUND, e.Message);

                value = new object();

                type = -1;

                ndoHandle.currentIndex = 0;

                return NDO_ERROR;
            }
            catch (Exception e)
            {

                //if there are any other issues, then we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                value = new object();

                type = -1;

                return NDO_ERROR;
            }

            //because everything has completed successfully, we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to append an element to an NDO_ARRAY
        public static object NDOArrayAppendItem(Netbasic_Dynamic_Object ndoHandle, object value)
        {

            //we begin a try/catch block
            try
            {

                //we append a key value pair to the NDO_ARRAY with the value passed and the ndo type of that value
                ndoHandle.DynamicObject.Add(new KeyValuePair<object, object>(GetNDODataType(value), value));
            }
            catch (Exception e)
            {

                //if there is an issue for whatever reason, we want to display an error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                return NDO_ERROR;
            }

            //we return true because everything has been completed successfully
            return NDO_SUCCESS;
        }
        
        //a public method that can be called to remove an element from an NDO_ARRAY
        public static object NDOArrayDeleteItem(Netbasic_Dynamic_Object ndoHandle, int index)
        {

            //we begin a try/catch block
            try
            {

                //we remove the element from the ndoHandle NDO_ARRAY given index - 1
                ndoHandle.DynamicObject.RemoveAt(index - 1);
            }
            catch (Exception e)
            {

                //if there is an issue, we want to display and error message and return false
                LAST_ERROR = new KeyValuePair<object, string>(NDOERROR_EXCEPTION, e.Message);

                return NDO_ERROR;
            }

            //because everything went smoothly, we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to clone a Netbasic_Dynamic_Object
        public static object NDOClone(Netbasic_Dynamic_Object ndoHandle, out Netbasic_Dynamic_Object newNdoHandle)
        {

            //we set the newNdoHandle to the ndoHandle
            newNdoHandle = ndoHandle;

            //we return true
            return NDO_SUCCESS;
        }

        //a public method that can be called to get the code and message of the last error that was thrown
        public static object NDOGetLastError(out object errorCode, out string errorMessage)
        {

            //we set our out values to the values currently in our LAST_ERROR key value pair
            errorCode = LAST_ERROR.Key;

            errorMessage = LAST_ERROR.Value;

            //we return success
            return NDO_SUCCESS;
        }
    }
}
