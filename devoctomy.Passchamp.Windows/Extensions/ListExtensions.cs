using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Windows.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }
}
