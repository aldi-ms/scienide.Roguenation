namespace scienide.Common.Infrastructure;

using System.Runtime.CompilerServices;

public enum Props : uint
{
    None            = 1 << 0,
    IsOpaque        = 1 << 1,
    IsVisible       = 1 << 2,
    IsFloodFilled   = 1 << 3,
    HasBeenSeen     = 1 << 4,
    IsWalkable      = 1 << 5,
    Highlight       = 1 << 6,
}

public class BitProperties
{
    private uint _props;

    public BitProperties()
    {
        _props = 0;
    }

    public bool this[Props prop]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_props & (uint)prop) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (value)
            {
                _props |= (uint)prop;
            }
            else
            {
                _props &= ~(uint)prop;
            }
        }
    }
}
