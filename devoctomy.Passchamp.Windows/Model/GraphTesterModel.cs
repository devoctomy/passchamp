using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.Extensions;
using System.Collections.ObjectModel;
using System.Linq;

namespace devoctomy.Passchamp.Windows.Model
{
    public class GraphTesterModel : ModelBase
    {
        private IGraph _graph;

        private ObservableCollection<IPin> _pins;

        public IGraph Graph
        { 
            get
            {
                return _graph;
            }
            set
            {
                if(_graph != value)
                {
                    _graph = value;
                    OnPropertyChanged(nameof(Graph));
                    OnPropertyChanged(nameof(Pins));
                }
            }
        }

        public ObservableCollection<IPin> Pins
        {
            get
            {
                if(_pins == null)
                {
                    _pins = Graph?.Pins.Values.ToList().ToObservableCollection();
                }

                return _pins;
            }
        }
    }
}
