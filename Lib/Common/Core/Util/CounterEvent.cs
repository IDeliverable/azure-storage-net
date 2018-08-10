//-----------------------------------------------------------------------
// <copyright file="CounterEvent.cs" company="Microsoft">
//    Copyright 2013 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
    using System;
    using System.Threading;
	using System.Threading.Tasks;
	using Nito.AsyncEx;

	public sealed class CounterEvent
    {
        private AsyncManualResetEvent internalEvent = new AsyncManualResetEvent(true);
        private object counterLock = new object();
        private int counter = 0;

        /// <summary>
        /// Gets a WaitHandle that is used to wait for the event to be set.
        /// </summary>
        /// <value>A WaitHandle that is used to wait for the event to be set.</value>
        //public WaitHandle WaitHandle
        //{
        //    get
        //    {
        //        return this.internalEvent;
        //    }
        //}

        /// <summary>
        /// Increments the counter by one and thus sets the state of the event to non-signaled, causing threads to block.
        /// </summary>
        public void Increment()
        {
            lock (this.counterLock)
            {
                this.counter++;
                this.internalEvent.Reset();
            }
        }

        /// <summary>
        /// Decrements the counter by one. If the counter reaches zero, sets the state of the event to signaled, allowing one or more waiting threads to proceed.
        /// </summary>
        public void Decrement()
        {
            lock (this.counterLock)
            {
                if (--this.counter == 0)
                {
                    this.internalEvent.Set();
                }
            }
        }

        /// <summary>
        /// Blocks the current thread until the CounterEvent is set.
        /// </summary>
        public Task WaitAsync(CancellationToken cancellationToken)
        {
			return this.internalEvent.WaitAsync(cancellationToken);
        }

        /// <summary>
        /// Blocks the current thread until the CounterEvent is set, using a 32-bit signed integer to measure the timeout.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or Infinite(-1) to wait indefinitely.</param>
        /// <returns>true if the CounterEvent was set; otherwise, false.</returns>
        //public bool Wait(int millisecondsTimeout)
        //{
        //    return this.internalEvent.WaitOne(millisecondsTimeout);
        //}
    }
}
