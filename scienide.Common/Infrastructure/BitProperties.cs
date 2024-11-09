namespace scienide.Common.Infrastructure;

public enum Props : uint
{
    None = 1 << 0,
    IsOpaque = 1 << 1,
    IsVisible = 1 << 2
}

public class BitProperties
{
    private uint _props = 0;

    public bool this[Props prop]
    {
        get => GetProperty(prop);
        set => SetProperty(prop, value);
    }

    public bool GetProperty(Props prop)
    {
        return IsBitSet((int)prop);
    }

    public void SetProperty(Props prop, bool value)
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
