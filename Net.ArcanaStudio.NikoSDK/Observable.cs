using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.ArcanaStudio.NikoSDK
{
    internal class Observable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers;

        public Observable()
        {
            _observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber<T>(_observers, observer);
        }

        public void Add(T element)
        {
            foreach (var observer in _observers.ToList())
            {
                observer.OnNext(element);
            }
        }

        public void End()
        {
            foreach (var observer in _observers.ToArray())
                if (_observers.Contains(observer))
                    observer.OnCompleted();

            _observers.Clear();
        }

#pragma warning disable 693
        private class Unsubscriber<T> : IDisposable
#pragma warning restore 693
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
