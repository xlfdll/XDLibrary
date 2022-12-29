using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Xlfdll.Collections
{
    public class NotifiableObservableCollection<T> : ObservableCollection<T>
    {
        public void Refresh()
        {
            for (Int32 i = 0; i < this.Count; i++)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}