using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    struct NotificationItem
    {
        public string message;
        public string addData;
        public double timeSentMS;
        public float durationMS;
    }

    /// <summary>
    /// This class is responsible for managing  text notifications (shown in the ui) sent to the player.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }
        private TextMeshProUGUI text;
        private List<NotificationItem> notifications = new();
        private bool hasNotificationUpdate = false;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of NotificationManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }

            Instance = this;
        }

        /// <summary>
        /// Pushes a notification to the player. For example "Not Enough Mana!"
        /// </summary>
        /// <param name="text">The notification text</param>
        /// <param name="durationMS">duration of the notification in milliseconds.</param>
        /// <param name="addData">Additional text data to append to the end of the message.
        /// By using this instead of putting it in the message parameter, it allows similar messages (with different data) to stack (Eg: "Not Enough Mana (10 more) x 3")
        /// </param>
        public void PushNotification(string text, float durationMS = 500f, string addData = "")
        {
            notifications.Add(new NotificationItem {
                    message = text,
                    addData = addData,
                    timeSentMS = Time.timeAsDouble * 1000,
                    durationMS = durationMS
            });
            hasNotificationUpdate = true;
        }

        /// <summary>
        /// Removes expired notifications
        /// </summary>
        void RemoveExpiredNotifications()
        {
            int before = notifications.Count();
            notifications.RemoveAll(x => (x.timeSentMS + x.durationMS) < Time.timeAsDouble);
            int after = notifications.Count();
            if (before != after)
            {
                hasNotificationUpdate = true;
            };
        }

        void UpdateText()
        {
            // Group similar messages together so we can avoid printing them out multiple times
            var stackedMessages = notifications.GroupBy(
                a => a.message,
                b => b.addData,
                (msg, addDatas) => $"{msg} {addDatas.First()} x {addDatas.Count()}"
            );

            // Combine all messages into a single one
            var combinedText = stackedMessages.JoinString("\n");
            text.text = combinedText;
        }

        private void Update()
        {
            RemoveExpiredNotifications();
            if (hasNotificationUpdate)
            {
                UpdateText();
            }
        }
    }
}