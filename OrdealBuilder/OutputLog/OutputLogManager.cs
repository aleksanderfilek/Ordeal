using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrdealBuilder
{
    public class OutputLogArgs : EventArgs
    {
        public OutputLogType Type { get; private set; }
        public string Message { get; private set; }
        public OutputLogArgs(OutputLogType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public class OutputMultipleLogArgs : EventArgs
    {
        public OutputLogType Type { get; private set; }
        public List<string> Messages { get; private set; }
        public OutputMultipleLogArgs(OutputLogType type, List<string> messages)
        {
            Type = type;
            Messages = messages;
        }
    }

    public struct OutputLogMessage
    {
        public OutputLogType Type;
        public string Message;

        public OutputLogMessage(OutputLogType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public class OutputLogManager
    {
        public event EventHandler<EventArgs>? OnClear;
        public event EventHandler<OutputLogArgs>? OnMsgAdded;
        public event EventHandler<OutputMultipleLogArgs>? OnMultipleMsgAdded;

        private static OutputLogManager Instance;
        public static OutputLogManager Get() { return Instance; }

        private List<OutputLogMessage> messsages = new List<OutputLogMessage>();
        public List<OutputLogMessage> Messages { get => messsages; }

        public OutputLogManager()
        {
            Instance = this;
        }

        ~OutputLogManager()
        {
            Clear();
        }

        public static void Log(OutputLogType Type, string Message)
        {
            if (Instance != null)
            {
                Instance.Add(Type, Message);
            }
        }

        public static void LogMultiple(OutputLogType Type, List<string> Messages)
        {
            if (Instance != null)
            {
                Instance.AddMultiple(Type, Messages);
            }
        }

        private void Add(OutputLogType Type, string Message)
        {
            messsages.Add(new OutputLogMessage(Type, Message));
            Debug.WriteLine(Message);
            OnMsgAdded?.Invoke(this, new OutputLogArgs(Type, Message));
        }

        private void AddMultiple(OutputLogType Type, List<string> Messages)
        {
            foreach(var message in Messages)
            {
                messsages.Add(new OutputLogMessage(Type, message));
                Debug.WriteLine(message);

            }
            OnMultipleMsgAdded?.Invoke(this, new OutputMultipleLogArgs(Type, Messages));
        }

        public void Clear()
        {
            messsages.Clear();
            OnClear?.Invoke(this, new EventArgs());
        }
    }
}
