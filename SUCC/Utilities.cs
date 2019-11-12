using SUCC.InternalParsingLogic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SUCC
{
    /// <summary>
    /// Provides various helpful functions for working with SUCC files.
    /// </summary>
    public static class Utilities
    {
        private static string _DefaultPath = GetDefaultDefaultPath();

        /// <summary>
        /// The path that DataFile locations will be relative to if you assign them a non-absolute path. By default this is the same folder your game executable is in, or [project folder]/Game in the editor. You can change it if you like.
        /// </summary>
        public static string DefaultPath
        {
            get => _DefaultPath;
            set
            {
                if (!Path.IsPathRooted(value))
                    throw new Exception($"When setting a custom default path, you must set an absolute path. The path {value} is not absolute.");
                _DefaultPath = value;
            }
        }

        private static string GetDefaultDefaultPath()
        {
#if UNITY_EDITOR
            string ProjectFolder = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(ProjectFolder, "Game");
#else
            return Directory.GetParent(Application.dataPath).FullName;
#endif
        }

        /// <summary> All SUCC files have this file extension. </summary>
        public static readonly string FileExtension = ".succ";

        /// <summary> detects whether a file path is relative or absolute, and returns the absolute path </summary>
        public static string AbsolutePath(string relativeOrAbsolutePath)
        {
            if (Path.IsPathRooted(relativeOrAbsolutePath)) return relativeOrAbsolutePath;
            return Path.Combine(DefaultPath, relativeOrAbsolutePath);
        }

        /// <summary> Does a SUCC file exist at the path? </summary>
        public static bool SuccFileExists(string relativeOrAbsolutePath)
        {
            var path = AbsolutePath(relativeOrAbsolutePath);
            path = Path.ChangeExtension(path, FileExtension);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return PlayerPrefs.GetString(path, "") == "";

            return File.Exists(path);
        }

        internal static bool IsValidKey(string potentialKey) => IsValidKey(potentialKey, out _);
        internal static bool IsValidKey(string potentialKey, out string whyNot)
        {
            whyNot = null;

            if (string.IsNullOrEmpty(potentialKey))
                whyNot = "SUCC keys must contain at least one character";
            else if (potentialKey[0] == '-')
                whyNot = "SUCC keys may not begin with the character '-'";
            else if (potentialKey.Contains(':'))
                whyNot = "SUCC keys may not contain the character ':'";
            else if (potentialKey.Contains('#'))
                whyNot = "SUCC keys may not contain the character '#'";
            else if (potentialKey.ContainsNewLine())
                whyNot = "SUCC keys cannot contain a newline";
            else if (potentialKey[0] == ' ' || potentialKey[potentialKey.Length - 1] == ' ')
                whyNot = "SUCC keys may not start or end with a space";

            return whyNot == null;
        }

        /// <summary> Controls how SUCC saves line endings. </summary>
        public static LineEndingStyle LineEndingStyle { get; set; } = LineEndingStyle.PlatformDefault;
        internal static string NewLine
        {
            get
            {
                switch (LineEndingStyle)
                {
                    case LineEndingStyle.Unix:
                        return "\n";
                    case LineEndingStyle.Windows:
                        return "\r\n";
                    case LineEndingStyle.PlatformDefault: default:
                        return Environment.NewLine;
                }
            }
        }
    }

    /// <summary>
    /// Different ways of saving line endings.
    /// </summary>
    public enum LineEndingStyle
    {
        /// <summary> Line endings are Windows style (CR LF) if on Windows and Unix style (LF) everywhere else. </summary>
        PlatformDefault = 0,

        /// <summary> Line endings are Unix style (LF) </summary>
        Unix,

        /// <summary> Line endings are Windows style (CR LF) </summary>
        Windows,
    }
}