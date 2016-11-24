namespace Genki
{
    public enum Importance
    {
        Low,
        Normal,
        High
    }

    public static class ImportanceExtensionMethods
    {
        public static string GetAsString(this Importance importance)
        {
            switch(importance)
            {
                case(Importance.High):
                    return "high";
                case(Importance.Low):
                    return "low";
                default:
                    return "normal";
            }
        }
    }

}