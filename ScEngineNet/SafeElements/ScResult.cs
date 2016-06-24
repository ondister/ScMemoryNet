
namespace ScEngineNet.SafeElements
{
   public enum ScResult
    {
    SC_RESULT_ERROR = 0,                // unknown error
    SC_RESULT_OK = 1,                   // no any error
    SC_RESULT_ERROR_INVALID_PARAMS = 3, // invalid function parameters error
    SC_RESULT_ERROR_INVALID_TYPE = 5,   // invalied type error
    SC_RESULT_ERROR_IO = 7,             // input/output error
    SC_RESULT_ERROR_INVALID_STATE = 9,  // invalid state of processed object
    SC_RESULT_ERROR_NOT_FOUND = 11,     // item not found
    SC_RESULT_ERROR_NO_WRITE_RIGHTS = 2,// no ritghs to change or delete object
    SC_RESULT_ERROR_NO_READ_RIGHTS = 4, // no ritghs to read object
    SC_RESULT_ERROR_NO_RIGHTS = SC_RESULT_ERROR_NO_WRITE_RIGHTS | SC_RESULT_ERROR_NO_READ_RIGHTS
    }
}
