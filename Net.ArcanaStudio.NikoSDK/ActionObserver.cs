using System;

namespace Net.ArcanaStudio.NikoSDK
{
    internal class ActionObserver<T> : IObserver<T>
    {
        private readonly Action<T> _onNextAction;
        private readonly Action<Exception> _onErrorAction;

        public ActionObserver(Action<T> onNextAction, Action<Exception> onErrorAction = null)
        {
            _onNextAction = onNextAction ?? throw new ArgumentNullException(nameof(onNextAction));
            _onErrorAction = onErrorAction;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            _onErrorAction?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _onNextAction(value);
        }
    }
}
