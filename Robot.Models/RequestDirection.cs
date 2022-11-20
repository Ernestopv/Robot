namespace Robot.Models;

/// <summary>
/// Set direction on Motors
/// 
/// </summary>
public class RequestDirection
{
    /// <summary>
    /// Gets or sets angle
    /// </summary>
    public string Angle { get; set; } = null!;

    /// <summary>
    /// Gets or sets direction
    /// </summary>
    public string Direction { get; set; } = null!;
}