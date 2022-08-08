namespace DcsExportLib.DcsObjects
{
    public struct LockOnOptions
    {
        public string script_path;

        public LockOnOptions(string modulePath)
        {
            script_path = @$"{modulePath}\Cockpit\Scripts\";
        }
    }
}
