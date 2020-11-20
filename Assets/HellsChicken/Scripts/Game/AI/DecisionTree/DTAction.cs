namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public class DTAction : DTNode {

        public DTCall Action;
    
        public DTAction (DTCall action) {
            Action = action;
        }

        public DTAction Walk() {
            return this;
        }
    }
}
