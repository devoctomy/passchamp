using devoctomy.Passchamp.Core.Graph;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Windows.Model
{
    public class GraphTesterModel : ModelBase
    {
        private IGraph _graph;

        private List<IPin> _pins;

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

        public List<IPin> Pins
        {
            get
            {
                if(_pins == null)
                {
                    _pins = Graph?.Pins.Values.ToList();
                }

                return _pins;
            }
        }
    }
}
