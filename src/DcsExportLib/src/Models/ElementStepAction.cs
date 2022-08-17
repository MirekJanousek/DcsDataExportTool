namespace DcsExportLib.Models
{
    public class ElementStepAction : IElementStepAction
    {
        public decimal StepValue
        {
            get;
            set;
        } = 0;

        public decimal? MinLimit
        {
            get;
            set;
        } = null;

        public decimal? MaxLimit
        {
            get;
            set;
        } = null;

    }
}
