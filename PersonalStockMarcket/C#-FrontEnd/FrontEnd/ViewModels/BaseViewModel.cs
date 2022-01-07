using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontEnd.ViewModels
{
    //     Notifies clients that a property value has changed.
    public class BaseViewModel : INotifyPropertyChanged
    {
        //     Occurs when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        //     CallerMemberName -> Allows you to obtain the method or property name of the caller to the method.

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;

            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
