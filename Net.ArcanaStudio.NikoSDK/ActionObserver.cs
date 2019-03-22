using System;

namespace Net.ArcanaStudio.NikoSDK
{
    internal class ActionObserver<T> : IObserver<T>
    {
        private readonly Action<T> _onNextAction;
        private readonly Action<Exception> _onErrorAction;

        public ActionObserver(Action<T> on_next_action, Action<Exception> on_error_action)
        {
            _onNextAction = on_next_action ?? throw new ArgumentNullException(nameof(on_next_action));
            _onErrorAction = on_error_action ?? throw new ArgumentNullException(nameof(on_error_action));
        }

        public void OnCompleted()
        {
       
        }

        public void OnError(Exception error)
        {
            _onErrorAction(error);
        }

        public void OnNext(T value)
        {
            _onNextAction(value);
        }
    }
}
