using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
public class Counter : MonoBehaviour
{
    DatabaseReference reference;
    [SerializeField] private Text scoreText; 
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance
            .GetReference("LeaderBoard").Child(PlayerPrefs.GetString("currentPlayerID")).Child("score").ValueChanged += Counter_ValueChanged;
    }

    private void Counter_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        DataSnapshot snapshot = e.Snapshot;

        scoreText.text = snapshot.Value.ToString();
    }


    public void UpdateScore()
    {
        Debug.Log("update score");
        FirebaseDatabase.DefaultInstance
            .GetReference("LeaderBoard").Child(PlayerPrefs.GetString("currentPlayerID")).Child("score").GetValueAsync().ContinueWith(task =>
            {
                if(task.IsFaulted)
                {
                    Debug.LogError(task);
                }
                else if(task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    int value = int.Parse(snapshot.Value.ToString());
                    value++;
                    reference.Child("LeaderBoard").Child(PlayerPrefs.GetString("currentPlayerID")).Child("score").SetValueAsync(value);
                }
            });
    }
}
