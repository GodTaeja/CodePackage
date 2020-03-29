
using TrueSync;

public class ComMovement  : MComponent
{
    public FP MoveSpeed { get; set; }
    public TrueSync.TSVector3 MoveDirection { get; set; }

    public bool HasTarget{ get; set; }

    public TrueSync.TSVector3 MoveTarget { get; set; }

    public bool IsArrived { get; set; }

    public void Clear()
    {
        MoveSpeed = 0;
        MoveDirection = TrueSync.TSVector3.zero;
        HasTarget = false;
        MoveTarget = TrueSync.TSVector3.zero;
    }







}
