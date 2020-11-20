namespace HellsChicken.Scripts.Game.AI.DecisionTree
{
    public interface DTNode { 
        DTAction Walk();
    }

    public delegate object DTCall ();
}