namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public interface IDTNode { 
        DTAction Walk();
    }

    public delegate object DTCall ();
}