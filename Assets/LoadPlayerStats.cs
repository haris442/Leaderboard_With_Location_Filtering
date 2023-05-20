using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;

public class LoadPlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject rowPreb;
    [SerializeField] private Transform rowParent;
    DatabaseReference reference;
    int playersPerPage = 5;
    int lastPlayerScore = 0;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        LoadPlayers(playersPerPage);
    }

    void LoadPlayers(int limit)
    {
       reference.Child("Leaderboard").OrderByChild("1b4e3d17 - 343a - 427d - aaa8 - 531ceae1a4ac").StartAt(lastPlayerScore).LimitToFirst(limit)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Task Completed!!!");
                    DataSnapshot snapshot = task.Result;

                    int playersLoaded = 0;
                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        Debug.Log("Loop start!!!");

                        // Process the player data and update the UI
                        string playerId = childSnapshot.Key;
                        string playerName = childSnapshot.Child("name").Value.ToString();
                        int playerScore = int.Parse(childSnapshot.Child("score").Child("score").Value.ToString()); // Modified line
                        string playerProfile = childSnapshot.Child("profile_pic_name").Value.ToString();
                        string playerRank = childSnapshot.Child("rank").Value.ToString();
                        // Instantiate the row prefab and set the values
                        GameObject rowObject = Instantiate(rowPreb, rowParent);
                        rowObject.GetComponent<PlayerRow>().SetData(playerId, playerName, playerScore, playerProfile, playerRank);

                        lastPlayerScore = playerScore;
                        playersLoaded++;
                    }
                    if (playersLoaded >= limit)
                    {
                        // If the number of players loaded is less than the limit, it means that we have loaded all the players.
                        // We can hide the load more button.
                        gameObject.SetActive(false);
                    }
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load players: " + task.Exception);
                    // Display an error message to
                }
            });

    }

    public void LoadNextPage()
    {
        LoadPlayers(playersPerPage);
    }
}
