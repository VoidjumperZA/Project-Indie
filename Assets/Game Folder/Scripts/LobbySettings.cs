using UnityEngine;
using System.Collections;

public static class LobbySettings
{
    private enum MatchTimeOfDay { Daytime, Nighttime };
    private enum CooldownModifiers { Standard, Short, Long };
    //private enum GoalsToWin { Five, One, Two, Ten, Fifteen, Twenty };
    //private enum MatchTime { FiveMinutes, TwoMinutes, TenMinutes, FifteenMinutes}

    private static int team_1PlayerCount;
    private static int team_2PlayerCount;
    private static int numberOfPlayers;
    private static int matchTimeInMinutes;
    private static int goalsToWin;
    private static int sceneToLoad;
    private static float modifierValue;

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

    public static void SetArena(string pArenaName)
    {
        Debug.Log("The give arena name is " + pArenaName);
        if (pArenaName == "Sunlight")
        {
            sceneToLoad = 1;
            Debug.Log("Sunlight Arena has been set with a scene ID of " + sceneToLoad);
        }
        else if (pArenaName == "Moonbeam")
        {
            sceneToLoad = 2;
            Debug.Log("Moonbeam Arena has been set with a scene ID of " + sceneToLoad);
        }
        else
        {
            Debug.Log("An incorrect arena name has been entered as an argument to the button.");
            sceneToLoad = 0; //Defaults back to loading the menu scene. If this bug is ever left untouched, at least the menu
            //will load and it won't crash
        }
    }

    public static void SetCooldownModifier(float pModifierValue)
    {
        modifierValue = pModifierValue;
    }

    public static int GetSceneToLoad()
    {
        return sceneToLoad;
    }

    public static void SetMatchTimeInMinutes(int pMinutes)
    {
        matchTimeInMinutes = pMinutes;
    }

    public static void SetGoalsToWin(int pGoalsToWin)
    {
        goalsToWin = pGoalsToWin;
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

    public static int GetMatchTimeInMinutes()
    {
        return matchTimeInMinutes;
    }

    public static int GetGoalsToWin()
    {
        return goalsToWin;
    }

    public static float GetModifierValue()
    {
        return modifierValue;
    }

    public static void ResetLobbySettingsData()
    {
        team_1PlayerCount = 0;
        team_2PlayerCount = 0;
    }
}

