﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squiggle.Chat;
using Squiggle.UI.Windows;

namespace Squiggle.UI.MessageFilters
{
    class CommandsFilter: IMessageFilter
    {
        public static CommandsFilter Instance = new CommandsFilter();

        public FilterDirection Direction
        {
            get { return FilterDirection.Out; }
        }

        static Dictionary<string, Action<ChatWindow>> simpleCommands = new Dictionary<string, Action<ChatWindow>>();

        static CommandsFilter()
        {
            simpleCommands["CLS"] = window => window.chatTextBox.Clear();
            simpleCommands["/QUIT"] = window => MainWindow.Instance.Quit();
            simpleCommands["/ONLINE"] = window => MainWindow.Instance.ChatClient.CurrentUser.Status = Core.Presence.UserStatus.Online;
            simpleCommands["/OFFLINE"] = window => MainWindow.Instance.ChatClient.CurrentUser.Status = Core.Presence.UserStatus.Offline;
            simpleCommands["/AWAY"] = window => MainWindow.Instance.ChatClient.CurrentUser.Status = Core.Presence.UserStatus.Away;
            simpleCommands["/BRB"] = window => MainWindow.Instance.ChatClient.CurrentUser.Status = Core.Presence.UserStatus.BeRightBack;
            simpleCommands["/BUSY"] = window => MainWindow.Instance.ChatClient.CurrentUser.Status = Core.Presence.UserStatus.Busy;
            simpleCommands["/BUZZ"] = window => window.SendBuzz();
            simpleCommands["/MAIN"] = window => MainWindow.Instance.RestoreWindow();
        }

        public bool Filter(StringBuilder message, ChatWindow window)
        {
            string command = message.ToString().Trim().ToUpperInvariant();

            Action<ChatWindow> action;
            if (simpleCommands.TryGetValue(command, out action))
            {
                action(window);
                return false;
            }

            return true;
        }
    }
}
