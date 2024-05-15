﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Platform.Keyboard;
using OpenTabletDriver.Plugin.Tablet;

namespace OpenTabletDriver.Desktop.Binding
{
    [PluginName("DualActionBinds")]
    public class DualActionBinds : IStateBinding
    {
        private const char KEYS_SPLITTER = '+';
        private const char GROUP_SPLITTER = ':';

        private IList<string> keys_press = new List<string>();
        private IList<string> keys_release = new List<string>();
        private string keysString = null!;

        [Resolved]
        public IVirtualKeyboard Keyboard { set; get; } = null!;

        [Property("Keys")]
        public string Keys
        {
            set
            {
                keysString = value;
                (keys_press, keys_release) = ParseKeys(Keys);
            }
            get => keysString;
        }

        public void Press(TabletReference tablet, IDeviceReport report)
        {
            if (keys_press.Count > 0)
                Keyboard.Press(keys_press);
            Keyboard.Release(keys_press);
        }

        public void Release(TabletReference tablet, IDeviceReport report)
        {
            if (keys_release.Count > 0)
                Keyboard.Press(keys_release);
            Keyboard.Release(keys_release);
        }

        private (IList<string>, IList<string>) ParseKeys(string str)
        {

            var keysList1 = new List<string>();
            var keysList2 = new List<string>();

            var groups = str.Split(":", StringSplitOptions.TrimEntries);


            // The plugin cannot be used correctly without exactly 2 groups.
            if (groups.Length != 2)
            {
                Log.WriteNotify("DualActionBinds", $"You must have 2 groups of keys!\nGroups are split by '{GROUP_SPLITTER}'.");
                return (new List<string>(), new List<string>());
            }


            for (int i = 0; i < groups.Length; i++)
            {
                var newKeys = groups[i].Split(KEYS_SPLITTER, StringSplitOptions.TrimEntries);
                if (newKeys.Length > 0 && newKeys.All(k => Keyboard.SupportedKeys.Contains(k)))
                {
                    if (i == 0)
                    {
                        keysList1.AddRange(newKeys);
                    }
                    else
                    {
                        keysList2.AddRange(newKeys);
                    }
                }
            }

            return (keysList1, keysList2);
        }

        public override string ToString() => "DualActionBinds: {Keys}";
    }
}
