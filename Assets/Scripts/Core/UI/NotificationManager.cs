using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    /// <summary>
    /// Represents a notification sent to the player.
    /// </summary>
    struct NotificationItem
    {
        public string message;
        public string addData;
        public double timeSentMS;
        public float durationMS;

        /// <summary>
        /// The exact time when the notification will expire (in MS)
        /// </summary>
        public double expireTimeMS => timeSentMS + durationMS;
    }

    /// <summary>
    /// This class is responsible for managing text notifications (shown in the ui) sent to the player.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        // reference to The text mesh pro component
        private TextMeshProUGUI text;

        // List of notifications to display
        private List<NotificationItem> notifications = new();

        // Whether or not the notification list has been updated
        private bool hasNotificationUpdate = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of NotificationManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }

            Instance = this;
        }

        private void Start()
        {
            // Get the text mesh pro component and clear any text in it.
            text = GetComponent<TextMeshProUGUI>();
            text.text = "";
        }

        /// <summary>
        /// Pushes a notification to the player. For example "Not Enough Mana!"
        /// </summary>
        /// <param name="text">The notification text</param>
        /// <param name="durationMS">duration of the notification in milliseconds.</param>
        /// <param name="addData">Additional text data to append to the end of the message.
        /// By using this instead of putting it in the message parameter, it allows similar messages (with different data) to stack (Eg: "Not Enough Mana (10 more) x 3")
        /// </param>
        public void PushNotification(string text, float durationMS = 3000f, string addData = "")
        {
            // Add the notification to the list
            notifications.Add(new NotificationItem {
                    message = text, // The main message
                    addData = addData, // Additional data to display
                    timeSentMS = Time.timeAsDouble * 1000, // The time the notification was sent in MS
                    durationMS = durationMS // The duration of the notification in MS
            });
            hasNotificationUpdate = true; // Mark that the notification list has been updated
        }


        /// <summary>
        /// Removes expired notifications
        /// </summary>
        void RemoveExpiredNotifications()
        {
            int before = notifications.Count; // Get the count before removing
            // Remove all notifications that have expired (expireTimeMS < current time)
            notifications.RemoveAll(x => x.expireTimeMS < Time.timeAsDouble * 1000);
            int after = notifications.Count; // Get the count after removing
            if (before != after)
            {
                hasNotificationUpdate = true;
            }

            ;
        }

        void UpdateText()
        {
            // Group similar messages together so we can avoid printing them out multiple times
            var stackedMessages = notifications.GroupBy(
                key => key.message, // The message is the key to group by 
                value => value, // The data is the NotificationItem
                (msg, item) => new {
                        Text = msg,
                        // Only use the most recent the add data in the group (which should have the biggest expireTimeMS) 
                        AddText = item.Aggregate((biggest, next) => next.expireTimeMS > biggest.expireTimeMS ? next : biggest).addData,
                        Count = item.Count()
                }
            ).Select(x => $"{x.Text} {x.AddText}" +
                          (x.Count > 1 ? $" x {x.Count}" : "") // If there is more than one message grouped together, add a count to the end of the message
            );

            // Combine all messages into a single one
            var combinedText = stackedMessages.JoinString("\n");
            text.text = combinedText; // Set the text
        }

        private void Update()
        {
            // Check for and remove expired notifications
            RemoveExpiredNotifications();
            // If the notification list has been updated, update the text
            if (hasNotificationUpdate)
            {
                UpdateText();
            }
        }
    }
}