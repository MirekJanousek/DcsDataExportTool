namespace DcsExportLib.Models
{
    // TODO MJ: Test SD plugin behavior of Min/Max values being excessively long decimal numbers (truncate them? numeric conversion error?
    //          Example is F-18 DCS ID: pnt_236 with Max Value 0.30000000000000004
    public class ElementStepAction : IElementStepAction
    {
        public decimal StepValue
        {
            get;
            set;
        } = decimal.MinValue;

        public decimal MinLimit
        {
            get;
            set;
        } = decimal.MinValue;

        public decimal MaxLimit
        {
            get;
            set;
        } = decimal.MinValue;

    }
}
