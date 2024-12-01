using System.Runtime.CompilerServices;

namespace scienide.Common.Infrastructure;

public enum Props : uint
{
    None          = 1 << 0,
    IsOpaque      = 1 << 1,
    IsVisible     = 1 << 2,
    IsFloodFilled = 1 << 3,
    HasBeenSeen   = 1 << 4,
}

public class BitProperties
{
    private uint _props = 0;

    public bool this[Props prop]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_props & 1 << (int)prop) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            int bitPos = (int)prop;
            if (value)
            {
                _props |= 1u << bitPos;
            }
            else
            {
                _props &= ~(1u << bitPos);
            }
        }
    }
}
