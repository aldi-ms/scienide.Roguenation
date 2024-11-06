namespace scienide.Common.Infrastructure;

public enum NamedBits : uint
{
    None = 1 << 0,
    BlocksLight = 1 << 1,
    IsVisible = 1 << 2
}

public class BitProperties
{
    private uint _props = 0;

    public bool this[NamedBits prop]
    {
        get => GetProperty(prop);
        set => SetProperty(prop, value);
    }

    public bool GetProperty(NamedBits prop)
    {
        return IsBitSet((int)prop);
    }

    public void SetProperty(NamedBits prop, bool value)
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

    private bool IsBitSet(int bitPos)
    {
        return (_props & 1 << bitPos) != 0;
    }
}
