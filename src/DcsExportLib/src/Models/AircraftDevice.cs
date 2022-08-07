namespace DcsExportLib.Models
{
    /// <summary>
    /// The aircraft device to which the elements belong to
    /// </summary>
    public class AircraftDevice
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="AircraftDevice"/>
        /// </summary>
        public long DeviceId { get; set; }


        /// <summary>
        /// Gets or sets the name of the <see cref="AircraftDevice"/>
        /// </summary>
        public string DeviceName { get; set; } = string.Empty;
    }
}
