namespace UserService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for wallet balance operations.
/// Used after add balance or wallet operations.
/// </summary>
public class WalletBalanceResponse
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the current wallet balance after the operation.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the operation.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets a message describing the operation result.
    /// </summary>
    public string? Message { get; set; }
}
