namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public class DTAction : IDTNode 
    {
        public readonly DTCall Action;
    
        public DTAction (DTCall action) 
        {
            Action = action;
        }

        public DTAction Walk() 
        {
            return this;
        }
    }
}
