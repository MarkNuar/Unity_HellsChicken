namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public class DecisionTree {
        private readonly IDTNode _root;

        public DecisionTree(IDTNode root) {
            _root = root;
        }

        public object Start() {
            DTAction result = _root.Walk();
            if (result != null) 
                return result.Action();
            return null;
        }

    }
}
