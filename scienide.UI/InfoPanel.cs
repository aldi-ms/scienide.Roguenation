namespace scienide.UI;

using SadConsole;
using scienide.Engine.Core.Messaging;

public class InfoPanel
{
    private ICellSurface _surface;
    
    public InfoPanel(ICellSurface surface)
    {
        _surface = surface;
        MessageBroker.Instance.Subscribe(SelectedCellChanged)
    }
}
