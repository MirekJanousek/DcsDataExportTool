namespace DcsExportLib.Models;

public interface IElementStepAction
{
    public decimal StepValue
    {
        get;
        set;
    }

    public decimal? MinLimit
    {
        get;
        set;
    }

    public decimal? MaxLimit
    {
        get;
        set;
    }
}