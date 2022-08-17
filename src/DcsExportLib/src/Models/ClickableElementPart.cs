namespace DcsExportLib.Models
{
    public class ClickableElementPart
    {
        public ElementPartType Type
        {
            get;
            set;
        } = ElementPartType.Null;

        public long? DcsId
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Gets or sets the Action ID of the <see cref="ClickableElementPart"/>
        /// </summary>
        public long? ActionId
        {
            get;
            set;
        } = null;


        public IElementStepAction ElementAction
        {
            get;
            set;
        } = new NoElementStepAction();
    }
}
