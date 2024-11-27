using UserSettings.ServerSpecific;

namespace Exiled.API.Features.Core.UserSettings
{
    public class TwoButtonsSetting : SettingBase
    {
        public TwoButtonsSetting(SSTwoButtonsSetting settingBase)
            : base(settingBase)
        {
            Base = settingBase;
        }

        public TwoButtonsSetting(int id, string label, string firstOption, string secondOption, bool defaultIsSecond = false, string hintDescription = "")
            : this(new SSTwoButtonsSetting(id, label, firstOption, secondOption, defaultIsSecond, hintDescription))
        {
        }

        public new SSTwoButtonsSetting Base { get; }

        public bool IsSecond
        {
            get => Base.SyncIsB;
            set => Base.SyncIsB = value;
        }

        public bool IsFirst
        {
            get => Base.SyncIsA;
            set => Base.SyncIsB = !value;
        }

        public bool IsSecondDefalut
        {
            get => Base.DefaultIsB;
            set => Base.DefaultIsB = value;
        }

        public string FirstOption
        {
            get => Base.OptionA;
            set => Base.OptionA = value;
        }

        public string SecondOption
        {
            get => Base.OptionB;
            set => Base.OptionB = value;
        }
    }
}