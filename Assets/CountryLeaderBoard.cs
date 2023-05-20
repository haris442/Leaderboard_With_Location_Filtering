using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CountryLeaderBoard : MonoBehaviour
{
    private string userCountry = "";
    //  DatabaseReference databaseReference;
    DatabaseReference countryRef;
    [SerializeField] private GameObject rowPreb;
    [SerializeField] private Transform rowParent;
    private List<PlayerData> players = new List<PlayerData>(); // List to store player data

    private void Awake()
    {
        // FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);

    }
    void Start()
    {
        GetUserLocationData.instance.GetUserCountryRegionData(GetUserCountryRegion);

        // Application.targetFrameRate = 60;
        // Set up the database reference
        //databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Set up the listener for changes to the player data
        //databaseReference.Child("LeaderBoard").ChildAdded += HandleChildAdded;


    }
    private void GetUserCountryRegion(string country, string region)
    {
        if (country != null && region != null)
        {
            Debug.Log("User Country is " + country + " And Region is " + region);
            userCountry = country;
           
            countryRef = FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("Country").Child(userCountry);
            countryRef.ChildAdded += HandleChildAdded;

        }
        else
        {
            Debug.Log("Failed to get the user Country and Region");
        }
    }
    // Handle new child added to the database
    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        // Add the new player data to the list
        DataSnapshot snapshot = args.Snapshot;
        string playerId = snapshot.Key;
        Debug.Log("this is the player id " + playerId);
        string playerName = snapshot.Child("name").Value.ToString();
        int playerScore = int.Parse(snapshot.Child("score").Value.ToString());
        string playerProfile = snapshot.Child("profile_pic_name").Value.ToString();
        string playerRank = "";

        players.Add(new PlayerData(playerId, playerName, playerScore, playerProfile, playerRank));

        // Assign a unique rank to each player based on their score
        int rank = 0;
        int lastScore = int.MaxValue;
        players.Sort((p1, p2) => p2.score.CompareTo(p1.score));

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].score <= lastScore) // if the player's score is lower than the previous score
            {
                rank++; // increment the rank
            }
            players[i].rank = rank.ToString(); // assign the rank to the player
                                               //  databaseReference.Child("LeaderBoard").Child(players[i].playerId).Child("rank").SetValueAsync(players[i].rank);
            countryRef.Child(players[i].playerId).Child("rank").SetValueAsync(players[i].rank);
            lastScore = players[i].score; // update the previous score
        }

        // Update the UI
        // Clear the existing rows
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the row prefabs and set the data
        for (int i = 0; i < players.Count; i++)
        {
            GameObject rowObject = Instantiate(rowPreb, rowParent);
            rowObject.GetComponent<PlayerRow>().SetData(players[i].playerId, players[i].playerName, players[i].score, players[i].playerProfile, players[i].rank);
        }
    }


    public class PlayerData
    {
        public string playerId;
        public string playerName;
        public int score;
        public string playerProfile;
        public string rank;

        public PlayerData(string playerId, string playerName, int score, string playerProfile, string rank)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.score = score;
            this.playerProfile = playerProfile;
            this.rank = rank;
        }
    }
}

