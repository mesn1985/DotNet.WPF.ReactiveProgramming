using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LAB12.Extensions
{
    public static class MediaElementExtensions
    {
        public static void MediaLoadFailed(this MediaElement mediaElement)
        {
            mediaElement.Stop();
            mediaElement.Source = null;
        }

        public static void Dispatch(this MediaElement mediaElement,Action action)
        {
            mediaElement.Dispatcher.Invoke(action);
        }

        public static void PauseIfPossible(this MediaElement mediaElement)
        {
            if(mediaElement.CanPause)
                mediaElement.Pause();
        }

        public static Boolean IsNotPlayingAndHaveSource(this MediaElement mediaElement)
        {

            return IsNotPlaying(mediaElement) && haveSource(mediaElement);
        }

        private static Boolean haveSource(MediaElement mediaElement)
            => !string.IsNullOrEmpty(mediaElement.Source?.AbsolutePath ?? null);
        
        private static Boolean IsNotPlaying(MediaElement mediaElement)
        {
            FieldInfo helperField = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            // AVElementHelper type
            object helperFieldValueObject = helperField.GetValue(mediaElement);
            FieldInfo stateField = helperFieldValueObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            return (MediaState)stateField.GetValue(helperFieldValueObject) != MediaState.Play;
        }
    }
}
