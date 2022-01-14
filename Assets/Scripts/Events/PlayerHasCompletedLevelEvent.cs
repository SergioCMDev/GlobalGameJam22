using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHasCompletedLevelEvent", menuName = "Events/Level/PlayerHasCompletedLevelEvent")]
public class PlayerHasCompletedLevelEvent : GameEventScriptable
{
    public int level;
}