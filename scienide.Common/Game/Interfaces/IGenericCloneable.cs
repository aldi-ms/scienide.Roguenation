namespace scienide.Common.Game.Interfaces;

public interface IGenericCloneable<T>
{
    T Clone(bool deepClone);
}
