namespace SquirrelStash.Models
{
    public class OverviewItem
    {
        public string Category { get; set; }

        public int Quantity { get; set; }

        public bool IsCritical { get; set; }

        public string[]? PropertiesValues { get; set; }

        public string Name
        {
            get
            {
                if (PropertiesValues == null)
                {
                    return "N/A";
                }
                else
                {
                    return string.Join(" ",
                        PropertiesValues
                            .Where(v => !string.IsNullOrWhiteSpace(v)));
                }
            }
        }
    }
}
