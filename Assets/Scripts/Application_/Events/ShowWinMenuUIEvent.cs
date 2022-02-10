using Application_.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHasWonEvent", menuName = "Events/Game/PlayerHasWonEvent")]
public class ShowWinMenuUIEvent : GameEventScriptable
{
    public int level;
}