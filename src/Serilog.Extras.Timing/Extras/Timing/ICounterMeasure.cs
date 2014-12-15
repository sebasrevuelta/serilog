// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Serilog.Extras.Timing
{
    /// <summary>
    /// Provides a counter which allow increments and decrements. 
    /// </summary>
    public interface ICounterMeasure  : IMeasure
    {
        /// <summary>
        /// Increments the counter.
        /// </summary>
        void Increment();

        /// <summary>
        /// Decrements the counter.
        /// </summary>
        void Decrement();

        /// <summary>
        /// Resets the counter back to zero.
        /// </summary>
        void Reset();

		/// <summary>
		/// Retrieves the current value.
		/// </summary>
		long Value ();
    }

}