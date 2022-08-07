namespace DcsExportLib.Models
{
    /// <summary>
    /// Clickable element representing cockpit clickable element
    /// </summary>
    public class ClickableElement
    {
        /// <summary>
        /// Gets or sets the collection of parts that the clickable element consist of
        /// </summary>
        public ICollection<ClickableElementPart> ElementParts
        {
            get;
            set;
        } = new List<ClickableElementPart>();

        /// <summary>
        /// Gets or sets the device of the element
        /// </summary>
        public AircraftDevice Device
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="ClickableElement"/>
        /// </summary>
        public string Name
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Gets or sets the hint of the <see cref="ClickableElement"/>
        /// </summary>
        public string Hint
        {
            get;
            set;
        } = string.Empty;
    }
}