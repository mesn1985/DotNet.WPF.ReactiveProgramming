using System;
using System.Windows;
using System.Windows.Forms;
using System.Reactive.Linq;
using LAB12.Extensions;

namespace Assignment6
{
    /// <summary>
    /// RX.Net is used for event logic
    /// A better solution could be working directly on MediaPlayer
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            ObserverInteractions();

        }
        private void ObserverInteractions()
        {
            Observable.FromEventPattern(ButtonOpen, "Click")
                      .Subscribe(_ =>
                                 {
                                     using (var fileDialog = FileDialog())
                                     {
                                         MediaElement1.Source = fileDialog.Open().AsUri();
                                     }
                                 }
                      );
                    
            Observable.FromEventPattern(MediaElement1, "MediaOpened")
                      .Subscribe(_ => ProgressBar1.Maximum = MediaElement1.NaturalDuration.TimeSpan.TotalSeconds);

            Observable.FromEventPattern(ButtonPlay, "Click")
              .Subscribe(e => {
                             if (MediaElement1.IsNotPlayingAndHaveSource())
                                 ObservePlayStream();
                         });
        }
        private void ObservePlayStream()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(10))
                      .TakeUntil(Observable.FromEventPattern(ButtonStop, "Click")
                                           .Do(_ => MediaElement1.Stop())
                      )
                      .TakeUntil(Observable.FromEventPattern(ButtonPause, "Click")
                                           .Do(_ => MediaElement1.PauseIfPossible())
                      )
                      .TakeUntil(Observable.FromEventPattern(MediaElement1, "MediaEnded")
                                           .Do(_ => MediaElement1.Stop())
                      )
                      .TakeUntil(Observable.FromEventPattern(MediaElement1, "MediaFailed")
                                           .Do(eventArgs =>
                                               {
                                                   MediaElement1.MediaLoadFailed();
                                                   System.Windows.MessageBox.Show(eventArgs.GetErrorMessage());
                                               }))
                      .Subscribe(_ => 
                                     MediaElement1.Dispatch(
                                         () => ProgressBar1.Value = MediaElement1.Position.TotalSeconds));
            MediaElement1.Play();
        }
        private OpenFileDialog FileDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = $@"{Environment.CurrentDirectory}\MediaFiles\";
            return dialog;
        }
        
      

    }
}
