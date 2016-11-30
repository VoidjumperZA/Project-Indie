using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MatchStatistics
{
    private static int T1_Score;
    private static int T2_Score;
    private static Dictionary<int, int> matchGoals = new Dictionary<int, int>();            //(Team,   Goals)
    private static Dictionary<int, int> lifeFireLeft = new Dictionary<int, int>();          //(Team,   LifeFire)
    private static Dictionary<int, int> teamInfo = new Dictionary<int, int>();              //(Player, Team)
    private static Dictionary<int, int> goalsScored = new Dictionary<int, int>();           //(Player, Goals Scored)
    private static Dictionary<int, int> playersSquished = new Dictionary<int, int>();       //(Player, Number of enemies squished)
    private static Dictionary<int, int> playersDropped = new Dictionary<int, int>();        //(Player, Number of enemies dropped)
    private static Dictionary<int, int> deaths = new Dictionary<int, int>();                //(Player, Deaths)
    private static Dictionary<int, int> assists = new Dictionary<int, int>();               //(Player, Number of Assists)
    private static Dictionary<int, float> playerPossession = new Dictionary<int, float>();      //(Player, % of ball possession)
    private static Dictionary<int, float> teamPossession = new Dictionary<int, float>();        //(Team, % of ball possession)
    private static int matchTimeInMinutes;

    /// <summary>
    /// Should be called on match creation. Creats a dictionary to store goals for Team 1 and Team 2.
    /// </summary>
    public static void IntialiseGoalTracking()
    {
        matchGoals.Add(1, 0);
        matchGoals.Add(2, 0);

        lifeFireLeft.Add(1, LobbySettings.GetGoalsToWin());
        lifeFireLeft.Add(2, LobbySettings.GetGoalsToWin());

        matchTimeInMinutes = LobbySettings.GetMatchTimeInMinutes();
        Debug.Log("MatchStatistics matchTime set from LobbySettings to a value of " + matchTimeInMinutes);
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
        assists.Add(pPlayerID, 0);
        playerPossession.Add(pPlayerID, 0.0f);
        teamPossession.Add(pPlayerID, 0.0f);
    }

    //adds a death to the tally for the spesified player
    /// <summary>
    /// Adds a death to the tally for the spesified player. K: Player | V: Deaths
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerDeath(int pPlayerID)
    {
        IncrementItemByOne(deaths, pPlayerID);
    }

    //adds a goal to the tally for the spesified player
    /// <summary>
    /// Adds a goal to the tally for the spesified player. K: Player | V: Goals Scored
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerGoal(int pPlayerID, int pGoalScoredInOwnerID)
    {
        IncrementItemByOne(goalsScored, pPlayerID);

        //add a goal for the team of that player
        int playerTeam;
        teamInfo.TryGetValue(pPlayerID, out playerTeam);
        IncrementItemByOne(matchGoals, playerTeam);
        ReduceItemByOne(lifeFireLeft, pGoalScoredInOwnerID);
    }

    /// <summary>
    /// Add a goal to a team with no player getting the attributation for the goal. 
    /// </summary>
    /// <param name="pTeamID"></param>
    public static void AddUnattributedGoal(int pTeamID, int pEnemyTeamID)
    {
        IncrementItemByOne(matchGoals, pTeamID);
        ReduceItemByOne(lifeFireLeft, pEnemyTeamID);
    }

    //adds a count to the tally of squished players for the spesified player
    /// <summary>
    /// Adds a count to the tally of squished players for the spesified player. K: Player | V: No. Players Squished
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerSquished(int pPlayerID)
    {
        IncrementItemByOne(playersSquished, pPlayerID);
    }

    //adds a count to the tally of dropped players for the spesified player
    /// <summary>
    /// Adds a count to the tally of dropped players for the spesified player. K: Player | V: No. Players Dropped
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static void AddPlayerDropped(int pPlayerID)
    {
        IncrementItemByOne(playersDropped, pPlayerID);
    }

    ////adds a count to the tally of assists that player has gotten while helping to score
    /// <summary>
    /// Add an assist to a player for helping score a goal. K: Player | V: Assists
    /// </summary>
    /// <param name="pPlayerID"></param>
    public static void AddAssist(int pPlayerID)
    {
        IncrementItemByOne(assists, pPlayerID);
    }

    //the percentage of match time the ball was held by the spesified player
    /// <summary>
    /// Update the percentage of match time the ball was held by the give player. The value should only be the delta of the previous % to the current %.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <param name="pPercent"></param>
    public static void UpdatePlayerPossession(int pPlayerID, float pPercent)
    {
        UpdateItem(playerPossession, pPlayerID, pPercent);
    }

    //the percentage of match time the ball was in possession by one team
    /// <summary>
    /// /// Update the percentage of match time the ball was held by one particular team. The value should only be the delta of the previous % to the current %.
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <param name="pPercent"></param>
    public static void UpdateTeamPossession(int pTeamID, float pPercent)
    {
        UpdateItem(playerPossession, pTeamID, pPercent);
    }

    //returns the number of goals that team has
    /// <summary>
    /// Returns the number of times that team has thrown the ball into the enemy's LifeFire. K: Team | V: No. Goals
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <returns></returns>
    public static int GetTeamGoals(int pTeamID)
    {
        return ReturnDictionaryValue(matchGoals, pTeamID);
    }

    //returns the number of goals that team has
    /// <summary>
    /// Returns the number of times that team has thrown the ball into the enemy's LifeFire. K: Team | V: No. Goals
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <returns></returns>
    public static int Get(int pTeamID)
    {
        return ReturnDictionaryValue(matchGoals, pTeamID);
    }

    //returns a vec2 of the goals of both teams
    /// <summary>
    /// Returns a Vector2 with the x element being the LifeFire left for Team 1 and the y that left for Team 2.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetLifeFireValuesLeft()
    {
        //this could be cleaned up instead of hardtyping values
        Vector2 lifefireValues;
        lifefireValues.x = ReturnDictionaryValue(lifeFireLeft, 1);
        lifefireValues.y = ReturnDictionaryValue(lifeFireLeft, 2);
        return lifefireValues;
    }

    //returns a vec2 of the goals of both teams
    /// <summary>
    /// Returns a Vector2 with the x element being the LifeFire taken off the total for Team 1 and the y that taken off Team 2.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetLifeFireValuesRemoved()
    {
        //this could be cleaned up instead of hardtyping values
        Vector2 lifefireValues;
        lifefireValues.x = LobbySettings.GetGoalsToWin() - GetTeamGoals(1);
        lifefireValues.y = LobbySettings.GetGoalsToWin() - GetTeamGoals(2);
        return lifefireValues;
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

    /// <summary>
    /// Returns the number of assists that player has scored.
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static int GetPlayerAssists(int pPlayerID)
    {
        return ReturnDictionaryValue(assists, pPlayerID);
    }

    /// <summary>
    /// Returns the percentage possession that player has
    /// </summary>
    /// <param name="pPlayerID"></param>
    /// <returns></returns>
    public static float GetPlayerPossession(int pPlayerID)
    {
        return ReturnDictionaryValue(playerPossession, pPlayerID);
    }

    /// <summary>
    /// Returns the percentage possession for that team.
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <returns></returns>
    public static float GetTeamPossession(int pTeamID)
    {
        return ReturnDictionaryValue(teamPossession, pTeamID);
    }

    /// <summary>
    /// Returns the amount of 'hitpoints' left on the Lifefire. Should end the game when zero.
    /// </summary>
    /// <param name="pTeamID"></param>
    /// <returns></returns>
    public static float GetLifeFireLeft(int pTeamID)
    {
        return ReturnDictionaryValue(lifeFireLeft, pTeamID);
    }

    /// <summary>
    /// Return an integer with the amount of minutes the match lasts.
    /// </summary>
    /// <returns></returns>
    public static int GetMatchTimeInMinutes()
    {
        return matchTimeInMinutes;
    }

    //internal method for incrementing a value of a spesified statistic
    private static void IncrementItemByOne(Dictionary<int, int> pDictionary, int pKey)
    {
        int numberOfItems;
        pDictionary.TryGetValue(pKey, out numberOfItems);
        pDictionary[pKey] = numberOfItems + 1;
    }

    //internal method for incrementing a value of a spesified statistic
    private static void ReduceItemByOne(Dictionary<int, int> pDictionary, int pKey)
    {
        int numberOfItems;
        pDictionary.TryGetValue(pKey, out numberOfItems);
        pDictionary[pKey] = numberOfItems - 1;
    }

    //update dictionary in the case of floats
    private static void UpdateItem(Dictionary<int, float> pDictionary, int pKey, float pValueDelta)
    {
        float numberOfItems;
        pDictionary.TryGetValue(pKey, out numberOfItems);
        pDictionary[pKey] = numberOfItems + pValueDelta;
    }

    private static int ReturnDictionaryValue(Dictionary<int, int> pDictionary, int pKey)
    {
        int value;
        pDictionary.TryGetValue(pKey, out value);
        return value;
    }

    //overload for floats
    private static float ReturnDictionaryValue(Dictionary<int, float> pDictionary, int pKey)
    {
        float value;
        pDictionary.TryGetValue(pKey, out value);
        return value;
    }
}
