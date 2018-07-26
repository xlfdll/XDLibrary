using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Xlfdll.Core
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Use [CallerMemberName] before property name parameter to fill in property name automatically
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression?.Member.Name));
        }

        protected void SetField<T>(ref T field, T value, [CallerMemberName] String propertyName = null)
        {
            field = value;

            this.OnPropertyChanged(propertyName);
        }
    }
}