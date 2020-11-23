using System.Collections.Generic;

namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public class DTDecision : IDTNode 
    {
        private readonly DTCall _selector;

        private readonly Dictionary<object, IDTNode> _links;

        public DTDecision(DTCall selector) 
        {
            _selector = selector;
            _links = new Dictionary<object, IDTNode>();
        }
    
        public DTAction Walk() 
        {
            object o = _selector();
            return _links.ContainsKey(o) ? _links[o].Walk() : null;
        }

        public void AddLink(object key, IDTNode value) 
        {
            _links.Add(key,value);
        }
    }
}
