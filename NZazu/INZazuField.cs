﻿using System.Windows.Controls;

namespace NZazu
{
    public interface INZazuField
    {
        string Key { get; }
        string Type { get; }
        string Prompt { get; }
        string Hint { get; }
        string Description { get; }

        string Value { get; set; }
        void Validate();

        // WPF specific
        Control LabelControl { get; }
        Control ValueControl { get; }
    }
}