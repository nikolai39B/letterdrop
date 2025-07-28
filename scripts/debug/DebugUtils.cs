using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterDrop.Debug
{    
    internal class DebugUtils
    {
        //-- ASSERTION

        /// <summary>
        /// Asserts that the given condition is true.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <returns>The condition</returns>
        public static bool AssertTrue(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
            return condition;
        }

        /// <summary>
        /// Asserts that the given condition is false.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <returns>The condition</returns>
        public static bool AssertFalse(bool condition)
        {
            System.Diagnostics.Debug.Assert(!condition);
            return condition;
        }
    }
}
