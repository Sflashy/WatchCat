using System.Collections.ObjectModel;

namespace WatchCat.Core.Extensions;

public static class ToObservableCollectionExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return new ObservableCollection<T>(source);
    }
}
