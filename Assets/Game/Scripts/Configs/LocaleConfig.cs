using System;

namespace Assets.Game.Scripts.Configs {
    [Serializable]
    public class LocaleConfig {
        public string ID;
        public string Text;

        public object Clone() {
            return new LocaleConfig {
                Text = Text,
                ID = ID,
            };
        }
    }
}

public enum Locale {
    None,
    RU,
    EN
}