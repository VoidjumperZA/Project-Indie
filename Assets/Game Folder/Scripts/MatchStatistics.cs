using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MatchStatistics
{
    private static int T1_Score;
    private static int T2_Score;
    private static Dictionary<int, int> matchGoals = new Dictionary<int, int>();            //(Team,   Goals)
    private static Dictionary<int, int> teamInfo = new Dictionary<int, int>();              //(Player, Team)
    private static Dictionary<int, int> goalsScored = new Dictionary<int, int>();           //(Player, Goals Scored)
    private static Dictionary<int, int> playersSquished = new Dictionary<int, int>();       //(Player, Number of enemies squished)
    private static Dictionary<int, int> playersDropped = new Dictionary<int, int>();        //(Player, Number of enemies dropped)
    private static Dictionary<int, int> deaths = new Dictionary<int, int>();                //(Player, Deaths)
   
    /// <summary>
    /// Should be called on match creation. Creats a dictionary to store goals for Team 1 and Team 2.
    /// </summary>
    public static void IntialiseGoalTracking()
    {
        matchGoals.Add(1, 0);
        matchGoals.Add(2, 0);
    }

    //assigns a player to a team
    /// <summary>
    /// Assigns a player to a team. Automatically generates internal dictionary entries for that player's stats.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <param name="pTeamID"></param>
    public static void AssignPlayerToTeam(int pPlayerID, int pTeamID)
    {
        teamInfo.Add(pPlayerID, pTeamID);

        goalsScored.Add(pPlayerID, 0);
        playersSquished.Add(pPlayerID, 0);
        playersDropped.Add(pPlayerID, 0);
        deaths.Add(pPlayerID, 0);
    }

    //adds a death to the tally for the spesified player
    /// <summary>
    /// Adds a death to the tally for the spesified player.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerDeath(int pPlayerID)
    {
        IncrementItemByOne(deaths, pPlayerID);
    }

    //adds a goal to the tally for the spesified player
    /// <summary>
    /// Adds a goal to the tally for the spesified player.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerGoal(int pPlayerID)
    {
        IncrementItemByOne(goalsScored, pPlayerID);

        //add a goal for the team of that player
        int playerTeam;
        teamInfo.TryGetValue(pPlayerID, out playerTeam);
        IncrementItemByOne(matchGoals, playerTeam);
    }

    //adds a count to the tally of squished players for the spesified player
    /// <summary>
    /// Adds a count to the tally of squished players for the spesified player.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerSquished(int pPlayerID)
    {
        IncrementItemByOne(playersSquished, pPlayerID);
    }

    //adds a count to the tally of dropped players for the spesified player
    /// <summary>
    /// Adds a count to the tally of dropped players for the spesified player.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerDropped(int pPlayerID)
    {
        IncrementItemByOne(playersDropped, pPlayerID);
    }


    //returns the number of goals that team has
    /// <summary>
    /// Returns the number of goals that team has.
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <returns></returns>
    public static int GetGoalsForTeam(int pTeamID)
    {
        return ReturnDictionaryValue(matchGoals, pTeamID);
    }

    //returns a vec2 of the goals of both teams
    /// <summary>
    /// Returns a Vector2 with the x element being Team 1's goals and the y being Team 2's.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetMatchGoals()
    {
        //this could be cleaned up instead of hardtyping values
        Vector2 goals;
        goals.x = ReturnDictionaryValue(matchGoals, 1);
        goals.y = ReturnDictionaryValue(matchGoals, 2);
        return goals;
    }

    //returns the number of times that player has died
    /// <summary>
    /// Returns the number of times that player has died.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetPlayerDeaths(int pPlayerID)
    {
        return ReturnDictionaryValue(deaths, pPlayerID);
    }

    //returns the number of players that player has squished
    /// <summary>
    /// Returns the number of players that player has squished.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetPlayersSquished(int pPlayerID)
    {
        return ReturnDictionaryValue(playersSquished, pPlayerID);
    }

    //returns the number of players that player has dropped
    /// <summary>
    /// Returns the number of players that player has dropped.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetPlayersDropped(int pPlayerID)
    {
        return ReturnDictionaryValue(playersDropped, pPlayerID);
    }

    //returns the goals scored by that player
    /// <summary>
    /// Returns the goals scored by that player.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetPlayerGoals(int pPlayerID)
    {
        return ReturnDictionaryValue(goalsScored, pPlayerID);
    }

    /// <summary>
    /// Returns the team the given player is on.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetTeamIDofPlayer(int pPlayerID)
    {
        return ReturnDictionaryValue(teamInfo, pPlayerID);
    }

    //internal method for incrementing a value of a spesified statistic
    private static void IncrementItemByOne(Dictionary<int, int> pDictionary, int pKey)
    {
        int numberOfItems;
        pDictionary.TryGetValue(pKey, out numberOfItems);
        pDictionary[pKey] = numberOfItems + 1;
    }

    private static int ReturnDictionaryValue(Dictionary<int, int> pDictionary, int pKey)
    {
        int value;
        pDictionary.TryGetValue(pKey, out value);
        return value;
    }
}
