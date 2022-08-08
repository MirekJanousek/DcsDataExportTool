namespace DcsExportLib.DcsObjects
{
    public struct LockOnOptions
    {
        public string script_path;

        public LockOnOptions(string moduleScriptPath)
        {
            script_path = moduleScriptPath + @"\";
        }
    }
}
