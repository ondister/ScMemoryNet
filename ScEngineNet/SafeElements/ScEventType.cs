namespace ScEngineNet.SafeElements
{
    public enum ScEventType
    {
        SC_EVENT_UNKNOWN = -1,
        SC_EVENT_ADD_OUTPUT_ARC = 0,
        SC_EVENT_ADD_INPUT_ARC = 1,
        SC_EVENT_REMOVE_OUTPUT_ARC = 2,
        SC_EVENT_REMOVE_INPUT_ARC = 3,
        SC_EVENT_REMOVE_ELEMENT = 4,
        SC_EVENT_CONTENT_CHANGED = 5
    }
}
