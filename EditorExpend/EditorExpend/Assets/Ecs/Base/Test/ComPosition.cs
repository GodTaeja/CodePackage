public class ComPosition : MComponent
{
    public TrueSync.TSVector3 LastPosition { get; set; }

    private TrueSync.TSVector3 _position = new TrueSync.TSVector3(-99999, 0, 0);

    public bool HasFirstSyncRotation = false;
    public TrueSync.TSVector3 Position
    {
        get => _position;
        set
        {
            if (_position.x <= -99998)
            {
                LastPosition = value; //首次设置pos的时候，lastPos和pos设置成相同的值
            }
            else
            {
                LastPosition = _position;
            }
            _position = value;
        }
    }
}
