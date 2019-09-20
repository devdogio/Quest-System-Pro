using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Chooses a random edge at run-time.")]
    [Category("Devdog/Random edge node")]
    public class RandomEdgeNode : ActionNodeBase
    {
        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            // TODO: Random edge chosen currently doesn't abbide the edge conditions (always succeeds)
            var edge = edges[UnityEngine.Random.Range(0, edges.Length)];
            Finish(owner.nodes[edge.toNodeIndex]);
        }

        public override ValidationInfo Validate()
        {
            return base.Validate();
        }
    }
}