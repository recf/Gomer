using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Gomer
{
    public abstract class ViewModel : ReactiveObject
    {
        [Reactive]
        public string Title { get; set; }

        public virtual void Refresh()
        {
        }
    }
}
