//Script name should be what it does (PlayerMovement,InventoryManager).
using Unity.VisualScripting.FullSerializer;

public class ExampleScript 
{
    //Private variables.
    private float _variableName;
    //Public variables.
    public float VariableName;
    //Constants.
    const float VARIABLENAME = 0F;

    //Functions should be named after what they do (MovePlayer,PickUpItem).
    private void FunctionName(float variableName)
    {
        //You don't need to add function before the variable name.
        float functionVariableName = 0f;
        float warningStopper = functionVariableName;
    }
}
