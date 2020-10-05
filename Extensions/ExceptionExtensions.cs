using System;
using System.IO;
using System.Reactive;
using System.Windows;

namespace LAB12.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorMessage(this EventPattern<object> reactiveEventArgs)
        {
            var exceptionArgs = reactiveEventArgs.EventArgs as ExceptionRoutedEventArgs;

            switch (exceptionArgs.ErrorException)
            {
                case NotSupportedException e:
                    return $"file format not supported:\nMessage:\n${e.Message}";
                case FileFormatException e:
                    return $"Bad file format:\nMessage:\n${e.Message}";
                case FileNotFoundException e:  //Thrown with bad FileFormat, MediaElement bug?
                    return $"File path Error caused: {e.Message}";
                case Exception e:
                    return $"Unexpected Error Occoured Message:\n{e.Message}\nsource:\n{e.Source}";
                default:
                    return "UnIdentified Error Occoured";
            }
        }

    }
}
