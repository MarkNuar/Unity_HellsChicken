using System.Collections.Generic;

namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public class DTDecision : DTNode {

        private DTCall _selector;

        private Dictionary<object, DTNode> _links;

        public DTDecision(DTCall selector) {
            this._selector = selector;
            _links = new Dictionary<object, DTNode>();
        }
    
        public DTAction Walk() {
            object o = _selector();
            return _links.ContainsKey(o) ? _links[o].Walk() : null;
        }

        public void AddLink(object key, DTNode value) {
            _links.Add(key,value);
        }
    
    }
}
