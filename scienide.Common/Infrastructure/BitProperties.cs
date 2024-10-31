namespace scienide.Common.Infrastructure;

public enum NamedBits
{
    None = 0,
}

public class BitProperties
{
    private uint _props = 0;

    private void SetBit(int bitPos)
    {
        _props |= 1u << bitPos;
    }

    private bool IsBitSet(int bitPos)
    {
        return (_props & 1 << bitPos) != 0;
    }
}
