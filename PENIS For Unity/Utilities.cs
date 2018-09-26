﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PENIS
{
    public static class Utilities
    {
        public static string DefaultPath { get; set; } = Application.persistentDataPath;
        public static readonly string FileExtension = ".PENIS";

        static int _indentationCount = 4;
        public static int IndentationCount
        {
            get { return _indentationCount; }
            set
            {
                if (value < 1)
                    _indentationCount = 1;
                else
                    _indentationCount = value;
            }
        }

        /// <summary> detects whether a file path is relative or absolute, and returns the absolute path </summary>
        public static string AbsolutePath(string RelativeOrAbsolutePath)
        {
            if (Path.IsPathRooted(RelativeOrAbsolutePath)) { return RelativeOrAbsolutePath; }
            return Path.Combine(DefaultPath, RelativeOrAbsolutePath);
        }
    }
}