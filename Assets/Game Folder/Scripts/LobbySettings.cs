using UnityEngine;
using System.Collections;

public static class LobbySettings
{
    private enum MatchTimeOfDay { Daytime, Nighttime };
    private enum PlayerCount { OneVOne, TwoVTwo };
    private enum CooldownModifiers { Standard, Short, Long };
    private enum GoalsToWin { Five, One, Two, Ten, Fifteen, Twenty };
    private enum MatchTime { FiveMinutes, TwoMinutes, TenMinutes, FifteenMinutes}
}
