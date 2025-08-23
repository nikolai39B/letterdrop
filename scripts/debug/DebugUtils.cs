using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterDrop.Debug
{    
    internal class DebugUtils
    {
        static DebugUtils()
        {
            // Set the log file path and clear it
            string path = "user://debug_log.txt";
            LogFilePath = ProjectSettings.GlobalizePath(path);
            System.IO.File.WriteAllText(LogFilePath, "");
        }

        //-- ASSERTION

        /// <summary>
        /// Asserts that the given condition is true.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        public static void Assert(bool condition)
        {
            AssertTrue(condition);
        }

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

        public static void Log(string message)
        {
            // Print to godot
            GD.Print(message);

            // Print to logfile
            System.IO.File.AppendAllText(LogFilePath, message + "\n");
        }

        public static readonly string LogFilePath;
    }
}
