public struct CineConfig
{
    public static CineConfig Left = new CineConfig(2, 0, -90);
    public static CineConfig Right = new CineConfig(-2, 0, 90);
    public static CineConfig Down = new CineConfig(0, -2, 0);
    public static CineConfig Up = new CineConfig(0, 2, 180);

    public readonly float OffsetX => _offsetX;
    public readonly float OffsetZ => _offsetZ;
    public readonly float RotationY => _rotationY;

    private float _offsetX;
    private float _offsetZ;
    private float _rotationY;

    public CineConfig(float x, float z, float rotY)
    {
        _offsetX = x;
        _offsetZ = z;
        _rotationY = rotY;
    }
}