using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace OrdealBuilder.ViewModels
{
    public class OutputLogViewModel : ViewModelBase
    {
        private ObservableCollection<TextBlock> messageObjects = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> MessageObjects
        {
            get
            {
                return messageObjects;
            }
            set
            {
                messageObjects = value;
                OnPropertyChanged("MessageObjects");
            }
        }

        public OutputLogViewModel() 
        {
            OutputLogManager outputLogManager = OutputLogManager.Get();
            outputLogManager.OnClear += OutputLogManager_OnClear;
            outputLogManager.OnMsgAdded += OutputLogManager_OnMsgAdded;
            outputLogManager.OnMultipleMsgAdded += OutputLogManager_OnMultipleMsgAdded;
            foreach (var message in outputLogManager.Messages)
            {
                AddMessage(message.Type, message.Message);
            }
        }

        private void OutputLogManager_OnMultipleMsgAdded(object? sender, OutputMultipleLogArgs e)
        {
            foreach (var message in e.Messages)
            {
                AddMessage(e.Type, message);
            }
        }

        private void OutputLogManager_OnMsgAdded(object? sender, OutputLogArgs e)
        {
            AddMessage(e.Type, e.Message);
        }

        private void OutputLogManager_OnClear(object? sender, EventArgs e)
        {
            Clear();
        }


        public void Clear()
        {
            MessageObjects.Clear();
        }

        private readonly Dictionary<OutputLogType, Color> typeToColorMap = new Dictionary<OutputLogType, Color>()
        {
            { OutputLogType.Normal, Colors.Black },
            { OutputLogType.Warning, Colors.Orange },
            { OutputLogType.Error, Colors.Red }
        };

        private void AddMessage(OutputLogType Type, string message)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate ()
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = message;
                            textBlock.Foreground = new SolidColorBrush(typeToColorMap[Type]);
                            MessageObjects.Add(textBlock);
                        }
                        );
        }
    }
}
