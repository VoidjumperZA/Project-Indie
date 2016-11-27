using UnityEngine;
using System.Collections;

public static class LobbySettings
{
    private enum MatchTimeOfDay { Daytime, Nighttime };
    private enum CooldownModifiers { Standard, Short, Long };
    private enum GoalsToWin { Five, One, Two, Ten, Fifteen, Twenty };
    private enum MatchTime { FiveMinutes, TwoMinutes, TenMinutes, FifteenMinutes}

    private static int team_1PlayerCount;
    private static int team_2PlayerCount;
    private static int numberOfPlayers;

    public static void SetTeamPlayerCount(int pTeam, int pPlayerCount)
    {
        if (pTeam == 1)
        {
            team_1PlayerCount = pPlayerCount;
            numberOfPlayers += pPlayerCount;
        }
        if (pTeam == 2)
        {
            team_2PlayerCount = pPlayerCount;
            numberOfPlayers += pPlayerCount;
        }
    }

    public static void ResetNumberOfPlayers()
    {
        numberOfPlayers = 0;
    }

    public static int GetNumberOfPlayers()
    {
        return numberOfPlayers;
    }

    public static int GetTeam_1PlayerCount()
    {
        return team_1PlayerCount;
    }

    public static int GetTeam_2PlayerCount()
    {
        return team_2PlayerCount;
    }



    public static void ResetLobbySettingsData()
    {
        team_1PlayerCount = 0;
        team_2PlayerCount = 0;
    }
}

