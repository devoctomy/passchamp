using devoctomy.Passchamp.Core.Graph;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace devoctomy.Passchamp.Windows.Model
{
    public class GraphTesterModel : ModelBase
    {
        private IGraph _graph;

        private List<IPin> _inputPins;
        private List<INode> _nodes;

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
                    OnPropertyChanged(nameof(InputPins));
                    OnPropertyChanged(nameof(Nodes));
                }
            }
        }

        public List<IPin> InputPins
        {
            get
            {
                if(_inputPins == null)
                {
                    _inputPins = Graph?.InputPins.Values.ToList();
                }

                return _inputPins;
            }
        }

        public List<INode> Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = Graph?.Nodes.Values.ToList();
                }

                return _nodes;
            }
        }

        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
    }
}
