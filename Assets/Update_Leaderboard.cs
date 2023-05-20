using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;



public class Update_Leaderboard : MonoBehaviour
{
    private DatabaseReference _databaseReference;
    [SerializeField] private GameObject rowPreb;
    [SerializeField] private Transform rowParent;

    private void Start()
    {
        // Set up the database reference
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Load the leaderboard data
        LoadLeaderboardData();
    }

    private void LoadLeaderboardData()
    {
        _databaseReference.Child("LeaderBoard").OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve leaderboard data");
                return;
            }

            // Clear existing rows from the UI
          //  ClearRows();

            // Get the leaderboard data from the snapshot
            DataSnapshot snapshot = task.Result;
           // IEnumerable<DataSnapshot> leaderboardData = snapshot.Children;

            // Instantiate rows for each player and set the data
            foreach (DataSnapshot playerData in snapshot.Children)
            {
                string playerId = playerData.Key;
                string playerName = snapshot.Child("name").Value.ToString();
                int playerScore = int.Parse(snapshot.Child("score").Value.ToString());
                string playerProfile = snapshot.Child("profile_pic_name").Value.ToString();
                string playerRank = snapshot.Child("rank").Value.ToString();

                GameObject rowObject = Instantiate(rowPreb, rowParent);
                rowObject.GetComponent<PlayerRow>().SetData(playerId, playerName, playerScore, playerProfile, playerRank);
            }
        });
    }

    private void ClearRows()
    {
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }
    }

    // Callback function for the "Update Score" button
    public void UpdateScore()
    {
        // Get the current player's ID (you will need to implement this yourself)
        string playerId = PlayerPrefs.GetString("currentPlayerID");

        // Increment the player's score by 1
        _databaseReference.Child("LeaderBoard").Child(playerId).Child("score").RunTransaction(mutableData =>
        {
            long score = (long)mutableData.Value;
            mutableData.Value = score + 1;
            return TransactionResult.Success(mutableData);
        });
    }

    // Callback function for the "Refresh" button
    public void RefreshLeaderboard()
    {
        LoadLeaderboardData();
    }
}
